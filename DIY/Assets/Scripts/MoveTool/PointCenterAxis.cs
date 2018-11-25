using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace liu
{
    /// <summary>
    /// 选中Center Axis 进行整体拖拽
    /// </summary>
    public class PointCenterAxis : MonoBehaviour
    {
        [HideInInspector]
        public bool IsBeSelected = false;

        private bool isMouseDrag = false;
        private Vector3 screenPosition;
        private Vector3 offset;

        private Color normalColor;
        private Color baseSelectedColor;
        private MeshRenderer mr;

        private Transform dragObjTransform;

        void Start()
        {
            dragObjTransform = transform.parent.parent.parent;

            mr = GetComponent<MeshRenderer>();
            normalColor = mr.material.color;
            baseSelectedColor = new Color32(210, 137, 242, 255);
        }

        void Update()
        {
            mr.material.color = IsBeSelected ? baseSelectedColor : normalColor;

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                screenPosition = Camera.main.WorldToScreenPoint(transform.position);
                offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
                             Input.mousePosition.y, screenPosition.z));
                isMouseDrag = true;
            }

            if (Input.GetKey(KeyCode.Mouse0))
            {
                if (isMouseDrag && IsBeSelected)
                {
                    Vector3 currentScreenSpace = new Vector3(Input.mousePosition.x,Input.mousePosition.y,
                        screenPosition.z);
                    Vector3 currentPosition = Camera.main.ScreenToWorldPoint(currentScreenSpace) + offset;
                    dragObjTransform.position = currentPosition;
                }
            }

            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                isMouseDrag = false;
            }
        }
    }
}