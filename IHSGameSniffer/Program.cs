using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiveSplit.ComponentUtil;

namespace IHSGameSniffer
{
    static class Program
    {
        private const int WORLD = 0x56A6B60;
        private static DeepPointer VelocityX = new DeepPointer(ProcessManager.PROCESS_NAME + ".exe", WORLD, 0x118, 0x3C8, 0x0, 0x260, 0x290, 0x140);
        private static DeepPointer VelocityZ = new DeepPointer(ProcessManager.PROCESS_NAME + ".exe", WORLD, 0x118, 0x3C8, 0x0, 0x260, 0x290, 0x144);
        private static DeepPointer VelocityY = new DeepPointer(ProcessManager.PROCESS_NAME + ".exe", WORLD, 0x118, 0x3C8, 0x0, 0x260, 0x290, 0x148);

        static void Main(string[] args)
        {
            while (true)
            {
                Console.SetCursorPosition(0, 0);
                Process proc = ProcessManager.GetProcess(null);

                if (proc == null)
                {
                    ClearAndWrite("Waiting for game to be launched...");
                    System.Threading.Thread.Sleep(1000);
                    continue;
                }

                ClearAndWrite("Game running!");

                // Update on-screen values
                try
                {

                    Console.SetCursorPosition(0, 2);

                    var velX = VelocityX.Deref<float>(proc);
                    var velZ = VelocityZ.Deref<float>(proc);
                    var velY = VelocityY.Deref<float>(proc);

                    var velHori = Math.Sqrt(Math.Pow(velX, 2) + Math.Pow(velZ, 2));
                    ClearAndWrite("Horizontal Velocity: " + velHori);
                    ClearAndWrite("Veritcal Velocity: " + velY);
                }
                catch (Exception e) {
                    Console.WriteLine(e);
                }

                System.Threading.Thread.Sleep(100);
            }
        }

        static void ClearAndWrite(string line)
        {
            int currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(line + new string(' ', Console.WindowWidth - line.Length));
        }
    }
}
