using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Diagnostics;

public static class WApi
{
    private static class InputData
    {
        public static string BName;
        public static string EName;
        public static string WClass;
        public static string PName;
        public static IntPtr Result;
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct WINDOWINFO
    {
        public uint cbSize;
        public Message.RECT rcWindow;
        public Message.RECT rcClient;
        public uint dwStyle;
        public uint dwExStyle;
        public uint dwWindowStatus;
        public uint cxWindowBorders;
        public uint cyWindowBorders;
        public ushort atomWindowType;
        public ushort wCreatorVersion;
        public WINDOWINFO(Boolean filler)
            : this()
        {
            cbSize = (UInt32)(Marshal.SizeOf(typeof(WINDOWINFO)));
        }
    }
    [DllImport("user32.dll", SetLastError = true)]
    public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);
    [DllImport("user32.dll", SetLastError = true)]
    public static extern bool EnumThreadWindows(int dwThreadId, EnumThreadDelegate lpfn, IntPtr lParam);
    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool IsWindowVisible(IntPtr handle);
    [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    public static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);
    [return: MarshalAs(UnmanagedType.Bool)]
    [DllImport("user32.dll", SetLastError = true)]
    public static extern bool GetWindowInfo(IntPtr hwnd, ref WINDOWINFO pwi);
    [DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
    public static extern IntPtr GetParent(IntPtr hWnd);
    [DllImport("user32.dll", EntryPoint = "FindWindowEx", CharSet = CharSet.Auto)]
    public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);
    [DllImport("user32.dll")]
    public static extern int GetDlgCtrlID(IntPtr hWnd);

    public const int WM_GETTEXT = 0x0D;
    public const int WM_GETTEXTLENGTH = 0x0E;

    private static int Delta = 0;
    private static IntPtr Res = IntPtr.Zero;
    private static int N = 0;
    private static WINDOWINFO d = new WINDOWINFO(true);
    private static List<MyWindowLight> WL = new List<MyWindowLight>();

    public delegate bool EnumThreadDelegate(IntPtr hWnd, IntPtr lParam);

    public static int GetProcessIDNum(string N)
    {
        return Process.GetProcessesByName(N).Length;
    }
    public static IntPtr GetProcessID(string N, int n)
    {
        if (Process.GetProcessesByName(N).Length > 0)
        {
            return (IntPtr)Process.GetProcessesByName(N)[n].Id;
        }
        else return IntPtr.Zero;
    }
    public static string GetTextWin(IntPtr H)
    {
        StringBuilder Buff = new StringBuilder(GetTextLength(H) + 1);
        int j = Message.I.SM(H, WM_GETTEXT, Buff.Capacity, Buff);
        return Buff.ToString().Trim();
    }
    public static int GetTextLength(IntPtr H)
    {
        return Message.I.SM(H, WM_GETTEXTLENGTH, 0, 0);
    }
    public static string GetClassName(IntPtr H)
    {
        StringBuilder Buff = new StringBuilder(256);
        int j = GetClassName(H, Buff, Buff.Capacity);
        return Buff.ToString().Trim();
    }
    private static bool ThreadWindows(IntPtr handle, IntPtr param)
    {
        string TW = GetTextWin(handle);
        if ((IsWindowVisible(handle)) && (TW.Length >= InputData.BName.Length) && (TW.Length >= InputData.EName.Length))
        {
            //if (TW.Substring(0, InputData.BName.Length) == InputData.BName) {    }
            if ((GetClassName(handle).Contains(InputData.WClass)) && (TW.Substring(0, InputData.BName.Length) == InputData.BName) && (TW.Substring(TW.Length - InputData.EName.Length) == InputData.EName))
            {
                InputData.Result = handle;
                return false;
            }
        }
        return true;
    }
    private static bool ThreadWindows1(IntPtr handle, IntPtr param)
    {
        string TW = GetTextWin(handle);
        if ((IsWindowVisible(handle)) && (TW.Length >= InputData.BName.Length))
        {
            if ((GetClassName(handle).Contains(InputData.WClass)) && (TW.Substring(0, InputData.BName.Length) == InputData.BName)) { InputData.Result = handle; return false; }
        }
        return true;
    }
    private static bool ThreadWindows2(IntPtr handle, IntPtr param)
    {
        string TW = GetTextWin(handle);
        if ((IsWindowVisible(handle)) && (TW.Length >= InputData.EName.Length))
        {
            if ((GetClassName(handle).Contains(InputData.WClass)) && (TW.Substring(TW.Length - InputData.EName.Length) == InputData.EName)) { InputData.Result = handle; return false; }
        }
        return true;
    }

    public static IntPtr GetMainWin(string Prc, string Cl, string N, string n)
    {
        uint pid = 0; InputData.Result = IntPtr.Zero; Process p;
        for (int i = 0; i < WApi.GetProcessIDNum(Prc); i++)
        {
            GetWindowThreadProcessId(GetProcessID(Prc, i), out pid);
            var callback = new EnumThreadDelegate(WApi.ThreadWindows);
            p = Process.GetProcessById((int)pid);
            InputData.PName = Prc; InputData.WClass = Cl; InputData.BName = N; InputData.EName = n;
            if ((N != "") && (n != ""))
            {
                foreach (ProcessThread thread in Process.GetProcessById((int)pid).Threads)
                {
                    EnumThreadWindows(thread.Id, callback, IntPtr.Zero);
                    if (InputData.Result != IntPtr.Zero) return InputData.Result;
                }
            }
            if (N != "")
            {
                callback = new EnumThreadDelegate(WApi.ThreadWindows1);
                foreach (ProcessThread thread in Process.GetProcessById((int)pid).Threads)
                {
                    EnumThreadWindows(thread.Id, callback, IntPtr.Zero);
                    if (InputData.Result != IntPtr.Zero) return InputData.Result;
                }
            }
            if (n != "")
            {
                callback = new EnumThreadDelegate(WApi.ThreadWindows2);
                foreach (ProcessThread thread in Process.GetProcessById((int)pid).Threads)
                {
                    EnumThreadWindows(thread.Id, callback, IntPtr.Zero);
                    if (InputData.Result != IntPtr.Zero) return InputData.Result;
                }
            }
        }
        return InputData.Result;
    }

    private class MyWindowLight
    {
        public IntPtr Win;
        public string TextL;
        public int X;
        public int Y;
        public int Num;
        public int MainNum;
        public string ParentTextL;
        public MyWindowLight(IntPtr W)
        { InitME(W); }
        public MyWindowLight(IntPtr W, int NMain, int N)
        { InitME(W, NMain, N); }
        public MyWindowLight()
        { }
        public void InitME(IntPtr W)
        {
            Win = W;
            TextL = GetTextWinL(W);
            GetWindowInfo(W, ref d);
            X = d.rcWindow.Left;
            Y = d.rcWindow.Top;
            ParentTextL = GetTextWinL(GetParent(W));
        }
        public void InitME(IntPtr W, int NMain, int N)
        {
            Num = N;
            MainNum = NMain;
            InitME(W);
        }
    }
    public static string GetTextWinL(IntPtr H)
    {
        string s = GetTextWin(H);
        if (s.Length > 50) s = s.Substring(0, 50) + "...";
        return s;
    }

    private static void getListWindow(IntPtr Main, int NM, string Class, int ID, string ParentClass, int ParentID)
    {
        IntPtr Active = FindWindowEx(Main, IntPtr.Zero, null, null);
        while (Active != IntPtr.Zero)
        {
            N = N + 1;
            IntPtr P = GetParent(Active);
            if ((GetClassName(Active) == Class) && (GetDlgCtrlID(Active) == ID) &&
             (GetClassName(P) == ParentClass) && (GetDlgCtrlID(P) == ParentID))
            {
                if ((N - NM) == Delta) WL.Add(new MyWindowLight(Active, NM, N));
            }


            getListWindow(Active, N, Class, ID, ParentClass, ParentID);
            Active = FindWindowEx(Main, Active, null, null);
        }
    }
    private static void getListWindow(IntPtr Main, int NM, string Class, string ParentTextL, string ParentClass)
    {
        IntPtr Active = FindWindowEx(Main, IntPtr.Zero, null, null);
        while (Active != IntPtr.Zero)
        {
            N = N + 1;
            IntPtr P = GetParent(Active);
            if ((GetClassName(Active) == Class) && (GetTextWinL(P) == ParentTextL) &&
             (GetClassName(P) == ParentClass))
            {
                if ((N - NM) == Delta) WL.Add(new MyWindowLight(Active, NM, N));
            }
            getListWindow(Active, N, Class, ParentTextL, ParentClass);
            Active = FindWindowEx(Main, Active, null, null);
        }
    }
    private static void getWindowbyNum(IntPtr Main, int n)
    {
        IntPtr Active = FindWindowEx(Main, IntPtr.Zero, null, null);
        while (Active != IntPtr.Zero)
        {
            N = N + 1;
            if (N == n)
            {
                Res = Active;
                return;
            }
            getWindowbyNum(Active, n);
            if (N == n) { return; }
            Active = FindWindowEx(Main, Active, null, null);
        }
    }
    public static IntPtr GetChildWindow(IntPtr Main, int n, string Class, int ID,
        string TextL, int X, int Y, string ParentClass, int ParentID, string ParentTextL, int NParent)
    {
        Res = IntPtr.Zero;
        Delta = n - NParent;
        if (n == 0) return Main;
        N = 0;
        getWindowbyNum(Main, n);
        if ((GetClassName(Res) != Class) || (GetDlgCtrlID(Res) != ID))
        {
            WL.Clear();
            N = 0;
            getListWindow(Main, n, Class, ID, ParentClass, ParentID);
            if (WL.Count == 0) { N = 0; getListWindow(Main, n, Class, ParentTextL, ParentClass); N = 0; }
            if (WL.Count == 0) return Res;
            for (int i = 0; i < WL.Count; i++)
            {
                if (WL[i].TextL == TextL) return WL[i].Win;
            }
            GetWindowInfo(Main, ref d);
            int x = d.rcWindow.Left;
            int y = d.rcWindow.Top;
            for (int i = 0; i < WL.Count; i++)
            {
                if ((WL[i].X - x == X) && (WL[i].Y - y == Y)) return WL[i].Win;
            }
            for (int i = 0; i < WL.Count; i++)
            {
                if (WL[i].ParentTextL == ParentTextL) return WL[i].Win;
            }
            for (int i = 0; i < WL.Count; i++)
            {
                if (GetFirstWord(WL[i].ParentTextL) == GetFirstWord(ParentTextL)) return WL[i].Win;
            }
            for (int i = 0; i < WL.Count; i++)
            {
                if (GetLastWord(WL[i].ParentTextL) == GetLastWord(ParentTextL)) return WL[i].Win;
            }
        }
        return Res;
    }

    private static string GetFirstWord(string S)
    {
        for (int i = 0; i < S.Length; i++)
            if (IsDelimB(S[i])) return S.Substring(0, i);
        return S;
    }
    private static string GetLastWord(string S)
    {
        for (int i = S.Length - 1; i > -1; i--)
            if (IsDelimB(S[i])) return S.Substring(i + 1);
        return S;
    }
    private static bool IsDelimB(string s)
    {
        return ((s == " ") || (s == '\u0022'.ToString()) || (s == '\u0027'.ToString()) || (s == "-") || (s == "_") || (s == "(") || (s == "{") || (s == "[") || (s == "<")
            || (s == "=") || (s == "~") || (s == "`") || (s == "!") || (s == "?") || (s == ".") || (s == ",") || (s == "/") || (s == '\u005C'.ToString())
            || (s == ":") || (s == ";"));
    }
    private static bool IsDelimB(char s)
    {
        return ((s == ' ') || (s == '\u0022') || (s == '\u0027') || (s == '-') || (s == '_') || (s == '(') || (s == '{') || (s == '[') || (s == '<')
            || (s == '=') || (s == '~') || (s == '`') || (s == '!') || (s == '?') || (s == '.') || (s == ',') || (s == '/') || (s == '\u005C')
            || (s == ':') || (s == ';'));
    }
}

