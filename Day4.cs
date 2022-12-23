namespace AdventOfCode;

using System;
using System.IO;
using System.Text.RegularExpressions;
using NUnit.Framework;

[TestFixture]
public class Day4
{
    [Test]
    public void Part1()
    {
        var contains = 0;
        var overlaps = 0;
        using var reader = new StreamReader("C:\\git\\input4.txt");
        while (!reader.EndOfStream)
        {
            var match = Regex.Match(reader.ReadLine(), @"(\d+)-(\d+),(\d+)-(\d+)");
            var a = int.Parse(match.Groups[1].Value);
            var b = int.Parse(match.Groups[2].Value);
            var c = int.Parse(match.Groups[3].Value);
            var d = int.Parse(match.Groups[4].Value);
            if ((a >= c && b <= d) || (c >= a && d <= b))
            {
                contains++;
            }

            if (b >= c && d >= a)
            {
                overlaps++;
            }
        }

        Console.WriteLine(contains);
        Console.WriteLine(overlaps);
    }
}