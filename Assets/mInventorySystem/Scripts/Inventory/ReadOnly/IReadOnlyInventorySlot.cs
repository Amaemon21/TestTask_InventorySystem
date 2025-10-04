using System;
using UnityEngine;

namespace Inventory
{
    public interface IReadOnlyInventorySlot
    {
        string ItemId { get; }
        int Amount { get; }
        Vector2Int SlotCoords { get; }
        bool IsEmpty { get; }

        InventoryItemConfig GetItemConfig();
        
        public event Action<InventoryItemConfig> ItemChanged;
        public event Action<int> ItemAmountChanged;
    }
}