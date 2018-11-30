using UnityEngine;
using System.Collections;

/*
** Author      : Runing
** Time        : 11/30/2018 10:42:31 PM
** description : 
*/

public class PendantModel : ModelCategory
{
    protected override void Start()
    {
        base.Start();
        selfCategory = ECategory.pendant;
        recognitionCategory = ECategory.none;
    }

    public override bool CanDelete()
    {
        return true;
    }
}