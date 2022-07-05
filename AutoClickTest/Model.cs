using System;
using System.Collections.Generic;
using System.Text;

namespace AutoClickTest
{
    /// <summary>
    /// 動作基底
    /// </summary>
    public class Action
    {
        /// <summary>
        /// 編號
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 工具說明
        /// </summary>
        public string Tool_Name { get; set; }

        /// <summary>
        /// 動作說明
        /// </summary>
        public string Action_Desc { get; set; }

        /// <summary>
        /// 延遲毫秒
        /// </summary>
        public int Delay_MS { get; set; }
    }

    /// <summary>
    /// 鍵盤動作
    /// </summary>
    public class KeyCodeAction : Action
    {
        /// <summary>
        /// 鍵盤事件參數
        /// </summary>
        public string KeyCode { get; set; }
    }

    /// <summary>
    /// 滑鼠動作
    /// </summary>
    public class MouseAction : Action
    {
        /// <summary>
        /// 滑鼠事件 X 參數
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// 滑鼠事件 Y 參數
        /// </summary>
        public int Y { get; set; }
    }

    public enum ActionNameId {
        鍵盤按一下 = 0,
        滑鼠點一下左鍵 = 1,
        滑鼠點兩下左鍵 = 2,
        滑鼠點一下右鍵 = 3,
        滑鼠點一下中鍵 = 4,
        滑鼠前滾 = 5,
        滑鼠後滾 = 6
    }
}
