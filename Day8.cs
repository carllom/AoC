namespace aoc2k21
{
    internal class Day8 : IAocTask
    {
        public long Task1(string indatafile)
        {
            var indata = File.ReadAllLines(indatafile).Select(l => l.Split('|')[1].Split(' ', StringSplitOptions.RemoveEmptyEntries)).ToArray();

            var count = 0;
            foreach (var item in indata)
            {
                for (int i = 0; i < item.Length; i++)
                {
                    if (item[i].Length < 5 || item[i].Length == 7) count++;
                }
            }
            return count;
        }

        public long Task2(string indatafile)
        {
            var indata = File.ReadAllLines(indatafile).Select(l => l.Split('|')).ToArray();

            long total = 0;
            foreach (var p in indata)
            {
                var signals = p[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(s => new string(s.OrderBy(c => c).ToArray())).OrderBy(c => c).ToArray(); // sorted signals+digits, 
                var digits = p[1].Split(' ', StringSplitOptions.RemoveEmptyEntries);

                var digitone = signals.FirstOrDefault(s => s.Length == 2); // 1 is easy to optimize on - just two combination possibilities
                char[] resultmap = (digitone != null? // if we have an encoded '1', try calculate with the two possible combinations already set - otherwise brute force all combinations
                    FindMatchingPermutation(new char[7].Map(digitone[0],'c').Map(digitone[1],'f'), signals) ?? FindMatchingPermutation(new char[7].Map(digitone[0], 'f').Map(digitone[1], 'c'), signals) :
                    FindMatchingPermutation(new char[7], signals)) 
                    ?? throw new Exception("Bummer! No combination works..");

                total += TranslateNumber(digits, resultmap);
            }

            return total;
        }

        private string[] validDigits = { "abcefg", "cf", "acdeg", "acdfg", "bcdf", "abdfg", "abdefg", "acf", "abcdefg", "abcdfg" };

        private char[]? FindMatchingPermutation(char[] startPermutation, string[] encodedData) => CalcPermutations(startPermutation, (map) => encodedData.All(enc => Array.IndexOf(validDigits, Translate(enc, map)) > -1));

        private int TranslateNumber(string[] digits, char[] map) => digits.Select(d => TranslateDigit(d, map)).Aggregate(0, (acc, digit) => acc * 10 + digit);

        private int TranslateDigit(string from, char[] map) => Array.IndexOf(validDigits, Translate(from, map));

        private string Translate(string from, char[] map) => new string(from.Select(c => map[c - 'a']).OrderBy(c => c).ToArray());

        private char[]? CalcPermutations(char[] map, Func<char[], bool> permValidator)
        {
            var i = Array.IndexOf(map, (char)0); // First unset segment in permutation
            if (i == -1) return permValidator(map) ? map : null; // Permutation complete - validate

            var choices = "abcdefg".Where(x => !map.Contains(x)).ToArray();
            for (int j = 0; j < choices.Length; j++)
            {
                var newmap = (char[]) map.Clone();
                newmap[i] = choices[j]; // select next value
                var res = CalcPermutations(newmap, permValidator);
                if (res != null) return res;
            }
            return null;
        }
    }

    public static class Ext8
    {
        public static char[] Map(this char[] encoding, char encoded, char decoded) { encoding[encoded - 'a'] = decoded; return encoding; }
    }
}
