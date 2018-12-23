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
    /// 右键在屏幕滑动时候，上一次的位置。  （用来控制旋转）
    /// </summary>
    private Vector3 _lastPosition = Vector3.zero;

    private Transform _wall;

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
        SwitchObjectHighing();
    }

    /// <summary>
    /// 切换物体高亮（也就是隐藏上一次高亮，显示当前高亮）
    /// </summary>
    private void SwitchObjectHighing()
    {
        //取消上一次点击物体的高亮
        Transform preClickObject = RayEvent.Instance.previousClickObjectOfLeftButton;
        if (null != preClickObject)
        {
            SetObjectHighing(preClickObject.gameObject, false);
        }

        //显示当前点击物体的高亮
        Transform currentClickObject = RayEvent.Instance.clickObjectOfLeftButton;
        if (null != currentClickObject)
        {
            SetObjectHighing(currentClickObject.gameObject, true);
        }
    }

    /// <summary>
    /// 设置物体是否高亮
    /// </summary>
    /// <param name="isShow"></param>
    private void SetObjectHighing(GameObject obj, bool isShow)
    {
        HighlightableObject hitHighing = obj.GetComponent<HighlightableObject>();

        if (null != hitHighing && isShow)
            hitHighing.ConstantOn();

        if (null != hitHighing && !isShow)
            hitHighing.ConstantOff();
    }

    private void mouseLeftUp()
    {
        if (null != _wall)
        {
            ModelCategory selectCategory = RayEvent.Instance.clickObjectOfLeftButton.GetComponentInParent<ModelCategory>();
            ModelCategory targetObjCategory = _wall.GetComponentInParent<ModelCategory>();
            //拖动的是墙纸，目标物体是墙
            if (selectCategory.selfCategory == targetObjCategory.recognitionCategory && selectCategory.name.Contains("wallPaper"))
            {
                //给墙指定上墙纸的贴图，然后销毁墙纸
                Texture wallTexture = selectCategory.GetComponent<MeshRenderer>().material.mainTexture;
                targetObjCategory.GetComponent<MeshRenderer>().material.mainTexture = wallTexture;
                GameObject destroyObj = RayEvent.Instance.clickObjectOfLeftButton.gameObject;
                RayEvent.Instance.clickObjectOfLeftButton = null;

                EventCenter.Instance.PostEvent(EventName.DeleteModel, destroyObj);
            }
        }

        _wall = null;
    }

    /// <summary>
    /// 思路是，鼠标点下时候记录点中的物体A。发出射线，遍历所有被射线穿透的物体，与物体A比较，
    /// 看物体A能在谁上面移动。
    /// </summary>
    /// <param name="mousePosition">射线在屏幕的位置</param>
    /// <param name="hitInfoArray">射线在移动时候，射中的所有物体数组</param>
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
            //if (null != hitTran.GetComponent<ModelCategory>() && hitTran.tag != "AxisMove")
            if (null != hitTran.GetComponent<ModelCategory>())
            {
                ModelCategory hitCategory = hitTran.GetComponent<ModelCategory>();
                ModelCategory selectCategory = clickObjectOfLeftButton.GetComponent<ModelCategory>();

                if ((hitCategory.recognitionCategory & selectCategory.selfCategory) > 0)
                {
                    //如果是墙
                    if ((hitCategory.selfCategory & ModelCategory.ECategory.wall) > 0)
                    {
                        _wall = hitTran;
                    }

                    if (selectCategory.AfterBeRay(hitInfo))
                        break;
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