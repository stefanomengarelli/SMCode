/*  ===========================================================================
 *  
 *  File:       SMDatabases.cs
 *  Version:    2.0.200
 *  Date:       January 2025
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2025 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *
 *  SMCode database collection class.
 *
 *  ===========================================================================
 */

using System;
using System.Collections.Generic;

namespace SMCodeSystem
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
        private readonly SMCode SM = null;

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

        /// <summary>Get databases items count.</summary>
        public int Count { get { return items.Count; } }

        /// <summary>Get or set default database command timeout.</summary>
        public int DefaultCommandTimeout { get; set; } = 30;

        /// <summary>Get or set default database connection timeout.</summary>
        public int DefaultConnectionTimeout { get; set; } = 60;

        /// <summary>If true enable creation of new database connection every keep function call.</summary>
        public bool NewDatabaseOnKeep { get; set; } = false;

        #endregion

        /* */

        #region Initialization

        /*  ===================================================================
         *  Initialization
         *  ===================================================================
         */

        /// <summary>Class constructor.</summary>
        public SMDatabases(SMCode _SM)
        {
            SM = SMCode.CurrentOrNew(_SM);
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
            SMDatabaseType _Type,
            string _Host = "",
            string _Database = "",
            string _ConnectionString = "",
            string _Path = "",
            string _User = "",
            string _Password = "",
            bool _Save = false)
        {
            SMDatabase db;
            if (Find(_Alias) < 0)
            {
                db = new SMDatabase(SM);
                db.Alias = _Alias;
                db.ConnectionString = _ConnectionString;
                db.Database = _Database;
                db.Host = _Host;
                db.Password = _Password;
                if (SM.Empty(_Path)) db.Path = SM.DataPath;
                else db.Path = _Path;
                db.Type = _Type;
                db.User = _User;
                items.Add(db);
                if (_Type == SMDatabaseType.None) items[items.Count - 1].Load(_Alias);
                else if (_Save) items[items.Count - 1].Save();
                return true;
            }
            else return false;
        }

        /// <summary>Add new database to collection, loading parameter from configuration INI file.</summary>
        public bool Add(string _Alias)
        {
            SMDatabase db;
            if (Find(_Alias) < 0)
            {
                db = new SMDatabase(SM);
                if (db.Load(_Alias))
                {
                    items.Add(db);
                    return true;
                }
                else return false;
            }
            else return false;
        }

        /// <summary>Close all databases with alias or all if not specified.</summary>
        public bool Close(string _Alias = "")
        {
            int i = 0;
            bool rslt = true;
            while (i < items.Count)
            {
                if (items[i] == null) rslt = false;
                else if ((_Alias == "") || (_Alias == items[i].Alias))
                {
                    if (!items[i].Close()) rslt = false;
                }
                i++;
            }
            return rslt;
        }

        /// <summary>Return index of database collection item with alias.</summary>
        public int Find(string _Alias)
        {
            int i = 0;
            _Alias = _Alias.Trim().ToUpper();
            if (_Alias.Length > 0)
            {
                while (i < items.Count)
                {
                    if (items[i].Alias == _Alias) return i;
                    i++;
                }
            }
            return -1;
        }

        /// <summary>Return database with alias and open it if not active.</summary>
        public SMDatabase Keep(string _Alias = "MAIN")
        {
            int i;
            SMDatabase db;
            try
            {
                if (NewDatabaseOnKeep)
                {
                    db = new SMDatabase(SM);
                    db.Open(_Alias);
                    items.Add(db);
                }
                else
                {
                    i = Find(_Alias);
                    if (i > -1)
                    {
                        items[i].Keep();
                        db = items[i];
                    }
                    else db = null;
                }
            }
            catch (Exception ex)
            {
                SM.Error(ex);
                db = null;
            }
            return db;
        }

        #endregion

        /* */

    }

    /* */

}
