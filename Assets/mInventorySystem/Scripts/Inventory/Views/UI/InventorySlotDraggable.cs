using Inventory;
using UnityEngine;
using UnityEngine.EventSystems;

namespace mInventorySystem.Scripts.Inventory.Views.UI
{
    public class InventorySlotDraggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private InventorySlotView _inventorySlotView;
        [SerializeField] private DraggableSlot _draggableSlot;
        [SerializeField] private DropArea _dropArea;

        public void OnBeginDrag(PointerEventData eventData)
        {
            _dropArea.Show();
            
            if (_inventorySlotView.ItemConfig != null)
            {
                _draggableSlot.gameObject.SetActive(true);

                _draggableSlot.Icon.sprite = _inventorySlotView.ItemConfig.ItemIcon;
                
                _draggableSlot.SlotCoordA = _inventorySlotView.SlotCoords;
            }
        }
        
        public void OnDrag(PointerEventData eventData)
        {
            _draggableSlot.transform.position = Input.mousePosition;
        }
        
        public void OnEndDrag(PointerEventData eventData)
        {
            if (eventData.pointerCurrentRaycast.gameObject != null)
            {
                if (eventData.pointerCurrentRaycast.gameObject.TryGetComponent(out InventorySlotView inventorySlotView))
                {
                    _draggableSlot.SlotCoordB = inventorySlotView.SlotCoords;
                    
                    _inventorySlotView.InventoryService.SwithSlots(_draggableSlot.SlotCoordA, _draggableSlot.SlotCoordB);
                }
                else if (eventData.pointerCurrentRaycast.gameObject.TryGetComponent(out DropArea dropArea))
                {
                    Vector2Int slotCoords = _inventorySlotView.SlotCoords;
                    InventoryItemConfig config = _inventorySlotView.ItemConfig;
                    
                    _inventorySlotView.InventoryService.RemoveItemsToInventory(slotCoords, config);
                }
            }
            
            _draggableSlot.gameObject.SetActive(false);
            _dropArea.Hide();
        }
    }
}