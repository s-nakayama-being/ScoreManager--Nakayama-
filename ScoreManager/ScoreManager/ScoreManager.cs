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
        private readonly List<StudentScore> FScoreRecords = new List<StudentScore>();

        /// <summary>
        /// 表示科目の教科リスト
        /// </summary>
        private static readonly string[] C_TargetSubjects = { "数学", "国語", "理科", "英語", "社会" };

        /// <summary>
        /// 正規表現パターン：名前、科目、点数
        /// </summary>
        private const string C_ScoreRegexPattern =
            @"^\s*(?<name>[a-zA-Z\p{IsHiragana}\p{IsKatakana}\p{IsCJKUnifiedIdeographs}ー々]+)\s*," +
            @"\s*(?<subject>[\p{IsHiragana}\p{IsKatakana}\p{IsCJKUnifiedIdeographs}]+)\s*," +
            @"\s*(?<score>100|[1-9][0-9]?|0)\s*$";

        /// <summary>
        /// 合格点の基準値
        /// </summary>
        private static readonly int C_PassingScore = 60;

        /// <summary>
        /// CSVファイルを読み込み、成績リストに格納する
        /// </summary>
        /// <param name="vInputCsvFilePath">CSVファイルのパス</param>
        public void LoadCsv(string vInputCsvFilePath) {
            if (!File.Exists(vInputCsvFilePath)) {
                Console.WriteLine(vInputCsvFilePath + "が見つかりません");
                return;
            }

            foreach (string wLine in File.ReadLines(vInputCsvFilePath, Encoding.UTF8)) {
                if (string.IsNullOrEmpty(wLine) || wLine.StartsWith("#")) continue;

                Match wMatch = Regex.Match(wLine, C_ScoreRegexPattern);
                if (!wMatch.Success) {
                    Console.WriteLine($"不正な行: {wLine}");
                    continue;
                }

                string wName = wMatch.Groups["name"].Value;
                string wSubject = wMatch.Groups["subject"].Value;
                int wScore = int.Parse(wMatch.Groups["score"].Value);

                this.FScoreRecords.Add(new StudentScore(wName, wSubject, wScore));
            }

            Console.WriteLine($"{this.FScoreRecords.Count} 件のデータを読み込みました。");
        }

        /// <summary>
        /// 成績一覧を表示する
        /// </summary>
        public void DisplayScoreRecords() {
            Console.WriteLine("=== 成績一覧 ===");

            foreach (var wScoreRecord in this.FScoreRecords) {
                this.PrintScoreRecord(wScoreRecord);
            }
        }

        /// <summary>
        /// 成績レコードをフォーマットして出力するヘルパーメソッド
        /// </summary>
        /// <param name="vScoreRecord">成績データ</param>
        private void PrintScoreRecord(StudentScore vScoreRecord) {
            Console.WriteLine("{0, -3} | {1, -2} | {2, 2}", vScoreRecord.Name, vScoreRecord.Subject, vScoreRecord.Score);
        }

        /// <summary>
        /// 全教科の平均点を計算する
        /// </summary>
        /// <returns>平均点（データがない場合は0）</returns>
        private double CalculateAllSubjectsAverage() {
            if (this.FScoreRecords.Count == 0) return 0;

            return this.FScoreRecords.Average(x => x.Score);
        }

        /// <summary>
        /// 全教科の平均点を表示する
        /// </summary>
        public void DisplayAllSubjectsAverage() {
            double wAverageScore = this.CalculateAllSubjectsAverage();
            Console.WriteLine("平均点: {0:F2}", wAverageScore);
        }

        /// <summary>
        /// 各教科の平均点を表示する
        /// </summary>
        public void DisplayEachSubjectAverage() {
            Console.WriteLine("科目別平均点:");

            foreach (string wSubject in C_TargetSubjects) {
                var wEachSubjectRecords = this.GetScoreRecordsPerSubject(wSubject);

                double wEachSubjectAverage = wEachSubjectRecords.Any() ? wEachSubjectRecords.Average(x => x.Score) : 0;

                Console.WriteLine("{0}: {1:F2}", wSubject, wEachSubjectAverage);
            }
        }

        /// <summary>
        /// 合格した成績レコードを抽出する
        /// </summary>
        /// <returns>合格点以上の成績リスト</returns>
        private List<StudentScore> GetPassingRecords() => this.FScoreRecords.Where(x => x.Score >= C_PassingScore).ToList();

        /// <summary>
        /// 合格した成績レコードを表示する
        /// </summary>
        public void DisplayPassingRecords() {
            var wPassingStudents = this.GetPassingRecords();
            Console.WriteLine($"合格者 {wPassingStudents.Count} 名");
            foreach (var wStudent in wPassingStudents) {
                this.PrintScoreRecord(wStudent);
            }
        }

        /// <summary>
        /// ユーザーに指定された科目の成績レコードを抽出する
        /// </summary>
        /// <param name="vSubject">入力された科目名</param> 
        private List<StudentScore> GetScoreRecordsPerSubject(string vSubject) {
            return this.FScoreRecords.Where(x => x.Subject.Equals(vSubject, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        /// <summary>
        /// ユーザーに指定された科目の成績レコードを表示する
        /// </summary>
        /// <param name="vSubject">入力された科目名</param>
        public void DisplayScoreRecordsPerSubject(string vSubject) {
            if (string.IsNullOrWhiteSpace(vSubject)) {
                Console.WriteLine("エラー：科目名が指定されていません。");
                return;
            }

            Console.WriteLine($"{vSubject} の成績一覧");

            var wFilteredStudentsBySubject = this.GetScoreRecordsPerSubject(vSubject);

            if (!wFilteredStudentsBySubject.Any()) {
                Console.WriteLine($"エラー：{vSubject} の成績データが見つかりません。");
                return;
            }

            foreach (var wStudent in wFilteredStudentsBySubject) {
                this.PrintScoreRecord(wStudent);
            }
        }

        /// <summary>
        /// 成績リストに格納している全データをCSV形式で出力する
        /// </summary> 
        /// <param name="vOutputCsvFilePath">出力先CSVファイルのパス</param>
        public void ExportScoreRecordsToCsv(string vOutputCsvFilePath) {
            try {
                using (var wWriter = new StreamWriter(vOutputCsvFilePath, false, Encoding.UTF8)) {
                    wWriter.WriteLine("Name,Subject,Score");

                    foreach (var wStudent in this.FScoreRecords) {
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