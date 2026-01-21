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
        /// CSVファイルの読み込み
        /// </summary>
        /// <param name="vFilePath">CSVファイルのパス</param>
        public void ReadCsv(string vFilePath) {
            if (!File.Exists(vFilePath)) {
                Console.WriteLine(vFilePath + "が見つかりません：");
                return;
            }

            string[] wLines = File.ReadAllLines(vFilePath, Encoding.UTF8);

            const string C_ScoreRegexPattern = @"^\s*(?<name>[a-zA-Z\p{IsHiragana}\p{IsKatakana}\p{IsCJKUnifiedIdeographs}ー々]+)\s*," +
                  @"\s*(?<subject>[\p{IsHiragana}\p{IsKatakana}\p{IsCJKUnifiedIdeographs}]+)\s*," +
                  @"\s*(?<score>100|[1-9][0-9]?|0)\s*$";

            foreach (string wLine in wLines) {
                if (string.IsNullOrEmpty(wLine) || wLine.StartsWith("#")) {
                    continue;
                }

                bool wIsPattenMatch = Regex.IsMatch(wLine, C_ScoreRegexPattern);
                if (!wIsPattenMatch) {
                    Console.WriteLine($"不正な行: {wLine}");
                    continue;
                }

                StudentScore wNewStudent = CreateStudentFromLine(wLine);

                this.FStudents.Add(wNewStudent);
            }

            Console.WriteLine($"{this.FStudents.Count} 件のデータを読み込みました。");
        }

        /// <summary>
        /// CSVの1行を分解してStudentScoreオブジェクトを生成する
        /// </summary>
        /// <param name="vLine">カンマ区切りの文字列</param>
        /// <returns>作成されたStudentScoreオブジェクト</returns>
        public StudentScore CreateStudentFromLine (string vLine) {
            string[] wItems = vLine.Split(',');

            string wName = wItems[0].Trim();
            string wSubject = wItems[1].Trim();
            var wScore = int.Parse(wItems[2].Trim());

            return new StudentScore(wName, wSubject, wScore);
        }
    }
}
