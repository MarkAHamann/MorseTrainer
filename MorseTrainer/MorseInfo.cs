using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MorseTrainer
{
    public class MorseInfo
    {
        public const char PROSIGN_BT = '\x80';
        public const char PROSIGN_SK = '\x81';
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

        public static int ToElements(Char c)
        {
            if (c < 0 || c >= __elements.Length)
            {
                throw new ArgumentException("c");
            }
            return __elements[c];
        }

        public static String ToMorse(Char c)
        {
            if (c < 0 || c >= __elements.Length)
            {
                throw new ArgumentException("c");
            }
            String s = __conversions[c];
            return s;
        }

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
