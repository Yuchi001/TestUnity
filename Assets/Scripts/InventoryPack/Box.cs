using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ItemsPack.SO;
using Managers;
using Other;
using UnityEngine;

namespace InventoryPack
{
    public class Box : MonoBehaviour
    {
        [SerializeField] private List<BoxGridData> gridDataList;

        [SerializeField] private List<SoItem> debugItems; // TODO: REMOVE ITS DEBUG ONLY OPTION
        [SerializeField] private int itemsCount; // TODO: REMOVE ITS DEBUG ONLY OPTION

        protected readonly List<ItemSlot> _itemSlots = new();
        public int CurrentWeight => _itemSlots.Where(i => !i.IsEmpty()).Sum(i => i.ViewItem().ItemWeight);
        public Canvas CurrentCanvas { get; private set; }

        private IEnumerator Start()
        {
            yield return new WaitUntil(() => GameManager.Instance != null);
            
            CurrentCanvas = GetComponentInParent<Canvas>();
            
            var itemSlotPrefab = GameManager.Instance.GetPrefab<ItemSlot>(PrefabNames.ItemSlot);

            var index = 0;
            foreach (var gridData in gridDataList)
            {
                for (var i = 0; i < gridData.Capacity; i++)
                {
                    var spawnedSlot = Instantiate(itemSlotPrefab, gridData.Grid);
                    spawnedSlot.Setup(this, index);
                    if (i < itemsCount) spawnedSlot.TryAddNewItem(debugItems[Random.Range(0, debugItems.Count)]);
                    _itemSlots.Add(spawnedSlot);
                    index++;
                }
            }
            
            Init();
        }

        protected virtual void Init()
        {
        }

        public void SwitchItems(int current, int target)
        {
            var item1 = _itemSlots[current].ViewItem();
            var item2 = _itemSlots[target].ViewItem();

            AddItemAtSlot(target, item1);
            AddItemAtSlot(current, item2);
        }

        public virtual void AddItemAtSlot(int index, SoItem item)
        {
            _itemSlots[index].SetItem(item);
        }
        
        /// <summary>
        /// Checks if item can be added to box.
        /// </summary>
        /// <param name="item">New item to be added.</param>
        /// <returns>true if it is possible to add and item</returns>
        public virtual bool CanAdd(SoItem item)
        {
            return true;
        }
        
        /// <summary>
        /// Checks if item can be switched between boxes.
        /// </summary>
        /// <param name="currentItem">Item in this box to be switched.</param>
        /// <param name="newItem">Item in other box to be switched.</param>
        /// <returns>true if it is possible to switch items</returns>
        public virtual bool CanSwitch(SoItem currentItem, SoItem newItem)
        {
            return true;
        }

        public virtual void RemoveItemAtSlot(int index)
        {
            _itemSlots[index].SetItem(null);
        }

        public ItemSlot GetSlotAtIndex(int index)
        {
            return _itemSlots[index];
        }

        public void Open()
        {
            gameObject.SetActive(true);
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }

        [System.Serializable]
        public struct BoxGridData
        {
            [SerializeField] private int capacity;
            [SerializeField] private Transform grid;

            public int Capacity => capacity;
            public Transform Grid => grid;
        }
    }
}
