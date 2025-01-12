using UnityEngine;

namespace QFramework.Example
{
    public partial class UGUIInventoryExample : ViewController
    {
        private void Start()
        {
            ItemKit.AddItemConfig(ConfigManager.Iron.Value);
            ItemKit.AddItemConfig(ConfigManager.Powder.Value);

            ItemKit.CreateSlot(ConfigManager.Iron.Value, 1);
            ItemKit.CreateSlot(ConfigManager.Powder.Value, 15);

            UISlot.Hide();
            Refresh();

            BtnAddItem1.onClick.AddListener(() =>
            {
                ItemKit.AddItem(ConfigManager.Iron.Value.GetKey, 1);
                Refresh();
            });
            BtnAddItem2.onClick.AddListener(() =>
            {
                ItemKit.AddItem(ConfigManager.Powder.Value.GetKey, 1);
                Refresh();
            });

            BtnSubItem1.onClick.AddListener(() =>
            {
                ItemKit.SubItem(ConfigManager.Iron.Value.GetKey, 1);
                Refresh();
            });
            BtnSubItem2.onClick.AddListener(() =>
            {
                ItemKit.SubItem(ConfigManager.Powder.Value.GetKey, 1);
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
