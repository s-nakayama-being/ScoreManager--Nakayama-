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

            wManager.DisplayAllSubjectsAverage();

            wManager.DisplayEachSubjectAverage();

            wManager.DisplayPassingStudents();

            Console.Write("科目名を入力：");
            var wInputSubject = Console.ReadLine();
            wManager.DisplayScoresPerSubject(wInputSubject);
        }
    }
}


