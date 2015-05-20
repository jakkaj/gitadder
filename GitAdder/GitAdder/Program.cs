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
            g.Scan();

            Console.ReadKey();
        }
    }
}
