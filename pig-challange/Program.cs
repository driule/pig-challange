using System;

namespace pig_challenge
{
    class Program
    {
        const int NUM_GAMES = 1;
        const int MAX_ITERATIONS = 25;

        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the pig challenge!");
            Console.WriteLine("Press Enter to start a new tournament.");

            AgentConfiguration agentConfigurationA = new AgentConfiguration { alpha = 0.1f, beta = 0.2f, gamma = 0.3f, delta = 0.2f, minCooperationLimit = 0.75f, maxDefectLimit = 0.25f };
            AgentConfiguration agentConfigurationB = new AgentConfiguration { alpha = 0.1f, beta = 0.2f, gamma = 0.3f, delta = 0.1f, minCooperationLimit = 0.75f, maxDefectLimit = 0.25f };

            Tournament tournament = new Tournament(NUM_GAMES, MAX_ITERATIONS);
            tournament.Run(agentConfigurationA, agentConfigurationB);
        }
    }
}
