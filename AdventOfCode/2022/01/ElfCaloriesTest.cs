using FluentAssertions;
using NUnit.Framework;

namespace AdventOfCode;

public class ElfCaloriesTest
{
    [Test]
    public void Test1()
    {
        var result = ElfCalories.FindMaxTotalCalories();
        result.Should().BeNegative();
    }

    [Test]
    public void Test2()
    {
        var result = ElfCalories.FindMaxThreeTotalCalories();
        result.Should().BeNegative();
    }
}
