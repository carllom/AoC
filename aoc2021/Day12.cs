namespace aoc2021
{
    internal class Day12 : IAocTask
    {
        public long Task1(string indatafile)
        {
            var indata = File.ReadAllLines(indatafile).Select(l => l.Split('-')).ToArray();
            indata = indata.Concat(indata.Where(e => e[0] != "start" && e[1] != "end").Select(e => new[] { e[1], e[0] })).ToArray(); // add reversed connections except for start & end
            var graph = new Dictionary<string, List<string>>();
            foreach (var item in indata) // Convert to dictionary form
            {
                if (!graph.ContainsKey(item[0])) graph.Add(item[0], new List<string>() { item[1] }); else if (!graph[item[0]].Contains(item[1])) graph[item[0]].Add(item[1]);
                if (!graph.ContainsKey(item[1])) graph.Add(item[1], new List<string>() { item[0] }); else if (!graph[item[1]].Contains(item[0])) graph[item[1]].Add(item[0]);
            }
            var visitedCaves = new List<string>();
            var routes = SearchCave(graph, visitedCaves, "start");
            return routes.Count();
        }

        private List<string> SearchCave(Dictionary<string, List<string>> graph, IEnumerable<string> visited, string path)
        {
            var cave = path.Split('-').Last();
            if (cave == "end") return new List<string>() { path }; // No more spelunking
            if (cave.ToLowerInvariant() == cave) visited = visited.Concat(new[] { cave }); // Only visit small caves once
            var result = new List<string>();
            foreach (var nextCave in graph[cave])
            {
                if (visited.Contains(nextCave)) continue; // Already visited this cave
                result.AddRange(SearchCave(graph, visited, $"{path}-{nextCave}"));
            }
            return result;
        }
        public long Task2(string indatafile)
        {
            var indata = File.ReadAllLines(indatafile).Select(l => l.Split('-')).ToArray();
            indata = indata.Concat(indata.Where(e => !e.Contains("start") && !e.Contains("end")).Select(e => e.Reverse().ToArray())).ToArray(); // add reversed connections except for start & end
            var graph = new Dictionary<string, List<string>>();
            foreach (var item in indata) // Convert to dictionary form
            {
                if (!graph.ContainsKey(item[0])) graph.Add(item[0], new List<string>() { item[1] }); else if (!graph[item[0]].Contains(item[1])) graph[item[0]].Add(item[1]);
                if (!graph.ContainsKey(item[1])) graph.Add(item[1], new List<string>() { item[0] }); else if (!graph[item[1]].Contains(item[0])) graph[item[1]].Add(item[0]);
            }
            IEnumerable<string> routes = new List<string>();
            foreach (var specialInterest in indata.Where(c => c[1].IsLower() && c[1] != "end" && c[1] != "start").Select(c => c[1]).Distinct()) // Select one of the small caves to be of special interest
            {
                routes = routes.Concat(SearchCave2(graph, "", specialInterest, "start")).Distinct(); // optimized linq hotspots sacrificing readability => about twice the speed
            }
            return routes.Count();
        }

        private List<string> SearchCave2(Dictionary<string, List<string>> graph, string visited, string specialInterest, string path)
        {
            var cave = path[(path.LastIndexOf('-')+1)..]; // Last segment in path
            if (cave == "end") return new List<string>() { path }; // No more spelunking
            if (cave.IsLower()) visited = $"{visited} {cave}"; // Record visiting small caves 

            var result = new List<string>();
            foreach (var nextCave in graph[cave])
            {
                if (nextCave != specialInterest && visited.Contains(nextCave)) continue; // Already visited this cave
                if (nextCave == specialInterest && visited.CountContains(nextCave) > 1) continue; // Special interest caves can be visited twice
                result.AddRange(SearchCave2(graph, visited, specialInterest, $"{path}-{nextCave}"));
            }
            return result;
        }
    }

    internal static class Day12Ext
    {
        public static bool IsLower(this string s) => s.ToLowerInvariant() == s;

        public static int CountContains(this string s, string other)
        {
            int n = 0, i = 0;
            while ((i = s.IndexOf(other, i, StringComparison.Ordinal)) != -1) // Use ordinal - we know the cave names are only [a..zA..Z]
            {
                i+= other.Length; // Skip search past token
                n++;
            }
            return n;
        }
    }
}