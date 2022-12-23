namespace AdventOfCode;

using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

[TestFixture]
public class Day11
{
    [TestCase(1)]
    [TestCase(2)]
    public void Test(int part)
    {
        var monkeys = new List<Monkey>
        {
            new(new List<long> { 54, 89, 94 }, old => old * 7, n => n % 17 == 0 ? 5 : 3),
            new(new List<long> { 66, 71 }, old => old + 4, n => n % 3 == 0 ? 0 : 3),
            new(new List<long> { 76, 55, 80, 55, 55, 96, 78 }, old => old + 2, n => n % 5 == 0 ? 7 : 4),
            new(new List<long> { 93, 69, 76, 66, 89, 54, 59, 94 }, old => old + 7, n => n % 7 == 0 ? 5 : 2),
            new(new List<long> { 80, 54, 58, 75, 99 }, old => old * 17, n => n % 11 == 0 ? 1 : 6),
            new(new List<long> { 69, 70, 85, 83 }, old => old + 8, n => n % 19 == 0 ? 2 : 7),
            new(new List<long> { 89 }, old => old + 6, n => n % 2 == 0 ? 0 : 1),
            new(new List<long> { 62, 80, 58, 57, 93, 56 }, old => old * old, n => n % 13 == 0 ? 6 : 4),
        };

        var rounds = part == 1 ? 20 : 10000;
        Func<long, long> reduce = part == 1 ? n => n / 3 : n => n % (2 * 3 * 5 * 7 * 11 * 13 * 17 * 19);

        for (var round = 0; round < rounds; round++)
        {
            foreach (var monkey in monkeys)
            {
                while (monkey.Items.Count > 0)
                {
                    monkey.Inspections++;
                    var item = monkey.Items.Dequeue();
                    item = reduce(monkey.Operation(item));
                    monkeys[monkey.ThrowTo(item)].Items.Enqueue(item);
                }
            }
        }

        var result = monkeys.Select(monkey => monkey.Inspections).OrderByDescending(i => i).Take(2).Aggregate((a, b) => a * b);
        Console.WriteLine(result);
    }

    private class Monkey
    {
        public Monkey(List<long> initialItems, Func<long, long> operation, Func<long, int> throwTo)
        {
            this.Items = new Queue<long>(initialItems);
            this.Operation = operation;
            this.ThrowTo = throwTo;
        }

        public Queue<long> Items { get; }

        public Func<long, long> Operation { get; }

        public Func<long, int> ThrowTo { get; }

        public long Inspections { get; set; }
    }
}