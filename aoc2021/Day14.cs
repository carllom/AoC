using System.Text;

namespace aoc2021
{
    internal class Day14 : IAocTask
    {
        public long Task1(string indatafile)
        {
            var (initial, rules) = ReadData(indatafile);
            var polymer = initial;
            for (int j = 0; j < 10; j++)
            {
                var sb = new StringBuilder(polymer[0..1]);
                for (int i = 0; i < polymer.Length-1; i++)
                {
                    sb.Append(rules[polymer[i..(i+2)]]);
                    sb.Append(polymer[i+1]);
                }
                polymer = sb.ToString();
            }
            var count = new Dictionary<char, int>();
            foreach (var c in polymer)
            {
                if (!count.ContainsKey(c)) count.Add(c, 1);
                else count[c]++;
            }
            return count.Values.Max() - count.Values.Min();
        }

        public long Task2(string indatafile)
        {
            var (initial, rules) = ReadData(indatafile);
            var count = new Dictionary<char, long>();
            var lookup = new Dictionary<string, Dictionary<char,long>>();
            foreach (var item in initial)
            {
                if (!count.ContainsKey(item)) count.Add(item, 1);
                else count[item]++;
            }
            var cres = DoStep(initial, rules, lookup, 39);
            count.Merge(cres);
            return count.Values.Max() - count.Values.Min(); ;
        }

        private Dictionary<char,long> DoStep(string polymer, Dictionary<string,string> rules, Dictionary<string,Dictionary<char,long>> lookup, int depth) 
        {
            if (lookup.ContainsKey(polymer+depth)) return lookup[polymer+depth]; // We already had this in the lookup

            var res = new Dictionary<char, long>();
            for (int i = 0; i < polymer.Length-1; i++)
            {
                var exp = rules[polymer[i..(i+2)]][0];
                if (depth > 0)
                {
                    var c1 = DoStep($"{polymer[i]}{exp}", rules, lookup, depth-1); // For AxB, expand Ax
                    var c2 = DoStep($"{exp}{polymer[i+1]}", rules, lookup, depth-1); // For AxB, expand xB
                    res.Merge(c1).Merge(c2);
                }

                if (!res.ContainsKey(exp)) res.Add(exp, 1);
                else res[exp]++;
            }

            lookup.Add(polymer+depth, res);
            return res;
        }

        (string initial, Dictionary<string, string> rules) ReadData(string indatafile)
        {
            var f = File.OpenText(indatafile);
            var initial = f.ReadLine() ?? "";
            f.ReadLine(); // Read blank line
            string? l;
            var rules = new Dictionary<string, string>();
            while ((l = f.ReadLine()) != null)
            {
                var tok = l.Split("->");
                rules.Add(tok[0].Trim(), tok[1].Trim());
            }
            return (initial, rules);
        }
    }

    public static class Day14Ext
    {
        public static Dictionary<char,long> Merge(this Dictionary<char,long> dict, Dictionary<char, long> other)
        {
            foreach (var kv in other)
            {
                if (!dict.ContainsKey(kv.Key)) dict.Add(kv.Key, kv.Value);
                else dict[kv.Key] += kv.Value;
            }
            return dict;
        }
    }
}
