namespace AdventOfCode;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Dijkstra.NET.Graph.Simple;
using Dijkstra.NET.ShortestPath;
using NUnit.Framework;

[TestFixture]
public class Day12
{
    [Test]
    public void Test()
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
}