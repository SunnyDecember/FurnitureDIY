using UnityEngine;
using System.Data;
using Excel;
using System.IO;
using System.Collections.Generic;

public struct ModelInfo
{
    public string Category;
    public string Name;
    public string ImageLocalPos;
    public string ModelLocalPos;
};

public class ReadCsv
{
    //private static string excelPath = "F:/WorkProject/PlayAnimation/Assets/Resources/Data/模型数据.xls";
    private static string excelPath = Application.streamingAssetsPath + "/模型数据.xls";
    //public List<ModelInfo> ModelsInfo;
    //void Start()
    //{
    //    //Debug.Log(Application.dataPath);
    //    //string realyPath = Path.Combine(Application.dataPath, excelPath);
    //    ModelsInfo = Load(excelPath);
    //    if (null != ModelsInfo)
    //    {
    //        foreach (ModelInfo data in ModelsInfo)
    //        {
    //            Debug.Log(string.Format("<color=red>{0},{1},{2},{3}</color>", data.Category, data.Name, data.ImageLocalPos, data.ModelLocalPos));
    //        }
    //    }
    //}


    static DataSet ReadExcel()
    {
       
        FileStream stream = File.Open(excelPath, FileMode.Open, FileAccess.Read, FileShare.Read);
        IExcelDataReader excelReader = null;
        if (excelPath.ToLower().EndsWith(".xlsx"))
        {
            excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
        }
        else if (excelPath.ToLower().EndsWith(".xls"))
        {
            excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
        }

        DataSet result = excelReader.AsDataSet();
        return result;
    }

    public static List<ModelInfo> Load()
    {
        List<ModelInfo> _data = new List<ModelInfo>();
        DataSet resultds = ReadExcel();
        int columns = resultds.Tables[0].Columns.Count;
        int rows = resultds.Tables[0].Rows.Count;
        for (int i = 1; i < rows; i++)
        {
            ModelInfo temp_data = new ModelInfo();
            temp_data.Category = resultds.Tables[0].Rows[i][0].ToString();
            temp_data.Name = resultds.Tables[0].Rows[i][1].ToString();
            temp_data.ImageLocalPos = resultds.Tables[0].Rows[i][2].ToString();
            temp_data.ModelLocalPos = resultds.Tables[0].Rows[i][3].ToString();
            _data.Add(temp_data);
        }

        return _data;
    }
}
