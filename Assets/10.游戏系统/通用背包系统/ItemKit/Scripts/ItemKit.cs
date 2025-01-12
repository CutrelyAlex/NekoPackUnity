using System.Collections.Generic;
using UnityEngine;

namespace QFramework
{
    public class ItemKit
    {
        // 所有的Slot
        public static List<Slot> Slots = new List<Slot>();
        // 所有的物品配置
        public static Dictionary<string, IItem> ItemByKey = new Dictionary<string, IItem>();


        /// <summary>
        /// 从Resources文件夹中加载物品配置
        /// </summary>
        /// <param name="itemName">物品名称</param>
        public static void LoadItemConfigByResources(string itemName)
        {
            AddItemConfig(Resources.Load<ItemConfig>(itemName));
        }

        /// <summary>
        /// 添加物品配置
        /// </summary>
        /// <param name="itemConfig">物品配置</param>
        public static void AddItemConfig(IItem itemConfig)
        {
            ItemByKey.Add(itemConfig.GetKey, itemConfig);
        }

        /// <summary>
        /// 创建一个新的Slot
        /// </summary>
        /// <param name="item">物品</param>
        /// <param name="count">物品数量</param>
        public static void CreateSlot(IItem item, int count)
        {
            Slots.Add(new Slot(item, count));
        }


        /// <summary>
        /// 根据物品Key查找对应的Slot
        /// </summary>
        /// <param name="itemKey">物品的Key</param>
        /// <returns>找到的Slot，如果没有找到则返回null</returns>
        public static Slot FindSlotByKey(string itemKey)
        {
            return ItemKit.Slots.Find(s => s.Item != null && s.Item.GetKey == itemKey && s.Count != 0);
        }

        /// <summary>
        /// 查找一个空的Slot
        /// </summary>
        /// <returns>找到的空Slot，如果没有找到则返回null</returns>
        public static Slot FindEmptySlot()
        {
            return ItemKit.Slots.Find(s => s.Count == 0);
        }

        /// <summary>
        /// 查找一个可以添加物品的Slot
        /// </summary>
        /// <param name="itemKey">物品的Key</param>
        /// <returns>找到的Slot，如果没有找到则返回null</returns>
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
        /// 添加物品到背包
        /// </summary>
        /// <param name="itemKey">物品的Key</param>
        /// <param name="addCount">添加的数量</param>
        /// <returns>添加成功返回true，失败返回false</returns>
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
        /// 从背包中减少物品
        /// </summary>
        /// <param name="itemKey">物品的Key</param>
        /// <param name="subCount">减少的数量</param>
        /// <returns>减少成功返回true，失败返回false</returns>
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
