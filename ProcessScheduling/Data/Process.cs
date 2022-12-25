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
            this.ArrivalTime = arrivalTime;
            this.BurstTime = burstTime;
            this.RemainingTime = burstTime;
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
        public int ArrivalTime { get; }
        /// <summary>
        /// How much time it takes to complete.
        /// </summary>
        public int BurstTime { get; }
        /// <summary>
        /// When was the process started.
        /// </summary>
        public int StartTime { get; set; }
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
            return $"Name: {Id} Arrival Time: {ArrivalTime} Burst Time: {BurstTime} Start Time: {StartTime} Finish Time: {FinishTime}.";
        }
    }
}