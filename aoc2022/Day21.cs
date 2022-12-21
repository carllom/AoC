using common;

namespace aoc2022
{
    [AocDay(21, Caption = "Monkey Math")]
    internal class Day21
    {
        [AocTask(1)]
        public long Task1()
        {
            var input = AocInput.GetLines(21)
                .Select(l => l.Split(':', StringSplitOptions.TrimEntries))
                .Select(l => new MathMonkey(l[0], l[1].Split(' '))).ToList();
            foreach (var line in input)
            {
                if (!line.number.HasValue)
                {
                    line.depa = input.First(mm => mm.name == line.opa);
                    line.depb = input.First(mm => mm.name == line.opb);
                }
            }
            return FindNumber(input.First(mm => mm.name == "root"));
        }

        [AocTask(2)]
        public long Task2()
        {
            var input = AocInput.GetLines(21, true)
                .Select(l => l.Split(':', StringSplitOptions.TrimEntries))
                .Select(l => new MathMonkey(l[0], l[1].Split(' '))).ToList();
            foreach (var line in input)
            {
                if (!line.number.HasValue)
                {
                    line.depa = input.First(mm => mm.name == line.opa);
                    line.depb = input.First(mm => mm.name == line.opb);
                }
            }

            var root = input.First(mm => mm.name == "root");
            var me = input.First(mm => mm.name == "humn");
            var deponme = input.Where(mm => mm.depa == me || mm.depb == me);

            var mynumber = 0L;
            var u1 = FindNumber(root.depa, mynumber);
            var u2 = FindNumber(root.depb, mynumber);
            while (u2 != u1) {
                var diff = Math.Abs(u1-u2);
                var logd = Convert.ToInt32(Math.Max(1, Math.Floor(Math.Log(diff))));
                mynumber += diff < 10 ? 1 : diff < 30000 ? diff/10 : diff/logd;
                u1 = FindNumber(root.depa, mynumber);
            }
            return mynumber;
        }

        private long FindNumber(MathMonkey mm, long? mynumber = null)
        {
            if (mm.name == "humn" && mynumber.HasValue) return mynumber.Value;
            if (mm.number.HasValue) return mm.number.Value;
            long numa = FindNumber(mm.depa, mynumber);
            long numb = FindNumber(mm.depb, mynumber);
            return mm.op switch
            {
                "*" => numa * numb,
                "/" => numa / numb,
                "-" => numa - numb,
                "+" => numa + numb,
                _ => throw new ApplicationException("oops")
            };
        }
    }

    public class MathMonkey
    {
        public string name;
        public string opa;
        public string opb;
        public string op;
        public long? number;

        public MathMonkey depa;
        public MathMonkey depb;

        public MathMonkey(string name, string[] ops)
        {
            this.name=name;
            if (ops.Length == 1) number = long.Parse(ops[0]);
            else
            {
                opa=ops[0];
                op=ops[1];
                opb=ops[2];
            }
        }
    }
}
