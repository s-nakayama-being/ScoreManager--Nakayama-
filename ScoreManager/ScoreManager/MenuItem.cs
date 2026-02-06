using System;

namespace ScoreManager {
    /// <summary>
    /// メニュー項目を定義するクラス
    /// </summary>
    public class MenuItem {
        /// <summary>
        /// メニューId
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// メニュー説明文
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// メニュー選択時のアクション
        /// </summary>
        public Action Action { get; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="vId">メニューId</param>
        /// <param name="vDescription">メニュー説明文</param>
        /// <param name="vAction">メニュー選択時のアクション</param>
        public MenuItem(int vId, string vDescription, Action vAction) {
            this.Id = vId;
            this.Description = vDescription;
            this.Action = vAction;
        }
    }
}