using System;
using System.Linq;
using ItemsPack.SO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace General
{
    public class ItemHoverManager : MonoBehaviour
    {
        [SerializeField] private Image itemImage;
        [SerializeField] private TextMeshProUGUI itemName;
        [SerializeField] private TextMeshProUGUI itemDescription;

        private SoItem _current = null;
        
        #region Singleton

        private static ItemHoverManager Instance { get; set; }

        private void Awake()
        {
            if (Instance != null && Instance != this) Destroy(gameObject);
            else Instance = this;

            Init();
        }

        #endregion

        private void Init()
        {
            itemName.text = "";
            itemDescription.text = "";
            itemImage.gameObject.SetActive(false);
            
            Instance._current = null;
        }

        public static void HoverItemStart(SoItem item)
        {
            if (!item)
            {
                if (Instance._current) Instance.Init(); 
                return;
            }

            Instance.itemImage.sprite = item.ItemSprite;
            Instance.itemName.text = item.ItemName.ToUpper();
            Instance.itemDescription.text = item.ItemDescription.ToLower();
            Instance.itemImage.gameObject.SetActive(true);

            Instance._current = item;
        }

        public static void HoverItemEnd(SoItem item)
        {
            if (Instance._current != null && Instance._current != item) return;
            
            Instance.itemName.text = "";
            Instance.itemDescription.text = "";
            Instance.itemImage.gameObject.SetActive(false);

            Instance._current = null;
        }
    }
}