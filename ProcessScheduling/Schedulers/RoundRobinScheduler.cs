﻿using ProcessScheduling.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessScheduling.Core.Schedulers
{
    public class RoundRobinScheduler : Scheduler
    {
        private readonly int timeSlice;

        public RoundRobinScheduler(List<Process> processes, int timeSlice) : base(processes, false)
        {
            this.timeSlice = timeSlice;
        }

        protected override Process GetNext()
        {
            return this.NotFinishedNotInterrupted.Find(process => process.LastArrivalTime <= this.currentTime);
        }

        protected override int GetExecutionLength(Process nextProcess)
        {
            return Math.Min(this.timeSlice, nextProcess.RemainingTimePartial);
        }

        protected override void BeforeProcessOnce(Process nextProcess)
        {
            this.processes.Remove(nextProcess);
        }

        protected override void AfterProcessOnce(Process nextProcess)
        {
            int insertAfter = this.processes.OrderBy(process => process.LastArrivalTime).ToList().FindLastIndex(process => process.LastArrivalTime <= this.currentTime);
            if (insertAfter >= 0)
            {
                this.processes.Insert(insertAfter + 1, nextProcess);
            }
            else
            {
                this.processes.Add(nextProcess);
            }
        }
    }
}
