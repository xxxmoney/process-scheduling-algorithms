﻿using ProcessScheduling.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessScheduling.Core.Schedulers
{
    public class FirstComeFirstServeScheduler : Scheduler
    {
        public FirstComeFirstServeScheduler(List<Process> processes) : base(processes)
        {
        }

        protected override Process GetNext()
        {
            return this.NotFinishedNotInterrupted.Find(process => process.LastArrivalTime <= this.currentTime);
        }
    }
}
