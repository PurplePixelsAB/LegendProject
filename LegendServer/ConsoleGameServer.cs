using DataClient;
using LegendServer.Network;
using Network;
using Network.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UdpServer.Network;
using UdpServer.Network.Packets;

namespace UdpServer
{
    class ConsoleGameServer : IDisposable
    {
        private SocketServer socketServer;
        private Task worldServerTask;
        private Task socketServerTask;
        private ServerWorldState worldState;
        private WorldPump worldPump;
        private ServerWorldDataContext worldDataContext;

        internal ConsoleGameServer()
        {
        }

        private void WaitOnKeyPress()
        {
            while (Console.Read() == 0)
                Thread.Sleep(500);
        }

        public void Start()
        {
            //this.Status = ServerStatus.Starting;

            Assembly assembly = Assembly.GetEntryAssembly();
            Version ver = assembly.GetName().Version;

            ConsoleGameServer.WriteLine("LegendServer Version {0}.{1}, Build {2}.{3}", ver.Major, ver.Minor, ver.Build, ver.Revision);
            ConsoleGameServer.WriteLine("Running on .NET Framework Version {0}.{1}.{2}", Environment.Version.Major, Environment.Version.Minor, Environment.Version.Build);

            try
            {
                ConsoleGameServer.WriteLine("Starting Data Connection...");
                worldDataContext = new ServerWorldDataContext(string.Format(@"http://{0}:{1}/", LegendServer.Properties.Settings.Default.DataServerAddress, LegendServer.Properties.Settings.Default.DataServerPort));
                if (!worldDataContext.AuthServer(LegendServer.Properties.Settings.Default.DataServerUsername, LegendServer.Properties.Settings.Default.DataServerPassword))
                {
                    throw new Exception("Failed to auth with data server.");                    
                }
            }
            catch (Exception e)
            {
                ConsoleGameServer.WriteLine("Starting Data Connection failed, general Exception occured:");
                ConsoleGameServer.WriteLine(e.ToString());
                this.WaitOnKeyPress();
                return;
            }
            finally
            {
                ConsoleGameServer.WriteLine("Data Connection Successfull.");
            }

            try
            {
                ConsoleGameServer.WriteLine("Starting WorldServer...");
                worldState = new ServerWorldState(worldDataContext);
                worldPump = new WorldPump();
                worldPump.State = worldState;                
                worldServerTask = Task.Factory.StartNew(() => worldPump.Start());
            }
            catch (Exception se)
            {
                ConsoleGameServer.WriteLine("Starting WorldServer failed:");
                ConsoleGameServer.WriteLine(se.ToString());
                this.WaitOnKeyPress();
                return;
            }
            finally
            {
                ConsoleGameServer.WriteLine("WorldServer running.");
                //this.Status = ServerStatus.Running;
            }


            try
            {
                ConsoleGameServer.WriteLine("Starting SocketServer...");
                socketServer = new SocketServer(this);
                //socketServer.Activity += SocketServer_Activity;
                socketServer.ProcessPacket += SocketServer_ProcessPacket;
                socketServerTask = Task.Factory.StartNew(() => socketServer.Start());
            }
            catch (SocketException se)
            {
                ConsoleGameServer.WriteLine("Starting SocketServer failed:");
                ConsoleGameServer.WriteLine(se.ToString());
                this.WaitOnKeyPress();
                return;
            }
            finally
            {
                ConsoleGameServer.WriteLine("SocketServer running on port {0}.", SocketServer.Port);
                //this.Status = ServerStatus.Running;
            }

            var returnval = Task.WaitAny(worldServerTask, socketServerTask);
            worldPump.IsRunning = socketServer.IsRunning = false;
            //worldPump.State.SaveAndClose(); //Sould be located in DataContex. Game state should not be datalayer aware.
            socketServer.Close();
            //this.Status = ServerStatus.Halting;
        }
        
        private void SocketServer_ProcessPacket(object sender, IPacket packet)
        {
            NetState netState = (NetState)sender;
            ServerPacketHandler packetHandler = ServerPacketHandler.GetHandler(packet.PacketId);
            if (packetHandler != null)
            {
                packetHandler.Handle(packet, netState, worldState);
            }            
        }

        public static void WriteLine(string text)
        {
            Console.WriteLine("[{0}] {1}", DateTime.Now.ToLongTimeString(), text);
        }
        public static void WriteLine(string format, params object[] arg)
        {
            ConsoleGameServer.WriteLine(String.Format(format, arg));
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~ConsoleServer() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
