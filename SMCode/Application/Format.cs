/*  ===========================================================================
 *  
 *  File:       Format.cs
 *  Version:    2.0.0
 *  Date:       February 2024
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2024 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *
 *  SMCode application class: format.
 *  
 *  ===========================================================================
 */

namespace SMCode
{

    /* */

    /// <summary>SMCode application class: format.</summary>
    public partial class SMApplication
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
        public string ToStr(string _String, string _Format)
        {
            _Format = _Format.Trim(); 
            if (_Format.Length > 0)
            {
                if ((_Format[0] == '#') || (_Format[0] == '0')) return Val(_String).ToString(_Format);
                else if ((_Format == @"&D") || (_Format == @"&DATE")) return ToStr(ToDate(_String, DateFormat, false));
                else if ((_Format == @"$") || (_Format == @"&CUR")) return Val(_String).ToString(CurrencyFormat);
                else if (_Format == @"&CURNZ") return Val(_String).ToString(CurrencyFormat + ";; ");
                else if ((_Format == @"!") || (_Format == @"&UPPER")) return _String.ToUpper();
                else if (_Format == @"&HM") return Mid(FixTime(_String), 0, 5);
                else if ((_Format == @"&T") || (_Format == @"&HMS") || (_Format == @"&TIME")) return FixTime(_String);
                else if ((_Format == @"&DT") || (_Format == @"&DATETIME")) return ToStr(ToDate(_String, DateFormat, true), true);
                else if (_Format == @"&LOWER") return _String.ToLower();
                else if (_Format == @"&CUR+") return Val(_String).ToString(CurrencyPrecisionFormat);
                else if (_Format == @"&CUR+NZ") return Val(_String).ToString(CurrencyPrecisionFormat + ";; ");
                else if (_Format == @"&QTY") return Val(_String).ToString(QuantityFormat);
                else if (_Format == @"&QTYNZ") return Val(_String).ToString(QuantityFormat + ";; ");
                else if (_Format == @"&QTY+") return Val(_String).ToString(QuantityPrecisionFormat);
                else if (_Format == @"&QTY+NZ") return Val(_String).ToString(QuantityPrecisionFormat + ";; ");
                else if ((_Format == @"&EU") || (_Format == @"&EURO") || (_Format == @"&EUR")
                    || (_Format == @"&USD") || (_Format == @"&DOLLAR")) return Val(_String).ToString("###,###,###,###,##0.00");
                else if ((_Format == @"&EUNZ") || (_Format == @"&EURONZ") || (_Format == @"&EURNZ")
                    || (_Format == @"&USDNZ") || (_Format == @"&DOLLARNZ")) return Val(_String).ToString("###,###,###,###,##0.00;; ");
                else if ((_Format == @"&EUNZ") || (_Format == @"&EURONZ") || (_Format == @"&EURNZ")
                    || (_Format == @"&USDNZ") || (_Format == @"&DOLLARNZ")) return Val(_String).ToString("###,###,###,###,##0.00;; ");
                else return String.Format(@"{0:" + _Format + @"}", _String);
            }
            else return _String; 
        }

        #endregion

        /* */

    }

    /* */

}
