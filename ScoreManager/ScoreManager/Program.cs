using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoreManager {
    // Program「ユーザーとのやり取り」だけを担当。実際の処理はScoreManagerに任せる。
    public class Program {
        static void Main(string[] args) {
        }
    }

    // StudentScore「データを保持するだけ」のクラス。名前、科目、点数のデータを保持。コンストラクタを作成。
    public class  StudentScore {
        // 名前
        public string Name { get; private set; }
        
        // 科目
        public string Subject { get; private set; }

        // 点数
        public int Score { get; private set; }

        // コンストラクタ
        public StudentScore(string vName, string vSubject, int vScore) {
            this.Name = vName;
            this.Subject = vSubject;
            this.Score = vScore;
        }

    }

    // ScoreManager「データを扱う処理の集合」。CSV読み込み、表示、集計、出力などを担当。
    public class  ScoreManager {
        
    }
}
