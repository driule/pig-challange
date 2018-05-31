﻿using System;
using RoyT.AStar;

namespace pig_challenge
{
    class State
    {
        public Game.ExitCodes ExitCode;
        public Position PositionAgentA;
        public Position PositionAgentB;
        public Position PositionPig;
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
            this.PositionAgentA = new Position(0,0);
            this.PositionAgentB = new Position(0,0);
            this.PositionPig = new Position(0,0);

            this.ScoreAgentA = maxTurns;
            this.ScoreAgentB = maxTurns;
            this.TurnsLeft = maxTurns;
            this.maxTurns = maxTurns;

            this.ExitCode = Game.ExitCodes.InProgress;

            this.IsPigCapturable = true; //TODO
        }

        public void PlaceAgents(Map map)
        {
            this.PositionAgentA = map.GetRandomStartPosition(this);
            this.PositionAgentB = map.GetRandomStartPosition(this);
            this.PositionPig = map.GetRandomStartPosition(this);
        }

        public void MoveAgent(BasicAgent.AgentIdentifier agentId, Position location)
        {
            switch (agentId)
            {
                case BasicAgent.AgentIdentifier.AgentA: { this.MoveAgentA(location); break; }
                case BasicAgent.AgentIdentifier.AgentB: { this.MoveAgentB(location); break; }
                default: { this.MovePig(location); break; }
            }
        }

        private void MoveAgentA(Position location)
        {
            this.PositionAgentA = location;
            this.ScoreAgentA--;
        }

        private void MoveAgentB(Position location)
        {
            this.PositionAgentB = location;
            this.ScoreAgentB--;
        }

        private void MovePig(Position location)
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

        public Position GetPosition(BasicAgent.AgentIdentifier identifier)
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
            Console.WriteLine("PositionAgentA: " + PositionAgentA.X + ","+ PositionAgentA.Y);
            Console.WriteLine("PositionAgentB: " + PositionAgentB.X + "," + PositionAgentB.Y);
            Console.WriteLine("PositionPig: " + PositionPig.X + "," + PositionPig.Y);
            Console.WriteLine("ScoreAgentA: " + ScoreAgentA);
            Console.WriteLine("ScoreAgentB: " + ScoreAgentB);
            Console.WriteLine("TurnsLeft: " + TurnsLeft);
            Console.WriteLine("IsPigCapturable: " + IsPigCapturable);
            Console.WriteLine("maxTurns: " + maxTurns);
        }
    }
}
