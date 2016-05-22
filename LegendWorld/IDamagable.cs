namespace Data.World
{
    internal interface IDamagable
    {
        int Health { get; set; }
        int MaxHealth { get; }

        //void ApplyDamage(byte damageAmount);
    }
}