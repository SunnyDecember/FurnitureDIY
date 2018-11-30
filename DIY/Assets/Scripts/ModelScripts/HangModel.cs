using UnityEngine;
using System.Collections;

/*
** Author      : Runing
** Time        : 11/30/2018 10:40:17 PM
** description : 
*/

public class HangModel : ModelCategory
{
    protected override void Start()
    {
        base.Start();
        selfCategory = ECategory.hang;
        recognitionCategory = ECategory.none;
    }

    public override bool CanDelete()
    {
        return true;
    }
}
