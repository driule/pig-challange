using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pig_challange
{
    class Agent
    {
        public Tuple<int, int> Position { get; set; }
        public int Score { get; set; }

        private Random randomizer;

        public Agent()
        {
            this.randomizer = new Random();

            this.Position = new Tuple<int, int>(-1, -1);
            this.Score = 0;
        }

        public void DetermineStep(Map map)
        {
            // TODO: smart movements
        }
    }
}
