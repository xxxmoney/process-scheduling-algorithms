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
        public ShortestRemainingTimeFirstScheduler(List<Process> processes) : base(processes, true)
        {
        }

        protected override Process GetNext()
        {
            Process nextProcess = null;
            int minRemainingTime = int.MaxValue;

            foreach (var process in this.NotFinished)
            {
                if (process.ArrivalTime > this.currentTime)
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
