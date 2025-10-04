using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "Inventory Grid", menuName = "Inventory System/Inventory Grid")]
public class InventoryGridConfig : ScriptableObject
{
    [field: SerializeField, HorizontalLine] public string OwnerId {get; private set;}
    [field: SerializeField] public Vector2Int GridSize {get; private set;}
}
