namespace AdventOfCode;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using NUnit.Framework;

[TestFixture]
public class Day18
{
    [Test]
    public void Test()
    {
        var timer = new Stopwatch();
        timer.Start();

        var cubes = File.ReadLines("C:\\git\\input18.txt")
            .Select(s => s.Split(',').Select(int.Parse).ToList())
            .Select(c => new Cube(c[0], c[1], c[2])).ToList();

        var cubeHash = new HashSet<Cube>(cubes);

        Cube Move(Cube cube, int x, int y, int z) => new(cube.X + x, cube.Y + y, cube.Z + z);

        IEnumerable<Cube> Neighbours(Cube cube)
        {
            yield return Move(cube, 1, 0, 0);
            yield return Move(cube, -1, 0, 0);
            yield return Move(cube, 0, 1, 0);
            yield return Move(cube, 0, -1, 0);
            yield return Move(cube, 0, 0, 1);
            yield return Move(cube, 0, 0, -1);
        }

        Console.WriteLine($"Part 1: {cubes.Sum(cube => 6 - Neighbours(cube).Count(c => cubeHash.Contains(c)))} in {timer.ElapsedMilliseconds}ms");
        timer.Restart();

        var xMin = cubes.Min(cube => cube.X);
        var xMax = cubes.Max(cube => cube.X);
        var yMin = cubes.Min(cube => cube.Y);
        var yMax = cubes.Max(cube => cube.Y);
        var zMin = cubes.Min(cube => cube.Z);
        var zMax = cubes.Max(cube => cube.Z);

        var outside = new HashSet<Cube>();

        var found = true;
        while (found)
        {
            found = false;
            for (var x = xMin - 1; x <= xMax + 1; x++)
            {
                for (var y = yMin - 1; y <= yMax + 1; y++)
                {
                    for (var z = zMin - 1; z <= zMax + 1; z++)
                    {
                        var cube = new Cube(x, y, z);
                        if (outside.Contains(cube) || cubeHash.Contains(cube))
                        {
                            continue;
                        }

                        if (x == xMin - 1 || y == yMin - 1 || z == zMin - 1 || x == xMax + 1 || y == yMax + 1 || z == zMax + 1)
                        {
                            outside.Add(cube);
                            found = true;
                            continue;
                        }

                        if (Neighbours(cube).Any(c => outside.Contains(c)))
                        {
                            outside.Add(cube);
                            found = true;
                        }
                    }
                }
            }
        }

        Console.WriteLine($"Part 2: {cubes.Sum(cube => Neighbours(cube).Count(c => outside.Contains(c)))} in {timer.ElapsedMilliseconds}ms");
    }

    private record Cube(int X, int Y, int Z);
}