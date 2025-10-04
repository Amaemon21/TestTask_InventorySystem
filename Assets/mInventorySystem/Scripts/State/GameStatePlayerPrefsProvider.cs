using System.Collections.Generic;
using System.Linq;
using Inventory;
using UnityEngine;

public class GameStatePlayerPrefsProvider : IGameStateProvider
{
    private readonly GameplayConfig _gameplayConfig;
    private const string KEY = "GAME STATE";
        
    public GameStateData GameState { get; private set; }

    public GameStatePlayerPrefsProvider(GameplayConfig gameplayConfig)
    {
        _gameplayConfig = gameplayConfig;
    }
        
    public void Save()
    {
        string json = JsonUtility.ToJson(GameState);
        PlayerPrefs.SetString(KEY, json);
    }

    public void Load()
    {
        if (PlayerPrefs.HasKey(KEY))
        {
            string json = PlayerPrefs.GetString(KEY);
            GameState = JsonUtility.FromJson<GameStateData>(json);
        }
        else
        {
            GameState = InitFromSettings();
            Save();
        }
    }

    private GameStateData InitFromSettings()
    {
        return new GameStateData
        {
            Inventories = _gameplayConfig.InventoryGrids
                .Select(CreateInventoryFromSettings)
                .ToList()
        };
    }
        
    private InventoryGridData CreateInventoryFromSettings(InventoryGridConfig gridConfig)
    {
        List<InventorySlotData> createdInventorySlots = new List<InventorySlotData>();
        float lenght = gridConfig.GridSize.x * gridConfig.GridSize.y;

        for (int i = 0; i < lenght; i++)
        {
            createdInventorySlots.Add(new InventorySlotData());
        }

        InventoryGridData createdInventoryData = new InventoryGridData()
        {
            OwnerId = gridConfig.OwnerId,
            Size = gridConfig.GridSize,
            Slots = createdInventorySlots
        };
        
        return createdInventoryData;
    }
}