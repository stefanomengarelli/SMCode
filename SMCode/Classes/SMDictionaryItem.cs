/*  ===========================================================================
 *  
 *  File:       SMDictionaryItem.cs
 *  Version:    2.0.16
 *  Date:       May 2024
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2024 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *
 *  SMCode dictionary item class.
 *
 *  ===========================================================================
 */

namespace SMCodeSystem
{

    /* */

    /// <summary>SMCode dictionary item class.</summary>
    public class SMDictionaryItem
    {

        /* */

        #region Properties

        /*  ===================================================================
         *  Properties
         *  ===================================================================
         */

        /// <summary>Get or set item key.</summary>
        public string Key { get; set; }

        /// <summary>Get or set item value.</summary>
        public string Value { get; set; }

        /// <summary>Get or set item tag object.</summary>
        public object Tag { get; set; }

        #endregion

        /* */

        #region Initialization

        /*  ===================================================================
         *  Initialization
         *  ===================================================================
         */

        /// <summary>Class constructor.</summary>
        public SMDictionaryItem()
        {
            Clear();
        }

        /// <summary>Class constructor.</summary>
        public SMDictionaryItem(SMDictionaryItem _DictionaryItem)
        {
            Assign(_DictionaryItem);
        }

        /// <summary>Class constructor.</summary>
        public SMDictionaryItem(string _Key, string _Value, object _Tag)
        {
            Key = _Key;
            Value = _Value;
            Tag = _Tag;
        }

        #endregion

        /* */

        #region Methods

        /*  ===================================================================
         *  Methods
         *  ===================================================================
         */

        /// <summary>Assign instance properties from another.</summary>
        public void Assign(SMDictionaryItem _DictionaryItem)
        {
            Key = _DictionaryItem.Key;
            Value = _DictionaryItem.Value;
            Tag = _DictionaryItem.Tag;
        }

        /// <summary>Clear item.</summary>
        public void Clear()
        {
            Key = "";
            Value = "";
            Tag = null;
        }

        /// <summary>Compare this dictionary item instance with passed.</summary>
        public int Compare(SMDictionaryItem _DictionaryItem)
        {
            return Key.CompareTo(_DictionaryItem.Key);
        }

        #endregion

        /* */

    }

    /* */

}
