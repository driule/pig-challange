using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pig_challange
{
    abstract class BasicAgent
    {
        public Tuple<int, int> Position { get; set; }

        protected Random randomizer;

        public abstract void DetermineStep(Map map, Pig pig, Agent agentA, Agent agentB);

        protected bool IsCellFree(int x, int y, Map map, Pig pig, Agent agentA, Agent agentB)
        {
            Tuple<int, int> position = new Tuple<int, int>(x, y);

            if (map.Grid[x, y] != 1 && !pig.Position.Equals(position) && !agentA.Position.Equals(position) && !agentB.Position.Equals(position))
            {
                return true;
            }

            return false;
        }
    }
}
