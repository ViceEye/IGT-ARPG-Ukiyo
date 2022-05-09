namespace Ukiyo.Serializable.Entity
{
    public interface IEntityStat
    {
        double Health { get; set; }
        double MaxHealth { get; set; }
        double Damage { get; set; }
        double Mana { get; set; }
        double MaxMana { get; set; }
        double CriticalRate { get; set; }
        double CriticalDamage { get; set; }
        int Exp { get; set; }
    }
}