using FluentAssertions;
using NUnit.Framework;

namespace AdventOfCode;

public class Rucksacks
{
    [TestCase("2022/03/Rucksacks.Input.txt")]
    [TestCase("2022/03/Rucksacks.Sample.Input.txt")]
    public void Test1(string inputPath)
    {
        var lines = File.ReadAllLines(inputPath);

        var result = 0;

        foreach (var line in lines)
        {
            var mid = line.Length / 2;
            var first = line.Take(mid);
            var second = line.Skip(mid);
            var overlap = first.Distinct().Intersect(second.Distinct()).Single();

            result += (char.IsUpper(overlap) ? overlap - 'A' + 26 : overlap - 'a') + 1;
        }

        result.Should().Be(157);
    }

    [TestCase("2022/03/Rucksacks.Input.txt")]
    [TestCase("2022/03/Rucksacks.Sample.Input.txt")]
    public void Test2(string inputPath)
    {
        var lines = File.ReadAllLines(inputPath);

        var result = 0;

        for (int i = 0; i < lines.Length; i+=3)
        {
            var line1 = lines[i];
            var line2 = lines[i+1];
            var line3 = lines[i+2];
            var overlap = line1.Distinct().Intersect(line2.Distinct()).Intersect(line3.Distinct()).Single();

            result += (char.IsUpper(overlap) ? overlap - 'A' + 26 : overlap - 'a') + 1;
        }
        
        result.Should().Be(70);
    }
}
