using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace AutoClickTest
{
    public static class Keybord
    {
        [DllImport("user32.dll", EntryPoint = "keybd_event", SetLastError = true)]
        private static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);

        [DllImport("user32.dll")]
        private static extern uint MapVirtualKey(uint uCode, uint uMapType);

        private const int KEYEVENTF_KEYDOWN  = 0x0000;
        private const int KEYEVENTF_KEYUP    = 0x0002;
        private const int KEYEVENTF_SCANCODE = 0x0008;

        /// <summary>按一下（按下 + 放開）</summary>
        public static void Press(string keyName)
        {
            if (!Enum.TryParse(keyName, true, out Keys key)) return;
            byte vk = (byte)key;
            byte scan = (byte)MapVirtualKey(vk, 0);
            keybd_event(vk, scan, KEYEVENTF_KEYDOWN | KEYEVENTF_SCANCODE, 0);
            Thread.Sleep(20);
            keybd_event(vk, scan, KEYEVENTF_KEYUP | KEYEVENTF_SCANCODE, 0);
        }

        /// <summary>只按下（不放開），用於組合鍵</summary>
        public static void PressDown(string keyName)
        {
            if (!Enum.TryParse(keyName, true, out Keys key)) return;
            byte vk = (byte)key;
            byte scan = (byte)MapVirtualKey(vk, 0);
            keybd_event(vk, scan, KEYEVENTF_KEYDOWN | KEYEVENTF_SCANCODE, 0);
        }

        /// <summary>只放開，用於組合鍵</summary>
        public static void PressUp(string keyName)
        {
            if (!Enum.TryParse(keyName, true, out Keys key)) return;
            byte vk = (byte)key;
            byte scan = (byte)MapVirtualKey(vk, 0);
            keybd_event(vk, scan, KEYEVENTF_KEYUP | KEYEVENTF_SCANCODE, 0);
        }

        /// <summary>按住指定毫秒後放開</summary>
        public static void Hold(string keyName, int ms)
        {
            if (!Enum.TryParse(keyName, true, out Keys key)) return;
            byte vk = (byte)key;
            byte scan = (byte)MapVirtualKey(vk, 0);
            keybd_event(vk, scan, KEYEVENTF_KEYDOWN | KEYEVENTF_SCANCODE, 0);
            Thread.Sleep(ms);
            keybd_event(vk, scan, KEYEVENTF_KEYUP | KEYEVENTF_SCANCODE, 0);
        }
    }
}
