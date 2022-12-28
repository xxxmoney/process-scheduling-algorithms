using ProcessScheduling.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessScheduling.Core.Schedulers
{
    internal class ShortestRemainingTimeFirstScheduler : Scheduler
    {
        public ShortestRemainingTimeFirstScheduler(List<Process> processes) : base(processes, false)
        {
        }
        
        protected override int GetExecutionLength(Process nextProcess)
        {
            return 1;
        }

        protected override Process GetNext()
        {
            if (this.lastProcess != null && !this.lastProcess.IsFinished && !this.NotFinishedNotInterrupted.Any(process => process != this.lastProcess && process.GetLastArrivalTime() > this.currentTime))
            {
                return this.lastProcess;
            }

            Process nextProcess = null;
            int minRemainingTime = int.MaxValue;

            foreach (var process in this.NotFinishedNotInterrupted)
            {
                if (process.GetLastArrivalTime() > this.currentTime)
                {
                    continue;
                }

                if (process.RemainingTime < minRemainingTime)
                {
                    nextProcess = process;
                    minRemainingTime = process.RemainingTime;
                }
            }

            return nextProcess;
        }
    }
}
