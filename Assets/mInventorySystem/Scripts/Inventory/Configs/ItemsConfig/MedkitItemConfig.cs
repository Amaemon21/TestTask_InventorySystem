using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "MedkitItemConfig", menuName = "Inventory System/Items/Medkit Config")]
public class MedkitItemConfig : InventoryItemConfig
{
    [field: SerializeField, BoxGroup("MedkitConfig"), HorizontalLine] public int HealAmount { get; private set; } = 25;
}