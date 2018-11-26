using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace liu.MoveTool
{
    /// <summary>
    /// 控制物体的移动
    /// </summary>
    public class ControlObjMove : MonoBehaviour
    {
        private static ControlObjMove _instance;

        private bool isMouseDrag = false;

        private PointAxis PA;

        private PointCenterAxis PCA;

        public static ControlObjMove Instance
        {
            get
            {
                if (null == _instance) _instance = FindObjectOfType<ControlObjMove>();
                return _instance;
            }
        }

        void Update()
        {
            Ray tempRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(tempRay, out hit, 2000))
            {
                PointCenterAxis tempPCA = hit.collider.GetComponent<PointCenterAxis>();
                PointAxis tempPA = hit.collider.GetComponent<PointAxis>();
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    if (null != tempPCA)
                    {
                        Reset_SelectedState();
                        PCA = tempPCA;
                        PCA.IsBeSelected = true;
                    }
                    else if (null != tempPA)
                    {
                        Reset_SelectedState();
                        PA = tempPA;
                        PA.isBeSelected = true;
                    }
                }
            }

            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                Reset_SelectedState();
            }
        }

        /// <summary>
        /// 设置模型回归root 不需要移动，或者对选中物体失去选中状态
        /// 或者删除某物体，然后物体上有DragVector
        /// </summary>
        public void SetParentNull()
        {
            gameObject.transform.parent = null;
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// 设置模型的父物体
        /// </summary>
        /// <param name="parent"></param>
        public void SetParentDragObj(Transform parent)
        {
            transform.parent = parent;
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(true);
            }
        }

        /// <summary>
        /// 重置数据
        /// </summary>
        private void Reset_SelectedState()
        {
            if (null != PCA)
            {
                PCA.IsBeSelected = false;
                PCA = null;
            }

            if (null != PA)
            {
                PA.isBeSelected = false;
                PA = null;
            }
        }
    }
}