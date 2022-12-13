using common;
using System.Diagnostics.CodeAnalysis;

namespace aoc2022
{
    [AocDay(13, Caption = "Distress Signal")]
    internal class Day13 : IComparer<LItem>
    {
        [AocTask(1)]
        public int Task1()
        {
            var input = AocInput.GetLines(13);

            List<LItem> l = new(), r = new();
            for (int i = 0; i < input.Length; i+=3)
            {
                l.Add(ParseItems(input[i]));
                r.Add(ParseItems(input[i+1]));
            }

            int sum = 0;
            for (int i = 0; i < l.Count; i++)
            {
                var m = MatchPackets(l[i], r[i]);
                if (m != MatchResult.Mismatch) sum+=(i+1);
            }
            return sum;
        }

        [AocTask(2)]
        public int Task2()
        {
            var input = AocInput.GetLines(13);
            List<LItem> packets = new();
            foreach (var line in input) if (line.Length > 0) packets.Add(ParseItems(line));
            var div2 = ParseItems("[[2]]");
            packets.Add(div2);
            var div6 = ParseItems("[[6]]");
            packets.Add(div6);
            packets.Sort(this); // "this" implements ICompare<LItem>
            return (packets.IndexOf(div2) + 1) * (packets.IndexOf(div6) + 1);
        }

        private enum MatchResult { Match = -1, Continue = 0, Mismatch = 1 }

        public int Compare(LItem? x, LItem? y) => (int)MatchPackets(x!, y!);

        private MatchResult MatchPackets(LItem l, LItem r)
        {
            if (l.number.HasValue && r.number.HasValue) return l.number == r.number ? MatchResult.Continue : l.number < r.number ? MatchResult.Match : MatchResult.Mismatch; // both numbers

            if (l.number.HasValue)
            {
                LItem nl = new(); nl.children?.Add(l);
                l = nl;
            }
            if (r.number.HasValue)
            {
                LItem nr = new(); nr.children?.Add(r);
                r = nr;
            }

            if (l.children != null && r.children != null) // both lists
            {
                for (int i = 0; i < l.children.Count; i++)
                {
                    if (i>=r.children.Count) return MatchResult.Mismatch; // starve right hand side 
                    var m = MatchPackets(l.children[i], r.children[i]);
                    if (m == MatchResult.Continue) continue;
                    return m;
                }
                // if all values on the left equals those on the right and there are more items on the right we have a match. If the arrays are identical we have a tie (continue)
                return l.children.Count == r.children.Count ? MatchResult.Continue : MatchResult.Match;
            }
            throw new ApplicationException(); // Should not get here
        }

        private LItem ParseItems(string line)
        {
            var root = new LItem();
            var curr = root;
            string number = string.Empty;
            foreach (var c in line[1..^1])
            {
                switch(c)
                {
                    case '[': // new list
                        curr = new LItem(curr);
                        break;
                    case ']': // end list
                        if (number.Length > 0) new LItem(curr, int.Parse(number));
                        number = string.Empty;
                        curr = curr.parent;
                        break;
                    case ',':
                        if (number.Length > 0) new LItem(curr, int.Parse(number));
                        number = string.Empty;
                        break;
                    default: // number
                        number += c;
                        break;
                }
            }
            if (number.Length > 0) new LItem(curr, int.Parse(number));
            return root;
        }
    }


    public class LItem
    {
        public LItem? parent;
        public List<LItem>? children;
        public int? number;
        public LItem() { children = new(); }
        public LItem(LItem p) { children = new(); parent = p; parent.children.Add(this); }
        public LItem(LItem p, int num) { number = num; parent = p; parent.children.Add(this); }

        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            if (obj is not LItem) return false;
            var o = (LItem)obj;
            return ToString().Equals(o.ToString());
        }
        public override int GetHashCode() => HashCode.Combine(ToString());

        public override string ToString()
        {
            return number.HasValue ? number.ToString() : $"[{children.Aggregate("", (s, c) => (s=="" ? c.ToString() : $"{s},{c}"))}]";
        }
    }
}
