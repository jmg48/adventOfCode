namespace AdventOfCode;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;

[TestFixture]
public class Day9
{
    [Test]
    public void Test()
    {
        var coords = Enumerable.Range(0, 10).Select(_ => new Coord(0, 0)).ToList();
        var path = new List<List<Coord>>();

        foreach (var line in File.ReadLines("C:\\git\\advent-of-code\\input9.txt").Select(line => line.Split(' ')))
        {
            var direction = line[0];
            var length = int.Parse(line[1]);
            for (var i = 0; i < length; i++)
            {
                var head = coords[0];

                coords[0] = direction switch
                {
                    "L" => head with { X = head.X - 1 },
                    "R" => head with { X = head.X + 1 },
                    "U" => head with { Y = head.Y + 1 },
                    "D" => head with { Y = head.Y - 1 },
                };

                for (var j = 1; j < coords.Count; j++)
                {
                    coords[j] = this.Follow(coords[j - 1], coords[j]);
                }

                path.Add(coords.ToList());
            }
        }

        Console.WriteLine(path.Select(rope => rope[1]).Distinct().Count());
        Console.WriteLine(path.Select(rope => rope[9]).Distinct().Count());
    }

    private Coord Follow(Coord head, Coord tail)
    {
        var horiz = head.X - tail.X;
        var vert = head.Y - tail.Y;

        if (vert == 0)
        {
            return horiz switch
            {
                > 1 => tail with { X = tail.X + 1 },
                < -1 => tail with { X = tail.X - 1 },
                _ => tail,
            };
        }

        if (horiz == 0)
        {
            return vert switch
            {
                > 1 => tail with { Y = tail.Y + 1 },
                < -1 => tail with { Y = tail.Y - 1 },
                _ => tail,
            };
        }

        if (vert is > 1 or < -1 || horiz is > 1 or < -1)
        {
            return new Coord(X: tail.X + (horiz > 0 ? 1 : -1), Y: tail.Y + (vert > 0 ? 1 : -1));
        }

        return tail;
    }
}