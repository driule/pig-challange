using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pig_challange
{
    class Agent
    {
        public Tuple<int, int> Position;
        public int Score;

        public Agent(Tuple<int, int> startPosition)
        {
            this.Position = startPosition;
            this.Score = 0;
        }

        public Tuple<int,int> DetermineStep()
        {
            return new Tuple<int, int>(1, 1);
        }
    }
}
