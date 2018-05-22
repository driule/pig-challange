using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pig_challange
{
    class Agent : BasicAgent
    {
        public int Score { get; set; }

        public Agent()
        {
            this.randomizer = new Random();

            this.Position = new Tuple<int, int>(-1, -1);
            this.Score = 0;
        }

        public override void DetermineStep(Map map, int tries = 0)
        {
            // TODO: smart movements
        }
    }
}
