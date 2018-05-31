using System;
using System.Collections.Generic;
using System.Linq;
using RoyT.AStar;

namespace pig_challenge
{
    class Agent : BasicAgent
    {
        private AgentIdentifier Identifier { get; }

        public Agent(AgentIdentifier identifier)
        {
            this.Identifier = identifier;
            this.randomizer = new Random();
        }

        public override void DetermineStep(Map map, State state)
        {
            Position position = state.GetPosition(this.Identifier);

            List<float> conditionalProbabilities = this.GetConditionalPropabilities(map, state);

            IList<Position> path = map.GetPathToPig(this.Identifier, state);

            if (path.Count <= 2)
            {
                return;
            }

            Position newPos = new Position (path[1].X, path[1].Y );

            state.MoveAgent(this.Identifier, newPos);
        }

        /// <summary>
        /// Returns Conditional probabilities in P(0|m1),.. P(0|m4), P(1|m1) order 
        /// </summary>
        /// <param name="map"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public List<float> GetConditionalPropabilities(Map map, State state)
        {
            Position position = state.GetPosition(this.GetOtherAgentIdentifier(this.Identifier));

            List<Position> availablePositions = map.GetAvailablePositions(position, state);

            //This calculates the amount of steps until the position of the pig is reached, so we could subtract 1 to get to the position just before the pig
            //Basically, I actually wanted to calculate from the availablePosition to next to the pig, and then add 1 tot the total,
            //  but the GetPath method actually includes the startPosition in the path, so the +1 is not needed.
            List<int> costs = availablePositions
                                    .Select(availablePosition => (int)Math.Pow(map.GetPathToPigFromPosition(this.Identifier, state, availablePosition).Count(), 2))
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

        public AgentIdentifier GetOtherAgentIdentifier(AgentIdentifier identifier)
        {
            if (identifier == AgentIdentifier.AgentA)
                return AgentIdentifier.AgentB;
            else
                return AgentIdentifier.AgentA;
        }
    }
}
