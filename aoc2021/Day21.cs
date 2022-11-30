namespace aoc2021
{
    internal class Day21 : IAocTask
    {
        private const int p1_initpos = 5; // Player 1 initial position
        private const int p2_initpos = 9; // Player 2 initial position

        public long Task1(string indatafile)
        {
            (int pos, int points)[] indata = new[] { (p1_initpos-1, 0), (p2_initpos-1, 0) };
            int maxpoints = 1000;
            int nthrows = 0;
            int die = 0;
            int player = 0;

            while (indata[0].points < maxpoints && indata[1].points < maxpoints)
            {
                var roll = (die++ % 100) + (die++ % 100) + (die++ % 100) + 3; // die rotates through 0..99, but target range is 1..0 so three rolls need to add 3
                die = die % 100; // Reset die if necessary
                nthrows += 3;
                indata[player].pos = (indata[player].pos + roll)%10;
                indata[player].points += indata[player].pos + 1; // pos rotates through 0..9, but points are based on posnumbers 1..10 => add 1
                player++; player %= 2;
            }

            return nthrows * Math.Min(indata[0].points, indata[1].points);
        }

        public long Task2(string indatafile)
        {
            // Variables represent # of outcomes per the number of rounds to finish (21+ points) or not finish (<21) per starting position
            var (p1finish, p1nofinish) = CalcOutcomes(p1_initpos-1);
            var (p2finish, p2nofinish) = CalcOutcomes(p2_initpos-1);
            long p1wins = 0;
            long p2wins = 0;

            for (int r = 2; r < p1finish.Length; r++) // Game cannot finish until round 3 (r=2) - impossible to reach 21 before that
            {
                p1wins += p1finish[r] * p2nofinish[r-1]; // Player 1 has an advantage since he rolls first, so the number of combinations for a win is P1 finishing combinations times P2 non-finishing combinations in the last round
                p2wins += p2finish[r] * p1nofinish[r]; // Player 2 wins on P2 finishing combinations times P1 non-finishing combinations for the same round
            }

            return Math.Max(p1wins, p2wins);
        }

        private int[] d3x3 = new[] { 0, 0, 0, 1, 3, 6, 7, 6, 3, 1 }; // Number of outcomes per 3xD3 sum
        private const int winpoints = 21;
        private (long[] finish, long[] nofinish) CalcOutcomes(int initialpos)
        {
            var finish =new long[winpoints];
            var nofinish =new long[winpoints];
            // We need to keep record of outcome objects containing: # of outcomes, # of points and current position.
            OutcomeRecord?[] last = new OutcomeRecord?[winpoints + 10]; // We will never reach more than 30 points (20 + 10) before the records are removed (>= 21)

            var init_ocr = FindRecord(last, 0); // We have 0 points initially
            init_ocr.pos_outcomes[initialpos] = 1; // 1 combination for pos p0

            for (int round = 0; round < finish.Length; round++) // Iterate over rounds (1..21)
            {
                OutcomeRecord?[] current = new OutcomeRecord?[winpoints + 10]; // outcome records for current step
                for (int points = 0; points<last.Length; points++)
                {
                    var ocr = last[points];
                    if (ocr == null) continue; // no record with this number of points

                    for (int p = 0; p < ocr.pos_outcomes.Length; p++) // Iterate over previous positions
                    {
                        var ocrOutcome = ocr.pos_outcomes[p];
                        if (ocrOutcome == 0) continue; // no point in continuing 0 combinations (will be multiplied by 0)
                        for (int d = 0; d<d3x3.Length; d++) // Lookup # of outcomes for dice throw. 3xD3 gives sum in range [3..9]
                        {
                            if (d3x3[d] == 0) continue;
                            var newposidx = (p + d) % 10; // new position index is old index + dice throw (modulo 10)
                            var newpoints = (newposidx+1) + ocr.points; // add the new position [1..10] to the points
                            var newocr = FindRecord(current, newpoints);
                            newocr.pos_outcomes[newposidx] += ocrOutcome * d3x3[d]; // new # of outcomes is every previous outcome times # of dice outcomes for current sum
                        }
                    }
                }
                for (int points = 0; points < current.Length; points++) // add result of current round to outcome arrays
                {
                    if (current[points]==null) continue; // Skip points with no outcomes
                    var arr = points < winpoints ? nofinish : finish; // Select finished/non-finished array based on whether points are enough to win game
                    foreach (var oc in current[points].pos_outcomes) arr[round] += oc; // add all outcomes with this number of points
                    if (points >= winpoints) current[points] = null; // clear finished games
                }
                last = current; // Save current step outcome 
            }
            return (finish, nofinish);
        }

        private OutcomeRecord FindRecord(OutcomeRecord?[] recs, int points) => recs[points] == null ? recs[points] = new OutcomeRecord(points) : recs[points];

        private class OutcomeRecord
        {
            public readonly int points;
            public readonly long[] pos_outcomes = new long[10];
            public OutcomeRecord(int points)
            {
                this.points=points;
            }
        }
    }
}