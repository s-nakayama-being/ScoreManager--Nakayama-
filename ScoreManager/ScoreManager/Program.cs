namespace ScoreManager {
    public class Program {

        /// <summary>
        /// 今回読み取るCSVファイルのパス
        /// </summary>
        private const string C_FilePath = "scores.csv";
        static void Main(string[] args) {
            var wManager = new ScoreManager();

            wManager.ReadCsv(C_FilePath);             
        }
    }
}


