using System;
using System.Collections.Generic;
using System.Linq;

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
            int[] position = state.GetPosition(this.Identifier);

            // determine all available adjacent positions
            List<int[]> positions = map.GetAvailablePositions(position, state);

            // get a random new position
            int[] newPosition = (positions.Count() > 0) ? positions[this.randomizer.Next(0, positions.Count() - 1)] : position;

            state.MoveAgent(this.Identifier, newPosition);
        }
    }
}
