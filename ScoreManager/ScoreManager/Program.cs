using System;
using System.Linq;
using System.Text;

namespace ScoreManager {
    public class Program {

        /// <summary>
        /// 処理対象となるCSVファイルのパス
        /// </summary>
        private static readonly string C_InputCsvFilePath = "../../scores.csv";

        /// <summary>
        /// 成績データを出力するCSVファイルのパス
        /// </summary>
        private static readonly string C_OutputCsvFilePath = "../../output_scores.csv";

        static void Main(string[] args) {
            var wManager = new ScoreManager();

            var wMenuItems = new MenuItem[] {
                new MenuItem(1, "一覧表示",         wManager.DisplayScoreRecords),
                new MenuItem(2, "平均点表示",       wManager.DisplayAllSubjectsAverage),
                new MenuItem(3, "科目別平均点表示", wManager.DisplayEachSubjectAverage),
                new MenuItem(4, "合格者一覧",       wManager.DisplayPassingRecords),
                new MenuItem(5, "科目でフィルタ",   () => {
                    Console.Write("科目名を入力：");
                    var wInputSubject = Console.ReadLine();
                    wManager.DisplayScoreRecordsPerSubject(wInputSubject);
                }),
                new MenuItem(6, "CSV出力",          () => wManager.ExportScoreRecordsToCsv(C_OutputCsvFilePath)),
                new MenuItem(0, "終了",             () => {
                    Console.WriteLine("アプリケーションを終了します。");
                    Console.WriteLine("何かキーを押すと終了します。");
                    Console.ReadKey();
                    Environment.Exit(0);
                })
            };

            Console.WriteLine("=== 成績管理アプリ ===");
            Console.WriteLine(".exeと同階層にあるCSVファイルから成績データを読み込みます。");

            wManager.LoadCsv(C_InputCsvFilePath);

            while (true) {
                Console.WriteLine("=== 成績管理メニュー ===");
                foreach (var wItem in wMenuItems) {
                    Console.WriteLine($"{wItem.Id}. {wItem.Description}");
                }

                Console.Write("選択：");

                var wRawInput = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(wRawInput)) {
                    Console.WriteLine("無効な入力です。");
                    continue;
                }

                var wNormalizedInput = wRawInput.Normalize(NormalizationForm.FormKC);

                if (!int.TryParse(wNormalizedInput, out int wSelectedId)) {
                    Console.WriteLine("無効な入力です。");
                    continue;
                }

                var wTargetMenu = wMenuItems.SingleOrDefault(x => x.Id == wSelectedId);

                if (wTargetMenu == null) {
                    Console.WriteLine("無効な入力です。");
                    continue;
                }

                wTargetMenu.Action.Invoke();
            }
        }
    }
}