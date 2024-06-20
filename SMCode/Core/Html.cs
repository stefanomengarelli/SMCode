/*  ===========================================================================
 *  
 *  File:       Html.cs
 *  Version:    2.0.0
 *  Date:       February 2024
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2024 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *
 *  SMCode application class: HTML.
 *
 *  ===========================================================================
 */

using System;
using System.Text;
using System.Net;

namespace SMCodeSystem
{

    /* */

    /// <summary>SMCode application class: HTML.</summary>
    public partial class SMCode
    {

        /* */

        #region Methods

        /*  ===================================================================
         *  Methods
         *  ===================================================================
         */

        /// <summary>Decode string containing HTML entities in actual characters.</summary>
        public string DecodeHtml(string _String)
        {
            return WebUtility.HtmlDecode(_String);
        }

        /// <summary>Esegue la codifica forte in HTML rimpiazzando i singoli apici con &quot;
        /// e i doppi con &quot;.</summary>
        public string EncodeHtml(string _Value, char _ReplaceSpecialCharsWith = '\0')
        {
            int a, i;
            char[] c;
            StringBuilder r;
            if (_Value == null) return "";
            else if (_Value.Length < 1) return "";
            else
            {
                r = new StringBuilder();
                c = WebUtility.HtmlEncode(_Value).Replace("\"", "&quot;").Replace("'", "&apos;").ToCharArray();
                if (c != null)
                {
                    if (_ReplaceSpecialCharsWith == '\0')
                    {
                        for (i = 0; i < c.Length; i++)
                        {
                            a = Convert.ToInt32(c[i]);
                            if (a > 127) r.AppendFormat("&#{0};", a);
                            else r.Append(c[i]);
                        }
                    }
                    else
                    {
                        for (i = 0; i < c.Length; i++)
                        {
                            a = Convert.ToInt32(c[i]);
                            if (a > 127) r.Append(_ReplaceSpecialCharsWith);
                            else r.Append(c[i]);
                        }
                    }
                    return r.ToString();
                }
                else return "";
            }
        }

        #endregion

        /* */

    }

    /* */

}
