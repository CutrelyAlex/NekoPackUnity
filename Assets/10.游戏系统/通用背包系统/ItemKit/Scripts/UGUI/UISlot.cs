using QFramework.Example;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace QFramework
{

    /// <summary>
    /// UISlot类用于处理UI背包系统中的物品槽。
    /// </summary>
    public class UISlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public Image Icon;
        public Text Count;

        public Slot Data { get; set; }

        private bool mDragging = false;

        /// <summary>
        /// 初始化物品槽数据。
        /// </summary>
        /// <param name="data">物品槽数据。</param>
        /// <returns>返回初始化后的UISlot实例。</returns>
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
                if (data.Item.GetIcon != null)
                {
                    Icon.sprite = data.Item.GetIcon;
                }
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
                            uiSlot.Data.Item = this.Data.Item;
                            uiSlot.Data.Count = this.Data.Count;
                            this.Data.Item = cachedItem;
                            this.Data.Count = cachedCount;
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

