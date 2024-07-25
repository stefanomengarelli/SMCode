/*  ===========================================================================
 *  
 *  File:       SMUsers.cs
 *  Version:    2.0.38
 *  Date:       Jul 2024
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2024 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *
 *  SMCode user collection class.
 *
 *  ===========================================================================
 */

using System;

namespace SMCodeSystem
{

    /* */

    /// <summary>SMCode user collection class.</summary>
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

        /// <summary>Users collection.</summary>
        private SMDictionary items = new SMDictionary();

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
            get { return (SMUser)items[_Index].Tag; }
        }

        /// <summary>Get users count.</summary>
        public int Count { get { return items.Count; } }

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
            Clear();
        }

        /// <summary>Class constructor.</summary>
        public SMUsers(SMUsers _OtherInstance, SMCode _SM = null)
        {
            SM = SMCode.CurrentOrNew(_SM);
            Assign(_OtherInstance);
        }

        #endregion

        /* */

        #region Methods

        /*  ===================================================================
         *  Methods
         *  ===================================================================
         */

        /// <summary>Add user item.</summary>
        public int Add(SMUser _User)
        {
            items.Add(new SMDictionaryItem(_User.Id, _User.Name, _User));
            return items.Count - 1;
        }

        /// <summary>Add user item.</summary>
        public int Add(string _Id, string _Name, string _Password = "", string _Email = "", string _Uid = "", SMCode _SM = null)
        {
            if (_SM == null) _SM = SM;
            return Add(new SMUser(_Id, _Name, _Password, _Email, _Uid, _SM));
        }

        /// <summary>Assign instance properties from another.</summary>
        public void Assign(SMUsers _OtherInstance)
        {
            items.Assign(_OtherInstance.items);
        }

        /// <summary>Clear item.</summary>
        public void Clear()
        {
            items.Clear();
        }

        /// <summary>Find user by id.</summary>
        public int Find(string _Id)
        {
            return items.Find(_Id);
        }

        /// <summary>Find user by value of property.</summary>
        public int Find(string _Property, string _Value)
        {
            int i = 0, r = -1;
            while ((r < 0) && (i < items.Count))
            {
                if (((SMUser)items[i].Tag).Properties.ValueOf(_Property) == _Value) r = i;
                i++;
            }
            return r;
        }

        /// <summary>Get user by id.</summary>
        public SMUser Get(string _Id)
        {
            int i = items.Find(_Id);
            if (i < 0) return null;
            else return (SMUser)items[i].Tag;
        }

        /// <summary>Load users collection. Return 1 if success, 0 if fail or -1 if error.</summary>
        public int Load()
        {
            int rslt = -1;
            string sql;
            SMDataset ds;
            SMUser user;
            try
            {
                Clear();
                //
                ds = new SMDataset(Alias, SM);
                //
                sql = "SELECT * FROM " + TableName;
                if (!SM.Empty(DeletedColumn)) sql += " WHERE " + SM.SqlNotDeleted(DeletedColumn);
                sql += " ORDER BY " + IdColumn;
                //
                if (ds.Open(sql))
                {
                    while (!ds.Eof)
                    {
                        user = new SMUser(SM);
                        if (user.Read(ds, IdColumn, NameColumn, PasswordColumn, EmailColumn, UidColumn) > 0)
                        {
                            Add(user);
                        }
                        ds.Next();
                    }
                    rslt = Count;
                    ds.Close();
                }
            }
            catch (Exception ex)
            {
                SM.Error(ex);
                rslt = -1;
            }
            return rslt;
        }

        #endregion

        /* */

        #region Static Properties

        /*  ===================================================================
         *  Static Properties
         *  ===================================================================
         */

        /// <summary>Database alias.</summary>
        public static string Alias { get; set; } = "MAIN";

        /// <summary>Users table deleted column.</summary>
        public static string DeletedColumn { get; set; } = "Deleted";

        /// <summary>Users table email column.</summary>
        public static string EmailColumn { get; set; } = "Email";

        /// <summary>Users table id column.</summary>
        public static string IdColumn { get; set; } = "IdUser";

        /// <summary>Users table UID column.</summary>
        public static string UidColumn { get; set; } = "UidUser";

        /// <summary>Users table name column.</summary>
        public static string NameColumn { get; set; } = "Name";

        /// <summary>Users table password column.</summary>
        public static string PasswordColumn { get; set; } = "Password";

        /// <summary>Users table name.</summary>
        public static string TableName { get; set; } = "sm_users";

        #endregion

        /* */

    }

    /* */

}