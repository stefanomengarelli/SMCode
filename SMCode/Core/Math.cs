/*  ===========================================================================
 *  
 *  File:       Math.cs
 *  Version:    2.0.0
 *  Date:       February 2024
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2024 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *
 *  SMCode application class: math.
 *
 *  ===========================================================================
 */

using System;

namespace SMCodeSystem
{

    /* */

    /// <summary>SMCode application class: math.</summary>
    public partial class SMCode
    {

        /* */

        #region Declarations

        /*  ===================================================================
         *  Declarations
         *  ===================================================================
         */

        /// <summary>Rotor string.</summary>
        private const string RotorStr = "!#$%&'()*+,-./\"0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ abcdefghijklmnopqrstuvwxyz:;<=>?@[\\]^_`{|}~°¢£§•¶ß©´Æ±µªºΩæø¿¡¬√ƒ≈«»… ÀÃÕŒœ—“”‘’÷Ÿ⁄€‹›‡·‚„‰ÂÊÁËÈÍÎÏÌÓÔÒÚÛÙıˆ˘˙˚¸˝ˇ";

        #endregion

        /* */

        #region Properties

        /*  ===================================================================
         *  Properties
         *  ===================================================================
         */

        /// <summary>Currency format.</summary>
        public string CurrencyFormat { get; set; }

        /// <summary>Currency more accurate format.</summary>
        public string CurrencyPrecisionFormat { get; set; }

        /// <summary>Get max signed integer 32 value.</summary>
        public int MaxInteger32 { get; private set; }

        /// <summary>Quantity format.</summary>
        public string QuantityFormat { get; set; }

        /// <summary>Quantity more accurate format.</summary>
        public string QuantityPrecisionFormat { get; set; }

        /// <summary>Random generator instance.</summary>
        public Random Random { get; private set; }

        /// <summary>Get or set rotor counter.</summary>
        public int RotorCount { get; set; }

        /// <summary>Get or set rotor last key.</summary>
        public string RotorKey { get; set; }

        #endregion

        /* */

        #region Initialization

        /*  ===================================================================
         *  Initialization
         *  ===================================================================
         */

        /// <summary>Initialize settings class environment.</summary>
        public void InitializeMath()
        {
            CurrencyFormat = @"###,###,###,###,##0.00";
            CurrencyPrecisionFormat = @"###,###,###,###,##0.000";
            MaxInteger32 = 2147483647;
            QuantityFormat = @"###,###,###,###,##0.00";
            QuantityPrecisionFormat = @"###,###,###,###,##0.000";
            Random = new Random(Convert.ToInt32(DateTime.Now.Ticks % 2147483647));
            RotorCount = 0;
            RotorKey = "";
        }

        #endregion

        /* */

        #region Methods

        /*  ===================================================================
         *  Methods
         *  ===================================================================
         */

        /// <summary>Returns absolute value of integer.</summary>
        public int Abs(int _Value)
        {
            if (_Value < 0) return -_Value;
            else return _Value;
        }

        /// <summary>Returns absolute value of long integer.</summary>
        public long Abs(long _Value)
        {
            if (_Value < 0) return -_Value;
            else return _Value;
        }

        /// <summary>Returns absolute value of double.</summary>
        public double Abs(double _Value)
        {
            if (_Value < 0.0d) return -_Value;
            else return _Value;
        }

        /// <summary>Returns long integer corresponding the value represented by string with digit of base chars.</summary>
        public long BaseToInt(string _Value, string _BaseChars)
        {
            long r = 0, q = 1, b = (long)_BaseChars.Length;
            int i = _Value.Length, p;
            if ((b > 1) && (i > 0))
            {
                while (i > 0)
                {
                    i--;
                    p = _BaseChars.IndexOf(_Value[i]);
                    if (p > -1)
                    {
                        r += q * (long)p;
                        q *= b;
                    }
                    else i = 0;
                }
            }
            return r;
        }

        /// <summary>Returns integer value of binary string.</summary>
        public int BinToInt(string _String)
        {
            int r = 0, i = _String.Length, b = 1;
            while (i > 0)
            {
                i--;
                if ((_String[i] == '1') || (_String[i] == '+')
                || (_String[i] == 'S') || (_String[i] == 's')
                || (_String[i] == 'T') || (_String[i] == 't')
                || (_String[i] == 'V') || (_String[i] == 'v')
                || (_String[i] == 'Y') || (_String[i] == 'y'))
                {
                    r += b;
                }
                b *= 2;
            }
            return r;
        }

        /// <summary>Returns integer value of boolean array (first element less significant).</summary>
        public int BinToInt(bool[] _Array)
        {
            int r = 0, i = 0, b = 1;
            if (_Array != null)
            {
                while (i < _Array.Length)
                {
                    if (_Array[i]) r += b;
                    b *= 2;
                    i++;
                }
            }
            return r;
        }

        /// <summary>Divide integer dividend by divisor and store result and remainder.
        /// Returns true if the operation has been executed without errors.</summary>
        public bool DivMod(int _Dividend, int _Divisor, ref int _Result, ref int _Remainder)
        {
            try
            {
                _Result = _Dividend / _Divisor;
                _Remainder = _Dividend % _Divisor;
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>Returns fractional part of double precision value.</summary>
        public double Frac(double _Value)
        {
            return _Value - Math.Truncate(_Value);
        }

        /// <summary>Returns one of integer specified for test if true or false.</summary>
        public int Iif(bool _Test, int _ReturnIfTrue, int _ReturnIfFalse)
        {
            if (_Test) return _ReturnIfTrue; 
            else return _ReturnIfFalse;
        }

        /// <summary>Returns one of decimal specified for test if true or false.</summary>
        public decimal Iif(bool _Test, decimal _ReturnIfTrue, decimal _ReturnIfFalse)
        {
            if (_Test) return _ReturnIfTrue;
            else return _ReturnIfFalse;
        }

        /// <summary>Returns one of double specified for test if true or false.</summary>
        public double Iif(bool _Test, double _ReturnIfTrue, double _ReturnIfFalse)
        {
            if (_Test) return _ReturnIfTrue;
            else return _ReturnIfFalse;
        }

        /// <summary>Returns millimeters corresponding inches.</summary>
        public double InchToMm(double _Inches)
        {
            return _Inches * 25.4d;
        }

        /// <summary>Returns integer part of double precision value.</summary>
        public double Int(double _Value)
        {
            return Math.Truncate(_Value);
        }

        /// <summary>Convert unsigned integer in to integer.</summary>
        public int Int(uint _UnsignedInteger)
        {
            return Convert.ToInt32(_UnsignedInteger);
        }

        /// <summary>Returns string value representing the value with base chars digits.</summary>
        public string IntToBase(long _Value, string _BaseChars)
        {
            long b = (long)_BaseChars.Length;
            string r = "";
            if (b > 1)
            {
                while (_Value > 0)
                {
                    r = _BaseChars[(int)(_Value % b)] + r;
                    _Value /= b;
                }
                if (r.Length < 1) return "" + _BaseChars[0];
                else return r;
            }
            else return "0";
        }

        /// <summary>Setting elements of boolean array matching integer (max 16 bit). 
        /// First element is less significant.</summary>
        public void IntToBinArray(int _Value, bool[] _Array)
        {
            int i, j;
            bool[] q = new bool[16];
            if (_Array != null)
            {
                _Value %= 65536;
                i = 0;
                while ((_Value > 0) && (i < q.Length))
                {
                    q[i] = _Value % 2 > 0;
                    _Value /= 2;
                    i++;
                }
                j = 0;
                while ((j < _Array.Length) && (i > 0))
                {
                    i--;
                    _Array[j] = q[i];
                    j++;
                }
            }
        }

        /// <summary>Convert integer value in to unsigned integer.</summary>
        public uint IntToUInt(int _Value)
        {
            return Convert.ToUInt32(_Value);
        }

        /// <summary>Return greater value between a and b.</summary>
        public byte Max(byte _A, byte _B)
        {
            if (_A > _B) return _A;
            else return _B;
        }

        /// <summary>Return greater value between a and b.</summary>
        public int Max(int _A, int _B)
        {
            if (_A > _B) return _A;
            else return _B;
        }

        /// <summary>Return greater value between a and b.</summary>
        public long Max(long _A, long _B)
        {
            if (_A > _B) return _A;
            else return _B;
        }

        /// <summary>Return greater value between a and b.</summary>
        public double Max(double _A, double _B)
        {
            if (_A > _B) return _A;
            else return _B;
        }

        /// <summary>Return greater value between a and b.</summary>
        public decimal Max(decimal _A, decimal _B)
        {
            if (_A > _B) return _A;
            else return _B;
        }

        /// <summary>Return lesser value between a and b.</summary>
        public byte Min(byte _A, byte _B)
        {
            if (_A < _B) return _A;
            else return _B;
        }

        /// <summary>Return lesser value between a and b.</summary>
        public int Min(int _A, int _B)
        {
            if (_A < _B) return _A;
            else return _B;
        }

        /// <summary>Return lesser value between a and b.</summary>
        public long Min(long _A, long _B)
        {
            if (_A < _B) return _A;
            else return _B;
        }

        /// <summary>Return lesser value between a and b.</summary>
        public double Min(double _A, double _B)
        {
            if (_A < _B) return _A;
            else return _B;
        }

        /// <summary>Return lesser value between a and b.</summary>
        public decimal Min(decimal _A, decimal _B)
        {
            if (_A < _B) return _A;
            else return _B;
        }

        /// <summary>Returns inches value corresponding millimeters.</summary>
        public double MmToInch(double _Millimeters)
        {
            return _Millimeters / 25.4d;
        }

        /// <summary>Returns twips value correspondig millimeters.</summary>
        public int MmToTwips(int _Millimeters)
        {
            return Convert.ToInt32(MmToInch(Convert.ToDouble(_Millimeters)) * 1440.0d);
        }

        /// <summary>Returns value raised by power.</summary>
        public double NumExp(double _Value, double _Power)
        {
            return Math.Pow(_Value, _Power);
        }

        /// <summary>Returns percent value related to max.</summary>
        public int Percent(int _Value, int _Max)
        {
            int r;
            if (_Max == 0) r = 0;
            else r = _Value * 100 / _Max;
            if (r < 0) r = 0;
            else if (r > 100) r = 100;
            return r;
        }

        /// <summary>Returns percent value related to max.</summary>
        public int Percent(long _Value, long _Max)
        {
            int r;
            if (_Max == 0) r = 0;
            else r = Convert.ToInt32(_Value * 100 / _Max);
            if (r < 0) r = 0;
            else if (r > 100) r = 100;
            return r;
        }

        /// <summary>Returns percent value related to max.</summary>
        public int Percent(long _Value, long _Max, bool _MaxTo100)
        {
            int r;
            if (_Max == 0) r = 0;
            else r = Convert.ToInt32(_Value * 100 / _Max);
            if (r < 0) r = 0;
            else if (_MaxTo100 && (r > 100)) r = 100;
            return r;
        }

        /// <summary>Returns percent value related to max.</summary>
        public double Percent(double _Value, double _Max, bool _MaxTo100)
        {
            double r;
            if (_Max == 0.0d) r = 0.0d;
            else r = _Value * 100.0d / _Max;
            if (r < 0.0d) r = 0.0d;
            else if (_MaxTo100 && (r > 100.0d)) r = 100.0d;
            return r;
        }

        /// <summary>Returns percent value related to max standardized from range start to range end.</summary>
        public int Percent(int _Value, int _Max, int _RangeStart, int _RangeEnd)
        {
            int r, g = _RangeEnd - _RangeStart;
            if (g < 1) r = 0;
            else if (_Max == 0) r = _RangeEnd;
            else r = _Value * g / _Max + _RangeStart;
            if (r < _RangeStart) r = _RangeStart;
            else if (r > _RangeEnd) r = _RangeEnd;
            return r;
        }

        /// <summary>Returns percent double value related to max with decimals.</summary>
        public double Percent(int _Value, int _Max, int _DecimalsCount)
        {
            double r;
            if (_Max == 0) r = 0.0d;
            else r = RoundDouble(Convert.ToDouble(_Value) * 100.0d / Convert.ToDouble(_Max), _DecimalsCount);
            if (r < 0.0d) r = 0.0d;
            else if (r > 100.0d) r = 100.0d;
            return r;
        }

        /// <summary>Returns percent double value related to max with decimals.</summary>
        public double Percent(double _Value, double _Max, int _DecimalsCount)
        {
            double r;
            if (_Max == 0.0d) r = 0.0d;
            else r = RoundDouble(_Value * 100.0d / _Max, _DecimalsCount);
            if (r < 0.0d) r = 0.0d;
            else if (r > 100.0d) r = 100.0d;
            return r;
        }

        /// <summary>Return a random integer greater or equal to 0 and lower or equal to _Value.</summary>
        public int Rnd(int _Value)
        {
            return Random.Next(0, _Value);
        }

        /// <summary>Return a random value greater or equal to 0 and less than _Value.</summary>
        public double Rnd(double _Value)
        {
            return Random.NextDouble() * _Value;
        }

        /// <summary>Returns pseudo random number between 0 and value depend on key.</summary>
        public int Rnd(int _Value, string _Key)
        {
            return Rnd100(_Key) * _Value / 100;
        }

        /// <summary>Returns pseudo random number between 0 and 100 depend on key.</summary>
        public int Rnd100(string _Key)
        {
            int r;
            int[] a = new int[] { 29, 40, 71, 80, 31, 33, 57, 25, 88, 78,
                                  01, 67, 03, 19, 08, 60, 47, 63, 36, 49,
                                  23, 74, 48, 51, 06, 17, 58, 46, 81, 24,
                                  45, 93, 35, 10, 70, 94, 13, 30, 41, 05,
                                  15, 56, 73, 86, 95, 27, 22, 76, 84, 62,
                                  82, 14, 39, 00, 87, 100, 64, 96, 21, 77, 12,
                                  83, 11, 54, 65, 38, 09, 42, 97, 59, 18,
                                  52, 20, 32, 37, 44, 85, 04, 75, 68, 90,
                                  98, 91, 66, 26, 53, 34, 61, 43, 28, 02,
                                  99, 92, 69, 16, 50, 07, 89, 55, 72, 79 };
            r = Rotor(a.Length - 1, _Key);
            if ((r > -1) && (r < a.Length)) return a[r];
            else return 0;
        }

        /// <summary>Return a value between 0 and max depending on key.</summary>
        public int Rotor(int _Max, string _Key)
        {
            int i, k, r = 0;
            if ((_Max > 0) && (_Key != null))
            {
                if (_Key.Length > 0)
                {
                    if (_Key != RotorKey)
                    {
                        RotorKey = _Key;
                        RotorCount = 0;
                    }
                    else RotorCount++;
                    for (i = 0; i < _Key.Length; i++)
                    {
                        k = RotorStr.IndexOf(_Key[i]);
                        if (k > -1)
                        {
                            r += k;
                            r = r % 8192;
                        }
                    }
                    r = r + RotorCount;
                    r = r % _Max;
                }
            }
            return r;
        }

        /// <summary>Returns value rounded with decimals count digits.</summary>
        public double RoundDouble(double _Value, int _DecimalsCount)
        {
            double d, f = 1.0d, i;
            // calculate decimal count ten based factor
            while (_DecimalsCount > 0)
            {
                f *= 10.0d;
                _DecimalsCount--;
            }
            // calculate integer part
            i = Math.Truncate(_Value);
            // calculate and evaluate decimal part
            d = (_Value - i) * f;
            if (d - Math.Truncate(d) >= 0.5d) d += 1.0d;
            // return rounded double value
            return i + Math.Truncate(d) / f;
        }

        /// <summary>Return value converted from range scale to new range scale.</summary>
        public int Scale(int _Value, int _SourceRangeStart, int _SourceRangeEnd, 
            int _TargetRangeStart, int _TargetRangeEnd)
        {
            if (_SourceRangeStart == _SourceRangeEnd) return 0; 
            else return _TargetRangeStart + (_TargetRangeEnd - _TargetRangeStart) 
                    * (_Value - _SourceRangeStart) / (_SourceRangeEnd - _SourceRangeStart);
        }

        /// <summary>Return value converted from scale to new scale.</summary>
        public double Scale(double _Value, double _SourceRangeStart, double _SourceRangeEnd, 
            double _TargetRangeStart, double _TargetRangeEnd)
        {
            if (_SourceRangeStart == _SourceRangeEnd) return 0.0d;
            else return _TargetRangeStart + (_TargetRangeEnd - _TargetRangeStart) 
                    * (_Value - _SourceRangeStart) / (_SourceRangeEnd - _SourceRangeStart);
        }

        /// <summary>Returns integer -1 if value is negative, 1 if positive, 0 if zero.</summary>
        public int Sign(int _Value)
        {
            if (_Value > 0) return 1;
            else if (_Value < 0) return -1;
            else return 0;
        }

        /// <summary>Returns integer -1 if value is negative, 1 if positive, 0 if zero.</summary>
        public int Sign(double _Value)
        {
            if (_Value > 0.0d) return 1;
            else if (_Value < 0.0d) return -1;
            else return 0;
        }

        /// <summary>Returns value applying threshold and increment.</summary>
        public int Threshold(int _Value, int _Threshold, int _Increment)
        {
            int r;
            if ((_Threshold > 0) && (_Increment > 0))
            {
                r = _Value / _Increment * _Increment;
                if (_Value - r >= _Threshold) return r + _Increment;
                else return r;
            }
            else return _Value;
        }

        /// <summary>Returns integer from double precision value.</summary>
        public int Trunc(double _Value)
        {
            return Convert.ToInt32(Math.Truncate(_Value));
        }

        #endregion

        /* */

    }

    /* */

}
