using FluentAssertions;
using NUnit.Framework;

namespace AdventOfCode;

public class TuningTrouble
{
    [TestCase("2022/06/TuningTrouble.Input.txt", 0)]
    [TestCase("bvwbjplbgvbhsrlpgdmjqwftvncz", 5)]
    [TestCase("nppdvjthqldpwncqszvftbrmjlhg", 6)]
    [TestCase("nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg", 10)]
    [TestCase("zcfzfwzzqfrljwzlrfnpqdbhtmscgvjw", 11)]
    public void Test1(string input, int expected)
    {
        if (expected == 0)
        {
            input = File.ReadAllText(input);
        }

        const int packetLength = 14;
        int i = packetLength;

        for (; i < input.Length; i++)
        {
            if (input[(i - packetLength)..i].Distinct().Count() == packetLength)
            {
                break;
            }
        }

        i.Should().Be(expected);
    }
    
}
