using common;

namespace aoc2022
{
    [AocDay(16, Caption = "Proboscidea Volcanium")]
    internal class Day16
    {
        [AocTask(1)]
        public int Task1()
        {
            var lines = AocInput.GetLines(16, true);
            var nodes = new List<VNode>(lines.Length);
            foreach (var line in lines)
            {
                var id = line.Substring(6, 2);
                var rate = int.Parse(line.Split('=', ';')[1]);
                var l = line.Substring(line.IndexOf("valve"));
                var nextIds = l.Substring(l.IndexOf(' ')).Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                VNode? bleh = nodes.FirstOrDefault(j => j.id == id);
                if (bleh == null) { bleh = new VNode { id = id }; nodes.Add(bleh); }
                bleh.rate = rate;
                foreach (var nbid in nextIds)
                {
                    VNode nextB = nodes.FirstOrDefault(j => j.id == nbid);
                    if (nextB == null) { nextB = new VNode { id = nbid }; nodes.Add(nextB); }
                    bleh.next.Add(nextB);
                    bleh.nextidx.Add(nodes.IndexOf(nextB));
                }
            }

            var distmap = GenDistMap(nodes.ToArray());
            var toprates = nodes.Select((n, idx) => (n.rate, idx)).OrderByDescending(r => r.rate);

            var minutes = 30;
            var at = 0; // AA
            var taken = new List<int>() { };
            var totalscore = 0;
            var ratesum = 0;
            while (minutes > 0)
            {
                var maxscore = 0;
                var msidx = -1;
                var timetaken = 0;
                for (int i = 0; i < nodes.Count; i++)
                {
                    if (i==at) continue; // Do not go to same node
                    if (nodes[i].rate <= 0) continue; // No point visiting a broken valve or revisiting a valve
                    var dn = distmap[at][i];
                    var time = dn.distance+1;
                    if (time > minutes) continue; // No time to open valve
                    var score = (minutes - time) * nodes[i].rate; // Get total benefit of valve
                    if (score > maxscore) { maxscore = score; msidx = i; timetaken = time; }
                }
                if (msidx > 0)
                {
                    minutes -= timetaken; // Time taken (travel+valve)
                    totalscore += maxscore; // Add total benefit of opened valve
                    at = msidx; // Move to new location
                    ratesum += nodes[at].rate;
                    Console.WriteLine($"@{minutes} left, opened {nodes[at].id}({nodes[at].rate}) for a benefit of {maxscore}, sum to {ratesum} making the total {totalscore}");
                    nodes[at].rate = -nodes[at].rate; // Mark valve as opened
                }
            }

            foreach (var tr in toprates)
            {
                
            }

            return totalscore;
        }

        private void Spelunk2(DPath[][] distances, IEnumerable<VNode> nodes, int minutes)
        {
            if (minutes<=0) return;

            foreach (var n in nodes)
            {
                var rest = nodes.Where(x => x!=n);
                //Spelunk2(distances, rest, distances[n. minutes);
            }
        }


        private class DPath { public int distance; public int[] path; public DPath(int distance, int[] path) { this.distance = distance; this.path = path; } }

        private DPath[][] GenDistMap(VNode[] nodes)
        {
            var paths = new DPath[nodes.Length][];
            for (int fromIdx = 0; fromIdx < nodes.Length; fromIdx++)
            {
                paths[fromIdx] = new DPath[nodes.Length];
                for (int toIdx = 0; toIdx < nodes.Length; toIdx++)
                {
                    if (fromIdx == toIdx)
                    {
                        paths[fromIdx][toIdx] = new DPath(0, Array.Empty<int>());
                        continue;
                    }

                    var pred = new int[nodes.Length];
                    var dist = new int[nodes.Length];
                    if (BFS(nodes.ToArray(), fromIdx, toIdx, pred, dist))
                    {
                        paths[fromIdx][toIdx] = new DPath(dist[toIdx], pred);
                    }
                }
            }
            return paths;
        }

        //private (int points,string path) Spelunk(VNode current, HashSet<string> opened, int minutes)
        //{
        //    if (minutes <= 0) return (0, current.id); // Nothing left to do

        //    var (points, path) = (0, "");
        //    foreach (var next in current.next)
        //    {
        //        var (subpoints, subpath) = Spelunk(next, opened, minutes-1);
        //        if (subpoints > points)
        //        {
        //            path = subpath;
        //            points = subpoints;
        //        }
        //    }

        //    path = $"{current.id}:{path}";
        //    if (!opened.Contains(current.id) && current.rate > 0)
        //    {
        //        opened.Add(current.id);
        //        minutes--;
        //        points+= current.rate * minutes;
        //    }
        //    return (points, path);
        //}

        [AocTask(2)]
        public int Task2()
        {
            var input = AocInput.GetLines(16);
            return default;
        }

        private static bool BFS(VNode[] adj, int src, int dest, int[] pred, int[] dist)
        {
            Queue<int> queue = new Queue<int>();
            bool[] visited = new bool[adj.Length];

            for (int i = 0; i < adj.Length; i++)
            {
                visited[i] = false;
                dist[i] = int.MaxValue;
                pred[i] = -1;
            }
            visited[src] = true;
            dist[src] = 0;
            queue.Enqueue(src);

            // BFS
            while (queue.Count != 0)
            {
                int u = queue.Dequeue();

                for (int i = 0; i < adj[u].next.Count; i++)
                {
                    if (visited[adj[u].nextidx[i]] == false)
                    {
                        visited[adj[u].nextidx[i]] = true;
                        dist[adj[u].nextidx[i]] = dist[u] + 1;
                        pred[adj[u].nextidx[i]] = u;
                        queue.Enqueue(adj[u].nextidx[i]);

                        if (adj[u].nextidx[i] == dest)
                            return true;
                    }
                }
            }
            return false;
        }
    }
    public class VNode
    {
        public string id;
        public int rate;
        public List<VNode> next = new List<VNode>();
        public List<int> nextidx = new List<int>();
        public VNode route;
        public override string ToString()
        {
            var ids = next.Aggregate("", (a, b) => $"{a} {b.id}");
            return $"{id}({rate}) => {ids}";
        }
    }
}
