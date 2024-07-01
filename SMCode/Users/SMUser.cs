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
using System.Security.Cryptography;

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

        /// <summary>Get user related rules.</summary>
        public SMRules Rules { get; private set; }

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
            InitializeInstance();
        }

        /// <summary>Class constructor.</summary>
        public SMUser(SMUser _OtherInstance, SMCode _SM = null)
        {
            if (_SM == null) SM = SMCode.CurrentOrNew();
            else SM = _SM;
            InitializeInstance();
            Assign(_OtherInstance);
        }

        /// <summary>Class constructor.</summary>
        public SMUser(string _Id, string _Name, string _Password="", string _Email = "", string _Uid = "", SMCode _SM = null)
        {
            if (_SM == null) SM = SMCode.CurrentOrNew();
            else SM = _SM;
            InitializeInstance();
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

        /// <summary>Initialize instance.</summary>
        private void InitializeInstance()
        {
            Properties = new SMDictionary(SM);
            Rules = new SMRules(SM);
            Clear();
        }

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
        public bool Read(SMDataset _Dataset, string _IdColumn = null, string _NameColumn = null,
            string _PasswordColumn = null, string _EmailColumn = null, string _UidColumn = null)
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
                        //
                        if (SM.Empty(_IdColumn)) _IdColumn = SMUsers.IdColumn;
                        if (SM.Empty(_NameColumn)) _NameColumn = SMUsers.NameColumn;
                        if (SM.Empty(_PasswordColumn)) _PasswordColumn = SMUsers.PasswordColumn;
                        if (SM.Empty(_EmailColumn)) _EmailColumn = SMUsers.EmailColumn;
                        if (SM.Empty(_UidColumn)) _UidColumn = SMUsers.UidColumn;
                        //
                        if (!SM.Empty(_IdColumn)) Id = _Dataset.FieldStr(_IdColumn);
                        if (!SM.Empty(_NameColumn)) Name = _Dataset.FieldStr(_NameColumn);
                        if (!SM.Empty(_PasswordColumn)) Password = _Dataset.FieldStr(_PasswordColumn);
                        if (!SM.Empty(_EmailColumn)) Email = _Dataset.FieldStr(_EmailColumn);
                        if (!SM.Empty(_UidColumn)) Uid = _Dataset.FieldStr(_UidColumn);
                        //
                        for (i = 0; i < _Dataset.Columns.Count; i++)
                        {
                            c = _Dataset.Columns[i].ColumnName;
                            Properties.Add(new SMDictionaryItem(c, _Dataset.FieldStr(c), _Dataset.Field(c)));
                        }
                        //
                        LoadRules();
                        //
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
        public bool Load(string _Id, string _Password = "", string _TableName = null, string _Alias = null, string _IdColumn = null, string _NameColumn = null,
            string _PasswordColumn = null, string _EmailColumn = null, string _UidColumn = null, string _DeletedColumn = null)
        {
            bool r = false;
            string sql;
            SMDataset ds;
            try
            {
                Clear();
                if (SM.Empty(_Alias)) _Alias = SMUsers.Alias;
                ds = new SMDataset(_Alias, SM);
                //
                if (SM.Empty(_TableName)) _TableName = SMUsers.TableName;
                if (SM.Empty(_IdColumn)) _IdColumn = SMUsers.IdColumn;
                if (SM.Empty(_DeletedColumn)) _DeletedColumn = SMUsers.DeletedColumn;
                if (SM.Empty(_PasswordColumn)) _PasswordColumn = SMUsers.PasswordColumn;
                //
                sql = "SELECT * FROM " + _TableName + " WHERE (" + _IdColumn + "=" + SM.Quote(_Id) + ")";
                if (!SM.Empty(_DeletedColumn)) sql += "AND" + SM.SqlNotDeleted(_DeletedColumn);
                sql += " ORDER BY " + _IdColumn;
                //
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

        /// <summary>Load user related rules.</summary>
        public bool LoadRules()
        {
            bool r = false;
            string sql;
            SMDataset ds;
            SMRule rule;
            try
            {
                Rules.Clear();
                ds = new SMDataset(Alias, SM);
                //
                sql = "SELECT " + SMRules.TableName + ".* FROM " + TableName;
                sql += " INNER JOIN " + SMRules.TableName + " ON (" + TableName + "." + RuleIdColumn + "=" + SMRules.TableName + "." + SMRules.IdColumn + ")";
                sql += " WHERE (" + TableName + "." + UserIdColumn + "=" + SM.Quote(Id) + ")";
                if (!SM.Empty(DeletedColumn)) sql += "AND" + SM.SqlNotDeleted(DeletedColumn, TableName);
                sql += " ORDER BY " + TableName + "." + RuleIdColumn;
                //
                if (ds.Open(sql))
                {
                    while (!ds.Eof)
                    {
                        rule = new SMRule(SM);
                        rule.Read(ds);
                        Rules.Add(rule);
                        ds.Next();
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

        #region Static Properties

        /*  ===================================================================
         *  Static Properties
         *  ===================================================================
         */

        /// <summary>Database alias.</summary>
        public static string Alias { get; set; } = "MAIN";

        /// <summary>Users table name.</summary>
        public static string TableName { get; set; } = "SM_UsersRules";

        /// <summary>Users table deleted column.</summary>
        public static string DeletedColumn { get; set; } = "Deleted";

        /// <summary>Users table user id column.</summary>
        public static string UserIdColumn { get; set; } = "UserId";

        /// <summary>Users table user id column.</summary>
        public static string RuleIdColumn { get; set; } = "RuleId";

        #endregion

        /* */

    }

    /* */

}