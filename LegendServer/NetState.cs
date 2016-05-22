using LegendWorld.Network.Packets;
using Network;
using Network.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace UdpServer
{
    public class NetState : IDisposable
    {
        public static TimeSpan Timeout = new TimeSpan(0, 5, 0);

        private static List<NetState> m_Instances = new List<NetState>();
        public static List<NetState> Instances { get { return m_Instances; } }

        public int Id { get; set; }
        public int WorldId { get; set; }
        public long LastRecivedTick { get; set; }

        public NetState(Socket socket)
        {
            Socket = socket;
            Created = DateTime.Now;
            LastActive = DateTime.Now;
            Packets = new ThreadQueue<IPacket>(10);
            LastRecivedTick = 0;

            try
            {
                Address = ((IPEndPoint)Socket.RemoteEndPoint).Address;
            }
            catch (Exception)
            {
                //TraceException(ex);
                Address = IPAddress.None;
            }

            NetState.Instances.Add(this);
        }

        internal void SendError(int v1, string v2)
        {
            ErrorPacket errorPacket = new ErrorPacket(v1, v2);
            this.Send(errorPacket);
        }

        private AsyncCallback OnReceive, OnSend;
        private bool disposed = false;

        public Socket Socket { get; private set; }

        public DateTime Created { get; private set; }
        public DateTime LastActive { get; private set; }

        public IPAddress Address { get; private set; }
        public override string ToString() { return ((this.Address != IPAddress.None) ? this.Address.ToString() : "Error") + " (" + this.WorldId + ")"; }

        private byte[] buffer = new byte[Packet.DefaultBufferSize];
        public byte[] Buffer { get { return buffer; } }
        
        public ThreadQueue<IPacket> Packets;

        public void Start()
        {
            OnReceive = new AsyncCallback(BeginReceiveCallback);
            OnSend = new AsyncCallback(OnSendCallback);
            
            buffer = new byte[Packet.DefaultBufferSize];
            Socket.BeginReceive(this.Buffer, 0, Packet.DefaultBufferSize, 0, new AsyncCallback(OnReceive), this);

            this.WriteConsole("Connected. ({0} Online)", NetState.Instances.Count);
        }

        public EventHandler Disconnected;
        private void OnDisconnected()
        {
            if (this.Disconnected != null)
            {
                this.Disconnected(this, new EventArgs());
            }
        }

        public void BeginReceiveCallback(IAsyncResult ar)
        {
            NetState netState = (NetState)ar.AsyncState;

            try
            {
                int byteCount = netState.Socket.EndReceive(ar);
                if (byteCount > 0)
                {
                    netState.LastActive = DateTime.Now;

                    netState.CreatePacket(byteCount);

                    netState.buffer = new byte[Packet.DefaultBufferSize];
                    netState.Socket.BeginReceive(this.Buffer, 0, Packet.DefaultBufferSize, 0, new AsyncCallback(netState.OnReceive), netState);
                }
                else
                    Dispose();
            }
            catch (Exception ex)
            {
                Dispose();
            }
        }

        private void CreatePacket(int byteCount)
        {
            IPacket incomingPacket = PacketFactory.GetPacket(this.Buffer);
            incomingPacket.Size = byteCount;
            //if (incomingPacket.Tick >= this.LastRecivedTick)
            //{
                this.Packets.Enqueue(incomingPacket);
                this.OnIncomingPacket(incomingPacket);
            //}
        }

        public event EventHandler<IPacket> IncomingPacket;
        private void OnIncomingPacket(IPacket incomingPacket)
        {
            incomingPacket.ReadBuffer();
            if (IncomingPacket != null)
            {
                this.IncomingPacket(this, incomingPacket);
            }
        }

        public void Send(Packet packet)
        {
            try
            {
                packet.WriteBuffer();
                this.Socket.Send(packet.Buffer);
            }
            catch
            {
                Dispose();
            }
        }
        private void OnSendCallback(IAsyncResult asyncResult)
        {
        }

        public bool IsAlive()
        {
            if (this.Socket == null)
                return false;

            if (DateTime.Now < LastActive + NetState.Timeout)
                return true;

            WriteConsole("Disconnecting due to inactivity...");
            this.Dispose();

            return false;
        }

        public void WriteConsole(string text)
        {
            ConsoleGameServer.WriteLine("Client ({0}): {1}", this.ToString(), text);
        }

        public void WriteConsole(string format, params object[] args)
        {
            WriteConsole(String.Format(format, args));
        }

        public void Dispose()
        {
            Dispose(true);
        }
        private void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    if (NetState.Instances.Contains(this))
                        NetState.Instances.Remove(this);

                    //if (packetQueue != null)
                    //    packetQueue.Clear();
                    //packetQueue = null;

                    if (Socket != null)
                        Socket.Close();
                    Socket = null;

                    OnReceive = null;
                    OnSend = null;

                    buffer = null;

                    WriteConsole("Disconnected. ({0} Online)", NetState.Instances.Count);
                    this.OnDisconnected();

                    disposed = true;
                }
            }
        }
    }
}
