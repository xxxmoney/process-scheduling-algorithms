using ProcessScheduling.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessScheduling.Core.Schedulers
{
    public class ShortestRemainingTimeFirstScheduler : Scheduler
    {
        public ShortestRemainingTimeFirstScheduler(List<Process> processes) : base(processes)
        {
        }
        
        protected override int GetExecutionLength(Process nextProcess)
        {
            return 1;
        }

        protected override Process GetNext()
        {
            return this.NotFinishedNotInterrupted
                .OrderBy(p => p.RemainingTimePartial)
                .FirstOrDefault(process => process.LastArrivalTime <= this.currentTime);
        }
    }
}
