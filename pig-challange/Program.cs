using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace pig_challenge
{
    class Program
    {
        const int NUM_GAMES = 10;
        const int MAX_ITERATIONS = 25;

        static void Main(string[] args)
        {
            float[,] floatArray = new float[81796, 8];
            FillConfigurations(floatArray);

            Console.WriteLine("Welcome to the pig challenge!");
            Console.WriteLine("Press Enter to start a new tournament.");

            RunForRandomConfigurations(floatArray);

            Console.ReadLine();
            
        }

        static void FillConfigurations(float[,] floatArray)
        {
            int count = 0;
            for (float alphaA = 0.0f; alphaA <= 1.0f; alphaA += 0.1f)
                for (float betaA = 0.0f; betaA <= 1.0f; betaA += 0.1f)
                    for (float gammaA = 0.0f; gammaA <= 1.0f; gammaA += 0.1f)
                        for (float deltaA = 0.0f; deltaA <= 1.0f; deltaA += 0.1f)
                        {
                            if (alphaA + betaA + gammaA + deltaA != 1.0f)
                                continue;
                            for (float alphaB = 0.0f; alphaB <= 1.0f; alphaB += 0.1f)
                                for (float betaB = 0.0f; betaB <= 1.0f; betaB += 0.1f)
                                    for (float gammaB = 0.0f; gammaB <= 1.0f; gammaB += 0.1f)
                                        for (float deltaB = 0.0f; deltaB <= 1.0f; deltaB += 0.1f)
                                        {
                                            if (alphaB + betaB + gammaB + deltaB != 1.0f)
                                                continue;

                                            floatArray[count, 0] = alphaA;
                                            floatArray[count, 1] = betaA;
                                            floatArray[count, 2] = gammaA;
                                            floatArray[count, 3] = deltaA;

                                            floatArray[count, 4] = alphaB;
                                            floatArray[count, 5] = betaB;
                                            floatArray[count, 6] = gammaB;
                                            floatArray[count, 7] = deltaB;

                                            count++;
                                        }
                        }
        }

        static void RunForRandomConfigurations(float[,] floatArray)
        {
            var csv = new StringBuilder();
            //Stopwatch stopwatch = new Stopwatch();

            float[] valuesA = new float[4], valuesB = new float[4];

            int count = 0;
            for(int i = 0; i < 81796; i++)
            {
                count++;
                if(count % 100 == 0)
                    Console.WriteLine($"Iteration {count}");
                //stopwatch.Reset();
                //stopwatch.Start();


                AgentConfiguration agentConfigurationA = new AgentConfiguration { alpha = floatArray[i, 0], beta = floatArray[i, 1], gamma = floatArray[i, 2], delta = floatArray[i, 3], minCooperationLimit = 0.75f, maxDefectLimit = 0.25f };
                AgentConfiguration agentConfigurationB = new AgentConfiguration { alpha = floatArray[i, 4], beta = floatArray[i, 5], gamma = floatArray[i, 6], delta = floatArray[i, 7], minCooperationLimit = 0.75f, maxDefectLimit = 0.25f };

                Tournament tournament = new Tournament(NUM_GAMES, MAX_ITERATIONS);
                csv.AppendLine($"Configuration A: {valuesA[0]}, {valuesA[1]}, {valuesA[2]}, {valuesA[3]} \n Configuration B: {valuesB[0]}, {valuesB[1]}, {valuesB[2]}, {valuesB[3]} ");
                Result res = tournament.Run(agentConfigurationA, agentConfigurationB);
                csv.AppendLine($"Total scores: \n Agent A: {res.scoreA}, Agent B: {res.scoreB}");

                //stopwatch.Stop();
                //Console.WriteLine("Ticks: " + stopwatch.ElapsedTicks +
                //            " mS: " + stopwatch.ElapsedMilliseconds);
                //Console.ReadLine();
            }
            File.WriteAllText("./results.csv", csv.ToString());
        }
    }
}
