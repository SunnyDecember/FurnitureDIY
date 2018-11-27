
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;

namespace liu
{

    public static class GetModelPath
    {

        public static void OpenFileDialog()
        {
            OpenFileName ofn = new OpenFileName();

            ofn.structSize = Marshal.SizeOf(ofn);

            //三菱(*.gxw)\0*.gxw\0西门子(*.mwp)\0*.mwp\0All Files\0*.*\0\0
            ofn.filter = "模型(*.fbx)\0*.fbx\0模型(*.obg)\0*.obg\0All Files\0*.*\0\0";

            ofn.file = new string(new char[256]);

            ofn.maxFile = ofn.file.Length;

            ofn.fileTitle = new string(new char[64]);

            ofn.maxFileTitle = ofn.fileTitle.Length;

            ofn.initialDir = "桌面";//默认路径

            ofn.title = "选择模型所在的文件地址";

            //ofn.defExt = "FBX";//显示文件的类型、
            //ofn.defExt = "fbx files (*.fbx)|*.fbx|obj files (*.obj)|*.obj";  //显示文件的类型
            //注意 一下项目不一定要全选 但是0x00000008项不要缺少
            ofn.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000200 | 0x00000008;//OFN_EXPLORER|OFN_FILEMUSTEXIST|OFN_PATHMUSTEXIST| OFN_ALLOWMULTISELECT|OFN_NOCHANGEDIR

            if (DllTest.GetOpenFileName(ofn))
            {
                //字符串小写，好简便判断选取的类型
                string filePath = ofn.file.ToLower();

                FileInfo fi = new FileInfo(filePath);
                if (fi.Extension == ".fbx" || fi.Extension == ".obj")
                {
                    GameObject go = LoadModelFormLocal.LoadModel(filePath);
                    if (null != go)
                    {
                        //进行
                    }
                }

                Debug.Log("Selected file with full path: {0}" + ofn.file);
            }

        }
    }
}


[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]

public class OpenFileName
{
    public int structSize = 0;
    public IntPtr dlgOwner = IntPtr.Zero;
    public IntPtr instance = IntPtr.Zero;
    public String filter = null;
    public String customFilter = null;
    public int maxCustFilter = 0;
    public int filterIndex = 0;
    public String file = null;
    public int maxFile = 0;
    public String fileTitle = null;
    public int maxFileTitle = 0;
    public String initialDir = null;
    public String title = null;
    public int flags = 0;
    public short fileOffset = 0;
    public short fileExtension = 0;
    public String defExt = null;
    public IntPtr custData = IntPtr.Zero;
    public IntPtr hook = IntPtr.Zero;
    public String templateName = null;
    public IntPtr reservedPtr = IntPtr.Zero;
    public int reservedInt = 0;
    public int flagsEx = 0;
}

public class DllTest
{
    [DllImport("Comdlg32.dll", SetLastError = true, ThrowOnUnmappableChar = true, CharSet = CharSet.Auto)]
    public static extern bool GetOpenFileName([In, Out] OpenFileName ofn);
    public static bool GetOpenFileName1([In, Out] OpenFileName ofn)

    {
        return GetOpenFileName(ofn);
    }
}