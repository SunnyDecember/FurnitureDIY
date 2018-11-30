using UnityEngine;
using System.Collections;

/*
** Author      : Runing
** Time        : 11/30/2018 10:40:52 PM
** description : 
*/

public class FurnitureModel : ModelCategory
{
    protected override void Start()
    {
        base.Start();
        selfCategory = ECategory.furniture;
        recognitionCategory = ECategory.furniture;
    }

    public override void AfterRay(RaycastHit hitInfo)
    {
        //当前选中的物体，只能放在另一个物体的垂直位置上。也就是说，不能放在他的侧面。
        //比如，杯子能放在椅子上面，但是不能放在椅子周围的四个面上。
        if (hitInfo.normal.x > -0.1f && hitInfo.normal.x < 0.1f &&
           hitInfo.normal.y > 0.9f && hitInfo.normal.y < 1.1f &&
           hitInfo.normal.z > -0.1f && hitInfo.normal.z < 0.1f)
        {
            transform.position = hitInfo.point;
            transform.rotation = Quaternion.LookRotation(hitInfo.normal);
        }
    }

    public override bool CanDelete()
    {
        return true;
    }
}
