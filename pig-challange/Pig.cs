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

        public override void DetermineStep(Map map, int tries = 0)
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
                    this.DetermineStep(map, tries + 1);
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
                    this.DetermineStep(map, tries + 1);
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
                    this.DetermineStep(map, tries + 1);
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
                    this.DetermineStep(map, tries + 1);
                }
            }
        }
    }
}
