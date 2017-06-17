using System;
using System.Linq;
using System.Text;

namespace Library.Code
{
    public static class Common {
        private static Random random = new Random();
        public static string RandomString(int length = 8)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public static string RandomNumbers(int length = 8)
        {
            const string chars = "0123456789";
            return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public static string RandomKeys(int length = 8) {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[8];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            var finalString = new String(stringChars);
            return finalString;
        }
        // public static string GetUniqueKey(int maxSize)
        // {
        //     char[] chars = new char[62];
        //     chars =
        //     "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
        //     byte[] data = new byte[1];
        //     using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
        //     {
        //         crypto.GetNonZeroBytes(data);
        //         data = new byte[maxSize];
        //         crypto.GetNonZeroBytes(data);
        //     }
        //     StringBuilder result = new StringBuilder(maxSize);
        //     foreach (byte b in data)
        //     {
        //         result.Append(chars[b % (chars.Length)]);
        //     }
        //     return result.ToString();
        // }
    }
}