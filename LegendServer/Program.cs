﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UdpServer
{
    class Program
    {
        static void Main(string[] args)
        {
            using (ConsoleGameServer consoleServer = new ConsoleGameServer())
            {
                consoleServer.Start();
            }
        }
    }
}
