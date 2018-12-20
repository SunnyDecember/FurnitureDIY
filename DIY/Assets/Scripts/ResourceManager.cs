using UnityEngine;
using System.Collections;

/* Author:       Running
** Time:         18.11.18
** Describtion:  
*/

public class ResourceManager
{
    private static ResourceManager _instance = null;

    public static ResourceManager Instance
    {
        get
        {
            return _instance ?? (_instance = new ResourceManager());
        }
    }

    /// <summary>
    /// 加载预制体
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="prefabName"></param>
    /// <returns></returns>
    public GameObject LoadUIPrefab(Transform parent, string prefabName)
    {
        GameObject obj = GameObject.Instantiate(Resources.Load("UIPrefabs/" + prefabName)) as GameObject;
        obj.transform.parent = parent;
        (obj.transform as RectTransform).anchoredPosition = Vector2.zero;
        return obj;
    }

    /// <summary>
    /// 加载模型
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="modelName"></param>
    /// <returns></returns>
    public Transform LoadModel(Transform parent, string modelName)
    {
        Object originObj = Resources.Load("Model/" + modelName);
        if (null == originObj)
        {
            Debug.LogError("ResourceManager.LoadModel(): " + modelName + " is null");
            return null;
        } 

        GameObject obj = GameObject.Instantiate(originObj) as GameObject;
        RayEvent.Instance.tempInstanceObj = obj;
        //Attach script to model node
        ModelCategory modelCategory = ModelCategory.AttachToModel(obj.transform);

        if (null != modelCategory)
        {
            //Add tool node for model node
            GameObject tool = new GameObject("Tools");
            tool.transform.SetParent(obj.transform);
            tool.transform.localPosition = Vector3.zero;
            tool.transform.localScale = Vector3.one;
            tool.transform.localEulerAngles = Vector3.zero;

            modelCategory.toolNode = tool.transform;
        }

        //set parent
        obj.transform.parent = parent;
        obj.transform.localPosition = Vector3.zero;

        return obj.transform;
    }

    /// <summary>
    /// 加载贴图
    /// </summary>
    /// <param name="textureName"></param>
    /// <returns></returns>
    public Texture LoadTexture(string textureName)
    {
        Texture texture = null;
        texture = Resources.Load<Texture>("Texture/" + textureName);

        return texture;
    }
}