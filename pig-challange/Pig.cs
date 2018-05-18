using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pig_challange
{
    class Pig
    {
        public Tuple<int, int> Position { get; set; }

        private Random randomizer;

        public Pig()
        {
            this.randomizer = new Random();
            this.Position = new Tuple<int, int>(-1, -1);
        }

        public void DetermineStep(Map map, Agent agentA, Agent agentB)
        {
            int moveDirection = this.randomizer.Next(0, 3);

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

        private bool IsCellFree(int x, int y, Map map, Agent agentA, Agent agentB)
        {
            Tuple<int, int> position = new Tuple<int, int>(x, y);

            if (map.Grid[x, y] != 1 && !agentA.Position.Equals(position) && !agentB.Position.Equals(position))
            {
                return true;
            }

            return false;
        }
    }
}
