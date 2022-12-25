using common;

namespace aoc2022
{
    [AocDay(25, Caption = "Full of Hot Air")]
    internal class Day25
    {
        [AocTask(1)]
        public string Task1()
        {
            var input = AocInput.GetLines(25, false);
            var sum = new List<char>(); // Digits in reverse order
            char cb = '0'; // Carry/Borrow
            foreach (var line in input)
            {
                var lastsum = new string(sum.ToArray());
                var digits = new Stack<char>(line);
                var pos = 0;
                while (digits.Count > 0)
                {
                    var d = digits.Pop();
                    if (pos >= sum.Count) sum.Add('0');
                    sum[pos] = Add(d, sum[pos++], ref cb);
                }
                while (cb != '0') // Add remaining carry/borrow
                {
                    if (pos >= sum.Count) sum.Add('0');
                    sum[pos] = Add('0', sum[pos++], ref cb);
                }
            }
            return new string(sum.AsEnumerable().Reverse().ToArray());
        }

        private char Add(char a, char b, ref char cb)
        {
            var dsum = S2D(a) + S2D(b) + S2D(cb);
            if (dsum > 2) { cb = '1'; dsum-=5; } // Generated carry
            else if (dsum < -2) { cb = '-'; dsum+=5; } // Generated borrow
            else { cb = '0'; }
            return D2S(dsum);
        }

        private int S2D(char s) => s switch { '2' => 2, '1' => 1, '0' => 0, '-' => -1, '=' => -2, _ => throw new ApplicationException($"Illegal SNAFU digit {s}") };
        private char D2S(int d) => "=-012"[d+2];
    }
}
