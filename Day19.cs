namespace AdventOfCode;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using NUnit.Framework;

[TestFixture]
public class Day19
{
    [TestCase(1)]
    [TestCase(2)]
    public void Test(int part)
    {
        var timer = new Stopwatch();
        timer.Start();

        var minutes = part switch { 1 => 24, 2 => 32 };
        var take = part switch { 1 => 30, 2 => 3 };

        var blueprints = File.ReadLines("C:\\git\\advent-of-code\\input19.txt")
            .Select(line => Regex.Match(
                line,
                @"Blueprint (\d+): Each ore robot costs (\d+) ore. Each clay robot costs (\d+) ore. Each obsidian robot costs (\d+) ore and (\d+) clay. Each geode robot costs (\d+) ore and (\d+) obsidian."))
            .Select(match => match.Groups.Values.Skip(1).Select(group => int.Parse(group.Value)).ToList())
            .Select(values => (
                Id: values[0],
                Ore: new Resource(values[1], 0, 0, 0),
                Clay: new Resource(values[2], 0, 0, 0),
                Obsidian: new Resource(values[3], values[4], 0, 0),
                Geode: new Resource(values[5], 0, values[6], 0))).Take(take).ToList();

        var result1 = 0;
        var result2 = 1;
        foreach (var (id, oreCost, clayCost, obsidianCost, geodeCost) in blueprints)
        {
            var bestScore = new Resource(0, 0, 0, 0);
            var bestPath = new List<(Resource Bank, Resource Robots)>();
            var path = new Stack<(Resource Bank, Resource Robots)>();

            var maxOreCost = new[] { oreCost, clayCost, obsidianCost, geodeCost }.Max(cost => cost.Ore);

            void Search(int t, Resource bank, Resource robots)
            {
                var newBank = bank + robots;

                if (t == minutes - 1)
                {
                    if (newBank.Geode > bestScore.Geode || (bestScore.Geode == 0 && newBank.Obsidian > bestScore.Obsidian))
                    {
                        bestScore = newBank;
                        bestPath = path.Reverse().ToList();
                    }

                    return;
                }

                if (bestPath.Count > 0)
                {
                    var best = bestPath[t];
                    if (bank.Ore <= best.Bank.Ore && bank.Clay <= best.Bank.Clay &&
                        bank.Obsidian <= best.Bank.Obsidian && bank.Geode <= best.Bank.Geode &&
                        robots.Ore <= best.Robots.Ore && robots.Clay <= best.Robots.Clay &&
                        robots.Obsidian <= best.Robots.Obsidian && robots.Geode <= best.Robots.Geode)
                    {
                        return;
                    }
                }

                path.Push((bank, robots));

                var tryGeode = bank - geodeCost;
                if (!tryGeode.IsOverdrawn())
                {
                    Search(t + 1, tryGeode + robots, robots with { Geode = robots.Geode + 1 });
                    path.Pop();
                    return;
                }

                var tryObsidian = bank - obsidianCost;
                if (!tryObsidian.IsOverdrawn())
                {
                    Search(t + 1, tryObsidian + robots, robots with { Obsidian = robots.Obsidian + 1 });
                    path.Pop();
                    return;
                }

                var tryClay = bank - clayCost;
                if (!tryClay.IsOverdrawn())
                {
                    Search(t + 1, tryClay + robots, robots with { Clay = robots.Clay + 1 });
                }

                var tryOre = bank - oreCost;
                if (!tryOre.IsOverdrawn() && robots.Ore <= maxOreCost)
                {
                    Search(t + 1, tryOre + robots, robots with { Ore = robots.Ore + 1 });
                }

                if (bank.Ore < maxOreCost)
                {
                    Search(t + 1, newBank, robots);
                }

                path.Pop();
            }

            Search(0, new Resource(0, 0, 0, 0), new Resource(1, 0, 0, 0));

            result1 += bestScore.Geode * id;
            result2 *= bestScore.Geode;
        }

        var result = part switch { 1 => result1, 2 => result2 };
        Console.WriteLine($"Part {part}: {result} in {timer.ElapsedMilliseconds}ms");
    }

    private record Resource(int Ore, int Clay, int Obsidian, int Geode)
    {
        public static Resource operator +(Resource a, Resource b) => new(a.Ore + b.Ore, a.Clay + b.Clay, a.Obsidian + b.Obsidian, a.Geode + b.Geode);

        public static Resource operator -(Resource a, Resource b) => new(a.Ore - b.Ore, a.Clay - b.Clay, a.Obsidian - b.Obsidian, a.Geode - b.Geode);

        public bool IsOverdrawn() => this.Ore < 0 || this.Clay < 0 || this.Obsidian < 0 || this.Geode < 0;
    }
}