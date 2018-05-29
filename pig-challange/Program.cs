using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pig_challange
{
    class Program
    {
        const int NUM_GAMES = 1;
        const int MAX_ITERATIONS = 25;
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the pig challange!");
            Console.WriteLine("Press Enter to start a new tournament.");

            Tournament tournament = new Tournament(NUM_GAMES, MAX_ITERATIONS);
            tournament.Run();
        }
    }
}
