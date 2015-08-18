using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Diagnostics;
public static class SysHeader32//TestWindow
{
    [Flags]
    public enum MSG : uint
    {
        SETITEM = 0x120c, FIRST = 0x1200, GETITEMCOUNT = FIRST + 0,
        HITTEST = FIRST + 6, GETITEMRECT = FIRST + 7, GETITEMW = FIRST + 11,
        GETITEMA = FIRST + 3, ORDERTOINDEX = FIRST + 15, GETITEMDROPDOWNRECT = FIRST + 25,
        GETFOCUSEDITEM = FIRST + 27
    }
    [Flags]
    public enum SETGETMSG : uint
    {
        WIDTH = 0x0001, HEIGHT = WIDTH, TEXT = 0x0002,
        FORMAT = 0x0004, LPARAM = 0x0008, BITMAP = 0x0010,
        IMAGE = 0x0020, DI_SETITEM = 0x0040, ORDER = 0x0080,
        FILTER = 0x0100
    }
    /*[StructLayout(LayoutKind.Sequential)] //-Universal Elephant does not fly(
    public class MAINSTRUCT<U> : IGetSet
    {
        public uint mask;
        public int cxy;
        public uint pszText;
        public uint hbm;
        public int cchTextMax;
        public int fmt;
        public ulong lParam;
        public int iImage;
        public int iOrder;
        public uint type;
        public uint pvFilter;
        public uint state;
    } */

    [StructLayout(LayoutKind.Sequential)]
    public struct MAINSTRUCT64 : IGetSet
    {
        public uint mask;
        public int cxy;
        public ulong pszText;
        public ulong hbm;
        public int cchTextMax;
        public int fmt;
        public ulong lParam;
        public int iImage;
        public int iOrder;
        public uint type;
        public ulong pvFilter;
        public uint state;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MAINSTRUCT32 : IGetSet
    {
        public uint mask;
        public int cxy;
        public uint pszText;
        public uint hbm;
        public int cchTextMax;
        public int fmt;
        public ulong lParam;
        public int iImage;
        public int iOrder;
        public uint type;
        public uint pvFilter;
        public uint state;
    }
    public static int GetColumnCount(IntPtr Handle)
    {
        return Message.I.SM(Handle, (uint)MSG.GETITEMCOUNT, 0, IntPtr.Zero);
    }
    public static string GetItemString(IntPtr Handle, int N)
    {
        string CL_Name = MethodBase.GetCurrentMethod().DeclaringType.FullName + "+MAINSTRUCT";//`1";
        IntPtr ProcessPointer = MProcess.GetProcessPointer(Handle);
        if (MProcess.IsWin64(ProcessPointer)) CL_Name = CL_Name + "64"; else CL_Name = CL_Name + "32";
        Type myType = Type.GetType(CL_Name);
        /* myType = (MProcess.IsWin64(ProcessPointer)) ? myType.MakeGenericType(new Type[] { Type.GetType("System.UInt64") }) :
         myType.MakeGenericType(new Type[] { Type.GetType("System.UInt32") });*/
        object Data = Activator.CreateInstance(myType);
        IGetSet GS = Data as IGetSet;
        uint MaxT = 256; byte[] List_COL = new byte[MaxT];
        IntPtr ItemPointerT = Memory.VirtualAllocEx(ProcessPointer, IntPtr.Zero, MaxT, Memory.AllocationType.Commit, Memory.MemoryProtection.ReadWrite);
        GS.GetSet().SetFieldValue(new string[] { "mask", "cchTextMax", "pszText" }, (SETGETMSG.WIDTH | SETGETMSG.TEXT), MaxT, ItemPointerT);
        IntPtr ItemPointerD = Memory.WriteProcessMemory(ProcessPointer, Data);
        Message.P.SM(Handle, (uint)MSG.GETITEMA, N, ItemPointerD);
        bool bSuccess3 = Memory.ReadProcessMemory(ProcessPointer, ItemPointerT, (object)List_COL);
        Memory.VirtualFreeEx(ProcessPointer, ItemPointerT, 0, Memory.FreeType.Release);
        Memory.VirtualFreeEx(ProcessPointer, ItemPointerD, 0, Memory.FreeType.Release);
        return System.Text.Encoding.Default.GetString(List_COL).Trim('\0');
    }
}
