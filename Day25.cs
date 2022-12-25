namespace AdventOfCode;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;

[TestFixture]
internal class Day25
{
    [Test]
    public void Test()
    {
        long Val(char c) => c switch { '=' => -2L, '-' => -1L, '0' => 0L, '1' => 1L, '2' => 2L };

        var total = File.ReadLines("C:\\git\\advent-of-code\\input25.txt")
            .Sum(line => line.Select(Val).Aggregate(0L, (value, digit) => (value * 5) + digit));

        var result = new Stack<char>();
        while (total != 0)
        {
            result.Push((((total % 5) + 5) % 5) switch { 0 => '0', 1 => '1', 2 => '2', 3 => '=', 4 => '-' });
            total = (total - Val(result.Peek())) / 5;
        }

        Console.WriteLine(new string(result.ToArray()));
    }
}