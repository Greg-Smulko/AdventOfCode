using FluentAssertions;
using NUnit.Framework;

namespace AdventOfCode;

public class RopeBridge
{
    private record Point(int X, int Y)
    {
        public Point Right() => this with { X = X + 1 };
        public Point Left() => this with { X = X - 1 };
        public Point Up() => this with { Y = Y + 1 };
        public Point Down() => this with { Y = Y - 1 };

        public override string ToString()
        {
            return $"<{X}, {Y}>";
        }
    }

    [TestCase("2022/09/RopeBridge.Input.txt")]
    [TestCase("2022/09/RopeBridge.Sample.Input.txt")]
    public void Test1(string inputPath)
    {
        var lines = File.ReadAllLines(inputPath);

        var visited = new HashSet<Point>();
        var head = new Point(0, 0);
        var tail = new Point(0, 0);
        foreach (var line in lines)
        {
            var direction = char.Parse(line.Split()[0]);
            var moves = int.Parse(line.Split()[1]);

            for (int i = 0; i < moves; i++)
            {
                head = direction switch
                {
                    'R' => head.Right(),
                    'L' => head.Left(),
                    'U' => head.Up(),
                    'D' => head.Down(),
                    _ => throw new InvalidOperationException($"Unexpected direction: {direction}")
                };

                if (tail.X + 2 == head.X)
                {
                    tail = tail.Right() with { Y = head.Y};
                }
                else if (tail.X - 2 == head.X)
                {
                    tail = tail.Left() with { Y = head.Y};
                }
                else if (tail.Y + 2 == head.Y)
                {
                    tail = tail.Up() with { X = head.X};
                }
                else if (tail.Y - 2 == head.Y)
                {
                    tail = tail.Down() with { X = head.X};
                }

                visited.Add(tail);
            }
        }

        visited.Should().HaveCount(13);
    }


    [TestCase("2022/09/RopeBridge.Input.txt", 2, 5960)]
    [TestCase("2022/09/RopeBridge.Sample.Input.txt", 2, 13)]
    [TestCase("2022/09/RopeBridge.Sample2.Input.txt", 10, 36)]
    [TestCase("2022/09/RopeBridge.Input.txt", 10, 0)]
    public void Test2(string inputPath, int length, int expected)
    {
        var lines = File.ReadAllLines(inputPath);

        var visited = new HashSet<Point>();
        var snake = new Point[length];
        for (int i = 0; i < length; i++)
        {
            snake[i] = new(0,0);
        }
        var head = snake[0];
        foreach (var line in lines)
        {
            var direction = char.Parse(line.Split()[0]);
            var moves = int.Parse(line.Split()[1]);

            for (int i = 0; i < moves; i++)
            {
                head = direction switch
                {
                    'R' => head.Right(),
                    'L' => head.Left(),
                    'U' => head.Up(),
                    'D' => head.Down(),
                    _ => throw new InvalidOperationException($"Unexpected direction: {direction}")
                };
                snake[0] = head;

                for (int j = 1; j < length; j++)
                {
                    var elem = snake[j];
                    var prev = snake[j - 1];
                    if (elem.X + 2 <= prev.X)
                    {
                        elem = elem.Right() with { Y = prev.Y};
                    }
                    else if (elem.X - 2 >= prev.X)
                    {
                        elem = elem.Left() with { Y = prev.Y};
                    }
                    else if (elem.Y + 2 <= prev.Y)
                    {
                        elem = elem.Up() with { X = prev.X};
                    }
                    else if (elem.Y - 2 >= prev.Y)
                    {
                        elem = elem.Down() with { X = prev.X};
                    }

                    snake[j] = elem;

                    if (j == length - 1)
                    {
                        visited.Add(elem);
                    }
                }
            }
        }

        visited.Should().HaveCount(expected);
    }
}
