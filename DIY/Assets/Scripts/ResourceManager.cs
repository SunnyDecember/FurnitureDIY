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
    public ModelCategory LoadModel(Transform parent, string modelName)
    {
        GameObject obj = GameObject.Instantiate(Resources.Load("Model/" + modelName)) as GameObject;

        //Add tool node for model node
        GameObject tool = new GameObject("Tools");
        tool.transform.SetParent(obj.transform);
        tool.transform.localPosition = Vector3.zero;
        tool.transform.localScale = Vector3.one;
        tool.transform.localEulerAngles = Vector3.zero;

        //Attach script to model node
        ModelCategory modelCategory = ModelCategory.AttachToModel(obj.transform);
        modelCategory.toolNode = tool.transform;

        //set parent
        obj.transform.parent = parent;
        obj.transform.localPosition = Vector3.zero;
        return modelCategory;
    }
}