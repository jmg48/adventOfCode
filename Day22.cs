namespace AdventOfCode;

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using NUnit.Framework;

[TestFixture]
public class Day22
{
    [TestCase(1)]
    [TestCase(2)]
    public void Test(int part)
    {
        var timer = new Stopwatch();
        timer.Start();

        var lines = File.ReadAllLines("C:\\git\\input22.txt");
        var maze = lines.Take(lines.Length - 2).Select(line => line.ToList()).ToList();
        var height = maze.Count;
        var width = maze.Max(line => line.Count);

        var path = lines[^1];
        var pathIndex = 0;

        var facing = 0;

        var position = new Coord(0, 0);
        while (Maze(position) != '.')
        {
            position = position with { X = position.X + 1 };
        }

        char Maze(Coord coord) => coord.Y >= 0 && coord.Y < height && coord.X >= 0 && coord.X < maze[coord.Y].Count ? maze[coord.Y][coord.X] : ' ';

        (Coord Dest, int Facing) Move()
        {
            var vector = facing switch
            {
                0 => new Coord(1, 0),
                1 => new Coord(0, 1),
                2 => new Coord(-1, 0),
                3 => new Coord(0, -1),
            };

            if (part == 1)
            {
                var dest = position;
                do
                {
                    dest = new Coord((dest.X + vector.X + width) % width, (dest.Y + vector.Y + height) % height);
                }
                while (Maze(dest) == ' ');
                return (dest, facing);
            }
            else
            {
                var dest = new Coord(position.X + vector.X, position.Y + vector.Y);
                if (Maze(dest) != ' ')
                {
                    return (dest, facing);
                }

                var faceX = ((dest.X + 50) / 50) - 1;
                var faceY = ((dest.Y + 50) / 50) - 1;
                return (faceX, faceY, facing) switch
                {
                    (-1, 2, 2) => (new Coord(50, 149 - dest.Y), 0), // ae
                    (0, 0, 2) => (new Coord(0, 149 - dest.Y), 0), // ae
                    (-1, 3, 2) => (new Coord(dest.Y - 100, 0), 1), // eg
                    (1, -1, 3) => (new Coord(0, dest.X + 100), 0), // eg
                    (0, 1, 3) => (new Coord(50, dest.X + 50), 0), // ac
                    (0, 1, 2) => (new Coord(dest.Y - 50, 100), 1), // ac
                    (0, 4, 1) => (new Coord(dest.X + 100, 0), 1), // gh
                    (2, -1, 3) => (new Coord(dest.X - 100, 199), 3), // gh
                    (1, 3, 1) => (new Coord(49, dest.X + 100), 2), // fh
                    (1, 3, 0) => (new Coord(dest.Y - 100, 149), 3), // fh
                    (2, 1, 1) => (new Coord(99, dest.X - 50), 2), // bd
                    (2, 1, 0) => (new Coord(dest.Y + 50, 49), 3), // bd
                    (2, 2, 0) => (new Coord(149, 149 - dest.Y), 2), // hd
                    (3, 0, 0) => (new Coord(99, 149 - dest.Y), 2), // hd
                };
            }
        }

        while (pathIndex < path.Length)
        {
            switch (path[pathIndex])
            {
                case 'L':
                    facing = (facing + 3) % 4;
                    pathIndex++;
                    break;
                case 'R':
                    facing = (facing + 1) % 4;
                    pathIndex++;
                    break;
                default:
                    var dist = int.Parse(path[pathIndex].ToString());
                    pathIndex++;
                    while (pathIndex < path.Length && int.TryParse(path[pathIndex].ToString(), out var value))
                    {
                        dist = (dist * 10) + value;
                        pathIndex++;
                    }

                    for (var i = 0; i < dist; i++)
                    {
                        var (dest, newFacing) = Move();
                        if (Maze(dest) == '#')
                        {
                            break;
                        }

                        (position, facing) = (dest, newFacing);
                    }

                    break;
            }
        }

        Console.WriteLine($"{(1000 * (position.Y + 1)) + (4 * (position.X + 1)) + facing} in {timer.ElapsedMilliseconds}ms");
    }
}