using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pig_challange
{
    class GameBoard
    {
        const int MAX_ITERATIONS = 25;

        public enum GameState
        {
            InProgress = -1,
            IterationsExceeded = 0,
            AgentAQuit = 1,
            AgentBQuit = 2,
            PigCaught = 3
        }

        private Agent agentA, agentB;
        private Pig pig;
        private Map map;

        private Random randomizer;
        
        public GameBoard()
        {
            this.randomizer = new Random();

            this.agentA = new Agent(); 
            this.agentB = new Agent();
            this.pig = new Pig();

            this.map = new Map(this.agentA, this.agentB, this.pig);

            this.agentA.Position = this.GetRandomStartPosition();
            this.agentB.Position = this.GetRandomStartPosition();
            this.pig.Position = this.GetRandomStartPosition();
        }

        public void RunGame()
        {
            for (int i = 0; i < MAX_ITERATIONS; i++)
            {
                this.agentA.DetermineStep(this.map);
                this.agentB.DetermineStep(this.map);
                this.pig.DetermineStep(this.map);

                this.map.Draw();

                GameState state = this.EvaluateGameState(i + 1);

                if (state != GameState.InProgress)
                {
                    this.HandleGameEnding(state);
                    break;
                }
            }
        }

        // TODO: implement game end (if pig was caught or agent exited) and score tracking
        private GameState EvaluateGameState(int iteration)
        {
            if (iteration == MAX_ITERATIONS)
            {
                Console.WriteLine("Game finished!");

                return GameState.IterationsExceeded;
            }

            if (this.map.IsCellExit(this.agentA.Position.Item1, this.agentA.Position.Item2))
            {
                Console.WriteLine("Agent A quit");

                return GameState.AgentAQuit;
            }

            if (this.map.IsCellExit(this.agentB.Position.Item1, this.agentB.Position.Item2))
            {
                Console.WriteLine("Agent B quit");

                return GameState.AgentBQuit;
            }

            // pig caught
            int y = this.pig.Position.Item1;
            int x = this.pig.Position.Item2;

            if (!this.map.IsCellEmpty(y + 1, x) && !this.map.IsCellEmpty(y, x + 1) && !this.map.IsCellEmpty(y - 1, x) && !this.map.IsCellEmpty(y, x - 1))
            {
                Console.WriteLine("Pig was caught");

                return GameState.PigCaught;
            }

            return GameState.InProgress;
        }

        private void HandleGameEnding(GameState state)
        {
            // TODO: handle score
        }

        private Tuple<int,int> GetRandomStartPosition()
        {
            int x = 0, y = 0;
            while (true)
            {
                x = this.randomizer.Next(2, 6);
                y = this.randomizer.Next(2, 6);

                if (!this.map.IsCellEmpty(y, x))
                {
                    continue;
                }

                break;
            }

            return new Tuple<int, int>(y, x);
        }
    }
}
