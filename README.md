# 2FA
Generate's a One Time Password with c#.

Usage:
```cs
using _2FA;
using System;

namespace Example
{
    class Program
    {
        const string SECRET = "SECRET";
        
        static void Main(string[] args)
        {
            Console.WriteLine(new _2FAGenerator(SECRET).GenerateCode());
            Console.ReadLine();
        }
    }      
}
```
