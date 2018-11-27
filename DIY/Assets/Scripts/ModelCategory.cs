using UnityEngine;
using System.Collections;

/* Author:       Running
** Time:         18.11.16
** Describtion:  
*/

public class ModelCategory : MonoBehaviour
{
    public enum ECategory
    {
        none = 1 << 0,
        floor = 1 << 1,      //地板
        wall = 1 << 2,       //墙壁
        hang = 1 << 3,       //挂饰 (例如壁画之类的, 能挂在墙上的)
        furniture = 1 << 4,  //家具 (例如椅子，桌子,电视，床,水杯，台灯)
        ceiling = 1 << 5,     //天花板
        chandelier = 1 << 6,  //吊灯
        //wallPaper = 1<< 7,   //壁纸
    }

    //[Title("我是哪种类型")]
    //[HideInInspector]
    public ECategory selfCategory;

    //[Title("能被哪种类型  贴在自身上")]
    //[HideInInspector]
    public ECategory recognitionCategory;

    void Awake()
    {
        //通过名字前缀来判断当前的模型是什么类型的。
        if (name.Contains("floor_"))
        {
            selfCategory = ECategory.floor;
            recognitionCategory = ECategory.furniture;
        }
        else if (name.Contains("wall_"))
        {
            selfCategory = ECategory.wall;
            recognitionCategory = ECategory.hang;
        }
        else if (name.Contains("hang_"))
        {
            selfCategory = ECategory.hang;
            recognitionCategory = ECategory.none;
        }
        else if (name.Contains("furniture_"))
        {
            selfCategory = ECategory.furniture;
            recognitionCategory = ECategory.furniture;
        }
        else if (name.Contains("ceiling_"))
        {
            selfCategory = ECategory.ceiling;
            recognitionCategory = ECategory.chandelier;
        }
        else if(name.Contains("chandelier_"))
        {
            selfCategory = ECategory.chandelier;
            recognitionCategory = ECategory.none;
        }
        else if (name.Contains("wallPaper_"))
        {
            selfCategory = ECategory.hang;
            recognitionCategory = ECategory.none;
        }
        else
        {
            selfCategory = ECategory.none;
            recognitionCategory = ECategory.none;
        }
    }

    /// <summary>
    /// 在场景中，是否能被移除。
    /// </summary>
    /// <returns></returns>
    public bool CanDelete()
    {
        //目前只有挂饰和家具能被删除的。
        return (selfCategory & (ECategory.hang | ECategory.furniture)) > 0;
    }
}