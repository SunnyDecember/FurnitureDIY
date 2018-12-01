using UnityEngine;
using System.Collections.Generic;
using LitJson;
using System.IO;
using System.Text;

/*
** Author      : Runing
** Time        : 12/1/2018 8:20:24 PM
** description : Used for recording and recover scenes
*/

public class SceneBuffer
{
    private static SceneBuffer _instance;

    private Dictionary<string, AllObjectBuffer> _allObjectBufferDictionary = new Dictionary<string, AllObjectBuffer>();

    public static SceneBuffer Instance
    {
        get { return _instance ?? (_instance = new SceneBuffer()); }
    }

    /// <summary>
    /// Objects that need to be record
    /// </summary>
    public void Record(string sceneName, List<Transform> objectList)
    {
        AllObjectBuffer objectBuffer = new AllObjectBuffer();
        objectBuffer.Record(objectList);

        if (_allObjectBufferDictionary.ContainsKey(sceneName))
            _allObjectBufferDictionary.Remove(sceneName);
        _allObjectBufferDictionary.Add(sceneName, objectBuffer);
        
        //Save to local
        string jsonPath = Path.Combine(Application.dataPath, sceneName + ".runing").Replace("\\", "/");

        //StreamWriter streamWriter = File.CreateText(jsonPath);
        FileStream fileStream = File.Create(jsonPath);
        StreamWriter streamWriter = new StreamWriter(fileStream, Encoding.GetEncoding("utf-8"));

        JsonMapper.ToJson(objectBuffer, new JsonWriter(streamWriter) { PrettyPrint = true });
        streamWriter.Flush();
        streamWriter.Close();
        fileStream.Close();
    }

    /// <summary>
    /// Recover those objects that have been recorded
    /// </summary>
    public List<Transform> Recover(string sceneName, Transform modelRoot)
    {
        //这块可以搬到进入app时候马上加载。
        string jsonPath = Path.Combine(Application.dataPath, sceneName + ".runing").Replace("\\", "/");
        string json = File.ReadAllText(jsonPath, Encoding.GetEncoding("utf-8"));
        AllObjectBuffer buffer = JsonMapper.ToObject<AllObjectBuffer>(json);

        if (_allObjectBufferDictionary.ContainsKey(sceneName))
            _allObjectBufferDictionary.Remove(sceneName);
        _allObjectBufferDictionary.Add(sceneName, buffer);




        //if (!_allObjectBufferDictionary.ContainsKey(sceneName))
        //{
        //    Debug.LogError("SceneBuffer.Recover(): The scene was not found when recovering ! ! !");
        //    return null;
        //}

        AllObjectBuffer allObjectBuffer = _allObjectBufferDictionary[sceneName];
        return allObjectBuffer.Recover(modelRoot);
    }
}
