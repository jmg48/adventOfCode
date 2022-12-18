namespace AdventOfCode
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Dijkstra.NET.Graph.Simple;
    using Dijkstra.NET.ShortestPath;
    using FluentAssertions;
    using NUnit.Framework;
    using static global::AdventOfCode.AdventOfCode;

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

        [Test]
        public void Day10()
        {
            var cycle = 1;
            var x = 1;
            var s = new List<int> { 0, 1 };
            foreach (var line in File.ReadLines("C:\\git\\input10.txt"))
            {
                switch (line[..4])
                {
                    case "noop":
                        cycle++;
                        s.Add(x);
                        break;
                    case "addx":
                        cycle++;
                        s.Add(x);
                        cycle++;
                        x += int.Parse(line[5..]);
                        s.Add(x);
                        break;
                    default:
                        throw new NotSupportedException();
                }
            }

            int F(int i) => s[i] * i;

            Console.WriteLine(F(20) + F(60) + F(100) + F(140) + F(180) + F(220));

            for (var i = 1; i <= 240; i++)
            {
                var xPos = (i - 1) % 40;
                if (xPos == 0)
                {
                    Console.WriteLine();
                }

                var spritePos = s[i];
                if (xPos == spritePos - 1 || xPos == spritePos || xPos == spritePos + 1)
                {
                    Console.Write("#");
                }
                else
                {
                    Console.Write(".");
                }
            }
        }

        [TestCase(1)]
        [TestCase(2)]
        public void Day11(int part)
        {
            var monkeys = new List<Monkey>
            {
                new(new List<long> { 54, 89, 94 }, old => old * 7, n => n % 17 == 0 ? 5 : 3),
                new(new List<long> { 66, 71 }, old => old + 4, n => n % 3 == 0 ? 0 : 3),
                new(new List<long> { 76, 55, 80, 55, 55, 96, 78 }, old => old + 2, n => n % 5 == 0 ? 7 : 4),
                new(new List<long> { 93, 69, 76, 66, 89, 54, 59, 94 }, old => old + 7, n => n % 7 == 0 ? 5 : 2),
                new(new List<long> { 80, 54, 58, 75, 99 }, old => old * 17, n => n % 11 == 0 ? 1 : 6),
                new(new List<long> { 69, 70, 85, 83 }, old => old + 8, n => n % 19 == 0 ? 2 : 7),
                new(new List<long> { 89 }, old => old + 6, n => n % 2 == 0 ? 0 : 1),
                new(new List<long> { 62, 80, 58, 57, 93, 56 }, old => old * old, n => n % 13 == 0 ? 6 : 4),
            };

            var rounds = part == 1 ? 20 : 10000;
            Func<long, long> reduce = part == 1 ? n => n / 3 : n => n % (2 * 3 * 5 * 7 * 11 * 13 * 17 * 19);

            for (var round = 0; round < rounds; round++)
            {
                foreach (var monkey in monkeys)
                {
                    while (monkey.Items.Count > 0)
                    {
                        monkey.Inspections++;
                        var item = monkey.Items.Dequeue();
                        item = reduce(monkey.Operation(item));
                        monkeys[monkey.ThrowTo(item)].Items.Enqueue(item);
                    }
                }
            }

            var result = monkeys.Select(monkey => monkey.Inspections).OrderByDescending(i => i).Take(2).Aggregate((a, b) => a * b);
            Console.WriteLine(result);
        }

        [Test]
        public void Day12()
        {
            var graph = new Graph();
            var input = File.ReadLines("C:\\git\\input12.txt").Select(line => line.Select(c => (c, graph.AddNode())).ToList<(char C, uint N)>()).ToList();

            int Height((char C, uint N) cell) => cell.C switch
            {
                'S' => (int)'a',
                'E' => (int)'z',
                _ => (int)cell.C,
            };

            uint start = 0;
            uint end = 0;
            var starts = new List<uint>();
            for (var i = 0; i < input.Count; i++)
            {
                for (var j = 0; j < input[0].Count; j++)
                {
                    var cell = input[i][j];

                    switch (cell.C)
                    {
                        case 'S':
                            start = cell.N;
                            starts.Add(cell.N);
                            break;
                        case 'E':
                            end = cell.N;
                            break;
                        case 'a':
                            starts.Add(cell.N);
                            break;
                    }

                    if (i > 0)
                    {
                        var up = input[i - 1][j];
                        if (Height(up) - Height(cell) <= 1)
                        {
                            graph.Connect(cell.N, up.N, 1);
                        }
                    }

                    if (j > 0)
                    {
                        var left = input[i][j - 1];
                        if (Height(left) - Height(cell) <= 1)
                        {
                            graph.Connect(cell.N, left.N, 1);
                        }
                    }

                    if (i + 1 < input.Count)
                    {
                        var down = input[i + 1][j];
                        if (Height(down) - Height(cell) <= 1)
                        {
                            graph.Connect(cell.N, down.N, 1);
                        }
                    }

                    if (j + 1 < input[0].Count)
                    {
                        var right = input[i][j + 1];
                        if (Height(right) - Height(cell) <= 1)
                        {
                            graph.Connect(cell.N, right.N, 1);
                        }
                    }
                }
            }

            var result = graph.Dijkstra(start, end);

            Console.WriteLine(result.Distance);

            var path = new HashSet<uint>(result.GetPath());
            Console.WriteLine(string.Join(Environment.NewLine, input.Select(row => string.Concat(row.Select(cell => path.Contains(cell.N) ? "X" : " ")))));

            var result2 = starts.Select(n => graph.Dijkstra(n, end)).MinBy(path => path.Distance);

            Console.WriteLine(result2.Distance);

            var path2 = new HashSet<uint>(result.GetPath());
            Console.WriteLine(string.Join(Environment.NewLine, input.Select(row => string.Concat(row.Select(cell => path2.Contains(cell.N) ? "X" : " ")))));
        }

        [Test]
        public void Day13()
        {
            Packet Parse(string input)
            {
                var result = new Packet { List = new List<Packet>() };
                for (var i = 0; i < input.Length; i++)
                {
                    var c = input[i];
                    switch (c)
                    {
                        case '[':
                            var child = new Packet { Parent = result, List = new List<Packet>() };
                            result.List.Add(child);
                            result = child;
                            break;
                        case ',':
                            break;
                        case ']':
                            result = result.Parent;
                            break;
                        default:
                            var n = c.ToString();
                            while (int.TryParse(input[i + 1].ToString(), out _))
                            {
                                i++;
                                n += input[i];
                            }

                            result.List.Add(new Packet { Value = int.Parse(n) });
                            break;
                    }
                }

                return result.List.Single();
            }

            var result = 0;

            var packets = new List<Packet>();

            var lines = File.ReadAllLines("C:\\git\\input13.txt");
            var index = 1;
            for (var i = 0; i < lines.Length; i += 3)
            {
                var left = Parse(lines[i]);
                var right = Parse(lines[i + 1]);
                Console.WriteLine(left);
                Console.WriteLine(right);

                var comparison = left.CompareTo(right);
                if (comparison < 0)
                {
                    result += index;
                }

                index++;

                packets.Add(left);
                packets.Add(right);
            }

            Console.WriteLine(result);

            var divider1 = Parse("[[2]]");
            var divider2 = Parse("[[6]]");
            packets.Add(divider1);
            packets.Add(divider2);

            var ordered = packets.OrderBy(x => x).ToList();

            Console.WriteLine((ordered.IndexOf(divider1) + 1) * (ordered.IndexOf(divider2) + 1));
        }

        [TestCase(1)]
        [TestCase(2)]
        public void Day14(int part)
        {
            var input = File.ReadLines("C:\\git\\input14.txt")
                .Select(line =>
                    line.Split(" -> ").Select(s =>
                        new Coord(int.Parse(s.Split(',')[0]), int.Parse(s.Split(',')[1]))).ToList()).ToList();

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

        [Test]
        public void Day15()
        {
            var input = File.ReadLines("C:\\git\\input15.txt")
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

        [TestCase(1)]
        [TestCase(2)]
        public void Day16(int part)
        {
            var timer = new Stopwatch();
            timer.Start();

            var graph = new Graph();
            var input = File.ReadLines("C:\\git\\input16.txt")
                .Select(line =>
                    Regex.Match(line, @"Valve (\w+) has flow rate=(\d+); tunnels? leads? to valves? ([\w, ]+)"))
                .Select((match) => (Node: graph.AddNode(), Name: match.Groups[1].Value, Flow: int.Parse(match.Groups[2].Value),
                    Tunnels: match.Groups[3].Value.Split(", ")))
                .ToDictionary(valve => valve.Name, valve => valve);

            var valves = input.Values
                .Select(valve => (valve.Node, valve.Name, valve.Flow, Tunnels: valve.Tunnels.Select(s => input[s].Node).ToList()))
                .ToDictionary(valve => valve.Node, valve => valve);

            foreach (var (from, _, _, tunnels) in valves.Values)
            {
                foreach (var to in tunnels)
                {
                    graph.Connect(from, to, 1);
                }
            }

            var pathLengths = new Dictionary<(uint, uint), int>();
            (int T, int Score) Move(uint from, uint to, (int T, int Score) previous)
            {
                if (!pathLengths.TryGetValue((from, to), out var dist))
                {
                    dist = graph.Dijkstra(from, to).Distance;
                    pathLengths.Add((from, to), dist);
                }

                var tEnd = previous.T - dist - 1;
                return (tEnd, previous.Score + (tEnd * valves[to].Flow));
            }

            var highScore = 0;
            var twelve = valves.Values.Where(valve => valve.Flow > 0).Select(valve => valve.Node).ToArray();
            var start = valves.Values.Single(valve => valve.Name == "AA").Node;
            Search(start, twelve, (part == 1 ? 30 : 26, 0), part == 1);
            void Search(uint from, uint[] too, (int T, int Score) previous, bool elephant)
            {
                foreach (var to in too)
                {
                    var move = Move(from, to, previous);
                    if (move.T >= 0)
                    {
                        if (move.Score > highScore)
                        {
                            highScore = move.Score;
                        }

                        if (too.Length > 1)
                        {
                            Search(to, too.Where(j => j != to).ToArray(), move, elephant);
                        }
                    }
                    else if (!elephant && previous.Score >= highScore / 2)
                    {
                        Search(start, too, (26, previous.Score), true);
                    }
                }
            }

            Console.WriteLine($"{highScore} (in {timer.ElapsedMilliseconds}ms)");
        }

        [TestCase(1)]
        [TestCase(2)]
        public void Day17(int part)
        {
            var timer = new Stopwatch();
            timer.Start();

            var blocks = part switch { 1 => 2022, 2 => 1000000000000 };

            var jets = File.ReadLines("C:\\git\\input17.txt").Single().ToCharArray();

            var sprites = new List<List<Coord<long>>>
            {
                new() { new(0, 0), new(1, 0), new(2, 0), new(3, 0) },
                new() { new(1, 0), new(0, 1), new(1, 1), new(2, 1), new(1, 2) },
                new() { new(0, 0), new(1, 0), new(2, 0), new(2, 1), new(2, 2) },
                new() { new(0, 0), new(0, 1), new(0, 2), new(0, 3) },
                new() { new(0, 0), new(0, 1), new(1, 0), new(1, 1) },
            };

            var spriteIndex = -1;
            var jetIndex = -1;

            List<Coord<long>> NextSprite()
            {
                spriteIndex = (spriteIndex + 1) % sprites.Count;
                return sprites[spriteIndex];
            }

            int NextJet()
            {
                jetIndex = (jetIndex + 1) % jets.Length;
                return jets[jetIndex] == '<' ? -1 : 1;
            }

            var tower = new HashSet<Coord<long>>(Enumerable.Range(0, 7).Select(i => new Coord<long>(i, 0)));

            (bool Success, List<Coord<long>> To) TryMove(List<Coord<long>> from, long x, long y)
            {
                var to = from.Select(c => new Coord<long>(c.X + x, c.Y + y)).ToList();
                if (to.Any(c => c.X < 0 || c.X > 6 || tower.Contains(c)))
                {
                    return (false, from);
                }

                return (true, to);
            }

            var sigs = new Dictionary<(long X, int SpriteIndex, int JetIndex), (long Blocks, long Height)>();

            long cycleLength = 0;
            long cycleHeight = 0;
            long cycleStability = 0;
            var cycleFound = false;

            long height = 0;
            for (long i = 0; i < blocks; i++)
            {
                var (_, next) = TryMove(NextSprite(), 2, height + 4);

                var sprite = next;
                while (true)
                {
                    var (_, jetted) = TryMove(sprite, NextJet(), 0);

                    var (success, fallen) = TryMove(jetted, 0, -1);
                    if (success)
                    {
                        sprite = fallen;
                    }
                    else
                    {
                        foreach (var coord in fallen)
                        {
                            tower.Add(coord);
                            height = Math.Max(height, coord.Y);
                        }

                        if (cycleFound)
                        {
                            if ((blocks - i - 1) % cycleLength == 0)
                            {
                                var cycles = (blocks - i - 1) / cycleLength;
                                Console.WriteLine($"Found by cycle detection: {height + (cycles * cycleHeight)} in {timer.ElapsedMilliseconds}ms");
                                return;
                            }
                        }
                        else
                        {
                            var sig = (fallen.Min(c => c.X), spriteIndex, jetIndex);
                            if (sigs.TryGetValue(sig, out var prevSig))
                            {
                                if (cycleLength == i - prevSig.Blocks && cycleHeight == height - prevSig.Height)
                                {
                                    cycleStability++;
                                }
                                else
                                {
                                    cycleLength = i - prevSig.Blocks;
                                    cycleHeight = height - prevSig.Height;
                                    cycleStability = 0;
                                }

                                if (cycleStability > cycleLength)
                                {
                                    cycleFound = true;
                                }
                            }

                            sigs[sig] = (i, height);
                        }

                        break;
                    }
                }
            }

            Console.WriteLine($"Found by enumeration: {height} in {timer.ElapsedMilliseconds}ms");
        }

        [Test]
        public void Day18()
        {
            var cubes = File.ReadLines("C:\\git\\input18.txt")
                .Select(s => s.Split(',').Select(int.Parse).ToList())
                .Select(c => (X: c[0], Y: c[1], Z: c[2])).ToList();

            var cubeHash = new HashSet<(int X, int Y, int Z)>(cubes);

            (int X, int Y, int Z) Move((int X, int Y, int Z) cube, int x, int y, int z) =>
                (cube.X + x, cube.Y + y, cube.Z + z);

            var result = cubes.Sum(cube =>
            {
                var l = Move(cube, 1, 0, 0);
                var r = Move(cube, -1, 0, 0);
                var f = Move(cube, 0, 1, 0);
                var b = Move(cube, 0, -1, 0);
                var u = Move(cube, 0, 0, 1);
                var d = Move(cube, 0, 0, -1);
                return 6 - new[] { l, r, f, b, u, d }.Count(c => cubeHash.Contains(c));
            });

            Console.WriteLine(result);

            var xMin = cubes.Min(cube => cube.X);
            var xMax = cubes.Max(cube => cube.X);
            var yMin = cubes.Min(cube => cube.Y);
            var yMax = cubes.Max(cube => cube.Y);
            var zMin = cubes.Min(cube => cube.Z);
            var zMax = cubes.Max(cube => cube.Z);

            var outside = new HashSet<(int X, int Y, int Z)>();

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
                            var cube = (x, y, z);
                            if (outside.Contains(cube) || cubeHash.Contains(cube))
                            {
                                continue;
                            }

                            if (x == xMin - 1 || y == yMin - 1 || z == zMin - 1 || x == xMax + 1 || y == yMax + 1 ||
                                z == zMax + 1)
                            {
                                outside.Add(cube);
                                found = true;
                                continue;
                            }

                            var l = Move(cube, 1, 0, 0);
                            var r = Move(cube, -1, 0, 0);
                            var f = Move(cube, 0, 1, 0);
                            var b = Move(cube, 0, -1, 0);
                            var u = Move(cube, 0, 0, 1);
                            var d = Move(cube, 0, 0, -1);
                            if (new[] { l, r, f, b, u, d }.Any(c => outside.Contains(c)))
                            {
                                outside.Add(cube);
                                found = true;
                            }
                        }
                    }
                }
            }

            var result2 = cubes.Sum(cube =>
            {
                var l = Move(cube, 1, 0, 0);
                var r = Move(cube, -1, 0, 0);
                var f = Move(cube, 0, 1, 0);
                var b = Move(cube, 0, -1, 0);
                var u = Move(cube, 0, 0, 1);
                var d = Move(cube, 0, 0, -1);
                return new[] { l, r, f, b, u, d }.Count(c => outside.Contains(c));
            });

            Console.WriteLine(result2);
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

        private record Coord(int X, int Y);

        private record Coord<T>(T X, T Y);

        private class Monkey
        {
            public Monkey(List<long> initialItems, Func<long, long> operation, Func<long, int> throwTo)
            {
                this.Items = new Queue<long>(initialItems);
                this.Operation = operation;
                this.ThrowTo = throwTo;
            }

            public Queue<long> Items { get; }

            public Func<long, long> Operation { get; }

            public Func<long, int> ThrowTo { get; }

            public long Inspections { get; set; }
        }

        private class Packet : IComparable<Packet>
        {
            public int Value { get; set; }

            public List<Packet> List { get; set; }

            public Packet Parent { get; set; }

            public int CompareTo(Packet other)
            {
                if (this.List == null)
                {
                    if (other.List == null)
                    {
                        return this.Value.CompareTo(other.Value);
                    }

                    return new Packet { List = new List<Packet> { this } }.CompareTo(other);
                }

                if (other.List == null)
                {
                    return this.CompareTo(new Packet { List = new List<Packet> { other } });
                }

                for (var i = 0; i < this.List.Count && i < other.List.Count; i++)
                {
                    var elementComparison = this.List[i].CompareTo(other.List[i]);
                    if (elementComparison != 0)
                    {
                        return elementComparison;
                    }
                }

                return this.List.Count.CompareTo(other.List.Count);
            }

            public override string ToString()
            {
                return this.List == null ? this.Value.ToString() : $"[{string.Join(',', this.List)}]";
            }
        }
    }
}
