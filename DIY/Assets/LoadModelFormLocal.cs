using System.Collections;
using System.Collections.Generic;
using TriLib;
using UnityEngine;

namespace liu
{
    /// <summary>
    /// 从本地导入模型文件
    /// </summary>
    public static class LoadModelFormLocal
    {
        //private string path = "c:\\users\\administrator\\desktop\\ddj\\wl_dx_ddj.fbx";

        //void Start()
        //{
        //    LoadModel(path);
        //}

        //加载模型
        public static GameObject LoadModel(string modelPath)
        {
            GameObject modelObj = null;
            using (var assimpLoader = new AssetLoader())
            {
                try
                {
                    modelObj = assimpLoader.LoadFromFile(modelPath);
                }
                catch (System.Exception exception)
                {
                    Debug.Log(exception.Message);
                }
            }
            return modelObj;
        }

    }
}