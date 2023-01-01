using ProcessScheduling.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessScheduling.Core.Schedulers
{
    public class PriorityScheduler : Scheduler
    {
        private readonly bool isPreemptive;

        public PriorityScheduler(bool isPreemptive, List<Process> processes) : base(processes, true)
        {
            this.isPreemptive = isPreemptive;
        }

        protected override int GetExecutionLength(Process nextProcess)
        {
            return this.isPreemptive ? 1 : nextProcess.RemainingTimePartial;
        }

        protected override Process GetNext()
        {
            return this.NotFinishedNotInterrupted
                .Where(process => process.LastArrivalTime <= this.currentTime)
                .MinBy(process =>
                {
                    if(process is PriorityProcess priorityProcess)
                    {
                        return priorityProcess.Priority;
                    }

                    throw new Exception($"Process {process} should be priority process when used in {nameof(PriorityScheduler)}");
                });
        }

        
    }
}
