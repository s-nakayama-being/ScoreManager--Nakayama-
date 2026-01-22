namespace ScoreManager {
    public class Program {

        /// <summary>
        /// 処理対象となるCSVファイルのパス
        /// </summary>
        private const string C_FilePath = "scores.csv";
        static void Main(string[] args) {
            var wManager = new ScoreManager();

            wManager.ReadCsv(C_FilePath); 
            
            wManager.DisplayScores();
        }
    }
}


