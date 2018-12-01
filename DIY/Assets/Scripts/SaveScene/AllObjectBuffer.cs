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
    }

    /// <summary>
    /// String is converted to Vector3
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    private Vector3 ToVector3(string str)
    {
        String[] strArray = str.Split(new char[] {'|'}, StringSplitOptions.RemoveEmptyEntries);

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

        if(strArray.Length != 4)
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

        foreach (var kv in transformBufferDictionary)
        {
            string bufferModelNameEX = kv.Key;
            string bufferModelName = bufferModelNameEX;

            //todo 待改进, 用正则表达式
            for (int i = 0; i < 20; i++)
            {
                string ex = "_" + i;
                if (bufferModelName.Contains(ex))
                    bufferModelName = bufferModelName.Replace(ex, "");
            }

            //load model
            Transform model = ResourceManager.Instance.LoadModel(modelRoot, bufferModelName);
            modelList.Add(model);
            model.name = bufferModelNameEX;
            Transform[] childArray = model.gameObject.GetComponentsInChildren<Transform>(true);
            Dictionary<string, Transform> childDictionary = new Dictionary<string, Transform>();

            //Array is converted to Dictionary.
            for (int i = 0; i < childArray.Length; i++)
            {
                Transform child = childArray[i];
                childDictionary.Add(child.name, child);
            }

            //circulating buffer child.
            List<OneNodeTransform> bufferChildList = kv.Value;
            for (int i = 0; i < bufferChildList.Count; i++)
            {
                OneNodeTransform bufferChild = bufferChildList[i];

                if (childDictionary.ContainsKey(bufferChild.name))
                {
                    Transform child = childDictionary[bufferChild.name];
                    child.name = bufferChild.name;
                    child.position = ToVector3(bufferChild.position);
                    child.rotation = ToQuaterion(bufferChild.quaterion);
                    child.localScale = ToVector3(bufferChild.localScale);
                    child.gameObject.SetActive(bufferChild.isShow);
                }
                else
                {
                    //当前模型节点不在缓存模型节点中，说明当前模型有东西没有加载进来。
                    //加载模型
                    //递归
                }
            }
        }

        return modelList;
    }
}
