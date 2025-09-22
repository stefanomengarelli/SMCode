/*  ===========================================================================
 *  
 *  File:       Format.cs
 *  Version:    2.0.221
 *  Date:       March 2025
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2025 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *
 *  SMCode application class: format.
 *  
 *  ===========================================================================
 */

using System;

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


        /// <summary>
        /// Returns string formatted by format parameter which can assume special values as:
        /// if start by 0 or # will be formatted as common double value number format string; 
        /// &amp;D, &amp;DATE for date format;
        /// $,&amp;CUR for currency format;
        /// &amp;CURNZ for currency format, empty if zero;
        /// !, &amp;UP, &amp;UPPER for uppercase format; 
        /// &amp;HM for HH:MM time format; 
        /// &amp;T, &amp;HMS, &amp;TIME for HH:MM:SS time format; 
        /// &amp;DT, &amp;DATETIME for datetime format;
        /// &amp;DU, &amp;DURATION for duration format HHHHHH:MM:SS:ZZZ;
        /// &amp;DUS, &amp;DURATIONSEC for duration format HHHHHH:MM:SS;
        /// &amp;DUM, &amp;DURATIONMIN for duration format HHHHHH:MM;
        /// &amp;DUMNZ for duration format HHHHHH:MM or string empty if zero;
        /// &amp;DUC, &amp;DURATIONCENT for duration format HHHHHH.CC;
        /// &amp;DUD, &amp;DURATIONDAY for duration format DDDD.CC;
        /// &amp;LOW, &amp;LOWER for lowercase format;
        /// &amp;CUR+ for currency more accurate format;
        /// &amp;CUR+NZ for currency more accurate format, empty if zero;
        /// &amp;QTY for quantity format;
        /// &amp;QTYNZ for quantity format, empty if zero;
        /// &amp;QTY+ for quantity more accurate format;
        /// &amp;QTY+NZ for quantity more accurate format, empty if zero;
        /// &amp;EU, &amp;EUR, &amp;EURO for euro money format; 
        /// &amp;EUNZ, &amp;EURNZ, &amp;EURONZ for euro money format with empty string if zero; 
        /// &amp;USD, &amp;DOLLAR for US dollar money format; 
        /// &amp;USDNZ, &amp;DOLLARNZ for US dollar money format with empty string if zero.
        /// </summary>
        /// <param name="_String">The string to format.</param>
        /// <param name="_Format">The format to apply.</param>
        /// <returns>The formatted string.</returns>
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

        /// <summary>Return true if format string is a number.</summary>
        public bool FormatIsNumber(string _Format)
        {
            string fmt = _Format.TrimStart(new char[] { ' ', '&' }).TrimEnd().ToUpper();
            if (fmt.Length > 0)
            {
                return (fmt[0] == '#')
                    || (fmt == "INT")
                    || (fmt == "INTNZ")
                    || (fmt == "NZ")
                    || (fmt == "EU")
                    || (fmt == "EUR")
                    || (fmt == "EURO")
                    || (fmt == "EUNZ")
                    || (fmt == "EURNZ")
                    || (fmt == "EURONZ")
                    || (fmt == "USD")
                    || (fmt == "DOLLAR")
                    || (fmt == "CENT")
                    || (fmt == "USDNZ")
                    || (fmt == "DOLLARNZ")
                    || (fmt == "CENTNZ")
                    || (fmt == "$")
                    || fmt.StartsWith("CUR")
                    || fmt.StartsWith("QTY")
                    || (fmt.StartsWith("D") && (fmt != "D") && (fmt != "DT"));
            }
            else return false;
        }

        /// <summary>Return true if format string is a date or datetime.</summary>
        public bool FormatIsDate(string _Format)
        {
            string fmt = _Format.TrimStart(new char[] { ' ', '&' }).TrimEnd().ToUpper();
            if (fmt.Length > 0) return (fmt == "D") || (fmt == "DT");
            else return false;
        }

        #endregion

        /* */

    }

    /* */

}
