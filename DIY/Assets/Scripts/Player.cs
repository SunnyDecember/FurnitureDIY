using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;

/* Author:       Running
** Time:         18.11.16
** Describtion:  
*/

public class Player : MonoBehaviour
{
    /// <summary>
    /// 玩家眼睛摄像机
    /// </summary>
    [SerializeField]
    private Camera _eye;

    /// <summary>
    /// 移动速度
    /// </summary>
    private float _moveSpeed = 6f;

    /// <summary>
    /// 右键在屏幕滑动时候，上一次的位置。（用来控制旋转）
    /// </summary>
    private Vector3 _lastPosition = Vector3.zero;

    private void Awake()
    {
        //添加需要用的功能.
        (new GameObject("GlobalUpdate")).AddComponent<GlobalUpdate>();
        RayEvent.Instance.Init();
    }

    private void Start()
    {
        RayEvent.Instance.mouseLeftDownEvnet += mouseLeftDown;
        RayEvent.Instance.mouseLeftUpEvnet += mouseLeftUp;
        RayEvent.Instance.mouseLeftDragEvnet += mouseLeftDrag;

        RayEvent.Instance.mouseRightDownEvnet += mouseRightDown;
        RayEvent.Instance.mouseRightUpEvnet += mouseRightUp;
        RayEvent.Instance.mouseRightDragEvnet += mouseRightDrag;
    }

    private void Update()
    {
        //控制自身的位置
        OperateSelfPosition();
    }

    private void mouseLeftDown(Vector3 mousePosition, RaycastHit hit)
    {
        if (null != hit.transform)
        {

        }
    }

    private void mouseLeftUp()
    {
        if (null != RayEvent.Instance.clickObjectTargetObj)
        {
            ModelCategory selectCategory = RayEvent.Instance.clickObjectOfLeftButton.GetComponentInParent<ModelCategory>();
            ModelCategory targetObjCategory = RayEvent.Instance.clickObjectTargetObj.GetComponentInParent<ModelCategory>();
            //拖动的是墙纸，目标物体是墙
            if (selectCategory.selfCategory == targetObjCategory.recognitionCategory && selectCategory.name.Contains("wallPaper"))
            {
                //给墙指定上墙纸的贴图，然后销毁墙纸
                Texture wallTexture = selectCategory.GetComponent<MeshRenderer>().material.mainTexture;
                targetObjCategory.GetComponent<MeshRenderer>().material.mainTexture = wallTexture;
                //给RayEvent.Instance.clickObjectOfLeftButton = null，防止后续操作的空引用
                GameObject destroyObj = RayEvent.Instance.clickObjectOfLeftButton.gameObject;
                RayEvent.Instance.clickObjectOfLeftButton = null;
                Destroy(destroyObj);
            }
        }
    }

    /// <summary>
    /// 思路是，鼠标点下时候记录点中的物体A。发出射线，遍历所有被射线穿透的物体，与物体A比较，
    /// 看物体A能在谁上面移动。
    /// </summary>
    /// <param name="mousePosition"></param>
    /// <param name="hitInfoArray"></param>
    private void mouseLeftDrag(Vector3 mousePosition, RaycastHit[] hitInfoArray)
    {
        Transform clickObjectOfLeftButton = RayEvent.Instance.clickObjectOfLeftButton;

        if (null == clickObjectOfLeftButton || null == clickObjectOfLeftButton.GetComponent<ModelCategory>()) return;

        for (int i = 0; i < hitInfoArray.Length; i++)
        {
            RaycastHit hitInfo = hitInfoArray[i];
            Transform hitTran = hitInfo.transform;

            //如果射线射中的是正在选中的，不处理。
            if (hitTran == clickObjectOfLeftButton)
                continue;

            //射中的物体带有ModelCategory脚本。
            if (null != hitTran.GetComponent<ModelCategory>() && hitTran.tag != "AxisMove")
            {
                ModelCategory hitCategory = hitTran.GetComponent<ModelCategory>();
                ModelCategory selectCategory = clickObjectOfLeftButton.GetComponent<ModelCategory>();

                Debug.Log(hitTran.name + "  self:" + hitCategory.selfCategory + "  other:" + hitCategory.recognitionCategory);

                if ((hitCategory.recognitionCategory & selectCategory.selfCategory) > 0)
                {
                    //如果是墙
                    if ((hitCategory.selfCategory & ModelCategory.ECategory.wall) > 0)
                    {
                        clickObjectOfLeftButton.position = hitInfo.point;
                        clickObjectOfLeftButton.rotation = Quaternion.LookRotation(hitInfo.normal);
                        RayEvent.Instance.clickObjectTargetObj = hitTran;
                    }
                    //如果是天花板
                    else if ((hitCategory.selfCategory & ModelCategory.ECategory.ceiling) > 0)
                    {
                        clickObjectOfLeftButton.position = hitInfo.point;
                        clickObjectOfLeftButton.rotation = Quaternion.LookRotation(hitInfo.normal);
                    }
                    else
                    {
                        //当前选中的物体，只能放在另一个物体的垂直位置上。也就是说，不能放在他的侧面。
                        //比如，杯子能放在椅子上面，但是不能放在椅子周围的四个面上。
                        if (hitInfo.normal.x > -0.1f && hitInfo.normal.x < 0.1f &&
                           hitInfo.normal.y > 0.9f && hitInfo.normal.y < 1.1f &&
                           hitInfo.normal.z > -0.1f && hitInfo.normal.z < 0.1f)
                        {
                            clickObjectOfLeftButton.position = hitInfo.point;
                            clickObjectOfLeftButton.rotation = Quaternion.LookRotation(hitInfo.normal);
                        }
                    }

                    Debug.Log("Player.Update() 射中点的法向量  " + hitInfo.normal.x + " " + hitInfo.normal.y + " " + hitInfo.normal.z);
                }
            }
        }
    }

    /// <summary>
    /// 控制自身的移动
    /// </summary>
    void OperateSelfPosition()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        transform.Translate(transform.forward * Time.deltaTime * vertical * _moveSpeed, Space.World);
        transform.Translate(transform.right * Time.deltaTime * horizontal * _moveSpeed, Space.World);
    }

    private void mouseRightDown(Vector3 mousePosition, RaycastHit hit)
    {
        _lastPosition = mousePosition;
    }

    private void mouseRightUp()
    {
        _lastPosition = Vector3.zero;
    }

    private void mouseRightDrag(Vector3 mousePosition, RaycastHit[] hitArray)
    {
        Vector3 positionDelta = Input.mousePosition - _lastPosition;

        //横向旋转
        transform.Rotate(new Vector3(0, positionDelta.x * 0.2f, 0));

        //纵向旋转
        _eye.transform.Rotate(new Vector3(-positionDelta.y * 0.2f, 0, 0));

        Quaternion quaternion = _eye.transform.localRotation;
        Vector3 axis = Vector3.zero;
        float angle = 0f;

        //仰视和俯视最大的角度为40.
        quaternion.ToAngleAxis(out angle, out axis);
        angle = Mathf.Clamp(angle, -40, 40);
        quaternion = Quaternion.AngleAxis(angle, axis);
        _eye.transform.localRotation = quaternion;

        //保存当前的鼠标移动位置
        _lastPosition = Input.mousePosition;
    }
}