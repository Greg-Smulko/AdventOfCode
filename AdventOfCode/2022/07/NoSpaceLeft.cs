using FluentAssertions;
using NUnit.Framework;

namespace AdventOfCode;

public class NoSpaceLeft
{
    private class Node
    {
        private Node(Node? parent, string name, int size = 0)
        {
            Parent = parent;
            Name = name;
            Size = size;
        }

        public Node? Parent { get; }
        public string Name { get; }
        public int Size { get; }
        public List<Node> Children { get; } = new();

        public bool IsDir => Size == 0;

        public int TotalSize => Size + Children.Sum(x => x.TotalSize);

        public IEnumerable<Node> GetAllChildren() => Children.Union(Children.SelectMany(x => x.GetAllChildren()));

        public void AddChild(string name, int size = 0) => Children.Add(new Node(this, name, size));
        public Node GetChild(string name) => Children.Single(x => x.Name == name);

        public static Node CreateRoot() => new(null, "/");

        public override string ToString()
        {
            return Size == 0
                ? $"dir {Name}"
                : $"{Size} {Name}";
        }
    }

    [TestCase("2022/07/NoSpaceLeft.Input.txt")]
    [TestCase("2022/07/NoSpaceLeft.Sample.Input.txt")]
    public void Test1(string inputPath)
    {
        var lines = File.ReadAllLines(inputPath);

        var root = Node.CreateRoot();
        Node current = root;

        foreach (var line in lines)
        {
            var chunks = line.Split();
            var target = chunks.Last();
            if (line == "$ cd /")
            {
                current = root;
            }
            else if (line.StartsWith("$ cd "))
            {
                switch (target)
                {
                    case "..":
                        current = current.Parent;
                        break;
                    default:
                        current = current.GetChild(target);
                        break;
                }
            }
            else if (line == "$ ls")
            {
                continue;
            }
            else if (line.StartsWith("dir"))
            {
                current.AddChild(target);
            }
            else
            {
                var size = int.Parse(chunks[0]);
                current.AddChild(target, size);
            }
        }
        
        var result = root
            .GetAllChildren()
            .Where(x => x.IsDir && x.TotalSize <= 100_000)
            .Sum(x => x.TotalSize);

        // result.Should().Be(95437);
        
        // ------------- PART 2 ----------------------

        var total = 70000000;
        var unusedNeeded = 30000000;
        var totalUsed = root.TotalSize;
        var neededToFree = (totalUsed + unusedNeeded) - total;

        // neededToFree.Should().Be(8381165);

        var result2 = root
            .GetAllChildren().Union(new []{ root })
            .Where(x => x.IsDir)
            .Where(x => x.TotalSize >= neededToFree)
            .OrderBy(x => x.TotalSize)
            .First();

        result2.TotalSize.Should().BeNegative();
    }
}
