namespace ScoreManager {
    public class Program {

        /// <summary>
        /// 処理対象となるCSVファイルのパス
        /// </summary>
        private const string C_InputCsvFilePath = "../../scores.csv";

        /// <summary>
        /// 成績データを出力するCSVファイルのパス
        /// </summary>
        private const string C_OutputCsvFilePath = "../../output_scores.csv";

        static void Main(string[] args) {
            var wManager = new ScoreManager();

            wManager.LoadCsv(C_InputCsvFilePath);

            wManager.DisplayScores();

            wManager.DisplayAllSubjectsAverage();

            wManager.DisplayEachSubjectAverage();

            wManager.DisplayPassingStudents();

            wManager.ExportScoresToCsv(FOutputCsvFilePath);
        }
    }
}


