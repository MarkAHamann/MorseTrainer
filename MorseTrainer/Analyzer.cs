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
            Write("I sent   : ");
            ResultsDisplayFlags flags = ResultsDisplayFlags.Valid | ResultsDisplayFlags.Dropped;
            foreach (MorseSubstring substring in results.SubStrings)
            {
                Write(substring.Str(flags), substring.Color);
            }
            Write(Environment.NewLine);

            Write("You typed: ");
            flags = ResultsDisplayFlags.Valid | ResultsDisplayFlags.Extra;
            foreach (MorseSubstring substring in results.SubStrings)
            {
                Write(substring.Str(flags), substring.Color);
            }
            Write(Environment.NewLine);

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
