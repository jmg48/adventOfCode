namespace AdventOfCode;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using NUnit.Framework;

[TestFixture]
internal class Day24
{
    [Test]
    public void Test()
    {
        var timer = new Stopwatch();
        timer.Start();

        var input = File.ReadAllLines("C:\\git\\advent-of-code\\input24.txt");

        var height = input.Length - 2;
        var width = input[0].Length - 2;

        var upGrid = input.Skip(1).Take(input.Length - 2).Select(line => line.Skip(1).Take(line.Length - 2).Select(c => c == '^').ToList()).ToList();
        var downGrid = input.Skip(1).Take(input.Length - 2).Select(line => line.Skip(1).Take(line.Length - 2).Select(c => c == 'v').ToList()).ToList();
        var leftGrid = input.Skip(1).Take(input.Length - 2).Select(line => line.Skip(1).Take(line.Length - 2).Select(c => c == '<').ToList()).ToList();
        var rightGrid = input.Skip(1).Take(input.Length - 2).Select(line => line.Skip(1).Take(line.Length - 2).Select(c => c == '>').ToList()).ToList();

        int PosMod(int a, int b) => ((a % b) + b) % b;
        bool Left(int x, int y, int t) => leftGrid[y][PosMod(x + t, width)];
        bool Right(int x, int y, int t) => rightGrid[y][PosMod(x - t, width)];
        bool Up(int x, int y, int t) => upGrid[PosMod(y + t, height)][x];
        bool Down(int x, int y, int t) => downGrid[PosMod(y - t, height)][x];
        bool IsOccupied(int x, int y, int t) => x < 0 || y < 0 || x >= width || y >= height || Left(x, y, t) || Right(x, y, t) || Up(x, y, t) || Down(x, y, t);

        int Navigate(Coord from, Coord to, int t) => Dfs(from, t, from, to, new HashSet<(Coord Pos, int T)>(), t + 500);

        int Dfs(Coord pos, int t, Coord start, Coord end, ISet<(Coord Pos, int T)> tried, int timeLimit)
        {
            var bestTime = timeLimit;
            if (tried.Contains((pos, t)) || t >= bestTime)
            {
                return bestTime;
            }

            tried.Add((pos, t));

            foreach (var move in new[] { new Coord(0, 1), new Coord(1, 0), new Coord(0, 0), new Coord(0, -1), new Coord(-1, 0) })
            {
                var newPos = pos + move;
                if (newPos == end)
                {
                    return t;
                }

                if (newPos == start || !IsOccupied(newPos.X, newPos.Y, t))
                {
                    var endTime = Dfs(newPos, t + 1, start, end, tried, bestTime);
                    if (endTime < bestTime)
                    {
                        bestTime = endTime;
                    }
                }
            }

            return bestTime;
        }

        var topLeft = new Coord(0, -1);
        var bottomRight = new Coord(width - 1, height);
        var part1 = Navigate(topLeft, bottomRight, 1);
        Console.WriteLine($"Part 1: {part1} in {timer.ElapsedMilliseconds}ms");

        timer.Restart();
        var retrace = Navigate(bottomRight, topLeft, part1 + 1);
        var part2 = Navigate(topLeft, bottomRight, retrace + 1);
        Console.WriteLine($"Part 2: {part2} in {timer.ElapsedMilliseconds}ms");
    }
}