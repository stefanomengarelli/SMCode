/*  ===========================================================================
 *  
 *  File:       SMUsers.cs
 *  Version:    2.0.30
 *  Date:       May 2024
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

        /// <summary>Database alias.</summary>
        public string Alias { get; set; } = "MAIN";

        /// <summary>Get users count.</summary>
        public int Count { get { return items.Count; } }

        /// <summary>Users table email column.</summary>
        public string EmailColumn { get; set; } = "Email";

        /// <summary>Users table id column.</summary>
        public string IdColumn { get; set; } = "Id";

        /// <summary>Users table name column.</summary>
        public string NameColumn { get; set; } = "Name";

        /// <summary>Users table password column.</summary>
        public string PasswordColumn { get; set; } = "Password";

        /// <summary>Users table name.</summary>
        public string TableName { get; set; } = "SM_Users";

        /// <summary>Users table UID column.</summary>
        public string UidColumn { get; set; } = "Uid";

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
            if (_SM == null) SM = SMCode.CurrentOrNew();
            else SM = _SM;
            Clear();
        }

        /// <summary>Class constructor.</summary>
        public SMUsers(SMUsers _OtherInstance, SMCode _SM = null)
        {
            if (_SM == null) SM = SMCode.CurrentOrNew();
            else SM = _SM;
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

        /// <summary>Find user by id.</summary>
        public SMUser Get(string _Id)
        {
            int i = items.Find(_Id);
            if (i < 0) return null;
            else return (SMUser)items[i].Tag;
        }

        /// <summary>Load users collection.</summary>
        public bool Load()
        {
            bool r = false;
            SMDataset ds;
            SMUser user;
            try
            {
                ds = new SMDataset(Alias, SM);
                if (ds.Open("SELECT * FROM " + TableName + " ORDER BY " + IdColumn))
                {
                    Clear();
                    while (!ds.Eof)
                    {
                        user = new SMUser(SM);
                        if (user.Read(ds, IdColumn, NameColumn, PasswordColumn, EmailColumn, UidColumn))
                        {
                            Add(user);
                        }
                        ds.Next();
                    }
                    r = Count > 0;
                }
            }
            catch (Exception ex)
            {
                SM.Error(ex);
            }
            return r;
        }

        #endregion

        /* */

    }

    /* */

}