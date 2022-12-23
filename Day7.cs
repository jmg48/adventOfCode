namespace AdventOfCode;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;

[TestFixture]
public class Day7
{
    [Test]
    public void Test()
    {
        var dirSizes = new ConcurrentDictionary<string, int>();

        var paths = new Stack<string>(new[] { string.Empty });
        foreach (var line in File.ReadLines("C:\\git\\input7.txt"))
        {
            if (line.StartsWith("$ cd "))
            {
                var arg = line[5..];
                switch (arg)
                {
                    case "/":
                        paths.Clear();
                        paths.Push(string.Empty);
                        break;
                    case "..":
                        paths.Pop();
                        break;
                    default:
                        paths.Push($"{paths.Peek()}/{arg}");
                        break;
                }
            }
            else if (line.StartsWith("$ ls"))
            {
            }
            else
            {
                var ls = line.Split(' ');
                if (ls[0] != "dir")
                {
                    var fileSize = int.Parse(ls[0]);
                    foreach (var path in paths)
                    {
                        dirSizes.AddOrUpdate(
                            path,
                            _ => fileSize,
                            (_, dirSize) => dirSize + fileSize);
                    }
                }
            }
        }

        var result1 = dirSizes.Values.Where(i => i <= 100000).Sum();
        Console.WriteLine($"Part One: {result1}");

        var totalSize = dirSizes[string.Empty];
        var freeSpace = 70000000 - totalSize;
        var additionalRequired = 30000000 - freeSpace;

        var result2 = dirSizes.Values.OrderBy(i => i).First(i => i >= additionalRequired);
        Console.WriteLine($"Part Two: {result2}");
    }
}