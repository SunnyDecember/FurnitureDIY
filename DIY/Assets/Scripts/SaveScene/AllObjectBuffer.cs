using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/*
** Author      : Runing
** Time        : 12/1/2018 8:37:51 PM
** description : 
*/

public class AllObjectBuffer
{
    /// <summary>
    /// The key is model name
    /// The value is child node
    /// </summary>
    public Dictionary<string, List<OneNodeTransform>> transformBufferDictionary = new Dictionary<string, List<OneNodeTransform>>();

    public struct OneNodeTransform
    {
        public string name;
        public string position;
        public string quaterion;
        public string localScale;
        public bool isShow;
        public string textureName;
    }

    /// <summary>
    /// String is converted to Vector3
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    private Vector3 ToVector3(string str)
    {
        String[] strArray = str.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

        if (strArray.Length != 3)
            Debug.LogError("AllObjectBuffer.ToVector3(): strArray count is not 3");

        return new Vector3(Convert.ToSingle(strArray[0]), Convert.ToSingle(strArray[1]), Convert.ToSingle(strArray[2]));
    }

    /// <summary>
    /// Vector3 is converted to string
    /// </summary>
    /// <param name="vec"></param>
    /// <returns></returns>
    private string FromVector3(Vector3 vec)
    {
        return vec[0] + "|" + vec[1] + "|" + vec[2];
    }

    /// <summary>
    /// String is converted to Quaternion
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    private Quaternion ToQuaterion(string str)
    {
        String[] strArray = str.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

        if (strArray.Length != 4)
            Debug.LogError("AllObjectBuffer.ToQuaterion(): strArray count is not 4");

        return new Quaternion(Convert.ToSingle(strArray[0]), Convert.ToSingle(strArray[1]), Convert.ToSingle(strArray[2]), Convert.ToSingle(strArray[3]));
    }

    /// <summary>
    /// Quaternion is converted to string
    /// </summary>
    /// <param name="quaterion"></param>
    /// <returns></returns>
    private string FromQuaterion(Quaternion quaterion)
    {
        return quaterion.x + "|" + quaterion.y + "|" + quaterion.z + "|" + quaterion.w;
    }

    /// <summary>
    /// Objects that need to be record
    /// </summary>
    public void Record(List<Transform> objectList)
    {
        List<string> nodeNameList = new List<string>();
        transformBufferDictionary.Clear();

        for (int i = 0; i < objectList.Count; i++)
        {
            Transform model = objectList[i];
            nodeNameList.Clear();

            //Get all children in model
            Transform[] childTran = model.GetComponentsInChildren<Transform>(true);

            if (!transformBufferDictionary.ContainsKey(model.name))
            {
                List<OneNodeTransform> list = new List<OneNodeTransform>();
                transformBufferDictionary.Add(model.name, list);
            }

            //Circulating all chlidren
            for (int j = 0; j < childTran.Length; j++)
            {
                Transform child = childTran[j];

                OneNodeTransform oneNodeTransform = new OneNodeTransform();
                oneNodeTransform.name = child.name;
                oneNodeTransform.position = FromVector3(child.position);
                oneNodeTransform.quaterion = FromQuaterion(child.rotation);
                oneNodeTransform.localScale = FromVector3(child.localScale);
                oneNodeTransform.isShow = child.gameObject.activeSelf;

                //获取纹理名字并保存到缓存中。
                MeshRenderer mesh = child.GetComponent<MeshRenderer>();
                string textureName = "";

                if (null != mesh && null != mesh.material && null != mesh.material.mainTexture)
                {
                    textureName = mesh.material.mainTexture.name;
                }

                oneNodeTransform.textureName = textureName;
                transformBufferDictionary[model.name].Add(oneNodeTransform);

                //Check if the node name is repeated.
                if (!nodeNameList.Contains(child.name))
                {
                    nodeNameList.Add(child.name);
                }
                else
                {
                    Debug.LogError(string.Format("AllObjectBuffer.Record(): The node:{0} of the model:{1} is repeated !!!", child.name, model.name));
                }
            }
        }
    }

    /// <summary>
    /// Recover those objects that have been recorded
    /// </summary>
    public List<Transform> Recover(Transform modelRoot)
    {
        List<Transform> modelList = new List<Transform>();

        //遍历缓存中所有模型
        foreach (var kv in transformBufferDictionary)
        {
            //加载最新模型到场景中，并获取他的所有子节点。
            Transform model = LoadModel(kv.Key, modelRoot);
            modelList.Add(model);
            Dictionary<string, Transform> childrenDictionary = GetModelChildren(model);

            //circulating buffer child.
            //遍历一个缓存模型的所有子节点
            List<OneNodeTransform> bufferChildList = kv.Value;
            for (int i = 0; i < bufferChildList.Count; i++)
            {
                //用缓存模型的节点设置给最新模型节点,达到恢复场景的目的。
                OneNodeTransform bufferChild = bufferChildList[i];
                RecoverOneNode(childrenDictionary, bufferChild);
            }
        }

        return modelList;
    }

    /// <summary>
    /// 从本地加载模型
    /// </summary>
    /// <param name="bufferModelNameEX">EX 为带后缀名</param>
    /// <param name="parent"></param>
    /// <returns></returns>
    private Transform LoadModel(string bufferModelNameEX, Transform parent)
    {
        string bufferModelName = bufferModelNameEX;

        //todo 待改进, 用正则表达式
        for (int i = 0; i < 20; i++)
        {
            string ex = "_" + i;
            if (bufferModelName.Contains(ex))
                bufferModelName = bufferModelName.Replace(ex, "");
        }

        //load model
        Transform model = ResourceManager.Instance.LoadModel(parent, bufferModelName);
        model.name = bufferModelNameEX;
        return model;
    }

    /// <summary>
    /// 获取一个模型下所有的节点
    /// </summary>
    /// <param name="model"></param>
    /// <returns>key为节点名字，value为节点Transform</returns>
    private Dictionary<string, Transform> GetModelChildren(Transform model)
    {
        Transform[] childArray = model.gameObject.GetComponentsInChildren<Transform>(true);
        Dictionary<string, Transform> childDictionary = new Dictionary<string, Transform>();

        //Array is converted to Dictionary.
        for (int i = 0; i < childArray.Length; i++)
        {
            Transform child = childArray[i];
            childDictionary.Add(child.name, child);
        }

        return childDictionary;
    }

    /// <summary>
    /// 恢复一个节点
    /// </summary>
    /// <param name="childrenDictionary">当前一个模型所有的节点</param>
    /// <param name="bufferChild">缓存模型中的一个节点</param>
    private void RecoverOneNode(Dictionary<string, Transform> childrenDictionary, OneNodeTransform bufferChild)
    {
        if (childrenDictionary.ContainsKey(bufferChild.name))
        {
            Transform child = childrenDictionary[bufferChild.name];
            child.name = bufferChild.name;
            child.position = ToVector3(bufferChild.position);
            child.rotation = ToQuaterion(bufferChild.quaterion);
            child.localScale = ToVector3(bufferChild.localScale);
            child.gameObject.SetActive(bufferChild.isShow);

            //加载纹理
            MeshRenderer mesh = child.GetComponent<MeshRenderer>();
            if (!string.IsNullOrEmpty(bufferChild.textureName) && null != mesh)
            {
                Texture texture = ResourceManager.Instance.LoadTexture(bufferChild.textureName);

                //如果在Resources下找不到纹理，就不要管了。
                //很可能这个纹理没有被替换过的，那么就不要去更新它，保持默认的纹理就行。
                if (null != texture)
                {
                    mesh.material.mainTexture = texture;
                }
            }
        }
        else
        {
            //当前模型节点不在缓存模型节点中，说明当前模型有东西没有加载进来。
            //加载模型
            //递归
            //Transform model = LoadModel(kv.Key, modelRoot);
            //modelList.Add(model);
            //Dictionary<string, Transform> childrenDictionary = GetModelChildren(model);
        }
    }
}
