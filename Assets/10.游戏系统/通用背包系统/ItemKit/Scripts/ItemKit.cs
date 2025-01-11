using System.Collections.Generic;

namespace QFramework
{

    /// <summary>
    /// ItemKit管理物品的增删查改
    /// 提供以下API:
    /// - FindSlotByKey: 根据物品Key查找对应的Slot
    /// - FindEmptySlot: 查找一个空的Slot
    /// - FindAddableSlot: 查找一个可以添加物品的Slot
    /// - AddItem: 添加物品到背包
    /// - SubItem: 从背包中减少物品
    /// </summary>
    public class ItemKit
    {
        public static Item Item1 = new Item("item_1", "物品1");
        public static Item Item2 = new Item("item_2", "物品2");
        public static Item Item3 = new Item("item_3", "物品3");
        public static Item Item4 = new Item("item_4", "物品4");
        public static Item Item5 = new Item("item_5", "物品5");

        public static List<Slot> Slots = new List<Slot>()
            {
                new Slot(ItemKit.Item1, 1),
                new Slot(ItemKit.Item2, 5),
                new Slot(ItemKit.Item3, 1),
                new Slot(ItemKit.Item4, 1),
            };

        public static Dictionary<string, Item> ItemByKey = new Dictionary<string, Item>
            {
                {ItemKit.Item1.Key, ItemKit.Item1},
                {ItemKit.Item2.Key, ItemKit.Item2},
                {ItemKit.Item3.Key, ItemKit.Item3},
                {ItemKit.Item4.Key, ItemKit.Item4},
                {ItemKit.Item5.Key, ItemKit.Item5},
            };

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
            var slot = FindSlotByKey(itemKey);
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
            var slot = FindAddableSlot(itemKey);
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
