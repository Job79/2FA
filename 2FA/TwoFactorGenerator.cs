/* TwoFactor
 * 
 * Copyright (c) 2019 henkje
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
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
        /// </summary>
        public readonly byte[] Secret;
        public TwoFactor(string secret)=>
            Secret = Base32.ToArray(secret);

        /// <summary>
        /// Generate a 2FA code
        /// </summary>
        /// <returns>2FA code</returns>
        /// See: https://tools.ietf.org/html/rfc6238
        public string GenerateCode()
        {
            //Get timestamp.
            DateTime EPOCH = new DateTime(1970, 1, 1, 0, 0, 0);
            long timestamp = Convert.ToInt64(
                Math.Round((DateTime.UtcNow - EPOCH).TotalSeconds)) / 30;

            //Generate hash.
            byte[] HMAC = new HMACSHA1(Secret)
                .ComputeHash(
                BitConverter.GetBytes(timestamp).Reverse().ToArray());

            //Generate 6 digit number.
            int offset = HMAC.Last() & 0x0F;
            int number = (
               ((HMAC[offset + 0] & 0x7f) << 24) |
               ((HMAC[offset + 1] & 0xff) << 16) |
               ((HMAC[offset + 2] & 0xff) << 8) |
               (HMAC[offset + 3] & 0xff)) % 1000000;

            //int doesn't start with a 0.
            //Add the zero's back to it.
            return number.ToString().PadLeft(6, '0');
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
        /// <returns>Generated secret</returns>
        public static string GenerateSecret()
        {
            byte[] randomBytes = new byte[32];
            new RNGCryptoServiceProvider().GetBytes(randomBytes);
            return string.Concat(randomBytes.Select(v => "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567"[v & 31]));
        }
    }
}
