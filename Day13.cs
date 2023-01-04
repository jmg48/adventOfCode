namespace AdventOfCode;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;

[TestFixture]
public class Day13
{
    [Test]
    public void Test()
    {
        Packet Parse(string input)
        {
            var result = new Packet { List = new List<Packet>() };
            var parsingNumber = false;
            var number = 0;
            foreach (var c in input)
            {
                if (int.TryParse(c.ToString(), out var digit))
                {
                    number = (number * 10) + digit;
                    parsingNumber = true;
                    continue;
                }

                if (parsingNumber)
                {
                    result.List.Add(new Packet { Value = number });
                    parsingNumber = false;
                    number = 0;
                }

                switch (c)
                {
                    case '[':
                        var child = new Packet { Parent = result, List = new List<Packet>() };
                        result.List.Add(child);
                        result = child;
                        break;
                    case ',':
                        break;
                    case ']':
                        result = result.Parent;
                        break;
                    default:
                        throw new NotSupportedException();
                }
            }

            return result.List.Single();
        }

        var result = 0;

        var packets = new List<Packet>();

        var lines = File.ReadAllLines("C:\\git\\advent-of-code\\input13.txt");
        var index = 1;
        for (var i = 0; i < lines.Length; i += 3)
        {
            var left = Parse(lines[i]);
            var right = Parse(lines[i + 1]);

            var comparison = left.CompareTo(right);
            if (comparison < 0)
            {
                result += index;
            }

            index++;

            packets.Add(left);
            packets.Add(right);
        }

        Console.WriteLine($"Part 1: {result}");

        var divider1 = Parse("[[2]]");
        var divider2 = Parse("[[6]]");
        packets.Add(divider1);
        packets.Add(divider2);

        var ordered = packets.OrderBy(x => x).ToList();

        Console.WriteLine($"Part 2: {(ordered.IndexOf(divider1) + 1) * (ordered.IndexOf(divider2) + 1)}");
    }

    private class Packet : IComparable<Packet>
    {
        public int Value { get; set; }

        public List<Packet> List { get; set; }

        public Packet Parent { get; set; }

        public int CompareTo(Packet other)
        {
            if (this.List == null)
            {
                if (other.List == null)
                {
                    return this.Value.CompareTo(other.Value);
                }

                return new Packet { List = new List<Packet> { this } }.CompareTo(other);
            }

            if (other.List == null)
            {
                return this.CompareTo(new Packet { List = new List<Packet> { other } });
            }

            for (var i = 0; i < this.List.Count && i < other.List.Count; i++)
            {
                var elementComparison = this.List[i].CompareTo(other.List[i]);
                if (elementComparison != 0)
                {
                    return elementComparison;
                }
            }

            return this.List.Count.CompareTo(other.List.Count);
        }

        public override string ToString()
        {
            return this.List == null ? this.Value.ToString() : $"[{string.Join(',', this.List)}]";
        }
    }
}