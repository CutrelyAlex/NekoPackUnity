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
            UGUIInventoryExample controller = FindAnyObjectByType<UGUIInventoryExample>();
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(controller.transform as RectTransform, mousePos, null,
                                                                        out Vector2 localPos))
            {
                Name.LocalPosition2D(localPos); // ������Ʒ���ֵ�λ��
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (mDragging || Data.Count == 0) return;
            mDragging = true;

            UGUIInventoryExample controller= FindAnyObjectByType<UGUIInventoryExample>();
            Name.Parent(controller);
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
                Name.Parent(transform); // �ָ���Ʒ���ֵĸ��ڵ�
                Name.LocalPositionIdentity(); // �ָ���Ʒ���ֵ�λ��

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
                        if(Data.Count != 0)
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

