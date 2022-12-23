namespace AdventOfCode;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using NUnit.Framework;

[TestFixture]
public class Day5
{
    [TestCase(1)]
    [TestCase(2)]
    public void Test(int part)
    {
        using var reader = new StreamReader("C:\\git\\input5.txt");
        var stackLines = new List<string>();
        while (!reader.EndOfStream)
        {
            var line = reader.ReadLine();
            if (string.IsNullOrWhiteSpace(line))
            {
                break;
            }

            stackLines.Add(line);
        }

        var stacks = new Dictionary<int, Stack<char>>();
        foreach (var i in stackLines.Last().Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).Select(int.Parse))
        {
            stacks.Add(i, new Stack<char>());
        }

        foreach (var stackLine in stackLines.AsEnumerable().Reverse().Skip(1))
        {
            foreach (var stack in stacks.Keys)
            {
                var val = stackLine[(stack * 4) - 3];
                if (val != ' ')
                {
                    stacks[stack].Push(val);
                }
            }
        }

        while (!reader.EndOfStream)
        {
            var line = Regex.Match(reader.ReadLine(), @"move (\d+) from (\d+) to (\d+)");
            var n = int.Parse(line.Groups[1].Value);
            var from = int.Parse(line.Groups[2].Value);
            var to = int.Parse(line.Groups[3].Value);

            switch (part)
            {
                case 1:
                    for (var i = 0; i < n; i++)
                    {
                        stacks[to].Push(stacks[from].Pop());
                    }

                    break;
                case 2:
                    var crane = new Stack<char>();
                    for (var i = 0; i < n; i++)
                    {
                        crane.Push(stacks[from].Pop());
                    }

                    for (var i = 0; i < n; i++)
                    {
                        stacks[to].Push(crane.Pop());
                    }

                    break;
                default:
                    throw new NotSupportedException();
            }
        }

        foreach (var kvp in stacks.OrderBy(stack => stack.Key))
        {
            Console.Write(kvp.Value.Peek());
        }

        Console.WriteLine();
    }
}