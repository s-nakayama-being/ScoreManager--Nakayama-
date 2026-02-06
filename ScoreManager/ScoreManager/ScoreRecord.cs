namespace ScoreManager {
    /// <summary>
    /// 生徒の名前、科目、点数のデータを保持するクラス
    /// </summary>
    public class ScoreRecord {
        /// <summary>
        /// 名前
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 科目
        /// </summary>
        public string Subject { get; private set; }

        /// <summary>
        /// 点数
        /// </summary>
        public int Score { get; private set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="vName">名前</param>
        /// <param name="vSubject">科目</param>
        /// <param name="vScore">点数</param>
        public ScoreRecord(string vName, string vSubject, int vScore) {
            this.Name = vName;
            this.Subject = vSubject;
            this.Score = vScore;
        }
    }
}
