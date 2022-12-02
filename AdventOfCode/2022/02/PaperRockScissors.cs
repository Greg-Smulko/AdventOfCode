using FluentAssertions;
using NUnit.Framework;

namespace AdventOfCode;

public class PaperRockScissors
{
    enum Hand
    {
        Rock = 1,
        Paper = 2,
        Scissors = 3
    }

    [Test]
    public void Test1()
    {
        var lines = File.ReadAllLines("2022/02/PaperRockScissors.Input.txt");

        var total = 0;

        foreach (var line in lines)
        {
            var theirHand = (Hand)(line[0] - 'A' + 1);
            var myHand = (Hand)(line[2] - 'X' + 1);

            var victoryPoints = CalculateVictoryPoints(myHand, theirHand);
            total += victoryPoints + (int)myHand;
        }

        total.Should().BeNegative();
    }

    enum Result
    {
        Defeat = 1,
        Draw = 2,
        Victory = 3
    }

    [Test]
    public void Test2()
    {
        var lines = File.ReadAllLines("2022/02/PaperRockScissors.Input.txt");

        var total = 0;

        foreach (var line in lines)
        {
            var theirHand = (Hand)(line[0] - 'A' + 1);
            var expectedResult = (Result)(line[2] - 'X' + 1);
            var myHand = expectedResult switch
            {
                Result.Draw => theirHand,
                Result.Victory => theirHand switch
                {
                    Hand.Rock => Hand.Paper,
                    Hand.Paper => Hand.Scissors,
                    Hand.Scissors => Hand.Rock
                },
                Result.Defeat => theirHand switch
                {
                    Hand.Rock => Hand.Scissors,
                    Hand.Paper => Hand.Rock,
                    Hand.Scissors => Hand.Paper
                }
            };

            var victoryPoints = CalculateVictoryPoints(myHand, theirHand);
            total += victoryPoints + (int)myHand;
        }

        total.Should().BeNegative();
    }

    private static int CalculateVictoryPoints(Hand myHand, Hand theirHand)
    {
        var victoryPoints = 0;
        if (theirHand == myHand)
        {
            victoryPoints = 3; //draw
        }
        else if (myHand == Hand.Rock && theirHand == Hand.Scissors
                 || myHand == Hand.Paper && theirHand == Hand.Rock
                 || myHand == Hand.Scissors && theirHand == Hand.Paper)
        {
            victoryPoints = 6; //victory
        }

        return victoryPoints;
    }
}
