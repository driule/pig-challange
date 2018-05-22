using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pig_challange
{
    class Program
    {
        const int NUM_GAMES = 5;
        static void Main(string[] args)
        {
            Tuple<int, int>[] scoreList = new Tuple<int, int>[NUM_GAMES];
            int totalScoreAgentA = 0;
            int totalScoreAgentB = 0;

            Console.WriteLine("Welcome to pig challange!");

            for (int i = 0; i < NUM_GAMES; i++)
            {
                GameBoard gameBoard = new GameBoard();
                scoreList[i] = gameBoard.RunGame();
            }

            Console.WriteLine("Scores for the two agents:");
            for (int i = 0; i < NUM_GAMES; i++)
            {
                Console.WriteLine($" Agent A: {scoreList[i].Item1}, Agent B: {scoreList[i].Item2}");
                totalScoreAgentA += scoreList[i].Item1;
                totalScoreAgentB += scoreList[i].Item2;
            }
            Console.WriteLine($"Total scores: \n Agent A: {totalScoreAgentA}, Agent B: {totalScoreAgentB}");

            Console.ReadLine();
        }
    }
}
