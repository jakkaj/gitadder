using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitAdder
{
    class Program
    {
        static void Main(string[] args)
        {
            var g = new GitBits();
            var added = g.Scan();

            if (added)
            {
                Console.WriteLine("Press the any key to continue...");
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("Nothing found to add :)");
            }
        }
    }
}
