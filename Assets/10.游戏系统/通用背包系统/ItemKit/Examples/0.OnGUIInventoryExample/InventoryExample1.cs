using UnityEngine;

namespace QFramework.Example
{
    public partial class InventoryExample1 : ViewController
    {
        private void Awake()
        {
            ItemKit.AddItemConfig(ConfigManager.Iron.Value);
            ItemKit.AddItemConfig(ConfigManager.Powder.Value);

            ItemKit.CreateSlot(ConfigManager.Iron.Value, 1);
            ItemKit.CreateSlot(ConfigManager.Powder.Value, 15);
        }
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
                    GUILayout.Label($"格子: {slot.Item.GetName} 数量:{slot.Count}");
                }
                GUILayout.EndHorizontal();
            }

            GUILayout.BeginHorizontal();
            GUILayout.Label("物品1");
            if (GUILayout.Button("+"))
            {
                if (!ItemKit.AddItem(ConfigManager.Iron.Value.GetKey))
                {
                    Debug.Log("背包已满");
                }
            }

            if (GUILayout.Button("-"))
            {
                if (!ItemKit.SubItem(ConfigManager.Iron.Value.GetKey))
                {
                    Debug.Log("背包没有该物品");
                }
            }

            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("物品2");
            if (GUILayout.Button("+")) ItemKit.AddItem(ConfigManager.Powder.Value.GetKey);
            if (GUILayout.Button("-")) ItemKit.SubItem(ConfigManager.Powder.Value.GetKey);
            GUILayout.EndHorizontal();
        }
    }
}
