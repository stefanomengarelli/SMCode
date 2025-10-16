/*  ===========================================================================
 *  
 *  File:       SMDictionaryItem.cs
 *  Version:    2.0.252
 *  Date:       May 2025
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

using System;

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

        /// <summary>Get or set item type.</summary>
        public Type Type { get; set; }

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
        public SMDictionaryItem(string _Key, string _Value, object _Tag, Type _Type = null)
        {
            Key = _Key;
            Value = _Value;
            Tag = _Tag;
            Type = _Type;
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
            Type = _DictionaryItem.Type;
        }

        /// <summary>Clear item.</summary>
        public void Clear()
        {
            Key = "";
            Value = "";
            Tag = null;
            Type = null;
        }

        /// <summary>Compare this dictionary item instance with passed.</summary>
        public int Compare(SMDictionaryItem _DictionaryItem)
        {
            return Key.CompareTo(_DictionaryItem.Key);
        }

        /// <summary>Return string representation of this instance.</summary>
        public override string ToString()
        {
            return "SMCodeSystem.SMDictionaryItem { Key: \"" + Key + "\"; Value: \"" + Value + "\" }";
        }

        #endregion

        /* */

    }

    /* */

}
