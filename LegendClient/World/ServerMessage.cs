namespace WindowsClient.World
{
    internal class ServerMessage
    {
        private int code;
        private string message;

        public ServerMessage(int code, string message)
        {
            this.Code = code;
            this.Message = message;
            this.Duration = 5000D;
        }

        public int Code
        {
            get
            {
                return code;
            }

            set
            {
                code = value;
            }
        }

        public double Duration { get; internal set; }

        public string Message
        {
            get
            {
                return message;
            }

            set
            {
                message = value;
            }
        }
    }
}