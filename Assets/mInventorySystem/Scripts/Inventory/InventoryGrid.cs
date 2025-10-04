using System;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory
{
    public class InventoryGrid : IReadOnlyInventoryGrid
    {
        private readonly InventoryGridData _data;
        private readonly Dictionary<Vector2Int, InventorySlot> _slotsMap = new();
        
        public string OwnerId => _data.OwnerId;
        
        public Vector2Int Size
        {
            get => _data.Size;
            set
            {
                if (_data.Size != value)
                {
                    _data.Size = value;
                    SizeChanged?.Invoke(value);
                }
            }
        }

        public event Action<Vector2Int> SizeChanged;
        public event Action<string, int> ItemsAdded;
        public event Action<string, int> ItemsRemoved;

        public InventoryGrid(InventoryGridData data, InventoryItemsDatabaseConfig databaseConfig)
        {
            _data = data;
            
            var size = _data.Size;

            for (int i = 0; i < size.x; i++)
            {
                for (int j = 0; j < size.y; j++)
                {
                    int index = i * size.y + j;
                    Vector2Int position = new Vector2Int(i, j);
                    
                    InventorySlotData slotData = _data.Slots[index];
                    slotData.SlotCoords = position;
                    
                    InventorySlot slot = new InventorySlot(slotData, databaseConfig);
                    _slotsMap[position] = slot; 
                }
            }
        }
        
        public AddItemsToInventoryGridResult AddItems(string itemId, int amount = 1)
        {
            int remainingAmount = amount;
            int itemsAddedToSlotsWithSameItemsAmount = AddToSlotsWithSameItems(itemId, remainingAmount, out remainingAmount);

            if (remainingAmount <= 0)
            {
                return new AddItemsToInventoryGridResult(OwnerId, amount, itemsAddedToSlotsWithSameItemsAmount);
            }
            
            int itemsAddedToAvailableSlotsAmount = AddToFistAvailableSlots(itemId, remainingAmount, out remainingAmount);
            int totalAddedItemsAmount = itemsAddedToSlotsWithSameItemsAmount + itemsAddedToAvailableSlotsAmount;
            
            return new AddItemsToInventoryGridResult(OwnerId, amount, totalAddedItemsAmount);
        }

        public AddItemsToInventoryGridResult AddItems(Vector2Int slotCoords, string itemId, int amount)
        {
            InventorySlot slot = _slotsMap[slotCoords];
            int newValue = slot.Amount + amount;
            int itemsAddedAmount = 0;

            if (slot.IsEmpty)
            {
                slot.ItemId = itemId;
            }
            
            int itemSlotCapacity = slot.GetItemConfig().MaxCapacity;

            if (newValue > itemSlotCapacity)
            {
                int remainingItems = newValue - itemSlotCapacity;
                int itemsToAddedAmount = itemSlotCapacity - slot.Amount;
                itemsAddedAmount += itemsToAddedAmount;
                slot.Amount = itemSlotCapacity;
                
                AddItemsToInventoryGridResult result = AddItems(itemId, remainingItems);
                itemsAddedAmount += result.ItemsAddedAmount;
            }
            else
            {
                itemsAddedAmount = amount;
                slot.Amount = newValue;
            }
            
            return new AddItemsToInventoryGridResult(OwnerId, amount, itemsAddedAmount);
        }
        
        public RemoveItemsToInventoryGridResult RemoveItems(string itemId, int amount = 1)
        {
            if (!Has(itemId, amount))
            {
                return new RemoveItemsToInventoryGridResult(OwnerId, amount, false);
            }
            
            int amountToRemove = amount;

            for (int i = 0; i < Size.x; i++)
            {
                for (int j = 0; j < Size.y; j++)
                {
                    Vector2Int slotCoords = new Vector2Int(i, j);
                    InventorySlot slot = _slotsMap[slotCoords];

                    if (slot.ItemId != itemId)
                    {
                        continue;
                    }

                    if (amountToRemove > slot.Amount)
                    {
                        amountToRemove -= slot.Amount;
                        
                        RemoveItems(slotCoords, itemId, slot.Amount);

                        if (amountToRemove == 0)
                        {
                            return new RemoveItemsToInventoryGridResult(OwnerId, amount, true);
                        }
                    }
                    else
                    {
                        RemoveItems(slotCoords, itemId, amountToRemove);
                        
                        return new RemoveItemsToInventoryGridResult(OwnerId, amount, true);
                    }
                }
            }
            
            throw new Exception("Something went wrong, could not remove items from this grid.");
        }
        
        public RemoveItemsToInventoryGridResult RemoveItems(Vector2Int slotCoords, string itemId, int amount)
        {
            var slot = _slotsMap[slotCoords];

            if (slot.IsEmpty || slot.ItemId != itemId || slot.Amount < amount)
            {
                return new RemoveItemsToInventoryGridResult(OwnerId, amount, false);
            }
            
            slot.Amount -= amount;

            if (slot.Amount == 0)
            {
                slot.ItemId = null;
            }
            
            return new RemoveItemsToInventoryGridResult(OwnerId, amount, true);
        }
        
        public int GetAmount(string itemId)
        {
            int amount = 0;
            List<InventorySlotData> slots = _data.Slots;

            foreach (InventorySlotData slot in slots)
            {
                if (slot.ItemId == itemId)
                {
                    amount += slot.Amount;
                }
            }
            
            return amount;
        }

        public bool Has(string itemId, int amount)
        { 
            int amountExit = GetAmount(itemId);
            return amountExit >= amount;
        }

        public void SwithSlots(Vector2Int slotCoordsA, Vector2Int slotCoordsB)
        {
            InventorySlot slotA = _slotsMap[slotCoordsA];
            InventorySlot slotB = _slotsMap[slotCoordsB];
            
            string tempSlotItemId = slotA.ItemId;
            int tempSlotItemAmount = slotA.Amount;
            
            slotA.ItemId = slotB.ItemId;
            slotA.Amount = slotB.Amount;
            slotB.ItemId = tempSlotItemId;
            slotB.Amount = tempSlotItemAmount;
        }
        
        public IReadOnlyInventorySlot[,] GetSlots()
        {
            var array = new IReadOnlyInventorySlot[Size.x, Size.y];
            
            for (int i = 0; i < Size.x; i++)
            {
                for (int j = 0; j < Size.y; j++)
                {
                    var position = new Vector2Int(i, j);
                    array[i, j] = _slotsMap[position];
                }
            }
            
            return array;
        }
        
        private int AddToSlotsWithSameItems(string itemId, int amount, out int remainingAmount)
        {
            int itemsAddedAmount = 0;
            remainingAmount = amount;

            for (int i = 0; i < Size.x; i++)
            {
                for (int j = 0; j < Size.y; j++)
                {
                    Vector2Int coords = new Vector2Int(i, j);
                    InventorySlot slot = _slotsMap[coords];

                    if (slot.IsEmpty)
                        continue;
                    
                    int slotItemCapacity = slot.GetItemConfig().MaxCapacity;

                    if (slot.Amount >= slotItemCapacity)
                        continue; 

                    if (slot.ItemId != itemId)
                        continue;

                    int newValue = slot.Amount + remainingAmount;

                    if (newValue > slotItemCapacity)
                    {
                        remainingAmount = newValue - slotItemCapacity;
                        int itemsToAddAmount = slotItemCapacity - slot.Amount;
                        itemsAddedAmount += itemsToAddAmount;  
                        slot.Amount = slotItemCapacity;

                        if (remainingAmount == 0)
                        {
                            return itemsAddedAmount;
                        }
                    }
                    else
                    {
                        itemsAddedAmount += remainingAmount;
                        slot.Amount = newValue;
                        remainingAmount = 0;
                        
                        return itemsAddedAmount;
                    }
                }
            }
            
            return itemsAddedAmount;
        }
        
        private int AddToFistAvailableSlots(string itemId, int amount, out int remainingAmount)
        {
            int itemsAddedAmount = 0;
            remainingAmount = amount;

            for (int i = 0; i < Size.x; i++)
            {
                for (int j = 0; j < Size.y; j++)
                {
                    Vector2Int coords = new Vector2Int(i, j);
                    var slot = _slotsMap[coords];

                    if (!slot.IsEmpty)
                        continue;
                    
                    slot.ItemId = itemId;
                    int newValue = remainingAmount;
                    int slotItemCapacity = slot.GetItemConfig().MaxCapacity;

                    if (newValue > slotItemCapacity)
                    {
                        remainingAmount = newValue - slotItemCapacity;
                        int itemsToAddAmount = slotItemCapacity;
                        itemsAddedAmount += itemsToAddAmount;
                        slot.Amount = slotItemCapacity; 
                    }
                    else
                    {
                        itemsAddedAmount += remainingAmount;
                        slot.Amount = newValue;
                        remainingAmount = 0;
                        
                        return itemsAddedAmount;
                    }
                }
            }
            
            return itemsAddedAmount;
        }
    }
}