using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MorseTrainer
{
    public class Koch
    {
        /// <summary>
        /// Gets the 
        /// </summary>
        /// <param name="c"></param>
        /// <returns>A string with the characters</returns>
        static public String CharsUpToAndIncluding(Char c)
        {
            int end = IndexOf(c);
            return CharsUpToAndIncluding(end);
        }

        /// <summary>
        /// Gets the 
        /// </summary>
        /// <param name="index"></param>
        /// <returns>A string with the characters</returns>
        static public String CharsUpToAndIncluding(int index)
        {
            String ret = null;
            if (index > 0 && index < Length)
            {
                ret = Order.Substring(0, index);
            }
            return ret;
        }

        static public Char[] RecentFromChar(Char c, int numberOfChars)
        {
            int end = IndexOf(c);
            int start = Math.Max(0, end - numberOfChars);
            return Order.Substring(start, numberOfChars).ToCharArray();
        }

        static public Char[] RecentFromIndex(Char c, int numberOfChars)
        {
            int end = IndexOf(c);
            int start = Math.Max(0, end - numberOfChars);
            return Order.Substring(start, numberOfChars).ToCharArray();
        }

        static public int IndexOf(Char c)
        {
            return Order.IndexOf(c);
        }

        static Koch()
        {
            Order = String.Concat("KMRSUAPTLOWI.NJEF0Y,VG5/Q9ZH38B?427C1D6X",
                "\x80\x81\x82");
            Length = Order.Length;
        }

        static public readonly String Order;
        static public readonly int Length;
    }
}
