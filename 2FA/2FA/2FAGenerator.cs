/* 2FA
 * Copyright (C) 2019  Henkje (henkje@pm.me)
 * 
 * MIT license
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

namespace _2FA
{
    class _2FAGenerator
    {     
        /// <summary>
        /// Used to Generate 2FA code,
        /// </summary>
        byte[] Secret;
        public _2FAGenerator(string Secret)=>
            this.Secret = Base32.ToBytes(Secret);

        /// <summary>
        /// Generate a 2FA code
        /// </summary>
        /// <returns>2FA code</returns>
        /// See: https://tools.ietf.org/html/rfc6238
        public string GenerateCode()
        {
            //Get timestamp.
            DateTime EPOCH = new DateTime(1970, 1, 1, 0, 0, 0);
            long Timestamp = Convert.ToInt64(
                Math.Round((DateTime.UtcNow - EPOCH).TotalSeconds)) / 30;

            //Generate hash.
            byte[] HMAC = new HMACSHA1(Secret)
                .ComputeHash(
                BitConverter.GetBytes(Timestamp).Reverse().ToArray());

            //Generate 6 digit number.
            int Offset = HMAC.Last() & 0x0F;
            int Number = (
               ((HMAC[Offset + 0] & 0x7f) << 24) |
               ((HMAC[Offset + 1] & 0xff) << 16) |
               ((HMAC[Offset + 2] & 0xff) << 8) |
               (HMAC[Offset + 3] & 0xff)) % 1000000;

            //int doesn't start with a 0.
            //Add the zero's back to it.
            string _2FACode = Number.ToString();
            while (_2FACode.Length < 6)
                _2FACode = '0' + _2FACode;

            return _2FACode;
        }
    }
}
