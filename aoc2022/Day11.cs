using common;
using common.Algorithms;

namespace aoc2022
{
    [AocDay(11, Caption = "Monkey in the Middle")]
    internal class Day11
    {
        private Monkey[] GetInput() => new[] // Hardcoded - I don't feel like parsing files with arithmetic expressions...
        {
            new Monkey(new long[]{66,79},old => old*11,7, x => x ? 6 : 7),
            new Monkey(new long[]{84,94,94,81,98,75},old => old*17,13, x => x ? 5 : 2),
            new Monkey(new long[]{85,79,59,64,79,95,67},old => old+8,5, x => x ? 4 : 5),
            new Monkey(new long[]{70},old => old+3,19, x => x ? 6 : 0),
            new Monkey(new long[]{57,69,78,78},old => old+4,2, x => x ? 0 : 3),
            new Monkey(new long[]{65,92,60,74,72},old => old+7,11, x => x ? 3 : 4),
            new Monkey(new long[]{77,91,91},old => old*old,17, x => x ? 1 : 7),
            new Monkey(new long[]{76,58,57,55,67,77,54,99},old => old+6,3, x => x ? 2 : 1),
        };

        [AocTask(1)]
        public int Task1()
        {
            var monkeys = GetInput();
            for (int round = 0; round < 20; round++)
            {
                for (int i = 0; i < monkeys.Length; i++)
                {
                    while (monkeys[i].Items.Any())
                    {
                        var item = monkeys[i].Items.Dequeue();
                        monkeys[i].Inspections++;
                        var worry = monkeys[i].Operation(item) / 3;
                        var nextMonkey = monkeys[i].ThrowTo((worry % monkeys[i].Test) == 0); // modulo 0 => divisible by
                        monkeys[nextMonkey].Items.Enqueue(worry);
                    }
                }
            }
            var top2 = monkeys.Select(m => m.Inspections).OrderByDescending(i => i).ToArray();
            return top2[0] * top2[1];
        }

        [AocTask(2)]
        public long Task2()
        {
            var monkeys = GetInput();
            var lcm = monkeys.Aggregate(1L, (a, b) => a.Lcm(b.Test)); // Find least common multiple for the divisors/modulos (monkey tests)
            for (int round = 0; round < 10000; round++)
            {
                for (int i = 0; i < monkeys.Length; i++)
                {
                    while (monkeys[i].Items.Any())
                    {
                        var item = monkeys[i].Items.Dequeue();
                        monkeys[i].Inspections++;
                        item = item % lcm; // lcm is "safe" to rotate around as it is divisible by all monkey divisors(tests).
                        var worry = monkeys[i].Operation(item);
                        var nextMonkey = monkeys[i].ThrowTo((worry % monkeys[i].Test) == 0);
                        monkeys[nextMonkey].Items.Enqueue(worry);
                    }
                }
            }
            var top2 = monkeys.Select(m => (long)m.Inspections).OrderByDescending(i => i).ToArray();
            return top2[0] * top2[1];
        }
    }

    class Monkey
    {
        public Queue<long> Items = new Queue<long>();
        public Func<long, long> Operation;
        public int Test;
        public Func<bool, int> ThrowTo;
        public int Inspections;

        public Monkey(IEnumerable<long> items, Func<long, long> op, int test, Func<bool, int> throwto)
        {
            foreach (var item in items) { Items.Enqueue(item); }
            Operation = op; Test = test; ThrowTo = throwto;
        }
    }
}
