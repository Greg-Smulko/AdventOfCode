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

            void ThrowTo(int receiver, ulong item) => _monkeys[receiver].Catch(item);
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
                // Console.WriteLine($"Monkey {id}: {string.Join(", ", monkey.InspectItems())}");
                Console.WriteLine($"Monkey {id} inspected items {monkey.InspectionCounter} times.");
            }

            Console.WriteLine();
        }

        public IEnumerable<ulong> GetInspectionCounters()
        {
            return _monkeys.Select(x => x.InspectionCounter);
        }
    }

    public class Monkey
    {
        public Monkey(Action<int, ulong> throwTo, MonkeyInitParams data)
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

        private Queue<ulong> Items { get; } = new();
        private Func<ulong, ulong> Operation { get; }
        private int Divisor { get; }
        private int MonkeyOnTrue { get; }
        private int MonkeyOnFalse { get; }
        private Action<int, ulong> ThrowTo { get; }

        public ulong InspectionCounter { get; private set; }

        public void Turn()
        {
            while (Items.TryDequeue(out var item))
            {
                Round(item);
            }
        }

        private void Round(ulong item)
        {
            // part 1
            // var worryLevel = Math.Floor((decimal)Operation(item) / 3);
            
            // part 2
            //var worryLevel = Operation(item) % (23*19*13*17); // for sample
            var worryLevel = Operation(item) % (11*19*5*3*13*17*7*2); // for puzzle

            int receiver = worryLevel % (ulong)Divisor == 0 ? MonkeyOnTrue : MonkeyOnFalse;
            ThrowTo(receiver, (ulong)worryLevel);
            InspectionCounter++;
            // Console.WriteLine($"Monkey {Id} inspected item {item} with worry level {worryLevel} thrown to {receiver}.");
        }

        public void Catch(ulong item)
        {
            Items.Enqueue(item);
        }

        public IEnumerable<ulong> InspectItems()
        {
            return Items.ToImmutableArray();
        }
    }

    public class MonkeyInitParams
    {
        public MonkeyInitParams(int id, IEnumerable<ulong> startingItems, Func<ulong, ulong> operation, int divisor, int monkeyOnTrue, int monkeyOnFalse)
        {
            Id = id;
            StartingItems = startingItems;
            Operation = operation;
            Divisor = divisor;
            MonkeyOnTrue = monkeyOnTrue;
            MonkeyOnFalse = monkeyOnFalse;
        }

        public int Id { get; }
        public IEnumerable<ulong> StartingItems { get; }
        public Func<ulong, ulong> Operation { get; }
        public int Divisor { get; }
        public int MonkeyOnTrue { get; }
        public int MonkeyOnFalse { get; }
    }

    [Test]
    public void RunSample()
    {
        // part 1
        // int howManyRounds = 20;
        // ulong expectedResult = 10605;
        
        // part 2
        int howManyRounds = 10000;
        ulong expectedResult = 2713310158;
        
        var initData = new MonkeyInitParams[]
        {
            new(0, new ulong[] { 79, 98 }, i => i * 19, 23, 2, 3),
            new(1, new ulong[] { 54, 65, 75, 74 }, i => i + 6, 19, 2, 0),
            new(2, new ulong[] { 79, 60, 97 }, i => i * i, 13, 1, 3),
            new(3, new ulong[] { 74 }, i => i + 3, 17, 0, 1),
        };

        var engine = new Engine(initData, howManyRounds);
        engine.Start();

        var twoMostActive = engine.GetInspectionCounters().OrderDescending().Take(2).ToArray();
        (twoMostActive[0] * twoMostActive[1]).Should().Be(expectedResult);
    }
    
    [Test]
    public void RunInput()
    {
        //int howManyRounds = 20; // for part 1
        int howManyRounds = 10000; // for part 2
        var initData = new MonkeyInitParams[]
        {
            new(0, new ulong[] { 73, 77 }, i => i * 5, 11, 6,5),
            new(1, new ulong[] { 57, 88, 80 }, i => i + 5, 19, 6,0),
            new(2, new ulong[] { 61, 81, 84, 69, 77, 88 }, i => i * 19,5 , 3,1),
            new(3, new ulong[] { 78, 89, 71, 60, 81, 84, 87, 75 }, i => i + 7, 3,  1,0),
            new(4, new ulong[] { 60, 76, 90, 63, 86, 87, 89 }, i => i + 2, 13, 2,7),
            new(5, new ulong[] { 88 }, i => i + 1, 17, 4,7),
            new(6, new ulong[] { 84, 98, 78, 85 }, i => i * i, 7,  5,4),
            new(7, new ulong[] { 98, 89, 78, 73, 71 }, i => i + 4, 2,  3,2),
        };

        var engine = new Engine(initData, howManyRounds);
        engine.Start();

        var twoMostActive = engine.GetInspectionCounters().OrderDescending().Take(2).ToArray();
        (twoMostActive[0] * twoMostActive[1]).Should().Be(56120);
    }
}
