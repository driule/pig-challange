using System;
using System.Collections.Generic;
using System.Linq;

namespace pig_challenge
{
    class Pig : BasicAgent
    {
        public Pig()
        {
            this.randomizer = new Random();
        }

        public override void DetermineStep(Map map, State state)
        {
            int[] position = state.GetPosition(AgentIdentifier.Pig);

            // determine all available adjacent positions
            List<int[]> positions = map.GetAvailablePositions(position, state);

            foreach(var pos in positions)
            {
                if (map.IsCellExit(pos[0], pos[1]))
                {
                    positions.Remove(pos);
                    break;
                }
            }

            // get a random new position
            int[] newPosition = (positions.Count() > 0) ? positions[this.randomizer.Next(0, positions.Count() - 1)] : position;

            // analyse the gamestate and board to determine if the pig is capturable form its new position
            this.DetermineIsPigCapturable(map, state, newPosition);

            state.MoveAgent(AgentIdentifier.Pig, newPosition);
        }

        // the pig is capturable if two or fewer nearby squares are empty
        private void DetermineIsPigCapturable(Map map, State state, int[] position)
        {
            state.IsPigCapturable = (map.GetAvailablePositions(position, state)).Count() <= 2;
        }
    }
}
