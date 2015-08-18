﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Diagnostics;

public static class Message
{
    public static class P
    {

        [DllImport("user32.dll", EntryPoint = "SendMessage", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern IntPtr SM(IntPtr hWnd, uint wMsg, IntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll", EntryPoint = "SendMessage", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern IntPtr SM(IntPtr hWnd, uint wMsg, int wParam, IntPtr lParam);
        [DllImport("user32.dll", EntryPoint = "SendMessage", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern IntPtr SM(IntPtr hWnd, uint wMsg, uint wParam, IntPtr lParam);

        [DllImport("user32.dll", EntryPoint = "SendMessage", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern IntPtr SM(IntPtr hWnd, uint wMsg, IntPtr wParam, int lParam);
        [DllImport("user32.dll", EntryPoint = "SendMessage", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern IntPtr SM(IntPtr hWnd, uint wMsg, int wParam, int lParam);
        [DllImport("user32.dll", EntryPoint = "SendMessage", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern IntPtr SM(IntPtr hWnd, uint wMsg, uint wParam, int lParam);

        [DllImport("user32.dll", EntryPoint = "SendMessage", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern IntPtr SM(IntPtr hWnd, uint wMsg, IntPtr wParam, StringBuilder lParam);
        [DllImport("user32.dll", EntryPoint = "SendMessage", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern IntPtr SM(IntPtr hWnd, uint wMsg, int wParam, StringBuilder lParam);
        [DllImport("user32.dll", EntryPoint = "SendMessage", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern IntPtr SM(IntPtr hWnd, uint wMsg, uint wParam, StringBuilder lParam);

        [DllImport("user32.dll", EntryPoint = "SendMessage", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern IntPtr SM(IntPtr hWnd, uint wMsg, IntPtr wParam, bool lParam);
        [DllImport("user32.dll", EntryPoint = "SendMessage", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern IntPtr SM(IntPtr hWnd, uint wMsg, int wParam, bool lParam);
        [DllImport("user32.dll", EntryPoint = "SendMessage", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern IntPtr SM(IntPtr hWnd, uint wMsg, uint wParam, bool lParam);

        [DllImport("user32.dll", EntryPoint = "SendMessage", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern IntPtr SM(IntPtr hWnd, uint wMsg, IntPtr wParam, ref RECT R);
        [DllImport("user32.dll", EntryPoint = "SendMessage", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern IntPtr SM(IntPtr hWnd, uint wMsg, int wParam, ref RECT R);
        [DllImport("user32.dll", EntryPoint = "SendMessage", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern IntPtr SM(IntPtr hWnd, uint wMsg, uint wParam, ref RECT R);

        [DllImport("user32.dll", EntryPoint = "PostMessage", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern IntPtr PM(IntPtr hWnd, uint wMsg, IntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll", EntryPoint = "PostMessage", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern IntPtr PM(IntPtr hWnd, uint wMsg, int wParam, IntPtr lParam);
        [DllImport("user32.dll", EntryPoint = "PostMessage", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern IntPtr PM(IntPtr hWnd, uint wMsg, uint wParam, IntPtr lParam);

        [DllImport("user32.dll", EntryPoint = "PostMessage", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern IntPtr PM(IntPtr hWnd, uint wMsg, IntPtr wParam, int lParam);
        [DllImport("user32.dll", EntryPoint = "PostMessage", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern IntPtr PM(IntPtr hWnd, uint wMsg, int wParam, int lParam);
        [DllImport("user32.dll", EntryPoint = "PostMessage", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern IntPtr PM(IntPtr hWnd, uint wMsg, uint wParam, int lParam);

        [DllImport("user32.dll", EntryPoint = "PostMessage", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern IntPtr PM(IntPtr hWnd, uint wMsg, IntPtr wParam, StringBuilder lParam);
        [DllImport("user32.dll", EntryPoint = "PostMessage", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern IntPtr PM(IntPtr hWnd, uint wMsg, int wParam, StringBuilder lParam);
        [DllImport("user32.dll", EntryPoint = "PostMessage", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern IntPtr PM(IntPtr hWnd, uint wMsg, uint wParam, StringBuilder lParam);

        [DllImport("user32.dll", EntryPoint = "PostMessage", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern IntPtr PM(IntPtr hWnd, uint wMsg, IntPtr wParam, bool lParam);
        [DllImport("user32.dll", EntryPoint = "PostMessage", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern IntPtr PM(IntPtr hWnd, uint wMsg, int wParam, bool lParam);
        [DllImport("user32.dll", EntryPoint = "PostMessage", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern IntPtr PM(IntPtr hWnd, uint wMsg, uint wParam, bool lParam);

        [DllImport("user32.dll", EntryPoint = "PostMessage", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern IntPtr PM(IntPtr hWnd, uint wMsg, IntPtr wParam, ref RECT R);
        [DllImport("user32.dll", EntryPoint = "PostMessage", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern IntPtr PM(IntPtr hWnd, uint wMsg, int wParam, ref RECT R);
        [DllImport("user32.dll", EntryPoint = "PostMessage", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern IntPtr PM(IntPtr hWnd, uint wMsg, uint wParam, ref RECT R);
    }
    public static class I
    {

        [DllImport("user32.dll", EntryPoint = "SendMessage", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern int SM(IntPtr hWnd, uint wMsg, IntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll", EntryPoint = "SendMessage", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern int SM(IntPtr hWnd, uint wMsg, int wParam, IntPtr lParam);
        [DllImport("user32.dll", EntryPoint = "SendMessage", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern int SM(IntPtr hWnd, uint wMsg, uint wParam, IntPtr lParam);

        [DllImport("user32.dll", EntryPoint = "SendMessage", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern int SM(IntPtr hWnd, uint wMsg, IntPtr wParam, int lParam);
        [DllImport("user32.dll", EntryPoint = "SendMessage", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern int SM(IntPtr hWnd, uint wMsg, int wParam, int lParam);
        [DllImport("user32.dll", EntryPoint = "SendMessage", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern int SM(IntPtr hWnd, uint wMsg, uint wParam, int lParam);

        [DllImport("user32.dll", EntryPoint = "SendMessage", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern int SM(IntPtr hWnd, uint wMsg, IntPtr wParam, StringBuilder lParam);
        [DllImport("user32.dll", EntryPoint = "SendMessage", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern int SM(IntPtr hWnd, uint wMsg, int wParam, StringBuilder lParam);
        [DllImport("user32.dll", EntryPoint = "SendMessage", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern int SM(IntPtr hWnd, uint wMsg, uint wParam, StringBuilder lParam);

        [DllImport("user32.dll", EntryPoint = "SendMessage", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern int SM(IntPtr hWnd, uint wMsg, IntPtr wParam, bool lParam);
        [DllImport("user32.dll", EntryPoint = "SendMessage", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern int SM(IntPtr hWnd, uint wMsg, int wParam, bool lParam);
        [DllImport("user32.dll", EntryPoint = "SendMessage", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern int SM(IntPtr hWnd, uint wMsg, uint wParam, bool lParam);

        [DllImport("user32.dll", EntryPoint = "SendMessage", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern int SM(IntPtr hWnd, uint wMsg, IntPtr wParam, ref RECT R);
        [DllImport("user32.dll", EntryPoint = "SendMessage", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern int SM(IntPtr hWnd, uint wMsg, int wParam, ref RECT R);
        [DllImport("user32.dll", EntryPoint = "SendMessage", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern int SM(IntPtr hWnd, uint wMsg, uint wParam, ref RECT R);

        [DllImport("user32.dll", EntryPoint = "PostMessage", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern int PM(IntPtr hWnd, uint wMsg, IntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll", EntryPoint = "PostMessage", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern int PM(IntPtr hWnd, uint wMsg, int wParam, IntPtr lParam);
        [DllImport("user32.dll", EntryPoint = "PostMessage", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern int PM(IntPtr hWnd, uint wMsg, uint wParam, IntPtr lParam);

        [DllImport("user32.dll", EntryPoint = "PostMessage", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern int PM(IntPtr hWnd, uint wMsg, IntPtr wParam, int lParam);
        [DllImport("user32.dll", EntryPoint = "PostMessage", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern int PM(IntPtr hWnd, uint wMsg, int wParam, int lParam);
        [DllImport("user32.dll", EntryPoint = "PostMessage", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern int PM(IntPtr hWnd, uint wMsg, uint wParam, int lParam);

        [DllImport("user32.dll", EntryPoint = "PostMessage", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern int PM(IntPtr hWnd, uint wMsg, IntPtr wParam, StringBuilder lParam);
        [DllImport("user32.dll", EntryPoint = "PostMessage", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern int PM(IntPtr hWnd, uint wMsg, int wParam, StringBuilder lParam);
        [DllImport("user32.dll", EntryPoint = "PostMessage", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern int PM(IntPtr hWnd, uint wMsg, uint wParam, StringBuilder lParam);

        [DllImport("user32.dll", EntryPoint = "PostMessage", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern int PM(IntPtr hWnd, uint wMsg, IntPtr wParam, bool lParam);
        [DllImport("user32.dll", EntryPoint = "PostMessage", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern int PM(IntPtr hWnd, uint wMsg, int wParam, bool lParam);
        [DllImport("user32.dll", EntryPoint = "PostMessage", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern int PM(IntPtr hWnd, uint wMsg, uint wParam, bool lParam);

        [DllImport("user32.dll", EntryPoint = "PostMessage", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern int PM(IntPtr hWnd, uint wMsg, IntPtr wParam, ref RECT R);
        [DllImport("user32.dll", EntryPoint = "PostMessage", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern int PM(IntPtr hWnd, uint wMsg, int wParam, ref RECT R);
        [DllImport("user32.dll", EntryPoint = "PostMessage", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern int PM(IntPtr hWnd, uint wMsg, uint wParam, ref RECT R);
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct RECT//"Stole" as is
    {
        public int Left, Top, Right, Bottom;

        public RECT(int left, int top, int right, int bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }
        public RECT(System.Drawing.Rectangle r) : this(r.Left, r.Top, r.Right, r.Bottom) { }
        public int X
        {
            get { return Left; }
            set { Right -= (Left - value); Left = value; }
        }
        public int Y
        {
            get { return Top; }
            set { Bottom -= (Top - value); Top = value; }
        }
        public int Height
        {
            get { return Bottom - Top; }
            set { Bottom = value + Top; }
        }

        public int Width
        {
            get { return Right - Left; }
            set { Right = value + Left; }
        }
        public System.Drawing.Point Location
        {
            get { return new System.Drawing.Point(Left, Top); }
            set { X = value.X; Y = value.Y; }
        }
        public System.Drawing.Size Size
        {
            get { return new System.Drawing.Size(Width, Height); }
            set { Width = value.Width; Height = value.Height; }
        }
        public static implicit operator System.Drawing.Rectangle(RECT r)
        {
            return new System.Drawing.Rectangle(r.Left, r.Top, r.Width, r.Height);
        }
        public static implicit operator RECT(System.Drawing.Rectangle r)
        {
            return new RECT(r);
        }
        public static bool operator ==(RECT r1, RECT r2)
        {
            return r1.Equals(r2);
        }

        public static bool operator !=(RECT r1, RECT r2)
        {
            return !r1.Equals(r2);
        }

        public bool Equals(RECT r)
        {
            return r.Left == Left && r.Top == Top && r.Right == Right && r.Bottom == Bottom;
        }

        public override bool Equals(object obj)
        {
            if (obj is RECT)
                return Equals((RECT)obj);
            else if (obj is System.Drawing.Rectangle)
                return Equals(new RECT((System.Drawing.Rectangle)obj));
            return false;
        }

        public override int GetHashCode()
        {
            return ((System.Drawing.Rectangle)this).GetHashCode();
        }

        public override string ToString()
        {
            return string.Format(System.Globalization.CultureInfo.CurrentCulture, "{{Left={0},Top={1},Right={2},Bottom={3}}}", Left, Top, Right, Bottom);
        }
    }
}