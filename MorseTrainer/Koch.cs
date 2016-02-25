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
    /// The Koch class contains information about the Koch order
    /// </summary>
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
