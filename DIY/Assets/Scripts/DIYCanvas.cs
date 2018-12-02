using UnityEngine;
using System.Collections;
using liu;
using liu.MoveTool;
using UnityEngine.UI;

/* Author:       Running
** Time:         18.11.18
** Describtion:  
*/

/// <summary>
/// 均为测试代码!!!!!!!!!!!!!!!!!!!!!!!!!
/// </summary>
public class DIYCanvas : MonoBehaviour
{
    [SerializeField]
    private Button _environmentColorButton;

    [SerializeField]
    private Button _house;

    [SerializeField]
    private Button _paint;

    [SerializeField]
    private Button _chair;

    [SerializeField]
    private Button _cup;

    [SerializeField]
    private Button _deleteModel;

    [SerializeField]
    private Button _loadModel;

    [SerializeField]
    private Button _loadChandelier;

    [SerializeField]
    private Button _loadWallPaper;

    [SerializeField]
    private Button _record;

    [SerializeField]
    private Button _recover;

    void Start ()
    {
        _environmentColorButton.onClick.AddListener(()=> 
        {
            GameObject colorPanel = ResourceManager.Instance.LoadUIPrefab(transform, "ColorPanel");
            colorPanel.transform.localScale = Vector3.one * 2;
        });

        _house.onClick.AddListener(() =>
        {
            EventCenter.Instance.PostEvent(EventName.CreateModel, "House");
        });

        _paint.onClick.AddListener(()=> 
        {
            EventCenter.Instance.PostEvent(EventName.CreateModel, "hang_paint");
        });

        _chair.onClick.AddListener(() =>
        {
            EventCenter.Instance.PostEvent(EventName.CreateModel, "furniture_chair");
        });

        _cup.onClick.AddListener(() =>
        {
            EventCenter.Instance.PostEvent(EventName.CreateModel, "furniture_cup");
        });

        _loadModel.onClick.AddListener(() =>
        {
            GetModelPath.OpenFileDialog();
        });

        _loadChandelier.onClick.AddListener(() =>
        {
            EventCenter.Instance.PostEvent(EventName.CreateModel, "chandelier_light");
        });

        _loadWallPaper.onClick.AddListener(() =>
        {
            EventCenter.Instance.PostEvent(EventName.CreateModel, "hang_wallPaper");
        });

        //record the scene model
        _record.onClick.AddListener(() =>
        {
            EventCenter.Instance.PostEvent(EventName.RecordScene);
        });

        //recover the scene model
        _recover.onClick.AddListener(() =>
        {
            EventCenter.Instance.PostEvent(EventName.RecoverScene);
        });

        //删除模型
        _deleteModel.onClick.AddListener(()=> 
        {
            Transform TouchObject = RayEvent.Instance.clickObjectOfLeftButton;
            if (null != TouchObject && TouchObject.GetComponent<ModelCategory>())
            {
                ModelCategory modelCategory = TouchObject.GetComponent<ModelCategory>();
                bool canDelete = modelCategory.CanDelete();

                if (canDelete)
                {
                    Transform tools = modelCategory.toolNode;

                    if (tools.childCount > 0)
                    {
                        Transform dragVectorTool = tools.Find("DragVector");
                        if (null != dragVectorTool)
                        {
                            ControlObjMove.Instance.SetParentNull();
                        }
                    }
                    if(TouchObject)
                    GameObject.Destroy(TouchObject.gameObject);
                }
            }
        });
    }
}