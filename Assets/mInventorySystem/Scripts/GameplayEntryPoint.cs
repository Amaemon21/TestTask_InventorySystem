using Inventory;
using NaughtyAttributes;
using UnityEngine;

public class GameplayEntryPoint : MonoBehaviour
{
    [SerializeField, BoxGroup("GameplayConfig"), HorizontalLine] private GameplayConfig _gameplayConfig;
    
    [SerializeField, BoxGroup("Inventory"), HorizontalLine] private InventoryEntryPoint _inventoryEntryPoint;
    
    [SerializeField, BoxGroup("View"), HorizontalLine] private ScreenView _screenView;

    [SerializeField] private PlayerController _playerController;
    [SerializeField] private InteractController _interactController;
    
    private IInputSystem _inputSystem;
    private GameStatePlayerPrefsProvider _gameStateProvider;

    private void Awake()
    {
        _inputSystem = new InputSystem();
        _gameStateProvider = new GameStatePlayerPrefsProvider(_gameplayConfig);
        
        _gameStateProvider.Load();
        
        _screenView.Initialize(_inputSystem);
        
        _inventoryEntryPoint.Initialize(_gameStateProvider, _gameplayConfig, _screenView);
        
        _playerController.Initialize(_inputSystem);
        _interactController.Initialize(_inventoryEntryPoint.InventoryService);
    }

    private void OnDisable()
    {
        _inputSystem.Dispose();
    }
}