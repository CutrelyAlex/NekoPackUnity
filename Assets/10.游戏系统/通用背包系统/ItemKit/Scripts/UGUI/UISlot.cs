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
                Name.text = "��";
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
            // ����Ļ����ת��Ϊ��������, �Ա��ڼ�����ק��λ��
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(transform as RectTransform, mousePos, null,
                                                                        out Vector2 localPos))
            {
                Name.LocalPosition2D(localPos); // ������Ʒ���ֵ�λ��
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (mDragging) return;
            mDragging = true;
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
                Name.LocalPositionIdentity(); // �ָ���Ʒ���ֵ�λ��

                bool throwItem = true;

                // ����Ƿ�������UISlot��
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

