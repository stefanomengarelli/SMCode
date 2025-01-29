/*  ===========================================================================
 *  
 *  File:       CodiceFiscale.cs
 *  Version:    2.0.140
 *  Date:       January 2025
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2025 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *
 *  SMCode Italian TAX code check and calc functions.
 *
 *  ===========================================================================
 */

using System;

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

        /// <summary>Returns the tax code calculated with the parameters passed or an empty string if invalid.</summary>
        public string CodiceFiscale(string _Cognome, string _Nome, string _Sesso, DateTime _DataNascita, string _CodiceComune)
        {
            string r = "";
            if (_CodiceComune.Length > 0)
            {
                _Cognome = CodiceFiscaleCognome(_Cognome);
                if (_Cognome.Length > 0)
                {
                    _Nome = CodiceFiscaleNome(_Nome);
                    if (_Nome.Length > 0)
                    {
                        _Sesso = CodiceFiscaleDate(_DataNascita, _Sesso);
                        if (_Sesso.Length > 0)
                        {
                            r = _Cognome + _Nome + _Sesso + _CodiceComune;
                            r = r + CodiceFiscaleCheck(r);
                        }
                    }
                }
            }
            return r;
        }

        /// <summary>Returns the part of the tax code relating to the surname or an empty string if invalid.</summary>
        public string CodiceFiscaleCognome(string _Cognome) 
        {
            string cons = "", voca = "";
            _Cognome = _Cognome.Trim().ToUpper();
            if (_Cognome.Length>0)
            {
                cons = GetConsonants(_Cognome);
                voca = GetVocals(_Cognome);
                if (voca.Length > 0) return (cons + voca + "XX").Substring(0, 3);
                else return "";
            }
            else return "";
        }

        /// <summary>Returns the part of the tax code relating to the name or an empty string if invalid.</summary>
        public string CodiceFiscaleNome(string _Nome)
        {
            string cons = "", voca = "";
            _Nome = _Nome.Trim().ToUpper();
            if (_Nome.Length > 0)
            {
                cons = GetConsonants(_Nome);
                voca = GetVocals(_Nome);
                if (cons.Length > 3) cons = cons.Substring(0, 1) + cons.Substring(2);
                if (voca.Length > 0) return (cons + voca + "XX").Substring(0, 3);
                else return "";
            }
            else return "";
        }

        /// <summary>Returns the part of the tax code relating to the date of birth and gender or an empty string if invalid.</summary>
        public string CodiceFiscaleDate(DateTime _DataNascita, string _Sesso)
        {
            const string months = "ABCDEHLMPRST";
            string r = "";
            int i;
            _Sesso = _Sesso.Trim().ToUpper();
            if (_DataNascita != null)
            {
                if (_Sesso.Length > 0)
                {
                    r = PadL(_DataNascita.Year.ToString(), 2, '0');
                    r += months[_DataNascita.Month - 1];
                    i = _DataNascita.Day;
                    if (_Sesso[0] == 'F') i += 40;
                    r += PadL(i.ToString(), 2, '0');
                }
            }
            return r;
        }

        /// <summary>Returns the part of the tax code relating to the control code or an empty string if invalid.</summary>
        public string CodiceFiscaleCheck(string _CodiceFiscale)
        {
            int i, sum, c;
            int[] dispari = new int[26] { 1, 0, 5, 7, 9, 13, 15, 17, 19, 21, 2, 4, 18, 20, 11, 3, 6, 8, 12, 14, 16, 10, 22, 25, 24, 23 };
            string r = "";
            _CodiceFiscale = _CodiceFiscale.Trim().ToUpper();
            if (_CodiceFiscale.Length > 14)
            {
                sum = 0;
                for (i = 0; i < 15; i++)
                {
                    c = Asc(_CodiceFiscale[i]);
                    if ((_CodiceFiscale[i] >= '0') && (_CodiceFiscale[i] <= '9')) c += 17;
                    c = c - 65;
                    if (i % 2 != 0) sum += c;
                    else sum += dispari[c];
                }
                sum = sum % 26;
                r = Chr(sum + 65) + "";
            }
            return r;
        }

        /// <summary>Returns the sex of the subject represented by tax code.</summary>
        public string CodiceFiscaleSex(string _CodiceFiscale)
        {
            if (ValidateCodiceFiscale(_CodiceFiscale, false))
            {
                if (_CodiceFiscale[9] > '3') return "F";
                else return "M";
            }
            else return "";
        }

        /// <summary>Returns true if the passed tax code is valid. If the passed string is empty, it returns the passed value.</summary>
        public bool ValidateCodiceFiscale(string _CodiceFiscale, bool _ValidateIfEmpty)
        {
            _CodiceFiscale = _CodiceFiscale.Trim().ToUpper();
            if (_CodiceFiscale.Length > 0)
            {
                if (_CodiceFiscale.Length == 16)
                {
                    return _CodiceFiscale == _CodiceFiscale.Substring(0, 15) + CodiceFiscaleCheck(_CodiceFiscale.Substring(0, 15));
                }
                else return false;
            }
            else return _ValidateIfEmpty;
        }

        #endregion

        /* */

    }

    /* */

}
