using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pig_challange
{
    class Pig
    {
        public Tuple<int, int> Position;

        public Pig(Tuple<int, int> startPosition)
        {
            this.Position = startPosition;
        }

        public Tuple<int,int> DetermineStep()
        {
            return new Tuple<int, int>(1, 1);
        }

    }
}
