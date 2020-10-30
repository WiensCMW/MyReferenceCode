using System;
using System.Diagnostics;

namespace CSVParser
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Stopwatch watch = new Stopwatch();
            watch.Restart();
            CParser.CSVParse();

            Console.WriteLine($"Parse took {watch.Elapsed.TotalMilliseconds}ms");
            Console.ReadLine();
        }
    }
}
