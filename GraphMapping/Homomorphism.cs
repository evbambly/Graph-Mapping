using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace GraphMapping
{
    class Homomorphism
    {
        private List<Word> Definitions
        {
            get;
        }
        public void SetDefinition(Word definition)
        {
            int setToIndex = Definitions.IndexOf(Definitions.Find(word => word.name == definition.name));
            if (setToIndex == -1)
            {
                Definitions.Add(definition);
                Definitions.Add(definition.Reverse());
            }
            else
            {
                Definitions[setToIndex] = definition;
                definition = definition.Reverse();
                setToIndex = Definitions.IndexOf(Definitions.Find(word => word.name == definition.name));
                Definitions[setToIndex] = definition;
            }
        }
        public void RemoveDefinition(char definitionName)
        {
            Definitions.Remove(Definitions.Find(word => word.name == definitionName));
            Definitions.Remove(Definitions.Find(word => word.name == Char.FlipCase(definitionName)));
        }
        public List<Word> cumulitiveChanges;
        public Homomorphism(List<Word> definitions)
        {
            this.Definitions = new List<Word>();
            this.cumulitiveChanges = new List<Word>();
            if (definitions != null)
            {
                foreach (Word definition in definitions)
                {
                    SetDefinition(definition);
                    cumulitiveChanges.Add(Word.Create($"{definition.name}", definition.name));
                }
            }
        }

        public void PerformOperation(string whiteHead)
        {
            if (TryGetWordsFromWhiteHead(whiteHead, out char firstWordName, out char secondWordName))
            {
                Word firstWord = Definitions.Where(word => word.name == firstWordName).FirstOrDefault();
                Word secondWord = Definitions.Where(word => word.name == secondWordName).FirstOrDefault();

                if (firstWord != null && secondWord != null)
                {
                    int multiplicationType = Word.Multiply(firstWord, secondWord, out Word firstWordProduct, out Word secondWordProduct, out Word commonLetters);

                    List<Word> currentChange = new List<Word>();

                    // unflip whiteHead
                    secondWordName = Char.FlipCase(secondWordName);

                    if (multiplicationType == 2)
                    {
                        //// same letter?
                        char newDefinitionName = char.MinValue;
                        for (int c = (int)'z'; c >= (int)'a'; c--)
                        {
                            if (Definitions.Where(word => word.name == (char)c).Count() == 0)
                            {
                                newDefinitionName = (char)c;
                                break;
                            }
                        }
                        SetDefinition(Word.Create(new String(commonLetters.letters.ToArray()), newDefinitionName));

                        string firstWordString;
                        string secondWordString;
                        // if firstWordName is -1, it will be affected before. Else, after itself
                        if (Char.IsLetter(firstWordName, Char.LetterType.upperCase))
                        {
                            firstWordName = Char.FlipCase(firstWordName);
                            firstWordString = $"{newDefinitionName}{firstWordName}";
                        }
                        else
                        {
                            firstWordString = $"{firstWordName}{newDefinitionName}";
                        }
                        if (Char.IsLetter(secondWordName, Char.LetterType.upperCase))
                        {
                            secondWordName = Char.FlipCase(secondWordName);
                            secondWordString = $"{newDefinitionName}{secondWordName}";
                        }
                        else
                        {
                            secondWordString = $"{secondWordName}{newDefinitionName}";
                        }

                        currentChange.Add(Word.Create(firstWordString, firstWordName));
                        currentChange.Add(Word.Create(secondWordString, secondWordName));

                        SetDefinition(firstWordProduct);
                        SetDefinition(secondWordProduct);
                    }
                    // first changes, unless is negative
                    else if (multiplicationType == 3 || multiplicationType == 4)
                    {
                        ////Same letter?

                        // the one that has no letters left will change the one that has letters left
                        char changedWordName = firstWordProduct.letters.Count > 0 ? firstWordName : secondWordName;
                        Word changedWordProduct = changedWordName == firstWordName ? firstWordProduct : secondWordProduct;
                        char unchangedWordName = changedWordName == firstWordName ? secondWordName : firstWordName;

                        string changeString;
                        // if the changedWord is -1, it will be affected before. Else, after itself
                        if (Char.IsLetter(changedWordName, Char.LetterType.upperCase))
                        {
                            changedWordName = Char.FlipCase(changedWordName);
                            unchangedWordName = Char.FlipCase(unchangedWordName);
                            changeString = $"{unchangedWordName}{changedWordName}";
                        }
                        else
                        {
                            changeString = $"{changedWordName}{unchangedWordName}";
                        }

                        currentChange.Add(Word.Create(changeString, changedWordName));
                        SetDefinition(changedWordProduct);
                    }

                    // remove word
                    if (multiplicationType == 5)
                    {
                        if (Char.IsLetter(firstWordName, Char.LetterType.upperCase))
                        {
                            firstWordName = Char.FlipCase(firstWordName);
                        }
                        if (Char.IsLetter(secondWordName, Char.LetterType.upperCase))
                        {
                            secondWordName = Char.FlipCase(secondWordName);
                        }
                        char removeWordName = char.MinValue;
                        char retainWordName = char.MinValue;

                        foreach (Word word in cumulitiveChanges)
                        {
                            if (word.name == firstWordName)
                            {
                                retainWordName = firstWordName;
                                removeWordName = secondWordName;
                                break;
                            }
                            if (word.name == secondWordName)
                            {
                                retainWordName = secondWordName;
                                removeWordName = firstWordName;
                                break;
                            }
                        }
                        currentChange.Add(Word.Create($"{retainWordName}", removeWordName));
                        RemoveDefinition(removeWordName);
                    }

                    foreach (Word definition in Definitions)
                    {
                        if (Char.IsLetter(definition.name, Char.LetterType.lowerCase) && currentChange.Where(word => word.name == definition.name).Count() == 0)
                        {
                            currentChange.Add(Word.Create($"{definition.name}", definition.name));
                        }
                    }

                    GetCumulativeChange(currentChange);
                    Print(multiplicationType, currentChange, whiteHead);
                }
            }
        }
        private bool TryGetWordsFromWhiteHead(string whiteHead, out char firstWordName, out char secondWordName)
        {
            bool isValidWhiteHead = false;

            string[] whiteHeadHalves = whiteHead.ToLower().Split('.');

            firstWordName = char.MinValue;
            secondWordName = char.MinValue;

            if (whiteHeadHalves.Length == 2)
            {
                string leftSide = whiteHeadHalves[0];
                string rightSide = whiteHeadHalves[1];

                if (leftSide.Length > 0 && leftSide.Length < 4 && rightSide.Length > 0 && rightSide.Length < 4)
                {
                     firstWordName = leftSide[0];
                     secondWordName = rightSide[0];
                    if (Char.IsLetter(firstWordName, Char.LetterType.anyCase) && Char.IsLetter(secondWordName, Char.LetterType.anyCase))
                    {
                        if (leftSide.Length == 3 && leftSide.Remove(0,1) == "-1")
                        {
                            firstWordName = Char.FlipCase(firstWordName);
                        }
                        if (rightSide.Length == 3 && rightSide.Remove(0, 1) == "-1")
                        {
                            secondWordName = Char.FlipCase(secondWordName);
                        }

                        // Whitehead flips the second letter
                        secondWordName = Char.FlipCase(secondWordName);

                        isValidWhiteHead = true;
                    }
                }
            }

            return isValidWhiteHead;
        }
        private void GetCumulativeChange(List<Word> currentChange)
        {
            List<Word> changes = currentChange.Where(word => word.letters.Count > 1 || word.letters[0] != word.name).ToList();
            foreach (Word originalDefiniton in cumulitiveChanges)
            {
                foreach (Word change in changes)
                {
                    bool sameLetter(char letter) => letter == change.name || letter == Char.FlipCase(change.name);
                    int changeOccurance = originalDefiniton.letters.FindAll(sameLetter).Count;
                    if (changeOccurance > 0)
                    {
                        for (int i = 0; i < changeOccurance; i++)
                        {
                            int replaceIndex = originalDefiniton.letters.IndexOf(originalDefiniton.letters.Find(sameLetter));
                            Word relativeChange = Char.IsLetter(originalDefiniton.letters[replaceIndex], Char.LetterType.lowerCase) ? change : change.Reverse();
                            originalDefiniton.letters.RemoveAt(replaceIndex);
                            originalDefiniton.letters.InsertRange(replaceIndex, relativeChange.letters);
                        }   
                    }
                }
            }
        }
        private void Print(int multiplicationType, List<Word> currentChange, string whiteHead)
        {
            Console.WriteLine($"{whiteHead} ({multiplicationType})");
            Console.WriteLine("<--------->");
            Console.WriteLine("Definitions: ");
            foreach (Word word in Definitions.Where(word => Char.IsLetter(word.name, Char.LetterType.lowerCase)))
            {
                Console.Write($"{word.name} -> ");
                word.Print();
                Console.WriteLine();
            }
            Console.WriteLine("<--------->");
            Console.WriteLine("Cumulative Changes: ");
            foreach (Word word in cumulitiveChanges)
            {
                Console.Write($"{word.name} -> ");
                word.Print();
                Console.WriteLine();
            }
            Console.WriteLine("<--------->");
            Console.WriteLine("Current Change:");
            foreach (Word word in currentChange)
            {
                Console.Write($"{word.name} -> ");
                word.Print();
                Console.WriteLine();
            }
            Console.WriteLine();
            Console.WriteLine();
        }
    }
}
