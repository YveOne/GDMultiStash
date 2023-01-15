using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

public static partial class Native
{
    public static partial class Keyboard
    {

        public class KeyState
        {
            public bool Down = false;
            public bool Toggled = false;
        }

        public static KeyState GetKeyState(Keys key)
        {
            KeyState state = new KeyState();

            short retVal = Native.GetKeyState((int)key);

            //If the high-order bit is 1, the key is down
            //otherwise, it is up.
            if ((retVal & 0x8000) == 0x8000) state.Down = true;

            //If the low-order bit is 1, the key is toggled.
            if ((retVal & 1) == 1) state.Toggled = true;

            return state;
        }

        public static uint KeyToScanCode(Keys key)
        {
            return MapVirtualKeyEx((uint)key, MAPVK_VK_TO_VSC_EX, GetKeyboardLayout(0));
        }

        public static uint ScanCode2LParam(uint sc, bool extended)
        {
            sc = sc << 16;
            if (extended) sc |= 0x1000000;
            return sc;
        }

        public static string GetKeyName(long lParam)
        {
            var sb = new StringBuilder(256);
            GetKeyNameText((int)lParam, sb, 256);
            return sb.ToString();
        }

        public static void SendKeyboardInput(ushort c, Native.KeyEventF f)
        {
            Input inp = new Input
            {
                type = InputType.Keyboard,
                u = new InputUnion
                {
                    ki = new KeyboardInput
                    {
                        wVk = 0,
                        wScan = c,
                        dwFlags = (uint)f
                    }
                }
            };
            SendInput(1, new Input[] { inp }, Marshal.SizeOf(typeof(Input)));
        }

        public static void SendKey(uint vsc)
        {
            SendKeyDown(vsc);
            System.Threading.Thread.Sleep(100);
            Application.DoEvents();
            SendKeyUp(vsc);
        }

        public static void SendKeyDown(uint vsc)
        {
            SendKeyboardInput((ushort)vsc, KeyEventF.Scancode | KeyEventF.KeyDown);
        }

        public static void SendKeyUp(uint vsc)
        {
            SendKeyboardInput((ushort)vsc, KeyEventF.Scancode | KeyEventF.KeyUp);
        }

    }
}
