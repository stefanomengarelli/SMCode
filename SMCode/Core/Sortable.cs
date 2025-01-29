/*  ===========================================================================
 *  
 *  File:       Format.cs
 *  Version:    2.0.140
 *  Date:       January 2025
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
using System.Data;

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

        /// <summary>Return sortable string related to format type.</summary>
        public string ToSortable(string _String, string _Format)
        {
            _Format = _Format.Trim();
            if (_Format.Length > 0)
            {
                if ((_Format[0] == '#') || (_Format[0] == '0') || (_Format == @"$") || (_Format == @"&CUR")
                    || (_Format == @"&CURNZ") || (_Format == @"&CUR+") || (_Format == @"&CUR+NZ") || (_Format == @"&QTY")
                    || (_Format == @"&QTYNZ") || (_Format == @"&QTY+") || (_Format == @"&QTY+NZ") || (_Format == @"&EU")
                    || (_Format == @"&EURO") || (_Format == @"&EUR") || (_Format == @"&USD") || (_Format == @"&DOLLAR")
                    || (_Format == @"&EUNZ") || (_Format == @"&EURONZ") || (_Format == @"&EURNZ")
                    || (_Format == @"&USDNZ") || (_Format == @"&DOLLARNZ"))
                {
                    return Val(_String).ToString("0000000000000000.000000000000");
                }
                else if ((_Format == @"&D") || (_Format == @"&DATE"))
                {
                    return ToDate(_String, DateFormat, true).ToString(@"yyyy-MM-ddTHH:mm:ss.fff");
                }
                else return _String;
            }
            else return _String;
        }

        /// <summary>Returns sortable string representing object field content according with data column type.</summary>
        public string ToSortable(DataColumn _DataColumn, object _Object)
        {
            char t = Type(_DataColumn);
            if (t == 'D') return ToSortable(ToDate(_Object.ToString()));
            else if (t == 'N') return ToSortable(Val(_Object.ToString()));
            else return _Object.ToString();
        }

        /// <summary>Returns sortable string representing double value.</summary>
        public string ToSortable(double _Value)
        {
            return _Value.ToString("0000000000000000.000000000000");
        }

        /// <summary>Returns sortable string representing integer value.</summary>
        public string ToSortable(int _Value)
        {
            return _Value.ToString("0000000000000000");
        }

        /// <summary>Returns sortable string representing string value.</summary>
        public string ToSortable(string _Value)
        {
            return _Value;
        }

        /// <summary>Returns sortable string representing datetime value.</summary>
        public string ToSortable(DateTime _Value)
        {
            return _Value.ToString("yyyy-MM-ddThh:mm:ss.fff");
        }

        /// <summary>Return value type related to format type (C=chars, N=number, D=Date, B=Boolean, O=Binary object).</summary>
        public char Type(string _Format)
        {
            _Format = _Format.Trim();
            if (_Format.Length > 0)
            {
                if ((_Format[0] == '#') || (_Format[0] == '0') || (_Format == @"$") || (_Format == @"&CUR")
                    || (_Format == @"&CURNZ") || (_Format == @"&CUR+") || (_Format == @"&CUR+NZ") || (_Format == @"&QTY")
                    || (_Format == @"&QTYNZ") || (_Format == @"&QTY+") || (_Format == @"&QTY+NZ") || (_Format == @"&EU")
                    || (_Format == @"&EURO") || (_Format == @"&EUR") || (_Format == @"&USD") || (_Format == @"&DOLLAR")
                    || (_Format == @"&EUNZ") || (_Format == @"&EURONZ") || (_Format == @"&EURNZ")
                    || (_Format == @"&USDNZ") || (_Format == @"&DOLLARNZ"))
                {
                    return 'N';
                }
                else if ((_Format == @"&D") || (_Format == @"&DATE")) return 'D';
                else return 'C';
            }
            else return 'C';
        }

        /// <summary>Return value type related to data column type (C=chars, N=number, D=Date, B=Boolean, O=Binary object).</summary>
        public char Type(DataColumn _DataColumn)
        {
            if ((_DataColumn.DataType == System.Type.GetType("System.Int32"))
                || (_DataColumn.DataType == System.Type.GetType("System.Double"))
                || (_DataColumn.DataType == System.Type.GetType("System.Byte"))
                || (_DataColumn.DataType == System.Type.GetType("System.Char"))
                || (_DataColumn.DataType == System.Type.GetType("System.Int16"))
                || (_DataColumn.DataType == System.Type.GetType("System.Int64"))
                || (_DataColumn.DataType == System.Type.GetType("System.Decimal"))
                || (_DataColumn.DataType == System.Type.GetType("System.Single"))
                || (_DataColumn.DataType == System.Type.GetType("System.SByte"))
                || (_DataColumn.DataType == System.Type.GetType("System.UInt16"))
                || (_DataColumn.DataType == System.Type.GetType("System.UInt32"))
                || (_DataColumn.DataType == System.Type.GetType("System.UInt64"))) return 'N';
            else if ((_DataColumn.DataType == System.Type.GetType("System.DateTime"))
                || (_DataColumn.DataType == System.Type.GetType("System.TimeSpan"))) return 'D';
            else if (_DataColumn.DataType == System.Type.GetType("System.Boolean")) return 'B';
            else if (_DataColumn.DataType == System.Type.GetType("System.Byte[]")) return 'O';
            else return 'C';            
        }

        #endregion

        /* */

    }

    /* */

}
