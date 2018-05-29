using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pig_challange
{
    abstract class BasicAgent
    {
        public enum AgentIdentifier
        {
            AgentA = 0,
            AgentB = 1,
            Pig = 2
        }

        protected Random randomizer;

        abstract public void DetermineStep(Map map, State state);
    }
}
