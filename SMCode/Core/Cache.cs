/*  ===========================================================================
 *  
 *  File:       Cache.cs
 *  Version:    2.0.21
 *  Date:       May 2024
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2024 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *
 *  SMCode cache values management class.
 *
 *  ===========================================================================
 */

using System;

namespace SMCodeSystem
{

    /* */

    /// <summary>SMCode cache values management class.</summary>
    public partial class SMCode
    {

        /* */

        #region Methods

        /*  ===================================================================
         *  Methods
         *  ===================================================================
         */

        /// <summary>Return cached value of parameter with id in to section. 
        /// If expire is greater than zero function will return empty string
        /// if value is older than these days.</summary>
        public string CacheRead(string _Section, string _ParameterId, int _ExpirationDays)
        {
            string r = @"";
            SMIni ini = new SMIni();
            if (ini.Load(Merge(TempPath, ExecutableName.ToLower() + @"-cache.dat"), InternalPassword))
            {
                if (_ExpirationDays > 0)
                {
                    if (ini.ReadDateTime(_Section, _ParameterId + @"->date", DateTime.MinValue).AddDays(_ExpirationDays) > DateTime.Now)
                    {
                        r = Base64Decode(ini.ReadString(_Section, _ParameterId + @"->value", "").Trim());
                    }
                }
            }
            return r;
        }

        /// <summary>Write cached value of parameter with id in to section.</summary>
        public bool CacheWrite(string _Section, string _ParameterId, string _Value)
        {
            bool r = false;
            SMIni ini = new SMIni();
            if (ini.Load(Merge(TempPath, ExecutableName.ToLower() + @"-cache.dat"), InternalPassword))
            {
                ini.WriteDateTime(_Section, _ParameterId + @"->date", DateTime.Now);
                ini.WriteString(_Section, _ParameterId + @"->value", Base64Encode(_Value));
                r = ini.Save();
            }
            return r;
        }

        #endregion

        /* */

    }

    /* */

}
