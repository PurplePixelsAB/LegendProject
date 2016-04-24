using Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace WindowsClient.Net
{
    internal class SocketClient
    {
        //public static int Port = 27960;
        //public static string Server = "";
        private static Socket socket;

        public State State { get; private set; }
        public int Seed { get; private set; }
        public DateTime LastActive { get; private set; }

        private byte[] buffer;
        //private uint LastRecivedTick;
        private ThreadQueue<IPacket> packetQueue;

        //public Queue<ServerResponse> ResponseQueue;

        internal SocketClient()
        {
            packetQueue = new ThreadQueue<IPacket>();
            //ResponseQueue = new Queue<ServerResponse>();
            State = State.Disconnected;
        }

        public void Connect(string serverAdress, int port)
        {
            State = State.Connecting;
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.NoDelay = true;
            socket.BeginConnect(serverAdress, port, new AsyncCallback(BeginConnectCallback), null);
        }
        private void BeginConnectCallback(IAsyncResult ar)
        {
            if (socket != null)
            {
                if (socket.Connected)
                {
                    BeginReceive();
                    State = State.Connected;
                }
                else
                {
                    socket.Close();
                    socket = null;
                    State = State.Disconnected;
                }
            }
            else
                State = State.Disconnected;
        }
        private void BeginReceive()
        {
            //Packet incomingPacket = new Packet();
            this.buffer = new byte[Packet.DefaultBufferSize];
            socket.BeginReceive(this.buffer, 0, Packet.DefaultBufferSize, SocketFlags.None, new AsyncCallback(BeginReceiveCallback), null);
        }
        private void BeginReceiveCallback(IAsyncResult ar)
        {
            //Packet packet = (Packet)ar.AsyncState;
            try
            {
                int byteCount = socket.EndReceive(ar);
                if (byteCount > 0)
                {
                    this.LastActive = DateTime.Now;

                    this.CreatePacket(byteCount);
                    //packet.Size = byteCount;
                    //lock (packetQueue)
                    //    packetQueue.Enqueue(packet);

                    BeginReceive();
                }
                else
                    Disconnect();
            }
            catch (Exception)
            {
                Disconnect();
            }
        }
        private void CreatePacket(int byteCount)
        {
            IPacket incomingPacket = PacketFactory.GetPacket(this.buffer);
            incomingPacket.Size = byteCount;
            //if (incomingPacket.Tick >= this.LastRecivedTick)
            //{
                this.packetQueue.Enqueue(incomingPacket);
                //this.OnIncomingPacket(incomingPacket); //Only needed on server
            //}
        }

        public void Disconnect()
        {
            State = State.Disconnecting;

            if (socket != null)
                socket.Close();
            socket = null;

            //packetQueue.Clear();
            State = State.Disconnected;
        }

        public void Send(Packet packet)
        {
            try
            {
                packet.WriteBuffer();
                socket.Send(packet.Buffer, packet.Size, SocketFlags.None);
            }
            catch (Exception)
            {
                Disconnect();
            }
        }

        public void Process()
        {
            this.ProcessPackets();
        }

        private void ProcessPackets()
        {
            var WorkingQueue = this.packetQueue.DequeueAll();
            lock (WorkingQueue)
            {
                while (WorkingQueue.Count > 0)
                    this.OnProcessPacket(WorkingQueue.Dequeue());
            }
        }

        internal event EventHandler ProcessPacket;
        private void OnProcessPacket(IPacket packet)
        {
            if (this.ProcessPacket != null)
            {
                this.ProcessPacket(packet, new EventArgs());
            }
        }
    }

    public enum State
    {
        Connecting,
        Connected,
        Disconnecting,
        Disconnected
    }
}
