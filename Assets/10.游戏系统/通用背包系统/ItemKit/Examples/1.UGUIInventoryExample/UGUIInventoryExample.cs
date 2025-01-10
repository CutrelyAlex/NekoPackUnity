using UnityEngine;
using QFramework;

namespace QFramework.Example
{
	public partial class UGUIInventoryExample : ViewController
	{
        private void Start()
        {
            if (UISlotRoot == null)
            {
                Debug.LogError("UISlotRoot is not assigned.");
                return;
            }

            UISlot.Hide();
            Refresh();

            BtnAddItem1.onClick.AddListener(() =>
            {
                ItemKit.AddItem(ItemKit.Item1.Key, 1);
                Refresh();
            });
            BtnAddItem2.onClick.AddListener(() =>
            {
                ItemKit.AddItem(ItemKit.Item2.Key, 1);
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
                ItemKit.SubItem(ItemKit.Item1.Key, 1);
                Refresh();
            });
            BtnSubItem2.onClick.AddListener(() =>
            {
                ItemKit.SubItem(ItemKit.Item2.Key, 1);
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

        void Refresh()
        {
            UISlotRoot.DestroyChildren();
            foreach (var slot in ItemKit.Slots)
            {
                UISlot uiSlotInstance = UISlot.InstantiateWithParent(UISlotRoot);
                if (uiSlotInstance == null)
                {
                    Debug.LogError("UISlot.InstantiateWithParent(UISlotRoot) return null.");
                    return;
                }
                uiSlotInstance.Self(self =>
                {
                    if (slot.Count == 0)
                    {
                        self.Name.text = "Пе";
                        self.Count.text = "";
                    }
                    else
                    {
                        self.Name.text = slot.Item.Name;
                        self.Count.text = slot.Count.ToString();

                    }
                }).Show();
            }
        }

    }
}
