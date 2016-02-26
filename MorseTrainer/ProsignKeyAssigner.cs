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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MorseTrainer
{
    /// <summary>
    /// The ProsignKeyAssigner is a dialog box that the user can use
    /// to specify keys to be used to enter prosigns.
    /// </summary>
    public partial class ProsignKeyAssigner : Form
    {
        public const Char DefaultBT = '\r';
        public const Char DefaultSK = '[';
        public const Char DefaultAR = ']';

        /// <summary>
        /// Create a new prosign key assigner dialog box
        /// </summary>
        public ProsignKeyAssigner()
        {
            InitializeComponent();
            _keyBT = DefaultBT;
            _keySK = DefaultSK;
            _keyAR = DefaultAR;
        }

        private Char DispToChar(String text)
        {
            Char c = '\0';
            if (text.Length > 0)
            {
                if (text == @"\r")
                {
                    c = '\r';
                }
                else
                {
                    c = text[0];
                }
            }
            return c;
        }

        private String CharToDisp(Char c)
        {
            if (c == '\r')
            {
                return @"\r";
            }
            else
            {
                return c.ToString();
            }
        }

        /// <summary>
        /// Gets or sets the key to be used to indicate the BT prosign
        /// </summary>
        public Char Key_BT
        {
            get
            {
                return _keyBT;
            }
            set
            {
                _keyBT = value;
            }
        }

        /// <summary>
        /// Gets or sets the key to be used to indicate the SK prosign
        /// </summary>
        public Char Key_SK
        {
            get
            {
                return _keySK;
            }
            set
            {
                _keySK = value;
            }
        }

        /// <summary>
        /// Gets or sets the key to be used to indicate the AR prosign
        /// </summary>
        public Char Key_AR
        {
            get
            {
                return _keyAR;
            }
            set
            {
                _keyAR = value;
            }
        }

        private void txt_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            if (e.KeyChar == '\r')
            {
                tb.Text = @"\r";
            }
            else if (e.KeyChar >= ' ')
            {
                tb.Text = e.KeyChar.ToString();
            }
            else
            {
                tb.Text = "";
            }
            e.Handled = true;
        }

        private void ProsignKeyAssigner_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible == false)
            {
                _keyBT = DispToChar(txtBT.Text);
                _keySK = DispToChar(txtSK.Text);
                _keyAR = DispToChar(txtAR.Text);
            }
            else
            {
                txtBT.Text = CharToDisp(_keyBT);
                txtSK.Text = CharToDisp(_keySK);
                txtAR.Text = CharToDisp(_keyAR);
            }
        }

        private char _keyBT;
        private char _keySK;
        private char _keyAR;

    }
}
