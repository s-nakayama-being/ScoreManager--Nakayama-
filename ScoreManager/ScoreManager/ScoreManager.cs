using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        /// 正規表現パターン：名前、科目、点数
        /// </summary>
        private const string C_ScoreRegexPattern = @"^\s*(?<name>[a-zA-Z\p{IsHiragana}\p{IsKatakana}\p{IsCJKUnifiedIdeographs}ー々]+)\s*," +
                                           @"\s*(?<subject>[\p{IsHiragana}\p{IsKatakana}\p{IsCJKUnifiedIdeographs}]+)\s*," +
                                           @"\s*(?<score>100|[1-9][0-9]?|0)\s*$";

        /// <summary>
        /// CSVファイルを読み込み、成績リストに格納する
        /// </summary>
        /// <param name="vFilePath">CSVファイルのパス</param>
        public void LoadCsv(string vFilePath) {
            if (!File.Exists(vFilePath)) {
                Console.WriteLine(vFilePath + "が見つかりません");
                return;
            }

            string[] wLines = File.ReadAllLines(vFilePath, Encoding.UTF8);

            foreach (string wLine in wLines) {
                if (string.IsNullOrEmpty(wLine) || wLine.StartsWith("#")) {
                    continue;
                }

                Match wMatch = Regex.Match(wLine, C_ScoreRegexPattern);
                if (!wMatch.Success) {
                    Console.WriteLine($"不正な行: {wLine}");
                    continue;
                }

                string wName = wMatch.Groups["name"].Value;
                string wSubject = wMatch.Groups["subject"].Value;
                int wScore = int.Parse(wMatch.Groups["score"].Value);

                this.FStudents.Add(new StudentScore(wName, wSubject, wScore));
            }

            Console.WriteLine($"{this.FStudents.Count} 件のデータを読み込みました。");
        }

        /// <summary>
        /// 成績一覧を表示する
        /// </summary>
        public void DisplayScores() {
            Console.WriteLine("=== 成績一覧 ===");

            foreach (var wStudent in this.FStudents) {
                Console.WriteLine("{0, -3} | {1, -2} | {2, 2}", wStudent.Name, wStudent.Subject, wStudent.Score);
            }
        }

        /// <summary>
        /// 全教科の平均点を計算する
        /// </summary>
        /// <returns>平均点（データがない場合は0）</returns>
        public double CalculateAllSubjectsAverage() {
            if (this.FStudents.Count == 0) {
                return 0;
            }

            return this.FStudents.Average(student => student.Score);
        }
    }
}
