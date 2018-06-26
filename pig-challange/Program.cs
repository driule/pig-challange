using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace pig_challenge
{
    class Program
    {
        public const bool PRINT_DEBUG_INFO = true;

        const int NUM_GAMES = 10;
        const int MAX_ITERATIONS = 25;

        const int TOTAL_CONFIGURATIONS = 108900;

        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the pig challenge!");
            Console.WriteLine("Press Enter to start a new tournament.");

            if (PRINT_DEBUG_INFO)
                RunDemo();
            else
                RunForRandomConfigurations();

            Console.ReadLine();
            
        }

        static void FillConfigurations(float[,] floatArray)
        {
            int step = 10;
            int count = 0;
            for (int alphaA = 0; alphaA <= 100; alphaA += step)
                for (int betaA = 0; betaA <= 100; betaA += step)
                    for (int gammaA = 0; gammaA <= 100; gammaA += step)
                        //for (int deltaA = 0; deltaA <= 100; deltaA += step)
                    {
                        if (alphaA + betaA + gammaA != 100)
                            continue;
                        for (int alphaB = 0; alphaB <= 100; alphaB += step)
                            for (int betaB = 0; betaB <= 100; betaB += step)
                                for (int gammaB = 0; gammaB <= 100; gammaB += step)
                                    //for (int deltaB = 0; deltaB <= 100; deltaB += step)
                                {
                                    if (alphaB + betaB + gammaB != 100)
                                        continue;

                                    int initialGuessStep = 25;
                                    for (int initialGuessA = 0; initialGuessA <= 100; initialGuessA += initialGuessStep)
                                    {
                                        for (int initialGuessB = 0; initialGuessB <= 100; initialGuessB += initialGuessStep)
                                        {
                                            floatArray[count, 0] = alphaA / 100.0f;
                                            floatArray[count, 1] = betaA / 100.0f;
                                            floatArray[count, 2] = gammaA / 100.0f;
                                            floatArray[count, 3] = initialGuessA / 100.0f;

                                            floatArray[count, 4] = alphaB / 100.0f;
                                            floatArray[count, 5] = betaB / 100.0f;
                                            floatArray[count, 6] = gammaB / 100.0f;
                                            floatArray[count, 7] = initialGuessB / 100.0f;

                                            count++;
                                        }
                                    }
                                }
                    }

            //Console.WriteLine(count);
            //Console.ReadLine();
        }

        static void RunDemo()
        {
            AgentConfiguration agentConfigurationA = new AgentConfiguration(0.3f, 0.3f, 0.3f, 0.1f, 0.75f, 0.25f, 0.5f, true);
            AgentConfiguration agentConfigurationB = new AgentConfiguration(0.3f, 0.3f, 0.3f, 0.1f, 0.75f, 0.25f, 0.5f, true);

            Tournament tournament = new Tournament(NUM_GAMES, MAX_ITERATIONS);
            Result res = tournament.Run(agentConfigurationA, agentConfigurationB);
        }

        static void RunForRandomConfigurations()
        {
            float[,] floatArray = new float[TOTAL_CONFIGURATIONS, 8];
            FillConfigurations(floatArray);

            Result[] resultArray = new Result[TOTAL_CONFIGURATIONS];
            var csv = new StringBuilder();

            //Stopwatch stopwatch = new Stopwatch();

            int count = 0;
            Parallel.For(
                0,
                4,
                id =>
                {

                    for (int i = id * TOTAL_CONFIGURATIONS / 4; i < (id + 1) * TOTAL_CONFIGURATIONS / 4; i++)
                    {
                        count++;
                        if (count % 100 == 0)
                            Console.WriteLine($"Iteration {count}");
                        //stopwatch.Reset();
                        //stopwatch.Start();


                        AgentConfiguration agentConfigurationA = new AgentConfiguration(floatArray[i, 0], floatArray[i, 1], floatArray[i, 2], 0.0f, 1.0f, 0.0f, floatArray[i, 3], false );
                        AgentConfiguration agentConfigurationB = new AgentConfiguration(floatArray[i, 4], floatArray[i, 5], floatArray[i, 6], 0.0f, 1.0f, 0.0f, floatArray[i, 7], false );

                        Tournament tournament = new Tournament(NUM_GAMES, MAX_ITERATIONS);

                        Result res = tournament.Run(agentConfigurationA, agentConfigurationB);
                        resultArray[i] = res; ;

                        //stopwatch.Stop();
                        //Console.WriteLine("Ticks: " + stopwatch.ElapsedTicks +
                        //            " mS: " + stopwatch.ElapsedMilliseconds);
                        //Console.ReadLine();
                    }
                }
            );

            csv.AppendLine($"\"sep=;\"");

            for (int i = 0; i < TOTAL_CONFIGURATIONS; i++)
            {
                csv.AppendLine(
                    $"{floatArray[i, 0]}_{floatArray[i, 1]}_{floatArray[i, 2]}_{floatArray[i, 3]};" +
                    $"{floatArray[i, 4]}_{floatArray[i, 5]}_{floatArray[i, 6]}_{floatArray[i, 7]};" +
                    $"{resultArray[i].scoreA};{resultArray[i].scoreB};"
                );
            }

            File.WriteAllText("./results.csv", csv.ToString());

            Console.WriteLine("Writing results done!");
        }
    }
}
