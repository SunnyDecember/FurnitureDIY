using UnityEditor;
using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Text;

/*
** Author      : Runing
** Time        : 18.11.19 01:01
** description : 用于创建脚本
*/

public class CreateCSharp : EditorWindow
{
    private static string _scriptFolder = "";

    private static string _scriptName = "";

    private static string _scriptDes = "";

    [MenuItem("Assets/Create/Add C# Class #&c")]
    private static void Create()
    {
        EditorWindow window = EditorWindow.GetWindow(typeof(CreateCSharp), true, "创建C#");
        window.Show();
    }

    private void OnGUI()
    {
        GUILayout.Label("脚本名字");
        _scriptName = GUILayout.TextField(_scriptName);

        GUILayout.Label("脚本作用描述");
        _scriptDes = GUILayout.TextField(_scriptDes);

        if (string.IsNullOrEmpty(_scriptName))
            return;

        if (GUILayout.Button("确定创建"))
        {
            UnityEngine.Object obj = Selection.activeObject;
            int id = Selection.activeInstanceID;
            _scriptFolder = AssetDatabase.GetAssetPath(id);

            if (string.IsNullOrEmpty(_scriptFolder))
            {
                ShowNotification(new GUIContent("选个文件夹吧"));
                return;
            }

            CreateScript();
        }
    }

    private void CreateScript()
    {
        string scriptPath = Path.Combine(_scriptFolder, _scriptName + ".cs");

        if (File.Exists(scriptPath))
        {
            ShowNotification(new GUIContent("已有这名字了"));   
            return;
        }
        
        using (StreamWriter outfile = new StreamWriter(scriptPath))
        {
            outfile.WriteLine("using UnityEngine;");
            outfile.WriteLine("using System.Collections;");
            outfile.WriteLine("");
            outfile.WriteLine("/*");
            outfile.WriteLine("** Author      : Runing");
            outfile.WriteLine("** Time        : " + DateTime.Now);
            outfile.WriteLine("** description : " + _scriptDes);
            outfile.WriteLine("*/");
            outfile.WriteLine("");
            outfile.WriteLine("public class " + _scriptName + " : MonoBehaviour");
            outfile.WriteLine("{");
            outfile.WriteLine("");
            outfile.WriteLine("    private void Start()");
            outfile.WriteLine("    {");
            outfile.WriteLine("        ");
            outfile.WriteLine("    }");
            //outfile.WriteLine("");
            //outfile.WriteLine("    private void Update()");
            //outfile.WriteLine("    {");
            //outfile.WriteLine("        ");
            //outfile.WriteLine("    }");
            outfile.WriteLine("}");
        }

        AssetDatabase.Refresh();
        Close();
        //selected.AddComponent(Type.GetType(name));
    }
}