namespace AdventOfCode;

using System;
using System.IO;
using NUnit.Framework;

[TestFixture]
public class Day2
{
    private enum Guess
    {
        Rock,
        Paper,
        Scissors,
    }

    private enum Result
    {
        Loss,
        Draw,
        Win,
    }

    [Test]
    public void Part1()
    {
        var total = 0;

        using var reader = new StreamReader("C:\\git\\advent-of-code\\input2.txt");
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

            var result = (theirGuess, myGuess) switch
            {
                (Guess.Rock, Guess.Rock) => Result.Draw,
                (Guess.Rock, Guess.Paper) => Result.Win,
                (Guess.Rock, Guess.Scissors) => Result.Loss,
                (Guess.Paper, Guess.Rock) => Result.Loss,
                (Guess.Paper, Guess.Paper) => Result.Draw,
                (Guess.Paper, Guess.Scissors) => Result.Win,
                (Guess.Scissors, Guess.Rock) => Result.Win,
                (Guess.Scissors, Guess.Paper) => Result.Loss,
                (Guess.Scissors, Guess.Scissors) => Result.Draw,
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
    public void Part2()
    {
        var total = 0;

        using var reader = new StreamReader("C:\\git\\advent-of-code\\input2.txt");
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