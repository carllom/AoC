using common;

namespace aoc2022
{
    [AocDay(7, Caption = "No Space Left On Device")]
    internal class Day7
    {
        [AocTask(1)]
        public long Task1()
        {
            return BuildTree(AocInput.GetLines(7)).ListDirs().Where(d => d.TotalSize <= 100000).Sum(d => d.TotalSize);
        }

        [AocTask(2)]
        public long Task2()
        {
            var root = BuildTree(AocInput.GetLines(7));
            var spaceneeded = 30000000 - (70000000 - root.TotalSize);
            return root.ListDirs().Where(d => d.TotalSize > spaceneeded).OrderBy(d => d.TotalSize).First().TotalSize;
        }

        private Node BuildTree(IEnumerable<string> lines)
        {
            var root = new Node(null, "/");
            var curr = root;
            foreach (var line in lines.Skip(1))
            {
                if (line.StartsWith("$ ls")) continue;
                if (line.StartsWith("$ cd")) { var dir = line.Split()[2]; curr = dir == ".." ? curr.Parent : curr = curr.Children.Single(n => n.Name == dir); continue; };
                if (line.StartsWith("dir")) { curr.Children.Add(new Node(curr, line.Substring(4))); continue; };
                curr.Children.Add(new Node(curr, line.Split()[1], int.Parse(line.Split()[0])));
            }
            return root;
        }
    }

    public class Node
    {
        public string Name { get; set; }
        public long Size { get; set; }
        public Node Parent { get; set; }
        public List<Node> Children { get; set; } = new List<Node>();
        public Node(Node parent, string name, long size) { Parent = parent; Name = name; Size = size; }
        public Node(Node parent, string name) { Parent = parent; Name = name; Size = 0; }
        public long TotalSize => Size + Children.Sum(c => c.TotalSize);
        public IEnumerable<Node> ListDirs()
        {
            var dirs = new List<Node>() { this };
            dirs.AddRange(Children.Where(c => c.Size == 0).SelectMany(c => c.ListDirs()));
            return dirs;
        }
        //private string Path => Parent == null ? Name : $"{Parent.Path}/{Name}";
        //public override string ToString() => $"{Path} ({TotalSize})";
    }
}
