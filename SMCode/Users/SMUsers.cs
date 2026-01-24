/*  ===========================================================================
 *  
 *  File:       SMUsers.cs
 *  Version:    2.0.321
 *  Date:       January 2026
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2026 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *
 *  Users collection class. 
 *
 *  ===========================================================================
 */

using System;
using System.Collections.Generic;

namespace SMCodeSystem
{

    /* */

    /// <summary>Users collection class.</summary>
    public class SMUsers
    {

        /* */

        #region Declarations

        /*  ===================================================================
         *  Declarations
         *  ===================================================================
         */

        /// <summary>SM session instance.</summary>
        private readonly SMCode SM = null;

        /// <summary>Ignore case flag.</summary>
        private bool ignoreCase = true;

        /// <summary>Dictionary items collection.</summary>
        private List<SMUser> items = new List<SMUser>();

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
        public SMUser this[int _Index]
        {
            get { return items[_Index]; }
            set
            {
                items[_Index] = value;
                if (sorted) Sort();
            }
        }

        /// <summary>Get or set ignore case flag.</summary>
        public bool IgnoreCase
        {
            get { return ignoreCase; }
            set
            {
                if (ignoreCase != value)
                {
                    ignoreCase = value;
                    if (sorted) Sort();
                }
            }
        }

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
        public SMUsers(SMCode _SM = null)
        {
            SM = SMCode.CurrentOrNew(_SM);
            InitializeInstance();
        }

        /// <summary>Class constructor.</summary>
        public SMUsers(SMUsers _OtherInstance, SMCode _SM = null)
        {
            if (_SM == null) _SM = _OtherInstance.SM;
            SM = SMCode.CurrentOrNew(_SM);
            InitializeInstance();
            Assign(_OtherInstance);
        }

        /// <summary>Initialize instance.</summary>
        public void InitializeInstance()
        {
            Clear();
        }

        #endregion

        /* */

        #region Methods

        /*  ===================================================================
         *  Methods
         *  ===================================================================
         */

        /// <summary>Add user item and sort collection.</summary>
        public int Add(SMUser _User)
        {
            items.Add(_User);
            if (sorted) return Sort(true);
            else return items.Count - 1;
        }

        /// <summary>Assign instance properties from another.</summary>
        public void Assign(SMUsers _OtherInstance)
        {
            int i;
            items.Clear();
            for (i = 0; i < _OtherInstance.items.Count; i++)
            {
                items.Add(_OtherInstance.items[i]);
            }
            sorted = _OtherInstance.sorted;
        }

        /// <summary>Clear item.</summary>
        public void Clear()
        {
            items.Clear();
            ignoreCase = true;
            sorted = true;
        }

        /// <summary>Find first user with passed username.
        /// Return item index or -1 if not found.</summary>
        public int Find(string _UserName)
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
                        i = String.Compare(_UserName, items[mid].UserName, ignoreCase);
                        if (i == 0) rslt = mid;
                        else if (i < 0) max = mid - 1;
                        else min = mid + 1;
                    }
                    //
                    // search first
                    //
                    while (rslt > 0)
                    {
                        if (String.Compare(_UserName, items[rslt - 1].UserName, ignoreCase) == 0) rslt--;
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
                        if (String.Compare(_UserName, items[i].UserName, ignoreCase) == 0) rslt = i;
                        i++;
                    }
                }
            }
            return rslt;
        }

        /// <summary>Load users collection from SQL selection.</summary>
        public int Load(string _SQL = null)
        {
            int rslt = -1;
            SMUser user = null;
            SMDataset ds = new SMDataset("MAIN", SM, true);
            if (_SQL == null) _SQL = $"SELECT * FROM {SMDefaults.UsersTableName} ORDER BY {SMDefaults.UsersTableName_UserName}";
            items.Clear();
            if (ds.Open(_SQL))
            {
                rslt = 0;
                while (!ds.Eof)
                {
                    user = new SMUser(SM);
                    user.Read(ds);
                    Add(user);
                    rslt++;
                    ds.Next();
                }
            }
            ds.Close();
            ds.Dispose();
            return rslt;
        }

        /// <summary>Sort list.</summary>
        public int Sort(bool _AppendOnSortedList = false)
        {
            bool b = true;
            int i = items.Count - 1, rslt = -1;
            SMUser swap;
            while (b)
            {
                b = false;
                rslt = i;
                while (i > 0)
                {
                    if (String.Compare(items[i].UserName, items[i - 1].UserName, ignoreCase) < 0)
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

        #endregion

        /* */

    }

    /* */

}
