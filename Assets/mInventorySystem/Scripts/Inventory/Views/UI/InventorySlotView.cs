using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Inventory
{
    [RequireComponent(typeof(DubleClickHandler))]
    public class InventorySlotView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private TMP_Text _textTitle;
        [SerializeField] private TMP_Text _textAmount;
        [SerializeField] private Image _imageIcon;
        
        private DubleClickHandler _dubleClickHandler;
        
        public InventoryService InventoryService { get; private set; }
        public Vector2Int SlotCoords { get; private set; }
        public InventoryItemConfig ItemConfig { get; private set; }
        public string OwnerId { get; private set; }
        public int Amount { get; private set; }
        
        public event Action<InventoryItemConfig> OnPointerEnterChanged;
        public event Action OnPointerExitChanged;

        private void Awake()
        {
            _dubleClickHandler = GetComponent<DubleClickHandler>();
        }

        private void OnEnable()
        {
            _dubleClickHandler.OnDubleClick += UseItem;
        }

        private void OnDisable()
        {
            _dubleClickHandler.OnDubleClick -= UseItem;
        }

        public void Initialize(string ownerId, InventoryService inventoryService, Vector2Int slotCoords)
        {
            OwnerId = ownerId;
            InventoryService = inventoryService;
            SlotCoords = slotCoords;
        }

        public void RefreshItem(InventoryItemConfig config)
        {
            if (config == null)
            {
                ItemConfig = null;
                
                _textTitle.text = String.Empty;
                    
                _imageIcon.enabled = false;
                _imageIcon.sprite = null;
            }
            else
            {
                ItemConfig = config;
                
                _textTitle.text = config.ItemName;
                
                _imageIcon.enabled = true;
                _imageIcon.sprite = config.ItemIcon;
            }
        }
        
        public void RefreshAmount(int amount)
        {
            Amount = amount;
            
            _textAmount.text = amount <= 1 ? String.Empty : amount.ToString();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            OnPointerEnterChanged?.Invoke(ItemConfig);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            OnPointerExitChanged?.Invoke();
        }

        private void UseItem()
        {
            InventoryService.UseItem(SlotCoords, ItemConfig);
        }
    }
}