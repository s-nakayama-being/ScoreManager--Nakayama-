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

        /// <summary>
        /// メニュー項目をh定義する内部クラス
        /// </summary>
        private class MenuItem {
            /// <summary>
            /// メニューId
            /// </summary>
            public int Id { get; set; }

            /// <summary>
            /// メニュー説明文
            /// </summary>
            public string Description { get; set; }

            /// <summary>
            /// メニュー選択時のアクション
            /// </summary>
            public Action Action { get; set; }
        }

        static void Main(string[] args) {
            var wManager = new ScoreManager();

            var wMenuItems = new MenuItem[] {
                new MenuItem { Id = 1, Description = "一覧表示",         Action = wManager.DisplayScoreRecords },
                new MenuItem { Id = 2, Description = "平均点表示",       Action = wManager.DisplayAllSubjectsAverage },
                new MenuItem { Id = 3, Description = "科目別平均点表示", Action = wManager.DisplayEachSubjectAverage },
                new MenuItem { Id = 4, Description = "合格者一覧",       Action = wManager.DisplayPassingRecords },
                new MenuItem { Id = 5, Description = "科目でフィルタ",   Action = () => {
                    Console.Write("科目名を入力：");
                    var wInputSubject = Console.ReadLine();
                    wManager.DisplayScoreRecordsPerSubject(wInputSubject);
                }},
                new MenuItem { Id = 6, Description = "CSV出力", Action = () => wManager.ExportScoreRecordsToCsv(C_OutputCsvFilePath) },
                new MenuItem { Id = 0, Description = "終了", Action = () => {
                    Console.WriteLine("アプリケーションを終了します。");
                    Console.WriteLine("何かキーを押すと終了します。");
                    Console.ReadKey();
                }}
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