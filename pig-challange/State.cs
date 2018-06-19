using System;
using System.Collections.Generic;
using RoyT.AStar;

namespace pig_challenge
{
    class State
    {
        public Game.ExitCodes ExitCode;
        public Position PrevPositionAgentA;
        public Position PositionAgentA;
        public Position PrevPositionAgentB;
        public Position PositionAgentB;
        public Position PositionPig;
        public int ScoreAgentA;
        public int ScoreAgentB;
        public int TurnsLeft;
        public bool IsPigCapturable;
        public float CooperationProbabilityGuessA;
        public float CooperationProbabilityGuessB;
        private int maxTurns;

        public State(int maxTurn, AgentConfiguration configurationA, AgentConfiguration configurationB)
        {
            this.PrevPositionAgentA = new Position(0, 0);
            this.PrevPositionAgentB = new Position(0, 0);

            // initially set positions to 0,0
            // the positions will be changed after PlaceAgents was called
            // these positions are only used as a placeholder for the IsCellFree method
            this.PositionAgentA = new Position(0,0);
            this.PositionAgentB = new Position(0,0);
            this.PositionPig = new Position(0,0);

            this.ScoreAgentA = maxTurn;
            this.ScoreAgentB = maxTurn;
            this.TurnsLeft = maxTurn;
            this.maxTurns = maxTurn;

            this.ExitCode = Game.ExitCodes.InProgress;

            this.IsPigCapturable = true; //TODO

            if (configurationA.useCooperationHistory)
            {
                this.CooperationProbabilityGuessA = configurationA.initialCooperationGuess;
            } else
            {
                this.CooperationProbabilityGuessA = AgentConfiguration.DEFAULT_INITIAL_GUESS;
            }

            if (configurationB.useCooperationHistory)
            {
                this.CooperationProbabilityGuessB = configurationB.initialCooperationGuess;
            }
            else
            {
                this.CooperationProbabilityGuessB = AgentConfiguration.DEFAULT_INITIAL_GUESS;
            }
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
            this.PrevPositionAgentA = this.PositionAgentA;
            this.PositionAgentA = location;
            this.ScoreAgentA--;
        }

        private void MoveAgentB(Position location)
        {
            this.PrevPositionAgentB = this.PositionAgentB;
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

        public void UpdateCooperationProbability(BasicAgent.AgentIdentifier identifier, float cooperationProbability)
        {
            switch (identifier)
            {
                case BasicAgent.AgentIdentifier.AgentA:
                    this.CooperationProbabilityGuessA = cooperationProbability;
                    break;
                case BasicAgent.AgentIdentifier.AgentB:
                    this.CooperationProbabilityGuessB = cooperationProbability;
                    break;
                default: throw new Exception("identifier has no cooperation probability");
            }
        }

        public Position GetAgentPosition(BasicAgent.AgentIdentifier identifier)
        {
            switch (identifier) {
                case BasicAgent.AgentIdentifier.AgentA: return this.PositionAgentA;
                case BasicAgent.AgentIdentifier.AgentB: return this.PositionAgentB;
                default: return this.PositionPig;
            }
        }

        public Position GetPrevAgentPosition(BasicAgent.AgentIdentifier identifier)
        {
            switch (identifier)
            {
                case BasicAgent.AgentIdentifier.AgentA: return this.PrevPositionAgentA;
                case BasicAgent.AgentIdentifier.AgentB: return this.PrevPositionAgentB;
                default: throw new Exception("identifier has no prevposition");
            }
        }

        public float GetCooperationProbabilityGuess(BasicAgent.AgentIdentifier identifier)
        {
            switch (identifier)
            {
                case BasicAgent.AgentIdentifier.AgentA: return this.CooperationProbabilityGuessA;
                case BasicAgent.AgentIdentifier.AgentB: return this.CooperationProbabilityGuessB;
                default: throw new Exception("identifier has no cooperation probability");
            }
        }

        public int GetAgentScore(BasicAgent.AgentIdentifier identifier)
        {
            switch (identifier)
            {
                case BasicAgent.AgentIdentifier.AgentA: return this.ScoreAgentA;
                case BasicAgent.AgentIdentifier.AgentB: return this.ScoreAgentB;
                default: throw new Exception("identifier has no score");
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
