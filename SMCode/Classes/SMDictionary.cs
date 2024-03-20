/*  ===========================================================================
 *  
 *  File:       SMDictionary.cs
 *  Version:    1.0.0
 *  Date:       February 2024
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2024 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *
 *  SMCode dictionary class.
 *
 *  ===========================================================================
 */

using System.Reflection.Metadata;
using System.Text;
using System.Text.Json;

namespace SMCode
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
        private readonly SMApplication SM = null;

        /// <summary>Dictionary items collection.</summary>
        private List<SMDictionaryItem> items = new List<SMDictionaryItem>();

        /// <summary>Last index found.</summary>
        private int lastIndex = -1;

        /// <summary>Last key found.</summary>
        private string lastKey = "";

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
                else ResetLastFound();
            }
        }

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
                    else ResetLastFound();
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
        public SMDictionary(SMApplication _SMApplication)
        {
            if (_SMApplication == null) SM = SMApplication.Application;
            else SM = _SMApplication;
            Clear();
        }

        /// <summary>Class constructor.</summary>
        public SMDictionary(SMApplication _SMApplication, SMDictionary _Dictionary)
        {
            if (_SMApplication == null) SM = SMApplication.Application;
            else SM = _SMApplication;
            Assign(_Dictionary);
        }

        #endregion

        /* */

        #region Methods

        /*  ===================================================================
         *  Methods
         *  ===================================================================
         */

        /// <summary>Add dictionary item and sort collection.</summary>
        public void Add(SMDictionaryItem _DictionaryItem)
        {
            ResetLastFound();
            items.Add(_DictionaryItem);
            if (sorted) Sort(true);
        }

        /// <summary>Add dictionary item and sort collection.</summary>
        public void Add(string _Key, string _Value, object _Tag = null)
        {
            Add(SM.NewDictionaryItem(_Key, _Value, _Tag));
        }

        /// <summary>Assign instance properties from another.</summary>
        public void Assign(SMDictionary _Dictionary)
        {
            int i;
            ResetLastFound();
            sorted = _Dictionary.sorted;
            IgnoreCase = _Dictionary.IgnoreCase;
            items.Clear();
            if (_Dictionary.items != null)
            {
                for (i = 0; i < _Dictionary.items.Count; i++)
                {
                    items.Add(_Dictionary.items[i]);
                }
            }
        }

        /// <summary>Clear item.</summary>
        public void Clear()
        {
            ResetLastFound();
            items.Clear();
        }

        /// <summary>Find first item with passed key. It possible to indicate 
        /// if search must be binary otherwise sequential. 
        /// Return item index or -1 if not found.</summary>
        public int Find(string _Key)
        {
            int i, max, mid, min, r = -1;
            if (items.Count > 0)
            {
                if ((lastKey == _Key) && (lastIndex > -1)) r = lastIndex;
                else if (sorted)
                {
                    //
                    // binary search
                    //
                    min = 0;
                    max = items.Count - 1;
                    while ((r < 0) && (min <= max))
                    {
                        mid = (min + max) / 2;
                        i = String.Compare(_Key, items[mid].Key, IgnoreCase);
                        if (i == 0) r = mid;
                        else if (i < 0) max = mid - 1;
                        else min = mid + 1;
                    }
                    //
                    // search first
                    //
                    while (r > 0)
                    {
                        if (String.Compare(_Key, items[r - 1].Key, IgnoreCase) == 0) r--;
                        else break;
                    }
                    lastIndex = r;
                    lastKey = _Key;
                }
                else
                {
                    //
                    // sequential search
                    //
                    i = 0;
                    while ((r < 0) && (i < items.Count))
                    {
                        if (String.Compare(_Key, items[i].Key, IgnoreCase) == 0) r = i;
                        i++;
                    }
                    lastIndex = r;
                    lastKey = _Key;
                }
            }
            return r;
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
            catch
            {
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
                dict = JsonSerializer.Deserialize<Dictionary<string, string>>(_Value);
                for (i = 0; i < dict.Count; i++)
                {
                    Add(dict.ElementAt(i).Key, dict.ElementAt(i).Value, null);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>Load dictionary from base 64 JSON string.</summary>
        public bool FromJSON64(string _Value)
        {
            return FromJSON(SM.Base64Decode(_Value));
        }

        /// <summary>Reset last element found cache.</summary>
        private void ResetLastFound()
        {
            lastIndex = -1;
            lastKey = "";
        }

        /// <summary>Sort list.</summary>
        public void Sort(bool _AppendOnSortedList = false)
        {
            bool b = true;
            int i = items.Count;
            SMDictionaryItem swap;
            ResetLastFound();
            while (b)
            {
                b = false;
                while (i > 1)
                {
                    i--;
                    if (String.Compare(items[i].Key, items[i - 1].Key, IgnoreCase) < 0)
                    {
                        b = true;
                        swap = items[i];
                        items[i] = items[i - 1];
                        items[i - 1] = swap;
                    }
                    else if (_AppendOnSortedList) i = 0;
                }
            }
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

        /// <summary>Return value of first items with passed key.
        /// Return empty string if not found.</summary>
        public string ValueOf(string _Key)
        {
            int i = Find(_Key);
            if (i > -1) return items[i].Value;
            else return "";
        }

        #endregion

        /* */

    }

    /* */

}
