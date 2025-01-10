using System.Collections.Generic;
using UnityEngine;
using static QFramework.Example.InventoryExample1;

namespace QFramework.Example
{
    public partial class InventoryExample1 : ViewController
    {
        public class Slot
        {
            public Item Item;
            public int Count;

            public Slot(Item item, int count)
            {
                Item = item;
                Count = count;
            }
        }

        public class Item
        {
            public string Key;
            public string Name;

            public Item(string key, string name)
            {
                Key = key;
                Name = name;
            }
        }

        public Item Item1 = new Item("item_1", "物品1");
        public Item Item2 = new Item("item_2", "物品2");
        public Item Item3 = new Item("item_3", "物品3");
        public Item Item4 = new Item("item_4", "物品4");
        public Item Item5 = new Item("item_5", "物品5");

        private List<Slot> mSlots = null;

        private Dictionary<string, Item> mItemByKey = null;

        private void Awake()
        {
            mSlots = new List<Slot>
            {
                new Slot(Item1, 1),
                new Slot(Item2, 10),
                new Slot(Item3, 1),
                new Slot(Item4, 1),
            };

            mItemByKey = new Dictionary<string, Item>
            {
                {Item1.Key, Item1},
                {Item2.Key, Item2},
                {Item3.Key, Item3},
                {Item4.Key, Item4},
                {Item5.Key, Item5},
            };
        }

        private void OnGUI()
        {
            IMGUIHelper.SetDesignResolution(640, 360);

            foreach (var slot in mSlots)
            {
                GUILayout.BeginHorizontal();
                if (slot.Count == 0)
                {
                    GUILayout.Label($"格子: 空");
                }
                else
                {
                    GUILayout.Label($"格子: {slot.Item.Name} 数量:{slot.Count}");
                }
                GUILayout.EndHorizontal();
            }

            GUILayout.BeginHorizontal();
            GUILayout.Label("物品1");
            if (GUILayout.Button("+")) AddItem("item_1"); 
            if (GUILayout.Button("-")) SubItem("item_1");
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("物品2");
            if (GUILayout.Button("+")) AddItem("item_2"); 
            if (GUILayout.Button("-")) SubItem("item_2");
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("物品3");
            if (GUILayout.Button("+")) AddItem("item_3"); 
            if (GUILayout.Button("-")) SubItem("item_3");
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("物品4");
            if (GUILayout.Button("+")) AddItem("item_4"); 
            if (GUILayout.Button("-")) SubItem("item_4");
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("物品5");
            if (GUILayout.Button("+")) AddItem("item_5"); 
            if (GUILayout.Button("-")) SubItem("item_5");
            GUILayout.EndHorizontal();
        }
        Slot FindSlotByKey(string itemKey)
        {
            return mSlots.Find(s => s.Item != null && s.Item.Key == itemKey && s.Count != 0);
        }

        Slot FindEmptySlot()
        {
            return mSlots.Find(s => s.Count == 0);
        }

        Slot FindAddableSlot(string itemKey)
        {
            var slot = FindSlotByKey(itemKey);
            if (slot == null)
            {
                slot = FindEmptySlot();
                if (slot != null)
                {
                    slot.Item = mItemByKey[itemKey];
                }
            }
            return slot;
        }

        bool AddItem(string itemKey, int addCount = 1)
        {
            var slot = FindAddableSlot(itemKey);
            if (slot != null)
            {
                slot.Count += addCount;
            }
            else
            {
                Debug.Log("背包已满");
                return false;
            }
            return true;
        }

        bool SubItem(string itemKey, int addCount = 1)
        {
            var slot = FindSlotByKey(itemKey);
            if (slot != null)
            {
                slot.Count -= addCount;
            }
            else
            {
                Debug.Log("背包没有该物品");
                return false;
            }
            return true;
        }
    }
}
