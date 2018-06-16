using System;

namespace pig_challenge
{
    class Program
    {
        const int NUM_GAMES = 100;
        const int MAX_ITERATIONS = 25;

        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the pig challenge!");
            Console.WriteLine("Press Enter to start a new tournament.");

            Tournament tournament = new Tournament(NUM_GAMES, MAX_ITERATIONS);
            tournament.Run();
        }
    }
}
