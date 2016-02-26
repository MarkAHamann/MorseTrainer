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
using System.Media;

namespace MorseTrainer
{
    /// <summary>
    /// The ToneGenerator creates waveforms from the information about frequency,
    /// words per minute, and volume
    /// </summary>
    public class ToneGenerator
    {
        /// <summary>
        /// The minimum frequency in Hertz
        /// </summary>
        public const UInt16 MIN_FREQUENCY = 300;

        /// <summary>
        /// The maximum frequency in Hertz
        /// </summary>
        public const UInt16 MAX_FREQUENCY = 1200;

        /// <summary>
        /// The minimum words per minute
        /// </summary>
        public const float MIN_WPM = 3.0f;

        /// <summary>
        /// The maximum words per minute
        /// </summary>
        public const float MAX_WPM = 40.0f;

        /// <summary>
        /// The minimum Farnsworth words per minute
        /// </summary>
        public const float MIN_FARNSWORTH_WPM = 3.0f;

        /// <summary>
        /// The maximum Farnsworth words per minute
        /// </summary>
        public const float MAX_FARNSWORTH_WPM = 40.0f;

        /// <summary>
        /// The minimum volume 0-1
        /// </summary>
        public const float MIN_VOLUME = 0.0f;

        /// <summary>
        /// The maximum volume 0-1
        /// </summary>
        public const float MAX_VOLUME = 1.0f;

        /// <summary>
        /// The number of samples per second for the waveform
        /// </summary>
        public const Int32 SAMPLES_PER_SECOND = 8000;

        /// <summary>
        /// Creates a new ToneGenerator
        /// </summary>
        public ToneGenerator()
        {
            _frequency = 0;
            _WPM = 0;
            _farnsworthWPM = 0;
            _volume = 0.0f;
            _updateRequired = true;
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

        /// <summary>
        /// Use new values for generating tones
        /// </summary>
        public void Update()
        {
            if (_updateRequired)
            {
                _updateRequired = false;

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
        }

        /// <summary>
        /// The user has made a change that will require an update
        /// </summary>
        public event EventHandler UpdateRequired;
        protected void OnUpdateRequired()
        {
            if (!_updateRequired)
            {
                _updateRequired = true;
                EventHandler handler = UpdateRequired;
                if (handler != null)
                {
                    handler(this, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Gets if there is a required update to generated tones
        /// </summary>
        public bool IsUpdateRequired
        {
            get
            {
                return _updateRequired;
            }
        }

        /// <summary>
        /// Gets the current frequency in Hertz
        /// </summary>
        public UInt16 CurrentFrequency
        {
            get
            {
                return _frequency;
            }
        }

        /// <summary>
        /// Gets or sets the set frequency in Hertz
        /// </summary>
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
                OnUpdateRequired();
            }
        }

        /// <summary>
        /// Gets or sets the set volume (0-1)
        /// </summary>
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
                OnUpdateRequired();
            }
        }

        /// <summary>
        /// Gets the current volume (0-1)
        /// </summary>
        public float CurrentVolume
        {
            get
            {
                return _volume;
            }
        }

        /// <summary>
        /// Gets or sets the set words per minute
        /// </summary>
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
                OnUpdateRequired();
            }
        }

        /// <summary>
        /// Gets the current Farnsworth words per minute
        /// </summary>
        public float CurrentFarnsworthWPM
        {
            get
            {
                return _farnsworthWPM;
            }
        }

        /// <summary>
        /// Gets or sets the set Farnsworth words per minute
        /// </summary>
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
                OnUpdateRequired();
            }
        }

        /// <summary>
        /// Gets the current samples per cycle
        /// </summary>
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

        /// <summary>
        /// Gets the dot waveform array
        /// </summary>
        public Int16[] DotToneWaveform
        {
            get
            {
                return _dotToneWaveform;
            }
        }

        /// <summary>
        /// Gets the dash waveform array
        /// </summary>
        public Int16[] DashToneWaveform
        {
            get
            {
                return _dashToneWaveform;
            }
        }

        /// <summary>
        /// Gets the dot-sized space waveform array
        /// </summary>
        public Int16[] DotSpaceWaveform
        {
            get
            {
                return _dotSpaceWaveform;
            }
        }

        /// <summary>
        /// Gets the letter-sized space waveform array
        /// </summary>
        public Int16[] LetterSpaceWaveform
        {
            get
            {
                return _letterSpaceWaveform;
            }
        }

        /// <summary>
        /// Gets the word-sized space waveform array
        /// </summary>
        public Int16[] WordSpaceWaveform
        {
            get
            {
                return _wordSpaceWaveform;
            }
        }

        /// <summary>
        /// Creates a space waveform array based on the Farnsworth timing
        /// </summary>
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

        private bool _updateRequired;
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
