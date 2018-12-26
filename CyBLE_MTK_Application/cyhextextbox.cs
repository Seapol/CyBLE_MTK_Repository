using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using CySmart.Common;

namespace CySmart.GUI.Controls
{
    public partial class CyHexTextBox : TextBox
    {
        #region consts

        const char DEFAULT_SPLIT_CHAR = ' ';
        const char KEY_CHAR_CTRL_A = (char)0x0001;
        const char KEY_CHAR_CTRL_C = (char)0x0003;
        const char KEY_CHAR_CTRL_V = (char)0x0016;

        #endregion

        #region members

        char m_splitChar;

        #endregion

        #region ctor

        public CyHexTextBox()
        {
            m_splitChar = DEFAULT_SPLIT_CHAR;
        }

        #endregion

        #region Public

        #region Split Char

        public char SplitChar
        {
            get { return m_splitChar; }
            set
            {
                if (m_splitChar != value)
                {
                    string text = RemoveFormat(Text);
                    m_splitChar = value;
                    Text = FormatText(text);
                }
            }
        }

        #endregion

        #region IsHexByteStreamValid

        public bool IsHexByteStreamValid
        {
            get
            {
                return IsValidHexStream(FormatText(Text));
            }
        }

        #endregion

        #region GetHexByteStream

        public byte[] GetHexByteStream()
        {
            List<byte> byteStream = new List<byte>();
            if (IsHexByteStreamValid)
            {
                string text = FormatText(Text);
                string[] byteStr = text.Split(new char[] { m_splitChar }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string item in byteStr)
                    byteStream.Add(byte.Parse(item, System.Globalization.NumberStyles.HexNumber));
            }

            return byteStream.ToArray();
        }

        #endregion

        #region Set Hex Byte Stream

        public void SetHexByteStream(params byte[] byteStream)
        {
            Text = String.Empty;
            if ((byteStream != null) && (byteStream.Length > 0))
            {
                StringBuilder builder = new StringBuilder();
                foreach (byte item in byteStream)
                    builder.AppendFormat("{0:X2}", item);

                Text = FormatText(builder.ToString());
            }
        }

        #endregion

        #endregion

        #region Utility

        #region FormatText

        private string RemoveFormat(string input)
        {
            return CyBleCommonUtils.RemoveValueTextFormat(input, m_splitChar);
        }

        private string FormatText(string input)
        {
            return CyBleCommonUtils.FormatValueText(input, m_splitChar);
        }

        #endregion

        #region IsValidHexStream

        private bool IsValidHexStream(string text)
        {
            bool valid = true;
            string[] byteStr = text.Split(new char[] { m_splitChar }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string item in byteStr)
            {
                byte temp;
                if (!Byte.TryParse(item, System.Globalization.NumberStyles.HexNumber, null, out temp))
                {
                    valid = false;
                    break;
                }
            }

            return valid;
        }

        #endregion

        #endregion

        #region overrides

        protected override void OnKeyPress(KeyPressEventArgs e)
        {            
            if (e.KeyChar == KEY_CHAR_CTRL_A)
                SelectAll();

            if (e.KeyChar == (char)Keys.Return)
                OnLostFocus(EventArgs.Empty);

            if ((e.KeyChar != (char)Keys.Back) &&
                (e.KeyChar != KEY_CHAR_CTRL_C) &&
                (e.KeyChar != KEY_CHAR_CTRL_V))
            {
                e.Handled = !Uri.IsHexDigit(e.KeyChar);
            }

            base.OnKeyPress(e);
        }

        protected override void OnLostFocus(EventArgs e)
        {
            string text = FormatText(Text);
            if (IsValidHexStream(text))
                Text = text;

            base.OnLostFocus(e);
        }

        protected override void OnValidated(EventArgs e)
        {
            if (!IsValidHexStream(FormatText(Text)))
            {
                MessageBox.Show("One or more characters in the field is not a valid HEX digit", "Error: Invalid HEX number stream", MessageBoxButtons.OK, MessageBoxIcon.Error);
                SelectAll();
                Focus();
            }

            base.OnValidated(e);
        }

        #endregion
    }
}
