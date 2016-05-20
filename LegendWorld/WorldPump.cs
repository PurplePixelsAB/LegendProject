using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Network
{
    public class WorldPump
    {
        public readonly static int Interval = 50;

        private TimeSpan updateIntervalTimeSpan;
        private long lastUpdateTicks;
        private long nextUpdateTicks;

        public WorldState State { get; set; }
        public bool IsRunning { get; set; }

        public WorldPump()
        {
            updateIntervalTimeSpan = new TimeSpan(0, 0, 0, 0, WorldPump.Interval);
        }

        public void Update(GameTime gameTime)
        {
            this.Update(gameTime.TotalGameTime.Ticks);
        }
        public void Update(long ticks)
        {
            if (ticks >= nextUpdateTicks)
            {
                this.UpdateWorld(new GameTime(new TimeSpan(ticks), new TimeSpan(ticks - lastUpdateTicks)));
                lastUpdateTicks = ticks;
                nextUpdateTicks = lastUpdateTicks + updateIntervalTimeSpan.Ticks;
            }
        }
        private void UpdateWorld(GameTime gameTime)
        {
            this.State.Update(gameTime);
        }

        public void Start()
        {
            IsRunning = true;
            DateTime serverStart = DateTime.Now;
            bool quit = false;
            while (!quit)
            {
                TimeSpan totalTimeElapsed = DateTime.Now - serverStart;
                this.Update(totalTimeElapsed.Ticks);
                //worldPump.Update()
                //quit = socketServerTask.Status != TaskStatus.Running;
                quit = !this.IsRunning;
            }
        }
    }
}
