using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pig_challange
{
    class Pig : BasicAgent
    {
        public Pig()
        {
            this.randomizer = new Random();
        }

        public override void DetermineStep(Map map, State state)
        {
            int[] position = state.GetPosition(2);

            //Determine all available adjacent positions
            List<int[]> positions = map.AvailablePositions(position, state);

            //Get a random new position
            int[] newPosition = (positions.Count() > 0) ? positions[this.randomizer.Next(0, positions.Count() - 1)] : position;

            //Analyse the gamestate and board to determine if the pig is capturable form its new position
            this.determineCapturable(map, state, newPosition);

            state.move(2, newPosition);
        }

        //The pig is capturable if two or fewer nearby squares are empty
        private void determineCapturable(Map map, State state, int[] position)
        {
            state.IsPigCapturable = (map.AvailablePositions(position, state)).Count() <= 2;
        }

        //TODO rewrite to deterministic function
        //determine the possible destination squares (max 4) and then pick a random number determining which will be chosen
        //don't use recursion
        /*
        public override void DetermineStep(Map map, GameState state, int tries = 0)
        {
            if (tries > 5)
            {
                return;
            }

            int moveDirection = this.randomizer.Next(0, 3);

            if (moveDirection == 0)
            {
                if (map.IsCellEmpty(this.Position.Item1 + 1, this.Position.Item2) && !map.IsCellExit(this.Position.Item1 + 1, this.Position.Item2))
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
                if (map.IsCellEmpty(this.Position.Item1, this.Position.Item2 + 1) && !map.IsCellExit(this.Position.Item1, this.Position.Item2 + 1))
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
                if (map.IsCellEmpty(this.Position.Item1 - 1, this.Position.Item2) && !map.IsCellExit(this.Position.Item1 - 1, this.Position.Item2))
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
                if (map.IsCellEmpty(this.Position.Item1, this.Position.Item2 - 1) && !map.IsCellExit(this.Position.Item1, this.Position.Item2 - 1))
                {
                    this.Position = new Tuple<int, int>(this.Position.Item1, this.Position.Item2 - 1);
                }
                else
                {
                    this.DetermineStep(map, state, tries + 1);
                }
            }
        }*/
    }
}
