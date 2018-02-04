using System;
using System.Runtime.InteropServices;

namespace GVV
{
	/// <summary>
	/// Win32 PInvoke declarations
	/// </summary>
	internal class Win32 
	{
		[DllImport("User32.dll")]
		public static extern int GetUpdateRect( IntPtr hwnd, ref RECT rect, bool erase );

		[DllImport("User32.dll", SetLastError = true)]
		public static extern bool GetWindowRect( IntPtr handle, ref RECT rect );

		[DllImport("User32.dll")]
		public static extern IntPtr BeginPaint( IntPtr hWnd, ref PAINTSTRUCT paintStruct );

		[DllImport("User32.dll")]
		public static extern bool EndPaint( IntPtr hWnd, ref PAINTSTRUCT paintStruct );

		[StructLayout( LayoutKind.Sequential )]
			public struct RECT 
		{
			public int left;
			public int top;
			public int right;
			public int bottom;
		}

		[StructLayout( LayoutKind.Sequential) ]
			public struct PAINTSTRUCT 
		{
			public IntPtr hdc;
			public int fErase;
			public RECT rcPaint;
			public int fRestore;
			public int fIncUpdate;
			public int Reserved1;
			public int Reserved2;
			public int Reserved3;
			public int Reserved4;
			public int Reserved5;
			public int Reserved6;
			public int Reserved7;
			public int Reserved8;
		}
	}
}
