namespace AdventOfCode;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using NUnit.Framework;

[TestFixture]
public class Day15
{
    [Test]
    public void Test()
    {
        var input = File.ReadLines("C:\\git\\advent-of-code\\input15.txt")
            .Select(line => Regex.Match(line, @"Sensor at x=(-?\d+), y=(-?\d+): closest beacon is at x=(-?\d+), y=(-?\d+)"))
            .Select(match => (
                Sensor: new Coord(int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value)),
                Beacon: new Coord(int.Parse(match.Groups[3].Value), int.Parse(match.Groups[4].Value))))
            .ToList();

        int MDist(Coord from, Coord to) => Math.Abs(from.Y - to.Y) + Math.Abs(from.X - to.X);

        var y1 = 2000000;
        var beacons = new HashSet<Coord>();
        var excluded = new HashSet<Coord>();
        foreach (var (sensor, beacon) in input)
        {
            beacons.Add(beacon);

            var radius = MDist(beacon, sensor);
            var yDist = Math.Abs(sensor.Y - y1);
            if (yDist <= radius)
            {
                var xDist = radius - yDist;
                var xMin = sensor.X - xDist;
                var xMax = sensor.X + xDist;
                for (var x = xMin; x <= xMax; x++)
                {
                    excluded.Add(new Coord(x, y1));
                }
            }
        }

        excluded.ExceptWith(beacons);
        Console.WriteLine($"Part 1: {excluded.Count}");

        for (var y = 0; y <= 4000000; y++)
        {
            var exclusions = new List<(int From, int To)>();
            foreach (var (sensor, beacon) in input)
            {
                var radius = MDist(beacon, sensor);
                var yDist = Math.Abs(sensor.Y - y);
                if (yDist <= radius)
                {
                    var xDist = radius - yDist;
                    var xMin = Math.Max(0, sensor.X - xDist);
                    var xMax = Math.Min(sensor.X + xDist, 4000000);
                    exclusions.Add((xMin, xMax));
                }
            }

            var merged = true;
            while (merged)
            {
                merged = false;
                for (var i = 0; i < exclusions.Count && !merged; i++)
                {
                    var left = exclusions[i];
                    for (var j = 0; j < exclusions.Count && !merged; j++)
                    {
                        var right = exclusions[j];
                        if (i != j && right.From <= left.To && left.To <= right.To)
                        {
                            exclusions[i] = (Math.Min(left.From, right.From), Math.Max(left.To, right.To));
                            exclusions.RemoveAt(j);
                            merged = true;
                        }
                    }
                }
            }

            if (exclusions.Count != 1 || exclusions[0] != (0, 4000000))
            {
                Console.WriteLine($"y: {y}");
                var xValues = new HashSet<int>(Enumerable.Range(0, 4000001));
                foreach (var (from, to) in exclusions)
                {
                    Console.WriteLine($"  from: {from}, to: {to}");
                    xValues.ExceptWith(Enumerable.Range(from, to - from + 1));
                }

                Console.WriteLine($"Part 2: {((long)xValues.Single() * 4000000) + y}");
            }
        }
    }
}