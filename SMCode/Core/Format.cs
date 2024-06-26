/*  ===========================================================================
 *  
 *  File:       Format.cs
 *  Version:    2.0.0
 *  Date:       February 2024
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2024 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *
 *  SMCode application class: format.
 *  
 *  ===========================================================================
 */

using System;
using System.Diagnostics.Eventing.Reader;

namespace SMCodeSystem
{

    /* */

    /// <summary>SMCode application class: format.</summary>
    public partial class SMCode
    {

        /* */

        #region Methods

        /*  ===================================================================
         *  Methods
         *  ===================================================================
         */

        /// <summary>Returns string formatted by format parameter wich can assume special values as
        /// if start by 0 or # will be formatted as common double value number format string; 
        /// &amp;D,&amp;DATE for date format;
        /// $,&amp;CUR for currency format;
        /// &amp;CURNZ for currency format, empty if zero;
        /// !,&amp;UPPER for uppercase format; 
        /// &amp;HM for HH:MM time format; 
        /// &amp;T, &amp;HMS, &amp;TIME for HH:MM:SS time format; 
        /// &amp;DT, &amp;DATETIME form datetime format;
        /// &amp;DU, &amp;DURATION form duration format HHHHHH:MM:SS:ZZZ;
        /// &amp;DUS, &amp;DURATIONSEC form duration format HHHHHH:MM:SS;
        /// &amp;DUM, &amp;DURATIONMIN form duration format HHHHHH:MM;
        /// &amp;DUMNZ form duration format HHHHHH:MM or string empty if zero;
        /// &amp;DUC, &amp;DURATIONCENT form duration format HHHHHH.CC;
        /// &amp;DUD, &amp;DURATIONDAY form duration format DDDD.CC;
        /// &amp;LOWER for lowercase format;
        /// &amp;CUR+ for currency more accurate format;
        /// &amp;CUR+NZ for currency more accurate format, empty if zero;
        /// &amp;QTY for quantity format;
        /// &amp;QTYNZ form quantity format, empty if zero;
        /// &amp;QTY+ for quantity more accurate format;
        /// &amp;QTY+NZ form quantity more accurate format, empty if zero;
        /// &amp;EU,&amp;EUR,&amp;EURO for euro money format; 
        /// &amp;EUNZ,&amp;EURNZ,&amp;EURONZ for euro money format with empty string if zero; 
        /// &amp;USD,&amp;DOLLAR for US dollar money format; 
        /// &amp;USDNZ,&amp;DOLLARNZ for US dollar money format with empty string if zero.
        /// </summary>
        public string Format(string _String, string _Format)
        {
            int n;
            string fmt = _Format.TrimStart(new char[] { ' ', '&' }).TrimEnd().ToUpper();
            n = fmt.Length;
            if (n > 0)
            {
                if ((fmt[0] == '#') || (fmt[0] == '0')) return Val(_String).ToString(_Format);
                //
                else if ((fmt == @"D") || (fmt == @"DATE")) return ToStr(ToDate(_String, DateFormat, false));
                else if ((fmt == @"DT") || (fmt == @"DATETIME")) return ToStr(ToDate(_String, DateFormat, true), true);
                else if ((fmt == @"HM") || (fmt == @"HHMM")) return Mid(FixTime(_String), 0, 5);
                else if ((fmt == @"HMS") || (fmt == @"HHMMSS") || (fmt == @"TM") || (fmt == @"TIME")) return FixTime(_String);
                //
                else if ((fmt == @"EU") || (fmt == @"EUR") || (fmt == @"EURO")
                    || (fmt == @"USD") || (fmt == @"DOLLAR") || (fmt == @"CENT")) return Val(_String).ToString("#,###,###,###,###,##0.00");
                else if ((fmt == @"EUNZ") || (fmt == @"EURNZ") || (fmt == @"EURONZ")
                    || (fmt == @"USDNZ") || (fmt == @"DOLLARNZ") || (fmt == @"CENTNZ")) return Val(_String).ToString("#,###,###,###,###,##0.00;; ");
                //
                else if ((fmt == @"$") || (fmt == @"CUR")) return Val(_String).ToString(CurrencyFormat);
                else if (fmt == @"CURNZ") return Val(_String).ToString(CurrencyFormat + ";; ");
                else if (fmt == @"CUR+") return Val(_String).ToString(CurrencyPrecisionFormat);
                else if (fmt == @"CUR+NZ") return Val(_String).ToString(CurrencyPrecisionFormat + ";; ");
                else if (fmt == @"QTY") return Val(_String).ToString(QuantityFormat);
                else if (fmt == @"QTYNZ") return Val(_String).ToString(QuantityFormat + ";; ");
                else if (fmt == @"QTY+") return Val(_String).ToString(QuantityPrecisionFormat);
                else if (fmt == @"QTY+NZ") return Val(_String).ToString(QuantityPrecisionFormat + ";; ");
                //
                else if ((fmt == @"!") || (fmt == @"UP") || (fmt == @"UPPER")) return _String.ToUpper();
                else if ((fmt == @"LOW") || (fmt == @"LOWER")) return _String.ToLower();
                //
                else if (fmt == @"INT") return Int(Val(_String)).ToString("###############0");
                else if (fmt == @"INTNZ") return Int(Val(_String)).ToString("################");
                else if (fmt == @"NZ") return Val(_String).ToString("#,###,###,###,###,###.############;; ");
                // 
                else if (fmt[0] == 'D')
                {
                    if (fmt.StartsWith("DNZ"))
                    {
                        n = ToInt(Mid(fmt, 3, 2));
                        if (n > 0) return Val(_String).ToString("###############0." + Mid("############", 0, n) + ";; ");
                        else return Val(_String).ToString("###############0;; ");
                    }
                    else
                    {
                        n = ToInt(Mid(fmt, 1, 2));
                        if (n > 0) return Val(_String).ToString("###############0." + Mid("############", 0, n));
                        else return Val(_String).ToString("###############0");
                    }
                }
                //
                else return String.Format(@"{0:" + _Format + @"}", _String);
            }
            else return _String; 
        }

        #endregion

        /* */

    }

    /* */

}
