namespace AdventOfCode;

public static class ElfCalories 
{
    public static double FindMaxTotalCalories()
    {
        double max = 0, total = 0;
        var lines = File.ReadAllLines("2022/01/ElfCaloriesInput.txt");

        foreach (var line in lines)
        {
            if (string.IsNullOrEmpty(line))
            {
                if (total > max)
                {
                    max = total;
                }
                total = 0;
                continue;
            }

            total += double.Parse(line);
        }

        return max;
    }

    public static double FindMaxThreeTotalCalories()
    {
        var totalPerElf = new List<double>();
        double total = 0;
        
        var lines = File.ReadAllLines("2022/01/ElfCaloriesInput.txt");

        foreach (var line in lines)
        {
            if (string.IsNullOrEmpty(line))
            {
                totalPerElf.Add(total);
                total = 0;
                continue;
            }

            total += double.Parse(line);
        }

        return totalPerElf.OrderDescending().Take(3).Sum();
    }
}
