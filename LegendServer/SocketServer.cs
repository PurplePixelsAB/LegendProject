using LegendWorld.Network.Packets;
using Network;
using Network.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UdpServer.Network;
using UdpServer.Network.Packets;

namespace UdpServer
{
    class SocketServer
    {
        public static int Port = 27960;

        private AutoResetEvent signal;
        private Socket mainSocket;
        private AsyncCallback onAccept;

        private Queue<Socket> acceptedQueue;
        private ThreadQueue<NetState> netStatesWithPacketQueue;

        private static Socket[] EmptySockets = new Socket[0];

        private bool IsClosing = false;

        public bool IsRunning { get; internal set; }

        //private ConsoleGameServer consoleGameServer;

        public SocketServer(ConsoleGameServer consoleGameServer)
        {
            signal = new AutoResetEvent(false);

            PacketFactory.Register(PacketIdentity.MoveTo, () => new MoveToPacket());
            ServerPacketHandler.Register(PacketIdentity.MoveTo, new MoveToPacketHandler());
            PacketFactory.Register(PacketIdentity.AimTo, () => new AimToPacket());
            ServerPacketHandler.Register(PacketIdentity.AimTo, new AimToPacketHandler());
            PacketFactory.Register(PacketIdentity.Auth, () => new AuthPacket());
            ServerPacketHandler.Register(PacketIdentity.Auth, new AuthPacketHandler());
            PacketFactory.Register(PacketIdentity.PerformAbility, () => new PerformAbilityPacket());
            ServerPacketHandler.Register(PacketIdentity.PerformAbility, new PerformAbilityPacketHandler());
            PacketFactory.Register(PacketIdentity.UseItem, () => new UseItemPacket());
            ServerPacketHandler.Register(PacketIdentity.UseItem, new UseItemPacketHandler());
            PacketFactory.Register(PacketIdentity.MoveItem, () => new MoveItemPacket());
            ServerPacketHandler.Register(PacketIdentity.MoveItem, new MoveItemPacketHandler());
            PacketFactory.Register(PacketIdentity.ChatMessage, () => new ChatMessagePacket());
            ServerPacketHandler.Register(PacketIdentity.ChatMessage, new ChatMessagePacketHandler());
            PacketFactory.Register(PacketIdentity.ChatStatus, () => new ChatStatusPacket());
            ServerPacketHandler.Register(PacketIdentity.ChatStatus, new ChatStatusPacketHandler());

            acceptedQueue = new Queue<Socket>();
            onAccept = new AsyncCallback(BeginAcceptCallback);
            netStatesWithPacketQueue = new ThreadQueue<NetState>();
        }

        public void Start()
        {
            if (IsClosing)
                throw new Exception("SocketServer is closing. Can not be re-started!");

            IsRunning = true;

            mainSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ipLocal = new IPEndPoint(IPAddress.Any, SocketServer.Port);
            mainSocket.NoDelay = true;

            mainSocket.Bind(ipLocal);
            mainSocket.Listen(8);
            mainSocket.BeginAccept(new AsyncCallback(this.onAccept), null);

            while (signal.WaitOne())
            {
                try
                {
                    this.Process();
                }
                catch (Exception ex)
                {
                    this.WriteConsole("Exception occured in socket server pump: {0}", ex.ToString());
                }
            }
            
            IsRunning = false;
            Console.WriteLine("Socket process pump has ended!");
            this.Close();
        }
        public void Close()
        {
            if (IsClosing)
                return;

            IsRunning = false;
            IsClosing = true;
            if (mainSocket != null)
                mainSocket.Close();

            Socket[] accepted = acceptedQueue.ToArray();
            acceptedQueue.Clear();
            for (int i = 0; i < accepted.Length; ++i)
            {
                accepted[i].Close();
                accepted[i] = null;
            }
            acceptedQueue = null;
            accepted = null;

            onAccept = null;
        }

        public void BeginAcceptCallback(IAsyncResult ar)
        {
            if (IsClosing)
                return;

            Socket newSocket = mainSocket.EndAccept(ar);
            newSocket.NoDelay = true;

            //Maybe add some check for the socket.
            Enqueue(newSocket);

            mainSocket.BeginAccept(new AsyncCallback(this.onAccept), null);
        }

        //public event EventHandler Activity;
        private void OnActivity()
        {
            //try
            //{
                signal.Set();
            //}
            //catch { }
        }
        private void Enqueue(Socket socket)
        {
            lock (acceptedQueue)
                acceptedQueue.Enqueue(socket);

            this.OnActivity();
        }

        private void Process()
        {
            this.ProcessNewConnections();
            this.ProcessNewPackets();
        }

        private void ProcessNewConnections()
        {
            Socket[] accepted = this.Slice();
            for (int i = 0; i < accepted.Length; ++i)
            {
                NetState netState = new NetState(accepted[i]);
                netState.IncomingPacket += this.NetState_IncomingPacket;
                netState.Start();
            }
        }

        private void ProcessNewPackets()
        {
            Queue<NetState> netStateQueue = netStatesWithPacketQueue.DequeueAll();
            while (netStateQueue.Count > 0)
            {
                NetState netState = netStateQueue.Dequeue();
                this.ProcessPackets(netState);
            }
        }

        private void ProcessPackets(NetState netState)
        {
            var WorkingQueue = netState.Packets.DequeueAll();
            lock (WorkingQueue)
            {
                while (WorkingQueue.Count > 0)
                    this.OnProcessPacket(netState, WorkingQueue.Dequeue());
            }
        }

        internal event EventHandler<IPacket> ProcessPacket;
        private void OnProcessPacket(NetState netState, IPacket packet)
        {
            if (this.ProcessPacket != null)
            {
                this.ProcessPacket(netState, packet);
            }
        }

        private void NetState_IncomingPacket(object sender, IPacket e)
        {
            this.netStatesWithPacketQueue.Enqueue((NetState)sender);
            this.OnActivity();
        }

        private Socket[] Slice()
        {
            Socket[] array;

            lock (acceptedQueue)
            {
                if (acceptedQueue.Count == 0)
                    return EmptySockets;

                array = acceptedQueue.ToArray();
                acceptedQueue.Clear();
            }

            return array;
        }

        public void WriteConsole(string text)
        {
            ConsoleGameServer.WriteLine(text);
        }

        public void WriteConsole(string format, params object[] args)
        {
            WriteConsole(String.Format(format, args));
        }
    }
}
