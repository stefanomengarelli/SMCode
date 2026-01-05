/*  ===========================================================================
 *  
 *  File:       Rot.cs
 *  Version:    2.0.321
 *  Date:       January 2026
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2026 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *
 *  SMCode application class: string rotation and cypher.
 *
 *  ===========================================================================
 */

using System.Text;

namespace SMCodeSystem
{

    /* */

    /// <summary>SMCode application class.</summary>
    public partial class SMCode
    {

        /* */

        #region Methods

        /*  ===================================================================
         *  Methods
         *  ===================================================================
         */

        /// <summary>Return portion of string from index for length. If portion 
        /// exceed string size, string will be considered in circular mode.</summary>
        public string Rot(string _String, int _Index, int _Length = 1)
        {
            StringBuilder r = new StringBuilder();
            if ((_String != null) && (_Length > 0))
            {
                if (_String.Length > 0)
                {
                    _Index = RotLength(_Index, _String.Length);
                    while (_Length > 0)
                    {
                        _Length--;
                        r.Append(_String[_Index]);
                        _Index++;
                        if (_Index >= _String.Length) _Index = 0;
                    }
                }
            }
            return r.ToString();
        }

        /// <summary>Encrypt string with password applying rotational Caesar cypher algorithm.</summary>
        public string RotEncrypt(string _String, string _Password)
        {
            char c;
            int i, j, k, q, z = 0;
            string a = BaseChars + BaseSymbols + BaseQuotes;
            StringBuilder r = new StringBuilder();
            if (_String != null)
            {
                if (_String.Length > 0)
                {
                    if (_Password == null) r.Append(_String);
                    else if (_Password.Length < 1) r.Append(_String);
                    else
                    {
                        // calculate password base offset
                        z = _Password.Length;
                        for (i = 0; i < _Password.Length; i++) z += i * a.IndexOf(_Password[i]);
                        z = z % a.Length;
                        // encrypting loop
                        j = 0;
                        for (i = 0; i < _String.Length; i++)
                        {
                            c = _String[i];
                            q = a.IndexOf(c);
                            if (q < 0) r.Append(c);
                            else
                            {
                                k = a.IndexOf(_Password[j]);
                                z += k;
                                r.Append(Rot(a, q + z, 1));
                                z += q;
                                //
                                j++;
                                if (j >= _Password.Length) j = 0;
                            }
                        }
                    }
                }
            }
            return r.ToString();
        }

        /// <summary>Decrypt string with password applying rotational Caesar cypher algorithm.</summary>
        public string RotDecrypt(string _String, string _Password)
        {
            char c;
            int i, j, k, q, z = 0;
            string a = BaseChars + BaseSymbols + BaseQuotes;
            StringBuilder r = new StringBuilder();
            if (_String != null)
            {
                if (_String.Length > 0)
                {
                    if (_Password == null) r.Append(_String);
                    else if (_Password.Length < 1) r.Append(_String);
                    else
                    {
                        // calculate password base offset
                        z = _Password.Length;
                        for (i = 0; i < _Password.Length; i++) z += i * a.IndexOf(_Password[i]);
                        z = z % a.Length;
                        // decrypting loop
                        j = 0;
                        for (i = 0; i < _String.Length; i++)
                        {
                            c = _String[i];
                            q = a.IndexOf(c);
                            if (q < 0) r.Append(c);
                            else
                            {
                                k = a.IndexOf(_Password[j]);
                                z += k;
                                c = Rot(a, q - z, 1)[0];
                                r.Append(c);
                                z += a.IndexOf(c);
                                //
                                j++;
                                if (j >= _Password.Length) j = 0;
                            }
                        }
                    }
                }
            }
            return r.ToString();
        }

        /// <summary>Return length normalized with rotational module.</summary>
        public int RotLength(int _Length, int _Module)
        {
            while (_Length < 0) _Length += _Module;
            return _Length % _Module;
        }

        #endregion

        /* */

    }

    /* */

}
