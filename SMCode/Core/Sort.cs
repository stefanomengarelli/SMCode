/*  ===========================================================================
 *  
 *  File:       Sort.cs
 *  Version:    2.0.140
 *  Date:       January 2025
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2025 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *
 *  SMCode application class: sort.
 *
 *  ===========================================================================
 */

using System.Collections.Generic;

namespace SMCodeSystem
{

    /* */

    /// <summary>SMCode application class: sort.</summary>
    public partial class SMCode
    {

        /* */

        #region Methods

        /*  ===================================================================
         *  Methods
         *  ===================================================================
         */

        /// <summary>Perform quicksort on object list by passed compare method.</summary>
        public void QuickSort(List<object> _Objects, SMOnCompare _CompareMethod, int _LeftIndex = -1, int _RightIndex = -1)
        {
            int i, j;
            object pivot, swap;
            if ((_Objects != null) && (_CompareMethod != null))
            {
                if (_Objects.Count > 1)
                {
                    if (_LeftIndex < 0) _LeftIndex = 0;
                    if (_RightIndex < 0) _RightIndex = _Objects.Count - 1;
                    i = _LeftIndex;
                    j = _RightIndex;
                    pivot = _Objects[i];
                    while (i <= j)
                    {
                        while (_CompareMethod(_Objects[i], pivot) < 0) i++;
                        while (_CompareMethod(_Objects[j], pivot) > 0) j--;
                        if (i <= j)
                        {
                            swap = _Objects[i];
                            _Objects[i] = _Objects[j];
                            _Objects[j] = swap;
                            i++;
                            j--;
                        }
                    }
                    if (_LeftIndex < j) QuickSort(_Objects, _CompareMethod, _LeftIndex, j);
                    if (i < _RightIndex) QuickSort(_Objects, _CompareMethod, i, _RightIndex);
                }
            }
        }

        /// <summary>Sort object list by passed compare method.</summary>
        public void Sort(List<object> _Objects, SMOnCompare _CompareMethod, bool _SortOnAddToSortedArray = false)
        {
            int i;
            bool b = true;
            object swap;
            while (b)
            {
                b = false;
                i = _Objects.Count - 1;
                while (i > 0)
                {
                    if (_CompareMethod(_Objects[i], _Objects[i - 1]) < 0)
                    {
                        b = true;
                        swap = _Objects[i];
                        _Objects[i] = _Objects[i - 1];
                        _Objects[i - 1] = swap;
                    }
                    else if (_SortOnAddToSortedArray)
                    {
                        b = false;
                        i = 0;
                    }
                    i--;
                }
            }
        }

        /// <summary>Sort index of object list by passed compare method.</summary>
        public void Sort(List<int> _Index, List<object> _Objects, SMOnCompare _CompareMethod, bool _SortOnAddToSortedArray = false)
        {
            int i, swap;
            bool b = true;
            while (b)
            {
                b = false;
                i = _Index.Count - 1;
                while (i > 0)
                {
                    if (_CompareMethod(_Objects[_Index[i]], _Objects[_Index[i - 1]]) < 0)
                    {
                        b = true;
                        swap = _Index[i];
                        _Index[i] = _Index[i - 1];
                        _Index[i - 1] = swap;
                    }
                    else if (_SortOnAddToSortedArray)
                    {
                        b = false;
                        i = 0;
                    }
                    i--;
                }
            }
        }

        #endregion

        /* */

    }

    /* */

}
