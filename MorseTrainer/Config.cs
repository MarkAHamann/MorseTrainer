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
    /// Config is a serializable class that contains configuration information.
    /// </summary>
    [Serializable]
    public class Config
    {
        public static readonly Config Default;
        public const int CurrentVersion = 1;

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

            Default._version = CurrentVersion;
        }

        /// <summary>
        /// Loads the configuration at path. Creates a default if it doesn't exist.
        /// </summary>
        /// <param name="path">Path to the config file</param>
        /// <returns>A Config object</returns>
        public static Config Load(String path)
        {
            Config config = null;

            if (!System.IO.File.Exists(path))
            {
                Config.Save(Config.Default, path);
            }

            System.IO.Stream stream = null;
            try
            {
                stream = System.IO.File.Open(path, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.None);
                System.Runtime.Serialization.Formatters.Binary.BinaryFormatter bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                config = (Config)bf.Deserialize(stream);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }
            return config;
        }

        /// <summary>
        /// Saves a configuration to path
        /// </summary>
        /// <param name="config">A config object</param>
        /// <param name="path">Path to the config file</param>
        public static void Save(Config config, String path)
        {
            System.IO.Stream stream = null;
            try
            {
                stream = System.IO.File.Open(path, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.None);
                System.Runtime.Serialization.Formatters.Binary.BinaryFormatter bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                bf.Serialize(stream, config);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }
        }

        /// <summary>
        /// Deletes the config file at path
        /// </summary>
        /// <param name="path">Path to the config file</param>
        public static void Delete(String path)
        {
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
        }

        private int _version;

        /// <summary>
        /// Gets the version of the Config file
        /// </summary>
        public int Version
        {
            get
            {
                return _version;
            }
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
