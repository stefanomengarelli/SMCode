/*  ===========================================================================
 *  
 *  File:       SMDictionary.cs
 *  Version:    2.0.252
 *  Date:       May 2025
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

        /// <summary>Get last parameters setted.</summary>
        public string Parameters { get; private set; }

        /// <summary>Get items count.</summary>
        public int Count { get { return items.Count; } }

        /// <summary>Get or set ignore case flag.</summary>
        public bool IgnoreCase { get; set; } = false;

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
            IgnoreCase = _Dictionary.IgnoreCase;
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
                        i = String.Compare(_Key, items[mid].Key, IgnoreCase);
                        if (i == 0) rslt = mid;
                        else if (i < 0) max = mid - 1;
                        else min = mid + 1;
                    }
                    //
                    // search first
                    //
                    while (rslt > 0)
                    {
                        if (String.Compare(_Key, items[rslt - 1].Key, IgnoreCase) == 0) rslt--;
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
                        if (String.Compare(_Key, items[i].Key, IgnoreCase) == 0) rslt = i;
                        i++;
                    }
                }
            }
            return rslt;
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
                    k = SM.ExtractArgument(ref _Value, "=;", true);
                    if (k.EndsWith("="))
                    {
                        if (k.Length > 0) k = k.Substring(0, k.Length - 1).TrimStart(aStart).TrimEnd(aEnd);
                        if (k.Length > 0)
                        {
                            v = SM.ExtractArgument(ref _Value, ";", false);
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

        /// <summary>Merge to current dictionary items from another.</summary>
        public void Merge(SMDictionary _Dictionary)
        {
            int i, j;
            if (_Dictionary != null)
            {
                for (i = 0; i < _Dictionary.items.Count; i++)
                {
                    j = Find(_Dictionary.items[i].Key);
                    if (j < 0)
                    {
                        Add(new SMDictionaryItem(_Dictionary.items[i]));
                    }
                    else
                    {
                        items[j].Value = _Dictionary.items[i].Value;
                        items[j].Tag = _Dictionary.items[i].Tag;
                    }
                }
            }
        }

        /// <summary>Set key item to string value, and tag.</summary>
        public int Set(string _Key, string _Value, object _Tag = null, Type _Type = null)
        {
            int i = Find(_Key);
            if (i < 0)
            {
                i = Add(_Key, _Value, _Tag, _Type);
            }
            else
            {
                items[i].Value = _Value;
                items[i].Tag = _Tag;
                items[i].Type = _Type;
            }
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

        /// <summary>Set dictionary keys with passed dictionary values.</summary>
        public void Set(SMDictionary _Dictionary)
        {
            int i;
            for (i = 0; i < _Dictionary.Count; i++)
            {
                Set(_Dictionary[i].Key, _Dictionary[i].Value, _Dictionary[i].Tag);
            }
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
                    if (String.Compare(items[i].Key, items[i - 1].Key, IgnoreCase) < 0)
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
        
        /// <summary>Return parameters prompt string.</summary>
        public string ToParameters()
        {
            int i;
            StringBuilder sb = new StringBuilder();
            for (i=0; i<Parameters.Count(); i++)
            {
                sb.Append(items[i].Key + '=' + SM.Quote2(items[i].Value) + ';');
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
