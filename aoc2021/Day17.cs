namespace aoc2021
{
    internal class Day17 : IAocTask
    {
        public long Task1(string indatafile)
        {
            int txMin = 240, txMax = 292, tyMin = -90, tyMax = -57; //target area: x=240..292, y=-90..-57
            int yMax = 0; // highest y

            int x0vMin = 0, q = 0;
            while (q<txMin) q += x0vMin++; // Min x0Vel must be high enough so we reach the target area before stopping
            for (var x0v = x0vMin-1; x0v <= txMax/2; x0v++)
            {
                for (var y0v = 0; y0v < -tyMin; y0v++) // When y = y0 again it has yVel = -y0Vel
                {
                    (int x, int y, int xv, int yv) p = (0, 0, x0v, y0v); // probe
                    int yApex = 0;
                    while (p.x < txMax && p.y > tyMin) // step while we still have not passed target
                    {
                        Step(ref p);
                        if (p.y > yApex) yApex = p.y;
                        if (p.x >=txMin && p.x <= txMax && p.y >= tyMin && p.y <= tyMax) // inside target?
                        {
                            yMax = Math.Max(yMax, yApex);
                            break;
                        }
                    }
                }
            }
            return yMax;
        }

        private void Step (ref (int x, int y, int xv, int yv) p)
        {
            p.x+=p.xv; p.y+=p.yv; // update pos
            p.yv--; // gravity
            if (p.xv > 0) p.xv--; // drag
        }

        public long Task2(string indatafile)
        {
            int txMin = 240, txMax = 292, tyMin = -90, tyMax = -57; //target area: x=240..292, y=-90..-57
            int hits = 0; // number of hits in the target area

            int x0vMin = 0, q = 0;
            while (q<txMin) q += x0vMin++; // Min x0Vel must be high enough so we reach the target area before stopping from drag
            for (var x0v = x0vMin-1; x0v <= txMax; x0v++)
            {
                for (var y0v = tyMin; y0v < -tyMin; y0v++) // When y = y0 again it has yVel = -y0Vel
                {
                    (int x, int y, int xv, int yv) p = (0, 0, x0v, y0v); // probe
                    while (p.x < txMax && p.y > tyMin) // step while we still have not passed target
                    {
                        Step(ref p);
                        if (p.x >=txMin && p.x <= txMax && p.y >= tyMin && p.y <= tyMax) // inside target?
                        {
                            hits++;
                            break;
                        }
                    }
                }
            }
            return hits;
        }
    }
}
