using UnityEngine;

public class HealUseEntity : UsableEntity
{
    private readonly int _healAmount;

    public HealUseEntity(int healAmount)
    {
        _healAmount = healAmount;
    }

    public override void Use(InventoryItemConfig config)
    {
        Debug.Log($"Used {config.ItemName} and healed {_healAmount} HP");
    }
}