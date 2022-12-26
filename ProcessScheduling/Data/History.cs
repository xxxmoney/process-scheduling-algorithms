using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcessScheduling.Core.Data
{
    public class History
    {
        public List<HistoryItem> items = new List<HistoryItem>();

        public void Add(int currentTime, Process process)
        {
            this.items.Add(new HistoryItem(currentTime, process));
        }

        public HistoryItem[] GetHistoryItems()
        {
            return this.items.ToArray();
        }

        public override string ToString()
        {
            return string.Join('\n', this.items);
        }
    }

    public class HistoryItem
    {
        public HistoryItem(int time, Process process)
        {
            Time = time;
            Process = process;
        }

        public int Time { get; }
        public Process Process { get; }

        public override string ToString()
        {
            return $"Time: {this.Time} Process: {this.Process.Id}";
        }
    }
}
