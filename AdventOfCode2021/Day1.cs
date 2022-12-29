using NUnit.Framework;

namespace AdventOfCode2021
{
    [TestFixture]
    public class Day1
    {
        [TestCase(1)]
        [TestCase(2)]
        public void Test(int part)
        {
            var window = part switch { 1 => 1, 2 => 3 };

            var input = File.ReadAllLines("C:\\git\\advent-of-code\\AdventOfCode2021\\input1.txt").Select(int.Parse)
                .ToList();

            var result = 0;
            for (var i = 0; i < input.Count - window; i++)
            {
                var a = input.Skip(i).Take(window).Sum();
                var b = input.Skip(i + 1).Take(window).Sum();
                if (b > a)
                {
                    result++;
                }
            }

            Console.WriteLine(result);
        }
    }
}