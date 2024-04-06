/*  ===========================================================================
 *  
 *  File:       SMDatabases.cs
 *  Version:    2.0.0
 *  Date:       March 2024
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2024 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *
 *  SMCode database collection class.
 *
 *  ===========================================================================
 */

using System.Collections.Generic;

namespace SMCode
{

    /* */

    /// <summary>SMCode database collection class.</summary>
    public class SMDatabases
    {

        /* */

        #region Declarations

        /*  ===================================================================
         *  Declarations
         *  ===================================================================
         */

        /// <summary>SM session instance.</summary>
        private readonly SMApplication SM = null;

        /// <summary>Database connections collection.</summary>
        private List<SMDatabase> items { get; set; } = new List<SMDatabase>();


        #endregion

        /* */

        #region Properties

        /*  ===================================================================
         *  Properties
         *  ===================================================================
         */

        /// <summary>Quick access declaration.</summary>
        public SMDatabase this[int _Index]
        {
            get
            {
                if ((_Index > -1) && (_Index < items.Count)) return items[_Index];
                else return null;
            }
            set
            {
                if ((_Index > -1) && (_Index < items.Count)) items[_Index] = value;
            }
        }

        /// <summary>Quick access declaration.</summary>
        public SMDatabase this[string _Alias]
        {
            get
            {
                int i = Find(_Alias);
                if (i < 0) return null;
                else return items[i];
            }
            set
            {
                int i = Find(_Alias);
                if (i > -1) items[i] = value;
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
        public SMDatabases(SMApplication _SMApplication)
        {
            if (_SMApplication == null) SM = SMApplication.Application;
            else SM = _SMApplication;
        }

        #endregion

        /* */

        #region Methods

        /*  ===================================================================
         *  Methods
         *  ===================================================================
         */

        /// <summary>Add new database to collection.</summary>
        public bool Add(string _Alias,
            SMDatabaseType _Type = SMDatabaseType.None,
            string _Host = "",
            string _Database = "",
            string _ConnectionString = "",
            string _Path = "",
            string _User = "",
            string _Password = "",
            bool _Save = false)
        {
            if (Find(_Alias) < 0)
            {
                items.Add(new SMDatabase(SM)
                {
                    Alias = _Alias,
                    ConnectionString = _ConnectionString,
                    Database = _Database,
                    Host = _Host,
                    Password = _Password,
                    Path = _Path,
                    Type = _Type,
                    User = _User
                });
                if (_Type == SMDatabaseType.None) items[items.Count - 1].Load(_Alias);
                else if (_Save) items[items.Count - 1].Save();
                return true;
            }
            else return false;
        }

        /// <summary>Return index of database collection item with alias.</summary>
        public int Find(string _Alias)
        {
            int i = 0;
            _Alias = _Alias.Trim().ToUpper();
            while (i < items.Count)
            {
                if (items[i].Alias == _Alias) return i;
                i++;
            }
            return -1;
        }

        /// <summary>Return database with alias and open it if not active.</summary>
        public SMDatabase Keep(string _Alias)
        {
            int i = Find(_Alias);
            if (i > 0)
            {
                items[i].Keep();
                return items[i];
            }
            else return null;
        }

        #endregion

        /* */

    }

    /* */

}
