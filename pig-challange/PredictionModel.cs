using RoyT.AStar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pig_challenge
{
    class PredictionModel
    {
        public PredictionModel()
        {
        }

        public List<float> GetCooperationConditionalProbabilities(Map map, State state, BasicAgent.AgentIdentifier agentId)
        {
            // first call GetPMCConditionalPropabilities and GetCooperationPrior, then calculate for all m and all c: P(m|c) * P(c), put them in a temporary list. 
            // then calculate over all m and all c: P(c|m) = ( P(m|c) * P(c)) / SUM_C( P(m|i)*P(i) ). So only bottom half really needs to be calculated and 
            // we use the precalculated ones from the list created.

            // get the PMCs, which are in P(m1|0),.. P(m5|0), P(m1|1) order 
            List<float> movesCondintionalProbabilities = this.GetMovesConditionalProbabilities(map, state, agentId);
            float cooperationProbability = state.GetCooperationProbabilityGuess(agentId);

            // precalculated bottom parts
            List<float> precalculatedMoveConditionalProbabilities = movesCondintionalProbabilities.Select((moveConditionalProbability, index) =>
            {
                // this switch accounts for the fact that the first 5 elements are PMC and 
                // last 5 are PMnotC, with notC being 1 - C
                if (index <= 4)
                {
                    return moveConditionalProbability * cooperationProbability;
                }
                else
                {
                    //index > 5, so we need to use P(-c) 
                    return moveConditionalProbability * (1.0f - cooperationProbability);
                }
            }).ToList();

            List<float> cooperationConditionalProbabilities = movesCondintionalProbabilities.Select((moveConditionalProbability, index) =>
            {
                // we won't use this prob. because it's unavailable or no path to goal, so really bad and thus 0.0f is perfect.
                if (moveConditionalProbability == 0.0f)
                {
                    return 0.0f;
                }

                //get both indices of precalculated value, for one movement and two C's:
                // C and not C. We need those two, because we sum over both possible C's. They are located at 0&4, 1&5,...
                if (index <= 4)
                {
                    return precalculatedMoveConditionalProbabilities[index]
                        / (precalculatedMoveConditionalProbabilities[index] + precalculatedMoveConditionalProbabilities[index + 5]);
                }
                else
                {
                    //index > 5, so we need to use P(-c) 
                    return precalculatedMoveConditionalProbabilities[index]
                        / (precalculatedMoveConditionalProbabilities[index] + precalculatedMoveConditionalProbabilities[index - 5]);
                }
            }).ToList();

            // calculate P(m) by going over all SUM_C( P(m|i) * P(i) ) //TODO: smartly

            return cooperationConditionalProbabilities;
        }

        /// <summary>
        /// Returns Conditional probabilities in P(m1|0),.. P(m5|0), P(m1|1) order 
        /// </summary>
        /// <param name="map"></param>
        /// <param name="state"></param>
        /// <param name="agentId"></param>
        /// <returns></returns>
        private List<float> GetMovesConditionalProbabilities(Map map, State state, BasicAgent.AgentIdentifier agentId)
        {
            Position agentPosition = state.GetAgentPosition(agentId);

            List<Position> availablePositions = map.GetAvailablePositions(agentPosition, state);

            // this calculates the amount of steps until the position of the pig is reached, so we could subtract 1 to get to the position just before the pig
            // basically, I actually wanted to calculate from the availablePosition to next to the pig, and then add 1 tot the total,
            // but the GetPath method actually includes the startPosition in the path, so the +1 is not needed.
            List<int> pathToPigCosts = availablePositions.Select(
                availablePosition => map.GetActualPathCost(
                    map.GetPathToPigFromPosition(state, availablePosition).Count
                )
            ).ToList();

            List<float> cooperationProbabilities = this.CalculateAgentMovesProbabilitiesForGivenPath(pathToPigCosts, agentPosition, availablePositions);

            // now we need to do the same for the exits, but now we take the minimum of the two lengths of the paths
            // position of exits: (x == 1 && y == 4) || (x == 7 && y == 4)
            List<int> pathToExitCosts = availablePositions.Select(
                availablePosition => Math.Min(
                    map.GetActualPathCost(
                        map.GetPathToGoalPositionFromStartPosition(
                            state,
                            availablePosition,
                            new Position(1, 4)
                        ).Count
                    ),
                    map.GetActualPathCost(
                        map.GetPathToGoalPositionFromStartPosition(
                            state,
                            availablePosition,
                            new Position(7, 4)
                        ).Count
                    )
                )
            ).ToList();

            List<float> defectProbabilities = this.CalculateAgentMovesProbabilitiesForGivenPath(pathToExitCosts, agentPosition, availablePositions);

            return cooperationProbabilities.Concat(defectProbabilities).ToList();
        }

        private List<float> CalculateAgentMovesProbabilitiesForGivenPath(List<int> pathCosts, Position agentPosition, List<Position> availablePositions)
        {
            // sum all costs
            float totalCost = (float)pathCosts.Sum();

            // divide every cost by the totalCost and invert
            List<float> probabilities = pathCosts.Select(
                cost => 1.0f - ((float)cost / (float)totalCost)
            ).ToList();

            // normalize again
            float probabilitiesSum = probabilities.Sum();
            probabilities = probabilities.Select(probability => (probability / probabilitiesSum)).ToList();

            // insert a probability of 0 for the movements that were not possible, to always have the same size for the list. 
            // handy to know what move the probability in the resulting List represents, otherwise more difficult.
            probabilities = this.InsertImpossibleMovesProbabilities(probabilities, availablePositions, agentPosition);

            return probabilities;
        }

        /// <summary>
        /// Insert a probability of 0 for the movements that were not possible, to always have the same size for the list. 
        ///   Handy to know what move the probability in the resulting List represents, otherwise more difficult.
        /// </summary>
        /// <param name="probabilities"></param>
        /// <param name="availablePositions"></param>
        /// <param name="agentPosition"></param>
        /// <returns></returns>
        private List<float> InsertImpossibleMovesProbabilities(List<float> probabilities, List<Position> availablePositions, Position agentPosition)
        {
            List<Position> cloneAvailablePositions = new List<Position>(availablePositions);
            List<Position> potentialPositions = new List<Position>
            {
                new Position(agentPosition.X, agentPosition.Y - 1), // up
                new Position(agentPosition.X + 1, agentPosition.Y), // right
                new Position(agentPosition.X, agentPosition.Y + 1), // down
                new Position(agentPosition.X - 1, agentPosition.Y)  // left
            };

            // standing still is always added
            for (int i = 0; i < 4; i++)
            {
                if (cloneAvailablePositions[i] != potentialPositions[i])
                {
                    cloneAvailablePositions.Insert(i, potentialPositions[i]);
                    probabilities.Insert(i, 0.0f);
                }
            }

            return probabilities;
        }

        public void CalculateAndUpdateCooperationPrior(State state, List<float> cooperationConditionalProbabilities, BasicAgent.AgentIdentifier agentId)
        {
            Position agentPosition = state.GetAgentPosition(agentId);
            Position previousAgentPosition = state.GetPrevAgentPosition(agentId);

            for (int i = 0; i < 5; i++)
            {
                if (
                    cooperationConditionalProbabilities[i] != 0.0f
                    && cooperationConditionalProbabilities[i + 5] != 0.0f
                    && Math.Abs(1.0f - cooperationConditionalProbabilities[i] - cooperationConditionalProbabilities[i + 5]) > 0.01f
                )
                {
                    throw new Exception("Invalid PCMs probabilities");
                }
            }

            // TODO: CHECK IF PCM[0] == 1 - PCM[5], P(C|m1) == 1 - P(-C|m1), this should be the case!
            // switch based on previous movement, up = 0, right = 1, down = 2, left = 3, same = 4
            // for this, we do need a fixed size of the PMC list, so all moves need to be represented
            state.UpdateCooperationProbability(agentId, cooperationConditionalProbabilities[this.GetMoveId(agentPosition, previousAgentPosition)]);
        }

        /// <summary>
        /// Return move code based on previous movement, up = 0, right = 1, down = 2, left = 3, same = 4
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="prevPos"></param>
        /// <returns></returns>
        private int GetMoveId(Position agentPosition, Position previousAgentPosition)
        {
            Position diff = agentPosition - previousAgentPosition;

            // vertical movement
            if (diff.X == 0)
            {
                // up
                if (diff.Y < 0)
                {
                    return 0;
                }
                // down
                else if (diff.Y > 0)
                {
                    return 2;
                }
                // same position
                else
                {
                    return 4;
                }
            }
            // horizontal movement
            else
            {
                // left
                if (diff.X < 0)
                {
                    return 3;
                }
                // right
                else
                {
                    return 1;
                }
            }
        }
    }
}
