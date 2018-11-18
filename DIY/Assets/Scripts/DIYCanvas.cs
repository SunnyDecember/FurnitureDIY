using UnityEngine;
using System.Collections;
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
    }
}