using UnityEngine;

namespace Inventory
{
    public class InventoryService
    {
        private readonly UsableEntityFactory _factory;
        private readonly IGameStateSaver _gameStateSaver;
        private InventoryGrid _inventory;

        public InventoryService(IGameStateSaver gameStateSaver, UsableEntityFactory factory)
        {
            _gameStateSaver = gameStateSaver;
            _factory = factory;
        }
        
        public InventoryGrid RegisterInventory(InventoryGridData data, InventoryItemsDatabaseConfig dataBaseConfig)
        {
            _inventory = new InventoryGrid(data, dataBaseConfig);
            return _inventory;
        }
        
        public AddItemsToInventoryGridResult AddItemsToInventory(InventoryItemConfig config)
        {
            AddItemsToInventoryGridResult result = _inventory.AddItems(config.ItemId, config.Amount);
            _gameStateSaver.Save();
            return result;
        }
        
        public AddItemsToInventoryGridResult AddItemsToInventory(Vector2Int slotCoords, InventoryItemConfig config)
        {
            AddItemsToInventoryGridResult result = _inventory.AddItems(config.ItemId, config.Amount);
            _gameStateSaver.Save();
            return result;
        }
        
        public RemoveItemsToInventoryGridResult RemoveItemsToInventory(InventoryItemConfig config, int amount = 1)
        {
            RemoveItemsToInventoryGridResult result = _inventory.RemoveItems(config.ItemId, amount);
            _gameStateSaver.Save();
            return result;
        }
        
        public RemoveItemsToInventoryGridResult RemoveItemsToInventory(Vector2Int slootCoords, InventoryItemConfig config, int amount = 1)
        {
            RemoveItemsToInventoryGridResult result = _inventory.RemoveItems(slootCoords, config.ItemId, amount);
            _gameStateSaver.Save();
            return result;
        }

        public bool SwithSlots(Vector2Int slotCoordsA, Vector2Int slotCoordsB)
        {
            _inventory.SwithSlots(slotCoordsA, slotCoordsB);
            _gameStateSaver.Save();
            return true;
        }
        
        public void UseItem(Vector2Int slootCoords, InventoryItemConfig config)
        {
            UsableEntity entity = _factory.Create(config);

            if (entity != null)
            {
                entity.Use(config);
            
                RemoveItemsToInventory(config);
            }
        }

        public bool Has(string itemId, int amount = 1)
        {
            return _inventory.Has(itemId, amount);
        }

        public IReadOnlyInventoryGrid GetInventory()
        {
            return _inventory;
        }
    }
}