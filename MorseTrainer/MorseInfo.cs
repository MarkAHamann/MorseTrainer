/*
    Morse Trainer
    Copyright (C) 2016 Mark Hamann

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MorseTrainer
{
    /// <summary>
    /// MorseInfo is a static class that contains information about
    /// Morse code
    /// </summary>
    public static class MorseInfo
    {
        /// <summary>
        /// A constant used as the BT prosign in strings
        /// </summary>
        public const char PROSIGN_BT = '\x80';

        /// <summary>
        /// A constant used as the SK prosign in strings
        /// </summary>
        public const char PROSIGN_SK = '\x81';

        /// <summary>
        /// A constant used as the AR prosign in strings
        /// </summary>
        public const char PROSIGN_AR = '\x82';

        static MorseInfo()
        {
            // save the morse conversions into an array
            __conversions = new String[256];
            __conversions['A'] = ".-";
            __conversions['B'] = "-...";
            __conversions['C'] = "-.-.";
            __conversions['D'] = "-..";
            __conversions['E'] = ".";
            __conversions['F'] = "..-.";
            __conversions['G'] = "--.";
            __conversions['H'] = "....";
            __conversions['I'] = "..";
            __conversions['J'] = ".---";
            __conversions['K'] = "-.-";
            __conversions['L'] = ".-..";
            __conversions['M'] = "--";
            __conversions['N'] = "-.";
            __conversions['O'] = "---";
            __conversions['P'] = ".--.";
            __conversions['Q'] = "--.-";
            __conversions['R'] = ".-.";
            __conversions['S'] = "...";
            __conversions['T'] = "-";
            __conversions['U'] = "..-";
            __conversions['V'] = "...-";
            __conversions['W'] = ".--";
            __conversions['X'] = "";
            __conversions['Y'] = "-.--";
            __conversions['Z'] = "--..";

            __conversions['0'] = "-----";
            __conversions['1'] = ".----";
            __conversions['2'] = "..---";
            __conversions['3'] = "...--";
            __conversions['4'] = "....-";
            __conversions['5'] = ".....";
            __conversions['6'] = "-....";
            __conversions['7'] = "--...";
            __conversions['8'] = "---..";
            __conversions['9'] = "----.";

            __conversions['.'] = ".-.-.-";
            __conversions[','] = "--..--";
            __conversions['?'] = "..--..";
            __conversions['/'] = "-..-.";

            __conversions[PROSIGN_BT] = "-...-";
            __conversions[PROSIGN_SK] = "...-.-";
            __conversions[PROSIGN_AR] = ".-.-.";

            __prosignExpansionToValue = new Dictionary<string, char>();
            __prosignExpansionToValue.Add("<BT>", PROSIGN_BT);
            __prosignExpansionToValue.Add("<SK>", PROSIGN_SK);
            __prosignExpansionToValue.Add("<AR>", PROSIGN_AR);

            __prosignValueToExpansion = new Dictionary<char, string>();
            foreach (KeyValuePair<String, char> kv in __prosignExpansionToValue)
            {
                __prosignValueToExpansion.Add(kv.Value, kv.Key);
            }


            // save the number of elements--needed to calculate Farnsworth timing
            __elements = new int[256];
            for (int i = 0; i < __elements.Length; ++i)
            {
                int elements = 0;
                String s = __conversions[i];
                if (!String.IsNullOrEmpty(s))
                {
                    foreach (char c in s)
                    {
                        if (c == '.')
                        {
                            elements += 1;
                        }
                        else if (c == '-')
                        {
                            elements += 3;
                        }
                    }
                }
                __elements[i] = elements;
            }
        }

        private static Dictionary<String, char> __prosignExpansionToValue;
        private static Dictionary<char, String> __prosignValueToExpansion;
        private static String[] __conversions;
        private static int[] __elements;

        /// <summary>
        /// Gets the number of dot-length elements in the character c.
        /// There is 1 per dot, 3 per dash, and 1 for each space between the.
        /// </summary>
        /// <param name="c">A character</param>
        /// <returns></returns>
        public static int ToElements(Char c)
        {
            if (c < 0 || c >= __elements.Length)
            {
                throw new ArgumentException("c");
            }
            return __elements[c];
        }

        /// <summary>
        /// Converts the character 'c' to Morse code using '.' and '-'.
        /// The ' ' is used for the space
        /// </summary>
        /// <param name="c">An ascii character or prosign constant</param>
        /// <returns>A string of /[.-]+| /</returns>
        public static String ToMorse(Char c)
        {
            if (c < 0 || c >= __elements.Length)
            {
                throw new ArgumentException("c");
            }
            String s = __conversions[c];
            return s;
        }

        /// <summary>
        /// Converts a string with expanded prosigns into a string containing
        /// the prosign constants
        /// </summary>
        /// <param name="expandedProsigns">A string with expanded prosigns</param>
        /// <returns>A string with prosign constants</returns>
        public static String ReplaceProsigns(String expandedProsigns)
        {
            String replaced = expandedProsigns;
            foreach (KeyValuePair<String, char> kv in __prosignExpansionToValue)
            {
                String expansion = kv.Key;
                String value = kv.Value.ToString();
                replaced = replaced.Replace(expansion, value);
            }
            return replaced;
        }

        /// <summary>
        /// Converts a string with prosign constants into a string containing
        /// the expanded prosigns
        /// </summary>
        /// <param name="valuedProsigns">A string with prosign constants</param>
        /// <returns>A string with expanded prosigns</returns>
        public static String ExpandProsigns(String valuedProsigns)
        {
            String replaced = valuedProsigns;
            foreach (KeyValuePair<String, char> kv in __prosignExpansionToValue)
            {
                String expansion = kv.Key;
                String value = kv.Value.ToString();
                replaced = replaced.Replace(value, expansion);
            }
            return replaced;
        }
    }
}
