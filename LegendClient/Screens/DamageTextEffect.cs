using WindowsClient.World.Mobiles;

namespace WindowsClient
{
    internal class DamageTextEffect
    {
        public DamageTextEffect()
        {
        }

        public ClientCharacter Character { get; internal set; }
        public double Duration { get; internal set; }
        public string Text { get; internal set; }
    }
}