/*  ===========================================================================
 *  
 *  File:       Empty.cs
 *  Version:    2.0.0
 *  Date:       April 2024
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2024 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *
 *  SMCode application class: empty.
 *  
 *  ===========================================================================
 */

using System;

namespace SMCodeSystem
{

    /* */

    /// <summary>SMCode application class: empty.</summary>
    public partial class SMCode
    {

        /* */

        #region Methods

        /*  ===================================================================
         *  Methods
         *  ===================================================================
         */

        /// <summary>Return true if date is null or empty (minimum value).</summary>
        public bool Empty(DateTime _Value)
        {
            if (_Value == null) return true;
            else return _Value <= DateTime.MinValue;
        }

        /// <summary>Ritorna true se la stringa passata è nulla, vuota o composta da soli spazi.</summary>
        public bool Empty(DateTime? _Value)
        {
            if (_Value.HasValue) return _Value.Value <= DateTime.MinValue;
            else return true;
        }

        /// <summary>Returns true if object is null.</summary>
        public bool Empty(object _Value, bool _TestIfStringIsEmpty = false)
        {
            if (_Value == null) return true;
            else if (_Value == DBNull.Value) return true;
            else if (_TestIfStringIsEmpty) return _Value.ToString().Trim().Length < 1;
            else return false;
        }

        /// <summary>Returns true if string is null, empty or contains only spaces.</summary>
        public bool Empty(string _Value)
        {
            if (_Value == null) return true;
            else return _Value.Trim().Length < 1;
        }

        /// <summary>Returns true if string array is null or with no items.</summary>
        public bool Empty(string[] _Value)
        {
            if (_Value == null) return true;
            else if (_Value.Length < 1) return true;
            return false;
        }

        #endregion

        /* */

    }

    /* */

}
