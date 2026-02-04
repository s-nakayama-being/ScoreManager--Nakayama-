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
        /// 表示科目の教科リスト
        /// </summary>
        private static readonly string[] C_TargetSubjects = { "数学", "国語", "理科", "英語", "社会" };

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
        /// <param name="vInputCsvFilePath">CSVファイルのパス</param>
        public void LoadCsv(string vInputCsvFilePath) {
            if (!File.Exists(vInputCsvFilePath)) {
                Console.WriteLine(vInputCsvFilePath + "が見つかりません");
                return;
            }

            string[] wLines = File.ReadAllLines(vInputCsvFilePath, Encoding.UTF8);

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

            foreach (string wSubject in C_TargetSubjects) {
                if (wSubjectAverage.TryGetValue(wSubject, out double wAverage)) {
                    Console.WriteLine("{0}: {1:F2}", wSubject, wAverage);
                } else {
                    Console.WriteLine("{0}: 0", wSubject);
                }
            }
        }

        /// <summary>
        /// 合格者を抽出する
        /// </summary>
        /// <returns>合格者リスト</returns>
        private List<StudentScore> GetPassingStudents() {
            return this.FStudentScores.Where(x => x.Score >= C_PassingScore).ToList();
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

        /// <summary>
        /// 科目ごとに成績データを抽出する
        /// </summary>
        /// <param name="vSubject">入力された科目名</param> 
        private List<StudentScore> GetScoresPerSubject(string vSubject) {
            return this.FStudentScores.Where(x => x.Subject.Equals(vSubject, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        /// <summary>
        /// ユーザーに指定された科目の成績データを表示する
        /// </summary>
        /// <param name="vSubject">入力された科目名</param>
        public void DisplayScoresPerSubject(string vSubject) {
            if (string.IsNullOrWhiteSpace(vSubject)) {
                Console.WriteLine("エラー：科目名が指定されていません。");
                return;
            }

            Console.WriteLine($"{vSubject} の成績一覧");

            var wFilteredStudentsBySubject = this.GetScoresPerSubject(vSubject);

            if (!wFilteredStudentsBySubject.Any()) {
                Console.WriteLine($"エラー：{vSubject} の成績データが見つかりません。");
                return;
            }

            foreach (var wStudent in wFilteredStudentsBySubject) {
                this.PrintStudent(wStudent);
            }
        }

        /// <summary>
        /// 成績リストに格納している全データをCSV形式で出力する
        /// </summary> 
        /// <param name="vOutputCsvFilePath">出力先CSVファイルのパス</param>
        public void ExportScoresToCsv(string vOutputCsvFilePath) {
            try {
                using (var wWriter = new StreamWriter(vOutputCsvFilePath, false, Encoding.UTF8)) {
                    wWriter.WriteLine("Name,Subject,Score");

                    foreach (var wStudent in this.FStudentScores) {
                        wWriter.WriteLine($"{wStudent.Name},{wStudent.Subject},{wStudent.Score}");
                    }
                }

                Console.WriteLine($"CSV出力完了：{Path.GetFileName(vOutputCsvFilePath)}");

            } catch (Exception wEx) {
                Console.WriteLine($"CSV出力エラー: {wEx.Message}");
            }
        }
    }
}