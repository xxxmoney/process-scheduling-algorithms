using ProcessScheduling.Core.Enums;

namespace ProcessScheduling.Core.Data
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
            this.RemainingTime = burstTime;

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
        public int RemainingTime { get; private set; }
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
        public int GetLastArrivalTime()
        {
            return !this.additionalArrivalTimes.Any() ? this.arrivalTime : this.additionalArrivalTimes.Last();
        }

        public void Run(int currentTime)
        {
            if (this.IsFinished)
            {
                throw new Exception("Process is already finished.");
            }

            if (this.RemainingTime == this.BurstTime)
            {
                StartTime = currentTime;
            }

            this.RemainingTime--;

            if (this.RemainingTime == 0)
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
}