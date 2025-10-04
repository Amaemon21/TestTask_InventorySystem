using NaughtyAttributes;
using UnityEngine;

public abstract class InventoryItemConfig : ScriptableObject
{
    [field: SerializeField, BoxGroup("Main config"), HorizontalLine] public string ItemId { get; private set; }
    
    [field: SerializeField, BoxGroup("Main config")] public RarityType ItemRarity { get; private set; }
    [field: SerializeField, BoxGroup("Main config")] public ItemType ItemType { get; private set; }
    [field: SerializeField, BoxGroup("Main config")] public string ItemName { get; private set; }

    [field: SerializeField, BoxGroup("Main config")] public string ItemDescription { get; private set; }
    [field: SerializeField, BoxGroup("Main config"), ShowAssetPreview] public Sprite ItemIcon { get; private set; }
    [field: SerializeField, BoxGroup("Main config"), MinValue(1)] public int Amount { get; private set; }
    [field: SerializeField, BoxGroup("Main config"), MinValue(1)] public int MaxCapacity { get; private set; }

    [field: SerializeField, BoxGroup("Main config")] public PickupbleInventoryItem ItemObject { get; private set; }
    
    private void OnValidate()
    {
        Amount = Mathf.Clamp(Amount, 1, MaxCapacity);
    }
}