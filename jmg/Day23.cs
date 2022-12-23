namespace AdventOfCode
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using NUnit.Framework;

    [TestFixture]
    internal class Day23
    {
        [Test]
        public void Test()
        {
            var grid = new HashSet<Coord>();
            var input = File.ReadAllLines("C:\\git\\input23.txt").Select(line => line.ToList()).ToList();
            {
                for (var i = 0; i < input.Count; i++)
                {
                    for (var j = 0; j < input[i].Count; j++)
                    {
                        if (input[i][j] == '#')
                        {
                            grid.Add(new Coord(j, i));
                        }
                    }
                }
            }

            var movements = new[]
            {
                new Coord(0, -1),
                new Coord(0, 1),
                new Coord(-1, 0),
                new Coord(1, 0),
            };

            var diagonals = new[]
            {
                new Coord(-1, -1),
                new Coord(1, -1),
                new Coord(-1, 1),
                new Coord(1, 1),
                new Coord(-1, -1),
                new Coord(-1, 1),
                new Coord(1, -1),
                new Coord(1, 1),
            };

            var round = 1;
            while (true)
            {
                var possibleMoves = new List<(Coord From, Coord To)>();
                foreach (var elf in grid)
                {
                    if (movements.Concat(diagonals).All(move => !grid.Contains(elf + move)))
                    {
                        continue;
                    }

                    for (var i = 0; i < 4; i++)
                    {
                        var movement = movements[i];
                        var diag1 = diagonals[i * 2];
                        var diag2 = diagonals[(i * 2) + 1];
                        if (!grid.Contains(elf + movement) && !grid.Contains(elf + diag1) && !grid.Contains(elf + diag2))
                        {
                            possibleMoves.Add((elf, elf + movement));
                            break;
                        }
                    }
                }

                var moved = false;
                foreach (var move in possibleMoves.GroupBy(move => move.To).Where(group => group.Count() == 1).Select(group => group.Single()))
                {
                    grid.Remove(move.From);
                    grid.Add(move.To);
                    moved = true;
                }

                movements = movements.Skip(1).Concat(movements.Take(1)).ToArray();
                diagonals = diagonals.Skip(2).Concat(diagonals.Take(2)).ToArray();

                if (round == 10)
                {
                    var left = grid.Min(elf => elf.X);
                    var right = grid.Max(elf => elf.X);
                    var top = grid.Max(elf => elf.Y);
                    var bottom = grid.Min(elf => elf.Y);
                    var result = ((right - left + 1) * (top - bottom + 1)) - grid.Count;
                    Console.WriteLine($"Part 1: {result}");
                }

                if (!moved)
                {
                    Console.WriteLine($"Part 2: {round}");
                    return;
                }

                round++;
            }
        }
    }
}
