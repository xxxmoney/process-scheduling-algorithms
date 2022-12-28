using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ProcessScheduling.Core.Data;
using ProcessScheduling.Core.Enums;

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
        private readonly bool sortEachTime;
        protected List<Process> NotFinishedNotInterrupted => this.processes.Where(process => !process.IsFinished && (process.Interruption == null || !process.Interruption.IsInterrupted)).ToList();
        protected List<Process> Interrupted => this.processes.Where(process => process.Interruption != null && process.Interruption.IsInterrupted).ToList();
        protected Process lastProcess;
        protected int currentTime;
        protected int consequtiveTime;

        protected Scheduler(List<Process> processes, bool sortEachTime = true)
        {
            this.processes = processes;
            this.sortEachTime = sortEachTime;
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
            this.processes.Sort((p1, p2) => p1.GetLastArrivalTime().CompareTo(p2.GetLastArrivalTime()));
        }

        protected abstract Process GetNext();

        protected virtual int GetExecutionLength(Process nextProcess)
        {
            return nextProcess.RemainingTime;
        }

        protected virtual bool ShouldInterrupt(Process nextProcess, int consecutive)
        {
            bool isEnabled = nextProcess.Interruption != null;
            if (!isEnabled)
            {
                return false;
            }

            bool exceedsLimit = consecutive >= nextProcess.Interruption.Limit;
            bool passesFrequency = nextProcess.Interruption.Frequency == InterruptionFrequency.EachTime || nextProcess.Interruption.Counter == 0;
            if (exceedsLimit && passesFrequency)
            {
                return true;
            }

            return false;
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

                if (this.lastProcess == nextProcess)
                {
                    this.consequtiveTime++;
                }
                else
                {
                    this.consequtiveTime = 0;
                }

                this.lastProcess = nextProcess;

                // Releases once all interrupted - thus decreasing waiting remaining and else.
                this.Interrupted.ForEach(process =>
                {
                    if (process.Interruption.ReleaseOnce())
                    {
                        process.AddAdditionalArrivalTime(this.currentTime);
                    }
                });

                // Decides whether to interrupt current.
                if (ShouldInterrupt(nextProcess, this.consequtiveTime + 1))
                {
                    nextProcess.Interruption.Interrupt();
                    break;
                }
            }
        }

        protected virtual void AfterProcessOnce(Process nextProcess)
        {
        }

        public List<Process> Process()
        {            
            // Iterates until all processes have completed.
            while (!processes.All(process => process.IsFinished))
            {
                // Sorts the processes.
                if (this.sortEachTime || this.currentTime == 0)
                {
                    this.SortBefore();
                }

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
