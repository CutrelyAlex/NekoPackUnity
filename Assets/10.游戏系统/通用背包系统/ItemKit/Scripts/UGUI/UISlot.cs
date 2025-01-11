using QFramework.Example;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace QFramework
{

    public class UISlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public Text Name;
        public Text Count;

        public Slot Data { get; set; }

        private bool mDragging = false;

        public UISlot InitWithData(Slot data)
        {
            Data = data;
            if (Data.Count == 0)
            {
                Name.text = "空";
                Count.text = "";
            }
            else
            {
                Name.text = Data.Item.Name;
                Count.text = Data.Count.ToString();
            }
            return this;
        }

        void SyncItemToMousePos()
        {
            var mousePos = Input.mousePosition;
            // 将屏幕坐标转换为本地坐标, 以便于计算拖拽的位置
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(transform as RectTransform, mousePos, null,
                                                                        out Vector2 localPos))
            {
                Name.LocalPosition2D(localPos); // 设置物品名字的位置
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (mDragging) return;
            mDragging = true;
            SyncItemToMousePos(); // 同步物品到鼠标位置
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (mDragging)
            {
                SyncItemToMousePos(); // 同步物品到鼠标位置
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (mDragging)
            {
                Name.LocalPositionIdentity(); // 恢复物品名字的位置

                bool throwItem = true;

                // 检测是否在任意UISlot上
                UISlot[] uiSlots = transform.parent.GetComponentsInChildren<UISlot>();
                foreach (UISlot slot in uiSlots)
                {
                    RectTransform recTransform = slot.transform as RectTransform;
                    if (RectTransformUtility.RectangleContainsScreenPoint(recTransform, Input.mousePosition))
                    {
                        throwItem = false;
                    };
                }

                if(throwItem)
                {
                    Data.Item = null;
                    Data.Count = 0;
                    FindAnyObjectByType<UGUIInventoryExample>().Refresh();
                }
            }
        }
    }
}

