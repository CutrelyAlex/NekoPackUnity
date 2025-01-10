using System.Collections.Generic;
using UnityEngine;
using static QFramework.Example.InventoryExample1;

namespace QFramework.Example
{
    public partial class InventoryExample1 : ViewController
    {
        private void OnGUI()
        {
            IMGUIHelper.SetDesignResolution(640, 360);

            foreach (var slot in ItemKit.Slots)
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
            if (GUILayout.Button("+"))
            {
                if(!ItemKit.AddItem("item_1"))
                {
                    Debug.Log("背包已满");
                }
            }

            if (GUILayout.Button("-"))
            {
                if(!ItemKit.SubItem("item_1"))
                {
                    Debug.Log("背包没有该物品");
                }
            }

            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("物品2");
            if (GUILayout.Button("+")) ItemKit.AddItem("item_2");
            if (GUILayout.Button("-")) ItemKit.SubItem("item_2");
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("物品3");
            if (GUILayout.Button("+")) ItemKit.AddItem("item_3");
            if (GUILayout.Button("-")) ItemKit.SubItem("item_3");
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("物品4");
            if (GUILayout.Button("+")) ItemKit.AddItem("item_4");
            if (GUILayout.Button("-")) ItemKit.SubItem("item_4");
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("物品5");
            if (GUILayout.Button("+")) ItemKit.AddItem("item_5");
            if (GUILayout.Button("-")) ItemKit.SubItem("item_5");
            GUILayout.EndHorizontal();
        }
    }
}
