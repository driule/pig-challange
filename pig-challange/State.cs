using System;

namespace pig_challenge
{
    class State
    {
        public Game.ExitCodes ExitCode;
        public int[] PositionAgentA;
        public int[] PositionAgentB;
        public int[] PositionPig;
        public int ScoreAgentA;
        public int ScoreAgentB;
        public int TurnsLeft;
        public bool IsPigCapturable;
        private int maxTurns;

        public State(int maxTurns)
        {
            // initially set positions to 0,0
            // the positions will be changed after PlaceAgents was called
            // these positions are only used as a placeholder for the IsCellFree method
            this.PositionAgentA = new int[] { 0, 0 };
            this.PositionAgentB = new int[] { 0, 0 };
            this.PositionPig = new int[] { 0, 0 };

            this.ScoreAgentA = maxTurns;
            this.ScoreAgentB = maxTurns;
            this.TurnsLeft = maxTurns;
            this.maxTurns = maxTurns;

            this.ExitCode = Game.ExitCodes.InProgress;

            this.IsPigCapturable = true; //TODO
        }

        public void PlaceAgents(int[] PositionAgentA, int[] PositionAgentB, int[] positionPig)
        {
            this.PositionAgentA = PositionAgentA;
            this.PositionAgentB = PositionAgentB;
            this.PositionPig = positionPig;
        }

        public void MoveAgent(BasicAgent.AgentIdentifier agentId, int[] location)
        {
            switch (agentId)
            {
                case BasicAgent.AgentIdentifier.AgentA: { this.MoveAgentA(location); break; }
                case BasicAgent.AgentIdentifier.AgentB: { this.MoveAgentB(location); break; }
                default: { this.MovePig(location); break; }
            }
        }

        private void MoveAgentA(int[] location)
        {
            this.PositionAgentA = location;
            this.ScoreAgentA--;
        }

        private void MoveAgentB(int[] location)
        {
            this.PositionAgentB = location;
            this.ScoreAgentB--;
        }

        private void MovePig(int[] location)
        {
            this.PositionPig = location;
            this.TurnsLeft--;
        }

        public void SetWinnerA()
        {
            this.ExitCode = Game.ExitCodes.AgentAQuit;
            this.ScoreAgentA += 5;
        }

        public void SetWinnerB()
        {
            this.ExitCode = Game.ExitCodes.AgentBQuit;
            this.ScoreAgentB += 5;
        }

        public void SetWinnerBoth()
        {
            this.ExitCode = Game.ExitCodes.PigCaught;
            this.ScoreAgentA += 25;
            this.ScoreAgentB += 25;
        }

        public void SetOutOfTurns()
        {
            this.ExitCode = Game.ExitCodes.IterationsExceeded;
        }

        public int[] GetPosition(BasicAgent.AgentIdentifier identifier)
        {
            switch (identifier) {
                case BasicAgent.AgentIdentifier.AgentA: return this.PositionAgentA;
                case BasicAgent.AgentIdentifier.AgentB: return this.PositionAgentB;
                default: return this.PositionPig;
            }
        }

        public void Print()
        {
            Console.WriteLine("ExitCode: " + ExitCode);
            Console.WriteLine("PositionAgentA: " + PositionAgentA[0] + ","+ PositionAgentA[1]);
            Console.WriteLine("PositionAgentB: " + PositionAgentB[0] + "," + PositionAgentB[1]);
            Console.WriteLine("PositionPig: " + PositionPig[0] + "," + PositionPig[1]);
            Console.WriteLine("ScoreAgentA: " + ScoreAgentA);
            Console.WriteLine("ScoreAgentB: " + ScoreAgentB);
            Console.WriteLine("TurnsLeft: " + TurnsLeft);
            Console.WriteLine("IsPigCapturable: " + IsPigCapturable);
            Console.WriteLine("maxTurns: " + maxTurns);
        }
    }
}
