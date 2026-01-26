using System;
using System.Collections.Generic;
using System.Xml.Linq;

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

            Dictionary<string, double> wSubjectAverage = wManager.CalculateEachSubjectAverage();
            string[] wTargetSubject = { "数学", "国語", "理科", "英語", "社会" };
            Console.WriteLine("科目別平均点:");
            if (wSubjectAverage.Count == 0) {
                Console.WriteLine(0);
                return;
            }

            foreach (string wSubject in wTargetSubject) {
                if (!wSubjectAverage.ContainsKey(wSubject)) {
                    Console.WriteLine("{0,-1}: {1:F0}", wSubject, 0);
                } else {
                    Console.WriteLine("{0,-1}: {1:F2}", wSubject, wSubjectAverage[wSubject]);
                }
            }
        }
    }
}


