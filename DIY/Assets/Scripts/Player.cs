using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

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
    /// 鼠标左键是否按下(点击并拖拽场景中的物体)
    /// </summary>
    private bool _isMouseLeftPress = false;

    /// <summary>
    /// 鼠标右键是否按下(控制旋转)
    /// </summary>
    private bool _isMouseRightPress = false;

    /// <summary>
    /// 鼠标按下选中的物体
    /// </summary>
    private Transform _selectTran = null;

    /// <summary>
    /// 移动速度
    /// </summary>
    private float _moveSpeed = 6f;

    /// <summary>
    /// 右键在屏幕滑动时候，上一次的位置。（用来控制旋转）
    /// </summary>
    private Vector3 _lastPosition = Vector3.zero;

    void Awake()
    {
        
    }
     
    void Start ()
    {
        
    }

    void Update()
    {
        //控制点击场景中的物体
        OperateHitObject();

        //控制自身的位置
        OperateSelfPosition();

        //控制自身的旋转
        OperateSelfRotation();
    }

    /// <summary>
    /// 思路是，鼠标点下时候记录点中的物体A。发出射线，遍历所有被射线穿透的物体，与物体A比较，
    /// 看物体A能在谁上面移动。
    /// </summary>
    void OperateHitObject()
    {
        //如果有UI遮挡，不发射线。
        if (EventSystem.current.IsPointerOverGameObject()) return;

        //鼠标按下，记录选中的物体
        if (Input.GetMouseButtonDown(0))
        {
            _isMouseLeftPress = true;
            Ray ray = _eye.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                _selectTran = hit.transform;
            }
            else
            {
                _selectTran = null;
            }
        }

        //鼠标放开，重置变量。
        if (Input.GetMouseButtonUp(0))
        {
            _isMouseLeftPress = false;
            _selectTran = null;
        }

        if (_isMouseLeftPress)
        {
            //如果选择的物体为空，或者物体不带ModelCategory脚本的。那么不再往下发射线检测；  （一般情况下，只有外部导入的物体没有ModelCategory脚本）
            if (null == _selectTran || null == _selectTran.GetComponent<ModelCategory>()) return;

            Ray ray = _eye.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hitInfoArray = Physics.RaycastAll(ray);

            //遍历射线射中的所有物体。
            for (int i = 0; i < hitInfoArray.Length; i++)
            {
                RaycastHit hitInfo = hitInfoArray[i];
                Transform hitTran = hitInfo.transform;

                //如果射线射中的是正在选中的，不处理。
                if (hitTran == _selectTran)
                    continue;

                //射中的物体带有ModelCategory脚本。
                if (null != hitTran.GetComponent<ModelCategory>())
                {
                    ModelCategory hitCategory = hitTran.GetComponent<ModelCategory>();
                    ModelCategory selectCategory = _selectTran.GetComponent<ModelCategory>();

                    Debug.Log(hitTran.name + "  self:" + hitCategory.selfCategory + "  other:" + hitCategory.recognitionCategory);

                    if ((hitCategory.recognitionCategory & selectCategory.selfCategory) > 0)
                    {
                        //如果是墙
                        if ((hitCategory.selfCategory & ModelCategory.ECategory.wall) > 0)
                        {
                            _selectTran.position = hitInfo.point;
                            _selectTran.rotation = Quaternion.LookRotation(hitInfo.normal);
                        }
                        //如果是天花板
                        else if ((hitCategory.selfCategory & ModelCategory.ECategory.ceiling) > 0)
                        {
                            _selectTran.position = hitInfo.point;
                            _selectTran.rotation = Quaternion.LookRotation(hitInfo.normal);
                        }
                        else
                        {
                            //当前选中的物体，只能放在另一个物体的垂直位置上。也就是说，不能放在他的侧面。
                            //比如，杯子能放在椅子上面，但是不能放在椅子周围的四个面上。
                            if (hitInfo.normal.x > -0.1f && hitInfo.normal.x < 0.1f &&
                               hitInfo.normal.y > 0.9f && hitInfo.normal.y < 1.1f &&
                               hitInfo.normal.z > -0.1f && hitInfo.normal.z < 0.1f)
                            {
                                _selectTran.position = hitInfo.point;
                                _selectTran.rotation = Quaternion.LookRotation(hitInfo.normal);
                            }
                        }
                        
                        Debug.Log("Player.Update() 射中点的法向量  " + hitInfo.normal.x + " " + hitInfo.normal.y + " " + hitInfo.normal.z);
                    }
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

    /// <summary>
    /// 控制自身的旋转
    /// </summary>
    void OperateSelfRotation()
    {
        if (Input.GetMouseButtonDown(1))
        {
            _isMouseRightPress = true;
            _lastPosition = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(1))
        {
            _isMouseRightPress = false;
            _lastPosition = Vector3.zero;
        }

        if (_isMouseRightPress)
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
}