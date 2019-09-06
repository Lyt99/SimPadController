using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimPadController.Enum
{

    // 类型为0，互斥
    public enum SimPadKeyNormal
    {
        None = 0,
        Esc = 41,
        F1 = 58,
        F2 = 59,
        F3 = 60,
        F4 = 61,
        F5 = 62,
        F6 = 63,
        F7 = 64,
        F8 = 65,
        F9 = 66,
        F10 = 67,
        F11 = 68,
        F12 = 69,
        PrtSc = 70,
        ScrLk = 71,
        PauseBreak = 72,
        WaveLine = 53,
        Num1 = 30,
        Num2 = 31,
        Num3 = 32,
        Num4 = 33,
        Num5 = 34,
        Num6 = 35,
        Num7 = 36,
        Num8 = 37,
        Num9 = 38,
        Num0 = 39,
        Bar = 45,
        Equal = 46,
        Bsp = 42,
        Ins = 73,
        Home = 74,
        PageUp = 75,
        NumPadNumLk = 83,
        NumPadSlash = 84,
        NumPadAsterisk = 85,
        NumPadBar = 86,
        Tab = 43,
        Q = 20,
        W = 26,
        E = 8,
        R = 21,
        T = 23,
        Y = 28,
        U = 24,
        I = 12,
        O = 18,
        P = 19,
        LeftParenthesis = 47,
        RightParenthesis = 48,
        BackSlant = 49,
        Del = 76,
        End = 77,
        PgDn = 78,
        NumPad7 = 95,
        NumPad8 = 96,
        NumPad9 = 97,
        NumPadAdd = 87,
        CapsLock = 57,
        A = 4,
        S = 22,
        D = 7,
        F = 9,
        G = 10,
        H = 11,
        J = 13,
        K = 14,
        L = 15,
        Semicolon = 51,
        QuotationMark = 52,
        Enter = 40,
        NumPad4 = 92,
        NumPad5 = 93,
        NumPad6 = 94,
        Z = 29,
        X = 27,
        C = 6,
        V = 25,
        B = 5,
        N = 17,
        M = 16,
        Comma = 54,
        Period = 55,
        QuestionMark = 56,
        UpArrow = 82,
        NumPad1 = 89,
        NumPad2 = 90,
        NumPad3 = 91,
        NumPadEnter = 88,
        Space = 44,
        Menu = 101,
        LeftArrow = 80,
        DownArrow = 81,
        RightArrow = 79,
        NumPad0 = 98,
        NumPadPeriod = 99
    }

    // 类型为1，取&运算(?) 我觉得应该是或运算
    public enum SimPadKeySpecial
    {
        None = 0,
        LeftShift = 2,
        RightShift = 32,
        LeftCtrl = 1,
        LeftWin = 8,
        LeftAlt = 4,
        RightAlt = 64,
        RightWin = 128,
        RightCtrl = 65536,
    }

    // 类型为2，鼠标
    public enum SimPadKeyMouse
    {
        None = 0,
        MouseLeft = 1,
        MouseMiddle = 4,
        MouseRight = 2
    }


}
