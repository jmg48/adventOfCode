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
        var total = File.ReadAllLines("C:\\git\\advent-of-code\\input25.txt")
            .Sum(line => line.Select(c => c switch
                {
                    '=' => -2,
                    '-' => -1,
                    '0' => 0,
                    '1' => 1,
                    '2' => 2,
                })
                .Aggregate(0L, (value, digit) => (value * 5) + digit));

        var result = new Stack<char>();
        while (total != 0)
        {
            var (digit, value) = (((total % 5) + 5) % 5) switch
            {
                0 => ('0', 0),
                1 => ('1', 1),
                2 => ('2', 2),
                3 => ('=', -2),
                4 => ('-', -1),
            };

            result.Push(digit);
            total = (total - value) / 5;
        }

        Console.WriteLine(new string(result.ToArray()));
    }
}