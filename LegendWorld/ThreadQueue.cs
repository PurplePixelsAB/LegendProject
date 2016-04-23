using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Network
{
    public class ThreadQueue<TItem> : IDisposable
    {
        private Queue<TItem> stateQueue;
        private Queue<TItem> workingQueue;
        private bool disposed = false;

        public ThreadQueue() : this(0)
        {

        }
        public ThreadQueue(int length)
        {
            stateQueue = new Queue<TItem>(length);
            workingQueue = new Queue<TItem>(length);
        }

        public void Enqueue(TItem state)
        {
            lock (stateQueue)
                stateQueue.Enqueue(state);
            //Core.Set();
        }

        public Queue<TItem> DequeueAll()
        {
            lock (workingQueue)
            {
                workingQueue.Clear();
                Queue<TItem> temp = workingQueue;
                lock (stateQueue)
                {
                    workingQueue = stateQueue;
                    stateQueue = temp;
                }

                return workingQueue;
            }
        }


        //public void Process()
        //{
        //    lock (this)
        //    {
        //        Queue<TItem> temp = WorkingQueue;
        //        WorkingQueue = StateQueue;
        //        StateQueue = temp;
        //    }

        //    lock (WorkingQueue)
        //    {
        //        while (WorkingQueue.Count > 0)
        //            Handle(WorkingQueue.Dequeue());
        //    }
        //}

        //private void Handle(TItem queuedState)
        //{
        //    Packet queuedPacket = queuedState.packetQueue.Dequeue();
        //    byte packetType = queuedPacket.Buffer[0];
        //    PacketHandler handler = queuedState.GetHandler((ClientIds)packetType);
        //    handler.OnReceive(queuedState, queuedPacket);
        //}

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
                    if (stateQueue != null)
                        stateQueue.Clear();
                    stateQueue = null;

                    if (workingQueue != null)
                        workingQueue.Clear();
                    workingQueue = null;

                    disposed = true;
                }
            }
        }
    }
}
