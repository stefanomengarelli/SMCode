/*  ===========================================================================
 *  
 *  File:       Conversion.cs
 *  Version:    2.0.30
 *  Date:       June 2024
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2024 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *
 *  SMCode application class: conversion.
 *  
 *  ===========================================================================
 */

using Org.BouncyCastle.Crypto.Modes.Gcm;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;

namespace SMCodeSystem
{

    /* */

    /// <summary>SMCode application class: conversion.</summary>
    public partial class SMCode
    {

        /* */

        #region Methods

        /*  ===================================================================
         *  Methods
         *  ===================================================================
         */

        /// <summary>Returns string decoded base64.</summary>
        public string Base64Decode(string _String)
        {
            byte[] b;
            try
            {
                if (_String == null) return "";
                else if (_String.Trim().Length < 1) return "";
                else
                {
                    b = Convert.FromBase64String(_String.Trim());
                    if (b == null) return "";
                    else return ToStr(b);
                }
            }
            catch (Exception ex)
            {
                Error(ex);
                return "";
            }
        }

        /// <summary>Returns string decoded base64.</summary>
        public byte[] Base64DecodeBytes(string _String)
        {
            try
            {
                if (_String == null) return null;
                else if (_String.Trim().Length < 1) return null;
                else return Convert.FromBase64String(_String.Trim());
            }
            catch (Exception ex)
            {
                Error(ex);
                return null;
            }
        }

        /// <summary>Returns string encoded base64.</summary>
        public string Base64Encode(string _String)
        {
            byte[] b;
            try
            {
                if (_String == null) return "";
                else if (_String == "") return "";
                else
                {
                    b = TextEncoding.GetBytes(_String);
                    if (b == null) return "";
                    else return Convert.ToBase64String(b);
                }
            }
            catch (Exception ex)
            {
                Error(ex);
                return "";
            }
        }

        /// <summary>Returns string encoded base64.</summary>
        public string Base64EncodeBytes(byte[] _Bytes)
        {
            try
            {
                if (_Bytes == null) return "";
                else if (_Bytes.Length < 1) return "";
                else return Convert.ToBase64String(_Bytes);
            }
            catch (Exception ex)
            {
                Error(ex);
                return "";
            }
        }

        /// <summary>Returns string from hexdump decoded with password.</summary>
        public string FromHexDump(string _HexDump, string _Password)
        {
            int i, j = 0, h = _HexDump.Length / 2, k = _Password.Length, z = 0;
            byte[] by = new byte[h], p;
            if (k > 0)
            {
                p = TextEncoding.GetBytes(_Password);
                for (i = 0; i < h; i++)
                {
                    try
                    {
                        by[i] = byte.Parse(_HexDump.Substring(i * 2, 2), System.Globalization.NumberStyles.HexNumber);
                    }
                    catch
                    {
                        by[i] = 0;
                    }
                    by[i] = (byte)(by[i] ^ (byte)((p[j] + z) % 256));
                    j++;
                    z += 3;
                    z %= 256;
                    if (j >= k) j = 0;
                }
            }
            else
            {
                for (i = 0; i < h; i++)
                {
                    try
                    {
                        by[i] = byte.Parse(_HexDump.Substring(i * 2, 2), System.Globalization.NumberStyles.HexNumber);
                    }
                    catch
                    {
                        by[i] = 0;
                    }
                }
            }
            return TextEncoding.GetString(by);
        }

        /// <summary>Return string from hex mask and decrypted with password.</summary>
        public string FromHexMask(string _HexMask, string _Password)
        {
            string r, s;
            if (IsHexMask(_HexMask))
            {
                s = FromHexDump(Mid(_HexMask, 1, _HexMask.Length - 3), _Password);
                r = Mid(s, 0, s.Length - 1);
                if (HexSum(r) == s.Substring(s.Length - 1, 1)) return r;
                else return "";
            }
            else return _HexMask;
        }

        /// <summary>Return an array of byte corresponding to hex couple of string.</summary>
        public byte[] FromHexBytes(string _Value)
        {
            int i, h;
            byte[] r = null;
            try
            {
                if (!Empty(_Value))
                {
                    _Value=_Value.Trim();
                    h = _Value.Length / 2;
                    if (h > 0)
                    {
                        r = new byte[h];
                        i = 0;
                        while (i < h)
                        {
                            r[i] = (byte)(int.Parse(_Value.Substring(i * 2, 2), System.Globalization.NumberStyles.HexNumber) % 256);
                            i++;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Error(ex);
                r = null;
            }
            return r;
        }

        /// <summary>Returntrue if type is simply valuable.</summary>
        public bool IsValuableType(Type _Type)
        {
            if (_Type == null) return false;
            else return (_Type == SMDataType.String)
                || (_Type == SMDataType.Boolean)
                || (_Type == SMDataType.DateTime)
                || (_Type == SMDataType.Guid)
                || (_Type == SMDataType.Char)
                || (_Type == SMDataType.Byte)
                || (_Type == SMDataType.SByte)
                || (_Type == SMDataType.Int16)
                || (_Type == SMDataType.Int32)
                || (_Type == SMDataType.Int64)
                || (_Type == SMDataType.UInt16)
                || (_Type == SMDataType.UInt32)
                || (_Type == SMDataType.UInt64)
                || (_Type == SMDataType.Decimal)
                || (_Type == SMDataType.Double)
                || (_Type == SMDataType.Single);
        }

        /// <summary>Return parameters combined as address.</summary>
        public string ToAddress(string _Address, string _Number, string _Zip, string _City, string _Province)
        {
            string r = "";
            if (!Empty(_Address))
            {
                r += _Address.Trim();
                if (!Empty(_Number)) r += ", " + _Number.Trim();
            }
            if (!Empty(_Zip)) r += " " + _Zip;
            if (!Empty(_City))
            {
                if (Empty(r)) r = _City.Trim();
                else r += " " + _City.Trim();
                if (!Empty(_Province))
                {
                    r += " (" + _Province.Trim() + ")";
                }
            }
            return r;
        }

        /// <summary>Returns true if char is one of following chars '1', '+', 'V', 'T', 'S', 'v', 't', 's'.</summary>
        public bool ToBool(char _Char)
        {
            return (_Char == '1') || (_Char == '+')
                || (_Char == 'S') || (_Char == 's')
                || (_Char == 'T') || (_Char == 't')
                || (_Char == 'V') || (_Char == 'v')
                || (_Char == 'Y') || (_Char == 'y');
        }

        /// <summary>Returns true if string has one of true boolean valid chars.</summary>
        public bool ToBool(string _String)
        {
            return ToBool((_String.Trim() + " ")[0]);
        }

        /// <summary>Return true if value is defined and has a true value.</summary>
        public bool ToBool(bool? _Value)
        {
            if (_Value.HasValue) return _Value.Value;
            else return false;
        }

        /// <summary>Return value or default if not defined.</summary>
        public bool ToBool(bool? _Value, bool _Default)
        {
            if (_Value.HasValue) return _Value.Value;
            else return _Default;
        }

        /// <summary>Return boolean value of object.</summary>
        public bool ToBool(object _Value, bool _Default = false)
        {
            try
            {
                if (_Value == null) return _Default;
                else if (_Value == DBNull.Value) return _Default;
                else if (_Value is bool) return (bool)_Value;
                else return ToBool(_Value.ToString());
            }
            catch
            {
                return _Default;
            }
        }

        /// <summary>Return true if value is greater of zero.</summary>
        public bool ToBool(int _Value)
        {
            return _Value > 0;
        }

        /// <summary>Returns "1" if b is true, otherwise returns "0".</summary>
        public string ToBool(bool _BoolValue)
        {
            if (_BoolValue) return "1";
            else return "0";
        }

        /// <summary>Returns true if string has one of true boolean valid chars or null if empty.</summary>
        public bool? ToBoolNull(string _Value)
        {
            if (Empty(_Value)) return null;
            else return ToBool(_Value);
        }

        /// <summary>Returns true if string has one of true boolean valid chars or null if empty.</summary>
        public string ToBoolNull(string _Value, string _NullValue)
        {
            if (Empty(_Value)) return _NullValue;
            else return ToBool(ToBool(_Value));
        }

        /// <summary>Return bytes array from object or null if not defined.</summary>
        public byte[] ToBytes(object _Value)
        {
            if (_Value == null) return null;
            else if (_Value == DBNull.Value) return null;
            else if (_Value is string) return Base64DecodeBytes(_Value.ToString());
            else return (byte[])_Value;
        }

        /// <summary>Return char representing object value or default value if null.</summary>
        public char ToChar(object _Value, char _Default = ' ')
        {
            return (ToStr(_Value, "" + _Default) + ' ')[0];
        }

        /// <summary>Return class definition C# code from object value.</summary>
        public string ToClassCode(object _Value, string _ClassName = null, bool _Summary = true, bool _ReadOnlyProperties = false, bool _WriteOnlyProperties = false)
        {
            int i;
            Type type;
            StringBuilder rslt = new StringBuilder();
            PropertyInfo property;
            PropertyInfo[] properties;
            if (_Value != null)
            {
                type = _Value.GetType();
                if (_ClassName == null) _ClassName = type.Name;
                properties = type.GetProperties();
                if (properties != null)
                {
                    if (_Summary) rslt.Append("/// <summary>Class definition for " + _ClassName + ".</summary>\r\n");
                    rslt.Append("public class " + _ClassName + "\r\n{\r\n");
                    for (i = 0; i < properties.Length; i++)
                    {
                        property = properties[i];
                        if (property.CanRead && property.CanWrite)
                        {
                            if (_Summary) rslt.Append("\t/// <summary>" + property.Name + " property.</summary>\r\n");
                            rslt.Append("\tpublic " + property.PropertyType.Name + " " + property.Name + " { get; set; }\r\n");
                        }
                        else if (property.CanRead && _ReadOnlyProperties)
                        {
                            if (_Summary) rslt.Append("\t/// <summary>" + property.Name + " read-only property.</summary>\r\n");
                            rslt.Append("\tpublic " + property.PropertyType.Name + " " + property.Name + " { get; }\r\n");
                        }
                        else if (property.CanWrite && _WriteOnlyProperties)
                        {
                            if (_Summary) rslt.Append("\t/// <summary>" + property.Name + " write-only property.</summary>\r\n");
                            rslt.Append("\tpublic " + property.PropertyType.Name + " " + property.Name + " { set; }\r\n");
                        }
                    }
                    rslt.Append("}\r\n");
                }
            }
            return rslt.ToString();
        }

        /// <summary>Returns datetime value with parameters year, month, day or 
        /// if specified also hours, minutes, seconds and milliseconds.</summary>
        public DateTime ToDate(DateTime _Value, bool _IncludeTime = false)
        {
            try
            {
                if (_IncludeTime) return new DateTime(_Value.Year, _Value.Month, _Value.Day,
                    _Value.Hour, _Value.Minute, _Value.Second, _Value.Millisecond);
                else return new DateTime(_Value.Year, _Value.Month, _Value.Day);
            }
            catch (Exception ex)
            {
                Error(ex);
                return DateTime.MinValue;
            }
        }

        /// <summary>Returns datetime value with parameters year, month, day, 
        /// hours, minutes, seconds and milliseconds.</summary>
        public DateTime ToDate(int _Year, int _Month, int _Day, int _Hours = 0, int _Minutes = 0, int _Seconds = 0, int _Milliseconds = 0)
        {
            try
            {
                return new DateTime(_Year, _Month, _Day, _Hours, _Minutes, _Seconds, _Milliseconds);
            }
            catch (Exception ex)
            {
                Error(ex);
                return DateTime.MinValue;
            }
        }

        /// <summary>Returns datetime value represented in string with format and including time 
        /// if specified, or minimum value if is not valid.</summary>
        public DateTime ToDate(string _Value, SMDateFormat _DateFormat, bool _IncludeTime = false)
        {
            int d, m, y, h, n, s;
            _Value = _Value.Trim();
            if (Empty(_Value)) return DateTime.MinValue;
            else
            {
                try
                {
                    _Value = _Value.Trim();
                    if (_DateFormat == SMDateFormat.auto)
                    {
                        try
                        {
                            DateTime r;
                            if (DateTime.TryParse(_Value, out r)) return ToDate(r, _IncludeTime);
                            else return DateTime.MinValue;
                        }
                        catch (Exception ex)
                        {
                            Error(ex);
                            return DateTime.MinValue;
                        }
                    }
                    else if ((_DateFormat == SMDateFormat.ddmmyyyy) || (_DateFormat == SMDateFormat.dmy))
                    {
                        try { d = Convert.ToInt32(ExtractDigits(ref _Value, 2)); } catch { d = 0; }
                        try { m = Convert.ToInt32(ExtractDigits(ref _Value, 2)); } catch { m = 0; }
                        try { y = YearFit(Convert.ToInt32(ExtractDigits(ref _Value, 4))); } catch { y = 0; }
                    }
                    else if ((_DateFormat == SMDateFormat.mmddyyyy) || (_DateFormat == SMDateFormat.mdy))
                    {
                        try { m = Convert.ToInt32(ExtractDigits(ref _Value, 2)); } catch { m = 0; }
                        try { d = Convert.ToInt32(ExtractDigits(ref _Value, 2)); } catch { d = 0; }
                        try { y = YearFit(Convert.ToInt32(ExtractDigits(ref _Value, 4))); } catch { y = 0; }
                    }
                    else if ((_DateFormat == SMDateFormat.yyyymmdd) || (_DateFormat == SMDateFormat.ymd)
                        || (_DateFormat == SMDateFormat.iso8601) || (_DateFormat == SMDateFormat.compact))
                    {
                        try { y = YearFit(Convert.ToInt32(ExtractDigits(ref _Value, 4))); } catch { y = 0; }
                        try { m = Convert.ToInt32(ExtractDigits(ref _Value, 2)); } catch { m = 0; }
                        try { d = Convert.ToInt32(ExtractDigits(ref _Value, 2)); } catch { d = 0; }
                    }
                    else if (_DateFormat == SMDateFormat.ddmmyy)
                    {
                        try { d = Convert.ToInt32(ExtractDigits(ref _Value, 2)); } catch { d = 0; }
                        try { m = Convert.ToInt32(ExtractDigits(ref _Value, 2)); } catch { m = 0; }
                        try { y = YearFit(Convert.ToInt32(ExtractDigits(ref _Value, 2))); } catch { y = 0; }
                    }
                    else if (_DateFormat == SMDateFormat.mmddyy)
                    {
                        try { m = Convert.ToInt32(ExtractDigits(ref _Value, 2)); } catch { m = 0; }
                        try { d = Convert.ToInt32(ExtractDigits(ref _Value, 2)); } catch { d = 0; }
                        try { y = YearFit(Convert.ToInt32(ExtractDigits(ref _Value, 2))); } catch { y = 0; }
                    }
                    else if (_DateFormat == SMDateFormat.yymmdd)
                    {
                        try { y = YearFit(Convert.ToInt32(ExtractDigits(ref _Value, 2))); } catch { y = 0; }
                        try { m = Convert.ToInt32(ExtractDigits(ref _Value, 2)); } catch { m = 0; }
                        try { d = Convert.ToInt32(ExtractDigits(ref _Value, 2)); } catch { d = 0; }
                    }
                    else return DateTime.MinValue;
                    if (_IncludeTime)
                    {
                        try { h = Convert.ToInt32(ExtractDigits(ref _Value, 2)); } catch { h = 0; }
                        try { n = Convert.ToInt32(ExtractDigits(ref _Value, 2)); } catch { n = 0; }
                        try { s = Convert.ToInt32(ExtractDigits(ref _Value, 2)); } catch { s = 0; }
                        return new DateTime(y, m, d, h, n, s);
                    }
                    else return new DateTime(y, m, d);
                }
                catch (Exception ex)
                {
                    Error(ex);
                    return DateTime.MinValue;
                }
            }
        }

        /// <summary>Return Returns datetime value with default format represented 
        /// in string or minimum value if fail.</summary>
        public DateTime ToDate(string _Value, bool _IncludeTime = false)
        {
            return ToDate(_Value, DateFormat, _IncludeTime);
        }

        /// <summary>Return Returns datetime value with default format represented 
        /// in string or minimum value if fail.</summary>
        public DateTime ToDate(object _Value, bool _IncludeTime = false)
        {
            if (_Value == null) return DateTime.MinValue;
            else if (_Value == DBNull.Value) return DateTime.MinValue;
            else if (_Value is DateTime) return ToDate((DateTime)_Value, _IncludeTime);
            else return ToDate(_Value.ToString(), _IncludeTime);
        }

        /// <summary>Return value date or min value if not defined.</summary>
        public DateTime ToDate(DateTime? _Value, bool _IncludeTime = false)
        {
            if (_Value.HasValue) return ToDate(_Value.Value, _IncludeTime);
            else return DateTime.MinValue;
        }

        /// <summary>Return date value or null if equal to min date.</summary>
        public DateTime? ToDateNull(DateTime _Value, bool _IncludeTime = false)
        {
            if (_Value <= DateTime.MinValue) return null;
            else if (_Value.Year < 1900) return null;
            else return ToDate(_Value, _IncludeTime);
        }

        /// <summary>Return date value or null if empty.</summary>
        public DateTime? ToDateNull(string _Value, bool _IncludeTime = false)
        {
            if (Empty(_Value)) return null;
            else return ToDate(_Value, _IncludeTime);
        }

        /// <summary>Return days occurred between dates.</summary>
        public int ToDays(DateTime _FromDate, DateTime _ToDate)
        {
            try
            {
                _FromDate = ToDate(_FromDate);
                _ToDate = ToDate(_ToDate);
                if (_ToDate.Ticks < _FromDate.Ticks) return 0;
                else return Convert.ToInt32((_ToDate.Ticks - _FromDate.Ticks) / TimeSpan.TicksPerDay) + 1;
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>Return decimal value or 0 if not defined.</summary>
        public decimal ToDecimal(decimal? _Value, decimal _Default = 0)
        {
            if (_Value.HasValue) return _Value.Value;
            else return _Default;
        }

        /// <summary>Return decimal value or 0 if not defined.</summary>
        public decimal ToDecimal(object _Value, decimal _Default = 0)
        {
            return Convert.ToDecimal(ToDouble(_Value, Convert.ToDouble(_Default)));
        }

        /// <summary>Returns double value of number represented in string. Return default value if fail. Same as Val().</summary>
        public double ToDouble(string _String, double _Default = 0.0d)
        {
            try
            {
                if (Empty(_String)) return _Default;
                return Convert.ToDouble(GetDigits(_String.Trim(), true));
            }
            catch
            {
                return _Default;
            }
        }

        /// <summary>Returns double value of number represented in object. Return default value if fail.</summary>
        public double ToDouble(object _Value, double _Default = 0.0d)
        {
            if (_Value == null) return _Default;
            else if (_Value == DBNull.Value) return _Default;
            else if (_Value is double) return (double)_Value;
            else return ToDouble(_Value.ToString());
        }

        /// <summary>Returns value in double precision or zero if not defined.</summary>
        public double ToDouble(decimal? _Value, double _Default = 0)
        {
            if (_Value.HasValue) return Convert.ToDouble(_Value.Value);
            else return _Default;
        }

        /// <summary>Returns hexadecimal string representing integer value with digits.</summary>
        public string ToHex(int _Value, int _Digits)
        {
            if (_Digits < 1) return _Value.ToString("X");
            else return _Value.ToString("X" + _Digits.ToString());
        }

        /// <summary>Return hex dump of bytes.</summary>
        public string ToHexBytes(byte[] _Bytes)
        {
            int i;
            StringBuilder sb = new StringBuilder();
            if (_Bytes != null)
            {
                for (i = 0; i < _Bytes.Length; i++) sb.Append(_Bytes[i].ToString("x2"));
            }
            return sb.ToString();
        }

        /// <summary>Returns hexdump of string coded by password if specified.</summary>
        public string ToHexDump(string _String, string _Password)
        {
            int i, j = 0, k = _Password.Length, z = 0;
            byte b;
            byte[] by = TextEncoding.GetBytes(_String), p = TextEncoding.GetBytes(_Password);
            StringBuilder r = new StringBuilder();
            if (k > 0)
            {
                for (i = 0; i < by.Length; i++)
                {
                    b = (byte)(by[i] ^ (byte)((p[j] + z) % 256));
                    r.Append(b.ToString("X2"));
                    j++;
                    z += 3;
                    z %= 256;
                    if (j >= k) j = 0;
                }
            }
            else for (i = 0; i < by.Length; i++) r.Append(by[i].ToString("X2"));
            return r.ToString();
        }

        /// <summary>Return string as hex masked, encrypted with password and delimited by { }.</summary>
        public string ToHexMask(string _String, string _Password)
        {
            if (!Empty(_String))
            {
                if (IsHexMask(_String)) _String = FromHexMask(_String, _Password);
                _String += HexSum(_String);
                _String = ToHexDump(_String, _Password);
                return "{" + _String + HexSum(_String) + "}";
            }
            else return "";
        }

        /// <summary>Convert minutes passed in a string with format HH[separator]MM (default = time separator)</summary>
        public string ToHM(int _Minutes, char _Separator = '?')
        {
            if (_Separator == '?') _Separator = TimeSeparator;
            if (_Minutes < 0) return "";
            else return ((_Minutes % 1440) / 60).ToString().PadLeft(2, '0')
                    + _Separator + ((_Minutes % 1440) % 60).ToString().PadLeft(2, '0');
        }

        /// <summary>Returns integer value of number represented in string. Return default value if fail.</summary>
        public int ToInt(string _Value, int _Default = 0, bool _Hexadecimal = false)
        {
            string s;
            try
            {
                if (_Value == null) return _Default;
                else
                {
                    s = _Value.Trim();
                    if (s.Length < 1) return _Default;
                    else if (_Hexadecimal || (s[0] == '$') || (s[0] == 'x') || (s[0] == 'X'))
                    {
                        if ("$xX".IndexOf(s[0]) < 0) s = "$" + s;
                        if (s.Length < 2) return 0;
                        else return int.Parse(s.Substring(1), System.Globalization.NumberStyles.HexNumber);
                    }
                    else
                    {
                        s = GetDigits(s, false);
                        return Convert.ToInt32(s);
                    }
                }
            }
            catch
            {
                return _Default;
            }
        }

        /// <summary>Returns integer value of number represented in object. Return default value if fail.</summary>
        public int ToInt(object _Value, int _Default = 0)
        {
            if (_Value == null) return _Default;
            else if (_Value == DBNull.Value) return _Default;
            else if (_Value is int) return (int)_Value;
            else return ToInt(_Value.ToString());
        }

        /// <summary>Returns integer value or default value if not defined.</summary>
        public int ToInt(int? _Value, int _Default = 0)
        {
            if (_Value.HasValue) return _Value.Value;
            else return _Default;
        }

        /// <summary>Return integer represented by boolean value.</summary>
        public int ToInt(bool _Value)
        {
            if (_Value) return 1;
            else return 0;
        }

        /// <summary>Return integer value or null if not defined.</summary>
        public int? ToIntNull(string _Value)
        {
            try
            {
                if (Empty(_Value)) return null;
                else return Int32.Parse(_Value.Trim());
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>Returns long integer value of number represented in string. Return default value if fail.</summary>
        public long ToLong(string _Value, long _Default = 0)
        {
            try
            {
                if (Empty(_Value)) return _Default;
                else return Convert.ToInt64(GetDigits(_Value.Trim(), false));
            }
            catch
            {
                return _Default;
            }
        }

        /// <summary>Returns long integer value of number represented in object. Return default value if fail.</summary>
        public long ToLong(object _Value, long _Default = 0)
        {
            if (_Value == null) return _Default;
            else if (_Value == DBNull.Value) return _Default;
            else if ((_Value is long) || (_Value is int)) return (long)_Value;
            else return ToLong(_Value.ToString());
        }

        /// <summary>Return value of type null conversion.</summary>
        public object ToNullValue(Type _Type)
        {
            if (_Type == null) return null;
            else if (_Type == SMDataType.String) return null;
            else if (_Type == SMDataType.Boolean) return false;
            else if (_Type == SMDataType.DateTime) return System.DateTime.MinValue;
            else if (_Type == SMDataType.TimeSpan) return System.TimeSpan.Zero;
            else if (_Type == SMDataType.Guid) return System.Guid.Empty;
            else if (_Type == SMDataType.Char) return '\0';
            else if (_Type == SMDataType.Byte) return (byte)0;
            else if (_Type == SMDataType.SByte) return (sbyte)0;
            else if (_Type == SMDataType.Int16) return (short)0;
            else if (_Type == SMDataType.Int32) return 0;
            else if (_Type == SMDataType.Int64) return 0;
            else if (_Type == SMDataType.UInt16) return 0;
            else if (_Type == SMDataType.UInt32) return 0;
            else if (_Type == SMDataType.UInt64) return 0;
            else if (_Type == SMDataType.Decimal) return 0.0;
            else if (_Type == SMDataType.Double) return 0.0d;
            else if (_Type == SMDataType.Single) return 0.0;
            else return null;
        }

        /// <summary>Convert passed string with format HH:MM in minutes or default if empty.</summary>
        public int ToMinutes(string _HHMM, int _Default = 0)
        {
            _HHMM = FixTime(_HHMM);
            if (Empty(_HHMM)) return _Default;
            else return Convert.ToInt32(_HHMM.Substring(0, 2)) * 60 + Convert.ToInt32(_HHMM.Substring(3, 2));
        }

        /// <summary>Return string representing integer value.</summary>
        public string ToStr(int _Value)
        {
            return _Value.ToString();
        }

        /// <summary>Returns string representing long integer value.</summary>
        public string ToStr(long _Value)
        {
            return _Value.ToString();
        }

        /// <summary>Return string representing double value.</summary>
        public string ToStr(double _Value)
        {
            return _Value.ToString("###############0.############").Replace('.', DecimalSeparator);
        }

        /// <summary>Return string or default value if null.</summary>
        public string ToStr(string _String, string _Default = "")
        {
            if (_String == null) return _Default;
            else return _String;
        }

        /// <summary>Return string from bytes array. If encoding is UTF8 initial BOM sequence will be removed.</summary>
        public string ToStr(byte[] _BytesArray, Encoding _Encoding = null)
        {
            bool bom;
            if (_BytesArray != null)
            {
                if (_Encoding == null) _Encoding = TextEncoding;
                if (_Encoding == System.Text.Encoding.UTF8)
                {
                    if (_BytesArray.Length > 2) bom = (_BytesArray[0] == 239) && (_BytesArray[1] == 187) && (_BytesArray[2] == 191);
                    else bom = false;
                    if (bom)
                    {
                        if (_BytesArray.Length > 3) return _Encoding.GetString(_BytesArray, 3, _BytesArray.Length - 3);
                        else return "";
                    }
                    else return _Encoding.GetString(_BytesArray);
                }
                else return _Encoding.GetString(_BytesArray);
            }
            else return "";
        }

        /// <summary>Return string with all strings in array separated by specified string or default carriage-return.</summary>
        public string ToStr(string[] _Strings, bool _TrimStrings = false, string _Separator = "\r\n")
        {
            int i;
            StringBuilder r = new StringBuilder();
            if (_Strings != null)
            {
                if (_TrimStrings)
                {
                    for (i = 0; i < _Strings.Length; i++)
                    {
                        r.Append(_Strings[i].Trim());
                        r.Append(_Separator);
                    }
                }
                else
                {
                    for (i = 0; i < _Strings.Length; i++)
                    {
                        r.Append(_Strings[i]);
                        r.Append(_Separator);
                    }
                }
            }
            return r.ToString();
        }

        /// <summary>Return string with all strings in list separated by specified string or default carriage-return.</summary>
        public string ToStr(List<string> _Strings, bool _TrimStrings = false, string _Separator = "\r\n")
        {
            int i;
            StringBuilder r = new StringBuilder();
            if (_Strings != null)
            {
                if (_TrimStrings)
                {
                    for (i = 0; i < _Strings.Count; i++)
                    {
                        r.Append(_Strings[i].Trim());
                        r.Append(_Separator);
                    }
                }
                else
                {
                    for (i = 0; i < _Strings.Count; i++)
                    {
                        r.Append(_Strings[i]);
                        r.Append("\r\n");
                    }
                }
            }
            return r.ToString();
        }

        /// <summary>Returns string representing values in binary array.</summary>
        public string ToStr(bool[] _Array)
        {
            int i;
            string r = "";
            if (_Array != null) for (i = 0; i < _Array.Length; i++) r += ToBool(_Array[i]);
            return r;
        }

        /// <summary>Returns string representing date with specified format.</summary>
        public string ToStr(DateTime _DateTime, SMDateFormat _DateFormat, bool _IncludeTime)
        {
            string r = "";
            if (Valid(_DateTime))
            {
                if (_DateFormat == SMDateFormat.auto)
                {
                    if (_IncludeTime) return _DateTime.ToString();
                    else return _DateTime.ToShortDateString();
                }
                else if (_DateFormat == SMDateFormat.ddmmyyyy) r = Zeroes(_DateTime.Day, 2) + DateSeparator + Zeroes(_DateTime.Month, 2) + DateSeparator + Zeroes(_DateTime.Year, 4);
                else if (_DateFormat == SMDateFormat.mmddyyyy) r = Zeroes(_DateTime.Month, 2) + DateSeparator + Zeroes(_DateTime.Day, 2) + DateSeparator + Zeroes(_DateTime.Year, 4);
                else if (_DateFormat == SMDateFormat.yyyymmdd) r = Zeroes(_DateTime.Year, 4) + DateSeparator + Zeroes(_DateTime.Month, 2) + DateSeparator + Zeroes(_DateTime.Day, 2);
                else if (_DateFormat == SMDateFormat.ddmmyy) r = Zeroes(_DateTime.Day, 2) + DateSeparator + Zeroes(_DateTime.Month, 2) + DateSeparator + Zeroes(_DateTime.Year, 2);
                else if (_DateFormat == SMDateFormat.mmddyy) r = Zeroes(_DateTime.Month, 2) + DateSeparator + Zeroes(_DateTime.Day, 2) + DateSeparator + Zeroes(_DateTime.Year, 2);
                else if (_DateFormat == SMDateFormat.yymmdd) r = Zeroes(_DateTime.Year, 2) + DateSeparator + Zeroes(_DateTime.Month, 2) + DateSeparator + Zeroes(_DateTime.Day, 2);
                else if (_DateFormat == SMDateFormat.dmy) r = _DateTime.Day.ToString() + DateSeparator + _DateTime.Month.ToString() + DateSeparator + _DateTime.Year.ToString();
                else if (_DateFormat == SMDateFormat.mdy) r = _DateTime.Month.ToString() + DateSeparator + _DateTime.Day.ToString() + DateSeparator + _DateTime.Year.ToString();
                else if (_DateFormat == SMDateFormat.ymd) r = _DateTime.Year.ToString() + DateSeparator + _DateTime.Month.ToString() + DateSeparator + _DateTime.Day.ToString();
                else if (_DateFormat == SMDateFormat.iso8601) r = Zeroes(_DateTime.Year, 4) + '-' + Zeroes(_DateTime.Month, 2) + '-' + Zeroes(_DateTime.Day, 2);
                else if (_DateFormat == SMDateFormat.compact) r = Zeroes(_DateTime.Year, 4) + Zeroes(_DateTime.Month, 2) + Zeroes(_DateTime.Day, 2);
                if (_IncludeTime)
                {
                    if (_DateFormat == SMDateFormat.iso8601)
                    {
                        r += 'T' + Zeroes(_DateTime.Hour, 2)
                            + ':' + Zeroes(_DateTime.Minute, 2)
                            + ':' + Zeroes(_DateTime.Second, 2);
                    }
                    else if (_DateFormat == SMDateFormat.compact)
                    {
                        r += Zeroes(_DateTime.Hour, 2)
                            + Zeroes(_DateTime.Minute, 2)
                            + Zeroes(_DateTime.Second, 2);
                    }
                    else
                    {
                        r += ' ' + Zeroes(_DateTime.Hour, 2)
                            + ':' + Zeroes(_DateTime.Minute, 2)
                            + ':' + Zeroes(_DateTime.Second, 2);
                    }
                }
            }
            return r;
        }

        /// <summary>Returns string representing date with default format.</summary>
        public string ToStr(DateTime _DateTime, bool _IncludeTime = false)
        {
            return ToStr(_DateTime, DateFormat, _IncludeTime);
        }

        /// <summary>Return string representing date or empty string not defined.</summary>
        public string ToStr(DateTime? _Value, bool _IncludeTime = false)
        {
            if (_Value.HasValue) return ToStr(_Value.Value, _IncludeTime);
            else return "";
        }

        /// <summary>Return string representing date value or empty string if not defined.</summary>
        public string ToStr(DateTime? _Value, string _Format)
        {
            if (_Value.HasValue) return ToStr((DateTime)_Value, _Format);
            else return "";
        }

        /// <summary>Return string representing date value or empty string if min value.</summary>
        public string ToStr(DateTime _Value, string _Format)
        {
            if (_Value > DateTime.MinValue)
            {
                if (_Format == "") return ToStr(_Value);
                else return _Value.ToString(_Format);
            }
            else return "";
        }

        /// <summary>Return string with char passed or empty string if null.</summary>
        public string ToStr(char? _Value)
        {
            if (_Value.HasValue) return "" + _Value;
            else return "";
        }

        /// <summary>Return string representing object value or default value if null.</summary>
        public string ToStr(object _Value, string _Default = "")
        {
            if (_Value == null) return _Default;
            else if (_Value == DBNull.Value) return _Default;
            else if (_Value is DateTime) return ToStr((DateTime)_Value);
            else if (_Value is byte[]) return Base64EncodeBytes((byte[])_Value);
            else return _Value.ToString();
        }

        /// <summary>Return string containing XML document or empty if fail.</summary>
        public string ToStr(XmlDocument _Document)
        {
            if (_Document != null)
            {
                try
                {
                    StringWriter sw = new StringWriter();
                    XmlTextWriter xw = new XmlTextWriter(sw);
                    xw.Formatting = Formatting.Indented;
                    _Document.Save(xw);
                    return sw.ToString();
                }
                catch (Exception ex)
                {
                    Error(ex);
                    return "";
                }
            }
            else return "";
        }

        /// <summary>Return a list containing all lines of passed string and separated by default carriage-return.</summary>
        public List<string> ToStrList(string _String)
        {
            List<string> r = new List<string>();
            while (_String.Length > 0) r.Add(ExtractLine(ref _String));
            return r;
        }

        /// <summary>Load string list with all lines of passed string and separated by default carriage-return.</summary>
        public void ToStrList(string _String, List<string> _StringList, bool _TrimStrings)
        {
            if (_StringList != null)
            {
                _StringList.Clear();
                if (_TrimStrings)
                {
                    while (_String.Length > 0) _StringList.Add(ExtractLine(ref _String).Trim());
                }
                else
                {
                    while (_String.Length > 0) _StringList.Add(ExtractLine(ref _String));
                }
            }
        }

        /// <summary>Load string list with all lines of passed string and separated by separators and ignoring empty values if setted.</summary>
        public void ToStrList(string _String, List<string> _StringList, string _Separators, bool _TrimStrings, bool _IgnoreEmpty)
        {
            string s;
            if (_StringList != null)
            {
                _StringList.Clear();
                while (_String.Length > 0)
                {
                    s = Extract(ref _String, _Separators);
                    if (_TrimStrings) s = s.Trim();
                    if (!_IgnoreEmpty || (s.Trim().Length > 0)) _StringList.Add(s.Trim());
                }
            }
        }

        /// <summary>Return today time value represented in string or minimum value if not valid.</summary>
        public DateTime ToTime(string _String)
        {
            int h, m, s;
            DateTime d;
            _String = _String.Trim();
            if (!Empty(_String))
            {
                try { h = Convert.ToInt32(ExtractDigits(ref _String, 2)); }
                catch { h = 0; }
                try { m = Convert.ToInt32(ExtractDigits(ref _String, 2)); }
                catch { m = 0; }
                try { s = Convert.ToInt32(ExtractDigits(ref _String, 2)); }
                catch { s = 0; }
                try
                {
                    if ((h < 24) && (m < 60) && (s < 60))
                    {
                        d = DateTime.Now;
                        return new DateTime(d.Year, d.Month, d.Day, h, m, s, 0);
                    }
                    else return DateTime.MinValue;
                }
                catch (Exception ex)
                {
                    Error(ex);
                    return DateTime.MinValue;
                }
            }
            else return DateTime.MinValue;
        }

        /// <summary>Returns DateTime value with today date and hours, minutes, seconds and milliseconds.</summary>
        public DateTime ToTime(int _Hours, int _Minutes = 0, int _Seconds = 0, int _Milliseconds = 0)
        {
            try
            {
                return new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, _Hours, _Minutes, _Seconds, _Milliseconds);
            }
            catch (Exception ex)
            {
                Error(ex);
                return DateTime.MinValue;
            }
        }

        /// <summary>Returns string representing time with default format.</summary>
        public string ToTimeStr(DateTime _DateTime, bool _IncludeSeconds = true, bool _IncludeSeparators = true)
        {
            string r = "";
            if (Valid(_DateTime))
            {
                if (_IncludeSeparators) r = Zeroes(_DateTime.Hour, 2) + TimeSeparator + Zeroes(_DateTime.Minute, 2);
                else r = Zeroes(_DateTime.Hour, 2) + Zeroes(_DateTime.Minute, 2);
                if (_IncludeSeconds)
                {
                    if (_IncludeSeparators) r += TimeSeparator + Zeroes(_DateTime.Second, 2);
                    else r += Zeroes(_DateTime.Second, 2);
                }
            }
            return r;
        }

        /// <summary>Return object converted to specified type or null if not defined.</summary>
        public object ToType(object _Value, Type _Type)
        {
            Type type;
            try
            {
                if (_Value == null) return ToNullValue(_Type);
                else
                {
                    type = _Value.GetType();
                    if (type == _Type) return _Value;
                    else if (_Type == SMDataType.String) return ToStr(_Value);
                    else if (_Type == SMDataType.Boolean) return ToBool(_Value);
                    else if (_Type == SMDataType.DateTime) return ToDate(_Value, true);
                    else if (_Type == SMDataType.Guid) return new Guid(SM.ToStr(_Value));
                    else if (_Type == SMDataType.Char) return ToChar(_Value);
                    else if (_Type == SMDataType.Byte) return Convert.ToByte(ToInt(_Value) % 256);
                    else if (_Type == SMDataType.SByte) return Convert.ToSByte(ToInt(_Value) % 127);
                    else if (_Type == SMDataType.Int16) return Convert.ToInt16(ToInt(_Value));
                    else if (_Type == SMDataType.Int32) return ToInt(_Value);
                    else if (_Type == SMDataType.Int64) return ToLong(_Value);
                    else if (_Type == SMDataType.UInt16) return Convert.ToInt16(ToInt(_Value));
                    else if (_Type == SMDataType.UInt32) return ToInt(_Value);
                    else if (_Type == SMDataType.UInt64) return ToLong(_Value);
                    else if (_Type == SMDataType.Decimal) return ToDecimal(_Value);
                    else if (_Type == SMDataType.Double) return ToDouble(_Value);
                    else if (_Type == SMDataType.Single) return Convert.ToSingle(ToDouble(_Value));
                    else if (_Type == SMDataType.BytesArray) return Base64DecodeBytes(ToStr(_Value));
                    else return Convert.ChangeType(_Value, _Type);
                }
            }
            catch (Exception ex)
            {
                Error(ex);
                return ToNullValue(_Type);
            }
        }

        /// <summary>Return string with all chars invalid for name replaced by specified char (undescore by default).</summary>
        public string ToValidName(string _String, char _ReplaceWith = '_')
        {
            int i;
            string invalidChars = " \\|!\"£$%&/()=?^'[]{}*+§@#°,;.:-<>";
            StringBuilder sb = new StringBuilder();
            if (_String != null)
            {
                for (i = 0; i < _String.Length; i++)
                {
                    if (invalidChars.IndexOf(_String[i]) < 0) sb.Append(_String[i]);
                    else if ((i == 0) && ("0123456789".IndexOf(_String[i]) > -1)) sb.Append(_ReplaceWith);
                    else sb.Append(_ReplaceWith);
                }
            }
            return sb.ToString();
        }

        #endregion

        /* */

    }

    /* */

}
