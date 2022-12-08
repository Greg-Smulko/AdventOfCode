using FluentAssertions;
using NUnit.Framework;

namespace AdventOfCode;

public class TreeHouse
{
    private record Point(int X, int Y);

    [TestCase("2022/08/TreeHouse.Input.txt")]
    [TestCase("2022/08/TreeHouse.Sample.Input.txt")]
    public void Test1(string inputPath)
    {
        var lines = File.ReadAllLines(inputPath);
        var size = lines.Length; // assuming square
        var grid = new Dictionary<Point, int>();
        for (var y = 0; y < size; y++)
        {
            for (var x = 0; x < size; x++)
            {
                var height = int.Parse(lines[y][x].ToString());
                grid.Add(new(y, x), height);
            }
        }

        var visibleTrees = size * 4 - 4;

        for (var y = 1; y < size - 1; y++)
        {
            for (var x = 1; x < size - 1; x++)
            {
                var height = grid[new(x, y)];

                if (!(grid.Any(p => p.Key.X == x && p.Key.Y < y && p.Value >= height)
                      && grid.Any(p => p.Key.X == x && p.Key.Y > y && p.Value >= height)
                      && grid.Any(p => p.Key.X < x && p.Key.Y == y && p.Value >= height)
                      && grid.Any(p => p.Key.X > x && p.Key.Y == y && p.Value >= height)))
                {
                    visibleTrees++;
                }
            }
        }

        visibleTrees.Should().Be(21);
    }


    [TestCase("2022/08/TreeHouse.Input.txt")]
    [TestCase("2022/08/TreeHouse.Sample.Input.txt")]
    public void Test2(string inputPath)
    {
        var lines = File.ReadAllLines(inputPath);
        var size = lines.Length; // assuming square
        var grid = new Dictionary<Point, int>();
        for (var y = 0; y < size; y++)
        {
            for (var x = 0; x < size; x++)
            {
                var height = int.Parse(lines[y][x].ToString());
                grid.Add(new(y, x), height);
            }
        }

        var bestScenicScore = 0;

        for (var y = 1; y < size - 1; y++)
        {
            for (var x = 1; x < size - 1; x++)
            {
                var height = grid[new(x, y)];

                int left = 0, right = 0, top = 0, down = 0;
                while (grid.TryGetValue(new(x - left - 1, y), out var h) && left++ >= 0 && h < height) ;
                while (grid.TryGetValue(new(x + right + 1, y), out var h) && right++ >= 0 && h < height) ;
                while (grid.TryGetValue(new(x, y - top - 1), out var h) && top++ >= 0 && h < height) ;
                while (grid.TryGetValue(new(x, y + down + 1), out var h) && down++ >= 0 && h < height) ;

                var score = left * right * top * down;
                if (score > bestScenicScore)
                {
                    bestScenicScore = score;
                }
            }
        }

        bestScenicScore.Should().Be(8);
    }
}
