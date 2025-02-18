using UnityEngine;

namespace ItemsPack.SO
{
    [CreateAssetMenu(menuName = "Custom/Item", fileName = "new Item")]
    public class SoItem : ScriptableObject
    {
        [SerializeField] private string itemName;
        [SerializeField, TextArea(10,10)] private string itemDescription;
        [SerializeField] private Sprite itemSprite;
        [SerializeField] private int itemWeight;

        public string ItemName => itemName;
        public string ItemDescription => itemDescription;
        public Sprite ItemSprite => itemSprite;
        public int ItemWeight => itemWeight;
    }
}