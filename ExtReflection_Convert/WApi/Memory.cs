using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Diagnostics;

public static class Memory
{
    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool VirtualFreeEx(IntPtr hProcess, IntPtr lpAddress, uint dwSize, FreeType dwFreeType);

    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [In] object lpBuffer, uint dwSize, out int NBytes);
    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [In] IntPtr lpBuffer, uint nSize, out int NBytes);
    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [In] StringBuilder lpBuffer, uint nSize, out int NBytes);

    [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
    public static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, uint dwSize, AllocationType flAllocationType, MemoryProtection flProtect);
    [Flags]
    public enum AllocationType : uint
    {
        Commit = 0x1000,
        Reserve = 0x2000,
        Decommit = 0x4000,
        Release = 0x8000,
        Reset = 0x80000,
        Physical = 0x400000,
        TopDown = 0x100000,
        WriteWatch = 0x200000,
        LargePages = 0x20000000
    }
    [Flags]
    public enum MemoryProtection : uint
    {
        Execute = 0x10,
        ExecuteRead = 0x20,
        ExecuteReadWrite = 0x40,
        ExecuteWriteCopy = 0x80,
        NoAccess = 0x01,
        ReadOnly = 0x02,
        ReadWrite = 0x04,
        WriteCopy = 0x08,
        GuardModifierflag = 0x100,
        NoCacheModifierflag = 0x200,
        WriteCombineModifierflag = 0x400
    }
    public static IntPtr WriteProcessMemory(IntPtr hProcess, object O)
    {
        int t; uint MaxO = Size(O);
        IntPtr ItemPointerD = Memory.VirtualAllocEx(hProcess, IntPtr.Zero, MaxO, Memory.AllocationType.Commit, Memory.MemoryProtection.ReadWrite);
        bool s = WriteProcessMemory(hProcess, ItemPointerD, O, MaxO);
        if (s) return ItemPointerD; else return IntPtr.Zero;
    }
    [Flags]
    public enum FreeType : uint
    {
        Decommit = 0x4000,
        Release = 0x8000,
    }

    public static uint Size(object lpBuffer)
    {
        uint S = 0;
        try { S = (uint)Marshal.SizeOf(lpBuffer.GetType()); }
        catch (Exception e)
        {
            try
            {
                S = Convert.ToUInt32(lpBuffer.GetType().GetProperty("Length").GetValue(lpBuffer, null));
            }
            catch (Exception e2) { return S; }
        }
        return S;
    }

    public static bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, object lpBuffer, uint Size)
    {
        int t;
        IntPtr P = IntPtr.Zero;
        if (lpBuffer.GetType().Name == "StringBuilder")
            return WriteProcessMemory(hProcess, lpBaseAddress, (StringBuilder)lpBuffer, Size, out t);
        try
        {
            P = GCHandle.Alloc(lpBuffer, GCHandleType.Pinned).AddrOfPinnedObject();
            return WriteProcessMemory(hProcess, lpBaseAddress, P, Size, out t);
        }
        catch (Exception e2)
        {
            return WriteProcessMemory(hProcess, lpBaseAddress, lpBuffer, Size, out t);
        }
    }
    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [Out] StringBuilder lpBuffer, uint nSize, out int NBytes);
    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [Out] IntPtr lpBuffer, uint nSize, out int NBytes);
    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [Out] object lpBuffer, uint dwSize, out int NBytes);

    public static bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, object lpBuffer)
    {
        int t; uint S = Size(lpBuffer);
        if (S == 0) return false;
        IntPtr P = IntPtr.Zero;
        if (lpBuffer.GetType().Name == "StringBuilder")
            return ReadProcessMemory(hProcess, lpBaseAddress, (StringBuilder)lpBuffer, S, out t);
        try
        {
            P = GCHandle.Alloc(lpBuffer, GCHandleType.Pinned).AddrOfPinnedObject();
            return ReadProcessMemory(hProcess, lpBaseAddress, P, S, out t);
        }
        catch (Exception e2)
        {
            return ReadProcessMemory(hProcess, lpBaseAddress, lpBuffer, S, out t);
        }
    }
}
