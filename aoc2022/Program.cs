using common;
using System.Reflection;

public class Program
{
    public static int Main(string[] args)
    {
        Console.WriteLine("=========================");
        Console.WriteLine("== Advent of Code 2022 ==");
        Console.WriteLine("=========================");

        var engine = new AocTaskEnumerator(Assembly.GetExecutingAssembly());

        if (args.Length > 0)
        {
            if (!int.TryParse(args[0], out int day))
            {
                Console.Error.WriteLine($"Expected single integer (day) argument");
                return -1;
            }
            engine.Execute(day);
        }
        else
        {
            engine.Execute();
        }
        return 0;
    }
}