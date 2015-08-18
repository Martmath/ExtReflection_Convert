using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Diagnostics;
public static class MProcess
{
    [Flags]
    public enum PAFs : uint
    {
        All = 0x001F0FFF,
        Terminate = 0x00000001,
        CreateThread = 0x00000002,
        VMOperation = 0x00000008,
        VMRead = 0x00000010,
        VMWrite = 0x00000020,
        DuplicateHandle = 0x00000040,
        CreateProcess = 0x000000080,
        SetQuota = 0x00000100,
        SetInformation = 0x00000200,
        QueryInformation = 0x00000400,
        QueryLimitedInformation = 0x00001000,
        Synchronize = 0x00100000,
        AllAccess = CreateThread | DuplicateHandle | QueryInformation | SetInformation | Terminate | VMOperation | VMRead | VMWrite | Synchronize,
        FullQuery = VMOperation | VMRead | VMWrite | QueryInformation
    }
    [DllImport("kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool IsWow64Process([In] IntPtr process, [Out] out bool wow64Process);
    [DllImport("user32")]
    public static extern IntPtr GetWindowThreadProcessId(IntPtr hWnd, out IntPtr lpdwProcessId);
    [DllImport("kernel32")]
    public static extern IntPtr OpenProcess(PAFs dwDesiredAccess, bool bInheritHandle, int dwProcessID);

    public static IntPtr GetProcessPointer(IntPtr hWnd)
    {
        IntPtr ProcessID;
        GetWindowThreadProcessId(hWnd, out ProcessID);
        return OpenProcess(PAFs.FullQuery, false, ProcessID.ToInt32());
    }

    public static bool IsWin64(IntPtr processorwin, params bool[] ItWin)
    {
        if ((ItWin.Length > 0) && ItWin[0]) processorwin = GetProcessPointer(processorwin);
        bool tt;
        bool yy = IsWow64Process(processorwin, out tt);
        if ((Environment.OSVersion.Version.Major > 5)
            || ((Environment.OSVersion.Version.Major == 5) && (Environment.OSVersion.Version.Minor >= 1)))
        {
            try
            {
                bool retVal;
                return !(IsWow64Process(processorwin, out retVal) && retVal);
            }
            catch
            { return false; }
        }
        return false; // not on 64-bit Windows
    }
}
