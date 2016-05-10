namespace Data.World
{
    internal interface IDamagable
    {
        byte Health { get; set; }
        byte MaxHealth { get; }

        //void ApplyDamage(byte damageAmount);
    }
}