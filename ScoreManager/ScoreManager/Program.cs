using System;

namespace ScoreManager {
    public class Program {

        /// <summary>
        /// 処理対象となるCSVファイルのパス
        /// </summary>
        private const string C_FilePath = "../../scores.csv";
        static void Main(string[] args) {
            var wManager = new ScoreManager();

            wManager.LoadCsv(C_FilePath);

            wManager.DisplayScores();

            double wAverageScore = wManager.CalculateAllSubjectsAverage();
            Console.WriteLine("平均点: {0:F2}", wAverageScore);
        }
    }
}


