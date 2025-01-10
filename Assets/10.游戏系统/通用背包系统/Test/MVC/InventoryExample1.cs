using System.Collections.Generic;
using UnityEngine;

namespace QFramework.Example
{
    public partial class InventoryExample1 : ViewController
    {
        public class Slot
        {
            public Item Item;
            public int Count;
        }

        public class Item
        {
            public string Name;
            public string Key;
        }

        private List<Slot> Slots = new List<Slot>
        {
            new Slot
            {
                Item = new Item()
                {
                    Name = "物品1",
                    Key = "item_1",
                },
                Count = 1,
            },
            new Slot
            {
                Item = new Item()
                {
                    Name = "物品2",
                    Key = "item_2",
                },
                Count = 1,
            },
        };

        private void OnGUI()
        {
            IMGUIHelper.SetDesignResolution(960, 540);

            for (int i = 0; i < 4; i++)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label($"格子{i} : 物品{i}");
                GUILayout.EndHorizontal();
            }
        }

    }
