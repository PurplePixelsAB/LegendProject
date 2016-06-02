namespace Network.Packets
{
    public enum PacketIdentity : byte
    {
        Auth = 253,
        Error = 254,
        Invalid = 0,

        MoveTo = 1,
        AimTo = 2,
        StatsChanged = 3,
        PerformAbility = 4,
        MoveItem = 5,
        UseItem = 6,
        ChatStatus = 7,
        ChatMessage = 8,
        NewItem = 9,
    }
}