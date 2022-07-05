using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoClickTest
{
    public static class Keybord
    {
        [DllImport("user32.dll", EntryPoint = "keybd_event", SetLastError = true)]
        public static extern void keybd_event(
            byte bVk,    //虛擬鍵值
            byte bScan,// 一般爲0
            int dwFlags,  //這裏是整數類型  0 爲按下，2爲釋放
            int dwExtraInfo  //這裏是整數類型 一般情況下設成爲 0
        );

        private enum KeyEvent
        {
            按下 = 0,
            放開 = 2
        }

        /// <summary>
        /// 鍵盤輸入事件-按一下
        /// </summary>
        /// <param name="key">組合鍵</param>
        /// <remarks>
        /// + = shift
        /// ^ = ctrl
        /// % = alt
        /// </remarks>
        public static void Press(string key)
        {
            SendKeys.Send($"{{{key.ToUpper()}}}");
        }

        /// <summary>
        /// 鍵盤按住事件
        /// </summary>
        /// <param name="key">鍵值</param>
        /// <param name="ms">按住毫秒</param>
        public static void Hold(string key, int ms)
        {
            CancellationTokenSource s_cts = new CancellationTokenSource();
            try
            {
                s_cts.CancelAfter(ms);

                Task t = HoldDown(key);
                t.Start();
            }
            catch (OperationCanceledException)
            {}
            finally
            {
                s_cts.Dispose();
            }
            keybd_event((byte)Enum.Parse<Keys>(key), 0, (int)KeyEvent.放開, 0);
        }

        /// <summary>
        /// 持續送出按鍵事件的排程
        /// </summary>
        /// <param name="key">鍵值</param>
        /// <returns></returns>
        private static Task HoldDown(string key)
        {
            while (true)
            {
                SendKeys.Send($"{{{key.ToUpper()}}}");
            }
        }
    }
}
