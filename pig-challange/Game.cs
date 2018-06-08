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

            this.agentA = new Agent(BasicAgent.AgentIdentifier.AgentA, 0.2f, 0.5f, 0.3f); 
            this.agentB = new Agent(BasicAgent.AgentIdentifier.AgentB, 0.2f, 0.5f, 0.3f);
            this.pig = new Pig();
            this.map = new Map();

            this.state = new State(this.MaxIterations);
            state.PlaceAgents(this.map);
        }

        public State Run()
        {
            List<float> tempPCM_Matrix;
            this.map.Draw(0, this.state);
            for (int i = 0; i < this.MaxIterations; i++)
            {
                tempPCM_Matrix = this.GetPCMConditionalProbabilities(map, state, BasicAgent.AgentIdentifier.AgentA);
                this.agentA.DetermineStep(this.map, this.state);
                this.CalculateAndUpdateCooperationPrior(state, tempPCM_Matrix, BasicAgent.AgentIdentifier.AgentA);
                this.EvaluateGameState();
                if (this.state.ExitCode != Game.ExitCodes.InProgress)
                {
                    this.map.Draw(i + 1, this.state);

                    return this.state;
                }

                tempPCM_Matrix = this.GetPCMConditionalProbabilities(map, state, BasicAgent.AgentIdentifier.AgentB);
                this.agentB.DetermineStep(this.map, this.state);
                this.CalculateAndUpdateCooperationPrior(state, tempPCM_Matrix, BasicAgent.AgentIdentifier.AgentB);
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
            if (this.map.GetAvailablePositions(this.state.PositionPig, state).Count() == 1)
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
            List<float> PMCs = this.GetPMCConditionalProbabilities(map, state, id);
            float cooperationProbability = state.GetCooperationProbability(id);

            List<float> precalculatedPmiXPi = PMCs.Select((PMC, index) =>
            {
                if (index <= 4)
                    return PMC * cooperationProbability;
                else //index > 5, so we need to use P(-c) 
                    return PMC * (1.0f - cooperationProbability);
            }).ToList();

            List<float> PCMs = PMCs.Select((PMC, index) =>
            {
                if (PMC == 0.0f)
                    return 0.0f;

                var upper = precalculatedPmiXPi[index];
                int div = index / 5; //0..4=>0, 5..9=>1
                int mulFactor = (1 - 2 * div); // 0=>1, 1=>-1
                int otherIndex = mulFactor * 5 + index; // results in: index of 0 <=> 5, 1 <=> 6
                return precalculatedPmiXPi[index] / (precalculatedPmiXPi[index] + precalculatedPmiXPi[otherIndex]);
            }).ToList();

            //Calculate P(m) by going over all SUM_C( P(m|i) * P(i) ) //TODO: smartly

            return PCMs;
        }

        /// <summary>
        /// Returns Conditional probabilities in P(0|m1),.. P(0|m5), P(1|m1) order 
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
            List<int> pigCosts = availablePositions
                                    .Select(availablePosition => (int)Math.Pow(map.GetPathToPigFromPosition(state, availablePosition).Count, 2))
                                    .ToList();

            float sum = (float)pigCosts.Sum();

            List<float> cooperationProbabilities = pigCosts
                                            .Select(cost => 1.0f - ((float)cost / (float)sum))
                                            .ToList();
            sum = cooperationProbabilities.Sum();
            cooperationProbabilities = cooperationProbabilities.Select(prob => (prob / sum))
                                    .ToList();

            cooperationProbabilities = this.InsertImpossibleMovesProbabilities(cooperationProbabilities, availablePositions, position);



            //Now we need to do the same for the exits, but now we take the minimum of the two lengths of the paths
            // Position of exits: (x == 1 && y == 4) || (x == 7 && y == 4)
            List<int> exitCosts = availablePositions
                                    .Select(availablePosition => Math.Min(
                                                (int)Math.Pow(map.GetPathToGoalPositionFromStartPosition(state, availablePosition, new Position(1, 4)).Count, 2),
                                                (int)Math.Pow(map.GetPathToGoalPositionFromStartPosition(state, availablePosition, new Position(7, 4)).Count, 2)
                                                ))
                                    .ToList();

            sum = (float)exitCosts.Sum();

            List<float> defectProbabilities = exitCosts
                                            .Select(cost => 1.0f - ((float)cost / (float)sum))
                                            .ToList();
            sum = defectProbabilities.Sum();
            defectProbabilities = defectProbabilities.Select(prob => (prob / sum))
                                .ToList();

            defectProbabilities = this.InsertImpossibleMovesProbabilities(defectProbabilities, availablePositions, position);

            return cooperationProbabilities.Concat(defectProbabilities).ToList();
        }

        private List<float> InsertImpossibleMovesProbabilities(List<float> probabilities, List<Position> availablePositions, Position agentPosition)
        {
            List<Position> cloneAvailablePositions = new List<Position>(availablePositions);
            List<Position> potentialPositions = new List<Position>
            {
                new Position( agentPosition.X,      agentPosition.Y - 1), //Up
                new Position( agentPosition.X + 1,  agentPosition.Y), //Right
                new Position( agentPosition.X,      agentPosition.Y + 1), //Down
                new Position( agentPosition.X - 1,  agentPosition.Y)  //Left
            };
            //Standing still is always added
            for (int i = 0; i < 4; i++)
            {
                if(cloneAvailablePositions[i] != potentialPositions[i])
                {
                    cloneAvailablePositions.Insert(i, potentialPositions[i]);
                    probabilities.Insert(i, 0.0f);
                }
            }

            return probabilities;
        }

        private void CalculateAndUpdateCooperationPrior(State state, List<float> PCMs, BasicAgent.AgentIdentifier id)
        {
            //WRONG return state.GetCooperationProbability(id);
            //Use conditional probability matrix from state to get the probability of cooperation from last turn. 
            // Or when it's the first turn, use the prior?

            Position pos = state.GetPosition(id);
            Position prevPos = state.GetPrevPosition(id);

            for(int i = 0; i < 5; i++)
            {
                if (PCMs[i] != 0.0f && PCMs[i + 5] != 0.0f && Math.Abs(1.0f - PCMs[i] - PCMs[i + 5]) > 0.01f)
                    throw new Exception("AHH INVALID PCMs");
            }

            //TODO: CHECK IF PCM[0] == 1 - PCM[5], P(C|m1) == 1 - P(-C|m1), this should be the case!
            //Switch based on previous movement, up = 0, right = 1, down = 2, left = 3, same = 4
            //For this, we do need a fixed size of the PMC list, so all moves need to be represented
            state.UpdateCooperationProbability(id, PCMs[this.GetMoveCode(pos, prevPos)]);
        }

        /// <summary>
        /// Return move code based on previous movement, up = 0, right = 1, down = 2, left = 3, same = 4
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="prevPos"></param>
        /// <returns></returns>
        private int GetMoveCode(Position pos, Position prevPos)
        {
            Position diff = pos - prevPos;

            //Vertical movement
            if (diff.X == 0)
            {
                //Up
                if(diff.Y < 0)
                {
                    return 0;
                }
                //Down
                else if(diff.Y > 0)
                {
                    return 2;
                }
                //Same position
                else
                {
                    return 4;
                }
            }
            //Horizontal movement
            else
            {
                //Left
                if(diff.X < 0)
                {
                    return 3;
                }
                //Right
                else
                {
                    return 1;
                }
            }
        }
    }
}
