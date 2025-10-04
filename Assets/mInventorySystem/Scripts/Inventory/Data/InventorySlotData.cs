using System;
using UnityEngine;

namespace Inventory
{
    [Serializable]
    public class InventorySlotData
    {
        public string ItemId;
        public int Amount;
        public Vector2Int SlotCoords;
    }
}