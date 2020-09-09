using System;
using System.Collections.Generic;

namespace GraphMapping
{
    class Program
    {
        static void Main(string[] args)
        {
            /* Word abc = Word.Create("abc", 'X');
             Word Ba = Word.Create("b-1a", 'Y');
             Word Ca = Word.Create("c-1a", 'Y');
             Word CB = Word.Create("c-1b-1", 'Y');
             Word CBAc = Word.Create("c-1b-1a-1c", 'Y');
             Word CBA = Word.Create("c-1b-1a-1", 'Y');
             PrintMultiplication(abc, Ba, out _, out _, out _);
             PrintMultiplication(abc, Ca, out _, out _, out _);
             PrintMultiplication(abc, CB, out _, out _, out _);
             PrintMultiplication(abc, CBAc, out _, out _, out _);
             PrintMultiplication(abc, CBA, out _, out _, out _);*/

            List<Word> definitionsA = new List<Word>(){
                Word.Create("aba-1bb", 'x'),
            Word.Create("a-1bb", 'y')};

            List<string> whiteHeadsA = new List<string>()
            {
                "x.y",
                "x.y",
                "x.y-1",
                "y.z",
            };

            SolveHomomorphism(definitionsA, whiteHeadsA);
        }
        static void PrintMultiplication(Word a, Word b, out Word aProduct, out Word bProduct, out Word commonLetters)
        {
            Console.WriteLine(Word.Multiply(a, b, out aProduct, out bProduct, out commonLetters));
            Console.Write("aProduct: ");
            aProduct.Print();
            Console.Write(" | bProduct: ");
            bProduct.Print();
            Console.Write(" | commonLetters: ");
            commonLetters.Print();
            Console.WriteLine();
        }
        static void SolveHomomorphism(List<Word> definitions, List<string> whiteheads)
        {
            Homomorphism hm = new Homomorphism(definitions);

            foreach (string whitehead in whiteheads)
            {
                hm.PerformOperation(whitehead);
            }
        }
    }
}
