using Inventory;
using NaughtyAttributes;
using UnityEngine;

public class InventoryEntryPoint : MonoBehaviour
{
    [SerializeField, BoxGroup("InventoryGridConfig"), HorizontalLine] private InventoryGridConfig _mainInventoryGridConfig;

    private ScreenView _screenView;
    
    private InventoryService _inventoryService;
    private UsableEntityFactory _usableEntityFactory;
    private ScreenController _screenController;
    
    public InventoryService InventoryService => _inventoryService;
    
    public void Initialize(GameStatePlayerPrefsProvider gameStateProvider, GameplayConfig gameplayConfig, ScreenView screenView)
    {
        _screenView = screenView;
        
        GameStateData gameState = gameStateProvider.GameState;

        _usableEntityFactory = new UsableEntityFactory();
        
        _inventoryService = new InventoryService(gameStateProvider, _usableEntityFactory);

        foreach (InventoryGridData inventoryData in gameState.Inventories)
        {
            _inventoryService.RegisterInventory(inventoryData, gameplayConfig.InventoryItemsDatabaseConfig);
        }

        _screenController = new ScreenController(_inventoryService, _screenView);
        _screenController.OpenInventory();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (_screenView.IsOpenInventory)
                _screenView.HideInventory();
            else
                _screenView.ShowInventory();
        }
    }
}