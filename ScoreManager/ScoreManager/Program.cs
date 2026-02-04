using System;

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

            Console.WriteLine("=== 成績管理アプリ ===");
            Console.WriteLine(".exeと同階層にあるCSVファイルから成績データを読み込みます。");
            wManager.LoadCsv(C_InputCsvFilePath);

            while (true) {
                Console.WriteLine();
                Console.WriteLine("=== 成績管理メニュー ===");
                Console.WriteLine("1. 一覧表示");
                Console.WriteLine("2. 平均点表示");
                Console.WriteLine("3. 科目別平均点表示");
                Console.WriteLine("4. 合格者一覧");
                Console.WriteLine("5. 科目でフィルタ");
                Console.WriteLine("6. CSV出力");
                Console.WriteLine("0. 終了");
                Console.Write("選択：");

                var wInputNumber = Console.ReadLine();

                switch (wInputNumber) {
                    case "1":
                        wManager.DisplayScores();
                        break;

                    case "2":
                        wManager.DisplayAllSubjectsAverage();
                        break;

                    case "3":
                        wManager.DisplayEachSubjectAverage();
                        break;

                    case "4":
                        wManager.DisplayPassingStudents();
                        break;

                    case "5":
                        Console.Write("科目名を入力：");
                        var wInputSubject = Console.ReadLine();
                        wManager.DisplayScoresPerSubject(wInputSubject);
                        break;

                    case "6":
                        wManager.ExportScoresToCsv(C_OutputCsvFilePath);
                        break;

                    case "0":
                        Console.WriteLine("アプリケーションを終了します。");
                        return;

                    default:
                        Console.WriteLine("無効な入力です。");
                        break;
                }
            }
        }
    }
}