using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace pig_challenge
{
    class Program
    {
        const int NUM_GAMES = 100;
        const int MAX_ITERATIONS = 25;

        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the pig challenge!");
            Console.WriteLine("Press Enter to start a new tournament.");

            RunForRandomConfigurations();

            Console.ReadLine();
            
        }

        static void RunForRandomConfigurations()
        {
            var csv = new StringBuilder();
            //Stopwatch stopwatch = new Stopwatch();

            float[] valuesA = new float[4], valuesB = new float[4];

            int count = 0;
            for (float alphaA = 0.3f; alphaA < 1.0f; alphaA += 0.3f)
                for (float alphaB = 0.3f; alphaB < 1.0f; alphaB += 0.3f)
                    for (float betaA = 0.3f; betaA < 1.0f; betaA += 0.3f)
                        for (float betaB = 0.3f; betaB < 1.0f; betaB += 0.3f)
                            for (float gammaA = 0.3f; gammaA < 1.0f; gammaA += 0.3f)
                                for (float gammaB = 0.3f; gammaB < 1.0f; gammaB += 0.3f)
                                    for (float deltaA = 0.3f; deltaA < 1.0f; deltaA += 0.3f)
                                        for (float deltaB = 0.3f; deltaB < 1.0f; deltaB += 0.3f)
                                        {
                                            count++;
                                            if(count % 50 == 0)
                                                Console.WriteLine($"Iteration {count}");
                                            //stopwatch.Reset();
                                            //stopwatch.Start();

                                            valuesA[0] = alphaA;
                                            valuesA[1] = betaA;
                                            valuesA[2] = gammaA;
                                            valuesA[3] = deltaA;

                                            valuesB[0] = alphaB;
                                            valuesB[1] = betaB;
                                            valuesB[2] = gammaB;
                                            valuesB[3] = deltaB;

                                            valuesA = NormalizeValues(valuesA);
                                            valuesB = NormalizeValues(valuesB);


                                            AgentConfiguration agentConfigurationA = new AgentConfiguration { alpha = alphaA, beta = betaA, gamma = gammaA, delta = deltaA };
                                            AgentConfiguration agentConfigurationB = new AgentConfiguration { alpha = alphaB, beta = betaB, gamma = gammaB, delta = deltaB };

                                            Tournament tournament = new Tournament(NUM_GAMES, MAX_ITERATIONS);
                                            csv.AppendLine($"Configuration A: {valuesA[0]}, {valuesA[1]}, {valuesA[2]}, {valuesA[3]} \n Configuration B: {valuesB[0]}, {valuesB[1]}, {valuesB[2]}, {valuesB[3]} ");
                                            Result res = tournament.Run(agentConfigurationA, agentConfigurationB);
                                            csv.AppendLine($"Total scores: \n Agent A: {res.scoreA}, Agent B: {res.scoreB}");

                                            //stopwatch.Stop();
                                            //Console.WriteLine("Ticks: " + stopwatch.ElapsedTicks +
                                            //            " mS: " + stopwatch.ElapsedMilliseconds);
                                            //Console.ReadLine();
                                            //300 ms
                                        }
            File.WriteAllText("./results.csv", csv.ToString());




        }

        static float[] NormalizeValues(float[] values)
        {
            float sum = 0.0f;
            for (int i = 0; i < 4; i++)
                sum += values[i];

            for (int i = 0; i < 4; i++)
                values[i] = values[i] / sum;

            return values;
        }
    }
}
