/*  ===========================================================================
 *  
 *  File:       SMUser.cs
 *  Version:    2.0.30
 *  Date:       May 2024
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2024 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *
 *  SMCode user class.
 *
 *  ===========================================================================
 */

using System;

namespace SMCodeSystem
{

    /* */

    /// <summary>SMCode user class.</summary>
    public class SMUser
    {

        /* */

        #region Declarations

        /*  ===================================================================
         *  Declarations
         *  ===================================================================
         */

        /// <summary>SM session instance.</summary>
        private readonly SMCode SM = null;

        #endregion

        /* */

        #region Properties

        /*  ===================================================================
         *  Properties
         *  ===================================================================
         */

        /// <summary>Get or set user id.</summary>
        public string Id { get; set; }

        /// <summary>Get or set user name.</summary>
        public string Name { get; set; }

        /// <summary>Get or set user email.</summary>
        public string Email { get; set; }

        /// <summary>Return true if user is empty.</summary>
        public bool Empty { get { return SM.Empty(Id) || SM.Empty(Name); } }

        /// <summary>Get or set user password.</summary>
        public string Password { get; set; }

        /// <summary>Get or set user UID.</summary>
        public string Uid { get; set; }

        /// <summary>Get or set user properties.</summary>
        public SMDictionary Properties { get; private set; }

        #endregion

        /* */

        #region Initialization

        /*  ===================================================================
         *  Initialization
         *  ===================================================================
         */

        /// <summary>Class constructor.</summary>
        public SMUser(SMCode _SM = null)
        {
            if (_SM == null) SM = SMCode.CurrentOrNew();
            else SM = _SM;
            Properties = new SMDictionary(SM);
            Clear();
        }

        /// <summary>Class constructor.</summary>
        public SMUser(SMUser _OtherInstance, SMCode _SM = null)
        {
            if (_SM == null) SM = SMCode.CurrentOrNew();
            else SM = _SM;
            Properties = new SMDictionary(SM);
            Assign(_OtherInstance);
        }

        /// <summary>Class constructor.</summary>
        public SMUser(string _Id, string _Name, string _Password="", string _Email = "", string _Uid = "", SMCode _SM = null)
        {
            if (_SM == null) SM = SMCode.CurrentOrNew();
            else SM = _SM;
            Properties = new SMDictionary(SM);
            Clear();
            Id = _Id;
            Name = _Name;
            Password = _Password;
            Email = _Email;
            Uid = _Uid;
        }

        #endregion

        /* */

        #region Methods

        /*  ===================================================================
         *  Methods
         *  ===================================================================
         */

        /// <summary>Assign instance properties from another.</summary>
        public void Assign(SMUser _OtherInstance)
        {
            Id = _OtherInstance.Id;
            Name = _OtherInstance.Name;
            Password = _OtherInstance.Password;
            Email = _OtherInstance.Email;
            Uid = _OtherInstance.Uid;
            Properties.Assign(_OtherInstance.Properties);
        }

        /// <summary>Clear item.</summary>
        public void Clear()
        {
            Id = "";
            Name = "";
            Email = "";
            Uid = "";
            Properties.Clear();
        }

        /// <summary>Read item from current record of dataset.</summary>
        public bool Read(SMDataset _Dataset, string _IdColumn = "Id", string _NameColumn = "Name", 
            string _PasswordColumn="Password", string _EmailColumn = "Email", string _UidColumn = "Uid")
        {
            int i;
            string c;
            try
            {
                if (_Dataset != null)
                {
                    if (!_Dataset.Eof)
                    {
                        Clear();
                        if (!SM.Empty(_IdColumn)) Id = _Dataset.FieldStr(_IdColumn);
                        if (!SM.Empty(_NameColumn)) Name = _Dataset.FieldStr(_NameColumn);
                        if (!SM.Empty(_PasswordColumn)) Password = _Dataset.FieldStr(_PasswordColumn);
                        if (!SM.Empty(_EmailColumn)) Email = _Dataset.FieldStr(_EmailColumn);
                        if (!SM.Empty(_UidColumn)) Uid = _Dataset.FieldStr(_UidColumn);
                        for (i = 0; i < _Dataset.Columns.Count; i++)
                        {
                            c = _Dataset.Columns[i].ColumnName;
                            Properties.Add(new SMDictionaryItem(c, _Dataset.FieldStr(c), _Dataset.Field(c)));
                        }
                        return true;
                    }
                    else return false;
                }
                else return false;
            }
            catch (Exception ex)
            {
                SM.Error(ex);
                return false;
            }
        }

        /// <summary>Load user information by id.</summary>
        public bool Load(string _Id, string _Password = "", string _TableName = "SM_Users", string _Alias = "MAIN", string _IdColumn = "Id", string _NameColumn = "Name",
            string _PasswordColumn = "Password", string _EmailColumn = "Email", string _UidColumn = "Uid", string _DeletedColumn = "Deleted")
        {
            bool r = false;
            string sql;
            SMDataset ds;
            try
            {
                Clear();
                ds = new SMDataset(_Alias, SM);
                sql = "SELECT * FROM " + _TableName + " WHERE (" + _IdColumn + "=" + SM.Quote(_Id) + ")";
                if (!SM.Empty(_DeletedColumn)) sql += "AND" + SM.SqlNotDeleted(_DeletedColumn);
                sql += " ORDER BY " + _IdColumn;
                if (ds.Open(sql))
                {
                    if (SM.Empty(_Password) || (_Password == ds.FieldStr(_PasswordColumn)))
                    {
                        if (!ds.Eof) r = Read(ds, _IdColumn, _NameColumn, _PasswordColumn, _EmailColumn, _UidColumn);
                    }
                }
            }
            catch (Exception ex)
            {
                SM.Error(ex);
                r = false;
            }
            return r;
        }

        #endregion

        /* */

    }

    /* */

}