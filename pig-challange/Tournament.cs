using System;
using System.Collections.Generic;

namespace pig_challenge
{
    class Tournament
    {
        private List<int[]> scoreList;
        private int totalScoreAgentA;
        private int totalScoreAgentB;
        private int NumberOfGames;
        private int MaxIterations;

        public Tournament(int numberOfGames, int maxIterations)
        {
            scoreList = new List<int[]>();
            totalScoreAgentA = 0;
            totalScoreAgentB = 0;

            this.NumberOfGames = numberOfGames;
            this.MaxIterations = maxIterations;
        }

        public void Run()
        {
            for (int i = 0; i < this.NumberOfGames; i++)
            {
                Game game = new Game(this.MaxIterations);
                State endState = game.Run();
                scoreList.Add(new int[] { endState.ScoreAgentA, endState.ScoreAgentB });
            }

            Console.WriteLine("Scores for the two agents:");
            for (int i = 0; i < this.NumberOfGames; i++)
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
