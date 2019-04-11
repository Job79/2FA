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
            var generator = new _2FAGenerator("SECRET");
            Console.WriteLine(generator.GenerateCode());
            
            //Get input from console.
            string code;
            do
                Console.Write("Enter a generated 2FA code to check: ");
            while ((code = Console.ReadLine()).Length != 6);

            //Check 2FA code(Server)
            Console.WriteLine(generator.ValidateCode(code)?"Code is valid.":"Code is invalid.");
            Console.ReadLine();
            
            //Generate new 2FA secret (static)
            string secret = _2FAGenerator.GenerateSecret();
        }
    }        
}
```
