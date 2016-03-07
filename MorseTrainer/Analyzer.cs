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
    public class Analyzer
    {
        public Analyzer(System.Windows.Forms.RichTextBox resultsRTB)
        {
            _resultsRTB = resultsRTB;
        }

        public void Analyze(String sent, String recorded)
        {
            _resultsRTB.Clear();
            MorseCompareResults results = Comparer.Compare(sent, recorded);
            ShowStrings(results);
            ShowStats(results);
        }

        private void ShowStrings(MorseCompareResults results)
        {
            Write("I sent   : ");
            ResultsFlags flags = ResultsFlags.Valid | ResultsFlags.Dropped;
            foreach (MorseSubstring substring in results.SubStrings)
            {
                Write(substring.Str(flags), substring.Color);
            }
            Write(Environment.NewLine);

            Write("You typed: ");
            flags = ResultsFlags.Valid | ResultsFlags.Extra;
            foreach (MorseSubstring substring in results.SubStrings)
            {
                Write(substring.Str(flags), substring.Color);
            }
            Write(Environment.NewLine);
        }

        private void ShowStats(MorseCompareResults results)
        {
            int[] sent = new int[256];
            int[] valid = new int[256];
            int[] dropped = new int[256];
            int[] extra = new int[256];
            int totalValid = 0;

            foreach (char c in results.Sent)
            {
                sent[c]++;
            }
            foreach (MorseSubstring substring in results.SubStrings)
            {
                int[] counter = null;
                switch (substring.ResultInfo & ResultsFlags.All)
                {
                    case ResultsFlags.Dropped:
                        counter = dropped;
                        break;
                    case ResultsFlags.Extra:
                        counter = extra;
                        break;
                    case ResultsFlags.Valid:
                        counter = valid;
                        totalValid += substring.Chars.Length;
                        break;
                }
                if (counter != null)
                {
                    foreach (char c in substring.Chars)
                    {
                        counter[c]++;
                    }
                }
            }

            Write(String.Format("Sent {0} and {1} recorded valid: {2:#}%" + Environment.NewLine, results.Sent.Length, totalValid, 100 * (float)totalValid / (float)results.Sent.Length));
            foreach (char c in MorseInfo.PossibleCharacters)
            {
                if (sent[c] != 0 || extra[c] != 0)
                {
                    Write(String.Format(" {0} {1}/{2} : {3},{4}" + Environment.NewLine, MorseInfo.ExpandProsigns(c), valid[c], sent[c], dropped[c], extra[c]));
                }
            }
        }

        private void Write(String text)
        {
            _resultsRTB.AppendText(text);
        }

        private void Write(String text, System.Drawing.Color color)
        {
            _resultsRTB.SelectionStart = _resultsRTB.TextLength;
            _resultsRTB.SelectionLength = 0;
            _resultsRTB.SelectionColor = color;
            _resultsRTB.AppendText(text);
            _resultsRTB.SelectionColor = _resultsRTB.ForeColor;
        }

        private System.Windows.Forms.RichTextBox _resultsRTB;
    }
}
