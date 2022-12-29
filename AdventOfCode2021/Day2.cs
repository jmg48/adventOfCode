using NUnit.Framework;

namespace AdventOfCode2021
{
    [TestFixture]
    public class Day2
    {
        public void Part1()
        {
            var x = 0;
            var z = 0;
            foreach (var (direction, distance) in File.ReadAllLines("C:\\git\\advent-of-code\\AdventOfCode2021\\input2.txt")
                         .Select(s => s.Split(' ')).Select(x => (Direction: x[0], Distance: int.Parse(x[1]))))
            {
                (x, z) = direction switch
                {
                    "forward" => (x + distance, z),
                    "down" => (x, z + distance),
                    "up" => (x, z - distance),
                };
            }

            Console.WriteLine(x * z);
        }

        [Test]
        public void Part2()
        {
            var x = 0;
            var z = 0;
            var aim = 0;
            foreach (var (direction, distance) in File.ReadAllLines("C:\\git\\advent-of-code\\AdventOfCode2021\\input2.txt")
                         .Select(s => s.Split(' ')).Select(x => (Direction: x[0], Distance: int.Parse(x[1]))))
            {
                (x, z, aim) = direction switch
                {
                    "forward" => (x + distance, z + (aim * distance), aim),
                    "down" => (x, z, aim + distance),
                    "up" => (x, z, aim - distance),
                };
            }

            Console.WriteLine(x * z);
        }
    }
}