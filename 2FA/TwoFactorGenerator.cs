/* TwoFactor
 * 
 * This program is free software. It comes without any warranty, to
 * the extent permitted by applicable law. You can redistribute it
 * and/or modify it under the terms of the Do What The Fuck You Want
 * To Public License, Version 2.
 */

using System;
using System.Linq;
using System.Security.Cryptography;

namespace TwoFactorAuthentication
{
    public class TwoFactor
    {     
        /// <summary>
        /// Used to Generate 2FA code,
        /// it is the key used to generate the code.
        /// Is set by the constructor and given as base32 encoded string.
        /// </summary>
        public readonly byte[] Secret;
        public TwoFactor(string secret)=>
            Secret = Base32.ToArray(secret);

        /// <summary>
        /// Generate a 2FA code
        /// </summary>
        /// <returns>6 digit 2FA code</returns>
        /// See: https://tools.ietf.org/html/rfc6238
        public string GenerateCode()
        {
            //Get timestamp.
            DateTime EPOCH = new DateTime(1970, 1, 1, 0, 0, 0);
            long timestamp = Convert.ToInt64(
                Math.Round((DateTime.UtcNow - EPOCH).TotalSeconds)) / 30;

            using (var HMAC = new HMACSHA1(Secret))
            {
                //Generate hash with secret as key and the current time.
                //See: https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.hmacsha1
                byte[] h = HMAC.ComputeHash(
                    BitConverter.GetBytes(timestamp).Reverse().ToArray());

                //Generate 6 digit number from hash.
                int offset = h.Last() & 0x0F;
                int number = (
                   ((h[offset + 0] & 0x7f) << 24) |
                   ((h[offset + 1] & 0xff) << 16) |
                   ((h[offset + 2] & 0xff) << 8)  |
                   (h[offset + 3] & 0xff)) % 1000000;

                //Create a string from the number.
                //Padleft to add zero's if needed.
                return number.ToString().PadLeft(6, '0');
            }
        }

        /// <summary>
        /// Validate a passed code.
        /// </summary>
        /// <param name="twoFactorCode">6 digit 2FA code</param>
        public bool ValidateCode(string twoFactorCode)=>
            GenerateCode().Equals(twoFactorCode);

        /// <summary>
        /// Generate a new secret
        /// </summary>
        /// <returns>Generated a secret</returns>
        public static string GenerateRandomSecret()
        {
            byte[] randomBytes = new byte[32];
            using (var rng = new RNGCryptoServiceProvider())
                rng.GetBytes(randomBytes);

            return string.Concat(randomBytes.Select(v => "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567"[v & 31]));
        }

        /// <summary>
        /// Generates the string that is needed to generate a qr code.
        /// Create a qr code of this string to scan with authy/Google Authenticator
        /// </summary>
        public static string GenerateQRCodeString(string secret, string label, string issuer)
            => $"otpauth://totp/{label}?secret={secret}&issuer={issuer}";
    }
}