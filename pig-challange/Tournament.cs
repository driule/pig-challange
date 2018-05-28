using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pig_challange
{
    class Tournament
    {
        private List<int[]> scoreList;
        private int totalScoreAgentA;
        private int totalScoreAgentB;
        private int NUM_GAMES;
        private int MAX_ITERATIONS;

        public Tournament(int NUM_GAMES, int MAX_ITERATIONS)
        {
            scoreList = new List<int[]>();
            totalScoreAgentA = 0;
            totalScoreAgentB = 0;

            this.NUM_GAMES = NUM_GAMES;
            this.MAX_ITERATIONS = MAX_ITERATIONS;
        }

        public void RunTournament()
        {
            for (int i = 0; i < NUM_GAMES; i++)
            {
                Game game = new Game(MAX_ITERATIONS);
                State endState = game.RunGame();
                scoreList.Add(new int[] { endState.scoreA, endState.scoreB });
            }

            Console.WriteLine("Scores for the two agents:");
            for (int i = 0; i < NUM_GAMES; i++)
            {
                Console.WriteLine($" Agent A: {scoreList[i][0]}, Agent B: {scoreList[i][1]}");
                totalScoreAgentA += scoreList[i][0];
                totalScoreAgentB += scoreList[i][1];
            }
            Console.WriteLine($"Total scores: \n Agent A: {totalScoreAgentA}, Agent B: {totalScoreAgentB}");

            Console.ReadLine();
        }
    }
}
