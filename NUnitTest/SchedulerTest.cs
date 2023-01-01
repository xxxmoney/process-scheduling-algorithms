using ProcessScheduling.Core.Data;
using ProcessScheduling.Core.Schedulers;

namespace NUnitTest
{   
    public class Tests
    {
        [Test]
        public void FirstComeFirstServe()
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
            Console.WriteLine(scheduler);
            Assert.Multiple(() =>
            {
                foreach (var process in processes)
                {
                    Assert.IsTrue(process.FinishTime == expected[process.Id]);
                }
            });
        }

        [Test]
        public void FirstComeFirstServeInterruption()
        {
            // Arrange
            var processes = new List<Process>()
            {
                new Process(1, 8, 2),
                new Process(2, 5, 1),
                new Process(3, 2, 7) { Interruption = new Interruption(3, 2) },
                new Process(4, 4, 3),
                new Process(5, 2, 8) { Interruption = new Interruption(4, 3) },
                new Process(6, 4, 2),
                new Process(7, 3, 5),
            };
            var expected = new Dictionary<int, int>
            {
                { 1, 26 },
                { 2, 20 },
                { 3, 24 },
                { 4, 17 },
                { 5, 30 },
                { 6, 19 },
                { 7, 14 },
            };
            var scheduler = new FirstComeFirstServeScheduler(processes);

            // Act
            scheduler.Process();

            // Assert
            Console.WriteLine(scheduler);
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
            Console.WriteLine(scheduler);
            Assert.Multiple(() =>
            {
                foreach (var process in processes)
                {
                    Assert.IsTrue(process.FinishTime == expected[process.Id]);
                }
            });
        }

        [Test]
        public void ShortestRemainingTimeFirst()
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
                { 1, 12 },
                { 2, 7 },
                { 3, 22 },
                { 4, 10 },
                { 5, 30 },
                { 6, 6 },
                { 7, 16 },
            };
            var scheduler = new ShortestRemainingTimeFirstScheduler(processes);

            // Act
            scheduler.Process();

            // Assert
            Console.WriteLine(scheduler);
            Assert.Multiple(() =>
            {
                foreach (var process in processes)
                {
                    Assert.IsTrue(process.FinishTime == expected[process.Id]);
                }
            });
        }

        [Test]
        public void ShortestRemainingTimeFirstInterruption()
        {
            // Arrange
            var processes = new List<Process>()
            {
                new Process(1, 8, 2),
                new Process(2, 5, 1),
                new Process(3, 2, 7) { Interruption = new Interruption(3, 2) },
                new Process(4, 4, 3),
                new Process(5, 2, 8) { Interruption = new Interruption(4, 3) },
                new Process(6, 4, 2),
                new Process(7, 3, 5),
            };
            var expected = new Dictionary<int, int>
            {
                { 1, 10 },
                { 2, 6 },
                { 3, 21 },
                { 4, 13 },
                { 5, 25 },
                { 6, 8 },
                { 7, 30 },
            };
            var scheduler = new ShortestRemainingTimeFirstScheduler(processes);

            // Act
            scheduler.Process();

            // Assert
            Console.WriteLine(scheduler);
            Assert.Multiple(() =>
            {
                foreach (var process in processes)
                {
                    Assert.IsTrue(process.FinishTime == expected[process.Id]);
                }
            });
        }

        [Test]
        public void RoundRobin()
        {
            // Arrange
            int timeSlice = 2;
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
                { 1, 19 },
                { 2, 15 },
                { 3, 28 },
                { 4, 22 },
                { 5, 30 },
                { 6, 12 },
                { 7, 27 },
            };
            var scheduler = new RoundRobinScheduler(processes, timeSlice);

            // Act
            scheduler.Process();

            // Assert
            Console.WriteLine(scheduler);
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