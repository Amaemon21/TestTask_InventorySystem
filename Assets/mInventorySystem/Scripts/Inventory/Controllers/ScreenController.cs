namespace Inventory
{
    public class ScreenController
    {
        private readonly InventoryService _inventoryService;
        private readonly ScreenView _view;

        private InventoryGridController _currentInventoryGridController;
        
        public ScreenController(InventoryService inventoryService, ScreenView view)
        {
            _inventoryService = inventoryService;
            _view = view;
        }

        public void OpenInventory()
        {
            IReadOnlyInventoryGrid inventory = _inventoryService.GetInventory();
            InventoryView inventoryView = _view.InventoryView;
            ItemInfoView itemInfoView = _view.ItemInfoView;
            
            _currentInventoryGridController = new InventoryGridController(inventory, inventoryView, itemInfoView, _inventoryService);
        }
    }
}