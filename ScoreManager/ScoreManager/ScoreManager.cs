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
        /// 合格点の基準値
        /// </summary>
        private const int C_PassingScore = 60;

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
                this.PrintStudent(wStudent);
            }
        }

        /// <summary>
        /// 生徒一人分の情報をフォーマットして出力するヘルパーメソッド
        /// </summary>
        /// <param name="vStudent">生徒データ</param>
        private void PrintStudent(StudentScore vStudent) {
            Console.WriteLine("{0, -3} | {1, -2} | {2, 2}", vStudent.Name, vStudent.Subject, vStudent.Score);
        }

        /// <summary>
        /// 合格者を抽出する
        /// </summary>
        /// <returns>合格者リスト</returns>
        private List<StudentScore> GetPassingStudents() {
            return this.FStudents.Where(x => x.Score >= C_PassingScore).ToList();
        }

        /// <summary>
        /// 合格者一覧を表示する
        /// </summary>
        public void DisplayPassingStudents() {
            var wPassingStudents = this.GetPassingStudents();
            Console.WriteLine($"合格者 {wPassingStudents.Count} 名");
            foreach (var wStudent in wPassingStudents) {
                this.PrintStudent(wStudent);
            }
        }
    }
}
