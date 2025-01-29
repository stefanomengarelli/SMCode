/*  ===========================================================================
 *  
 *  File:       IBAN.cs
 *  Version:    2.0.140
 *  Date:       January 2025
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2025 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *
 *                       012345678901234567890123456
 *  IBAN code structure: SSKKCAAAAACCCCCNNNNNNNNNNNN
 *  S = 2 chars nation code
 *  K = 2 chars check digit
 *  C = 1 char CIN
 *  A = 5 chars ABI
 *  C = 5 chars CAB
 *  N = 12 chars account number
 * 
 *  IBAN check functions.
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

        /// <summary>Returns true if the CIN code passed is valid in relation to the ABI, CAB 
        /// and current account number parameters passed. If the passed string is empty, 
        /// it returns the passed value.</summary>
        public string CalculateCIN(string _ABI, string _CAB, string _CC)
        {
            const string numeri = "0123456789";
            //                      
            const string lettere = "ABCDEFGHIJKLMNOPQRSTUVWXYZ-. ";
            const int divisore = 26;
            int[] pari = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28 };
            int[] dispari = { 1, 0, 5, 7, 9, 13, 15, 17, 19, 21, 2, 4, 18, 20, 11, 3, 6, 8, 12, 14, 16, 10, 22, 25, 24, 23, 27, 28, 26 };
            int i, j, k;
            string s, r = "";
            _ABI = _ABI.Trim().ToUpper();
            _CAB = _CAB.Trim().ToUpper();
            _CC = _CC.Trim().ToUpper();
            if (IsDigits(_ABI) && IsDigits(_CAB) && IsAlphaNum(_CC))
            {
                _ABI = Right("00000" + _ABI, 5);
                _CAB = Right("00000" + _CAB, 5);
                _CC = Right("000000000000" + _CC, 12);
                s = _ABI + _CAB + _CC;
                k = 0;
                i = 0;
                while (i < s.Length)
                {
                    j = numeri.IndexOf(s[i]);
                    if (j < 0) j = lettere.IndexOf(s[i]);
                    if (j < 0)
                    {
                        k = -1;
                        i = s.Length;
                    }
                    else
                    {
                        if (i % 2 == 0) k += dispari[j];
                        else k += pari[j];
                    }
                    i++;
                }
                if (k > -1) r = lettere.Substring(k % divisore, 1);
            }
            return r;
        }

        /// <summary>Returns true if the CIN code passed is valid in relation to the ABI, CAB 
        /// and current account number parameters passed. If the passed string is empty, 
        /// it returns the passed value.</summary>
        public bool ValidateCIN(string _CIN, string _ABI, string _CAB, string _CC, bool _ValidateIfEmpty)
        {
            _CIN = _CIN.Trim().ToUpper();
            if (_CIN.Length > 0) return _CIN == CalculateCIN(_ABI, _CAB, _CC);
            else return _ValidateIfEmpty;
        }

        /// <summary>Returns true if the IBAN code passed is valid. If the passed string is empty, 
        /// it returns the passed value.</summary>
        public bool ValidateIBAN(string _IBAN, bool _ValidateIfEmpty)
        {
            bool r;
            int i, j;
            string s;
            string[] c = new string[] { "AL,28", "AD,24", "AT,20","AZ,28", 
                "BE,16", "BH,22", "BA,20", "BR,29", "BG,22", "CR,21", "HR,21", "CY,28", "CZ,24", "DK,18",
                "DO,28", "EE,20", "FO,18", "FI,18", "FR,27", "GE,22", "DE,22", "GI,23", "GR,27", "GL,18", 
                "GT,28", "HU,28", "IS,26", "IE,22", "IL,23", "IT,27", "KZ,20", "KW,30", "LV,21", "LB,28",
                "LI,21", "LT,20", "LU,20", "MK,19", "MT,31", "MR,27", "MU,30", "MC,27", "MD,24", "ME,22",
                "NL,18", "NO,15", "PK,24", "PS,29", "PL,28", "PT,25", "RO,24", "SM,27", "SA,24", "RS,22",
                "SK,24", "SI,19", "ES,24", "SE,24", "CH,21", "TN,24", "TR,26", "AE,23", "GB,22", "VG,24" };
            // test empty code
            if (_IBAN.Length > 0)
            {
                // extract and validate IBAN code chars
                r = true;
                i = 0;
                s = "";
                _IBAN = _IBAN.Trim().ToUpper();
                while (r && (i < _IBAN.Length))
                {
                    if (((_IBAN[i] >= '0') && (_IBAN[i] <= '9'))
                        || ((_IBAN[i] >= 'A') && (_IBAN[i] <= 'Z'))) s += _IBAN[i];
                    else if (_IBAN[i] != ' ') r = false;
                    i++;
                }
                // if chars are valid, check minimum length
                if (r && (s.Length > 2))
                {
                    // validate code length and nation
                    i = 0;
                    j = 0;
                    while ((j==0) && (i<c.Length))
                    {
                        if (_IBAN.Substring(0, 2) == c[i].Substring(0, 2)) j = int.Parse(c[i].Substring(3, 2));
                        i++;
                    }
                    if (j == _IBAN.Length)
                    {
                        // if nation is IT check CIN code
                        if (_IBAN.Substring(0,2)=="IT")
                        {
                            r = ValidateCIN(_IBAN.Substring(4, 1), 
                                _IBAN.Substring(5, 5), 
                                _IBAN.Substring(10, 5), 
                                _IBAN.Substring(15, 12), 
                                false);
                        }
                        if (r)
                        {
                            // recalc code 
                            _IBAN = s.Substring(4, _IBAN.Length - 4) + s.Substring(0, 4);
                            s = "";
                            i = 0;
                            while (i < _IBAN.Length)
                            {
                                if ((_IBAN[i] >= '0') && (_IBAN[i] <= '9')) s += _IBAN[i];
                                else s += (_IBAN[i] - 55).ToString();
                                i++;
                            }
                            // calc 97 module
                            i = 1;
                            j = int.Parse(s.Substring(0, 1));
                            while (i < s.Length)
                            {
                                j *= 10;
                                j += int.Parse(s.Substring(i, 1));
                                j %= 97;
                                i++;
                            }
                            // validate code for module result = 1
                            r = j == 1;
                        }
                    }
                    else r = false;
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
