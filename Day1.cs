namespace AdventOfCode;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;

[TestFixture]
public class Day1
{
    [Test]
    public void Test()
    {
        Console.WriteLine(File.ReadLines("C:\\git\\advent-of-code\\input.txt")
            .Aggregate(
                (0, 0),
                (a, s) => string.IsNullOrWhiteSpace(s)
                    ? (Math.Max((int)a.Item1, (int)a.Item2), 0)
                    : (a.Item1, a.Item2 + int.Parse((string)s)),
                a => a.Item1));

        var top3 = File.ReadLines("C:\\git\\advent-of-code\\input.txt")
            .Aggregate(
                (new List<int> { 0, 0, 0 }, 0),
                (a, s) => string.IsNullOrWhiteSpace(s)
                    ? (a.Item1.Concat(new[] { a.Item2 }).OrderByDescending(i => i).Take(3).ToList(), 0)
                    : (a.Item1, a.Item2 + int.Parse(s)),
                a => a.Item1);

        var totals = new List<int>();

        var currentRunningTotal = 0;
        using var reader = new StreamReader("C:\\git\\advent-of-code\\input.txt");
        while (!reader.EndOfStream)
        {
            var line = reader.ReadLine();
            if (string.IsNullOrWhiteSpace(line))
            {
                totals.Add(currentRunningTotal);
                currentRunningTotal = 0;
            }
            else
            {
                currentRunningTotal += int.Parse(line);
            }
        }

        Console.WriteLine(totals.OrderByDescending(x => x).Take(3).Sum());
    }
}