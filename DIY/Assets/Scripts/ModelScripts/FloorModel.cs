using UnityEngine;
using System.Collections;

/*
** Author      : Runing
** Time        : 11/30/2018 10:39:11 PM
** description : 
*/

public class FloorModel : ModelCategory
{
    protected override void Start()
    {
        base.Start();
        selfCategory = ECategory.floor;
        recognitionCategory = ECategory.furniture;
    }
}