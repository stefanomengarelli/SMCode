/*  ===========================================================================
 *  
 *  File:       Date.cs
 *  Version:    2.0.300
 *  Date:       October 2025
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2025 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *
 *  SMCode application class: date.
 *
 *  ===========================================================================
 */

using System;

namespace SMCodeSystem
{

    /* */

    /// <summary>SMCode application class: date.</summary>
    public partial class SMCode
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
        /// <param name="_DateTime">The initial date.</param>
        /// <param name="_Months">The number of months to add.</param>
        /// <returns>The resulting date after adding the specified number of months.</returns>
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

        /// <summary>
        /// Compare date including time (up to seconds) if specified and return more than zero if date A is greater than date B,
        /// less than zero if date B is greater than date A, zero if date A and date B are the same value.
        /// </summary>
        /// <param name="_DateTimeA">The first date to compare.</param>
        /// <param name="_DateTimeB">The second date to compare.</param>
        /// <param name="_IncludeTime">Whether to include time in the comparison.</param>
        /// <returns>A long value indicating the comparison result.</returns>
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
        /// <returns>The current date with time set to 00:00:00.</returns>
        public DateTime Date()
        {
            DateTime d = DateTime.Now;
            return new DateTime(d.Year, d.Month, d.Day, 0, 0, 0);
        }

        /// <summary>Returns date without time of passed value.</summary>
        /// <param name="_Date">The date value to process.</param>
        /// <returns>The date with time set to 00:00:00.</returns>
        public DateTime Date(DateTime _Date)
        {
            return new DateTime(_Date.Year, _Date.Month, _Date.Day, 0, 0, 0);
        }

        /// <summary>Returns day ordinal number of the week (monday=1, sunday=7, ISO 8601).</summary>
        /// <param name="_DateTime">The date to process.</param>
        /// <returns>The ordinal number of the day in the week.</returns>
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

        /// <summary>Returns days count between dates.</summary>
        /// <param name="_FromDate">The start date.</param>
        /// <param name="_ToDate">The end date.</param>
        /// <returns>The number of days between the two dates.</returns>
        public int Days(DateTime _FromDate, DateTime _ToDate)
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

        /// <summary>Returns easter date of year.</summary>
        /// <param name="_Year">The year to calculate Easter for.</param>
        /// <returns>The date of Easter for the specified year.</returns>
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

        /// <summary>Returns first day of date month.</summary>
        /// <param name="_DataTime">The date to process.</param>
        /// <returns>The first day of the month for the specified date.</returns>
        public DateTime FirstOfMonth(DateTime _DataTime)
        {
            if (Valid(_DataTime)) return new DateTime(_DataTime.Year, _DataTime.Month, 1);
            else return DateTime.MinValue;
        }

        /// <summary>Returns first day of date week.</summary>
        /// <param name="_DateTime">The date to process.</param>
        /// <returns>The first day of the week for the specified date.</returns>
        public DateTime FirstOfWeek(DateTime _DateTime)
        {
            if (Valid(_DateTime)) return _DateTime.AddDays(1 - DayOfTheWeek(_DateTime));
            else return DateTime.MinValue;
        }

        /// <summary>Returns first day of date year.</summary>
        /// <param name="_DateTime">The date to process.</param>
        /// <returns>The first day of the year for the specified date.</returns>
        public DateTime FirstOfYear(DateTime _DateTime)
        {
            if (Valid(_DateTime)) return new DateTime(_DateTime.Year, 1, 1);
            else return DateTime.MinValue;
        }

        /// <summary>Returns string fixed to properly represent date value with 
        /// format and including time as specified, or empty string if is empty 
        /// or represent an invalid date.</summary>
        /// <param name="_String">The date string to process.</param>
        /// <param name="_Format">The date format to use.</param>
        /// <param name="_IncludeTime">Whether to include time in the output.</param>
        /// <returns>The formatted date string.</returns>
        public string FixDate(string _String, SMDateFormat _Format, bool _IncludeTime)
        {
            return ToStr(ToDate(_String, _Format, _IncludeTime), _Format, _IncludeTime);
        }

        /// <summary>Returns string fixed to properly represent date value without time, 
        /// or empty string if is empty or represent an invalid date.</summary>
        /// <param name="_String">The date string to process.</param>
        /// <returns>The formatted date string without time.</returns>
        public string FixDate(string _String)
        {
            return FixDate(_String, DateFormat, false);
        }

        /// <summary>Returns string fixed to properly represent time value,
        /// or empty string if is empty or represent an invalid datetime.</summary>
        /// <param name="_String">The time string to process.</param>
        /// <returns>The formatted time string.</returns>
        public string FixTime(string _String)
        {
            return ToTimeStr(ToTime(_String), true, true);
        }

        /// <summary>Return true if string in date has format yyyy-mm-dd</summary>
        public bool IsISODate(string _Value)
        {
            int v;
            string s, sep = "-/. ";
            if (_Value != null)
            {
                _Value = _Value.Trim();
                if (_Value.Length > 0)
                {
                    if (sep.IndexOf(DateSeparator) < 0) sep += DateSeparator;
                    s = Extract(ref _Value, sep);
                    if ((s.Length == 4) && SM.IsDigits(s))
                    {
                        s = Extract(ref _Value, sep);
                        v = SM.ToInt(s);
                        if ((v > 0) && (v < 13))
                        {
                            s = Extract(ref _Value, sep);
                            v = SM.ToInt(s);
                            return (v > 0) && (v < 32);
                        }
                    }
                }
            }
            return false;
        }

        /// <summary>Returns the date of last day of date month.</summary>
        /// <param name="_DateValue">The date to process.</param>
        /// <returns>The last day of the month for the specified date.</returns>
        public DateTime LastOfMonth(DateTime _DateValue)
        {
            if (!Valid(_DateValue)) return DateTime.MinValue;
            else if (_DateValue.Month < 12) return new DateTime(_DateValue.Year, _DateValue.Month + 1, 1, 0, 0, 0).AddDays(-1);
            else return new DateTime(_DateValue.Year, 12, 31, 0, 0, 0);
        }

        /// <summary>Returns the last day of date week.</summary>
        /// <param name="_DateValue">The date to process.</param>
        /// <returns>The last day of the week for the specified date.</returns>
        public DateTime LastOfWeek(DateTime _DateValue)
        {
            if (Valid(_DateValue)) return _DateValue.AddDays(7 - DayOfTheWeek(_DateValue));
            else return DateTime.MinValue;
        }

        /// <summary>Returns the last day of date year.</summary>
        /// <param name="_DateValue">The date to process.</param>
        /// <returns>The last day of the year for the specified date.</returns>
        public DateTime LastOfYear(DateTime _DateValue)
        {
            if (Valid(_DateValue)) return new DateTime(_DateValue.Year, 12, 31, 0, 0, 0);
            else return DateTime.MinValue;
        }

        /// <summary>Returns true if year is a leap year.</summary>
        /// <param name="_Year">The year to check.</param>
        /// <returns>True if the year is a leap year, otherwise false.</returns>
        public bool LeapYear(int _Year)
        {
            return (_Year % 4 == 0) && ((_Year % 100 != 0) || (_Year % 400 == 0));
        }

        /// <summary>Returns true if date is equal or greater than maximum value.</summary>
        /// <param name="_DateTime">The date to check.</param>
        /// <returns>True if the date is equal or greater than the maximum value, otherwise false.</returns>
        public bool MaxDate(DateTime _DateTime)
        {
            if (_DateTime == null) return false;
            else return _DateTime >= DateTime.MaxValue;
        }

        /// <summary>Returns true if date is null or minimum value.</summary>
        /// <param name="_DateTime">The date to check.</param>
        /// <returns>True if the date is null or the minimum value, otherwise false.</returns>
        public bool MinDate(DateTime _DateTime)
        {
            if (_DateTime == null) return true;
            else return _DateTime <= DateTime.MinValue;
        }

        /// <summary>Returns months between start date and end date.</summary>
        /// <param name="_StartDate">The start date.</param>
        /// <param name="_EndDate">The end date.</param>
        /// <returns>The number of months between the two dates.</returns>
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
        /// <returns>The current date and time.</returns>
        public DateTime Now()
        {
            return DateTime.Now;
        }

        /// <summary>Returns true if date is valid (if not null and between min date value and max date value.</summary>
        /// <param name="_DateTime">The date to check.</param>
        /// <returns>True if the date is valid, otherwise false.</returns>
        public bool Valid(DateTime _DateTime)
        {
            if (_DateTime == null) return false;
            else return (_DateTime > DateTime.MinValue) && (_DateTime < DateTime.MaxValue);
        }

        /// <summary>Returns year with 2 digit to 4 fitted to next 30 years or 70 previous year.</summary>
        /// <param name="_Year">The year to fit.</param>
        /// <returns>The fitted year.</returns>
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
