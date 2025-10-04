using System;
using UnityEngine;

namespace Inventory
{
    public class InventorySlot : IReadOnlyInventorySlot
    {
        private readonly InventorySlotData _data;
        private readonly InventoryItemsDatabaseConfig _databaseConfig;
        
        public string ItemId
        {
            get => _data.ItemId;
            set
            {
                if (_data.ItemId != value)
                {
                    _data.ItemId = value;
                    ItemChanged?.Invoke(GetItemConfig());
                }
            }
        }
        
        public int Amount
        {
            get => _data.Amount;
            set
            {
                if (_data.Amount != value)
                {
                    _data.Amount = value;
                    ItemAmountChanged?.Invoke(value);
                }
            }
        }

        public Vector2Int SlotCoords {get; private set;}
        
        public bool IsEmpty => Amount == 0 && string.IsNullOrEmpty(ItemId);

        public InventoryItemConfig GetItemConfig() => !IsEmpty ? _databaseConfig.GetItem(ItemId) : null;

        public event Action<InventoryItemConfig> ItemChanged;
        public event Action<int> ItemAmountChanged;

        public InventorySlot(InventorySlotData data, InventoryItemsDatabaseConfig databaseConfig)
        {
            _data = data;
            SlotCoords = data.SlotCoords;
            _databaseConfig = databaseConfig;
        }
    }
}