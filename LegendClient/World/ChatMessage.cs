using WindowsClient.World.Mobiles;

namespace WindowsClient.World
{
    internal class ChatMessage
    {
        public ChatMessage()
        {
        }

        public double Duration { get; internal set; }
        public ClientCharacter Owner { get; internal set; }
        public string Text { get; internal set; }
    }
}