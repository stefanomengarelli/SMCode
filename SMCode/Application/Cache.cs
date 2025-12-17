/*  ===========================================================================
 *  
 *  File:       Cache.cs
 *  Version:    2.0.221
 *  Date:       March 2025
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2025 by Stefano Mengarelli - All rights reserved - Use, 
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

        /// <summary>
        /// Return cached value of parameter with id in to section. 
        /// If expire is greater than zero function will return empty string
        /// if value is older than these days.
        /// </summary>
        /// <param name="_Section">The section where the parameter is stored.</param>
        /// <param name="_ParameterId">The ID of the parameter to read.</param>
        /// <param name="_ExpirationDays">The number of days after which the cached value expires.</param>
        /// <returns>Returns the cached value as a string, or an empty string if the value is expired or not found.</returns>
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

        /// <summary>
        /// Write cached value of parameter with id in to section.
        /// </summary>
        /// <param name="_Section">The section where the parameter will be stored.</param>
        /// <param name="_ParameterId">The ID of the parameter to write.</param>
        /// <param name="_Value">The value to be cached.</param>
        /// <returns>Returns true if the value was successfully written, otherwise false.</returns>
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
