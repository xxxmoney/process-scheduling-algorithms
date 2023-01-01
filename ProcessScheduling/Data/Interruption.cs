using ProcessScheduling.Core.Enums;

namespace ProcessScheduling.Core.Data
{
    public class Interruption
    {
        public Interruption(int limit, int waitTime, InterruptionFrequency frequency = InterruptionFrequency.Once)
        {
            this.Limit = limit;
            this.WaitTime = waitTime;
            this.Frequency = frequency;
        }
        
        /// <summary>
        /// Interruption time - after how much consequtive should switch process.
        /// </summary>
        public int Limit { get; }
        /// <summary>
        /// How much should process wait after interruption.
        /// </summary>
        public int WaitTime { get; }
        /// <summary>
        /// How much time remains for process to wait after interruption.
        /// </summary>
        public int WaitingRemaining { get; private set; }
        /// <summary>
        /// Interruption frequency for the process.
        /// </summary>
        public InterruptionFrequency Frequency { get; }
        /// <summary>
        /// Counter for how many times was process interrupted.
        /// </summary>
        public int Counter { get; private set; }
        /// <summary>
        /// Whether process is interrupted.
        /// </summary>
        public bool IsInterrupted { get; private set; }

        /// <summary>
        /// Starts the interruption by setting IsInterrupted to true and WaitingRemaining to WaitTime.
        /// </summary>
        public void Interrupt()
        {
            this.IsInterrupted = true;
            this.WaitingRemaining = this.WaitTime;
            this.Counter++;
        }

        /// <summary>
        /// Releases remaining interruption once - also ends interruption if waiting remaining is 0.
        /// Returns true if is released.
        /// </summary>
        public bool ReleaseOnce()
        {
            if (!this.IsInterrupted)
            {
                throw new Exception("Is not interrupted, can't release.");
            }

            this.WaitingRemaining--;
            if (WaitingRemaining == 0)
            {
                this.IsInterrupted = false;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Determines whether frequency is qualified - it can be allowed to interrupt.
        /// </summary>
        public bool PassesFrequency => this.Frequency == InterruptionFrequency.EachTime || this.Counter == 0;
    }
}