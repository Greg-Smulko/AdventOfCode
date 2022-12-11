using System.Collections.Immutable;
using FluentAssertions;
using NUnit.Framework;

namespace AdventOfCode;

public class MonkeyInTheMiddle
{
    public class Engine
    {
        private readonly int _howManyRounds;
        private readonly List<Monkey> _monkeys = new();

        public Engine(IEnumerable<MonkeyInitParams> initData, int howManyRounds)
        {
            _howManyRounds = howManyRounds;

            void ThrowTo(int receiver, int item) => _monkeys[receiver].Catch(item);
            foreach (var data in initData)
            {
                _monkeys.Add(new(ThrowTo, data));
            }
        }

        public void Start()
        {
            int currentRound = 0;
            while (currentRound++ < _howManyRounds)
            {
                foreach (var monkey in _monkeys)
                {
                    monkey.Turn();
                }

                PrintRoundSummary(currentRound);
            }
        }

        private void PrintRoundSummary(int currentRound)
        {
            Console.WriteLine($"Round {currentRound} summary:");
            for (var id = 0; id < _monkeys.Count; id++)
            {
                var monkey = _monkeys[id];
                Console.WriteLine($"Monkey {id}: {string.Join(", ", monkey.InspectItems())}");
            }

            Console.WriteLine();
        }

        public IEnumerable<int> GetInspectionCounters()
        {
            return _monkeys.Select(x => x.InspectionCounter);
        }
    }

    public class Monkey
    {
        public Monkey(Action<int, int> throwTo, MonkeyInitParams data)
        {
            Id = data.Id;
            Operation = data.Operation;
            Divisor = data.Divisor;
            MonkeyOnTrue = data.MonkeyOnTrue;
            MonkeyOnFalse = data.MonkeyOnFalse;
            ThrowTo = throwTo;

            foreach (var item in data.StartingItems)
            {
                Items.Enqueue(item);
            }
        }

        public int Id { get; }

        private Queue<int> Items { get; } = new();
        private Func<int, int> Operation { get; }
        private int Divisor { get; }
        private int MonkeyOnTrue { get; }
        private int MonkeyOnFalse { get; }
        private Action<int, int> ThrowTo { get; }

        public int InspectionCounter { get; private set; }

        public void Turn()
        {
            while (Items.TryDequeue(out var item))
            {
                Round(item);
            }
        }

        private void Round(int item)
        {
            var worryLevel = Operation(item) / 3;
            int receiver = worryLevel % Divisor == 0 ? MonkeyOnTrue : MonkeyOnFalse;
            ThrowTo(receiver, worryLevel);
            InspectionCounter++;
            Console.WriteLine($"Monkey {Id} inspected item {item} with worry level {worryLevel} thrown to {receiver}.");
        }

        public void Catch(int item)
        {
            Items.Enqueue(item);
        }

        public IEnumerable<int> InspectItems()
        {
            return Items.ToImmutableArray();
        }
    }

    public class MonkeyInitParams
    {
        public MonkeyInitParams(int id, IEnumerable<int> startingItems, Func<int, int> operation, int divisor, int monkeyOnTrue, int monkeyOnFalse)
        {
            Id = id;
            StartingItems = startingItems;
            Operation = operation;
            Divisor = divisor;
            MonkeyOnTrue = monkeyOnTrue;
            MonkeyOnFalse = monkeyOnFalse;
        }

        public int Id { get; }
        public IEnumerable<int> StartingItems { get; }
        public Func<int, int> Operation { get; }
        public int Divisor { get; }
        public int MonkeyOnTrue { get; }
        public int MonkeyOnFalse { get; }
    }

    [Test]
    public void RunSample()
    {
        int howManyRounds = 20;
        int expectedResult = 10605;
        var initData = new MonkeyInitParams[]
        {
            new(0, new[] { 79, 98 }, i => i * 19, 23, 2, 3),
            new(1, new[] { 54, 65, 75, 74 }, i => i + 6, 19, 2, 0),
            new(2, new[] { 79, 60, 97 }, i => i * i, 13, 1, 3),
            new(3, new[] { 74 }, i => i + 3, 17, 0, 1),
        };

        var engine = new Engine(initData, howManyRounds);
        engine.Start();

        var twoMostActive = engine.GetInspectionCounters().OrderDescending().Take(2).ToArray();
        (twoMostActive[0] * twoMostActive[1]).Should().Be(expectedResult);
    }
    
    [Test]
    public void RunInput()
    {
        int howManyRounds = 20;
        var initData = new MonkeyInitParams[]
        {
            new(0, new[] { 73, 77 }, i => i * 5, 11, 6,5),
            new(1, new[] { 57, 88, 80 }, i => i + 5, 19, 6,0),
            new(2, new[] { 61, 81, 84, 69, 77, 88 }, i => i * 19,5 , 3,1),
            new(3, new[] { 78, 89, 71, 60, 81, 84, 87, 75 }, i => i + 7, 3,  1,0),
            new(4, new[] { 60, 76, 90, 63, 86, 87, 89 }, i => i + 2, 13, 2,7),
            new(5, new[] { 88 }, i => i + 1, 17, 4,7),
            new(6, new[] { 84, 98, 78, 85 }, i => i * i, 7,  5,4),
            new(7, new[] { 98, 89, 78, 73, 71 }, i => i + 4, 2,  3,2),
        };

        var engine = new Engine(initData, howManyRounds);
        engine.Start();

        var twoMostActive = engine.GetInspectionCounters().OrderDescending().Take(2).ToArray();
        (twoMostActive[0] * twoMostActive[1]).Should().Be(56120);
    }
}
