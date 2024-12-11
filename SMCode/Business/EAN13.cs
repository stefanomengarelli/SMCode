/*  ===========================================================================
 *  
 *  File:       EAN13.cs
 *  Version:    2.0.3
 *  Date:       April 2024
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2024 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *
 *  EAN13 bar code functions.
 *
 *  ===========================================================================
 */

namespace SMCodeSystem
{

    /* */

    public partial class SMCode
    {

        /* */

        #region Methods

        /*  ===================================================================
         *  Methods
         *  ===================================================================
         */

        /// <summary>Return checksum for EAN code.</summary>
        public string EAN13CheckSum(string _EanCode)
        {
            int i = 11, s = 0, t = 0;
            string c = Right("000000000000" + Mid(_EanCode, 0, 12), 12);
            while (i > -1)
            {
                s = s + ToInt("" + c[i]);
                i = i - 2;
            }
            i = 10;
            while (i > -1)
            {
                t = t + ToInt("" + c[i]);
                i = i - 2;
            }
            s = s * 3 + t;
            while (s > 10) s = s - 10;
            return (10 - s).ToString();
        }

        /// <summary>Return EAN code with right checksum.</summary>
        public string EAN13Fix(string _EanCode)
        {
            string r = Right("000000000000" + Mid(_EanCode.Trim(), 0, 12), 12);
            return r + EAN13CheckSum(r);
        }

        /// <summary>Return true if EAN code is valid (with right checksum).</summary>
        public bool ValidateEAN13(string _EanCode)
        {
            bool r = true;
            int i = 0;
            while (r && (i < _EanCode.Length))
            {
                if ((_EanCode[i] < '0') || (_EanCode[i] > '9')) r = false;
                else i++;
            }
            if (r) r = _EanCode == EAN13Fix(_EanCode);
            return r;
        }

        #endregion

        /* */

    }

    /* */

}
