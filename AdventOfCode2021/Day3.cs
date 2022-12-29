using NUnit.Framework;

namespace AdventOfCode2021
{
    [TestFixture]
    public class Day3
    {
        [Test]
        public void Part1()
        {
            int[] counts = null;
            foreach (var input in File.ReadAllLines("C:\\git\\advent-of-code\\AdventOfCode2021\\input3.txt"))
            {
                counts ??= new int[input.Length];
                for (var i = 0; i < input.Length; i++)
                {
                    counts[i] += input[i] switch { '0' => -1, '1' => 1 };
                }
            }

            var gamma = 0;
            var epsilon = 0;
            foreach (var count in counts)
            {
                gamma *= 2;
                epsilon *= 2;
                switch (count)
                {
                    case > 0:
                        gamma++;
                        break;
                    case < 0:
                        epsilon++;
                        break;
                }
            }

            Console.WriteLine(gamma * epsilon);
        }

        [Test]
        public void Part2()
        {
            int[] Counts(string[] numbers)
            {
                var counts = new int[numbers[0].Length];
                foreach (var number in numbers)
                {
                    for (var i = 0; i < number.Length; i++)
                    {
                        counts[i] += number[i] switch { '0' => -1, '1' => 1 };
                    }
                }

                return counts;
            }

            int generator = 0;
            int scrubber = 0;
            var generators = File.ReadAllLines("C:\\git\\advent-of-code\\AdventOfCode2021\\input3.txt");
            for (var i = 0; i < generators[0].Length; i++)
            {
                var counts = Counts(generators);
                var keep = counts[i] >= 0 ? '1' : '0';
                generators = generators.Where(number => number[i] == keep).ToArray();
                if (generators.Length == 1)
                {
                    generator = Convert.ToInt32(generators[0], 2);
                    break;
                }
            }

            var scrubbers = File.ReadAllLines("C:\\git\\advent-of-code\\AdventOfCode2021\\input3.txt");
            for (var i = 0; i < scrubbers[0].Length; i++)
            {
                var counts = Counts(scrubbers);
                var keep = counts[i] < 0 ? '1' : '0';
                scrubbers = scrubbers.Where(number => number[i] == keep).ToArray();
                if (scrubbers.Length == 1)
                {
                    scrubber = Convert.ToInt32(scrubbers[0], 2);
                    break;
                }
            }

            Console.WriteLine(generator * scrubber);
        }
    }
}