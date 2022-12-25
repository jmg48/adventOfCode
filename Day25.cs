namespace AdventOfCode;

using System;
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
            .Sum(line => line.Select(c => c switch { '=' => -2, '-' => -1, '0' => 0, '1' => 1, '2' => 2 })
                .Aggregate(default(long), (value1, digit) => (value1 * 5) + digit));

        Console.WriteLine(total);

        var part1 = string.Empty;
        while (total != 0)
        {
            switch (((total % 5) + 5) % 5)
            {
                case 0:
                    part1 += "0";
                    break;
                case 1:
                    part1 += "1";
                    total -= 1;
                    break;
                case 2:
                    part1 += "2";
                    total -= 2;
                    break;
                case 3:
                    part1 += "=";
                    total += 2;
                    break;
                case 4:
                    part1 += "-";
                    total += 1;
                    break;
            }

            total /= 5;
        }

        Console.WriteLine(new string(part1.Reverse().ToArray()));
    }
}