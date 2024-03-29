﻿using System.Runtime.InteropServices;

namespace ECF.Utilities;

internal static class CommandLineUtilities
{
    [DllImport("shell32.dll", SetLastError = true)]
    private static extern IntPtr CommandLineToArgvW([MarshalAs(UnmanagedType.LPWStr)] string? lpCmdLine, out int pNumArgs);

    internal static string[] CommandLineToArgs(string? commandLine)
    {
        int argc;
        var argv = CommandLineToArgvW(commandLine, out argc);

        if (argv == IntPtr.Zero)
        {
            throw new System.ComponentModel.Win32Exception();
        }

        try
        {
            var args = new string[argc];
            for (int i = 0; i < args.Length; i++)
            {
                var p = Marshal.ReadIntPtr(argv, i * IntPtr.Size);
                args[i] = Marshal.PtrToStringUni(p)!;
            }

            return args;
        }
        finally
        {
            Marshal.FreeHGlobal(argv);
        }
    }
}
