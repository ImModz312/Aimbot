using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Windows;
using System.Threading;
using System.Windows.Input;

namespace ModTemplate
{
    public class MouseMover
    {
        public struct POINT
        {
            public int X;
            public int Y;
        }

        [DllImport("user32.dll")]
        public static extern bool SetCursorPos(int X, int Y);

        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(out POINT point);

        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll")]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);
        public static void MoveMouseSmoothly(float targetX, float targetY, float durationInSeconds)
        {
            const string gameWindowTitle = "ROTMGExalt";
            StringBuilder activeWindowTitle = new StringBuilder(256);
            IntPtr gameWindowHandle =
            if (GetForegroundWindow() == gameWindowHandle)
            {

            }
    }


            public static void MoveMouse(int x, int y)
        {
            SetCursorPos(x, y);
        }

        
        
        public static void MoveMouseSmoothly(float targetX, float targetY, float durationInSeconds)
        {
            POINT cursorPosition;
            if (GetCursorPos(out cursorPosition))
            {
                //Console.WriteLine($"Cursor X: {cursorPosition.X}");
                //Console.WriteLine($"Cursor Y: {cursorPosition.Y}");
            }
            int steps = (int)(durationInSeconds * 100); // Adjust the number of steps as needed
            float stepX = (targetX - cursorPosition.X) / steps;
            float stepY = (targetY - cursorPosition.Y) / steps;

            for (int i = 0; i < steps; i++)
            {
                float currentX = cursorPosition.X + stepX * i;
                float currentY = cursorPosition.Y + stepY * i;
                SetCursorPos((int)currentX, (int)currentY);
                Thread.Sleep(10); // Adjust the sleep time for desired speed
            }

            SetCursorPos((int)targetX, (int)targetY);
        }

        public static void MoveMouseSlowly(float targetX, float targetY, float durationInSeconds)
        {
            int steps = 100; // Number of steps to move the cursor
            int delay = (int)(durationInSeconds * 1000) / steps; // Calculate delay between steps

            for (int i = 1; i <= steps; i++)
            {
                float currentX = targetX * (i / (float)steps);
                float currentY = targetY * (i / (float)steps);

                SetCursorPos((int)currentX, (int)currentY);
                Thread.Sleep(delay);
            }

            SetCursorPos((int)targetX, (int)targetY);
        }

        [DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(Keys vKey);

        [DllImport("user32.dll")]
        private static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, IntPtr dwExtraInfo);

        private const uint KEYEVENTF_EXTENDEDKEY = 0x0001;
        private const uint KEYEVENTF_KEYUP = 0x0002;

        public static bool IsKeyDown(Keys key)
        {
            return (GetAsyncKeyState((Keys)key) & 0x8000) != 0;
        }
        public static void HoldKey(Keys key)
        {
            keybd_event((byte)key, 110, KEYEVENTF_EXTENDEDKEY, IntPtr.Zero);
        }
        public static void ReleaseKey(Keys key)
        {
            keybd_event((byte)key, 0, KEYEVENTF_KEYUP, IntPtr.Zero);
        }

        public static void PressKey(Keys key)
        {
            keybd_event((byte)key, 0, KEYEVENTF_EXTENDEDKEY, IntPtr.Zero);
            keybd_event((byte)key, 0, KEYEVENTF_KEYUP, IntPtr.Zero);
        }



    }

    public enum Keys : byte
    {
        // Function keys
        F1 = 0x70,
        F2 = 0x71,
        F3 = 0x72,
        F4 = 0x73,
        F5 = 0x74,
        F6 = 0x75,
        F7 = 0x76,
        F8 = 0x77,
        F9 = 0x78,
        F10 = 0x79,
        F11 = 0x7A,
        F12 = 0x7B,

        // Alphanumeric keys
        A = 0x41,
        B = 0x42,
        C = 0x43,
        D = 0x44,
        E = 0x45,
        F = 0x46,
        G = 0x47,
        H = 0x48,
        I = 0x49,
        J = 0x4A,
        K = 0x4B,
        L = 0x4C,
        M = 0x4D,
        N = 0x4E,
        O = 0x4F,
        P = 0x50,
        Q = 0x51,
        R = 0x52,
        S = 0x53,
        T = 0x54,
        U = 0x55,
        V = 0x56,
        W = 0x57,
        X = 0x58,
        Y = 0x59,
        Z = 0x5A,

        // Numeric keys
        D0 = 0x30,
        D1 = 0x31,
        D2 = 0x32,
        D3 = 0x33,
        D4 = 0x34,
        D5 = 0x35,
        D6 = 0x36,
        D7 = 0x37,
        D8 = 0x38,
        D9 = 0x39,

        // Special keys
        Space = 0x20,
        Enter = 0x0D,
        Esc = 0x1B,
        Tab = 0x09,
        Shift = 0x10,
        Ctrl = 0x11,
        Alt = 0x12,
        Windows = 0x5B,
        Backspace = 0x08,


        NumPad0 = 0x60,
        NumPad1 = 0x61,
        NumPad2 = 0x62,
        NumPad3 = 0x63,
        NumPad4 = 0x64,
        NumPad5 = 0x65,
        NumPad6 = 0x66,
        NumPad7 = 0x67,
        NumPad8 = 0x68,
        NumPad9 = 0x69
        // Add other key codes as needed...
    }
}
