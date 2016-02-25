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
using System.Threading;
using System.Threading.Tasks;

namespace MorseTrainer
{
    /// <summary>
    /// The WordToToneBuilder class builds a tone in a worker
    /// thread and makes it available.
    /// </summary>
    public class WordToToneBuilder
    {

        /// <summary>
        /// Creates a new WordToToneBuilder object that keeps a reference to a
        /// tone generator
        /// </summary>
        /// <param name="toneGenerator">The tone generator</param>
        public WordToToneBuilder(ToneGenerator toneGenerator)
        {
            _toneGenerator = toneGenerator;
        }

        /// <summary>
        /// Starts to build a waveform of the morse code of 'word'
        /// </summary>
        /// <param name="word">A string to convert to a morse code WAV</param>
        /// <param name="callback">A callback that is called when the WAV is ready</param>
        /// <returns></returns>
        public IAsyncResult StartBuildAsync(String word, AsyncCallback callback)
        {
            BuildWaverformAsync asyncResult = new BuildWaverformAsync(word, callback);
            WaitCallback cb = new WaitCallback(MakeWaveform);
            if (System.Threading.ThreadPool.QueueUserWorkItem(cb, asyncResult))
            {
            }
            return asyncResult;
        }

        private void MakeWaveform(object state)
        {
            BuildWaverformAsync buildInfo = (BuildWaverformAsync)state;
            List<Int16[]> soundsList = new List<short[]>();
            _toneGenerator.Update();
            foreach (Char c in buildInfo.Word)
            {
                String morse = MorseInfo.ToMorse(c);
                bool first = true;
                foreach (Char d in morse)
                {
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        soundsList.Add(_toneGenerator.DotSpaceWaveform);
                    }
                    switch (d)
                    {
                        case '.':
                            soundsList.Add(_toneGenerator.DotToneWaveform);
                            break;
                        case '-':
                            soundsList.Add(_toneGenerator.DashToneWaveform);
                            break;
                        case ' ':
                            soundsList.Add(_toneGenerator.WordSpaceWaveform);
                            break;
                    }
                }

                soundsList.Add(_toneGenerator.LetterSpaceWaveform);

                // Farnsworth timing
                if (_toneGenerator.FarnsworthWPM < _toneGenerator.WPM)
                {
                    soundsList.Add(_toneGenerator.FarnsworthSpacingWaveform(c));
                }
            }
            soundsList.Add(_toneGenerator.WordSpaceWaveform);
            WaveStream stream = new WaveStream(buildInfo.Word, soundsList, ToneGenerator.SAMPLES_PER_SECOND, _toneGenerator.SamplesPerCycle);
            buildInfo.SetWaveform(stream);
            buildInfo.Callback();
        }

        private ToneGenerator _toneGenerator;
    }

    /// <summary>
    /// A BuildWaverformAsync object implements the IAsyncResult interface for
    /// WordToToneBuilder.StartBuildAsync
    /// </summary>
    public class BuildWaverformAsync : IAsyncResult
    {
        /// <summary>
        /// Builds the IAsyncResult object for 'word'
        /// </summary>
        /// <param name="word">The word that being converted to morse code</param>
        /// <param name="callback">Callback called when the WAV is ready</param>
        public BuildWaverformAsync(String word, AsyncCallback callback)
        {
            _word = word;
            _callback = callback;
            _waveStream = null;
        }
        
        /// <summary>
        /// Sets the waveform that is the AsyncState object
        /// </summary>
        /// <param name="wavestream"></param>
        public void SetWaveform(WaveStream wavestream)
        {
            _waveStream = wavestream;
        }
        
        /// <summary>
        /// Gets the WAV file or null of it is not ready
        /// </summary>
        public object AsyncState
        {
            get
            {
                return _waveStream;
            }
        }
        
        /// <summary>
        /// Not implemented.
        /// </summary>
        public WaitHandle AsyncWaitHandle
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Always false
        /// </summary>
        public bool CompletedSynchronously
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Returns true if the WAV construction is complete
        /// </summary>
        public bool IsCompleted
        {
            get
            {
                return _waveStream != null;
            }
        }

        /// <summary>
        /// The word that the WAV corresponds to
        /// </summary>
        public String Word
        {
            get
            {
                return _word;
            }
        }

        /// <summary>
        /// Calls the callback
        /// </summary>
        public void Callback()
        {
            if (_callback != null)
            {
                _callback(this);
            }
        }

        private String _word;
        private AsyncCallback _callback;
        private WaveStream _waveStream;
    }
}
