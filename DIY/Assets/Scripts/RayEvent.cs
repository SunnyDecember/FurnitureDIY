using UnityEngine;
using System.Collections;
using System;
using UnityEngine.EventSystems;
//using zSpace.Core;

/*
** Author      : Runing
** Time        : 11/26/2018 10:21:16 PM
** description : 
*/

public class RayEvent
{
    //------------------------左键 Begin-----------------------------------------
    /// <summary>
    /// 鼠标按下回调事件(左键)
    /// </summary>
    public event Action<Vector3, RaycastHit> mouseLeftDownEvnet;

    /// <summary>
    /// 鼠标滑动回调事件(左键)
    /// </summary>
    public event Action<Vector3, RaycastHit[]> mouseLeftDragEvnet;

    /// <summary>
    /// 鼠标抬起回调事件(左键)
    /// </summary>
    public event Action mouseLeftUpEvnet;

    /// <summary>
    /// 上一次左键点击的物体
    /// </summary>
    public Transform previousClickObjectOfLeftButton;

    /// <summary>
    /// 点击的物体(左键)，哪怕鼠标抬起也会一直记录，保留到下一次点击。(从鼠标点下到下一次点下，为一次生命周期)
    /// </summary>
    public Transform clickObjectOfLeftButton;

    /// <summary>
    /// 鼠标点下(左键)，记录到点击的物体，鼠标放开就清空，不会保留。(从鼠标点下到放开，为一次生命周期)
    /// </summary>
    public Transform rawObjectOfLeftButton;

    /// <summary>
    /// 鼠标左键是否按下(点击并拖拽场景中的物体)
    /// </summary>
    public bool isMouseLeftPress = false;

    //------------------------左键 End-----------------------------------------

    //------------------------右键 Begin-----------------------------------------

    /// <summary>
    /// 鼠标按下回调事件(右键)
    /// </summary>
    public event Action<Vector3, RaycastHit> mouseRightDownEvnet;

    /// <summary>
    /// 鼠标滑动回调事件(右键)
    /// </summary>
    public event Action<Vector3, RaycastHit[]> mouseRightDragEvnet;

    /// <summary>
    /// 鼠标抬起回调事件(右键)
    /// </summary>
    public event Action mouseRightUpEvnet;

    /// <summary>
    /// 上一次右键点击的物体
    /// </summary>
    public Transform previousClickObjectOfRightButton;

    /// <summary>
    /// 点击的物体(右键)，哪怕鼠标抬起也会一直记录，保留到下一次点击。
    /// </summary>
    public Transform clickObjectOfRightButton;

    /// <summary>
    /// 鼠标点下(右键)，记录到点击的物体，鼠标放开就清空，不会保留。(从鼠标点下到放开，为一次生命周期)
    /// </summary>
    public Transform rawObjectOfRightButton;

    /// <summary>
    /// 鼠标右键是否按下(点击并拖拽场景中的物体)
    /// </summary>
    public bool isMouseRightPress = false;

    //------------------------右键 End-----------------------------------------

    private static RayEvent _instance;

    public static RayEvent Instance
    {
        get
        {
            return _instance ?? (_instance = new RayEvent());
        }
    }


    public bool isReadyInstanceObj = false;

    public GameObject tempInstanceObj;
    /// <summary>
    /// 初始化
    /// </summary>
    public void Init()
    {
        Timer.Add(-1, Update);
    }

    /// <summary>
    /// 每帧更新一次
    /// </summary>
    /// <param name="arg1"></param>
    /// <param name="arg2"></param>
    private void Update(int arg1, object[] arg2)
    {
        //如果有UI遮挡，不发射线。
        if (EventSystem.current.IsPointerOverGameObject()) return;

        MouseLeftButton();
        MouseRightButton();
    }

    /// <summary>
    /// 鼠标左键
    /// </summary>
    private void MouseLeftButton()
    {
        //ZCore.Pose pose = _core.GetTargetPose(ZCore.TargetType.Primary, ZCore.CoordinateSpace.World);
        //bool isButtonPressed = _core.IsTargetButtonPressed(ZCore.TargetType.Primary, 0);
        //左键按下
        if (Input.GetMouseButtonDown(0))
        {
            isMouseLeftPress = true;
            Vector3 mousePosition = Input.mousePosition;
            Ray ray = Camera.main.ScreenPointToRay(mousePosition);
            RaycastHit hit;

            //上一次点击的物体
            previousClickObjectOfLeftButton = clickObjectOfLeftButton;

            if (Physics.Raycast(ray, out hit))
            {
                //设置锚点的位置调整
                //ModelCategory modelCategory = hit.transform.GetComponentInParent<ModelCategory>();
                //if (modelCategory)
                //{
                //    clickObjectOfLeftButton = modelCategory.transform;
                //    rawObjectOfLeftButton = modelCategory.transform;
                //}
                clickObjectOfLeftButton = hit.transform;
                rawObjectOfLeftButton = hit.transform;
            }
            else
            {
                clickObjectOfLeftButton = null;
                rawObjectOfLeftButton = null;
            }

            if (null != mouseLeftDownEvnet)
            {
                mouseLeftDownEvnet(mousePosition, hit);
            }
        }

        //左键抬起
        if (Input.GetMouseButtonUp(0))
        {
            if (null != mouseLeftUpEvnet) mouseLeftUpEvnet();

            isMouseLeftPress = false;
            rawObjectOfLeftButton = null;
        }

        //左键滑动
        if (isMouseLeftPress)
        {
            Vector3 mousePosition = Input.mousePosition;
            Ray ray = Camera.main.ScreenPointToRay(mousePosition);
            RaycastHit[] hitInfoArray = Physics.RaycastAll(ray);

            if (null != mouseLeftDragEvnet)
            {
                mouseLeftDragEvnet(mousePosition, hitInfoArray);
            }
        }
    }

    private void MouseRightButton()
    {
        //右键按下
        if (Input.GetMouseButtonDown(1))
        {
            isMouseRightPress = true;
            Vector3 mousePosition = Input.mousePosition;
            Ray ray = Camera.main.ScreenPointToRay(mousePosition);
            RaycastHit hit;

            //上一次点击的物体
            previousClickObjectOfRightButton = clickObjectOfRightButton;

            if (Physics.Raycast(ray, out hit))
            {
                clickObjectOfRightButton = hit.transform;
                rawObjectOfRightButton = hit.transform;
            }
            else
            {
                clickObjectOfRightButton = null;
                rawObjectOfRightButton = null;
            }

            if (null != mouseRightDownEvnet)
            {
                mouseRightDownEvnet(mousePosition, hit);
            }
        }

        //右键抬起
        if (Input.GetMouseButtonUp(1))
        {
            isMouseRightPress = false;
            rawObjectOfRightButton = null;

            if (null != mouseRightUpEvnet) mouseRightUpEvnet();
        }

        //右键滑动
        if (isMouseRightPress)
        {
            Vector3 mousePosition = Input.mousePosition;
            Ray ray = Camera.main.ScreenPointToRay(mousePosition);
            RaycastHit[] hitInfoArray = Physics.RaycastAll(ray);

            if (null != mouseRightDragEvnet)
            {
                mouseRightDragEvnet(mousePosition, hitInfoArray);
            }
        }
    }
}