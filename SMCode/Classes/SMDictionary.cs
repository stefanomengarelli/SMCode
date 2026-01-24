/*  ===========================================================================
 *  
 *  File:       SMDictionary.cs
 *  Version:    2.0.321
 *  Date:       January 2026
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2025 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *
 *  SMCode dictionary class.
 *
 *  ===========================================================================
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace SMCodeSystem
{

    /* */

    /// <summary>SMCode dictionary class.</summary>
    public class SMDictionary
    {

        /* */

        #region Declarations

        /*  ===================================================================
         *  Declarations
         *  ===================================================================
         */

        /// <summary>SM session instance.</summary>
        private readonly SMCode SM = null;

        /// <summary>Ignore case flag.</summary>
        private bool ignoreCase = true;

        /// <summary>Dictionary items collection.</summary>
        private List<SMDictionaryItem> items = new List<SMDictionaryItem>();

        /// <summary>Sorted list flag.</summary>
        private bool sorted = true;

        #endregion

        /* */

        #region Properties

        /*  ===================================================================
         *  Properties
         *  ===================================================================
         */

        /// <summary>Quick access declaration.</summary>
        public SMDictionaryItem this[int _Index]
        {
            get { return items[_Index]; }
            set
            {
                items[_Index] = value;
                if (sorted) Sort();
            }
        }

        /// <summary>Get items count.</summary>
        public int Count { get { return items.Count; } }

        /// <summary>Return true of dictionary has no items.</summary>
        public bool Empty
        {
            get
            {
                if (items == null) return true;
                else return items.Count < 1;
            }
        }

        /// <summary>Get or set ignore case flag.</summary>
        public bool IgnoreCase
        {
            get { return ignoreCase; }
            set
            {
                if (ignoreCase != value)
                {
                    ignoreCase = value;
                    if (sorted) Sort();
                }
            }
        }

        /// <summary>Get last parameters setted.</summary>
        public string Parameters { get; private set; }

        /// <summary>Get or set sorted list.</summary>
        public bool Sorted
        {
            get { return sorted; }
            set
            {
                if (sorted != value)
                {
                    sorted = value;
                    if (sorted) Sort();
                }
            }
        }

        /// <summary>Get or set parameters trailing char.</summary>
        public char TrailingChar { get; set; } = '\\';

        #endregion

        /* */

        #region Initialization

        /*  ===================================================================
         *  Initialization
         *  ===================================================================
         */

        /// <summary>Class constructor.</summary>
        public SMDictionary(SMCode _SM = null)
        {
            SM = SMCode.CurrentOrNew(_SM);
            Clear();
        }

        /// <summary>Class constructor.</summary>
        public SMDictionary(SMDictionary _Dictionary, SMCode _SM = null)
        {
            if (_SM == null) _SM = _Dictionary.SM;
            SM = SMCode.CurrentOrNew(_SM);
            Assign(_Dictionary);
        }

        /// <summary>Class constructor.</summary>
        public SMDictionary(string _JSON, SMCode _SM = null)
        {
            SM = SMCode.CurrentOrNew(_SM);
            FromJSON(_JSON);
        }

        /// <summary>Class constructor.</summary>
        public SMDictionary(string[] _KeyValueArray, SMCode _SM = null)
        {
            int i;
            if (_SM == null) SM = SMCode.CurrentOrNew();
            else SM = _SM;
            Clear();
            if (_KeyValueArray != null)
            {
                i = 0;
                while (i < _KeyValueArray.Length - 1)
                {
                    if (!SM.Empty(_KeyValueArray[i]))
                    {
                        Add(new SMDictionaryItem(_KeyValueArray[i].Trim(), _KeyValueArray[i + 1], null));
                    }
                    i += 2;
                }
            }
        }

        #endregion

        /* */

        #region Methods

        /*  ===================================================================
         *  Methods
         *  ===================================================================
         */

        /// <summary>Add dictionary item and sort collection.</summary>
        public int Add(SMDictionaryItem _DictionaryItem)
        {
            items.Add(_DictionaryItem);
            if (sorted) return Sort(true);
            else return items.Count - 1;
        }

        /// <summary>Add dictionary item and sort collection.</summary>
        public int Add(string _Key, string _Value, object _Tag = null, Type _Type = null)
        {
            return Add(new SMDictionaryItem(_Key, _Value, _Tag, _Type));
        }

        /// <summary>Assign instance properties from another.</summary>
        public void Assign(SMDictionary _Dictionary)
        {
            int i;
            sorted = _Dictionary.sorted;
            ignoreCase = _Dictionary.ignoreCase;
            Parameters = _Dictionary.Parameters;
            items.Clear();
            if (_Dictionary.items != null)
            {
                for (i = 0; i < _Dictionary.items.Count; i++)
                {
                    Add(_Dictionary.items[i]);
                }
            }
        }

        /// <summary>Return boolean value of first items with passed key.
        /// Return default value if not found.</summary>
        public bool BoolOf(string _Key, bool _Default = false)
        {
            return SM.ToBool(ValueOf(_Key, SM.ToBool(_Default)));
        }

        /// <summary>Clear item.</summary>
        public void Clear()
        {
            Parameters = "";
            items.Clear();
        }

        /// <summary>Return datetime value of first items with passed key.
        /// Return default value if not found.</summary>
        public DateTime DateOf(string _Key, DateTime? _Default = null)
        {
            if (_Default == null) _Default = DateTime.MinValue;
            return SM.ToDate(ValueOf(_Key, SM.ToStr(_Default.Value)));
        }

        /// <summary>Find first item with passed key. It possible to indicate 
        /// if search must be binary otherwise sequential. 
        /// Return item index or -1 if not found.</summary>
        public int Find(string _Key)
        {
            int i, max, mid, min, rslt = -1;
            if (items.Count > 0)
            {
                if (sorted)
                {
                    //
                    // binary search
                    //
                    min = 0;
                    max = items.Count - 1;
                    while ((rslt < 0) && (min <= max))
                    {
                        mid = (min + max) / 2;
                        i = String.Compare(_Key, items[mid].Key, ignoreCase);
                        if (i == 0) rslt = mid;
                        else if (i < 0) max = mid - 1;
                        else min = mid + 1;
                    }
                    //
                    // search first
                    //
                    while (rslt > 0)
                    {
                        if (String.Compare(_Key, items[rslt - 1].Key, ignoreCase) == 0) rslt--;
                        else break;
                    }
                }
                else
                {
                    //
                    // sequential search
                    //
                    i = 0;
                    while ((rslt < 0) && (i < items.Count))
                    {
                        if (String.Compare(_Key, items[i].Key, ignoreCase) == 0) rslt = i;
                        i++;
                    }
                }
            }
            return rslt;
        }

        /// <summary>Find first item with passed value below matching mode.
        /// Return item index or -1 if not found.</summary>
        public int FindValue(string _Value, SMStringMatch _MatchingMode = SMStringMatch.Equal, bool _IgnoreCase = false)
        {
            int i = 0, rslt = -1;
            while ((rslt < 0) && (i < items.Count))
            {
                if (SM.Compare(items[i].Value, _Value, _MatchingMode, _IgnoreCase)) rslt = i;
                i++;
            }
            return rslt;
        }

        /// <summary>Load dictionary from object passed.</summary>
        public bool FromObject(object _Object, bool _ValuesOnTag = false)
        {
            int i;
            object v;
            string s;
            PropertyInfo property;
            PropertyInfo[] properties;
            SMDictionaryItem item;
            try
            {
                Clear();
                if (_Object != null)
                {
                    properties = _Object.GetType().GetProperties();
                    if (properties != null)
                    {
                        for (i = 0; i < properties.Length; i++)
                        {
                            s = "";
                            property = properties[i];
                            if (property.CanRead)
                            {
                                v = property.GetValue(_Object);
                                if (SM.IsValuableType(property.PropertyType))
                                {
                                    s = SM.ToStr(SM.ToType(v, SMDataType.String));
                                    item = new SMDictionaryItem(property.Name, s, null, property.PropertyType);
                                    if (_ValuesOnTag) item.Tag = v;
                                    Add(item);
                                }
                            }
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                SM.Error(ex);
                return false;
            }
        }

        /// <summary>Load dictionary from parameters string "key1=value1; ... keyN=valueN;".</summary>
        public bool FromParameters(string _Value)
        {
            string k, v;
            char[] aStart = new char[] { ' ', '-' }, aEnd = new char[] { '=', ' ', ';' };
            try
            {
                Clear();
                _Value = _Value.TrimStart();
                Parameters = _Value;
                while (_Value.Trim().Length > 0)
                {
                    k = SM.ExtractArgument(ref _Value, "=;", true, TrailingChar);
                    if (k.EndsWith("="))
                    {
                        if (k.Length > 0) k = k.Substring(0, k.Length - 1).TrimStart(aStart).TrimEnd(aEnd);
                        if (k.Length > 0)
                        {
                            v = SM.ExtractArgument(ref _Value, ";", false, TrailingChar);
                            Add(k, v, null);
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                SM.Error(ex);
                return false;
            }
        }

        /// <summary>Load dictionary from CSV string.</summary>
        public bool FromCSV(string _Value)
        {
            string l, k, v;
            try
            {
                Clear();
                while (_Value.Trim().Length > 0)
                {
                    l = SM.ExtractLine(ref _Value).Trim();
                    if (l.Length > 0)
                    {
                        k = SM.ExtractCSV(ref l);
                        v = SM.ExtractCSV(ref l);
                        Add(k, v, null);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                SM.Error(ex);
                return false;
            }
        }

        /// <summary>Load dictionary from JSON string.</summary>
        public bool FromJSON(string _Value)
        {
            int i;
            Dictionary<string, string> dict;
            try
            {
                Clear();
                if (!SM.Empty(_Value))
                {
                    dict = JsonSerializer.Deserialize<Dictionary<string, string>>(_Value);
                    for (i = 0; i < dict.Count; i++)
                    {
                        Add(dict.ElementAt(i).Key, dict.ElementAt(i).Value, null);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                SM.Error(ex);
                return false;
            }
        }

        /// <summary>Load dictionary from base 64 JSON string.</summary>
        public bool FromJSON64(string _Value)
        {
            return FromJSON(SM.Base64Decode(_Value));
        }

        /// <summary>Return value of first items with passed key.
        /// Return default string if not found.</summary>
        public string Get(string _Key, string _Default = "")
        {
            int i = Find(_Key);
            if (i > -1) return items[i].Value;
            else return _Default;
        }

        /// <summary>Return item by key or null if not found.</summary>
        public SMDictionaryItem GetItem(string _Key, bool _NewIfNotFound = false)
        {
            int i = Find(_Key);
            if (i < 0)
            {
                if (_NewIfNotFound) return new SMDictionaryItem();
                else return null;
            }
            else return items[i];
        }

        /// <summary>Return integer value of first items with passed key.
        /// Return default value if not found.</summary>
        public int IntOf(string _Key, int _Default = 0)
        {
            return SM.ToInt(ValueOf(_Key, _Default.ToString()));
        }

        /// <summary>Return keys list as a string with separator and quote specified.</summary>
        public string Keys(string _Quote = "", string _Separator = ",")
        {
            int i = 0;
            StringBuilder sb = new StringBuilder();
            while (i < items.Count)
            {
                if (i > 0) sb.Append(_Separator);
                sb.Append(_Quote);
                sb.Append(items[i].Key);
                sb.Append(_Quote);
                i++;
            }
            return sb.ToString();
        }

        /// <summary>Merge dictionary keys with passed dictionary values.</summary>
        public SMDictionary Merge(SMDictionary _Dictionary)
        {
            int i;
            if (_Dictionary != null)
            {
                for (i = 0; i < _Dictionary.Count; i++)
                {
                    Set(_Dictionary[i].Key, _Dictionary[i].Value, _Dictionary[i].Tag, _Dictionary[i].Type);
                }
            }
            return this;
        }

        /// <summary>Set key item to string value, and tag.</summary>
        public int Set(string _Key, string _Value, object _Tag = null, Type _Type = null)
        {
            int i = Find(_Key);
            if (i > -1)
            {
                items[i].Value = _Value;
                items[i].Tag = _Tag;
                items[i].Type = _Type;
            }
            else i = Add(_Key, _Value, _Tag, _Type);
            return i;
        }

        /// <summary>Set key item to boolean value, and tag.</summary>
        public int Set(string _Key, bool _Value, object _Tag = null, Type _Type = null)
        {
            return Set(_Key, SM.ToBool(_Value), _Tag, _Type);
        }

        /// <summary>Set key item to integer value, and tag.</summary>
        public int Set(string _Key, int _Value, object _Tag = null, Type _Type = null)
        {
            return Set(_Key, _Value.ToString(), _Tag, _Type);
        }

        /// <summary>Set key item to datetime value, and tag.</summary>
        public int Set(string _Key, DateTime _Value, object _Tag = null, Type _Type=null)
        {
            return Set(_Key, SM.ToStr(_Value), _Tag, _Type);
        }

        /// <summary>Sort list.</summary>
        public int Sort(bool _AppendOnSortedList = false)
        {
            bool b = true;
            int i = items.Count - 1, rslt = -1;
            SMDictionaryItem swap;
            while (b)
            {
                b = false;
                rslt = i;
                while (i > 0)
                {
                    if (String.Compare(items[i].Key, items[i - 1].Key, ignoreCase) < 0)
                    {
                        b = true;
                        swap = items[i];
                        items[i] = items[i - 1];
                        items[i - 1] = swap;
                        rslt--;
                        i--;
                    }
                    else if (_AppendOnSortedList) i = 0;
                }
            }
            return rslt;
        }

        /// <summary>Return value of first items with passed key.
        /// Return default string if not found.</summary>
        public string StrOf(string _Key, string _Default = "")
        {
            return ValueOf(_Key, _Default);
        }

        /// <summary>Return tag of first items with passed key.
        /// Return null if not found.</summary>
        public object TagOf(string _Key)
        {
            int i = Find(_Key);
            if (i > -1) return items[i].Tag;
            else return null;
        }

        /// <summary>Return item to CSV string.</summary>
        public string ToCSV()
        {
            int i;
            StringBuilder r = new StringBuilder();
            for (i = 0; i < items.Count; i++)
            {
                r.Append(SM.AddCSV(SM.AddCSV("", items[i].Key), items[i].Value));
                r.Append(SM.CR);
            }
            return r.ToString();
        }

        /// <summary>Return item to JSON string.</summary>
        public string ToJSON()
        {
            int i;
            string r = "";
            Dictionary<string, string> dict;
            try
            {
                dict = new Dictionary<string, string>();
                for (i = 0; i < items.Count; i++)
                {
                    dict.Add(items[i].Key, items[i].Value);
                }
                r = JsonSerializer.Serialize(dict);
            }
            catch (Exception ex)
            {
                SM.Error(ex);
            }
            return r;
        }

        /// <summary>Return item to base 64 JSON string.</summary>
        public string ToJSON64()
        {
            return SM.Base64Encode(ToJSON());
        }

        /// <summary>Load object properties from dictionary passed.</summary>
        public bool ToObject(object _Object, bool _ValuesOnTag = false)
        {
            int i;
            try
            {
                if (_Object != null)
                {
                    foreach (System.Reflection.PropertyInfo prop in _Object.GetType().GetProperties())
                    {
                        if (prop.CanWrite && SM.IsValuableType(prop.PropertyType))
                        {
                            i = Find(prop.Name);
                            if (i > -1)
                            {
                                if (_ValuesOnTag) prop.SetValue(_Object, SM.ToType(items[i].Tag, prop.PropertyType));
                                else prop.SetValue(_Object, SM.ToType(items[i].Value, prop.PropertyType));
                            }
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                SM.Error(ex);
                return false;
            }
        }

        /// <summary>Return parameters prompt string.</summary>
        public string ToParameters()
        {
            int i;
            string trail = "" + TrailingChar, trail2 = trail + TrailingChar;
            StringBuilder sb = new StringBuilder();
            for (i = 0; i < items.Count; i++)
            {
                sb.Append(items[i].Key);
                sb.Append('=');
                if (TrailingChar == '\0') sb.Append(SM.Quote2(items[i].Value));
                else sb.Append(SM.Quote2(items[i].Value).Replace(trail, trail2));
                sb.Append(';');
            }
            return sb.ToString();
        }

        /// <summary>Return value of first items with passed key.
        /// Return default string if not found.</summary>
        public string ValueOf(string _Key, string _Default = "")
        {
            int i = Find(_Key);
            if (i > -1) return items[i].Value;
            else return _Default;
        }

        #endregion

        /* */

    }

    /* */

}
