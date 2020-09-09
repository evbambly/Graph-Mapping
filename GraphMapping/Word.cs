using System;
using System.Collections.Generic;
using System.Text;

namespace GraphMapping
{
    class Word
    {
        public List<char> letters;
        public char name;
        private Word(List<char> letters, char name)
        {
            this.letters = new List<char>(letters);
            this.name = name;
        }
        public static Word Create(string input, char name)
        {
            Word result = null;
            if (TryFormat(input, out List<char> letters))
            {
                result = new Word(letters, name);
            }
            return result;
        }
        public static int Multiply(Word a, Word b, out Word aProduct, out Word bProduct, out Word commonLetters)
        {
            int multiplicationType;
            int shorterWord = a.letters.Count;
            if (b.letters.Count < shorterWord)
            {
                shorterWord = b.letters.Count;
            }
            int aLength = a.letters.Count;
            aProduct = new Word(a.letters, a.name);
            bProduct = new Word(b.letters, b.name);
            commonLetters = new Word(new List<char>(), char.MinValue);
            for (int i = 0; i < shorterWord; i++)
            {
                if (a.letters[aLength - i - 1] == Char.FlipCase(b.letters[i]))
                {
                    commonLetters.letters.Insert(0,a.letters[aLength - i - 1]);
                    aProduct.letters.RemoveAt(aProduct.letters.Count - 1);
                    bProduct.letters.RemoveAt(0);
                }
                else
                {
                    break;
                }
            }

            bool aHasLetters = aProduct.letters.Count > 0;
            bool bHasLetters = bProduct.letters.Count > 0;

            if (commonLetters.letters.Count == 0)
            {
                multiplicationType = 1;
            }
            else if (aHasLetters && bHasLetters)
            {
                multiplicationType = 2;
            }
            else if (!aHasLetters && !bHasLetters)
            {
                multiplicationType = 5;
            }
            // If bHasLetters or (a-1), multiplicationType == 4
            else if (aHasLetters && Char.IsLetter(a.name, Char.LetterType.lowerCase))
            {
                multiplicationType = 3;
            }
            else
            {
                multiplicationType = 4;
            }

            return multiplicationType;
        }
        private static bool TryFormat(string input, out List<char> letters)
        {
            bool isValid = true;
            letters = new List<char>();
            for (int i = 0; i < input.Length; i++)
            {
                if (Char.IsLetter(input[i], Char.LetterType.anyCase))
                {
                    // The only non-lowercase allowed is "-1", after a letter
                    if (i < input.Length - 2 && input[i + 1] == '-' && input[i + 2] == '1')
                    {
                        letters.Add(Char.FlipCase(input[i]));
                        i += 2;
                    }
                    else
                    {
                        letters.Add(input[i]);
                    }
                }
                else
                {
                    isValid = false;
                    break;
                }
            }
            return isValid;
        }
        public Word Reverse()
        {
            Word reversedWord = new Word(letters, Char.FlipCase(name));
            reversedWord.letters.Reverse();
            for (int i = 0; i < reversedWord.letters.Count; i++)
            {
                reversedWord.letters[i] = Char.FlipCase(reversedWord.letters[i]);
            }
            return reversedWord;
        }
        public void Print()
        {
            foreach (char c in letters)
            {
                if (Char.IsLetter(c, Char.LetterType.upperCase))
                {
                    Console.Write(Char.FlipCase(c) + "-1");
                }
                else
                {
                    Console.Write(c);
                }
            }
        }
    }
}
