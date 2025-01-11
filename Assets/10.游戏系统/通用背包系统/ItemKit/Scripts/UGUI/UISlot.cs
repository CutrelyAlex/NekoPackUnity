using QFramework.Example;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace QFramework
{

    public class UISlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public Image Icon;
        public Text Count;

        public Slot Data { get; set; }

        private bool mDragging = false;

        public UISlot InitWithData(Slot data)
        {
            Data = data;
            if (Data.Count == 0)
            {
                Icon.Hide();
                Count.text = "";
            }
            else
            {
                Icon.Show();
                Count.text = Data.Count.ToString();
            }
            return this;
        }

        void SyncItemToMousePos()
        {
            var mousePos = Input.mousePosition;
            // 将屏幕坐标转换为本地坐标, 以便于计算拖拽的位置
            UGUIInventoryExample controller = FindAnyObjectByType<UGUIInventoryExample>();
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(controller.transform as RectTransform, mousePos, null,
                                                                        out Vector2 localPos))
            {
                Icon.LocalPosition2D(localPos); // 设置物品名字的位置
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (mDragging || Data.Count == 0) return;
            mDragging = true;

            UGUIInventoryExample controller = FindAnyObjectByType<UGUIInventoryExample>();
            Icon.Parent(controller);
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
                Icon.Parent(transform); // 恢复物品名字的父节点
                Icon.LocalPositionIdentity(); // 恢复物品名字的位置

                bool throwItem = true;

                // 检测是否在任意UISlot上
                UISlot[] uiSlots = transform.parent.GetComponentsInChildren<UISlot>();
                foreach (UISlot uiSlot in uiSlots)
                {
                    RectTransform recTransform = uiSlot.transform as RectTransform;
                    if (RectTransformUtility.RectangleContainsScreenPoint(recTransform, Input.mousePosition))
                    {
                        throwItem = false;
                        // 交换物品
                        if (Data.Count != 0)
                        {
                            var cachedItem = uiSlot.Data.Item;
                            var cachedCount = uiSlot.Data.Count;
                            uiSlot.Data.Item = Data.Item;
                            uiSlot.Data.Count = Data.Count;
                            Data.Item = cachedItem;
                            Data.Count = cachedCount;
                            FindAnyObjectByType<UGUIInventoryExample>().Refresh();
                        }

                        break;
                    };
                }

                if (throwItem)
                {
                    Data.Item = null;
                    Data.Count = 0;
                    FindAnyObjectByType<UGUIInventoryExample>().Refresh();
                }
            }
        }
    }
}

