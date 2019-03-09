using System;

namespace _2FA
{
    internal class Base32
    {
        //Made by: Shane
        //http://stackoverflow.com/a/7135008/1242
        //[Edited by henkje]
        public static byte[] ToBytes(string Input)
        {
            Input = Input.TrimEnd('='); //Remove padding characters.
            int ByteCount = Input.Length * 5 / 8;
            byte[] ReturnArray = new byte[ByteCount];

            byte CurByte = 0, BitsRemaining = 8;
            int Mask = 0, ArrayIndex = 0;

            foreach (char c in Input)
            {
                int cValue = CharToValue(c);

                if (BitsRemaining > 5)
                {
                    Mask = cValue << (BitsRemaining - 5);
                    CurByte = (byte)(CurByte | Mask);
                    BitsRemaining -= 5;
                }
                else
                {
                    Mask = cValue >> (5 - BitsRemaining);
                    CurByte = (byte)(CurByte | Mask);
                    ReturnArray[ArrayIndex++] = CurByte;
                    CurByte = (byte)(cValue << (3 + BitsRemaining));
                    BitsRemaining += 3;
                }
            }

            //When not end with a full byte[].
            if (ArrayIndex != ByteCount)
                ReturnArray[ArrayIndex] = CurByte;

            return ReturnArray;
        }

        private static int CharToValue(char c)
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
            else throw new ArgumentException("Character is not a valid Base32 character.");
        }
    }
}
