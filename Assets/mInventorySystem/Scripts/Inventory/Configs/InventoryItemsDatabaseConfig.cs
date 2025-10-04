using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemsDatabase", menuName = "Inventory System/Items Database")]
public class InventoryItemsDatabaseConfig : ScriptableObject
{
    [SerializeField, Expandable] private List<InventoryItemConfig> _items;
    
    private Dictionary<string, InventoryItemConfig> _itemsMap;
    
    public InventoryItemConfig GetItem(string id)
    {
        if (_itemsMap == null)
        {
            _itemsMap = new Dictionary<string, InventoryItemConfig>();
            
            foreach (InventoryItemConfig item in _items)
                _itemsMap[item.ItemId] = item;
        }

        return _itemsMap.GetValueOrDefault(id);
    }
}
