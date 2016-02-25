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
        public WordToToneBuilder(ToneGenerator toneGenerator)
        {
            _toneGenerator = toneGenerator;
        }

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

    public class BuildWaverformAsync : IAsyncResult
    {
        public BuildWaverformAsync(String word, AsyncCallback callback)
        {
            _word = word;
            _callback = callback;
            _waveStream = null;
        }

        public void SetWaveform(WaveStream wavestream)
        {
            _waveStream = wavestream;
        }

        public object AsyncState
        {
            get
            {
                return _waveStream;
            }
        }

        public WaitHandle AsyncWaitHandle
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool CompletedSynchronously
        {
            get
            {
                return false;
            }
        }

        public bool IsCompleted
        {
            get
            {
                return _waveStream != null;
            }
        }

        public String Word
        {
            get
            {
                return _word;
            }
        }

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
