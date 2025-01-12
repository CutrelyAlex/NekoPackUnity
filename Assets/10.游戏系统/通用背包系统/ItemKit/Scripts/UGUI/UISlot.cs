using QFramework.Example;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace QFramework
{

    /// <summary>
    /// UISlot�����ڴ���UI����ϵͳ�е���Ʒ�ۡ�
    /// </summary>
    public class UISlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public Image Icon;
        public Text Count;

        public Slot Data { get; set; }

        private bool mDragging = false;

        /// <summary>
        /// ��ʼ����Ʒ�����ݡ�
        /// </summary>
        /// <param name="data">��Ʒ�����ݡ�</param>
        /// <returns>���س�ʼ�����UISlotʵ����</returns>
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
            // ����Ļ����ת��Ϊ��������, �Ա��ڼ�����ק��λ��
            UGUIInventoryExample controller = FindAnyObjectByType<UGUIInventoryExample>();
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(controller.transform as RectTransform, mousePos, null,
                                                                        out Vector2 localPos))
            {
                Icon.LocalPosition2D(localPos); // ������Ʒ���ֵ�λ��
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (mDragging || Data.Count == 0) return;
            mDragging = true;

            UGUIInventoryExample controller = FindAnyObjectByType<UGUIInventoryExample>();
            Icon.Parent(controller);
            SyncItemToMousePos(); // ͬ����Ʒ�����λ��
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (mDragging)
            {
                SyncItemToMousePos(); // ͬ����Ʒ�����λ��
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (mDragging)
            {
                Icon.Parent(transform); // �ָ���Ʒ���ֵĸ��ڵ�
                Icon.LocalPositionIdentity(); // �ָ���Ʒ���ֵ�λ��

                bool throwItem = true;

                // ����Ƿ�������UISlot��
                UISlot[] uiSlots = transform.parent.GetComponentsInChildren<UISlot>();
                foreach (UISlot uiSlot in uiSlots)
                {
                    RectTransform recTransform = uiSlot.transform as RectTransform;
                    if (RectTransformUtility.RectangleContainsScreenPoint(recTransform, Input.mousePosition))
                    {
                        throwItem = false;
                        // ������Ʒ
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

