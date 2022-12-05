namespace AdventOfCode
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
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
    }
}
