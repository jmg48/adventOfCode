namespace AdventOfCode;

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using NUnit.Framework;

[TestFixture]
public class Day20
{
    [TestCase(1)]
    [TestCase(2)]
    public void Test(int part)
    {
        var rounds = part switch { 1 => 1, 2 => 10 };
        var multiple = part switch { 1 => 1, 2 => 811589153 };

        int PosMod(int n, int m) => ((n % m) + m) % m;

        var digits = File.ReadLines("C:\\git\\advent-of-code\\input20.txt").Select(i => new Linked<long>(long.Parse((string)i) * multiple)).ToList();

        var listSize = digits.Count;
        for (var i = 0; i < listSize; i++)
        {
            digits[i].Prev = digits[PosMod(i - 1, listSize)];
            digits[i].Next = digits[PosMod(i + 1, listSize)];
        }

        for (var round = 0; round < rounds; round++)
        {
            foreach (var digit in digits)
            {
                var steps = digit.Value % (listSize - 1);
                steps = (steps + (listSize - 1)) % (listSize - 1);

                for (var i = 0; i < steps; i++)
                {
                    var a = digit.Prev;
                    var c = digit.Next;
                    var d = c.Next;
                    a.Next = c;
                    c.Prev = a;
                    c.Next = digit;
                    digit.Prev = c;
                    digit.Next = d;
                    d.Prev = digit;
                }
            }
        }

        var cursor = digits.Single(digit => digit.Value == 0);

        long result = 0;
        for (var i = 0; i < 3; i++)
        {
            for (var j = 0; j < 1000; j++)
            {
                cursor = cursor.Next;
            }

            result += cursor.Value;
        }

        Console.WriteLine(result);
    }

    [DebuggerDisplay("{Prev.Value,5} >> {Value,5} >> {Next.Value,5}")]
    private class Linked<T>
    {
        public Linked(T value) => this.Value = value;

        public T Value { get; }

        public Linked<T> Prev { get; set; }

        public Linked<T> Next { get; set; }
    }
}