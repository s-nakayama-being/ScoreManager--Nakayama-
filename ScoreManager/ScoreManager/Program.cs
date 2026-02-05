using System;
using System.Text;

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
                PrintMenu();

                var wInput = Console.ReadLine();

                var wNormalizedInput = wInput.Normalize(NormalizationForm.FormKC);

                if (Enum.TryParse(wNormalizedInput, true, out ScoreManager.MenuActions wSelectedAction) && Enum.IsDefined(typeof(ScoreManager.MenuActions), wSelectedAction)) {
                    switch (wSelectedAction) {
                        case ScoreManager.MenuActions.DisplayScores:
                            wManager.DisplayScores();
                            break;

                        case ScoreManager.MenuActions.DisplayAllSubjectsAverage:
                            wManager.DisplayAllSubjectsAverage();
                            break;

                        case ScoreManager.MenuActions.DisplayEachSubjectAverage:
                            wManager.DisplayEachSubjectAverage();
                            break;

                        case ScoreManager.MenuActions.DisplayPassingStudents:
                            wManager.DisplayPassingStudents();
                            break;

                        case ScoreManager.MenuActions.DisplayScoresPerSubject:
                            Console.Write("科目名を入力：");
                            var wInputSubject = Console.ReadLine();
                            wManager.DisplayScoresPerSubject(wInputSubject);
                            break;

                        case ScoreManager.MenuActions.ExportScoresToCsv:
                            wManager.ExportScoresToCsv(C_OutputCsvFilePath);
                            break;

                        case ScoreManager.MenuActions.Exit:
                            Console.WriteLine("アプリケーションを終了します。");
                            return;

                        default:
                            Console.WriteLine("無効な入力です。");
                            break;
                    }
                } else {
                    Console.WriteLine("無効な入力です。");
                }
            }
        }

        /// <summary>
        /// メニュー項目をコンソールに表示する
        /// </summary>
        private static void PrintMenu() {
            Console.WriteLine();
            Console.WriteLine("=== 成績管理メニュー ===");
            Console.WriteLine($"{(int)ScoreManager.MenuActions.DisplayScores}. 一覧表示");
            Console.WriteLine($"{(int)ScoreManager.MenuActions.DisplayAllSubjectsAverage}. 平均点表示");
            Console.WriteLine($"{(int)ScoreManager.MenuActions.DisplayEachSubjectAverage}. 科目別平均点表示");
            Console.WriteLine($"{(int)ScoreManager.MenuActions.DisplayPassingStudents}. 合格者一覧");
            Console.WriteLine($"{(int)ScoreManager.MenuActions.DisplayScoresPerSubject}. 科目でフィルタ");
            Console.WriteLine($"{(int)ScoreManager.MenuActions.ExportScoresToCsv}. CSV出力");
            Console.WriteLine($"{(int)ScoreManager.MenuActions.Exit}. 終了");
            Console.Write("選択：");
        }
    }
}