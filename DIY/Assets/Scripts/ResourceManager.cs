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
    public GameObject LoadModel(Transform parent, string modelName)
    {
        GameObject obj = GameObject.Instantiate(Resources.Load("Model/" + modelName)) as GameObject;

        GameObject tools = new GameObject("Tools");
        tools.transform.SetParent(obj.transform);
        tools.transform.localPosition = Vector3.zero;
        tools.transform.localScale = Vector3.one;
        tools.transform.localEulerAngles = Vector3.zero;

        obj.transform.parent = parent;
        obj.transform.localPosition = Vector3.zero;
        return obj;
    }
}