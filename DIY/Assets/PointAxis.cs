using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace liu.MoveTool
{
    /// <summary>
    /// X、Y、Z轴拖拽的实现
    /// </summary>
    public class PointAxis : MonoBehaviour
    {
        enum PointAxis_Type
        {
            Axis_X,
            Axis_Y,
            Axis_Z,
        }
        /// <summary>
        /// 没有被选中的时候，X、Y、Z轴的颜色
        /// </summary>
        public static Color32 colorNormalX = new Color32(255, 100, 100, 255);
        public static Color32 colorNormalY = new Color32(100, 255, 100, 255);
        public static Color32 colorNormalZ = new Color32(100, 100, 255, 255);

        /// <summary>
        /// 被选中后，X、Y、Z轴的颜色
        /// </summary>
        public static Color colorBeSelectedX = Color.red;
        public static Color colorBeSelectedY = Color.green;
        public static Color colorBeSelectedZ = Color.blue;

        public bool isBeSelected;

        [SerializeField]
        private PointAxis_Type curAxisType;

        /// <summary>
        /// 用于修改当前选中物体的颜色
        /// </summary>
        private MeshRenderer curMr;

        private Color colorNormal;
        private Color colorBeSelected;

        private bool isMouseDrag = false;
        private Vector3 screenPosition;
        private Vector3 offset;

        /// <summary>
        /// 需要拖动物体的Transform
        /// </summary>
        private Transform dragObjTransform;

        private Vector3 dragBrforeGameObjPos;

        void Start()
        {
            if (name.Contains("X"))
            {
                curAxisType = PointAxis_Type.Axis_X;
            }
            else if (name.Contains("Y"))
            {
                curAxisType = PointAxis_Type.Axis_Y;
            }
            else if (name.Contains("Z"))
            {
                curAxisType = PointAxis_Type.Axis_Z;
            }

            //transform 为当前轴，transform.parent为整个拖拽的物体
            //transform.parent.parent.parent 为拖拽的物体
            //transform.parent.parent 为挂在拖拽物体下的Tools
            dragObjTransform = transform.parent.parent.parent;

            curMr = GetComponent<MeshRenderer>();
            //给X、Y、Z轴设置颜色
            switch (curAxisType)
            {
                case PointAxis_Type.Axis_X:
                    colorNormal = colorNormalX;
                    colorBeSelected = colorBeSelectedX;
                    break;
                case PointAxis_Type.Axis_Y:
                    colorNormal = colorNormalY;
                    colorBeSelected = colorBeSelectedY;
                    break;
                case PointAxis_Type.Axis_Z:
                    colorNormal = colorNormalZ;
                    colorBeSelected = colorBeSelectedZ;
                    break;
            }
        }

        void Update()
        {
            curMr.material.color = isBeSelected ? colorBeSelected : colorNormal;
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                dragBrforeGameObjPos = dragObjTransform.position;
                screenPosition = Camera.main.WorldToScreenPoint(dragBrforeGameObjPos);
                offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
                             Input.mousePosition.y, screenPosition.z));
                isMouseDrag = true;
            }

            if (Input.GetKey(KeyCode.Mouse0))
            {
                if (isMouseDrag && isBeSelected)//开始拖拽
                {
                    Vector3 currentScreenSpace =
                        new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPosition.z);
                    Vector3 currentPosition = Camera.main.ScreenToWorldPoint(currentScreenSpace) + offset;
                    float tempLength = Vector3.Distance(currentPosition, transform.position);

                    Vector3 tempPos = Vector3.zero;

                    switch (curAxisType)
                    {
                        case PointAxis_Type.Axis_X:
                            tempPos = Vector3.Project(currentPosition, transform.right) + Vector3.Project(
                                          dragBrforeGameObjPos,
                                          transform.up) + Vector3.Project(dragBrforeGameObjPos, transform.forward);
                            //Debug.Log(PointAxis_Type.Axis_X);
                            break;
                        case PointAxis_Type.Axis_Y:
                            tempPos = Vector3.Project(currentPosition, transform.up) + Vector3.Project(
                                          dragBrforeGameObjPos,
                                          transform.right) + Vector3.Project(dragBrforeGameObjPos, transform.forward);
                            //Debug.Log(PointAxis_Type.Axis_Y);
                            break;
                        case PointAxis_Type.Axis_Z:
                            tempPos = Vector3.Project(currentPosition, transform.forward) + Vector3.Project(
                                          dragBrforeGameObjPos,
                                          transform.up) + Vector3.Project(dragBrforeGameObjPos, transform.right);
                            
                            //Debug.Log(PointAxis_Type.Axis_Z);
                            break;
                    }

                    dragObjTransform.position = tempPos;
                }

                if (Input.GetKeyUp(KeyCode.Mouse0))
                {
                    isMouseDrag = false;
                }
            }
        }
    }
}