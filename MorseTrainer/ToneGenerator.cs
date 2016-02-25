using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Media;

namespace MorseTrainer
{
    public class ToneGenerator
    {
        public const UInt16 MIN_FREQUENCY = 300;
        public const UInt16 MAX_FREQUENCY = 1200;
        public const float MIN_WPM = 3.0f;
        public const float MAX_WPM = 40.0f;
        public const float MIN_FARNSWORTH_WPM = 3.0f;
        public const float MAX_FARNSWORTH_WPM = 40.0f;
        public const float MIN_VOLUME = 0.0f;
        public const float MAX_VOLUME = 1.0f;

        public const Int32 SAMPLES_PER_SECOND = 8000;

        public ToneGenerator()
        {
            _frequency = 0;
            _WPM = 0;
            _farnsworthWPM = 0;
            _volume = 0.0f;
        }

        private float WpmToMillesecPerElement(float wpm)
        {
            float millisecPerElement =
                1000.0f /* ms per sec */
                * 60.0f /* Sec / min */
                / wpm
                / 50; /* elem per word */
            return millisecPerElement;
        }

        public void Update()
        {
            // changing tone frequency or WPM will cause the tones to be regenerated
            if (_frequency == 0 || _frequency != _newFrequency || _newWPM != _WPM || _newVolume != _volume)
            {
                _volume = _newVolume;
                _frequency = _newFrequency;
                _WPM = _newWPM;
                // ms per element = WPM * 1000 ms per sec / 60 words per second / 50 elements
                _msPerElement = WpmToMillesecPerElement(_WPM);

                float samplesPerCycle = SAMPLES_PER_SECOND / _frequency;

                _samplesPerCycle = (UInt16)(samplesPerCycle + 0.5);
                CreateTones();
            }
            if (_farnsworthWPM == 0 || _farnsworthWPM != _newFarnsworthWPM)
            {
                _farnsworthWPM = _newFarnsworthWPM;
                _msPerFarnsworthElement = WpmToMillesecPerElement(_farnsworthWPM);
            }
        }

        public UInt16 CurrentFrequency
        {
            get
            {
                return _frequency;
            }
        }

        public UInt16 Frequency
        {
            get
            {
                return _newFrequency;
            }
            set
            {
                if (value < MIN_FREQUENCY || value > MAX_FREQUENCY)
                {
                    throw new ArgumentOutOfRangeException("Frequency");
                }
                _newFrequency = value;
            }
        }

        public float Volume
        {
            get
            {
                return _newVolume;
            }
            set
            {
                if (value < MIN_VOLUME || value > MAX_VOLUME)
                {
                    throw new ArgumentOutOfRangeException("Volume");
                }
                _newVolume = value;
            }
        }
        public float CurrentVolume
        {
            get
            {
                return _volume;
            }
        }

        public float WPM
        {
            get
            {
                return _newWPM;
            }
            set
            {
                if (value < MIN_WPM || value > MAX_WPM)
                {
                    throw new ArgumentOutOfRangeException("WPM");
                }
                _newWPM = (float)Math.Round(value * 2) / 2;
            }
        }

        public float CurrentFarnsworthWPM
        {
            get
            {
                return _farnsworthWPM;
            }
        }

        public float FarnsworthWPM
        {
            get
            {
                return _newFarnsworthWPM;
            }
            set
            {
                if (value < MIN_WPM || value > MAX_WPM)
                {
                    throw new ArgumentOutOfRangeException("WPM");
                }
                _newFarnsworthWPM = (float)Math.Round(value * 2) / 2;
            }
        }

        public UInt32 SamplesPerCycle
        {
            get
            {
                return _samplesPerCycle;
            }
        }

        private void CreateTones()
        {
            System.IO.MemoryStream streamDot = new System.IO.MemoryStream();
            System.IO.MemoryStream streamDash = new System.IO.MemoryStream();

            _dotToneWaveform = CreateTone(_msPerElement);
            _dashToneWaveform = CreateTone(_msPerElement*3);
            _dotSpaceWaveform = CreateSpace(_msPerElement);
            _letterSpaceWaveform = CreateSpace(_msPerElement*3);
            _wordSpaceWaveform = CreateSpace(_msPerElement * 7);
        }

        private Int16[] CreateSpace(float millisec)
        {
            float samples = SAMPLES_PER_SECOND * millisec / 1000;

            UInt32 actualSamples = (UInt32)samples;

            Int16[] waveform = new Int16[actualSamples];

            // Fill in the space
            for (UInt32 sample = 0; sample < actualSamples; ++sample)
            {
                waveform[sample] = 0;
            }

            return waveform;
        }

        private Int16[] CreateTone(float millisec)
        {
            float samples = SAMPLES_PER_SECOND * millisec / 1000;

            // Nyquist check
            if (samples < _samplesPerCycle/2)
            {
                throw new ArgumentException("samples");
            }

            // Create stream
            // Get a number of cyucles to make the tone end at the end of a sinewave to prevent a pop
            UInt32 actualSamples = (UInt32)samples;
            actualSamples = actualSamples - (actualSamples % _samplesPerCycle) + 1;

            Int16[] waveform = new Int16[actualSamples];

            uint fade = _samplesPerCycle;
            // Fill in the tone
            for (UInt32 sample = 0; sample < actualSamples; ++sample)
            {
                float envelope = 1.0f;
                if (sample < fade)
                {
                    envelope = (float)sample / (float)fade;
                }
                else if (sample > actualSamples - fade)
                {
                    envelope = (float)(actualSamples - sample) / (float)fade;
                } 
                float instantaneous = (float)Math.Sin((float)(sample % _samplesPerCycle) / _samplesPerCycle * (2 * Math.PI )) * _volume * envelope;
                waveform[sample] = (Int16)(32767 * instantaneous);
            }

            return waveform;
        }

        public Int16[] DotToneWaveform
        {
            get
            {
                return _dotToneWaveform;
            }
        }

        public Int16[] DashToneWaveform
        {
            get
            {
                return _dashToneWaveform;
            }
        }

        public Int16[] DotSpaceWaveform
        {
            get
            {
                return _dotSpaceWaveform;
            }
        }

        public Int16[] LetterSpaceWaveform
        {
            get
            {
                return _letterSpaceWaveform;
            }
        }

        public Int16[] WordSpaceWaveform
        {
            get
            {
                return _wordSpaceWaveform;
            }
        }

        public Int16[] FarnsworthSpacingWaveform(Char c)
        {
            // Get the equivalent dots
            int elements = MorseInfo.ToElements(c);

            // Farnsworth timing stretches time between characters.
            // Character has already been sent WPM and now we have to
            // wait as if it were sent FarnsworthWPM
            float wpmTime = _msPerElement * elements;
            float farnsworthTime = _msPerFarnsworthElement * elements;
            float difference = farnsworthTime - wpmTime;

            return CreateSpace(difference);
        }

        private UInt16 _frequency;
        private UInt16 _newFrequency;
        private float _WPM;
        private float _newWPM;
        private float _farnsworthWPM;
        private float _newFarnsworthWPM;
        private float _volume;
        private float _newVolume;
        private float _msPerElement;
        private float _msPerFarnsworthElement;
        private UInt32 _samplesPerCycle;
        private Int16[] _dotToneWaveform;
        private Int16[] _dashToneWaveform;
        private Int16[] _dotSpaceWaveform;
        private Int16[] _letterSpaceWaveform;
        private Int16[] _wordSpaceWaveform;
    }
}
