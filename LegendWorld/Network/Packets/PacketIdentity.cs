namespace Network.Packets
{
    public enum PacketIdentity : byte
    {
        Invalid = 0,
        MoveTo = 1,
        AimTo = 2,
        StatsChanged = 3,
        //SelectCharacter = 4,
        PerformAbility = 5,
        //UpdateStatic = 6,
        UseItem = 7,
        PickUpItem = 8,
        //Ack = 253,
        Auth = 254,
        Error = 255,
    }
}