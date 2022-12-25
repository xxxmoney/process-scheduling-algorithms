using ProcessScheduling.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessScheduling.Core.Schedulers
{
    internal class ShortestJobFirstScheduler : Scheduler
    {
        public ShortestJobFirstScheduler(List<Process> processes) : base(processes)
        {
        }

        protected override Process GetNext()
        {
            Process nextProcess = null;
            int shortestBurstTime = int.MaxValue;
            foreach (var p in processes.Where(process => !process.IsFinished))
            {
                if (p.ArrivalTime <= currentTime && p.BurstTime < shortestBurstTime)
                {
                    nextProcess = p;
                    shortestBurstTime = p.BurstTime;
                }
            }
            return nextProcess;
        }
    }
}
