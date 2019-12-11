/* Made by: Shane
 * Edited by me.
 * http://stackoverflow.com/a/7135008/1242
 * 
 * This class is used to convert the secret 
 * from a base32 string to a byte array.
 */

using System;

namespace TwoFactorAuthentication
{
    internal class Base32
    {
        public static byte[] ToArray(string input)
        {
            input = input.TrimEnd('='); //Remove padding characters.
            int byteCount = input.Length * 5 / 8;//this must be TRUNCATED
            byte[] returnArray = new byte[byteCount];

            byte curByte = 0, bitsRemaining = 8;
            int mask, arrayIndex = 0;

            foreach (char c in input)
            {
                int cValue = EncodeChar(c);

                if (bitsRemaining > 5)
                {
                    mask = cValue << (bitsRemaining - 5);
                    curByte = (byte)(curByte | mask);
                    bitsRemaining -= 5;
                }
                else
                {
                    mask = cValue >> (5 - bitsRemaining);
                    curByte = (byte)(curByte | mask);
                    returnArray[arrayIndex++] = curByte;
                    curByte = (byte)(cValue << (3 + bitsRemaining));
                    bitsRemaining += 3;
                }
            }

            //if we didn't end with a full byte
            if (arrayIndex != byteCount)
                returnArray[arrayIndex] = curByte;

            return returnArray;
        }

        private static int EncodeChar(char c)
        {
            int value = (int)c;

            //Uppercase letters.
            if (value < 91 && value > 64)
                return value - 65;
            //Numbers.
            else if (value < 56 && value > 49)
                return value - 24;
            //Lowercase letters.
            else if (value < 123 && value > 96)
                return value - 97;
            else throw new ArgumentException("Secret is not a valid Base32 string.");
        }
    }
}
