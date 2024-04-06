/*  ===========================================================================
 *  
 *  File:       Date.cs
 *  Version:    2.0.0
 *  Date:       March 2024
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2024 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *
 *  SMCode application class: date.
 *
 *  ===========================================================================
 */

using System;
using System.Collections.Generic;
using System.IO;

namespace SMCode
{

    /* */

    /// <summary>SMCode application class: date.</summary>
    public partial class SMApplication
    {

        /* */

        #region Properties

        /*  ===================================================================
         *  Properties
         *  ===================================================================
         */

        /// <summary>Get or set default date format.</summary>
        public SMDateFormat DateFormat { get; set; }

        /// <summary>Get or set date default separator char.</summary>
        public char DateSeparator { get; set; }

        /// <summary>Get days names string array.</summary>
        public string[] DaysNames { get; private set; }

        /// <summary>Get days short names string array.</summary>
        public string[] DaysShortNames { get; private set; }

        /// <summary>Get months names string array.</summary>
        public string[] MonthsNames { get; private set; }

        /// <summary>Get months short names string array.</summary>
        public string[] MonthsShortNames { get; private set; }

        /// <summary>Get or set time separator char.</summary>
        public char TimeSeparator { get; set; }

        /// <summary>2 digit year century.</summary>
        public int Year2DigitCentury { get; set; }

        /// <summary>2 digit year leap.</summary>
        public int Year2DigitLeap { get; set; }

        #endregion

        /* */

        #region Initialization

        /*  ===================================================================
         *  Initialization
         *  ===================================================================
         */

        /// <summary>Initialize date and time class environment.</summary>
        public void InitializeDate()
        {
            DateFormat = SMDateFormat.ddmmyyyy;
            DateSeparator = '/';
            DaysNames = new string[] { "Lunedì", "Martedì", "Mercoledì",
            "Giovedì", "Venerdì", "Sabato", "Domenica" };
            DaysShortNames = new string[] { "Lun", "Mar", "Mer", "Gio", "Ven", "Sab", "Dom" };
            MonthsNames = new string[] { "Gennaio", "Febbraio", "Marzo", "Aprile", "Maggio", "Giugno",
            "Luglio", "Agosto", "Settembre", "Ottobre", "Novembre", "Dicembre" };
            MonthsShortNames = new string[] { "Gen", "Feb", "Mar", "Apr", "Mag", "Giu",
            "Lug", "Ago", "Set", "Ott", "Nov", "Dic" };
            TimeSeparator = ':';
            Year2DigitCentury = (DateTime.Today.Year / 100) * 100;
            Year2DigitLeap = (DateTime.Today.Year - 70) % 100;
        }

        #endregion

        /* */

        #region Methods

        /*  ===================================================================
         *  Methods
         *  ===================================================================
         */

        /// <summary>Return date adding months.</summary>
        public DateTime AddMonths(DateTime _DateTime, int _Months)
        {
            int y, m, d, q;
            DateTime z;
            if (Valid(_DateTime))
            {
                y = _DateTime.Year;
                m = _DateTime.Month;
                d = _DateTime.Day;
                q = _Months / 12;
                y += q;
                m += _Months - (q * 12);
                z = LastOfMonth(new DateTime(y, m, 1));
                if (d < z.Day) return new DateTime(y, m, d);
                else return z;
            }
            else return DateTime.MinValue;
        }

        /// <summary>Compare date including time (up to seconds) if specified and return more than zero if date A is greater than date B,
        /// less than zero if date B is greater than date A, zero if date A and date B are the same value.</summary>
        public long Compare(DateTime _DateTimeA, DateTime _DateTimeB, bool _IncludeTime)
        {
            long a, b;
            if (_IncludeTime)
            {
                a = _DateTimeA.Ticks / TimeSpan.TicksPerDay;
                b = _DateTimeB.Ticks / TimeSpan.TicksPerDay;
            }
            else
            {
                a = _DateTimeA.Ticks / TimeSpan.TicksPerSecond;
                b = _DateTimeB.Ticks / TimeSpan.TicksPerSecond;
            }
            return a - b;
        }

        /// <summary>Returns current date value without time.</summary>
        public DateTime Date()
        {
            DateTime d = DateTime.Now;
            return new DateTime(d.Year, d.Month, d.Day, 0, 0, 0);
        }

        /// <summary>Returns date without time of passed value.</summary>
        public DateTime Date(DateTime _Date)
        {
            return new DateTime(_Date.Year, _Date.Month, _Date.Day, 0, 0, 0);
        }

        /// <summary>Convert minutes passed in a string with format HH:MM</summary>
        public string ConvertToHM(int _Minutes)
        {
            if (_Minutes < 0) return "";
            else return ((_Minutes % 1440) / 60).ToString().PadLeft(2, '0') + TimeSeparator + ((_Minutes % 1440) % 60).ToString().PadLeft(2, '0');
        }

        /// <summary>Convert passed string with format HH:MM in minutes.</summary>
        public int ConvertToMinutes(string _HHMM)
        {
            _HHMM = FixTime(_HHMM);
            if (_HHMM.Trim().Length < 1) return -1;
            else return Convert.ToInt32(_HHMM.Substring(0, 2)) * 60 + Convert.ToInt32(_HHMM.Substring(3, 2));
        }

        /// <summary>Returns datetime value represented in string with format and including time 
        /// if specified, or minimum value if is not valid.</summary>
        public DateTime ToDate(string _Value, SMDateFormat _DateFormat, bool _IncludeTime)
        {
            int d, m, y, h, n, s;
            _Value = _Value.Trim();
            if (_Value.Length > 0)
            {
                try
                {
                    if ((_DateFormat == SMDateFormat.ddmmyyyy) || (_DateFormat == SMDateFormat.dmy))
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
            else return DateTime.MinValue;
        }

        /// <summary>Return Returns datetime value with default format represented 
        /// in string or minimum value if fail.</summary>
        public DateTime ToDate(string _Value)
        {
            return ToDate(_Value, DateFormat, true);
        }

        /// <summary>Return Returns datetime value with default format represented 
        /// in string or minimum value if fail.</summary>
        public DateTime ToDate(object _Value)
        {
            if (_Value == null) return DateTime.MinValue;
            else if (_Value is DateTime) return (DateTime)_Value;
            else return ToDate(_Value.ToString());
        }

        /// <summary>Return days occurred between dates.</summary>
        public int ToDays(DateTime _FromDate, DateTime _ToDate)
        {
            try
            {
                if (_ToDate.Ticks < _FromDate.Ticks) return 0;
                else return Convert.ToInt32((_ToDate.Ticks - _FromDate.Ticks) / TimeSpan.TicksPerDay) + 1;
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>Returns day ordinal number of the week (monday=1, sunday=7, ISO 8601).</summary>
        public int DayOfTheWeek(DateTime _DateTime)
        {
            int r = 0;
            if (Valid(_DateTime))
            {
                if (_DateTime.DayOfWeek == DayOfWeek.Monday) r = 1;
                else if (_DateTime.DayOfWeek == DayOfWeek.Tuesday) r = 2;
                else if (_DateTime.DayOfWeek == DayOfWeek.Wednesday) r = 3;
                else if (_DateTime.DayOfWeek == DayOfWeek.Thursday) r = 4;
                else if (_DateTime.DayOfWeek == DayOfWeek.Friday) r = 5;
                else if (_DateTime.DayOfWeek == DayOfWeek.Saturday) r = 6;
                else if (_DateTime.DayOfWeek == DayOfWeek.Sunday) r = 7;
            }
            return r;
        }

        /// <summary>Returns easter date of year.</summary>
        public DateTime Easter(int _Year)
        {
            int m = 24,
                n = 5,
                a = _Year % 19,
                b = _Year % 4,
                c = _Year % 7,
                d = (19 * a + m) % 30,
                e = (2 * b + 4 * c + 6 * d + n) % 7;
            c = 22 + d + e;
            if (c > 31) return new DateTime(_Year, 4, d + e - 9);
            else return new DateTime(_Year, 3, c);
        }

        /// <summary>Return true if date is null or empty (minimum value).</summary>
        public bool Empty(DateTime _DateTime)
        {
            if (_DateTime == null) return true;
            else return _DateTime <= DateTime.MinValue;
        }

        /// <summary>Ritorna true se la stringa passata è nulla, vuota o composta da soli spazi.</summary>
        public bool Empty(DateTime? _Value)
        {
            if (_Value.HasValue) return _Value.Value <= DateTime.MinValue;
            else return true;
        }

        ///<summary>Returns date value with parameters year, month and day.</summary>
        public DateTime Encode(int _Year, int _Month, int _Day)
        {
            try
            {
                return new DateTime(_Year, _Month, _Day);
            }
            catch (Exception ex)
            {
                Error(ex);
                return DateTime.MinValue;
            }
        }

        /// <summary>Returns datetime value with parameters year, month, day, 
        /// hours, minutes, seconds and milliseconds.</summary>
        public DateTime Encode(int _Year, int _Month, int _Day, int _Hours, int _Minutes, int _Seconds, int _Milliseconds)
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

        /// <summary>Returns DateTime value with today date and hours, minutes, seconds and milliseconds.</summary>
        public DateTime Encode(int _Hours, int _Minutes, int _Seconds, int _Milliseconds)
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

        /// <summary>Returns first day of date month.</summary>
        public DateTime FirstOfMonth(DateTime _DataTime)
        {
            if (Valid(_DataTime)) return new DateTime(_DataTime.Year, _DataTime.Month, 1);
            else return DateTime.MinValue;
        }

        /// <summary>Returns first day of date week.</summary>
        public DateTime FirstOfWeek(DateTime _DateTime)
        {
            if (Valid(_DateTime)) return _DateTime.AddDays(1 - DayOfTheWeek(_DateTime));
            else return DateTime.MinValue;
        }

        /// <summary>Returns first day of date year.</summary>
        public DateTime FirstOfYear(DateTime _DateTime)
        {
            if (Valid(_DateTime)) return new DateTime(_DateTime.Year, 1, 1);
            else return DateTime.MinValue;
        }

        /// <summary>Returns string fixed to properly represent date value with 
        /// format and including time as specified, or empty string if is empty 
        /// or represent an invalid date.</summary>
        public string FixDate(string _String, SMDateFormat _Format, bool _IncludeTime)
        {
            return ToStr(ToDate(_String, _Format, _IncludeTime), _Format, _IncludeTime);
        }

        /// <summary>Returns string fixed to properly represent date value without time, 
        /// or empty string if is empty or represent an invalid date.</summary>
        public string FixDate(string _String)
        {
            return FixDate(_String, DateFormat, false);
        }

        /// <summary>Returns string fixed to properly represent time value,
        /// or empty string if is empty or represent an invalid datetime.</summary>
        public string FixTime(string _String)
        {
            return ToTimeStr(ToTime(_String), true, true);
        }

        /// <summary>Returns the date of last day of date month.</summary>
        public DateTime LastOfMonth(DateTime _DateValue)
        {
            if (!Valid(_DateValue)) return DateTime.MinValue;
            else if (_DateValue.Month < 12) return new DateTime(_DateValue.Year, _DateValue.Month + 1, 1, 0, 0, 0).AddDays(-1);
            else return new DateTime(_DateValue.Year, 12, 31, 0, 0, 0);
        }

        /// <summary>Returns the last day of date week.</summary>
        public DateTime LastOfWeek(DateTime _DateValue)
        {
            if (Valid(_DateValue)) return _DateValue.AddDays(7 - DayOfTheWeek(_DateValue));
            else return DateTime.MinValue;
        }

        /// <summary>Returns the last day of date year.</summary>
        public DateTime LastOfYear(DateTime _DateValue)
        {
            if (Valid(_DateValue)) return new DateTime(_DateValue.Year, 12, 31, 0, 0, 0);
            else return DateTime.MinValue;
        }

        /// <summary>Returns true if year is a leap year.</summary>
        public bool LeapYear(int _Year)
        {
            return (_Year % 4 == 0) && ((_Year % 100 != 0) || (_Year % 400 == 0));
        }

        /// <summary>Returns true if date is equal or greater than maximum value.</summary>
        public bool MaxDate(DateTime _DateTime)
        {
            if (_DateTime == null) return false;
            else return _DateTime >= DateTime.MaxValue;
        }

        /// <summary>Returns true if date is null or minimum value.</summary>
        public bool MinDate(DateTime _DateTime)
        {
            if (_DateTime == null) return true;
            else return _DateTime <= DateTime.MinValue;
        }

        /// <summary>Returns months between start date and end date.</summary>
        public int Months(DateTime _StartDate, DateTime _EndDate)
        {
            int r;
            if (_StartDate < _EndDate)
            {
                r = (_EndDate.Year - _StartDate.Year) * 12 + _EndDate.Month - _StartDate.Month;
                if ((r > 0) && (_EndDate.Day < _StartDate.Day)) r--;
                return r;
            }
            else return 0;
        }

        /// <summary>Returns current date and time value.</summary>
        public DateTime Now()
        {
            return DateTime.Now;
        }

        /// <summary>Returns string representing date with specified format.</summary>
        public string ToStr(DateTime _DateTime, SMDateFormat _DateFormat, bool _IncludeTime)
        {
            string r = "";
            if (Valid(_DateTime))
            {
                if (_DateFormat == SMDateFormat.ddmmyyyy) r = Zeroes(_DateTime.Day, 2) + DateSeparator + Zeroes(_DateTime.Month, 2) + DateSeparator + Zeroes(_DateTime.Year, 4);
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

        /// <summary>Return today time value represented in string or minimum value if not valid.</summary>
        public DateTime ToTime(string _String)
        {
            int h, m, s;
            DateTime d;
            _String = _String.Trim();
            if (_String.Length > 0)
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

        /// <summary>Returns true if date is valid (if not null and between min date value and max date value.</summary>
        public bool Valid(DateTime _DateTime)
        {
            if (_DateTime == null) return false;
            else return (_DateTime > DateTime.MinValue) && (_DateTime < DateTime.MaxValue);
        }

        /// <summary>Returns year with 2 digit to 4 fitted to next 30 years or 70 previous year.</summary>
        public int YearFit(int _Year)
        {
            if (_Year > 99) return _Year;
            else if (_Year > Year2DigitLeap) return 2000 + _Year - 100;
            else return Year2DigitCentury + _Year;
        }

        #endregion

        /* */

    }

    /* */

}
