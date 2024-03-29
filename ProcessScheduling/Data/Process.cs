﻿namespace ProcessScheduling.Core.Data
{
    public class Process
    {
        /// <summary>
        /// Initializes process with identificator, arrival time and burst time.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="arrivalTime"></param>
        /// <param name="burstTime"></param>
        public Process(int id, int arrivalTime, int burstTime)
        {
            this.Id = id;
            this.arrivalTime = arrivalTime;
            this.BurstTime = burstTime;
            this.remainingTimeFull = burstTime;

            this.additionalArrivalTimes = new List<int>();
        }

        private Process()
        {
        }

        /// <summary>
        /// Identificator of process.
        /// </summary>
        public int Id { get; }
        /// <summary>
        /// Time of arrival.
        /// </summary>
        private readonly int arrivalTime;
        /// <summary>
        /// How much time it takes to complete.
        /// </summary>
        public int BurstTime { get; }
        /// <summary>
        /// When was the process started.
        /// </summary>
        public int StartTime { get; set; }
        /// <summary>
        /// Additional start times in case of interruptions.
        /// </summary>
        private readonly List<int> additionalArrivalTimes;
        /// <summary>
        /// How much of time remains for process to be fully processed.
        /// </summary>
        private int remainingTimeFull;
        /// <summary>
        /// How much of time remains for process to be partially processed.
        /// Note that if interruption is not defined this value is equal to full remaining time.
        /// </summary>
        public int RemainingTimePartial
        {
            get
            {
                if (this.Interruption != null && this.Interruption.PassesFrequency)
                {
                    return this.Interruption.Limit - this.ConsecutiveTime;
                }
                else
                {
                    return this.remainingTimeFull;
                }
            }
        }
        /// <summary>
        /// When was the process finished.
        /// </summary>
        public int FinishTime { get; private set; }
        /// <summary>
        /// Whether process is finished.
        /// </summary>
        public bool IsFinished { get; private set; }
        /// <summary>
        /// Interruption of process.
        /// </summary>
        public Interruption Interruption { get; set; }
        /// <summary>
        /// How many times was this process run consecutive.
        /// </summary>
        public int ConsecutiveTime { get; set; }

        /// <summary>
        /// Adds additional start time.
        /// </summary>
        /// <param name="startTime"></param>
        public void AddAdditionalArrivalTime(int startTime)
        {
            this.additionalArrivalTimes.Add(startTime);
        }

        /// <summary>
        /// Gets last arrival time.
        /// </summary>
        /// <returns></returns>
        public int LastArrivalTime => !this.additionalArrivalTimes.Any() ? this.arrivalTime : this.additionalArrivalTimes.Last();
        
        public void Run(int currentTime)
        {
            if (this.IsFinished)
            {
                throw new Exception("Process is already finished.");
            }

            if (this.remainingTimeFull == this.BurstTime)
            {
                StartTime = currentTime;
            }

            this.remainingTimeFull--;

            if (this.remainingTimeFull == 0)
            {
                this.Finish(currentTime);
            }
        }

        private void Finish(int currentTime)
        {
            this.FinishTime = currentTime;
            this.IsFinished = true;
        }

        public override string ToString()
        {
            return $"Id: {Id} Arrival Time: {arrivalTime} Burst Time: {BurstTime} Start Time: {StartTime} Finish Time: {FinishTime}.";
        }
    }

    public class PriorityProcess : Process
    {
        public PriorityProcess(int id, int arrivalTime, int burstTime, int priority) : base(id, arrivalTime, burstTime)
        {
            this.Priority = priority;
        }

        public int Priority { get; }
    }
    
}