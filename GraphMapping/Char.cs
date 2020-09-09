using System;
using System.Collections.Generic;
using System.Text;

namespace GraphMapping
{
    class Char
    {
        public enum LetterType
        {
            upperCase, 
            lowerCase, 
            anyCase
        }
        public static char FlipCase(char c)
        {
            if (IsLetter(c, LetterType.lowerCase))
            {
                return ToUpperCase(c);
            }
            if (IsLetter(c, LetterType.upperCase))
            {
                return ToLowerCase(c);
            }
            return c;
        }
        private static char ToUpperCase(char c)
        {
            int alpabeticalIndex = c - 'a';
            char result = 'A';
            result += (char)alpabeticalIndex;
            return result;
        }
        private static char ToLowerCase(char c)
        {
            int alpabeticalIndex = c - 'A';
            char result = 'a';
            result += (char)alpabeticalIndex;
            return result;
        }
        public static bool IsLetter(char c, LetterType letterType)
        {
            bool isLetter = false;

            if (letterType != LetterType.upperCase && c >= 'a' && c <= 'z')
            {
                isLetter = true;
            }
            if (!isLetter && letterType != LetterType.lowerCase && c >= 'A' && c <= 'Z')
            {
                isLetter = true;
            }
            return isLetter;
        }
        
    }
}
