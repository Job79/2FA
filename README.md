# 2FA
Generate's a One Time Password with c#,
or check if a One Time Password is valid for the secret.

Compatible with Google Authenticator and Authy.

Usage:
```cs
using _2FA;
using System;

namespace Example
{
    class Program
    {
        static void Main(string[] args)
        {
            //Generate 2FA code(Client)
            var _2FAGenerator = new _2FAGenerator("KQW4XT5ZUO7ERYRDUKU32FRMSSBDKFBU");
            Console.WriteLine(_2FAGenerator.GenerateCode());
            
            //Get input from console.
            string Code;
            do
                Console.Write("Enter a generated 2FA code to check: ");
            while ((Code = Console.ReadLine()).Length != 6);

            //Check 2FA code(Server)
            Console.WriteLine(_2FAGenerator.CheckCode(Code)?"Code is valid.":"Code is invalid.");
            Console.ReadLine();
        }
    }        
}
```
