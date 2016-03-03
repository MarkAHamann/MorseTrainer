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
using System.Windows.Forms;

namespace MorseTrainer
{
    public class KeyCaptureForm : System.Windows.Forms.Form
    {
        public KeyCaptureForm() : base()
        {
            _inputtingKeys = false;
        }

        /// <summary>
        /// This override will suppress system beeps for the key in the richtextbox
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        protected override bool ProcessKeyPreview(ref Message m)
        {
            if (_inputtingKeys && m.Msg == 0x0100)
            {
                switch ((uint)m.WParam)
                {
                    case 8: // BackSpace
                    case 13: // Enter
                    case 46: // DEL
                        return true;
                    default:
                        break;
                }
            }
            return base.ProcessKeyPreview(ref m);
        }

        /// <summary>
        /// When inputting keys, the form handles all key input
        /// </summary>
        public bool InputtingKeys
        {
            get
            {
                return _inputtingKeys;
            }
            set
            {
                _inputtingKeys = value;
            }
        }

        private bool _inputtingKeys;
    }
}
