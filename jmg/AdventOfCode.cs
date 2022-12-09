﻿namespace AdventOfCode
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using FluentAssertions;
    using NUnit.Framework;

    [TestFixture]
    public class AdventOfCode
    {
        public enum Guess
        {
            Rock,
            Paper,
            Scissors,
        }

        public enum Result
        {
            Loss,
            Draw,
            Win,
        }

        [Test]
        public void DayOne()
        {
            Console.WriteLine(File.ReadLines("C:\\git\\input.txt")
                .Aggregate(
                    (0, 0),
                    (a, s) => string.IsNullOrWhiteSpace(s)
                        ? (Math.Max(a.Item1, a.Item2), 0)
                        : (a.Item1, a.Item2 + int.Parse(s)),
                    a => a.Item1));

            var top3 = File.ReadLines("C:\\git\\input.txt")
                .Aggregate(
                    (new List<int> { 0, 0, 0 }, 0),
                    (a, s) => string.IsNullOrWhiteSpace(s)
                        ? (a.Item1.Concat(new[] { a.Item2 }).OrderByDescending(i => i).Take(3).ToList(), 0)
                        : (a.Item1, a.Item2 + int.Parse(s)),
                    a => a.Item1);

            var totals = new List<int>();

            var currentRunningTotal = 0;
            using var reader = new StreamReader("C:\\git\\input.txt");
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                if (string.IsNullOrWhiteSpace(line))
                {
                    totals.Add(currentRunningTotal);
                    currentRunningTotal = 0;
                }
                else
                {
                    currentRunningTotal += int.Parse(line);
                }
            }

            Console.WriteLine(totals.OrderByDescending(x => x).Take(3).Sum());
        }

        [Test]
        public void DayTwo_1()
        {
            var total = 0;

            using var reader = new StreamReader("C:\\git\\input2.txt");
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var theirGuess = line[0] switch
                {
                    'A' => Guess.Rock,
                    'B' => Guess.Paper,
                    'C' => Guess.Scissors,
                };

                var myGuess = line[2] switch
                {
                    'X' => Guess.Rock,
                    'Y' => Guess.Paper,
                    'Z' => Guess.Scissors,
                };

                total += myGuess switch
                {
                    Guess.Rock => 1,
                    Guess.Paper => 2,
                    Guess.Scissors => 3,
                };

                var result = theirGuess switch
                {
                    Guess.Rock => myGuess switch
                    {
                        Guess.Rock => Result.Draw,
                        Guess.Paper => Result.Win,
                        Guess.Scissors => Result.Loss,
                    },
                    Guess.Paper => myGuess switch
                    {
                        Guess.Rock => Result.Loss,
                        Guess.Paper => Result.Draw,
                        Guess.Scissors => Result.Win,
                    },
                    Guess.Scissors => myGuess switch
                    {
                        Guess.Rock => Result.Win,
                        Guess.Paper => Result.Loss,
                        Guess.Scissors => Result.Draw,
                    },
                };

                total += result switch
                {
                    Result.Loss => 0,
                    Result.Draw => 3,
                    Result.Win => 6,
                };
            }

            Console.WriteLine(total);
        }

        [Test]
        public void DayTwo_2()
        {
            var total = 0;

            using var reader = new StreamReader("C:\\git\\input2.txt");
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var theirGuess = line[0] switch
                {
                    'A' => Guess.Rock,
                    'B' => Guess.Paper,
                    'C' => Guess.Scissors,
                };

                var result = line[2] switch
                {
                    'X' => Result.Loss,
                    'Y' => Result.Draw,
                    'Z' => Result.Win,
                };

                var myGuess = theirGuess switch
                {
                    Guess.Rock => result switch
                    {
                        Result.Loss => Guess.Scissors,
                        Result.Draw => Guess.Rock,
                        Result.Win => Guess.Paper,
                    },
                    Guess.Paper => result switch
                    {
                        Result.Loss => Guess.Rock,
                        Result.Draw => Guess.Paper,
                        Result.Win => Guess.Scissors,
                    },
                    Guess.Scissors => result switch
                    {
                        Result.Loss => Guess.Paper,
                        Result.Draw => Guess.Scissors,
                        Result.Win => Guess.Rock,
                    },
                };

                total += myGuess switch
                {
                    Guess.Rock => 1,
                    Guess.Paper => 2,
                    Guess.Scissors => 3,
                };

                total += result switch
                {
                    Result.Loss => 0,
                    Result.Draw => 3,
                    Result.Win => 6,
                };
            }

            Console.WriteLine(total);
        }

        [Test]
        public void DayThree_1()
        {
            var total = 0;
            using var reader = new StreamReader("C:\\git\\input3.txt");
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var bag1 = new HashSet<char>(line.Take(line.Length / 2));
                var c = line.Skip(line.Length / 2).Where(c => bag1.Contains(c)).Distinct().Single();
                var i = (int)c;
                var score = i <= 'Z' ? (i - 'A' + 27) : (i - 'a' + 1);
                total += score;
            }

            Console.WriteLine(total);
        }

        [Test]
        public void DayThree_2()
        {
            var total = 0;
            using var reader = new StreamReader("C:\\git\\input3.txt");
            while (!reader.EndOfStream)
            {
                var line1 = new HashSet<char>(reader.ReadLine());
                line1.IntersectWith(reader.ReadLine());
                line1.IntersectWith(reader.ReadLine());
                var c = line1.Single();
                var i = (int)c;
                var score = i <= 'Z' ? (i - 'A' + 27) : (i - 'a' + 1);
                total += score;
            }

            Console.WriteLine(total);
        }

        [Test]
        public void DayFour_1()
        {
            var contains = 0;
            var overlaps = 0;
            using var reader = new StreamReader("C:\\git\\input4.txt");
            while (!reader.EndOfStream)
            {
                var match = Regex.Match(reader.ReadLine(), @"(\d+)-(\d+),(\d+)-(\d+)");
                var a = int.Parse(match.Groups[1].Value);
                var b = int.Parse(match.Groups[2].Value);
                var c = int.Parse(match.Groups[3].Value);
                var d = int.Parse(match.Groups[4].Value);
                if ((a >= c && b <= d) || (c >= a && d <= b))
                {
                    contains++;
                }

                if (b >= c && d >= a)
                {
                    overlaps++;
                }
            }

            Console.WriteLine(contains);
            Console.WriteLine(overlaps);
        }

        [TestCase(1)]
        [TestCase(2)]
        public void DayFive(int part)
        {
            using var reader = new StreamReader("C:\\git\\input5.txt");
            var stackLines = new List<string>();
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                if (string.IsNullOrWhiteSpace(line))
                {
                    break;
                }

                stackLines.Add(line);
            }

            var stacks = new Dictionary<int, Stack<char>>();
            foreach (var i in stackLines.Last().Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).Select(int.Parse))
            {
                stacks.Add(i, new Stack<char>());
            }

            foreach (var stackLine in stackLines.AsEnumerable().Reverse().Skip(1))
            {
                foreach (var stack in stacks.Keys)
                {
                    var val = stackLine[(stack * 4) - 3];
                    if (val != ' ')
                    {
                        stacks[stack].Push(val);
                    }
                }
            }

            while (!reader.EndOfStream)
            {
                var line = Regex.Match(reader.ReadLine(), @"move (\d+) from (\d+) to (\d+)");
                var n = int.Parse(line.Groups[1].Value);
                var from = int.Parse(line.Groups[2].Value);
                var to = int.Parse(line.Groups[3].Value);

                switch (part)
                {
                    case 1:
                        for (var i = 0; i < n; i++)
                        {
                            stacks[to].Push(stacks[from].Pop());
                        }

                        break;
                    case 2:
                        var crane = new Stack<char>();
                        for (var i = 0; i < n; i++)
                        {
                            crane.Push(stacks[from].Pop());
                        }

                        for (var i = 0; i < n; i++)
                        {
                            stacks[to].Push(crane.Pop());
                        }

                        break;
                    default:
                        throw new NotSupportedException();
                }
            }

            foreach (var kvp in stacks.OrderBy(stack => stack.Key))
            {
                Console.Write(kvp.Value.Peek());
            }

            Console.WriteLine();
        }

        [TestCase(4)]
        [TestCase(14)]
        public void Day6(int n)
        {
            var input = File.ReadAllText("C:\\git\\input6.txt");
            Console.WriteLine(
                Enumerable.Range(n, input.Length - n).First(i => new HashSet<char>(input.Skip(i - n).Take(n)).Count == n));
        }

        [Test]
        public void Day7()
        {
            var dirSizes = new ConcurrentDictionary<string, int>();

            var paths = new Stack<string>(new[] { string.Empty });
            foreach (var line in File.ReadLines("C:\\git\\input7.txt"))
            {
                if (line.StartsWith("$ cd "))
                {
                    var arg = line[5..];
                    switch (arg)
                    {
                        case "/":
                            paths.Clear();
                            paths.Push(string.Empty);
                            break;
                        case "..":
                            paths.Pop();
                            break;
                        default:
                            paths.Push($"{paths.Peek()}/{arg}");
                            break;
                    }
                }
                else if (line.StartsWith("$ ls"))
                {
                }
                else
                {
                    var ls = line.Split(' ');
                    if (ls[0] != "dir")
                    {
                        var fileSize = int.Parse(ls[0]);
                        foreach (var path in paths)
                        {
                            dirSizes.AddOrUpdate(
                                path,
                                _ => fileSize,
                                (_, dirSize) => dirSize + fileSize);
                        }
                    }
                }
            }

            var result1 = dirSizes.Values.Where(i => i <= 100000).Sum();
            Console.WriteLine($"Part One: {result1}");

            var totalSize = dirSizes[string.Empty];
            var freeSpace = 70000000 - totalSize;
            var additionalRequired = 30000000 - freeSpace;

            var result2 = dirSizes.Values.OrderBy(i => i).First(i => i >= additionalRequired);
            Console.WriteLine($"Part Two: {result2}");

            result1.Should().Be(1648397);
            result2.Should().Be(1815525);
        }

        [Test]
        public void Day8()
        {
            var visibleTrees = 0;
            var bestScore = 0;
            var grid = new List<List<int>>(File.ReadLines("C:\\git\\input8.txt")
                .Select(line => line.Select(c => int.Parse(new[] { c })).ToList()));

            for (var i = 0; i < grid.Count; i++)
            {
                for (var j = 0; j < grid[0].Count; j++)
                {
                    var height = grid[i][j];
                    if (Enumerable.Range(0, i).All(k => grid[k][j] < height)
                        || Enumerable.Range(0, j).All(k => grid[i][k] < height)
                        || Enumerable.Range(i + 1, grid.Count - i - 1).All(k => grid[k][j] < height)
                        || Enumerable.Range(j + 1, grid[0].Count - j - 1).All(k => grid[i][k] < height))
                    {
                        visibleTrees++;
                    }

                    var scoreUp = i;
                    for (var k = 1; i - k >= 0; k++)
                    {
                        if (grid[i - k][j] >= height)
                        {
                            scoreUp = k;
                            break;
                        }
                    }

                    var scoreLeft = j;
                    for (var k = 1; j - k >= 0; k++)
                    {
                        if (grid[i][j - k] >= height)
                        {
                            scoreLeft = k;
                            break;
                        }
                    }

                    var scoreDown = grid.Count - i - 1;
                    for (var k = 1; i + k < grid.Count; k++)
                    {
                        if (grid[i + k][j] >= height)
                        {
                            scoreDown = k;
                            break;
                        }
                    }

                    var scoreRight = grid[0].Count - j - 1;
                    for (var k = 1; j + k < grid[0].Count; k++)
                    {
                        if (grid[i][j + k] >= height)
                        {
                            scoreRight = k;
                            break;
                        }
                    }

                    bestScore = Math.Max(bestScore, scoreUp * scoreDown * scoreLeft * scoreRight);
                }
            }

            Console.WriteLine(visibleTrees);
            Console.WriteLine(bestScore);
        }

        [Test]
        public void Day9()
        {
            var coords = Enumerable.Range(0, 10).Select(_ => new Coord(0, 0)).ToList();
            var path = new List<List<Coord>>();

            foreach (var line in File.ReadLines("C:\\git\\input9.txt").Select(line => line.Split(' ')))
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

            if (vert > 1 || vert < -1 || horiz > 1 || horiz < -1)
            {
                return tail with { X = tail.X + (horiz > 0 ? 1 : -1), Y = tail.Y + (vert > 0 ? 1 : -1) };
            }

            return tail;
        }

        private record Coord(int X, int Y);
    }
}
