namespace Ukiyo.Player
{
    public interface IPlayerStat
    {
        float Health { get; set; }
        float Damage { get; set; }
        float Mana { get; set; }
        float CriticalRate { get; set; }
        float CriticalDamage { get; set; }
        int Exp { get; set; }
    }
}