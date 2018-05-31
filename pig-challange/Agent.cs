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

            List<Position> availablePositions = map.GetAvailablePositions(position, state);
            List<int> costs = new List<int>();

            foreach(Position availablePosition in availablePositions)
            {
                //This calculates the amount of steps until the position of the pig is reached, so we could subtract 1 to get to the position just before the pig
                //Basically, I actually wanted to calculate from the availablePosition to next to the pig, and then add 1 tot the total,
                //  but the GetPath method actually includes the startPosition in the path, so the +1 is not needed.
                costs.Add(map.GetPathToPigFromPosition(this.Identifier, state, availablePosition).Count());
            }

            IList<Position> path = map.GetPathToPig(this.Identifier, state);

            if (path.Count <= 2)
            {
                return;
            }

            Position newPos = new Position (path[1].X, path[1].Y );

            state.MoveAgent(this.Identifier, newPos);
        }
    }
}
