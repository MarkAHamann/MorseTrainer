using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MorseTrainer
{
    [Flags]
    public enum ResultsDisplayFlags
    {
        Valid = 0x01,
        Dropped = 0x02,
        Extra = 0x04,
        All = 0x07
    }

    public class MorseCompareResults
    {
        public MorseCompareResults(IEnumerable<MorseSubstring> substrings, String sent, String recorded)
        {
            _substringList = new List<MorseSubstring>(substrings);
            _sent = sent;
            _recorded = recorded;
        }

        public String Sent
        {
            get
            {
                return _sent;
            }
        }

        public String Recorded
        {
            get
            {
                return _recorded;
            }
        }

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

    public abstract class MorseSubstring
    {
        protected MorseSubstring() { }
        protected MorseSubstring(String str)
        {
            _str = str;
        }

        public String Chars
        {
            get
            {
                return _str;
            }
        }

        public abstract System.Drawing.Color Color
        {
            get;
        }
        public abstract String Str(ResultsDisplayFlags flags = ResultsDisplayFlags.All);

        protected string _str;
    }

    public class MorseValid : MorseSubstring
    {
        public MorseValid(String str) : base(str)
        {
        }

        public override string Str(ResultsDisplayFlags flags)
        {
            String ret = "";
            if ((flags & ResultsDisplayFlags.Valid) != 0)
            {
                ret = MorseInfo.ExpandProsigns(_str);
            }
            return ret;
        }

        public override Color Color
        {
            get
            {
                return Color.Black;
            }
        }
    }

    public class MorseDropped : MorseSubstring
    {
        public MorseDropped(String str) : base(str)
        {
        }

        public override string Str(ResultsDisplayFlags flags)
        {
            String ret = "";
            if ((flags & ResultsDisplayFlags.Dropped) != 0)
            {
                ret = MorseInfo.ExpandProsigns(_str);
            }
            return ret;
        }

        public override Color Color
        {
            get
            {
                return Color.Red;
            }
        }
    }

    public class MorseExtra : MorseSubstring
    {
        public MorseExtra(String str) : base(str)
        {
        }

        public override string Str(ResultsDisplayFlags flags)
        {
            String ret = "";
            if ((flags & ResultsDisplayFlags.Extra) != 0)
            {
                ret = MorseInfo.ExpandProsigns(_str);
            }
            return ret;
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
