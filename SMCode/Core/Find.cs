/*  ===========================================================================
 *  
 *  File:       Find.cs
 *  Version:    2.0.221
 *  Date:       March 2025
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2025 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *
 *  SMCode application class: find.
 *
 *  ===========================================================================
 */

using System.Collections.Generic;

namespace SMCodeSystem
{

    /* */

    /// <summary>SMCode application class: find.</summary>
    public partial class SMCode
    {

        /* */

        #region Methods

        /*  ===================================================================
         *  Methods
         *  ===================================================================
         */

        /// <summary>Return index of object matching with value according to compare method, or -1 if fails.</summary>
        /// <param name="_Value">The value to find.</param>
        /// <param name="_Objects">The list of objects to search in.</param>
        /// <param name="_CompareMethod">The method used to compare the objects.</param>
        /// <param name="_BinarySearch">Whether to use binary search (default is true).</param>
        /// <returns>The index of the matching object, or -1 if not found.</returns>
        public int Find(object _Value, List<object> _Objects, SMOnCompare _CompareMethod, bool _BinarySearch = true)
        {
            int i, max, mid, min, r = -1;
            if ((_Value != null) && (_Objects != null))
            {
                if (_BinarySearch)
                {
                    min = 0;
                    max = _Objects.Count - 1;
                    while ((r < 0) && (min <= max))
                    {
                        mid = (min + max) / 2;
                        i = _CompareMethod(_Value, _Objects[mid]);
                        if (i == 0) r = mid;
                        else if (i < 0) max = mid - 1;
                        else min = mid + 1;
                    }
                    while (r > 0)
                    {
                        if (_CompareMethod(_Value, _Objects[r - 1]) == 0) r--;
                        else break;
                    }
                }
                else
                {
                    i = 0;
                    while ((r < 0) && (i < _Objects.Count))
                    {
                        if (_CompareMethod(_Value, _Objects[i]) == 0) r = i;
                        i++;
                    }
                }
            }
            return r;
        }

        /// <summary>Return index of object matching with value according to compare method and index, or -1 if fails.</summary>
        /// <param name="_Value">The value to find.</param>
        /// <param name="_Objects">The list of objects to search in.</param>
        /// <param name="_Index">The list of indices to search in.</param>
        /// <param name="_CompareMethod">The method used to compare the objects.</param>
        /// <returns>The index of the matching object, or -1 if not found.</returns>
        public int Find(object _Value, List<object> _Objects, List<int> _Index, SMOnCompare _CompareMethod)
        {
            int i, max, mid, min, r = -1;
            if ((_Value != null) && (_Objects != null) && (_Index != null))
            {
                min = 0;
                max = _Index.Count - 1;
                while ((r < 0) && (min <= max))
                {
                    mid = (min + max) / 2;
                    i = _CompareMethod(_Value, _Objects[_Index[mid]]);
                    if (i == 0) r = mid;
                    else if (i < 0) max = mid - 1;
                    else min = mid + 1;
                }
                while (r > 0)
                {
                    if (_CompareMethod(_Value, _Objects[_Index[r - 1]]) == 0) r--;
                    else break;
                }
                if (r > -1) r = _Index[r];
            }
            return r;
        }

        #endregion

        /* */

    }

    /* */

}
