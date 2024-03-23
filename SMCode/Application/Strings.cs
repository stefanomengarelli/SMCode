/*  ===========================================================================
 *  
 *  File:       Strings.cs
 *  Version:    2.0.0
 *  Date:       February 2024
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2024 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *
 *  SMCode application class: strings.
 *  
 *  ===========================================================================
 */

using System.Text;
using System.Text.RegularExpressions;

namespace SMCode
{

    /* */

    /// <summary>SMCode application class: strings.</summary>
    public partial class SMApplication
    {

        /* */

        #region Declarations

        /*  ===================================================================
         *  Declarations
         *  ===================================================================
         */

        /// <summary>Base 32 string.</summary>
        public const string Base32 = @"0123456789ABCDEFGHKLMNPQRSTUVWXZ";

        /// <summary>Base chars (digits + uppercase + lowercase).</summary>
        public const string BaseChars = @"0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

        /// <summary>Base symbols.</summary>
        public const string BaseSymbols = @"àèéìòùç !?.,:;+-*/=<>#()[]{}@&%$£_§°";

        /// <summary>Consonants.</summary>
        public const string Consonants = @"BCDFGHJKLMNPQRSTVWXYZ";

        /// <summary>Unprobable string to test unknown string values</summary>
        public const string Unknown = @"?kN0^Wn#~;";

        /// <summary>Vocals.</summary>
        public const string Vocals = @"AEIOU";

        #endregion

        /* */

        #region Properties

        /*  ===================================================================
         *  Properties
         *  ===================================================================
         */

        /// <summary>Default carriage return.</summary>
        public string CR { get; set; } = "\r\n";

        /// <summary>Get or set decimal separator.</summary>
        public char DecimalSeparator { get; set; }

        /// <summary>Get or set memo trim char array.</summary>
        public char[] MemoTrimChars { get; set; }

        /// <summary>Get or set text string encoding mode.</summary>
        public Encoding TextEncoding { get; set; }

        /// <summary>Get or set thousand separator.</summary>
        public char ThousandSeparator { get; set; }

        /// <summary>Get or set trailing char.</summary>
        public char TrailingChar { get; set; }

        #endregion

        /* */

        #region Initialization

        /*  ===================================================================
         *  Initialization
         *  ===================================================================
         */

        /// <summary>Initialize string class environment.</summary>
        public void InitializeStrings()
        {
            CR = "\r\n";
            DecimalSeparator = ',';
            MemoTrimChars = new char[] { ' ', '\t', '\r', '\n' };
            TextEncoding = Encoding.Default;
            ThousandSeparator = '.';
            TrailingChar = '\\';
        }

        #endregion

        /* */

        #region Methods

        /*  ===================================================================
         *  Methods
         *  ===================================================================
         */

        /// <summary>Returns part of string after first recourrence of sub string. 
        /// If sub string is not present returns empty string.</summary>
        public string After(string _String, string _SubString)
        {
            int i = _String.IndexOf(_SubString);
            if (i > -1)
            {
                i += _SubString.Length;
                if (_String.Length > i) return _String.Substring(i);
                else return "";
            }
            else return "";
        }

        /// <summary>Returns ASCII code of char.</summary>
        public byte Asc(char _Char)
        {
            return (byte)((int)_Char % 256);
        }

        /// <summary>Returns ASCII code of string first char.</summary>
        public byte Asc(string _String)
        {
            if (_String.Length > 0) return (byte)(((int)_String[0]) % 256);
            else return 0;
        }

        /// <summary>Returns Unicode code of char.</summary>
        public int AscU(char _Char)
        {
            return (int)_Char;
        }

        /// <summary>Returns Unicode code of string first char.</summary>
        public int AscU(string _String)
        {
            if (_String.Length > 0) return (int)_String[0];
            else return 0;
        }

        /// <summary>Returns string decoded base64.</summary>
        public string Base64Decode(string _String)
        {
            byte[] b;
            try
            {
                if (Empty(_String)) return "";
                else
                {
                    b = Convert.FromBase64String(_String);
                    if (b == null) return "";
                    else return ToStr(b);
                }
            }
            catch
            {
                return "";
            }
        }

        /// <summary>Returns string decoded base64.</summary>
        public byte[] Base64DecodeBytes(string _String)
        {
            try
            {
                if (Empty(_String)) return null;
                else return Convert.FromBase64String(_String);
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
                b = TextEncoding.GetBytes(_String);
                if (b == null) return "";
                else return Convert.ToBase64String(b);
            }
            catch
            {
                return "";
            }
        }

        /// <summary>Returns string encoded base64.</summary>
        public string Base64EncodeBytes(byte[] _Bytes)
        {
            try
            {
                if (_Bytes == null) return "";
                else return Convert.ToBase64String(_Bytes);
            }
            catch
            {
                return "";
            }
        }

        /// <summary>Returns part of string before first recurrence of substring. 
        /// If subString is not present returns empty string.</summary>
        public string Before(string _String, string _Substring)
        {
            int i = _String.IndexOf(_Substring);
            if (i > 0) return _String.Substring(0, i);
            else return "";
        }

        /// <summary>Returns part of string between begin substring and ending substring.
        /// If one of two substrings are not present returns empty string.</summary>
        public string Btw(string _String, string _BeginSubstring, string _EndSubstring)
        {
            int i = _String.IndexOf(_BeginSubstring);
            if ((i > -1) && (_String.Length > i + _BeginSubstring.Length))
            {
                _String = _String.Substring(i + _BeginSubstring.Length);
                i = _String.IndexOf(_EndSubstring);
                if (i > 0) return _String.Substring(0, i);
                else return "";
            }
            else return "";
        }

        /// <summary>Returns part of string between begin substring and 
        /// ending substring without uppercase/lowercase matching. 
        /// If one of two substrings are not present returns empty string.</summary>
        public string BtwU(string _String, string _BeginSubstring, string _EndSubstring)
        {
            int i = _String.ToLower().IndexOf(_BeginSubstring.ToLower());
            if ((i > -1) && (_String.Length > i + _BeginSubstring.Length))
            {
                _String = _String.Substring(i + _BeginSubstring.Length);
                i = _String.ToLower().IndexOf(_EndSubstring.ToLower());
                if (i > 0) return _String.Substring(0, i);
                else return "";
            }
            else return "";
        }

        /// <summary>Returns string passed adding new string divided by separator.</summary>
        public string Cat(string _String, string _NewString, string _Separator)
        {
            if (_NewString == "") return _String;
            else if (_String.Length > 0) return _String + _Separator + _NewString;
            else return _NewString;
        }

        /// <summary>Returns a string containing strings in array, joined and divided by separator.
        /// If specified, empty strings will be no added.</summary>
        public string Cat(string[] _StringArray, string _Separator, bool _ExcludeEmptyStrings)
        {
            int i, h;
            StringBuilder r = new StringBuilder();
            if (_StringArray != null)
            {
                h = _StringArray.Length;
                for (i = 0; i < h; i++)
                {
                    if (_StringArray[i] != null)
                    {
                        if (!_ExcludeEmptyStrings || (_StringArray[i].Length > 0))
                        {
                            if (r.Length > 0) r.Append(_Separator);
                            r.Append(_StringArray[i]);
                        }
                    }
                }
            }
            return r.ToString();
        }

        /// <summary>Returns integer string checksum (0-9999).</summary>
        public int CheckSum(string _String)
        {
            int i = 0, j = 0, k, r = 0;
            int[] p = { 1, 7, 2, 3, 5 };
            string a = BaseChars + BaseSymbols;
            while (i < _String.Length)
            {
                k = a.IndexOf(_String[i]) + 1;
                r += (k * p[j]) % 10000;
                j++;
                if (j >= p.Length) j = 0;
                i++;
            }
            return r;
        }

        /// <summary>Returns ASCII char with code.</summary>
        public char Chr(int _AsciiCode)
        {
            return (char)(_AsciiCode % 256);
        }

        /// <summary>Returns how many char founded in string.</summary>
        public int ChrCount(string _String, char _Char)
        {
            int r = 0, i = _String.Length;
            while (i > 0)
            {
                i--;
                if (_String[i] == _Char) r++;
            }
            return r;
        }

        /// <summary>Returns char in string s with circular position index.</summary>
        public char ChrLoop(string _String, int _Index)
        {
            if (_String.Length > 0)
            {
                while (_Index < 0) _Index += _String.Length;
                while (_Index >= _String.Length) _Index -= _String.Length;
                return _String[_Index];
            }
            else return ' ';
        }

        /// <summary>Return true if string has repeated chars.</summary>
        public bool ChrRepeated(string _String)
        {
            bool r = false;
            int i = 0, j;
            while (!r && (i < _String.Length))
            {
                j = i + 1;
                while (!r && (j < _String.Length))
                {
                    r = _String[i] == _String[j];
                    j++;
                }
                i++;
            }
            return r;
        }

        /// <summary>Returns string with all occurrences of old char replaced by new char.</summary>
        public string ChrReplace(string _String, char _OldChar, char _NewChar)
        {
            return _String.Replace(_OldChar, _NewChar);
        }

        /// <summary>Returns string with all occurrences of each chars' char replaced by new char.</summary>
        public string ChrReplace(string _String, string _Chars, char _NewChar)
        {
            int i = 0;
            StringBuilder r = new StringBuilder();
            while (i < _String.Length)
            {
                if (_Chars.IndexOf(_String[i]) > -1) r.Append(_NewChar);
                else r.Append(_String[i]);
                i++;
            }
            return r.ToString();
        }

        /// <summary>Returns integer value of number represented in char.</summary>
        public int ChrToInt(char _Char)
        {
            if (_Char == '9') return 9;
            else if (_Char == '8') return 8;
            else if (_Char == '7') return 7;
            else if (_Char == '6') return 6;
            else if (_Char == '5') return 5;
            else if (_Char == '4') return 4;
            else if (_Char == '3') return 3;
            else if (_Char == '2') return 2;
            else if (_Char == '1') return 1;
            else return 0;
        }

        /// <summary>Returns Unicode char with code.</summary>
        public char ChrU(int _Unicode)
        {
            return (char)_Unicode;
        }

        /// <summary>Return a new array with same elements of passed.</summary>
        public string[] Clone(string[] _Array)
        {
            int i;
            string[] r = null;
            if (_Array!=null)
            {
                r = new string[_Array.Length];
                for (i = 0; i < _Array.Length; i++) r[i] = _Array[i];
            }
            return r;
        }

        /// <summary>Compare string a and b with ASCII/UNICODE mode and returns 
        /// &gt;0 if a&gt;b, &lt;0 if a&lt;0 and 0 if a=b.</summary>
        public int Compare(string _StringA, string _StringB)
        {
            int r = 0, i = 0;
            while ((r == 0) && (i < _StringA.Length) && (i < _StringB.Length))
            {
                r = (int)_StringA[i] - (int)_StringB[i];
                i++;
            }
            if (r == 0) r = _StringA.Length - _StringB.Length;
            return r;
        }

        /// <summary>Compare string a and b partially and or ignoring case if specified.</summary>
        public int Compare(string _StringA, string _StringB, bool _IgnoreCase, bool _PartialMode)
        {
            if (_PartialMode)
            {
                if (_StringA.Length < _StringB.Length) _StringB = _StringB.Substring(0, _StringA.Length);
                else if (_StringA.Length > _StringB.Length) _StringA = _StringA.Substring(0, _StringB.Length);
            }
            return String.Compare(_StringA, _StringB, _IgnoreCase);
        }

        /// <summary>Returns true if string is null, empty or contains only spaces.</summary>
        public bool Empty(string _String)
        {
            if (_String == null) return true;
            else return _String.Trim().Length < 1;
        }

        /// <summary>Returns true if string array is null or with no items.</summary>
        public bool Empty(string[] _StringArray)
        {
            if (_StringArray == null) return true;
            else if (_StringArray.Length < 1) return true;
            return false;
        }

        /// <summary>Return string escaping limited special chars.</summary>
        public string Escape(string _String)
        {
            int i;
            StringBuilder s;
            if (_String != null)
            {
                i = 0;
                s = new StringBuilder();
                while (i < _String.Length)
                {
                    if (_String[i] == '\n') s.Append(@"\n");
                    else if (_String[i] == '\r') s.Append(@"\r");
                    else if (_String[i] == '\t') s.Append(@"\t");
                    else if (_String[i] == '\'') s.Append(@"\'");
                    else if (_String[i] == '"') s.Append(@"\" + '"');
                    else if (_String[i] == '\\') s.Append(@"\\");
                    else if (_String[i] == '\0') s.Append(@"\0");
                    else s.Append(_String[i]);
                }
                return s.ToString();
            }
            else return "";
        }

        /// <summary>Returns the part of string before the first occurrence of separator character.
        /// The function store in to string the part remaining (without extracted part and separator).</summary>
        public string Extract(ref string _String, char _Separator)
        {
            string r;
            int i = _String.IndexOf(_Separator);
            if (i < 0)
            {
                r = _String;
                _String = "";
            }
            else
            {
                if (i > 0) r = _String.Substring(0, i);
                else r = "";
                if (_String.Length > i + 1) _String = _String.Substring(i + 1);
                else _String = "";
            }
            return r;
        }

        /// <summary>Returns the part of string before the first occurrence of one of separators characters.
        /// The function store in to string the part remaining (without extracted part and separator).</summary>
        public string Extract(ref string _String, string _Separators)
        {
            string r;
            int i = Pos(_Separators, _String, false);
            if (i < 0)
            {
                r = _String;
                _String = "";
            }
            else
            {
                if (i > 0) r = _String.Substring(0, i);
                else r = "";
                if (_String.Length > i + 1) _String = _String.Substring(i + 1);
                else _String = "";
            }
            return r;
        }

        /// <summary>Returns the first digits count from string. Extraction stop when non digit char encountered.
        /// The function store in string the part remaining (without extracted part and non digit chars encountered).</summary>
        public string ExtractDigits(ref string _String, int _DigitsCount)
        {
            bool b = true;
            string r = "";
            while (b && (_String.Length > 0) && (r.Length < _DigitsCount))
            {
                b = (_String[0] >= '0') && (_String[0] <= '9');
                if (b)
                {
                    r += _String[0];
                    if (_String.Length > 1) _String = _String.Substring(1);
                    else _String = "";
                }
            }
            b = true;
            while (b && (_String.Length > 0))
            {
                b = (_String[0] < '0') || (_String[0] > '9');
                if (b)
                {
                    if (_String.Length > 1) _String = _String.Substring(1);
                    else _String = "";
                }
            }
            return r;
        }

        /// <summary>Returns the part of string before the first occurrence of CR-LF sequence.
        /// The function store in string the part remaining (without extracted part and CR-LF sequence).
        /// Function are now implemented to find only LF delimiter also (UNIX).</summary>
        public string ExtractLine(ref string _String)
        {
            int i;
            string r;
            //
            // find line-feed
            //
            i = _String.IndexOf('\n');
            if (i > -1)
            {
                if (i > 0)
                {
                    if (_String[i - 1] == '\r')
                    {
                        if (i > 1) r = _String.Substring(0, i - 1);
                        else r = "";
                    }
                    else r = _String.Substring(0, i);
                }
                else r = "";
                if (_String.Length > i + 1) _String = _String.Substring(i + 1);
                else _String = "";
            }
            else
            {
                //
                // find carriage-return
                //
                i = _String.IndexOf('\r');
                if (i > -1)
                {
                    if (i > 0) r = _String.Substring(0, i);
                    else r = "";
                    if (_String.Length > i + 1) _String = _String.Substring(i + 1);
                    else _String = "";
                }
                else
                {
                    r = _String;
                    _String = "";
                }
            }
            //
            // return extracted line
            //
            return r;
        }

        /// <summary>Returns the part of string before the first occurrence of line separator sequence.
        /// The function store in string the part remaining (without extracted part and line separator sequence).</summary>
        public string ExtractLine(ref string _String, string _LineSeparator)
        {
            string r;
            int i = _String.IndexOf(_LineSeparator), l = _LineSeparator.Length;
            if (i < 0)
            {
                r = _String;
                _String = "";
            }
            else
            {
                if (i > 0) r = _String.Substring(0, i);
                else r = "";
                if (_String.Length > i + l) _String = _String.Substring(i + l);
                else _String = "";
            }
            return r;
        }

        /// <summary>Returns the version string entire version up to version level. 
        /// Version numbers will be unaffected.</summary>
        public string ExtractVersion(string _EntireVersion, int _VersionLevel)
        {
            string r = "";
            while (_VersionLevel > 0)
            {
                if (r != "") r += ".";
                r += Extract(ref _EntireVersion, ".,- ;_");
                _VersionLevel--;
            }
            return r;
        }

        /// <summary>Returns the version string entire version up to version level, 
        /// with numbers filled with zeroes. If zeroes is zero version numebers
        /// will be unaffected, if less than zero version numbers will be without 
        /// left padding zeroes.</summary>
        public string ExtractVersion(string _EntireVersion, int _VersionLevel, int _Zeroes)
        {
            string r = "";
            while (_VersionLevel > 0)
            {
                if (r != "") r += ".";
                if (_Zeroes < 0) r += Val(Extract(ref _EntireVersion, ".,- ;_")).ToString("###########0");
                else if (_Zeroes > 0) r += Zeroes(Val(Extract(ref _EntireVersion, ".,- ;_")), _Zeroes);
                else r += Extract(ref _EntireVersion, ".,- ;_");
                _VersionLevel--;
            }
            return r;
        }

        /// <summary>Return index of first occurrence of string passed in array, ignoring case is specified.
        /// Return -1 if not found.</summary>
        public int Find(string _String, string[] _StringArray, bool _IgnoreCase)
        {
            int r = -1, i = 0;
            if (_StringArray != null)
            {
                if (_IgnoreCase) _String = _String.ToLower();
                while ((r < 0) && (i < _StringArray.Length))
                {
                    if (_IgnoreCase)
                    {
                        if (_String == _StringArray[i].ToLower()) r = i;
                    }
                    else if (_String == _StringArray[i]) r = i;
                    i++;
                }
            }
            return r;
        }

        /// <summary>Return first string of array or empty string if null or empty array.</summary>
        public string First(string[] _StringArray)
        {
            if (_StringArray == null) return "";
            else if (_StringArray.Length < 1) return "";
            else if (_StringArray[0] == null) return "";
            else return _StringArray[0];
        }

        /// <summary>Return first string of list or empty string if null or empty list.</summary>
        public string First(List<string> _StringList)
        {
            if (_StringList == null) return "";
            else if (_StringList.Count < 1) return "";
            else if (_StringList[0] == null) return "";
            else return _StringList[0];
        }

        /// <summary>Return string replacing carriage-returns and tabs with spaces.</summary>
        public string Flat(string _String)
        {
            return _String.Replace("\r\n", " ").Replace("\n", " ").Replace("\r", " ").Replace("\t", " ");
        }

        /// <summary>Returns string representing values in binary array.</summary>
        public string FromBinArray(bool[] _Array)
        {
            int i;
            string r = "";
            if (_Array != null) for (i = 0; i < _Array.Length; i++) r += ToBool(_Array[i]);
            return r;
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

        /// <summary>Return only consonants of passed string.</summary>
        public string GetConsonants(string _String)
        {
            int i = 0;
            string u = _String.ToUpper();
            StringBuilder r = new StringBuilder();
            while (i < _String.Length)
            {
                if (Consonants.IndexOf(u[i]) > -1) r.Append(_String[i]);
                i++;
            }
            return r.ToString();
        }

        /// <summary>Returns a string containing only numeric valid characters of string 
        /// considering decimal if specified.</summary>
        public string GetDigits(string _String, bool _Decimals)
        {
            char c;
            int i = 0, l;
            string r = "";
            _String = _String.Trim();
            l = _String.Length;
            while (i < l)
            {
                c = _String[i];
                if ((c >= '0') && (c <= '9')) r += c;
                else if (_Decimals && (c == 'E') && (r.Length > 0) && (r.IndexOf('E') < 0)) r += 'E';
                else if (((r == "") || r.EndsWith("E")) && ((c == '-') || (c == '+'))) r += c;
                else if ((c == DecimalSeparator)
                         && (r.IndexOf(DecimalSeparator) < 0))
                {
                    if ((r == "") || (r == "+") || (r == "-")) r += '0';
                    if (_Decimals) r += DecimalSeparator;
                    else i = l;
                }
                else if (c != ThousandSeparator) i = l;
                i++;
            }
            if (r.Length > 0)
            {
                if (r[r.Length - 1] == DecimalSeparator)
                {
                    if (r.Length > 1) r = r.Substring(0, r.Length - 1);
                    else r = "0";
                }
                if ((r == "") || (r == "+") || (r == "-")) r = "0";
            }
            else r = "0";
            return r;
        }

        /// <summary>Returns string containing only characters included in valid chars.</summary>
        public string GetValidChars(string _String, string _ValidChars)
        {
            int i = 0;
            StringBuilder r = new StringBuilder();
            while (i < _String.Length)
            {
                if (_ValidChars.IndexOf(_String[i]) > -1) r.Append(_String[i]);
                i++;
            }
            return r.ToString();
        }

        /// <summary>Return only vocals of passed string.</summary>
        public string GetVocals(string _String)
        {
            int i = 0;
            string u = _String.ToUpper();
            StringBuilder r = new StringBuilder();
            while (i < _String.Length)
            {
                if (Vocals.IndexOf(u[i]) > -1) r.Append(_String[i]);
                i++;
            }
            return r.ToString();
        }

        /// <summary>Returns one digits hex check sum of string.</summary>
        public string HexSum(string _String)
        {
            return IntToHex(CheckSum(_String) % 16, 1);
        }

        /// <summary>Returns integer value of hexadecimal string. Return 0 if fail.</summary>
        public int HexToInt(string _HexString)
        {
            int r;
            try
            {
                if (_HexString.Trim().Length < 1) r = 0;
                else r = int.Parse(_HexString.Trim(), System.Globalization.NumberStyles.HexNumber);
            }
            catch
            {
                r = 0;
            }
            return r;
        }

        /// <summary>Returns value of string result if true if the expression bool test is true,
        /// otherwise returns string value of result if false.</summary>
        public string Iif(bool _BoolTest, string _ReturnIfTrue, string _ReturnIfFalse)
        {
            if (_BoolTest) return _ReturnIfTrue;
            else return _ReturnIfFalse;
        }

        /// <summary>Indent lines included in string.</summary>
        public string Indent(string _String, string _Indent, int _Shift)
        {
            int i;
            string l, q = "";
            StringBuilder r;
            if ((_Indent.Length > 0) && (_String.Trim().Length > 0))
            {
                r = new StringBuilder();
                if (_Shift > 0) for (i = 0; i < _Shift; i++) q += _Indent;
                while (_String.Trim().Length > 0)
                {
                    l = ExtractLine(ref _String);
                    if (l.Trim().Length > 0)
                    {
                        if (_Shift < 0)
                        {
                            i = -_Shift;
                            while ((i-- > 0) && (l.StartsWith(_Indent))) l = Mid(l, _Indent.Length, l.Length);
                        }
                        else if (_Shift > 0) r.Append(q);
                    }
                    else l = "";
                    r.Append(l);
                    r.Append("\r\n");
                }
                return r.ToString();
            }
            else return _String;
        }

        /// <summary>Returns string with the part between substring1 and substring2 replaced by new string.
        /// If substring1 are not present returns string with sequence substring1+new string+substring2 added.</summary>
        public string InsBtw(string _String, string _NewString, string _Substring1, string _Substring2)
        {
            string a, b;
            int i = _String.IndexOf(_Substring1);
            if (i > -1)
            {
                a = Mid(_String, 0, i) + _Substring1 + _NewString;
                b = Mid(_String, i + _Substring1.Length, _String.Length);
                i = b.IndexOf(_Substring2);
                if (i > -1) return a + Mid(b, i, b.Length);
                else return _String;
            }
            else return _String + _Substring1 + _NewString + _Substring2;
        }

        /// <summary>Returns string with the part between substring1 and substring2 replaced by new string.
        /// If substring1 are not present returns string with sequence substring1+new string+substring2 added.
        /// Substring1 and substring2 case match is ignored.</summary>
        public string InsBtwU(string _String, string _NewString, string _Substring1, string _Substring2)
        {
            string a, b;
            int i = _String.ToLower().IndexOf(_Substring1.ToLower());
            if (i > -1)
            {
                a = Mid(_String, 0, i) + _Substring1 + _NewString;
                b = Mid(_String, i + _Substring1.Length, _String.Length);
                i = b.ToLower().IndexOf(_Substring2.ToLower());
                if (i > -1) return a + Mid(b, i, b.Length);
                else return _String;
            }
            else return _String + _Substring1 + _NewString + _Substring2;
        }

        /// <summary>Returns hexadecimal string representing integer value with digits.</summary>
        public string IntToHex(Int64 _Value, int _Digits)
        {
            if (_Digits < 1) return _Value.ToString("X");
            else return _Value.ToString("X" + _Digits.ToString());
        }

        /// <summary>Returns inverted string.</summary>
        public string Invert(string _String)
        {
            StringBuilder r = new StringBuilder();
            int l = _String.Length;
            while (l > 0) r.Append(_String[--l]);
            return r.ToString();
        }

        /// <summary>Returns true if all characters included in string are alphabet characters.</summary>
        public bool IsAlpha(string _String)
        {
            int l = _String.Length;
            bool r = (l > 0);
            while (r && (l > 0))
            {
                l--;
                r = ((_String[l] >= 'a') && (_String[l] <= 'z'))
                    || ((_String[l] >= 'A') && (_String[l] <= 'Z'));
            }
            return r;
        }

        /// <summary>Returns true if all characters included in string are alphabet characters or digits.</summary>
        public bool IsAlphaNum(string _String)
        {
            int l = _String.Length;
            bool r = (l > 0);
            while (r && (l > 0))
            {
                l--;
                r = ((_String[l] >= '0') && (_String[l] <= '9'))
                    || ((_String[l] >= 'a') && (_String[l] <= 'z'))
                    || ((_String[l] >= 'A') && (_String[l] <= 'Z'));
            }
            return r;
        }

        /// <summary>Returns true if all characters included in string are included in passed char set.</summary>
        public bool IsCharSet(string _String, string _CharSet)
        {
            int l = _String.Length;
            bool r = true;
            while (r && l > 0)
            {
                l--;
                r = _CharSet.IndexOf(_String[l]) > -1;
            }
            return r;
        }

        /// <summary>Returns true if all characters included in string are digits.</summary>
        public bool IsDigits(string _String)
        {
            int l = _String.Length;
            bool r = (l > 0);
            while (r && (l > 0))
            {
                l--;
                r = (_String[l] >= '0') && (_String[l] <= '9');
            }
            return r;
        }

        /// <summary>Returns true if all characters included in string are hex digits.</summary>
        public bool IsHex(string _String)
        {
            int l = _String.Length;
            bool r = (l > 0);
            while (r && (l > 0))
            {
                l--;
                r = ((_String[l] >= '0') && (_String[l] <= '9'))
                    || ((_String[l] >= 'A') && (_String[l] <= 'F'))
                    || ((_String[l] >= 'a') && (_String[l] <= 'f'));
            }
            return r;
        }

        /// <summary>Return true if string is hex masked string with format: {h1h2h3...hnk} where h# is 2 cipher hex code
        /// and k is h1..hn 1 hex digit checksum.</summary>
        public bool IsHexMask(string _String)
        {
            bool r = false;
            if (_String.Length > 4)
            {
                if ((_String[0] == '{') && (_String[_String.Length - 1] == '}'))
                {
                    _String = _String.Substring(1, _String.Length - 2);
                    if (IsHex(_String))
                    {
                        r = (_String[_String.Length - 1] == HexSum(_String.Substring(0, _String.Length - 1))[0]);
                    }
                }
            }
            return r;
        }

        /// <summary>Returns first length characters of string from left.</summary>
        public string Left(string _String, int _Length)
        {
            if (_Length > _String.Length) _Length = _String.Length;
            if (_Length > 0) return _String.Substring(0, _Length);
            else return "";
        }

        /// <summary>Returns string converted in lowercase.</summary>
        public string Lower(string _String)
        {
            return _String.ToLower();
        }

        /// <summary>Returns portion of string starting at position index and getting length chars.</summary>
        public string Mid(string _String, int _Index, int _Length)
        {
            if (_String.Length > 0)
            {
                if (_Length > 0)
                {
                    if (_Index < 0) _Index = 0;
                    if (_Index < _String.Length)
                    {
                        if (_Index + _Length > _String.Length) return _String.Substring(_Index);
                        else return _String.Substring(_Index, _Length);
                    }
                    else return "";
                }
                else return "";
            }
            else return "";
        }

        /// <summary>Returns a string long length characters aligned to right and eventually filled with chars on the left.</summary>
        public string PadL(string _String, int _Length, char _FillChar)
        {
            if (_String.Length > _Length) return Right(_String, _Length);
            else
            {
                while (_String.Length < _Length) _String = _FillChar + _String;
                return _String;
            }
        }

        /// <summary>Returns a string long length characters aligned to left and eventually filled with chars on the right.</summary>
        public string PadR(string _String, int _Length, char _FillChar)
        {
            if (_String.Length > _Length) return Left(_String, _Length);
            else
            {
                while (_String.Length < _Length) _String += _FillChar;
                return _String;
            }
        }

        /// <summary>Returns position of first char in to string seeking from left to right. 
        /// The function returns -1 if char is not found.</summary>
        public int Pos(char _Char, string _String)
        {
            return _String.IndexOf(_Char);
        }

        /// <summary>Return position of first occurrence of substring in string.</summary>
        public int Pos(string _Substring, string _String)
        {
            return _String.IndexOf(_Substring);
        }

        /// <summary>Returns position of first of one of chars contained in to string seeking from left to right. 
        /// The function returns -1 if char is not found.</summary>
        public int Pos(string _Chars, string _String, bool _IgnoreCase)
        {
            int i = 0, j, r = -1;
            if ((_Chars.Length > 0) && (_String.Length > 0))
            {
                if (_IgnoreCase)
                {
                    _Chars = _Chars.ToLower();
                    _String = _String.ToLower();
                }
                while ((r < 0) && (i < _Chars.Length))
                {
                    j = 0;
                    while ((r < 0) && (j < _String.Length))
                    {
                        if (_Chars[i] == _String[j]) r = j;
                        j++;
                    }
                    i++;
                }
            }
            return r;
        }

        /// <summary>Returns position of first char in to string seeking from right to left. 
        /// The function returns -1 if char is not found.</summary>
        public int PosR(char _Char, string _String)
        {
            int r = -1, i = _String.Length;
            while ((r < 0) && (i > 0))
            {
                i--;
                if (_String[i] == _Char) r = i;
            }
            return r;
        }

        /// <summary>Return position of first occurrence of substring in string searching from right to left.</summary>
        public int PosR(string _String, string _Substring)
        {
            int i = _String.Length - _Substring.Length, r = -1;
            while ((r < 0) && (i > -1))
            {
                if (_Substring == Mid(_String, i, _Substring.Length)) r = i;
                i--;
            }
            return r;
        }

        /// <summary>Returns position of first of one of chars contained in to string seeking from right to left. 
        /// The function returns -1 if char is not found.</summary>
        public int PosR(string _Chars, string _String, bool _IgnoreCase)
        {
            int i = 0, j, r = -1;
            if ((_Chars.Length > 0) && (_String.Length > 0))
            {
                if (_IgnoreCase)
                {
                    _Chars = _Chars.ToLower();
                    _String = _String.ToLower();
                }
                while ((r < 0) && (i < _Chars.Length))
                {
                    j = _String.Length;
                    while ((r < 0) && (j > 0))
                    {
                        j--;
                        if (_Chars[i] == _String[j]) r = j;
                    }
                    i++;
                }
            }
            return r;
        }

        /// <summary>Return position of first occurrence of substring in string ignoring uppercase and lowercase.</summary>
        public int PosU(string _String, string _Substring)
        {
            return _String.ToLower().IndexOf(_Substring.ToLower());
        }

        /// <summary>Returns string included by single quote '...'.</summary>
        public string Quote(string _String)
        {
            return @"'" + _String.Replace(@"'", @"''") + @"'";
        }

        /// <summary>Returns string included by double quote "...".</summary>
        public string Quote2(string _String, string _DoubleQuoteTrail = "")
        {
            if (_DoubleQuoteTrail == "") return @"""" + _String.Replace(@"""", @"""""") + @"""";
            else return @"""" + _String.Replace(@"""", _DoubleQuoteTrail) + @"""";
        }

        /// <summary>Returns string without specified character.</summary>
        public string Remove(string _String, char _CharToRemove)
        {
            int i = 0;
            StringBuilder r = new StringBuilder();
            while (i < _String.Length)
            {
                if (_String[i] != _CharToRemove) r.Append(_String[i]);
                i++;
            }
            return r.ToString();
        }

        /// <summary>Returns string replacing all old string occurrences with new string.</summary>
        public string Replace(string _String, string _OldString, string _NewString)
        {
            int j = 0, i, l = _OldString.Length;
            StringBuilder r;
            if (l > 0)
            {
                r = new StringBuilder();
                i = Mid(_String, j, _String.Length).IndexOf(_OldString);
                while (i > -1)
                {
                    r.Append(Mid(_String, j, i));
                    r.Append(_NewString);
                    j += i + l;
                    i = Mid(_String, j, _String.Length).IndexOf(_OldString);
                }
                r.Append(Mid(_String, j, _String.Length));
                return r.ToString();
            }
            else return _String;
        }

        /// <summary>Returns a string containing count times given string.</summary>
        public string Replicate(string _String, int _Count)
        {
            StringBuilder r = new StringBuilder();
            while (_Count > 0)
            {
                r.Append(_String);
                _Count--;
            }
            return r.ToString();
        }

        /// <summary>Returns last length characters (from right) of string.</summary>
        public string Right(string _String, int _Length)
        {
            if (_Length < 1) return "";
            else if (_String.Length > _Length) return _String.Substring(_String.Length - _Length, _Length);
            else return _String;
        }

        /// <summary>Returns a string representing a random name with length chars.</summary>
        public string RndName(int _Length)
        {
            StringBuilder r = new StringBuilder();
            Random rnd = new Random(DateTime.Now.Hour * 3600 + DateTime.Now.Minute * 60 + DateTime.Now.Second);
            if (_Length > 0)
            {
                r.Append(BaseChars[rnd.Next(BaseChars.Length - 10) + 10]);
                _Length--;
                while (_Length > 0)
                {
                    r.Append(BaseChars[rnd.Next(BaseChars.Length)]);
                    _Length--;
                }
            }
            return r.ToString();
        }

        /// <summary>Returns a string containing length spaces.</summary>
        public string Space(int _Length)
        {
            StringBuilder s;
            if (_Length > 0)
            {
                s = new StringBuilder();
                s.Append(' ', _Length);
                return s.ToString();
            }
            else return "";
        }

        /// <summary>Return collection of strings splitted from original string 
        /// considering separator chars.</summary>
        public List<string> Split(string _String, string _Separators, bool _TrimSpaces)
        {
            List<string> r = new List<string>();
            if (_TrimSpaces)
            {
                while (_String.Trim().Length > 0) r.Add(Extract(ref _String, _Separators).Trim());
            }
            else
            {
                while (_String.Trim().Length > 0) r.Add(Extract(ref _String, _Separators));
            }
            return r;
        }

        /// <summary>Store collection of strings splitted from original string 
        /// considering separator chars.</summary>
        public void Split(List<string> _StringList, string _String, string _Separators, bool _TrimSpaces)
        {
            if (_StringList != null)
            {
                if (_TrimSpaces)
                {
                    while (_String.Trim().Length > 0) _StringList.Add(Extract(ref _String, _Separators).Trim());
                }
                else
                {
                    while (_String.Trim().Length > 0) _StringList.Add(Extract(ref _String, _Separators));
                }
            }
        }

        /// <summary>Return collection of strings splitted from original string 
        /// considering carriage return.</summary>
        public List<string> SplitLines(string _String, bool _TrimSpaces)
        {
            List<string> r = new List<string>();
            if (_TrimSpaces)
            {
                while (_String.Trim().Length > 0) r.Add(ExtractLine(ref _String).Trim());
            }
            else
            {
                while (_String.Trim().Length > 0) r.Add(ExtractLine(ref _String));
            }
            return r;
        }

        /// <summary>Return collection of strings splitted from original string 
        /// considering carriage return.</summary>
        public void SplitLines(List<string> _StringList, string _String, bool _TrimSpaces)
        {
            if (_StringList != null)
            {
                if (_TrimSpaces)
                {
                    while (_String.Trim().Length > 0) _StringList.Add(ExtractLine(ref _String).Trim());
                }
                else
                {
                    while (_String.Trim().Length > 0) _StringList.Add(ExtractLine(ref _String));
                }
            }
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

        /// <summary>Returns "1" if b is true, otherwise returns "0".</summary>
        public string ToBool(bool _BoolValue)
        {
            if (_BoolValue) return "1";
            else return "0";
        }

        /// <summary>Returns double value of number represented in string. Return 0 if fail. Same as Val().</summary>
        public double ToDouble(string _String)
        {
            try
            {
                return Convert.ToDouble(GetDigits(_String, true));
            }
            catch
            {
                return 0.0d;
            }
        }

        /// <summary>Returns double value of number represented in object. Return 0 if fail.</summary>
        public double ToDouble(object _Value)
        {
            if (_Value == null) return 0.0d;
            else if (_Value is double) return (double)_Value;
            else return ToDouble(_Value.ToString());
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
            if (_String.Length > 0)
            {
                if (IsHexMask(_String)) _String = FromHexMask(_String, _Password);
                _String += HexSum(_String);
                _String = ToHexDump(_String, _Password);
                return "{" + _String + HexSum(_String) + "}";
            }
            else return "";
        }

        /// <summary>Returns integer value of number represented in string. Return 0 if fail.</summary>
        public int ToInt(string _Value)
        {
            try
            {
                return Convert.ToInt32(GetDigits(_Value, false));
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>Returns integer value of number represented in object. Return 0 if fail.</summary>
        public int ToInt(object _Value)
        {
            if (_Value == null) return 0;
            else if (_Value is int) return (int)_Value;
            else return ToInt(_Value.ToString());
        }

        /// <summary>Returns long integer value of number represented in string. Return 0 if fail.</summary>
        public long ToLong(string _String)
        {
            try
            {
                return Convert.ToInt64(GetDigits(_String, false));
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>Returns long integer value of number represented in object. Return 0 if fail.</summary>
        public long ToLong(object _Value)
        {
            if (_Value == null) return 0;
            else if ((_Value is long) || (_Value is int)) return (long)_Value;
            else return ToLong(_Value.ToString());
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

        /// <summary>Return string or empty if null.</summary>
        public string ToStr(string _String)
        {
            if (_String == null) return "";
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

        /// <summary>Return string with all strings in array separated by default carriage-return.</summary>
        public string ToStr(string[] _Strings)
        {
            int i;
            StringBuilder r = new StringBuilder();
            if (_Strings != null)
            {
                for (i = 0; i < _Strings.Length; i++)
                {
                    r.Append(_Strings[i]);
                    r.Append("\r\n");
                }
            }
            return r.ToString();
        }

        /// <summary>Return string with all strings in list separated by default carriage-return.</summary>
        public string ToStr(List<string> _Strings, bool _TrimStrings)
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
                        r.Append("\r\n");
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

        /// <summary>Return string with all chars invalid for name replaced by undescore symbol.</summary>
        public string ToValidName(string _String)
        {
            return ChrReplace(_String, "\\|!\"£$%&/()=?^'[]{}*+§@#°,;.:-<> ", '_');
        }

        /// <summary>Returns string without leading spaces.</summary>
        public string TrimL(string _String)
        {
            return _String.TrimStart();
        }

        /// <summary>Returns string value removing all leading (from left) recurrences.</summary>
        public string TrimL(string _String, string _Recurrence)
        {
            int l = _Recurrence.Length;
            while (_String.StartsWith(_Recurrence)) _String = Mid(_String, l, _String.Length - l);
            return _String;
        }

        /// <summary>Returns string without ending spaces.</summary>
        public string TrimR(string _String)
        {
            return _String.TrimEnd();
        }

        /// <summary>Returns string value removing all ending (from right) recurrences.</summary>
        public string TrimR(string _String, string _Recurrence)
        {
            int l = _Recurrence.Length;
            while (_String.EndsWith(_Recurrence)) _String = Mid(_String, 0, _String.Length - l);
            return _String;
        }

        /// <summary>Return truncated string to max length with ellipses.</summary>
        public string Trunc(string _String, int _MaxLength)
        {
            if (_String.Length > _MaxLength) return Mid(_String, 0, _MaxLength - 3) + "...";
            else return _String;
        }

        /// <summary>Return string unescaping limited special chars.</summary>
        public string Unescape(string _String)
        {
            int i = 0;
            bool e = false;
            StringBuilder s = new StringBuilder();
            while (i < _String.Length)
            {
                if (e)
                {
                    if (_String[i] == 'n') s.Append("\n");
                    else if (_String[i] == 'r') s.Append("\r");
                    else if (_String[i] == 't') s.Append("\t");
                    else if (_String[i] == '\'') s.Append("\'");
                    else if (_String[i] == '\"') s.Append("\"");
                    else if (_String[i] == '\\') s.Append("\\");
                    else if (_String[i] == '\0') s.Append("\0");
                }
                else if (_String[i] == '\\') e = true;
                else s.Append(_String[i]);
            }
            return s.ToString();
        }

        /// <summary>Returns string removing eventually single quote at extremes.</summary>
        public string Unquote(string _String)
        {
            if (_String.Length > 1)
            {
                if ((_String[0] == '\'') && (_String[_String.Length - 1] == '\''))
                {
                    if (_String.Length > 2) return _String.Substring(1, _String.Length - 2);
                    else return "";
                }
                else return _String;
            }
            else return _String;
        }

        /// <summary>Returns string removing eventually double quote at extremes.</summary>
        public string Unquote2(string _String)
        {
            if (_String.Length > 1)
            {
                if ((_String[0] == '"') && (_String[_String.Length - 1] == '"'))
                {
                    if (_String.Length > 2) return _String.Substring(1, _String.Length - 2);
                    else return "";
                }
                else return _String;
            }
            else return _String;
        }

        /// <summary>Returns string converted in uppercase.</summary>
        public string Upper(string _String)
        {
            return _String.ToUpper();
        }

        /// <summary>Returns string with first char converted in uppercase.</summary>
        public string UpperFirst(string _String)
        {
            if (_String.Length > 1) return _String[0].ToString().ToUpper() + _String.Substring(1);
            else return _String.ToUpper();
        }

        /// <summary>Returns double value of number represented in string. Return 0 if fail. Same as ToDouble().</summary>
        public double Val(string _String)
        {
            try
            {
                return Convert.ToDouble(GetDigits(_String, true));
            }
            catch
            {
                return 0.0d;
            }
        }

        /// <summary>Returns string with the part between first substring and second substring replaced by new string.</summary>
        public string WBtw(string _String, string _NewString, string _BeginSubstring, string _EndSubstring)
        {
            string a, b;
            int i = _String.IndexOf(_BeginSubstring);
            if (i > -1)
            {
                a = Mid(_String, 0, i) + _BeginSubstring + _NewString;
                b = Mid(_String, i + _BeginSubstring.Length, _String.Length);
                i = b.IndexOf(_EndSubstring);
                if (i > -1) return a + Mid(b, i, b.Length);
                else return _String;
            }
            else return _String;
        }

        /// <summary>Returns regular expression related to wildcard.</summary>
        public string WildCardToRegEx(string _WildCard)
        {
            return "^" + Regex.Escape(_WildCard).Replace("\\*", ".*").Replace("\\?", ".") + "$";
        }

        /// <summary>Returns if string match with wildcard. Last parameter indicate 
        /// if matching will be case sensitive.</summary>
        public bool WildCardMatch(string _String, string _WildCard, bool _CaseSensitive)
        {
            Regex r;
            _WildCard = WildCardToRegEx(_WildCard);
            if (_CaseSensitive) r = new Regex(_WildCard);
            else r = new Regex(_WildCard, RegexOptions.IgnoreCase);
            return r.IsMatch(_String);
        }

        /// <summary>Return text wrapped in lines with char width.
        /// If flat string is true, all CR will be removed.</summary>
        public string Wrap(string _Text, int _CharWidth, bool _FlatString)
        {
            int i = 0;
            string l, p;
            StringBuilder r = new StringBuilder();
            _Text = _Text.Trim(MemoTrimChars);
            if (_FlatString) _Text = Flat(_Text).Trim();
            while (_Text.Length > 0)
            {
                l = ExtractLine(ref _Text).Trim();
                if ((l.Length > 0) && (r.Length > 0)) r.AppendLine("");
                while (l.Length > 0)
                {
                    p = Extract(ref l, " ").Trim();
                    if (p.Length > 0)
                    {
                        if (i + p.Length > _CharWidth)
                        {
                            if (r.Length > 0) r.Append(CR);
                            i = 0;
                        }
                        if (i > 0)
                        {
                            r.Append(' ');
                            i++;
                        }
                        r.Append(p);
                        i += p.Length;
                    }
                }
            }
            return r.ToString();
        }

        /// <summary>Returns a string with length characters, representing integer value leaded by zeroes.</summary>
        public string Zeroes(double _Value, int _Length)
        {
            string r = Math.Truncate(_Value).ToString();
            if (r.Length > _Length) r = Right(r, _Length);
            else while (r.Length < _Length) r = '0' + r;
            return r;
        }

        /// <summary>Returns a string with length characters, representing integer value leaded by zeroes.</summary>
        public string Zeroes(int _Value, int _Length)
        {
            string r = _Value.ToString();
            if (r.Length > _Length) r = Right(r, _Length);
            else while (r.Length < _Length) r = '0' + r;
            return r;
        }

        #endregion

        /* */

    }

    /* */

}
