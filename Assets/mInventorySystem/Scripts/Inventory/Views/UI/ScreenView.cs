using UnityEngine;

namespace Inventory
{
    public class ScreenView : MonoBehaviour
    {
        [SerializeField] private InventoryView _inventoryView;
        [SerializeField] private ItemInfoView _infoView;
        
        private IInputSystem _inputSystem;
        
        public InventoryView InventoryView  => _inventoryView;
        public ItemInfoView ItemInfoView  => _infoView;
        
        public bool IsOpenInventory{ get; private set; }

        public void Initialize(IInputSystem inputSystem)
        {
            _inputSystem = inputSystem;
            
            _inventoryView.gameObject.SetActive(false);
        }
        
        public void ShowInventory()
        {
            IsOpenInventory = true;
            
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = IsOpenInventory;
            
            _inputSystem.DisablePlayerMap();
            
            _inventoryView.gameObject.SetActive(IsOpenInventory);
        }

        public void HideInventory()
        {
            IsOpenInventory = false;
            
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = IsOpenInventory;
            
            _inputSystem.EnablePlayerMap();
            
            _inventoryView.gameObject.SetActive(IsOpenInventory);
        }
    }
}