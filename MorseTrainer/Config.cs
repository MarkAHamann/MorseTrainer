﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MorseTrainer
{
    [Serializable]
    public class Config
    {
        public static readonly Config Default;

        static Config()
        {
            Default = new Config();
            Default._frequency = 1000;
            Default._wpm = 20;
            Default._farnsworthWpm = 13;
            Default._duration = 60;
            Default._startDelay = 2;
            Default._stopDelay = 2;
            Default._volume = 1.0f;

            Default._method = CharGenerator.Method.Koch;
            Default._kochIndex = 1;
            Default._favorNew = true;
            Default._custom = "";
        }

        private UInt16 _frequency;

        /// <summary>
        /// Gets or sets the frequency in Hz
        /// </summary>
        public UInt16 Frequency
        {
            get
            {
                return _frequency;
            }
            set
            {
                _frequency = value;
            }
        }

        private float _wpm;

        /// <summary>
        /// Gets or sets the frequency in WPM in 0.5 increments
        /// </summary>
        public float WPM
        {
            get
            {
                return _wpm;
            }
            set
            {
                _wpm = value;
            }
        }

        private float _farnsworthWpm;

        /// <summary>
        /// Gets or sets the frequency in Farnsworth WPM in 0.5 increments
        /// </summary>
        public float FarnsworthWPM
        {
            get
            {
                return _farnsworthWpm;
            }
            set
            {
                _farnsworthWpm = value;
            }
        }

        private UInt16 _duration;

        /// <summary>
        /// Gets or sets the running duration in seconds increments
        /// </summary>
        public UInt16 Duration
        {
            get
            {
                return _duration;
            }
            set
            {
                _duration = value;
            }
        }

        private UInt16 _startDelay;

        /// <summary>
        /// Gets or sets the start delay in seconds
        /// </summary>
        public UInt16 StartDelay
        {
            get
            {
                return _startDelay;
            }
            set
            {
                _startDelay = value;
            }
        }

        private UInt16 _stopDelay;

        /// <summary>
        /// Gets or sets the stop delay in seconds
        /// </summary>
        public UInt16 StopDelay
        {
            get
            {
                return _stopDelay;
            }
            set
            {
                _stopDelay = value;
            }
        }

        private float _volume;

        /// <summary>
        /// Gets or sets the volume in 0.0f - 1.0f
        /// </summary>
        public float Volume
        {
            get
            {
                return _volume;
            }
            set
            {
                _volume = value;
            }
        }

        private UInt16 _kochIndex;

        /// <summary>
        /// Gets or sets the Koch order index
        /// </summary>
        public UInt16 KochIndex
        {
            get
            {
                return _kochIndex;
            }
            set
            {
                _kochIndex = value;
            }
        }

        private CharGenerator.Method _method;

        /// <summary>
        /// Gets or sets the generation method (Koch or Custom)
        /// </summary>
        public CharGenerator.Method GenerationMethod
        {
            get
            {
                return _method;
            }
            set
            {
                _method = value;
            }
        }

        private String _custom;

        /// <summary>
        /// Gets or sets the custom string generator
        /// </summary>
        public String Custom
        {
            get
            {
                return _custom;
            }
            set
            {
                _custom = value;
            }
        }

        private bool _favorNew;

        /// <summary>
        /// Gets or sets whether to favor newly learned Koch characters
        /// </summary>
        public bool FavorNew
        {
            get
            {
                return _favorNew;
            }
            set
            {
                _favorNew = value;
            }
        }
    }
}
