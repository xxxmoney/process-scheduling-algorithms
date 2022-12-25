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
        protected List<Process> processes;
        protected int currentTime;

        protected Scheduler(List<Process> processes)
        {
            this.processes = processes;
        }

        protected virtual void SortBefore()
        {
            processes.Sort((p1, p2) => p1.ArrivalTime.CompareTo(p2.ArrivalTime));
        }

        protected abstract Process GetNext();

        public List<Process> Process()
        {
            // Sort the processes.
            this.SortBefore();

            // Iterates the current time until all processes have completed.
            while (!processes.All(process => process.IsFinished))
            {
                // Find the next process to run
                var nextProcess = GetNext();

                // If no process is ready to run, increments the current time
                if (nextProcess == null)
                {
                    currentTime++;
                    continue;
                }

                // Runs the next process and update the current time.
                currentTime++;
                nextProcess.Run(currentTime);
            }

            return processes;
        }
    }

}
