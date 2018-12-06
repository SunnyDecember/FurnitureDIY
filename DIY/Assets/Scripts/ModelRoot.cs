using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/* Author:       Running
** Time:         18.11.18
** Describtion:  
*/

public class ModelRoot : MonoBehaviour
{
    private List<Transform> _allModel = new List<Transform>();

    private static int _modelIndex = 0;

    private void Awake()
    {
        EventCenter.Instance.RegisterEvent(EventName.CreateModel, CreateModel);
        EventCenter.Instance.RegisterEvent(EventName.DeleteModel, DeleteModel);
        EventCenter.Instance.RegisterEvent(EventName.RecordScene, RecordScene);
        EventCenter.Instance.RegisterEvent(EventName.RecoverScene, RecoverScene);
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
        Transform model = ResourceManager.Instance.LoadModel(transform, modelName);
        model.name = modelName + "_" + _modelIndex++;
        _allModel.Add(model);
    }

    private void DeleteModel(params object[] args)
    {
        Transform model = null;

        if (args[0] is GameObject)
        {
            model = (args[0] as GameObject).transform;
        }
        else
        {
            model = args[0] as Transform;
        }
        
        if (_allModel.Contains(model))
        {
            _allModel.Remove(model);
        }

        Destroy(model.gameObject);
    }

    /// <summary>
    /// Record the scene
    /// </summary>
    /// <param name="args"></param>
    private void RecordScene(params object[] args)
    {
        SceneBuffer.Instance.RecordModel("SceneName", _allModel);
    }

    /// <summary>
    /// Recover the scene
    /// </summary>
    /// <param name="args"></param>
    private void RecoverScene(params object[] args)
    {
        _allModel = SceneBuffer.Instance.Recover("SceneName", transform);
    }

    private void OnDestroy()
    {
        EventCenter.Instance.UnRegisterEvent(EventName.CreateModel, CreateModel);
        EventCenter.Instance.UnRegisterEvent(EventName.DeleteModel, DeleteModel);
        EventCenter.Instance.UnRegisterEvent(EventName.RecordScene, RecordScene);
        EventCenter.Instance.UnRegisterEvent(EventName.RecoverScene, RecoverScene);
    }
}