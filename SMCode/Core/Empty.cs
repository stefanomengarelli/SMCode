/*  ===========================================================================
 *  
 *  File:       Empty.cs
 *  Version:    2.0.274
 *  Date:       June 2025
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2025 by Stefano Mengarelli - All rights reserved - Use, 
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
        /// <param name="_Value">The date value to check.</param>
        /// <returns>True if the date is null or the minimum value, otherwise false.</returns>
        public bool Empty(DateTime _Value)
        {
            if (_Value == null) return true;
            else return _Value <= DateTime.MinValue;
        }

        /// <summary>Ritorna true se la data passata è nulla o vuota (valore minimo).</summary>
        /// <param name="_Value">Il valore della data da controllare.</param>
        /// <returns>True se la data è nulla o il valore minimo, altrimenti false.</returns>
        public bool Empty(DateTime? _Value)
        {
            if (_Value.HasValue) return _Value.Value <= DateTime.MinValue;
            else return true;
        }

        /// <summary>Returns true if object is null.</summary>
        /// <param name="_Value">The object to check.</param>
        /// <param name="_TestIfStringIsEmpty">Whether to test if the string is empty.</param>
        /// <returns>True if the object is null, otherwise false.</returns>
        public bool Empty(object _Value, bool _TestIfStringIsEmpty = false)
        {
            if (_Value == null) return true;
            else if (_Value == DBNull.Value) return true;
            else if (_TestIfStringIsEmpty) return _Value.ToString().Trim().Length < 1;
            else return false;
        }

        /// <summary>Returns true if string is null, empty or contains only spaces.</summary>
        /// <param name="_Value">The string to check.</param>
        /// <returns>True if the string is null, empty or contains only spaces, otherwise false.</returns>
        public bool Empty(string _Value)
        {
            if (_Value == null) return true;
            else return _Value.Trim().Length < 1;
        }

        /// <summary>Returns true if string array is null or with no items.</summary>
        /// <param name="_Value">The string array to check.</param>
        /// <returns>True if the string array is null or has no items, otherwise false.</returns>
        public bool Empty(string[] _Value)
        {
            if (_Value == null) return true;
            else if (_Value.Length < 1) return true;
            return false;
        }

        /// <summary>Returns true if Guid is null or empty.</summary>
        /// <param name="_Value">The Guid to check.</param>
        /// <returns>True if the string array is null or empty, otherwise false.</returns>
        public bool Empty(Guid? _Value)
        {
            if (_Value.HasValue)
            {
                if (_Value.Value == Guid.Empty) return true;
                else return false;
            }
            else return true;
        }

        #endregion

        /* */

    }

    /* */

}
