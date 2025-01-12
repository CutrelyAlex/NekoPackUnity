using System.Collections.Generic;
using UnityEngine;

namespace QFramework
{
    public class ItemKit
    {
        // ���е�Slot
        public static List<Slot> Slots = new List<Slot>();
        // ���е���Ʒ����
        public static Dictionary<string, IItem> ItemByKey = new Dictionary<string, IItem>();


        /// <summary>
        /// ��Resources�ļ����м�����Ʒ����
        /// </summary>
        /// <param name="itemName">��Ʒ����</param>
        public static void LoadItemConfigByResources(string itemName)
        {
            AddItemConfig(Resources.Load<ItemConfig>(itemName));
        }

        /// <summary>
        /// �����Ʒ����
        /// </summary>
        /// <param name="itemConfig">��Ʒ����</param>
        public static void AddItemConfig(IItem itemConfig)
        {
            ItemByKey.Add(itemConfig.GetKey, itemConfig);
        }

        /// <summary>
        /// ����һ���µ�Slot
        /// </summary>
        /// <param name="item">��Ʒ</param>
        /// <param name="count">��Ʒ����</param>
        public static void CreateSlot(IItem item, int count)
        {
            Slots.Add(new Slot(item, count));
        }


        /// <summary>
        /// ������ƷKey���Ҷ�Ӧ��Slot
        /// </summary>
        /// <param name="itemKey">��Ʒ��Key</param>
        /// <returns>�ҵ���Slot�����û���ҵ��򷵻�null</returns>
        public static Slot FindSlotByKey(string itemKey)
        {
            return ItemKit.Slots.Find(s => s.Item != null && s.Item.GetKey == itemKey && s.Count != 0);
        }

        /// <summary>
        /// ����һ���յ�Slot
        /// </summary>
        /// <returns>�ҵ��Ŀ�Slot�����û���ҵ��򷵻�null</returns>
        public static Slot FindEmptySlot()
        {
            return ItemKit.Slots.Find(s => s.Count == 0);
        }

        /// <summary>
        /// ����һ�����������Ʒ��Slot
        /// </summary>
        /// <param name="itemKey">��Ʒ��Key</param>
        /// <returns>�ҵ���Slot�����û���ҵ��򷵻�null</returns>
        public static Slot FindAddableSlot(string itemKey)
        {
            Slot slot = FindSlotByKey(itemKey);
            if (slot == null)
            {
                slot = FindEmptySlot();
                if (slot != null)
                {
                    slot.Item = ItemKit.ItemByKey[itemKey];
                }
            }
            return slot;
        }

        /// <summary>
        /// �����Ʒ������
        /// </summary>
        /// <param name="itemKey">��Ʒ��Key</param>
        /// <param name="addCount">��ӵ�����</param>
        /// <returns>��ӳɹ�����true��ʧ�ܷ���false</returns>
        public static bool AddItem(string itemKey, int addCount = 1)
        {
            Slot slot = FindAddableSlot(itemKey);
            if (slot != null)
            {
                slot.Count += addCount;
            }
            else
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// �ӱ����м�����Ʒ
        /// </summary>
        /// <param name="itemKey">��Ʒ��Key</param>
        /// <param name="subCount">���ٵ�����</param>
        /// <returns>���ٳɹ�����true��ʧ�ܷ���false</returns>
        public static bool SubItem(string itemKey, int subCount = 1)
        {
            var slot = FindSlotByKey(itemKey);
            if (slot != null && slot.Count - subCount >= 0)
            {
                slot.Count -= subCount;
                return true;
            }
            return false;
        }
    }
}
