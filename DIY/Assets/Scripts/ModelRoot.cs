using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/* Author:       Running
** Time:         18.11.18
** Describtion:  
*/

public class ModelRoot : MonoBehaviour
{
    private List<ModelCategory> _allModel = new List<ModelCategory>();

    private void Awake()
    {
        EventCenter.Instance.RegisterEvent(EventName.CreateModel, CreateModel);
    }

    private void Start ()
    {
        
    }

    /// <summary>
    /// 创建模型作为自己的子节点。
    /// </summary>
    /// <param name="args"></param>
    private void CreateModel(params object[] args)
    {
        string modelName = (string)args[0];
        ModelCategory model = ResourceManager.Instance.LoadModel(transform, modelName);
        _allModel.Add(model);
    }

    private void OnDestroy()
    {
        EventCenter.Instance.UnRegisterEvent(EventName.CreateModel, CreateModel);
    }
}