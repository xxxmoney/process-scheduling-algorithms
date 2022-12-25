using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
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

        History GetHistory();
    }

    internal abstract class Scheduler : IScheduler
    {
        private History history;
        protected readonly List<Process> processes;
        protected List<Process> NotFinished => this.processes.Where(process => !process.IsFinished).ToList();
        protected int currentTime;

        protected Scheduler(List<Process> processes)
        {
            this.processes = processes;
            this.history = new History();
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.AppendLine("Processes: ");
            builder.AppendLine(string.Join('\n', this.processes.OrderBy(process => process.Id)));
            builder.AppendLine("History: ");
            builder.AppendLine(this.history.ToString());
            return builder.ToString();
        }

        protected virtual void SortBefore()
        {
            this.processes.Sort((p1, p2) => p1.ArrivalTime.CompareTo(p2.ArrivalTime));
        }

        protected abstract Process GetNext();

        protected virtual int GetExecutionLength(Process nextProcess)
        {
            return nextProcess.RemainingTime;
        }

        protected virtual void BeforeProcessOnce(Process nextProcess)
        {
        }

        protected virtual void ProcessOnce(Process nextProcess)
        {
            int length = GetExecutionLength(nextProcess);
            for (int i = 0; i < length; i++)
            {
                this.currentTime++;
                nextProcess.Run(this.currentTime);
                this.history.Add(currentTime, nextProcess);
            }
        }

        protected virtual void AfterProcessOnce(Process nextProcess)
        {
        }


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

                this.BeforeProcessOnce(nextProcess);
                this.ProcessOnce(nextProcess);
                this.AfterProcessOnce(nextProcess);
            }

            return this.processes;
        }

        public History GetHistory()
        {
            return this.history;
        }
    }

}
