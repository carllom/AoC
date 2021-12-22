using aoc2k21;
using System.Diagnostics;

/// <summary>
/// https://adventofcode.com/2021
/// </summary>

var tasks = new (IAocTask Prog, string InputFile)[]
{
    new (new Day1(), "data/1-sonar-sweep.txt"),
    new (new Day2(), "data/2-dive.txt"),
    new (new Day3(), "data/3-binary-diagnostic.txt"),
    new (new Day4(), "data/4-giant-squid.txt"),
    new (new Day5(), "data/5-hydrothermal-venture.txt"),
    new (new Day6(), "data/6-lanternfish.txt"),
    new (new Day7(), "data/7-treachery-of-whales.txt"),
    new (new Day8(), "data/8-seven-segment-search.txt"),
    new (new Day9(), "data/9-smoke-basin.txt"),
    new (new Day10(), "data/10-syntax-scoring.txt"),
    new (new Day11(), "data/11-dumbo-octopus.txt"),
    new (new Day12(), "data/12-passage-pathing.txt"),
    new (new Day13(), "data/13-transparent-origami.txt"),
    new (new Day14(), "data/14-extended-polymerization.txt"),
    new (new Day15(), "data/15-chiton.txt"),
    new (new Day16(), "data/16-packet-decoder.txt"),
    new (new Day17(), ""), // Input is hardcoded in class
    new (new Day18(), "data/18-snailfish.txt"),
    new (new Day19(), "data/19-beacon-scanner.txt"),
    new (new Day20(), "data/20-trench-map.txt"),
    new (new Day21(), ""), // Input is hardcoded in class
    //new (new DayX(), "data/X-???.txt"),
};

// Param "all" runs all days
if (args.Length > 0 && args[0].ToLowerInvariant() == "all")
{
    for (int i = 0; i < tasks.Length; i++)
    {
        var t = tasks[i];
        Run(i+1, t.Prog, t.InputFile);
    }
    return;
}

// Param is Day# to run (default to last day in task list)
if (args.Length == 0 || !int.TryParse(args[0].ToString(), out var day))
    day = tasks.Length;

day = Math.Clamp(day, 1, tasks.Length); // Ensure day# is in valid range

var task = tasks[day - 1];
Run(day, task.Prog, task.InputFile);


static void Run(int day, IAocTask task, string inputFile)
{
    var sw = new Stopwatch();
    Console.WriteLine($"=== Day {day} ===");
    sw.Restart();
    var task1result = task.Task1(inputFile);
    sw.Stop();
    Console.WriteLine($"Task #1({sw.Elapsed.TotalMilliseconds}ms) = {task1result}");
    sw.Restart();
    var task2result = task.Task2(inputFile);
    sw.Stop();
    Console.WriteLine($"Task #2({sw.Elapsed.TotalMilliseconds}ms) = {task2result}");
}