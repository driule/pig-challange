using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pig_challange
{
    class Pig
    {
        private Random randomizer = new Random();

        public Tuple<int, int> Position;

        public Pig(Tuple<int, int> startPosition)
        {
            this.Position = startPosition;
        }

        public void DetermineStep(int[,] map, Agent agentA, Agent agentB)
        {
            int moveDirection = this.randomizer.Next(0, 4);

            if (moveDirection == 0)
            {
                if (this.IsCellFree(this.Position.Item1 + 1, this.Position.Item2, map, agentA, agentB))
                {
                    this.Position = new Tuple<int, int>(this.Position.Item1 + 1, this.Position.Item2);
                }
            }
            else if (moveDirection == 1)
            {
                if (this.IsCellFree(this.Position.Item1, this.Position.Item2 + 1, map, agentA, agentB))
                {
                    this.Position = new Tuple<int, int>(this.Position.Item1, this.Position.Item2 + 1);
                }
            }
            if (moveDirection == 2)
            {
                if (this.IsCellFree(this.Position.Item1 - 1, this.Position.Item2, map, agentA, agentB))
                {
                    this.Position = new Tuple<int, int>(this.Position.Item1 - 1, this.Position.Item2);
                }
            }
            else if (moveDirection == 3)
            {
                if (this.IsCellFree(this.Position.Item1, this.Position.Item2 - 1, map, agentA, agentB))
                {
                    this.Position = new Tuple<int, int>(this.Position.Item1, this.Position.Item2 - 1);
                }
            }
        }

        private bool IsCellFree(int x, int y, int[,] map, Agent agentA, Agent agentB)
        {
            Tuple<int, int> position = new Tuple<int, int>(x, y);

            if (map[x, y] != 1 && !agentA.Position.Equals(position) && !agentB.Position.Equals(position))
            {
                return true;
            }

            return false;
        }
    }
}
