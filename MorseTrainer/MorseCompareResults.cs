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
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MorseTrainer
{
    /// <summary>
    /// Flags that indicate how to display results
    /// </summary>
    [Flags]
    public enum ResultsFlags
    {
        /// <summary>
        /// Display characters that were received as sent
        /// </summary>
        Valid = 0x01,

        /// <summary>
        /// Display characters that were sent but not received
        /// </summary>
        Dropped = 0x02,

        /// <summary>
        /// Display characters that were recorded but not sent
        /// </summary>
        Extra = 0x04,

        /// <summary>
        /// Display all characters
        /// </summary>
        All = 0x07
    }

    /// <summary>
    /// MorseCompareResults manages the comparison results
    /// </summary>
    public class MorseCompareResults
    {

        /// <summary>
        /// Creates a new MorseCompareResults object from a list of substrings
        /// </summary>
        /// <param name="substrings">The substrings</param>
        /// <param name="sent">The string that was sent</param>
        /// <param name="recorded">The string that was typed by the listener</param>
        public MorseCompareResults(IEnumerable<MorseSubstring> substrings, String sent, String recorded)
        {
            _substringList = new List<MorseSubstring>(substrings);
            _sent = sent;
            _recorded = recorded;
        }

        /// <summary>
        /// Gets the string of characters sent
        /// </summary>
        public String Sent
        {
            get
            {
                return _sent;
            }
        }
        
        /// <summary>
        /// Gets the string of characters the user typed
        /// </summary>
        public String Recorded
        {
            get
            {
                return _recorded;
            }
        }

        /// <summary>
        /// Gets a list of substrings that are either matches, dropped, or extra strings
        /// </summary>
        public IList<MorseSubstring> SubStrings
        {
            get
            {
                return _substringList.AsReadOnly();
            }
        }

        private String _sent;
        private String _recorded;
        private List<MorseSubstring> _substringList;
    }

    /// <summary>
    /// MorseSubstring is an abstract class for the three different
    /// types of comparison results
    /// </summary>
    public abstract class MorseSubstring
    {
        protected MorseSubstring() { }
        protected MorseSubstring(String str)
        {
            _str = str;
        }

        /// <summary>
        /// Gets the characters of the substring
        /// </summary>
        public String Chars
        {
            get
            {
                return _str;
            }
        }

        /// <summary>
        /// Gets the color associated with the substring
        ///   Black: valid
        ///   Red: dropped
        ///   Orange: extra
        /// </summary>
        public abstract System.Drawing.Color Color
        {
            get;
        }

        /// <summary>
        /// Gets a string according to the ResultsDisplayFlags
        /// </summary>
        /// <param name="flags">Flags saying which characters to include</param>
        /// <returns></returns>
        public abstract String Str(ResultsFlags flags = ResultsFlags.All);

        /// <summary>
        /// Gets information about the string
        /// </summary>
        public abstract ResultsFlags ResultInfo
        {
            get;
        }

        protected string _str;
    }

    /// <summary>
    /// MorseValid holds a contiguous string of characters that exists in
    /// both the sent and recorded strings
    /// </summary>
    public class MorseValid : MorseSubstring
    {
        public MorseValid(String str) : base(str)
        {
        }

        public override string Str(ResultsFlags flags)
        {
            String ret = "";
            if ((flags & ResultsFlags.Valid) != 0)
            {
                ret = MorseInfo.ExpandProsigns(_str);
            }
            return ret;
        }

        public override ResultsFlags ResultInfo
        {
            get
            {
                return ResultsFlags.Valid;
            }
        }

        public override Color Color
        {
            get
            {
                return Color.Black;
            }
        }
    }

    /// <summary>
    /// MorseValid holds a contiguous string of characters that exists in
    /// the sent but not the recorded strings
    /// </summary>
    public class MorseDropped : MorseSubstring
    {
        public MorseDropped(String str) : base(str)
        {
        }

        public override string Str(ResultsFlags flags)
        {
            String ret = "";
            if ((flags & ResultsFlags.Dropped) != 0)
            {
                ret = MorseInfo.ExpandProsigns(_str);
            }
            return ret;
        }

        public override ResultsFlags ResultInfo
        {
            get
            {
                return ResultsFlags.Dropped;
            }
        }

        public override Color Color
        {
            get
            {
                return Color.Red;
            }
        }
    }

    /// <summary>
    /// MorseValid holds a contiguous string of characters that exists in
    /// the recorded but not the sent string
    /// </summary>
    public class MorseExtra : MorseSubstring
    {
        public MorseExtra(String str) : base(str)
        {
        }

        public override string Str(ResultsFlags flags)
        {
            String ret = "";
            if ((flags & ResultsFlags.Extra) != 0)
            {
                ret = MorseInfo.ExpandProsigns(_str);
            }
            return ret;
        }

        public override ResultsFlags ResultInfo
        {
            get
            {
                return ResultsFlags.Extra;
            }
        }

        public override Color Color
        {
            get
            {
                return Color.Orange;
            }
        }
    }
}
