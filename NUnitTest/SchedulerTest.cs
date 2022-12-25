using ProcessScheduling.Core.Data;
using ProcessScheduling.Core.Schedulers;

namespace NUnitTest
{
    public class Tests
    {
        

        [Test]
        public void FirstComeFirstServer()
        {
            // Arrange
            var processes = new List<Process>()
            {
                new Process(1, 8, 2),
                new Process(2, 5, 1),
                new Process(3, 2, 7),
                new Process(4, 4, 3),
                new Process(5, 2, 8),
                new Process(6, 4, 2),
                new Process(7, 3, 5),
            };
            var expected = new Dictionary<int, int>
            {
                { 1, 30 },
                { 2, 28 },
                { 3, 9 },
                { 4, 25 },
                { 5, 17 },
                { 6, 27 },
                { 7, 22 },
            };
            var scheduler = new FirstComeFirstServeScheduler(processes);
            
            // Act
            scheduler.Process();

            // Assert
            foreach (var process in processes.OrderBy(process => process.Id))
            {
                Console.WriteLine(process);
            }
            Assert.Multiple(() =>
            {
                foreach (var process in processes)
                {
                    Assert.IsTrue(process.FinishTime == expected[process.Id]);
                }
            });
        }

        [Test]
        public void ShortestJobFirst()
        {
            // Arrange
            var processes = new List<Process>()
            {
                new Process(1, 8, 2),
                new Process(2, 5, 1),
                new Process(3, 2, 7),
                new Process(4, 4, 3),
                new Process(5, 2, 8),
                new Process(6, 4, 2),
                new Process(7, 3, 5),
            };
            var expected = new Dictionary<int, int>
            {
                { 1, 14 },
                { 2, 10 },
                { 3, 9 },
                { 4, 17 },
                { 5, 30 },
                { 6, 12 },
                { 7, 22 },
            };
            var scheduler = new ShortestJobFirstScheduler(processes);

            // Act
            scheduler.Process();

            // Assert
            foreach (var process in processes.OrderBy(process => process.Id))
            {
                Console.WriteLine(process);
            }
            Assert.Multiple(() =>
            {
                foreach (var process in processes)
                {
                    Assert.IsTrue(process.FinishTime == expected[process.Id]);
                }
            });
        }
    }
}