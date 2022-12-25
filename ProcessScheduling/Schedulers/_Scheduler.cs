using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProcessScheduling.Core.Data;

namespace ProcessScheduling.Core.Schedulers
{
    public interface IScheduler
    {
        /// <summary>
        /// Processes all processes.
        /// Fluent - returns processed input list.
        /// </summary>
        /// <param name="processes"></param>
        /// <returns></returns>
        List<Process> Process();
    }

    internal abstract class Scheduler : IScheduler
    {
        protected readonly List<Process> processes;
        protected List<Process> NotFinished => this.processes.Where(process => !process.IsFinished).ToList();
        protected readonly bool preemptive;
        protected int currentTime;

        protected Scheduler(List<Process> processes, bool preemptive)
        {
            this.processes = processes;
            this.preemptive = preemptive;
        }

        protected virtual void SortBefore()
        {
            this.processes.Sort((p1, p2) => p1.ArrivalTime.CompareTo(p2.ArrivalTime));
        }

        protected abstract Process GetNext();

        public List<Process> Process()
        {
            // Sorts the processes.
            this.SortBefore();

            // Iterates until all processes have completed.
            while (!processes.All(process => process.IsFinished))
            {
                // Find the next process to run
                var nextProcess = GetNext();

                // If no process is ready to run, increments the current time.
                if (nextProcess == null)
                {
                    this.currentTime++;
                    continue;
                }

                // Non-preemptive - runs until process is finished.
                // Preemptive - runs only once.
                int length = this.preemptive ? 1 : nextProcess.BurstTime;
                for (int i = 0; i < length; i++)
                {
                    this.currentTime++;
                    nextProcess.Run(this.currentTime);
                }

            }

            return this.processes;
        }
    }

}
