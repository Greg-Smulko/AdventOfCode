using FluentAssertions;
using NUnit.Framework;

namespace AdventOfCode;

public class CampCleanup
{
    [TestCase("2022/04/CampCleanup.Input.txt")]
    [TestCase("2022/04/CampCleanup.Sample.Input.txt")]
    public void Test1(string inputPath)
    {
        var lines = File.ReadAllLines(inputPath);

        var result = 0;

        foreach (var line in lines)
        {
            var ranges = line.Split(",");
            var section1Indexes = ranges[0].Split("-").Select(int.Parse).ToArray();
            var section2Indexes = ranges[1].Split("-").Select(int.Parse).ToArray();
            var section1Range = Enumerable.Range(section1Indexes[0], section1Indexes[1] - section1Indexes[0] + 1);
            var section2Range = Enumerable.Range(section2Indexes[0], section2Indexes[1] - section2Indexes[0] + 1);

            var fullyContains = !section1Range.Except(section2Range).Any() ||
                                !section2Range.Except(section1Range).Any();
            if (fullyContains)
            {
                result++;
            }
        }

        result.Should().Be(2);
    }

    [TestCase("2022/04/CampCleanup.Input.txt")]
    [TestCase("2022/04/CampCleanup.Sample.Input.txt")]
    public void Test2(string inputPath)
    {
        var lines = File.ReadAllLines(inputPath);

        var result = 0;

        foreach (var line in lines)
        {
            var ranges = line.Split(",");
            var section1Indexes = ranges[0].Split("-").Select(int.Parse).ToArray();
            var section2Indexes = ranges[1].Split("-").Select(int.Parse).ToArray();

            var section1Range = Enumerable.Range(section1Indexes[0], section1Indexes[1] - section1Indexes[0] + 1);
            var section2Range = Enumerable.Range(section2Indexes[0], section2Indexes[1] - section2Indexes[0] + 1);

            var isOverlap = section1Range.Count() + section2Range.Count() !=
                            section1Range.Union(section2Range).Distinct().Count();
            if (isOverlap)
            {
                result++;
            }
        }

        result.Should().Be(4);
    }
}
