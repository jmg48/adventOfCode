namespace AdventOfCode;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using NUnit.Framework;

[TestFixture]
public class Day17
{
    [TestCase(1)]
    [TestCase(2)]
    public void Test(int part)
    {
        var timer = new Stopwatch();
        timer.Start();

        var blocks = part switch { 1 => 2022, 2 => 1000000000000 };

        var jets = File.ReadLines("C:\\git\\advent-of-code\\input17.txt").Single().ToCharArray();

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
}