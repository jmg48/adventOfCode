namespace AdventOfCode;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;

[TestFixture]
public class Day8
{
    [Test]
    public void Test()
    {
        var visibleTrees = 0;
        var bestScore = 0;
        var grid = new List<List<int>>(File.ReadLines("C:\\git\\advent-of-code\\input8.txt")
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
}