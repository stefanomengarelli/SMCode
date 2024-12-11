/*  ===========================================================================
 *  
 *  File:       CSV.cs
 *  Version:    2.0.0
 *  Date:       March 2024
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2024 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *
 *  SMCode application class: CSV.
 *
 *  ===========================================================================
 */

using System;
using System.Text;

namespace SMCodeSystem
{

    /* */

    /// <summary>SMCode application class: CSV.</summary>
    public partial class SMCode
    {

        /* */

        #region Initialization

        /*  ===================================================================
         *  Initialization
         *  ===================================================================
         */

        /// <summary>Initialize SMSharp CSV management functions.</summary>
        public void InitializeCSV()
        {
            CSVSeparator = ';';
            CSVDelimiter = '"';
            LoadCSVSettings();
        }

        #endregion

        /* */

        #region Properties

        /*  ===================================================================
         *  Properties
         *  ===================================================================
         */

        /// <summary>Get or set CSV delimiter char.</summary>
        public char CSVDelimiter { get; set; }

        /// <summary>Get or set CSV separator char.</summary>
        public char CSVSeparator { get; set; }

        #endregion

        /* */

        #region Methods

        /*  ===================================================================
         *  Methods
         *  ===================================================================
         */

        /// <summary>Return CSV string adding value.</summary>
        public string AddCSV(string _CSV, string _Value)
        {
            _CSV = _CSV.Trim();
            if (_CSV.Length > 0) _CSV += CSVSeparator;
            _Value = _Value.Replace("" + CSVDelimiter, @"\" + CSVDelimiter);
            return _CSV + CSVDelimiter + _Value + CSVDelimiter;
        }

        /// <summary>Return CSV string adding value with delimiter if specified.</summary>
        public string AddCSV(string _CSV, string _Value, bool _Delimiter)
        {
            _CSV = _CSV.Trim();
            if (_CSV.Length > 0) _CSV += CSVSeparator;
            _Value = _Value.Replace("" + CSVDelimiter, @"\" + CSVDelimiter);
            if (_Delimiter) return _CSV + CSVDelimiter + _Value + CSVDelimiter;
            else return _CSV + _Value;
        }

        /// <summary>Return CSV string adding value.</summary>
        public string AddCSV(string _CSV, int _Value)
        {
            _CSV = _CSV.Trim();
            if (_CSV.Length > 0) _CSV += CSVSeparator;
            return _CSV + _Value.ToString();
        }

        /// <summary>Return CSV string adding value.</summary>
        public string AddCSV(string _CSV, double _Value)
        {
            _CSV = _CSV.Trim();
            if (_CSV.Length > 0) _CSV += CSVSeparator;
            return _CSV + _Value.ToString("####################.################").Replace(DecimalSeparator, '.');
        }

        /// <summary>Return CSV string adding value.</summary>
        public string AddCSV(string _CSV, DateTime _Value)
        {
            _CSV = _CSV.Trim();
            if (_CSV.Length > 0) _CSV += CSVSeparator;
            return _CSV + ToStr(_Value, SMDateFormat.iso8601, true);
        }

        /// <summary>Estract first CSV value of string.</summary>
        public string ExtractCSV(ref string _CSV)
        {
            int i = 0;
            char c;
            bool loop = true, quoted, trailing = false;
            StringBuilder r = new StringBuilder();
            _CSV = _CSV.Trim();
            if (_CSV.Length > 0)
            {
                if (_CSV[0] == CSVSeparator)
                {
                    if (_CSV.Length > 1) _CSV = _CSV.Substring(1);
                    else _CSV = "";
                }
                else
                {
                    quoted = _CSV[0] == CSVDelimiter;
                    if (quoted) i++;
                    while (loop && (i < _CSV.Length))
                    {
                        c = _CSV[i];
                        if (trailing)
                        {
                            r.Append(c);
                            trailing = false;
                        }
                        else if (quoted)
                        {
                            if (c == CSVDelimiter) quoted = false;
                            else if (c == TrailingChar) trailing = true;
                            else r.Append(c);
                        }
                        else if (c == CSVDelimiter) quoted = true;
                        else if (c == CSVSeparator)
                        {
                            if (i < _CSV.Length - 1) _CSV = _CSV.Substring(i + 1);
                            else _CSV = "";
                            loop = false;
                        }
                        else r.Append(c);
                    }
                    if (loop) _CSV = "";
                }
            }
            return r.ToString();
        }

        /// <summary>Load CSV settings from default application INI file.</summary>
        public void LoadCSVSettings()
        {
            SMIni ini = new SMIni("", this);
            CSVDelimiter = (ini.ReadString("CSV", "DELIMITER", CSVDelimiter + "").Trim() + ';')[0];
            CSVSeparator = (ini.ReadString("CSV", "SEPARATOR", CSVSeparator + "").Trim() + '"')[0];
        }

        /// <summary>Save CSV settings to default application INI file.</summary>
        public bool SaveCSVSettings()
        {
            SMIni ini = new SMIni("", this);
            ini.WriteString("CSV", "DELIMITER", CSVDelimiter + "");
            ini.WriteString("CSV", "SEPARATOR", CSVSeparator + "");
            return ini.Save();
        }

        #endregion

        /* */

    }

    /* */

}
