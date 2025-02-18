﻿using ItemsPack.SO;
using UnityEngine;
using UnityEngine.UI;

namespace InventoryPack
{
    public class DragItemSlot : MonoBehaviour
    {
        private Image _itemImage;
        private ItemSlot _itemSlot;
        private Box _box;
        private Camera _cameraMain;

        private SoItem Item => _itemSlot.ViewItem();
        private int Index => _itemSlot.Index;
        
        public void Setup(ItemSlot itemSlot, Box box)
        {
            gameObject.SetActive(false);
            
            _itemSlot = itemSlot;
            _itemImage = GetComponent<Image>();
            _box = box;
            _cameraMain = Camera.main;
            
            transform.position = _itemSlot.transform.position;
        }

        public void BeginDrag()
        {
            _itemImage.sprite = _itemSlot.ViewItem().ItemSprite;
            gameObject.SetActive(true);

            transform.SetParent(_box.CurrentCanvas.transform);
            _box.transform.SetSiblingIndex(0);
        }

        public void EndDrag(int targetIndex, Box targetBox)
        {
            EndDrag();

            if (targetBox == _box)
            {
                if (!_box.GetSlotAtIndex(targetIndex).IsEmpty())
                {
                    _box.SwitchItems(Index, targetIndex);
                    return;
                }
                
                _box.AddItemAtSlot(targetIndex, Item);
                _box.RemoveItemAtSlot(Index);
                return;
            }

            if (targetBox.GetSlotAtIndex(targetIndex).IsEmpty())
            {
                if (!targetBox.CanAdd(Item)) return;
                
                targetBox.AddItemAtSlot(targetIndex, Item);
                _box.RemoveItemAtSlot(Index);
                return;
            }

            var item1 = Instantiate(Item);
            var item2 = targetBox.GetSlotAtIndex(targetIndex).ViewItem();

            if (!_box.CanSwitch(item1, item2) 
                || !targetBox.CanSwitch(item2, item1)) return;
            
            _box.AddItemAtSlot(Index, item2);
            targetBox.AddItemAtSlot(targetIndex, item1);
        }
        
        public void EndDrag()
        {
            var itemSlotTransform = _itemSlot.transform;
            var t = transform;
            t.SetParent(itemSlotTransform);
            
            t.position = itemSlotTransform.position;
            gameObject.SetActive(false);
        }

        private void Update()
        {
            var mousePosition = _cameraMain.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;
            transform.position = mousePosition;
        }
    }
}