using common;

namespace aoc2022
{
    [AocDay(14, Caption = "Regolith Reservoir")]
    internal class Day14
    {
        [AocTask(1)]
        public int Task1()
        {
            var input = AocInput.GetLines(14).Select(i => i.Split(" -> ").Select(c => c.Split(',').Select(int.Parse)).Select(c => new Point2(c.First(), c.Last())).ToList()).ToList();

            var span = input.SelectMany(i => i).Span();
            var map = new Map<char>('.', span.width + 1, span.y1+1);
            DrawPaths(input, span, map);

            int grains = 0, emitX = 500-span.x0, emitY = 0;
            while (DropGrain(map, emitX, emitY)) grains++;
            return grains;
        }

        [AocTask(2)]
        public int Task2()
        {
            var input = AocInput.GetLines(14).Select(i => i.Split(" -> ").Select(c => c.Split(',').Select(int.Parse)).Select(c => new Point2(c.First(), c.Last())).ToList()).ToList();
            
            var span = input.SelectMany(i => i).Span();
            span.y1 += 2;
            span.x0 = 500 - span.y1; // Diagonal from emitter
            span.x1 = 500 + span.y1; // Diagonal from emitter
            input.Add(new List<Point2>() { new(span.x0, span.y1), new(span.x1, span.y1) });
            var map = new Map<char>('.', span.width + 1, span.y1 + 1);
            DrawPaths(input, span, map);

            int grains = 0, emitX = 500 - span.x0, emitY = 0;
            while(DropGrain(map, emitX, emitY))
            {
                grains++;
                if (map.Get(emitX, emitY) == 'o') break; // exit condition
            }
            return grains;
        }

        private static void DrawPaths(List<List<Point2>> paths, Rectangle span, Map<char> map)
        {
            foreach (var path in paths)
            {
                var from = path[0];
                for (int i = 1; i < path.Count; i++) // For each line segment in path
                {
                    var to = path[i];
                    if (from.y != to.y) // Vertical line
                    {
                        for (int y = int.Min(from.y, to.y); y <= int.Max(from.y, to.y); y++) map.Set(from.x - span.x0, y, '#');
                    }
                    else if (from.x != to.x) // Horizontal line
                    {
                        for (int x = int.Min(from.x, to.x); x <= int.Max(from.x, to.x); x++) map.Set(x - span.x0, from.y, '#');
                    }
                    from = to;
                }
            }
        }

        private static bool DropGrain(Map<char> map, int grainX, int grainY)
        {
            char c;
            while (true)
            {
                if ((c = map.Check(grainX, grainY + 1)) == default(char)) return false; // Check downward move - is grain outside map?
                if (c == '.')
                {
                    grainY++; continue;
                }
                if ((c = map.Check(grainX - 1, grainY + 1)) == default(char)) return false; // Check down-left move - is grain outside map?
                if (c == '.')
                {
                    grainY++; grainX--; continue;
                }
                if ((c = map.Check(grainX + 1, grainY + 1)) == default(char)) return false; // Check down-right move - is grain outside map?
                if (c == '.')
                {
                    grainY++; grainX++; continue;
                }
                return map.Set2(grainX, grainY, 'o'); // Grain landed - add it to map
            }
        }
    }
}
