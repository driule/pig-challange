using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pig_challange
{
    class GameBoard
    {
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

            this.map = new Map();

            this.agentA.Position = this.GetRandomStartPosition();
            this.agentB.Position = this.GetRandomStartPosition();
            this.pig.Position = this.GetRandomStartPosition();
        }

        public void RunGame()
        {
            for (int i = 0; i < 15; i++)
            {
                this.agentA.DetermineStep(this.map, this.pig, this.agentB);
                this.agentB.DetermineStep(this.map, this.pig, this.agentA);
                this.pig.DetermineStep(this.map, this.agentA, this.agentB);

                this.map.Draw(this.agentA, this.agentB, this.pig);

                this.EvaluateGameState();
            }
        }

        // TODO: implement game end (if pig was caught or agent exited) and score tracking
        private void EvaluateGameState()
        {
        }

        private Tuple<int,int> GetRandomStartPosition()
        {
            int x = 0, y = 0;
            while (true)
            {
                x = this.randomizer.Next(2, 6);
                y = this.randomizer.Next(2, 6);

                if (!this.map.IsCellEmpty(x, y, this.agentA, this.agentB, this.pig))
                {
                    continue;
                }

                break;
            }

            return new Tuple<int, int>(y, x);
        }
    }
}
