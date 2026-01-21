namespace ScoreManager {
    public class Program {
        static void Main(string[] args) {
            var wManager = new ScoreManager();

            const string C_FilePath = "scores.csv";

            wManager.ReadCsv(C_FilePath);             
        }
    }
}


