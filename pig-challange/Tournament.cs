using System;
using System.Collections.Generic;

namespace pig_challenge
{
    struct Result
    {
        public int scoreA, scoreB;
    }

    class Tournament
    {
        private List<int[]> scoreList;
        private int totalScoreAgentA;
        private int totalScoreAgentB;
        private int NumberOfGames;
        private int MaxIterations;

        public Tournament(int numberOfGames, int maxIterations)
        {
            this.scoreList = new List<int[]>();
            this.totalScoreAgentA = 0;
            this.totalScoreAgentB = 0;

            this.NumberOfGames = numberOfGames;
            this.MaxIterations = maxIterations;
        }

        public Result Run(AgentConfiguration configurationA, AgentConfiguration configurationB)
        {
            for (int i = 0; i < this.NumberOfGames; i++)
            {
                Game game = new Game(this.MaxIterations, configurationA, configurationB);
                State endState = game.Run();
                this.scoreList.Add(new int[] { endState.ScoreAgentA, endState.ScoreAgentB });
            }

            //Console.WriteLine("Scores for the two agents:");
            for (int i = 0; i < this.NumberOfGames; i++)
            {
                //Console.WriteLine($" Agent A: {this.scoreList[i][0]}, Agent B: {this.scoreList[i][1]}");
                this.totalScoreAgentA += this.scoreList[i][0];
                this.totalScoreAgentB += this.scoreList[i][1];
            }
            //Console.WriteLine($"Total scores: \n Agent A: {this.totalScoreAgentA}, Agent B: {this.totalScoreAgentB}");

            return new Result { scoreA = this.totalScoreAgentA, scoreB = this.totalScoreAgentB };
            //Console.ReadLine();
        }
    }
}
