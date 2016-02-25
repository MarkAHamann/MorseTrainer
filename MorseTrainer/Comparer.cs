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
    /// The Comparer class creates a MorseCompareResults object that shows the
    /// difference between two strings.
    /// </summary>
    public class Comparer
    {
        private static readonly int[] matchLengthThresholdArray = { 5, 3, 2 };

        /// <summary>
        /// Compares two strings and returns the results.
        /// </summary>
        /// <param name="sent">The sent string</param>
        /// <param name="recorded">The string the user entered while listening</param>
        /// <returns>Am objecxt with results</returns>
        public static MorseCompareResults Compare(string sent, string recorded)
        {
            const int SEARCH_AMOUNT = 20;

            int sentStart = 0;
            int recordedStart = 0;
            int sentLength = sent.Length;
            int recordedLength = recorded.Length;
            int sum = 0;
            int sentOffset = 0;
            int recordedOffset = 0;
            int matched = 0;
            int threshold;
            int matchedTotal = 0;

            List<MorseSubstring> substringList = new List<MorseSubstring>();

            // As long as there are unprocessed characters
            while (sentStart < sentLength || recordedStart < recordedLength)
            {
                bool searching = true;
                int sentRemaining = sentLength - sentStart;
                int recordedRemaining = recordedLength - recordedStart;
                int maxSum = sentRemaining + recordedRemaining;

                // start with a higher threshold to filter false positives, then loosen
                for (int thresholdIndex = 0; searching && (thresholdIndex < matchLengthThresholdArray.Length); ++thresholdIndex)
                {
                    // a match is considered a match if it is contains at least 'threshold' contiguous matching characters
                    threshold = matchLengthThresholdArray[thresholdIndex];
                    int searchAmount = Math.Min(SEARCH_AMOUNT, maxSum);

                    // The algorithm here is to match positive offsets on each string
                    // starting from (0,0) (0,1) (1,0) (0,2) (1,1) (2,0) (0,3) (1,2) (2,1) (3,0) (0,4) ...
                    // until (0,searchAmount) ... (searchAmount,0)
                    for (sum = 0; searching && (sum < searchAmount); ++sum)
                    {
                        for (sentOffset = 0; sentOffset <= sum; ++sentOffset)
                        {
                            // get the respective offsets from the start
                            recordedOffset = sum - sentOffset;

                            // number of matching characters starting at the respective offsets
                            matched = matchCount(sent, recorded, sentStart + sentOffset, recordedStart + recordedOffset);

                            // got matched, leave sentOffset for loop
                            if (matched >= threshold)
                            {
                                // found!
                                searching = false;
                                break;
                            }
                        }
                    }
                }

                // At this point we have matched and the offsets
                if (searching)
                {
                    // didn't find a match--punt and just put everything into the mismatch output
                    sentOffset = sentLength - sentStart;
                    recordedOffset = recordedLength - recordedStart;
                }

                // sum > 0 means that there was either extra or dropped characters detected before a substring match
                if (sum > 0)
                {
                    if (sentOffset > 0)
                    {
                        // at least one character was dropped
                        MorseSubstring dropped = new MorseDropped(sent.Substring(sentStart, sentOffset));
                        substringList.Add(dropped);

                        // skip over the dropped characters to the start of the match
                        sentStart += sentOffset;
                    }
                    if (recordedOffset > 0)
                    {
                        // at least one unsent character was detected before a substring match
                        MorseSubstring extra = new MorseExtra(recorded.Substring(recordedStart, recordedOffset));
                        substringList.Add(extra);

                        // skip over the extra characters to the start of the match
                        recordedStart += recordedOffset;
                    }
                }
                if (matched > 0)
                {
                    // there was a match
                    MorseSubstring valid = new MorseValid(sent.Substring(sentStart, matched));
                    substringList.Add(valid);

                    // skip over the match in each string
                    recordedStart += matched;
                    sentStart += matched;

                    // keep track of the matched total
                    matchedTotal += matched;
                }
            }

            MorseCompareResults results = new MorseCompareResults(substringList, sent, recorded);
            return results;
        }

        private static int matchCount(string string1, string string2, int str1Offset, int str2Offset)
        {
            if (str1Offset >= string1.Length || str2Offset >= string2.Length)
            {
                return 0;
            }

            int str1Comparable = string1.Length - str1Offset;
            int str2Comparable = string2.Length - str2Offset;

            int maxCount = Math.Min(str1Comparable, str2Comparable);
            int match = 0;
            for (; match < maxCount; ++match)
            {
                if (string1[str1Offset + match] != string2[str2Offset + match])
                {
                    return match;
                }
            }
            return match;
        }
    }
}
