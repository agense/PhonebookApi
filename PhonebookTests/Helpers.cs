using System;
using System.Linq;

namespace PhonebookTests
{
    public static class Helpers
    {
        private static Random random = new Random();

        public static string GetAlpha(int length = 9)
        {
            const string chars = "abcdefghijklmnoprstuvyxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static string GetNumeric(int length = 6)
        {
            string numeric = "";
            for (int i = 0; i < length; i++)
            {
                numeric += random.Next(9);
            }
            return numeric;
        }
    }
}
