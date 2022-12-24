namespace AdventOfCode;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;

[TestFixture]
public class Day3
{
    [Test]
    public void Part1()
    {
        var total = 0;
        using var reader = new StreamReader("C:\\git\\advent-of-code\\input3.txt");
        while (!reader.EndOfStream)
        {
            var line = reader.ReadLine();
            var bag1 = new HashSet<char>(line.Take(line.Length / 2));
            var c = line.Skip(line.Length / 2).Where(c => bag1.Contains(c)).Distinct().Single();
            var i = (int)c;
            var score = i <= 'Z' ? (i - 'A' + 27) : (i - 'a' + 1);
            total += score;
        }

        Console.WriteLine(total);
    }

    [Test]
    public void Part2()
    {
        var total = 0;
        using var reader = new StreamReader("C:\\git\\advent-of-code\\input3.txt");
        while (!reader.EndOfStream)
        {
            var line1 = new HashSet<char>(reader.ReadLine());
            line1.IntersectWith(reader.ReadLine());
            line1.IntersectWith(reader.ReadLine());
            var c = line1.Single();
            var i = (int)c;
            var score = i <= 'Z' ? (i - 'A' + 27) : (i - 'a' + 1);
            total += score;
        }

        Console.WriteLine(total);
    }
}