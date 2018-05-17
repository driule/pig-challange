using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pig_challange
{
    class Pig
    {
        private Tuple<int, int> Position { get; set; }

        public Tuple<int,int> DetermineStep()
        {
            return new Tuple<int, int>(1, 1);
        }

    }
}
