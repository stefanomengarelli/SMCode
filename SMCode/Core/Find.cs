/*  ===========================================================================
 *  
 *  File:       Find.cs
 *  Version:    2.0.0
 *  Date:       February 2024
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2024 by Stefano Mengarelli - All rights reserved - Use, 
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
