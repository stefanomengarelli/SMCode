/*  ===========================================================================
 *  
 *  File:       Ini.cs
 *  Version:    1.0.0
 *  Date:       March 2024
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2024 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *
 *  SMCode application class: INI file.
 *
 *  ===========================================================================
 */

namespace SMCode
{

    /* */

    /// <summary>SMCode application class: INI file.</summary>
    public partial class SMApplication
    {

        /* */

        #region Methods

        /*  ===================================================================
         *  Methods
         *  ===================================================================
         */

        /// <summary>Restituisce una nuova istanza di file INI.</summary>
        public SMIni NewIni()
        {
            return new SMIni(this);
        }

        /// <summary>Restituisce una nuova istanza di file INI.</summary>
        public SMIni NewIni(string _FileName)
        {
            return new SMIni(this, _FileName);
        }

        /// <summary>Restituisce una nuova istanza di file INI.</summary>
        public SMIni NewIni(string _FileName, string _Password)
        {
            return new SMIni(this, _FileName, _Password);
        }

        /// <summary>Return string value of key at section of file INI specified. Return default value if not found.</summary>
        public string ReadIni(string _FileName, string _Section, string _Key, string _Default)
        {
            string r;
            SMIni ini = new SMIni(this, _FileName);
            ini.WriteDefault = true;
            r = ini.ReadString(_Section, _Key, _Default);
            ini.Save();
            return r;
        }

        /// <summary>Write and save string value of key at section of file INI specified. Return true if succeed.</summary>
        public bool WriteIni(string _FileName, string _Section, string _Key, string _Value)
        {
            SMIni ini = new SMIni(this, _FileName);
            ini.WriteString(_Section, _Key, _Value);
            return ini.Save();
        }

        #endregion

        /* */

    }

    /* */

}
