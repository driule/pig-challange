using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using RoyT.AStar;

namespace pig_challenge
{
    class Pig : BasicAgent
    {
        public Pig() : base()
        {
        }

        public override void DetermineStep(Map map, State state)
        {
            Position currentPigPosition = state.GetAgentPosition(AgentIdentifier.Pig);

            // determine all available adjacent positions
            List<Position> availablePositions = map.GetAvailablePositions(currentPigPosition, state);

            foreach (Position position in availablePositions)
            {
                if (map.IsCellExit(position.X, position.Y) || position == currentPigPosition)
                {
                    availablePositions.Remove(position);
                    break;
                }
            }

            // get a random new position
            Position newPigPosition = (availablePositions.Count() > 0) ? availablePositions[this.GetRandomInt(0, availablePositions.Count() - 1)] : currentPigPosition;

            state.MoveAgent(AgentIdentifier.Pig, newPigPosition);
        }
    }
}