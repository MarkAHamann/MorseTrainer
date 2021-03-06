﻿/*
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
    /// CharGenerator generates words of the expected characters.
    /// </summary>
    public class CharGenerator
    {
        public const int STRING_LENGTH_MIN = 2;
        public const int STRING_LENGTH_MAX = 7;

        /// <summary>
        /// The character generation method used to generate words
        /// </summary>
        public enum Method
        {
            /// <summary>
            /// Uses the Koch order and a Koch index
            /// </summary>
            Koch,

            /// <summary>
            /// Uses a custom string
            /// </summary>
            Custom
        };

        /// <summary>
        /// Creates a new word generator
        /// </summary>
        public CharGenerator()
        {
            _randomizer = new Random();
        }

        /// <summary>
        /// Get or set the generation method
        /// </summary>
        public Method GenerationMethod
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

        /// <summary>
        /// Get or set the index into the Koch order. Must be at least 1
        /// </summary>
        public int KochIndex
        {
            get
            {
                return _kochIndex;
            }
            set
            {
                if (value < 0 || value > Koch.Length)
                {
                    throw new ArgumentException("KochIndex");
                }
                _kochIndex = Math.Max(value, 1);
            }
        }

        /// <summary>
        /// Get or set whether to favor new characters in Koch order
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

        /// <summary>
        /// Gets or sets the custom string for custom. Characters are pulled from
        /// this string randomly in the custom method.
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

        /// <summary>
        /// Creates a random string. The caller is responsible for spaces
        /// </summary>
        /// <returns>A stringf containing characters to send</returns>
        public String CreateRandomString()
        {
            int size = 2 + _randomizer.Next() % (STRING_LENGTH_MAX - STRING_LENGTH_MIN);
            StringBuilder cc = new StringBuilder();
            for (int i = 0; i < size; ++i)
            {
                cc.Append(CreateRandomChar());
            }
            return cc.ToString();
        }

        /// <summary>
        /// Gets a random character
        /// </summary>
        /// <returns>A character</returns>
        private char CreateRandomChar()
        {
            int rangeStart;
            int rangeLength;
            int index;
            char c;
            String s;

            if (_method == Method.Koch)
            {
                rangeStart = 0;
                rangeLength = _kochIndex+1;
                if (_favorNew)
                {
                    // reduce the range to the upper part once in a while
                    if (_randomizer.Next() % 4 == 0)
                    {
                        rangeStart = Math.Max(0, rangeLength - 4);
                        rangeLength -= rangeStart;
                    }
                }
                s = Koch.Order;
            }
            else
            {
                rangeStart = 0;
                rangeLength = _custom.Length;
                s = _custom;
            }
            index = rangeStart + _randomizer.Next() % rangeLength;
            c = s[index];
            return c;
        }

        private Method _method;
        private int _kochIndex;
        private String _custom;
        private bool _favorNew;
        private Random _randomizer;
    }
}
