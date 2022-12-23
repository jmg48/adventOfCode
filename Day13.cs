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
            for (var i = 0; i < input.Length; i++)
            {
                var c = input[i];
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
                        var n = c.ToString();
                        while (int.TryParse(input[i + 1].ToString(), out _))
                        {
                            i++;
                            n += input[i];
                        }

                        result.List.Add(new Packet { Value = int.Parse(n) });
                        break;
                }
            }

            return result.List.Single();
        }

        var result = 0;

        var packets = new List<Packet>();

        var lines = File.ReadAllLines("C:\\git\\input13.txt");
        var index = 1;
        for (var i = 0; i < lines.Length; i += 3)
        {
            var left = Parse(lines[i]);
            var right = Parse(lines[i + 1]);
            Console.WriteLine(left);
            Console.WriteLine(right);

            var comparison = left.CompareTo(right);
            if (comparison < 0)
            {
                result += index;
            }

            index++;

            packets.Add(left);
            packets.Add(right);
        }

        Console.WriteLine(result);

        var divider1 = Parse("[[2]]");
        var divider2 = Parse("[[6]]");
        packets.Add(divider1);
        packets.Add(divider2);

        var ordered = packets.OrderBy(x => x).ToList();

        Console.WriteLine((ordered.IndexOf(divider1) + 1) * (ordered.IndexOf(divider2) + 1));
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