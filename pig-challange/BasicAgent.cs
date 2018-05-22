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

        abstract public void DetermineStep(Map map, int tries = 0);
    }
}
