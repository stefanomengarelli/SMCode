/*  ===========================================================================
 *  
 *  File:       Ini.cs
 *  Version:    2.0.140
 *  Date:       January 2025
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2025 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *
 *  SMCode application class: INI file.
 *
 *  ===========================================================================
 */

namespace SMCodeSystem
{

    /* */

    /// <summary>SMCode application class: INI file.</summary>
    public partial class SMCode
    {

        /* */

        #region Methods

        /*  ===================================================================
         *  Methods
         *  ===================================================================
         */

        /// <summary>Return string value of key at section of file INI specified. Return default value if not found.</summary>
        public string ReadIni(string _FileName, string _Section, string _Key, string _Default)
        {
            string r;
            SMIni ini = new SMIni(_FileName, this);
            ini.WriteDefault = true;
            r = ini.ReadString(_Section, _Key, _Default);
            ini.Save();
            return r;
        }

        /// <summary>Write and save string value of key at section of file INI specified. Return true if succeed.</summary>
        public bool WriteIni(string _FileName, string _Section, string _Key, string _Value)
        {
            SMIni ini = new SMIni(_FileName, this);
            ini.WriteString(_Section, _Key, _Value);
            return ini.Save();
        }

        #endregion

        /* */

    }

    /* */

}
