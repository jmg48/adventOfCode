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
            var input = File.ReadAllLines("C:\\git\\advent-of-code\\input23.txt").Select(line => line.ToList()).ToList();
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

            var movements = new[] { new Coord(0, -1), new Coord(0, 1), new Coord(-1, 0), new Coord(1, 0) };
            var diagonals = new[] { new Coord(-1, -1), new Coord(1, -1), new Coord(-1, 1), new Coord(1, 1), new Coord(-1, -1), new Coord(-1, 1), new Coord(1, -1), new Coord(1, 1) };
            var allDirections = movements.Concat(diagonals).Distinct().ToList();

            var round = 1;
            while (true)
            {
                var possibleMoves = new List<(Coord From, Coord To)>();
                foreach (var elf in grid.Where(elf => allDirections.Any(move => grid.Contains(elf + move))))
                {
                    for (var i = 0; i < 4; i++)
                    {
                        if (!grid.Contains(elf + movements[i]) && !grid.Contains(elf + diagonals[i * 2]) && !grid.Contains(elf + diagonals[(i * 2) + 1]))
                        {
                            possibleMoves.Add((elf, elf + movements[i]));
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

                if (round == 10)
                {
                    Console.WriteLine($"Part 1: {((grid.Max(elf => elf.X) - grid.Min(elf => elf.X) + 1) * (grid.Max(elf => elf.Y) - grid.Min(elf => elf.Y) + 1)) - grid.Count}");
                }

                if (!moved)
                {
                    Console.WriteLine($"Part 2: {round}");
                    return;
                }

                movements = movements.Skip(1).Concat(movements.Take(1)).ToArray();
                diagonals = diagonals.Skip(2).Concat(diagonals.Take(2)).ToArray();
                round++;
            }
        }
    }
}
