using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pig_challange
{
    class Game
    {
        private int MAX_ITERATIONS;
        private State state;

        
        public enum ExitCodes
        {
            InProgress = -1,
            IterationsExceeded = 3,
            AgentAQuit = 0,
            AgentBQuit = 1,
            PigCaught = 2
        }

        private Agent agentA, agentB;
        private Pig pig;
        private Map map;

        private Random randomizer;
        
        public Game(int MAX_ITERATIONS)
        {
            this.randomizer = new Random();
            this.MAX_ITERATIONS = MAX_ITERATIONS;

            this.agentA = new Agent(0); 
            this.agentB = new Agent(1);
            this.pig = new Pig();
            this.map = new Map();

            this.state = new State(MAX_ITERATIONS);
            state.PlaceAgents(this.GetRandomStartPosition(), this.GetRandomStartPosition(), this.GetRandomStartPosition());
            
        }

        public State RunGame()
        {
            this.map.Draw(-1, this.state);
            for (int i = 0; i < MAX_ITERATIONS; i++)
            {
                this.agentA.DetermineStep(this.map, this.state);
                this.EvaluateGameState();
                if(this.state.ExitCode > -1)
                    return this.state;

                this.agentB.DetermineStep(this.map, this.state);
                this.EvaluateGameState();
                if (this.state.ExitCode > -1)
                    return this.state;

                this.pig.DetermineStep(this.map, this.state);
                this.EvaluateGameState();
                if (this.state.ExitCode > -1)
                    return this.state;

                this.map.Draw(i + 1, this.state);
                Console.ReadLine();
            }

            throw new Exception("the game end check didn't fire successfully");
        }

        //Adjust the gamestate according to evaluation
        private void EvaluateGameState()
        {
            //Player A has won
            if (this.map.IsCellExit(this.state.PositionA[0], this.state.PositionA[1]))
            {
                this.state.SetWinnerA();
            }

            //Player B has won
            if (this.map.IsCellExit(this.state.PositionB[0], this.state.PositionB[1]))
            {
                this.state.SetWinnerB();
            }

            //The pig was caught, i.e. the pig has no adjacent squares that it can move to
            if (this.map.AvailablePositions(this.state.PositionPig, state).Count() == 0)
            {
                this.state.SetWinnerBoth();
            }

            //The turns ran out
            if (this.state.Turns == 0)
            {
                this.state.SetOutOfTurns();
            }
        }

        //unpredictable: find all suitable positions and randomly select one
        private int[] GetRandomStartPosition()
        {
            int x = 0, y = 0;
            while (true)
            {
                x = this.randomizer.Next(2, 6);
                y = this.randomizer.Next(2, 6);

                if (!this.map.IsCellEmpty(y, x,this.state))
                {
                    continue;
                }

                break;
            }

            return new int[] { y, x };
        }
    }
}
