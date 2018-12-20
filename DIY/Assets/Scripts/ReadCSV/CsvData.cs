using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CsvData : MonoBehaviour
{

    //public List<ModelInfo> ModelsInfo;
    //按分类来存模型数据
    Dictionary<string, List<ModelInfo>> categoryModelInfo = new Dictionary<string, List<ModelInfo>>();

    void Start()
    {
        //拿到表数据
        var ModelsInfo = ReadCsv.Load();
        //对表数据的模型按类型分类
        for (int i = 0; i < ModelsInfo.Count; i++)
        {
            string categoryatalogName = ModelsInfo[i].Category;
            List<ModelInfo> tempList;
            //判断分类是否存在，存在则在原基础上添加，不存在则创建，然后Add
            if (categoryModelInfo.ContainsKey(categoryatalogName))
            {
                tempList = categoryModelInfo[categoryatalogName];
                tempList.Add(ModelsInfo[i]);
            }
            else
            {
                tempList = new List<ModelInfo>();
                tempList.Add(ModelsInfo[i]);
                categoryModelInfo.Add(categoryatalogName, tempList);
            }
            
        }
    }

    //通过catalog对表格物体按类型分类
    public List<ModelInfo> GetCategoryModels(string categoryName)
    {
        if (categoryModelInfo.ContainsKey(categoryName))
        {
            return categoryModelInfo[categoryName];
        }
        else
        {
            return null;
        }
    }
}

