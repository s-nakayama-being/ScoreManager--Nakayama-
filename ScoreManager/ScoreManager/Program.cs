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

            var wPassingStudents = wManager.GetPassingStudents();
            Console.WriteLine($"合格者 {wPassingStudents.Count} 名:");
            foreach (var wStudent in wPassingStudents) {
                Console.WriteLine("{0, -3} | {1, -2} | {2, 2}", wStudent.Name, wStudent.Subject, wStudent.Score);
            }
        }
    }
}


