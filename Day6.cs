namespace AdventOfCode;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;

[TestFixture]
public class Day6
{
    [TestCase(1)]
    [TestCase(2)]
    public void Test(int part)
    {
        var n = part switch { 1 => 4, 2 => 14 };
        var input = File.ReadAllText("C:\\git\\input6.txt");
        Console.WriteLine(Enumerable.Range(n, input.Length - n).First(i => new HashSet<char>(input.Skip(i - n).Take(n)).Count == n));
    }
}