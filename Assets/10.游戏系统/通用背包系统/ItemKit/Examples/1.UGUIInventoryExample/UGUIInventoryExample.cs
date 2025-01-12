using UnityEngine;

namespace QFramework.Example
{
    public partial class UGUIInventoryExample : ViewController
    {
        private void Start()
        {
            ItemKit.AddItemConfig(ConfigManager.Default.Iron);
            ItemKit.AddItemConfig(ConfigManager.Default.Powder);

            ItemKit.Slots[0].Item = ConfigManager.Default.Iron;
            ItemKit.Slots[0].Count = 1;
            ItemKit.Slots[1].Item = ConfigManager.Default.Powder;
            ItemKit.Slots[1].Count = 15;

            if (UISlotRoot == null)
            {
                Debug.LogError("UISlotRoot is not assigned.");
                return;
            }

            UISlot.Hide();
            Refresh();

            BtnAddItem1.onClick.AddListener(() =>
            {
                ItemKit.AddItem(ConfigManager.Default.Iron.GetKey, 1);
                Refresh();
            });
            BtnAddItem2.onClick.AddListener(() =>
            {
                ItemKit.AddItem(ConfigManager.Default.Powder.GetKey, 1);
                Refresh();
            });
            BtnAddItem3.onClick.AddListener(() =>
            {
                ItemKit.AddItem(ItemKit.Item3.Key, 1);
                Refresh();
            });
            BtnAddItem4.onClick.AddListener(() =>
            {
                ItemKit.AddItem(ItemKit.Item4.Key, 1);
                Refresh();
            });
            BtnAddItem5.onClick.AddListener(() =>
            {
                ItemKit.AddItem(ItemKit.Item5.Key, 1);
                Refresh();
            });

            BtnSubItem1.onClick.AddListener(() =>
            {
                ItemKit.SubItem(ConfigManager.Default.Iron.GetKey, 1);
                Refresh();
            });
            BtnSubItem2.onClick.AddListener(() =>
            {
                ItemKit.SubItem(ConfigManager.Default.Powder.GetKey, 1);
                Refresh();
            });
            BtnSubItem3.onClick.AddListener(() =>
            {
                ItemKit.SubItem(ItemKit.Item3.Key, 1);
                Refresh();
            });
            BtnSubItem4.onClick.AddListener(() =>
            {
                ItemKit.SubItem(ItemKit.Item4.Key, 1);
                Refresh();
            });
            BtnSubItem5.onClick.AddListener(() =>
            {
                ItemKit.SubItem(ItemKit.Item5.Key, 1);
                Refresh();
            });
        }

        public void Refresh()
        {
            UISlotRoot.DestroyChildren();
            foreach (var slot in ItemKit.Slots)
            {
                UISlot uiSlotInstance = UISlot.InstantiateWithParent(UISlotRoot)
                                              .InitWithData(slot)
                                              .Show();
            }
        }

    }
}
