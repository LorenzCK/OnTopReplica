using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace OnTopReplica.Native {
    static class InputMethods {

        [DllImport("user32.dll")]
        static extern short GetKeyState(VirtualKeyState nVirtKey);

        const int KeyToggled = 0x1;

        const int KeyPressed = 0x8000;

        public static bool IsKeyPressed(VirtualKeyState virtKey) {
            return (GetKeyState(virtKey) & KeyPressed) != 0;
        }

        public static bool IsKeyToggled(VirtualKeyState virtKey) {
            return (GetKeyState(virtKey) & KeyToggled) != 0;
        }

    }
}
