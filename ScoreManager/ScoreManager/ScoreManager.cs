using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace ScoreManager {
    /// <summary>
    /// 成績リストの読み込み、集計、表示、出力を行うマネージャクラス。
    /// </summary>
    public class ScoreManager {
        /// <summary>
        /// 成績データを保持するリスト
        /// </summary>
        private List<StudentScore> FStudents = new List<StudentScore>();

        /// <summary>
        /// 今回読み取るCSVファイル（成績データ）のパス
        /// </summary>
        private const string C_FilePath = "scores.csv";

        /// <summary>
        /// CSVファイルの読み込み
        /// </summary>
        public void ReadCsv() {
            if (!File.Exists(C_FilePath)) {
                Console.WriteLine(C_FilePath + "が見つかりません：");
                return;
            }

            string[] wLines = File.ReadAllLines(C_FilePath, Encoding.UTF8);

            string wPattern = @"^\s*(?<name>[a-zA-Z\p{IsHiragana}\p{IsKatakana}\p{IsCJKUnifiedIdeographs}]+)\s*,"+
                              @"\s*(?<subject>[\p{IsHiragana}\p{IsKatakana}\p{IsCJKUnifiedIdeographs}]+)\s*,"+
                              @"\s*(?<score>100|[1-9][0-9]?|0)\s*$";

            foreach (string wLine in wLines) {
                if (String.IsNullOrEmpty(wLine) || wLine.StartsWith("#")) {
                    continue;
                }

                bool wPattenMatch = Regex.IsMatch(wLine, wPattern);
                if (!wPattenMatch) {
                    Console.WriteLine($"不正な行: {wLine}");
                    continue;
                }

                string[] wItems = wLine.Split(',');

                string wName = wItems[0].Trim();
                string wSubject = wItems[1].Trim();
                int wScore = int.Parse(wItems[2].Trim());

                StudentScore wNewStudents = new StudentScore(wName, wSubject, wScore);

                this.FStudents.Add(wNewStudents);
            }

            Console.WriteLine($"{this.FStudents.Count} 件のデータを読み込みました。");
        }
    }
}
