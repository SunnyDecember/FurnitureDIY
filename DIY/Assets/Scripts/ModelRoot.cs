using UnityEngine;
using System.Collections;

/* Author:       Running
** Time:         18.11.18
** Describtion:  
*/

public class ModelRoot : MonoBehaviour
{

    void Awake()
    {
        EventCenter.Instance.RegisterEvent(EventName.CreateModel, CreateModel);
    }
     
    void Start ()
    {
        
    }

    /// <summary>
    /// 创建模型作为自己的子节点。
    /// </summary>
    /// <param name="args"></param>
    void CreateModel(params object[] args)
    {
        string modelName = (string)args[0];
        GameObject model = ResourceManager.Instance.LoadModel(transform, modelName);
        if (null == model.GetComponent<ModelCategory>())
        {
            model.AddComponent<ModelCategory>();
        }
    }

    private void OnDestroy()
    {
        EventCenter.Instance.UnRegisterEvent(EventName.CreateModel, CreateModel);
    }
}