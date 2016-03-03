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
