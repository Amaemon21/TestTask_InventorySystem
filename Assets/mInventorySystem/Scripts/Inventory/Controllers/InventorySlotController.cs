namespace Inventory
{
    public class InventorySlotController
    {
        private readonly InventorySlotView _slotView;
        private readonly ItemInfoView _infoView;

        public InventorySlotController(string ownerId, IReadOnlyInventorySlot slot, InventorySlotView slotView, ItemInfoView infoView, InventoryService inventoryService)
        {
            _slotView = slotView;
            _infoView = infoView;
            _slotView.Initialize(ownerId, inventoryService, slot.SlotCoords);
            
            slot.ItemChanged += SlotOnItemChanged;
            slot.ItemAmountChanged += SlotOnItemAmountChanged;

            _slotView.OnPointerEnterChanged += OnPointerEnter;
            _slotView.OnPointerExitChanged += OnPointerExit;

            SlotOnItemChanged(slot.GetItemConfig());
            SlotOnItemAmountChanged(slot.Amount);
        }
        
        private void SlotOnItemChanged(InventoryItemConfig newConfig)
        {
            _slotView.RefreshItem(newConfig);
        }
        
        private void SlotOnItemAmountChanged(int newAmount)
        {
            _slotView.RefreshAmount(newAmount);
        }
        
        private void OnPointerEnter(InventoryItemConfig config)
        {
            if (config == null)
            {                
                _infoView.Hide();
                return;
            }
            
            _infoView.Show(config.ItemIcon, config.ItemDescription);
        }
        
        private void OnPointerExit()
        {
            _infoView.Hide();
        }
    }
}