using RoyT.AStar;
using System;
using System.Collections.Generic;
using System.Linq;

namespace pig_challenge
{
    class Game
    {
        private int MaxIterations;
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
        
        public Game(int maxIterations)
        {
            this.MaxIterations = maxIterations;

            this.agentA = new Agent(BasicAgent.AgentIdentifier.AgentA); 
            this.agentB = new Agent(BasicAgent.AgentIdentifier.AgentB);
            this.pig = new Pig();
            this.map = new Map();

            this.state = new State(this.MaxIterations);
            state.PlaceAgents(this.map);
        }

        public State Run()
        {
            this.map.Draw(0, this.state);
            for (int i = 0; i < this.MaxIterations; i++)
            {
                this.agentA.DetermineStep(this.map, this.state);
                this.EvaluateGameState();
                if (this.state.ExitCode != Game.ExitCodes.InProgress)
                {
                    this.map.Draw(i + 1, this.state);

                    return this.state;
                }

                this.agentB.DetermineStep(this.map, this.state);
                this.EvaluateGameState();
                if (this.state.ExitCode != Game.ExitCodes.InProgress)
                {
                    this.map.Draw(i + 1, this.state);

                    return this.state;
                }

                this.pig.DetermineStep(this.map, this.state);
                this.EvaluateGameState();
                if (this.state.ExitCode != Game.ExitCodes.InProgress)
                {
                    this.map.Draw(i + 1, this.state);

                    return this.state;
                }

                this.map.Draw(i + 1, this.state);
                Console.ReadLine();
            }

            throw new Exception("the game end check didn't fire successfully");
        }

        private void EvaluateGameState()
        {
            // agent A has won
            if (this.map.IsCellExit(this.state.PositionAgentA.X, this.state.PositionAgentA.Y))
            {
                this.state.SetWinnerA();
            }

            // agent B has won
            if (this.map.IsCellExit(this.state.PositionAgentB.X, this.state.PositionAgentB.Y))
            {
                this.state.SetWinnerB();
            }

            // the pig was caught, i.e. the pig has no adjacent squares that it can move to
            if (this.map.GetAvailablePositions(this.state.PositionPig, state).Count() == 0)
            {
                this.state.SetWinnerBoth();
            }

            // the turns ran out
            if (this.state.TurnsLeft == 0)
            {
                this.state.SetOutOfTurns();
            }
        }


        private void CalculateConditionalProbabilities(BasicAgent.AgentIdentifier id)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="map"></param>
        /// <param name=""></param>
        /// <returns></returns>
        public List<float> GetPCMConditionalProbabilities(Map map, State state, BasicAgent.AgentIdentifier id)
        {
            //First call GetPMCConditionalPropabilities and GetCooperationPrior, then calculate for all m and all c: P(m|c) * P(c), put them in a temporary list. 
            //  Then calculate over all m and all c: P(c|m) = ( P(m|c) * P(c)) / SUM_C( P(m|i)*P(i) ). So only bottom half really needs to be calculated and 
            //  we use the precalculated ones from the list created.

            return new List<float>();
        }

        /// <summary>
        /// Returns Conditional probabilities in P(0|m1),.. P(0|m4), P(1|m1) order 
        /// </summary>
        /// <param name="map"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public List<float> GetPMCConditionalProbabilities(Map map, State state, BasicAgent.AgentIdentifier id)
        {
            Position position = state.GetPosition(id);

            List<Position> availablePositions = map.GetAvailablePositions(position, state);

            //This calculates the amount of steps until the position of the pig is reached, so we could subtract 1 to get to the position just before the pig
            //Basically, I actually wanted to calculate from the availablePosition to next to the pig, and then add 1 tot the total,
            //  but the GetPath method actually includes the startPosition in the path, so the +1 is not needed.
            List<int> costs = availablePositions
                                    .Select(availablePosition => (int)Math.Pow(map.GetPathToPigFromPosition(state, availablePosition).Count(), 2))
                                    .ToList();

            float sum = (float)costs.Sum();

            List<float> probabilities = costs
                                            .Select(cost => ((float)cost / (float)sum))
                                            .ToList();

            int count = probabilities.Count;
            for (int i = 0; i < count; i++)
            {
                probabilities.Add(1 - probabilities[i]);
            }

            return probabilities;
        }

        private float GetCooperationPrior(State state)
        {
            return 0.0f;

            //Use conditional probability matrix from state to get the probability of cooperation from last turn. 
            // Or when it's the first turn, use the prior?



        }
    }
}
