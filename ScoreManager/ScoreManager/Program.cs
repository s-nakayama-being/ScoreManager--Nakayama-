namespace ScoreManager {
    public class Program {

        private const string C_FilePath = "scores.csv";
        static void Main(string[] args) {
            var wManager = new ScoreManager();

            wManager.ReadCsv(C_FilePath);             
        }
    }
}


