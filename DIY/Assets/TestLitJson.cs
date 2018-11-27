using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLitJson : MonoBehaviour
{

    public class Pet
    {
        public string name;
        public string localPath;
        public string storePath;
    }

    void Start()
    {
        Pet aPet = new Pet()
        {
            name = "lucky",
            localPath = "Loacl",
            storePath = "good",
        };

        Pet bPet = new Pet()
        {
            name = "lucky1",
            localPath = "Loacl2",
            storePath = "good3",
        };
        string text = JsonMapper.ToJson(aPet);
        Debug.Log(text);

        string text1 = @"
        {
            ""name""  : ""菜鸟海澜"",
            ""age""   : 2018,
            ""awake"" : true,
            ""n""     : 2018.0517,
            ""note""  : [ ""life"", ""is"", ""but"", ""a"", ""dream"" ]
        },{
            ""name""  : ""菜鸟海澜1111"",
            ""age""   : 2018,
            ""awake"" : true,
            ""n""     : 2018.0517,
            ""note""  : [ ""life"", ""is"", ""but"", ""a"", ""dream"" ]
        }";
        JsonReader reader = new JsonReader(text1);
        // Read（）方法返回false时，没有其他内容可读
        while (reader.Read())
        {
            string type = reader.Value != null ?
                reader.Value.GetType().ToString() : "";

            Debug.Log(type);
        }

    }
}
