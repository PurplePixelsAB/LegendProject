namespace Network.Packets
{
    public enum PacketIdentity : byte
    {
        Invalid = 0,
        MoveTo = 1,
        AimTo = 2,
        UpdateMobile = 3,
        SelectCharacter = 4,
        PerformAbility = 5,
        UpdateStatic = 6,
    }
}