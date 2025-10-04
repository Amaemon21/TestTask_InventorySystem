public class UsableEntityFactory
{
    public UsableEntityFactory()
    {
        
    }

    public UsableEntity Create(InventoryItemConfig config)
    {
        switch (config)
        {
            case MedkitItemConfig medkit:
                return new HealUseEntity(medkit.HealAmount);
        }
        
        return null;
    }
}