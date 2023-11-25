using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static partial class Native
{
    public static int HIWORD(int n)
    {
        return (n >> 16) & 0xffff;
    }

    public static int HIWORD(IntPtr n)
    {
        return HIWORD(unchecked((int)(long)n));
    }

    public static int LOWORD(int n)
    {
        return n & 0xffff;
    }

    public static int LOWORD(IntPtr n)
    {
        return LOWORD(unchecked((int)(long)n));
    }
}
