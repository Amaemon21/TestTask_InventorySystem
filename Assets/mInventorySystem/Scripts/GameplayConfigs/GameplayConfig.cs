using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "GameplayConfig", menuName = "Gameplay/GameplayConfig")]
public class GameplayConfig : ScriptableObject
{
    [field: SerializeField, BoxGroup("Inventory"), HorizontalLine] public InventoryGridConfig[] InventoryGrids { get; private set; }
    [field: SerializeField, BoxGroup("Inventory"), Expandable] public InventoryItemsDatabaseConfig InventoryItemsDatabaseConfig { get; private set; }
}
