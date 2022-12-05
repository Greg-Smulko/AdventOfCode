using FluentAssertions;
using NUnit.Framework;

namespace AdventOfCode;

public class SupplyStacks
{
    [TestCase("2022/05/SupplyStacks.Input.txt", 9, 8)]
    [TestCase("2022/05/SupplyStacks.Sample.Input.txt", 3, 3)]
    public void Test1(string inputPath, int noOfStacks, int highestStackSize)
    {
        var lines = File.ReadAllLines(inputPath);

        var result = string.Empty;
        var stacks = new Stack<char>[noOfStacks];
        for (int i = 0; i < noOfStacks; i++)
        {
            stacks[i] = new Stack<char>();
        }

        for (int i = highestStackSize - 1; i >= 0; i--)
        {
            var line = lines[i];
            for (int j = 0; j < noOfStacks; j++)
            {
                var crate = line[j * 4 + 1];
                if (crate != ' ')
                {
                    stacks[j].Push(crate);
                }
            }
        }

        foreach (var line in lines.Skip(highestStackSize + 2))
        {
            var nums = line
                .Replace("move ", "")
                .Replace("from ", "")
                .Replace("to ", "")
                .Split(' ');
            var moves = int.Parse(nums[0]);
            var from = int.Parse(nums[1]) - 1;
            var to = int.Parse(nums[2]) - 1;

            for (int i = 0; i < moves; i++)
            {
                var crate = stacks[from].Pop();
                stacks[to].Push(crate);
            }
        }

        foreach (var stack in stacks)
        {
            result += stack.Peek();
        }

        result.Should().Be("CMZ");
    }
    
    [TestCase("2022/05/SupplyStacks.Input.txt", 9, 8)]
    [TestCase("2022/05/SupplyStacks.Sample.Input.txt", 3, 3)]
    public void Test2(string inputPath, int noOfStacks, int highestStackSize)
    {
        var lines = File.ReadAllLines(inputPath);

        var result = string.Empty;
        var stacks = new Stack<char>[noOfStacks];
        for (int i = 0; i < noOfStacks; i++)
        {
            stacks[i] = new Stack<char>();
        }

        for (int i = highestStackSize - 1; i >= 0; i--)
        {
            var line = lines[i];
            for (int j = 0; j < noOfStacks; j++)
            {
                var crate = line[j * 4 + 1];
                if (crate != ' ')
                {
                    stacks[j].Push(crate);
                }
            }
        }

        foreach (var line in lines.Skip(highestStackSize + 2))
        {
            var nums = line
                .Replace("move ", "")
                .Replace("from ", "")
                .Replace("to ", "")
                .Split(' ');
            var moves = int.Parse(nums[0]);
            var from = int.Parse(nums[1]) - 1;
            var to = int.Parse(nums[2]) - 1;

            var tmpStack = new Stack<char>();
            for (int i = 0; i < moves; i++)
            {
                var crate = stacks[from].Pop();
                tmpStack.Push(crate);
            }

            while (tmpStack.Any())
            {
                stacks[to].Push(tmpStack.Pop());
            }
        }

        foreach (var stack in stacks)
        {
            result += stack.Peek();
        }

        result.Should().Be("MCD");
    }
}
