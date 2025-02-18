using System.Linq;
using ItemsPack.SO;
using TMPro;
using UnityEngine;

namespace InventoryPack
{
    public class Inventory : Box
    {
        [SerializeField] private int maxWeight;
        [SerializeField] private TextMeshProUGUI weightTextField;

        protected override void Init()
        {
            weightTextField.text = $"0/{maxWeight}";
        }

        public override void AddItemAtSlot(int index, SoItem item)
        {
            base.AddItemAtSlot(index, item);
            weightTextField.text = $"{CurrentWeight}/{maxWeight}";
        }

        public override void RemoveItemAtSlot(int index)
        {
            base.RemoveItemAtSlot(index);
            weightTextField.text = $"{CurrentWeight}/{maxWeight}";
        }

        public override bool CanAdd(SoItem item)
        {
            return _itemSlots.Where(i => !i.IsEmpty()).Sum(i => i.ViewItem().ItemWeight) + item.ItemWeight <= maxWeight;
        }
        
        public override bool CanSwitch(SoItem currentItem, SoItem newItem)
        {
            return CurrentWeight - currentItem.ItemWeight + newItem.ItemWeight <= maxWeight;
        }
    }
}