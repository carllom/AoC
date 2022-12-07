using common;

namespace aoc2022
{
    [AocDay(6, Caption = "Tuning Trouble")]
    internal class Day6
    {
        [AocTask(1)]
        public int Task1() => QuickerFindDistinct(AocInput.GetText(6), 4);

        [AocTask(2)]
        public int Task2() => QuickerFindDistinct(AocInput.GetText(6), 14);

        private int FindDistinct(string data, int size)
        {
            for (int i = 0; i < data.Length - size; i++)
            {
                if (data.Skip(i).Take(size).Distinct().Count() == size) return i + size; // Wasteful but elegant
            }
            return int.MinValue;
        }

        // More efficient solution utilizing a shifting "window" of varying size.
        // a) Grow the window to the right to include a new character.
        // b) If the added character gives a dupe inside the window we shift window start past the duplicate
        // c) We continue until there are no dupes within the window and the window is of expected size
        private int QuickerFindDistinct(string data, int size)
        {
            var winStart = 0; // Window start index
            var b2eSize = size-1; // "inner" size (distance between window start & end)
            for (int winEnd = 1; winEnd < data.Length; winEnd++)
            {
                var dupe = false;
                winStart = Math.Max(winStart, winEnd-b2eSize); // Shift window start if full size
                for (int i = winEnd-1; i >= winStart; i--) // Start scan closest to window end to maximize shift
                {
                    if (data[i]==data[winEnd])
                    {
                        dupe = true;
                        winStart = i+1; // Shift winStart enough to exclude duplicate character
                        break;
                    }
                }
                if (!dupe && winEnd-winStart == b2eSize) return winEnd+1; // no dupe and full window size means we found it
            }
            return -1;
        }
    }
}
