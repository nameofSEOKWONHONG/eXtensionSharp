using System;
using System.Numerics;
using System.Text;

namespace eXtensionSharp;

public static class NumberExtensions
{
    extension(int source)
    {
        public string xToRandomString(bool lowerCase = false)
        {
            var builder = new StringBuilder(source);

            // Unicode/ASCII Letters are divided into two blocks
            // (Letters 65–90 / 97–122):
            // The first group containing the uppercase letters and
            // the second group containing the lowercase.

            // char is a single Unicode character
            char offset = lowerCase ? 'a' : 'A';
            const int lettersOffset = 26; // A...Z or a..z: length=26

            var random = new Random();
            for (var i = 0; i < source; i++)
            {
                var @char = (char)random.Next(offset, offset + lettersOffset);
                builder.Append(@char);
            }

            return lowerCase ? builder.ToString().ToLower() : builder.ToString();
        }
    }
}

