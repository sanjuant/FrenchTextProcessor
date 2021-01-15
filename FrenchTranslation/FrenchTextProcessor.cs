// Copyright (c) Sorrow. All rights reserved.  

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using TaleWorlds.Core;
using static FrenchTranslation.FrenchTranslation.ConfigCulture;

namespace FrenchTranslation
{
    class FrenchTextProcessor
    {
        private readonly FrenchTranslation ft;
        public FrenchTextProcessor()
        {
            ft = FrenchTranslation.Instance;
        }

        public void ProcessToken(string sourceText, ref int cursorPos, string token, StringBuilder outputString)
        {
            string strBefore = sourceText.Substring(0, cursorPos);
            string strBeforeSanitize = StripHTML(strBefore);
            int newCursorPos = cursorPos - (strBefore.Length - strBeforeSanitize.Length);
            sourceText = StripHTML(sourceText);
            Tuple<string, string> nextWord = GetNextWord(sourceText, newCursorPos);

            if (TokenStartWithCulture(token))
            {
                outputString.Append(ProcessFaction(token).Item1);
            }
            else if (token == "._le" || token == "._la")
            {
                string previousWord = Regex.Match(sourceText.Substring(0, newCursorPos - (token.Length + 2)), @"(\w*)\s*$").Groups[1].Value;
                token = token.Replace("._", string.Empty).ToLower();

                string newValue;
                if (token == "le")
                {
                    if (CheckFirstCharIsVowel(previousWord))
                    {
                        outputString.Replace(previousWord, "l'");
                    } else
                    {
                        outputString.Replace(previousWord, "le ");
                    }
                    outputString.Append(previousWord);
                }
                else if (token == "la")
                {
                    if (CheckFirstCharIsVowel(previousWord))
                    {
                        outputString.Replace(previousWord, "l'");
                    }
                    else
                    {
                        outputString.Replace(previousWord, "la");
                    }
                    if (ft.wordConfig.JobName.TryGetValue(previousWord, out newValue))
                    {
                        outputString.Append(newValue);
                    } else
                    {
                        outputString.Append(previousWord);
                    }
                }
            }
            else if (token == ".es" || token == ".s")
            {
                Suffix(sourceText, outputString);
            }
            else
            {
                Prefix(nextWord, token, outputString);
            }
        }

        private bool TokenStartWithCulture(string token)
        {
            Dictionary<string, string> cultures = new Dictionary<string, string>
            {
                { ".ase", ".ase" },
                { ".bat", ".bat" },
                { ".emp", ".emp" },
                { ".khu", ".khu" },
                { ".stu", ".stu" },
                { ".vla", ".vla" },
                { ".aserai", ".aserai" },
                { ".battania", ".battania" },
                { ".empire", ".empire" },
                { ".khuzait", ".khuzait" },
                { ".sturgia", ".sturgia" },
                { ".vlandia", ".vlandia" },
            };
            if (cultures.TryGetValue(token.Split('_').First().ToLower(), out string poorValue))
            {
                return true;
            }
            return false;
        }

        private Tuple<string, string> ProcessFaction(string token)
        {
            bool isUpper = char.IsUpper(token[1]);
            string[] splittedToken = token.Split('.');
            splittedToken = splittedToken[1].Split('_');
            string tokenFaction = splittedToken.First().ToLower();
            string tokenGender = splittedToken.Last().ToLower();

            switch (tokenFaction)
            {
                case "ase": return new Tuple<string, string>(GetCulturePropertyValue(ft.configCulture.Aserai, tokenGender, isUpper), tokenGender);
                case "bat": return new Tuple<string, string>(GetCulturePropertyValue(ft.configCulture.Battanians, tokenGender, isUpper), tokenGender);
                case "emp": return new Tuple<string, string>(GetCulturePropertyValue(ft.configCulture.Empire, tokenGender, isUpper), tokenGender);
                case "khu": return new Tuple<string, string>(GetCulturePropertyValue(ft.configCulture.Khuzaits, tokenGender, isUpper), tokenGender);
                case "stu": return new Tuple<string, string>(GetCulturePropertyValue(ft.configCulture.Sturgians, tokenGender, isUpper), tokenGender);
                case "vla": return new Tuple<string, string>(GetCulturePropertyValue(ft.configCulture.Vlandians, tokenGender, isUpper), tokenGender);
                case "aserai": return new Tuple<string, string>(GetCulturePropertyValue(ft.configCulture.Aserai, tokenGender, isUpper), tokenGender);
                case "battania": return new Tuple<string, string>(GetCulturePropertyValue(ft.configCulture.Battanians, tokenGender, isUpper), tokenGender);
                case "empire": return new Tuple<string, string>(GetCulturePropertyValue(ft.configCulture.Empire, tokenGender, isUpper), tokenGender);
                case "khuzait": return new Tuple<string, string>(GetCulturePropertyValue(ft.configCulture.Khuzaits, tokenGender, isUpper), tokenGender);
                case "sturgia": return new Tuple<string, string>(GetCulturePropertyValue(ft.configCulture.Sturgians, tokenGender, isUpper), tokenGender);
                case "vlandia": return new Tuple<string, string>(GetCulturePropertyValue(ft.configCulture.Vlandians, tokenGender, isUpper), tokenGender);
                default: return new Tuple<string, string>(string.Empty, string.Empty);
            }
        }

        private string GetCulturePropertyValue(Culture item, string tokenGender, bool isUpper = false)
        {
            tokenGender = FormatCultureProperty(tokenGender);
            System.Reflection.PropertyInfo pi = item.GetType().GetProperty(tokenGender);
            if (null == pi)
            {
                return string.Empty;
            }
            string value = (string)pi.GetValue(item, null);
            if (null == value)
            {
                return string.Empty;
            }
            if (tokenGender == "Region")
            {
                return value;
            }
            return Capitalize(value, isUpper);
        }

        private string FormatCultureProperty(string property)
        {
            switch (property)
            {
                case "male": return "Male";
                case "female": return "Female";
                case "plural": return "MalePlural";
                case "maleplural": return "MalePlural";
                case "femaleplural": return "FemalePlural";
                case "region": return "Region";
                default: return property;
            }
        }

        private void Suffix(string sourceText, StringBuilder outputString)
        {
            sourceText = sourceText.ToLower();
            string lastWord = outputString.ToString().Split(' ').Last();
            string[] splittedSourceText = sourceText.Split(' ');

            string newValue;
            if (ft.wordItem.FemalePlural.Any(w => splittedSourceText.Contains(w.Key)))
            {
                if (ft.wordConfig.Adjective.FemalePlural.TryGetValue(lastWord, out newValue))
                {
                    outputString.Replace(lastWord, newValue);
                }
                else
                {
                    outputString.Append("es");
                }
            }
            else if (ft.wordItem.MalePlural.Any(w => splittedSourceText.Contains(w.Key)))
            {
                if (ft.wordConfig.Adjective.MalePlural.TryGetValue(lastWord, out newValue))
                {
                    outputString.Replace(lastWord, newValue);
                }
                else
                {
                    outputString.Append('s');
                }
            }
            else if (ft.wordItem.Female.Any(w => splittedSourceText.Contains(w.Key)))
            {
                if (ft.wordConfig.Adjective.Female.TryGetValue(lastWord, out newValue))
                {
                    outputString.Replace(lastWord, newValue);
                }
                else
                {
                    outputString.Append('e');
                }
            }
            return;
        }

        private void Prefix(Tuple<string, string> nextWord, string token, StringBuilder outputString)
        {
            bool isUpper = char.IsUpper(token[1]);
            token = token.Replace(".", string.Empty).ToLower();
            if (token == "le" || token == "la")
            {
                AppendPrefix(nextWord, token, outputString, isUpper, "le", "la", "l'", "les");
            }
            else if (token == "des" || token == "de")
            {
                AppendPrefix(nextWord, token, outputString, isUpper, "du", "de la", "de l'", "des");
            }
            else if (token == "de_")
            {
                AppendPrefix(nextWord, token, outputString, isUpper, "du", "de", "d'", "des");
            }
            else if (token == "a")
            {
                AppendPrefix(nextWord, token, outputString, isUpper, "au", "à la", "à l'", "aux");
            }
            return;
        }

        private void AppendPrefix(Tuple<string, string> nextWord, string token, StringBuilder outputString, bool isUpper, string male, string female, string singularVowel, string plural)
        {
            string nextWordText = nextWord.Item1;
            string nextWordGender = nextWord.Item2;

            if ("les".Equals(nextWordText))
            {
                outputString.Append(Capitalize("des", isUpper));
                outputString.Append(' ');
            }
            else if ("le".Equals(nextWordText))
            {
                outputString.Append(Capitalize("du", isUpper));
                outputString.Append(' ');
            }
            else if ("la".Equals(nextWordText))
            {
                outputString.Append(Capitalize("de", isUpper));
                outputString.Append(' ');
            }
            else if ("plural".Equals(nextWordGender))
            {
                outputString.Append(Capitalize(plural, isUpper));
                outputString.Append(' ');
            }
            else if (CheckFirstCharIsVowel(nextWordText))
            {
                outputString.Append(Capitalize(singularVowel, isUpper));
            }
            else if ("male".Equals(nextWordGender))
            {
                outputString.Append(Capitalize(male, isUpper));
                outputString.Append(' ');
            }
            else if ("female".Equals(nextWordGender))
            {
                outputString.Append(Capitalize(female, isUpper));
                outputString.Append(' ');
            }
            else
            {
                token = token.Replace("_", string.Empty);
                if (token == plural)
                {
                    outputString.Append(Capitalize(plural, isUpper));
                    outputString.Append(' ');
                }
                else if (CheckFirstCharIsVowel(nextWordText))
                {
                    outputString.Append(Capitalize(singularVowel, isUpper));
                }
                else if (token == female)
                {
                    outputString.Append(Capitalize(token, isUpper));
                    outputString.Append(' ');
                }
            }
            return;
        }

        private string Capitalize(string str, bool isUpper = false)
        {
            return isUpper ? System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(str) : str.ToLower();
        }

        private Tuple<string, string> GetNextWord(string sourceText, int cursorPos)
        {
            string badChar = " .,";
            string subString = sourceText.Substring(cursorPos, sourceText.Length - cursorPos);
            bool isToken = subString.StartsWith("{");
            Tuple<string, string> tupleToken = null;
            string text = "";
            string gender = "";
            string poorValue;

            if (isToken)
            {
                badChar = "}";
                cursorPos += 1;
            }
            while (cursorPos < sourceText.Length)
            {
                char value = sourceText[cursorPos];
                if (!badChar.Contains(value))
                {
                    text += value;
                }
                else
                {
                    break;
                }
                cursorPos++;
            }
            if (isToken)
            {
                tupleToken = ProcessFaction(text);
            }

            if (null != tupleToken)
            {
                text = tupleToken.Item1;
                string pattern = "plural";
                Match m = Regex.Match(text, pattern, RegexOptions.IgnoreCase);
                if (m.Success)
                {
                    gender = "plural";
                }
                else if ("region".Equals(tupleToken.Item2))
                {
                    string twoLastChar = text.Substring(text.Length - 2);

                    if (ft.wordConfig.RegionException.Male.TryGetValue(text, out poorValue))
                    {
                        gender = "male";
                    }
                    else if (ft.wordConfig.RegionException.Female.TryGetValue(text, out poorValue))
                    {
                        gender = "female";
                    }
                    else if (ft.wordConfig.RegionException.Plural.TryGetValue(text, out poorValue))
                    {
                        gender = "plural";
                    }
                    else if (twoLastChar == "ia" || twoLastChar[1] == 'e')
                    {
                        gender = "female";
                    }
                    else if (twoLastChar[1] == 's')
                    {
                        gender = "plural";
                    }
                    else
                    {
                        gender = "male";
                    }
                }
                else
                {
                    gender = tupleToken.Item2;
                }
            }
            else
            {
                if (ft.wordPlural.Words.TryGetValue(text, out poorValue))
                {
                    gender = "plural";
                }
                else if (ft.wordMale.Words.TryGetValue(text, out poorValue))
                {
                    gender = "male";
                }
                else if (ft.wordFemale.Words.TryGetValue(text, out poorValue))
                {
                    gender = "female";
                }
            }


            return new Tuple<string, string>(text.ToLower(), gender);
        }

		private bool CheckFirstCharIsVowel(string nextWord)
        {
            if (!nextWord.IsEmpty())
            {
                char value = nextWord[0];
                if (ft.wordConfig.Chars.Vowels.Contains(value))
                {
                    return true;
                }
                else if (ft.wordConfig.Chars.Consonants.Contains(value))
                {
                    return false;
                }
            }
            return false;
        }

        private string StripHTML(string input)
        {

            return Regex.Replace(input, "<.*?>", string.Empty);
        }

    }
}
