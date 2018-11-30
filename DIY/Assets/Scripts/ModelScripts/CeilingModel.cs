using UnityEngine;
using System.Collections;

/*
** Author      : Runing
** Time        : 11/30/2018 10:41:18 PM
** description : Ìì»¨°å
*/

public class CeilingModel : ModelCategory
{
    protected override void Start()
    {
        base.Start();
        selfCategory = ECategory.ceiling;
        recognitionCategory = ECategory.pendant;
    }
}
