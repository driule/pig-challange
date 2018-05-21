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
            this.Position = new Tuple<int, int>(-1, -1);
        }

        public void DetermineStep(Map map, Agent agentA, Agent agentB, int tries = 0)
        {
            if (tries > 5)
                return;

            int moveDirection = this.randomizer.Next(0, 3);

            if (moveDirection == 0)
            {
                if (this.IsCellFree(this.Position.Item1 + 1, this.Position.Item2, map, agentA, agentB))
                {
                    this.Position = new Tuple<int, int>(this.Position.Item1 + 1, this.Position.Item2);
                }
                else
                {
                    DetermineStep(map, agentA, agentB, tries + 1);
                }
            }
            else if (moveDirection == 1)
            {
                if (this.IsCellFree(this.Position.Item1, this.Position.Item2 + 1, map, agentA, agentB))
                {
                    this.Position = new Tuple<int, int>(this.Position.Item1, this.Position.Item2 + 1);
                }
                else
                {
                    DetermineStep(map, agentA, agentB, tries + 1);
                }
            }
            else if (moveDirection == 2)
            {
                if (this.IsCellFree(this.Position.Item1 - 1, this.Position.Item2, map, agentA, agentB))
                {
                    this.Position = new Tuple<int, int>(this.Position.Item1 - 1, this.Position.Item2);
                }
                else
                {
                    DetermineStep(map, agentA, agentB, tries + 1);
                }
            }
            else if (moveDirection == 3)
            {
                if (this.IsCellFree(this.Position.Item1, this.Position.Item2 - 1, map, agentA, agentB))
                {
                    this.Position = new Tuple<int, int>(this.Position.Item1, this.Position.Item2 - 1);
                }
                else
                {
                    DetermineStep(map, agentA, agentB, tries + 1);
                }
            }
        }

        
    }
}
