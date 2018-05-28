using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pig_challange
{
    class State
    {
        public int ExitCode;
        public int[] PositionA;
        public int[] PositionB;
        public int[] PositionPig;
        public int scoreA;
        public int scoreB;
        public int Turns;
        public bool IsPigCapturable;
        private int maxTurns;

        public State(int turns)
        {
            //Initially set positions to 0,0
            //The positions will be changed after PlaceAgents was called
            //These positions are only used as a placeholder for the IsCellFree method
            this.PositionA = new int[] { 0, 0 };
            this.PositionB = new int[] { 0, 0 };
            this.PositionPig = new int[] { 0, 0 };

            this.scoreA = turns;
            this.scoreB = turns;
            this.Turns = turns;
            this.maxTurns = turns;

            this.ExitCode = -1; //-1 = none, 0 = A won, 1 = B won, 2 = Both won, 3 both lost

            this.IsPigCapturable = true; //TODO
        }

        public void PlaceAgents(int[] positionA, int[] positionB, int[] positionPig)
        {
            this.PositionA = positionA;
            this.PositionB = positionB;
            this.PositionPig = positionPig;
        }

        //Performs movement for agent.
        public void move(int agent, int[] location)
        {
            switch (agent)
            {
                case 0: { this.moveA(location); break; }
                case 1: { this.moveB(location); break; }
                default: { this.movePig(location); break; }
            }
        }

        private void moveA(int[] location)
        {
            this.PositionA = location;
            this.scoreA--;
        }

        private void moveB(int[] location)
        {
            this.PositionB = location;
            this.scoreB--;
        }

        private void movePig(int[] location)
        {
            this.PositionPig = location;
            this.Turns--;
        }

        public void SetWinnerA()
        {
            this.ExitCode = 0;
            this.scoreA += 5;
        }

        public void SetWinnerB()
        {
            this.ExitCode = 1;
            this.scoreB += 5;
        }

        public void SetWinnerBoth()
        {
            this.ExitCode = 2;
            this.scoreA += 25;
            this.scoreB += 25;
        }

        public void SetOutOfTurns()
        {
            this.ExitCode = 3;
        }

        public int[] GetPosition(int identifier)
        {
            switch (identifier) {
                case 0: return this.PositionA;
                case 1: return this.PositionB;
                default: return this.PositionPig;
            }
        }

        //TEST
        public void Print()
        {
            Console.WriteLine("ExitCode: " + ExitCode);
            Console.WriteLine("PositionA: " + PositionA[0] + ","+ PositionA[1]);
            Console.WriteLine("PositionB: " + PositionB[0] + "," + PositionB[1]);
            Console.WriteLine("PositionPig: " + PositionPig[0] + "," + PositionPig[1]);
            Console.WriteLine("scoreA: " + scoreA);
            Console.WriteLine("scoreB: " + scoreB);
            Console.WriteLine("Turns: " + Turns);
            Console.WriteLine("IsPigCapturable: " + IsPigCapturable);
            Console.WriteLine("maxTurns: " + maxTurns);
        }
    }
}
