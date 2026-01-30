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
        private List<StudentScore> FStudentScores = new List<StudentScore>();

        /// <summary>
        /// 正規表現パターン：名前、科目、点数
        /// </summary>
        private const string C_ScoreRegexPattern = @"^\s*(?<name>[a-zA-Z\p{IsHiragana}\p{IsKatakana}\p{IsCJKUnifiedIdeographs}ー々]+)\s*," +
                                           @"\s*(?<subject>[\p{IsHiragana}\p{IsKatakana}\p{IsCJKUnifiedIdeographs}]+)\s*," +
                                           @"\s*(?<score>100|[1-9][0-9]?|0)\s*$";

        /// <summary>
        /// 表示科目の教科リスト
        /// </summary>
        private static readonly string[] FTargetSubjects = { "数学", "国語", "理科", "英語", "社会" };

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

                this.FStudentScores.Add(new StudentScore(wName, wSubject, wScore));
            }

            Console.WriteLine($"{this.FStudentScores.Count} 件のデータを読み込みました。");
        }

        /// <summary>
        /// 成績一覧を表示する
        /// </summary>
        public void DisplayScores() {
            Console.WriteLine("=== 成績一覧 ===");

            foreach (var wStudent in this.FStudentScores) {
                Console.WriteLine("{0, -3} | {1, -2} | {2, 2}", wStudent.Name, wStudent.Subject, wStudent.Score);
            }
        }

        /// 全教科の平均点を計算する
        /// </summary>
        /// <returns>平均点（データがない場合は0）</returns>
        private double CalculateAllSubjectsAverage() {
            if (this.FStudentScores.Count == 0) return 0;

            return this.FStudentScores.Average(x => x.Score);
        }

        /// <summary>
        /// 全教科の平均点を表示する
        /// </summary>
        public void DisplayAllSubjectsAverage() {
            double wAverageScore = this.CalculateAllSubjectsAverage();
            Console.WriteLine("平均点: {0:F2}", wAverageScore);
        }

        /// <summary>
        /// 各教科の平均点を計算する
        /// </summary>
        /// <returns>教科名をキー、平均点を値とする辞書</returns>
        private Dictionary<string, double> CalculateEachSubjectAverage() {
            if (this.FStudentScores.Count == 0) return new Dictionary<string, double>();

            return this.FStudentScores.GroupBy(x => x.Subject).ToDictionary(y => y.Key, y => y.Average(z => z.Score));
        }

        /// <summary>
        /// 各教科の平均点を表示する
        /// </summary>
        public void DisplayEachSubjectAverage() {
            var wSubjectAverage = this.CalculateEachSubjectAverage();

            Console.WriteLine("科目別平均点:");

            foreach (string wSubject in FTargetSubjects) {
                if (wSubjectAverage.TryGetValue(wSubject, out double wAverage)) {
                    Console.WriteLine("{0}: {1:F2}", wSubject, wAverage);
                } else {
                    Console.WriteLine("{0}: 0", wSubject);
                }
            }
        }
    }
}
