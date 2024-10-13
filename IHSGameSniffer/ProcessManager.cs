using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IHSGameSniffer
{
    public static class ProcessManager
    {
        private static Process _process;
        public const string PROCESS_NAME = "Greylock-Win64-Shipping";

        /// <summary>
        /// Returns the process. Will cache results between runs.
        /// </summary>
        public static Process GetProcess(Action<Process> onFound, Action<Process> onExit)
        {
            try
            {
                // We have a game cached and it's good
                if (_process != null && !_process.HasExited)
                {
                    return _process;
                }

                var proc = Process.GetProcesses().FirstOrDefault(x => PROCESS_NAME.ToLower().Contains(x.ProcessName.ToLower()));

                if (proc != null && !proc.HasExited)
                {
                    // Cache the found process
                    _process = proc;

                    // Run event on finding the proc
                    onFound?.Invoke(_process);

                    return _process;
                }

                // if we got to this point, and we previously had a valid process, we should exit
                if (_process != null)
                {
                    onExit?.Invoke(_process);
                }

                _process = null;
            }
            catch (Win32Exception e)
            {
                Console.WriteLine("Run the program as admin");
            }
            return null;
        }
    }
}
