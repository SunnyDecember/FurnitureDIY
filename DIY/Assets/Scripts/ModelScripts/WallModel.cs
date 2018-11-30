using UnityEngine;
using System.Collections;

/*
** Author      : Runing
** Time        : 11/30/2018 10:39:45 PM
** description : 
*/

public class WallModel : ModelCategory
{
    protected override void Start()
    {
        base.Start();
        selfCategory = ECategory.wall;
        recognitionCategory = ECategory.hang;
    }
}