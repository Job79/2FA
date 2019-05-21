<p align="center">
  <b>2FA</b>
  <br/>
  <img src="https://img.shields.io/badge/License-MIT-green.svg">
  <img src="https://img.shields.io/badge/version-1.0.1-green.svg">
  <img src="https://img.shields.io/badge/build-passing-green.svg">
  <br/>
  <br/>
  <a>2FA library compatible with google authenticator and authy<a/>
  <br/><br/>
</p>

```cs
using System;
using TwoFactorAuthentication;

namespace Example
{
    class Program
    {
        static void Main(string[] args)
        {
            //Generate 2FA code(Client)
            var generator = new TwoFactor("SECRET");
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
            string secret = TwoFactor.GenerateSecret();
        }
    }        
}
```
