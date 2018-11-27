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

    void Start ()
    {
        _environmentColorButton.onClick.AddListener(()=> 
        {
            GameObject colorPanel = ResourceManager.Instance.LoadUIPrefab(transform, "ColorPanel");
            colorPanel.transform.localScale = Vector3.one * 2;
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

        //删除模型
        _deleteModel.onClick.AddListener(()=> 
        {
            Transform TouchObject = GlobalVariable.Instance.TouchObject;
            if (null != TouchObject && TouchObject.GetComponent<ModelCategory>())
            {
                bool canDelete = TouchObject.GetComponent<ModelCategory>().CanDelete();

                if (canDelete)
                {
                    Transform tools = TouchObject.Find("Tools");
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