using System;
using System.Collections.Generic;
using System.Linq;

namespace pig_challenge
{
    class Game
    {
        private int MaxIterations;
        private State state;
        private PredictionModel PredictionModel;
        
        public enum ExitCodes
        {
            InProgress = -1,
            AgentAQuit = 0,
            AgentBQuit = 1,
            PigCaught = 2,
            IterationsExceeded = 3
        }

        private Agent agentA, agentB;
        private Pig pig;
        private Map map;
        
        public Game(int maxIterations, AgentConfiguration configurationA, AgentConfiguration configurationB )
        {
            this.MaxIterations = maxIterations;

            this.agentA = new Agent(BasicAgent.AgentIdentifier.AgentA, configurationA); 
            this.agentB = new Agent(BasicAgent.AgentIdentifier.AgentB, configurationB);
            this.pig = new Pig();
            this.map = new Map();

            this.state = new State(this.MaxIterations, configurationA, configurationB);
            this.state.PlaceAgents(this.map);
            this.PredictionModel = new PredictionModel();
        }

        public State Run()
        {
            List<float> cooperationConditionalProbabilities;
            this.map.Draw(0, this.state);
            for (int i = 0; i < this.MaxIterations; i++)
            {
                cooperationConditionalProbabilities = this.PredictionModel.GetCooperationConditionalProbabilities(map, state, BasicAgent.AgentIdentifier.AgentA);
                this.agentA.DetermineStep(this.map, this.state);
                this.PredictionModel.CalculateAndUpdateCooperationPrior(state, cooperationConditionalProbabilities, BasicAgent.AgentIdentifier.AgentA);
                this.EvaluateGameState();
                if (this.state.ExitCode != Game.ExitCodes.InProgress)
                {
                    this.Finish(i + 1, state);

                    return this.state;
                }

                cooperationConditionalProbabilities = this.PredictionModel.GetCooperationConditionalProbabilities(map, state, BasicAgent.AgentIdentifier.AgentB);
                this.agentB.DetermineStep(this.map, this.state);
                this.PredictionModel.CalculateAndUpdateCooperationPrior(state, cooperationConditionalProbabilities, BasicAgent.AgentIdentifier.AgentB);
                this.EvaluateGameState();
                if (this.state.ExitCode != Game.ExitCodes.InProgress)
                {
                    this.Finish(i + 1, state);

                    return this.state;
                }

                this.pig.DetermineStep(this.map, this.state);
                this.EvaluateGameState();
                if (this.state.ExitCode != Game.ExitCodes.InProgress)
                {
                    this.Finish(i + 1, state);

                    return this.state;
                }

                this.map.Draw(i + 1, this.state);

                if (Program.PRINT_DEBUG_INFO)
                    Console.ReadLine();
            }

            throw new Exception("The game end check didn't fire successfully");
        }

        private void Finish(int iteration, State state)
        {
            this.map.Draw(iteration, this.state);

            this.agentA.configuration.initialCooperationGuess = state.CooperationProbabilityGuessA;
            this.agentB.configuration.initialCooperationGuess = state.CooperationProbabilityGuessB;

            if (Program.PRINT_DEBUG_INFO)
                Console.ReadLine();
        }

        private void EvaluateGameState()
        {
            // agent A has won
            if (this.map.IsCellExit(this.state.PositionAgentA.X, this.state.PositionAgentA.Y))
            {
                this.state.SetWinnerA();
            }

            // agent B has won
            if (this.map.IsCellExit(this.state.PositionAgentB.X, this.state.PositionAgentB.Y))
            {
                this.state.SetWinnerB();
            }

            // the pig was caught, i.e. the pig has no adjacent squares that it can move to
            if (this.map.GetAvailablePositions(this.state.PositionPig, state).Count() == 1)
            {
                this.state.SetWinnerBoth();
            }

            // the turns ran out
            if (this.state.TurnsLeft == 0)
            {
                this.state.SetOutOfTurns();
            }
        }
    }
}
