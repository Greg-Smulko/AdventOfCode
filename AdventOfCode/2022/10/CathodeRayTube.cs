using System.Text;
using FluentAssertions;
using NUnit.Framework;

namespace AdventOfCode;

public class CathodeRayTube
{
    [TestCase("2022/10/CathodeRayTube.Input.txt")]
    [TestCase("2022/10/CathodeRayTube.Sample.Input.txt")]
    public void Test1(string inputPath)
    {
        var lines = File.ReadAllLines(inputPath);

        int regX = 1;
        int cycle = 1;
        int result = 0;

        int ptr = 0;
        bool isExecutingAdd = false;

        while (cycle <= 220)
        {
            if (cycle % 40 == 20)
            {
                result += regX * cycle;
            }

            var line = lines[ptr].Split();
            ptr++;
            if (line[0] == "addx")
            {
                if (isExecutingAdd == false)
                {
                    isExecutingAdd = true;
                    ptr--;
                }
                else
                {
                    regX += int.Parse(line[1]);
                    isExecutingAdd = false;
                }
            }

            cycle++;
        }

        result.Should().Be(13140);
    }

    [TestCase("2022/10/CathodeRayTube.Input.txt")]
    [TestCase("2022/10/CathodeRayTube.Sample.Input.txt")]
    public void Test2(string inputPath)
    {
        var lines = File.ReadAllLines(inputPath);

        int regX = 1;
        int cycle = 0;

        var sb = new StringBuilder(240);

        int ptr = 0;
        bool isExecutingAdd = false;

        while (cycle < 240)
        {
            if (cycle % 40 == 0)
            {
                sb.AppendLine();
            }
            bool isSpriteVisible = cycle % 40 >= regX - 1 && cycle % 40 <= regX + 1;
            sb.Append(isSpriteVisible ? '#' : '.');

            var line = lines[ptr++].Split();
            if (line[0] == "addx")
            {
                if (isExecutingAdd == false)
                {
                    isExecutingAdd = true;
                    ptr--;
                }
                else
                {
                    regX += int.Parse(line[1]);
                    isExecutingAdd = false;
                }
            }

            cycle++;
        }

        Console.Write(sb);
    }
}
