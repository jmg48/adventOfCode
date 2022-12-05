namespace AdventOfCode
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
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
    }
}
