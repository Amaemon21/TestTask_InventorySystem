using Inventory;
using UnityEngine;

public class InteractController : MonoBehaviour
{
    [SerializeField] private InteractProperty _interactProperty;
    
    private IInteractHandler _interactHandler;
    
    public void Initialize(InventoryService inventoryService)
    {
        _interactHandler = new InteractHandler(inventoryService, _interactProperty);
    }

    private void Update()
    {
        _interactHandler.Interactable();

        if (Input.GetKeyDown(KeyCode.E))
        {
            _interactHandler.PickupItem();
        }
    }
}