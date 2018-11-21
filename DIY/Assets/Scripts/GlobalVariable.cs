using UnityEngine;
using System.Collections;

/*
** Author      : Runing
** Time        : 11/21/2018 9:21:14 PM
** description : 全局变量
*/

public class GlobalVariable
{
    private static GlobalVariable _instance;

    public static GlobalVariable Instance
    {
        get { return _instance ?? (_instance = new GlobalVariable()); }
    }
    
    /// <summary>
    /// 点击到的物体。(Player.cs 类中，点击物体时候，会记录在这里)
    /// </summary>
    public Transform TouchObject;
}
