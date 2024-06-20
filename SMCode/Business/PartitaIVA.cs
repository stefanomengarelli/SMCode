/*  ===========================================================================
 *  
 *  File:       PartitaIVA.cs
 *  Version:    2.0.3
 *  Date:       April 2024
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2024 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *
 *  Italian VAT check functions.
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

        /// <summary>Returns true if the VAT number passed is valid. If the passed string is empty, it returns the passed value.</summary>
        public bool ValidatePartitaIVA(string _PartitaIVA, bool _ValidateIfEmpty)
        {
            bool r;
            int s, j, n;
            _PartitaIVA = _PartitaIVA.Trim().ToUpper();
            if (_PartitaIVA.Length > 0)
            {
                j = 0;
                r = true;
                while (r && (j < _PartitaIVA.Length))
                {
                    if ((_PartitaIVA[j] < '0') || (_PartitaIVA[j] > '9')) r = false;
                    else j++;
                }
                if (r && (_PartitaIVA.Length > 10))
                {
                    s = 0;
                    for (j = 1; j < _PartitaIVA.Length - 1; j += 2)
                    {
                        n = ChrToInt(_PartitaIVA[j]) * 2;
                        s += (n / 10) + (n % 10);
                    }
                    for (j = 0; j < _PartitaIVA.Length - 1; j += 2) s += ChrToInt(_PartitaIVA[j]);
                    r = ChrToInt(_PartitaIVA[_PartitaIVA.Length - 1]) == (10 - (s % 10)) % 10;
                }
                else r = false;
                return r;
            }
            else return _ValidateIfEmpty;
        }

        #endregion

        /* */

    }

    /* */

}
