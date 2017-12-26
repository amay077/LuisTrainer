using System;

namespace LuisTrainer
{
    class Program
    {
        /// <summary>
        /// The entry point of the program, where the program control starts and ends.
        /// </summary>
        /// <param name="args">
        /// 引数は 4つ、いずれも必須です。次のように渡します。
        /// dotnet LuisTrainer.dll "path/to/examples.csv" appid=LUISのAppID appkey=LUISのAppKey versionid=LUISAppのバージョン
        /// 例: dotnet LuisTrainer.dll "./csv/examples.csv" appid=516xxxxx-xxxx-xxxx-xxxx-xzxxxxxxxxxxx appkey=5e38xxxxxxxxxxxxxxxxxxxxxxxxxxxx versionid=0.1
        /// </param>
        static void Main(string[] args)
        {
            Console.WriteLine("Program started.");
            var config = args.ToConfig();
            Console.WriteLine(config);

            var trainer = new LuisTrainerModule();

            trainer.Run(config);
            Console.WriteLine("Program finished.");
        }
    }
}
