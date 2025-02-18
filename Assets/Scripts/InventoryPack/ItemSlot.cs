using General;
using ItemsPack.SO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace InventoryPack
{
    public class ItemSlot : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
    {
        //private Box _box;
        private SoItem _current = null;
        private DragItemSlot _dragItemSlot;
        private Image _itemImage;
        private Color _dragColor;
        
        public int Index { get; private set; }

        public void Setup(Box box, int index)
        {
            //_box = box;
            _dragItemSlot = transform.GetComponentInChildren<DragItemSlot>();
            _dragItemSlot.Setup(this, box);
            _itemImage = GetComponent<Image>();
            _dragColor = new Color(1, 1, 1, 0.3f);
            Index = index;
            _itemImage.color = Color.clear;
        }

        public bool TryAddNewItem(SoItem item)
        {
            if (_current != null || !item) return false;

            _current = Instantiate(item);
            _itemImage.sprite = _current.ItemSprite;
            _itemImage.color = Color.white;
            return true;
        }

        public void SetItem(SoItem item)
        {
            _current = item ? Instantiate(item) : null;
            _itemImage.sprite = _current ? _current.ItemSprite : null;
            _itemImage.color = item ? Color.white : Color.clear;
        }

        public SoItem ViewItem()
        {
            return _current;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (_current == null) return;
            
            _dragItemSlot.BeginDrag();
            _itemImage.color = _dragColor;
        }

        public bool IsEmpty()
        {
            return _current == null;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (IsEmpty()) return;
            
            var rayCastHitGO = eventData.pointerCurrentRaycast.gameObject;
            if (rayCastHitGO && rayCastHitGO.TryGetComponent(out ItemSlot itemSlot))
                _dragItemSlot.EndDrag(itemSlot.Index, itemSlot.GetComponentInParent<Box>());
            else _dragItemSlot.EndDrag();
            
            _itemImage.color = IsEmpty() ? Color.clear : Color.white;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            ItemHoverManager.HoverItemStart(_current);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            ItemHoverManager.HoverItemEnd(_current);
        }
    }
}