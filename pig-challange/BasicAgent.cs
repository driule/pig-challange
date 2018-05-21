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

        protected bool IsCellFree(int x, int y, Map map, BasicAgent pigOrAgent, BasicAgent agent)
        {
            Tuple<int, int> position = new Tuple<int, int>(x, y);

            if (map.Grid[x, y] != 1 && !pigOrAgent.Position.Equals(position) && !agent.Position.Equals(position))
            {
                return true;
            }

            return false;
        }
    }
}
