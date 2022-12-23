namespace AdventOfCode;

using System;
using System.IO;
using System.Linq;
using NUnit.Framework;

[TestFixture]
public class Day14
{
    [TestCase(1)]
    [TestCase(2)]
    public void Test(int part)
    {
        var input = File.ReadLines("C:\\git\\input14.txt")
            .Select(line =>
                line.Split(" -> ").Select(s =>
                    new Coord(int.Parse((string)s.Split(',')[0]), int.Parse((string)s.Split(',')[1]))).ToList()).ToList();

        var origin = new Coord(input.Min(line => line.Min(coord => coord.X)), input.Min(line => line.Min(coord => coord.Y)));
        var extent = new Coord(input.Max(line => line.Max(coord => coord.X)) + 1, input.Max(line => line.Max(coord => coord.Y)) + 1);
        var wall = new char[extent.X + 200, extent.Y + 2];

        foreach (var line in input)
        {
            for (var i = 1; i < line.Count; i++)
            {
                var from = line[i - 1];
                var to = line[i];
                if (from.X == to.X)
                {
                    for (var j = Math.Min(from.Y, to.Y); j <= Math.Max(from.Y, to.Y); j++)
                    {
                        wall[from.X, j] = '#';
                    }
                }
                else if (from.Y == to.Y)
                {
                    for (var j = Math.Min(from.X, to.X); j <= Math.Max(from.X, to.X); j++)
                    {
                        wall[j, from.Y] = '#';
                    }
                }
                else
                {
                    throw new NotSupportedException();
                }
            }
        }

        if (part == 2)
        {
            for (int i = 0; i < wall.GetLength(0); i++)
            {
                wall[i, extent.Y + 1] = '#';
            }
        }

        void PrintWall()
        {
            for (var j = 0; j < wall.GetLength(1); j++)
            {
                for (var i = origin.X; i < wall.GetLength(0); i++)
                {
                    Console.Write(wall[i, j] switch { '\0' => " ", var c => c.ToString() });
                }

                Console.WriteLine();
            }
        }

        var result = 0;
        while (true)
        {
            if (wall[500, 0] != '\0')
            {
                Console.WriteLine("Full");
                Console.WriteLine(result);
                PrintWall();
                return;
            }

            var s = new Coord(500, 0);
            while (true)
            {
                if (s.Y == wall.GetLength(1) - 1)
                {
                    Console.WriteLine("Fall");
                    Console.WriteLine(result);
                    PrintWall();
                    return;
                }

                if (wall[s.X, s.Y + 1] == '\0')
                {
                    s = new Coord(s.X, s.Y + 1);
                }
                else if (wall[s.X - 1, s.Y + 1] == '\0')
                {
                    s = new Coord(s.X - 1, s.Y + 1);
                }
                else if (wall[s.X + 1, s.Y + 1] == '\0')
                {
                    s = new Coord(s.X + 1, s.Y + 1);
                }
                else
                {
                    wall[s.X, s.Y] = 'S';
                    result++;
                    break;
                }
            }
        }
    }
}