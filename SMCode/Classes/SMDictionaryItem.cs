/*  ===========================================================================
 *  
 *  File:       SMDictionaryItem.cs
 *  Version:    2.0.0
 *  Date:       February 2024
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2024 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *
 *  SMCode dictionary item class.
 *
 *  ===========================================================================
 */

namespace SMCode
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

        /// <summary>SM session instance.</summary>
        private readonly SMApplication SM = null;

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
        public SMDictionaryItem(SMApplication _SMApplication)
        {
            if (_SMApplication == null) SM = SMApplication.Application;
            else SM = _SMApplication;
            Clear();
        }

        /// <summary>Class constructor.</summary>
        public SMDictionaryItem(SMApplication _SMApplication, SMDictionaryItem _DictionaryItem)
        {
            SM = _SMApplication;
            Assign(_DictionaryItem);
        }

        /// <summary>Class constructor.</summary>
        public SMDictionaryItem(SMApplication _SMApplication, string _Key, string _Value, object _Tag)
        {
            SM = _SMApplication;
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
