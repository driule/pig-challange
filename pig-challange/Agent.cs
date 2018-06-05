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

            //List<float> conditionalProbabilities = this.GetPMCConditionalPropabilities(map, state);

            IList<Position> path = map.GetPathToPig(this.Identifier, state);

            if (path.Count <= 2)
            {
                return;
            }

            Position newPos = new Position (path[1].X, path[1].Y );

            state.MoveAgent(this.Identifier, newPos);
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
