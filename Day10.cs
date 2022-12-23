namespace AdventOfCode;

using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;

[TestFixture]
public class Day10
{
    [Test]
    public void Test()
    {
        var cycle = 1;
        var x = 1;
        var s = new List<int> { 0, 1 };
        foreach (var line in File.ReadLines("C:\\git\\input10.txt"))
        {
            switch (line[..4])
            {
                case "noop":
                    cycle++;
                    s.Add(x);
                    break;
                case "addx":
                    cycle++;
                    s.Add(x);
                    cycle++;
                    x += int.Parse(line[5..]);
                    s.Add(x);
                    break;
                default:
                    throw new NotSupportedException();
            }
        }

        int F(int i) => s[i] * i;

        Console.WriteLine(F(20) + F(60) + F(100) + F(140) + F(180) + F(220));

        for (var i = 1; i <= 240; i++)
        {
            var xPos = (i - 1) % 40;
            if (xPos == 0)
            {
                Console.WriteLine();
            }

            var spritePos = s[i];
            if (xPos == spritePos - 1 || xPos == spritePos || xPos == spritePos + 1)
            {
                Console.Write("#");
            }
            else
            {
                Console.Write(".");
            }
        }
    }
}