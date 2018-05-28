using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pig_challange
{
    class Agent : BasicAgent
    {
        private int Identifier { get;} //0 for agent A, 1 for agent B


        public Agent(int identifier)
        {
            this.Identifier = identifier;
            this.randomizer = new Random();
        }

        public override void DetermineStep(Map map, State state)
        {
            int[] position = state.GetPosition(this.Identifier);

            //Determine all available adjacent positions
            List<int[]> positions = map.AvailablePositions(position, state);

            //Get a random new position
            int[] newPosition = (positions.Count() > 0) ? positions[this.randomizer.Next(0, positions.Count() - 1)] : position;

            state.move(this.Identifier, newPosition);
        }

        /*
        public override void DetermineStep(Map map, GameState state, int tries = 0)
        {
            // TODO: smart movements

            if (tries > 5)
            {
                return;
            }

            int moveDirection = this.randomizer.Next(0, 3);

            if (moveDirection == 0)
            {
                if (map.IsCellEmpty(this.Position.Item1 + 1, this.Position.Item2))
                {
                    this.Position = new Tuple<int, int>(this.Position.Item1 + 1, this.Position.Item2);
                }
                else
                {
                    this.DetermineStep(map, state, tries + 1);
                }
            }
            else if (moveDirection == 1)
            {
                if (map.IsCellEmpty(this.Position.Item1, this.Position.Item2 + 1))
                {
                    this.Position = new Tuple<int, int>(this.Position.Item1, this.Position.Item2 + 1);
                }
                else
                {
                    this.DetermineStep(map, state, tries + 1);
                }
            }
            else if (moveDirection == 2)
            {
                if (map.IsCellEmpty(this.Position.Item1 - 1, this.Position.Item2))
                {
                    this.Position = new Tuple<int, int>(this.Position.Item1 - 1, this.Position.Item2);
                }
                else
                {
                    this.DetermineStep(map, state, tries + 1);
                }
            }
            else if (moveDirection == 3)
            {
                if (map.IsCellEmpty(this.Position.Item1, this.Position.Item2 - 1))
                {
                    this.Position = new Tuple<int, int>(this.Position.Item1, this.Position.Item2 - 1);
                }
                else
                {
                    this.DetermineStep(map, state, tries + 1);
                }
            }
        } */
    }
}
