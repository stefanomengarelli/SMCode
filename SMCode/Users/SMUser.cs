/*  ===========================================================================
 *  
 *  File:       SMUser.cs
 *  Version:    2.0.38
 *  Date:       Jul 2024
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

        /// <summary>Get or set user UID.</summary>
        public string Uid { get; set; }

        /// <summary>Get or set user name.</summary>
        public string Name { get; set; }

        /// <summary>Get or set user email.</summary>
        public string Email { get; set; }

        /// <summary>Return true if user is empty.</summary>
        public bool Empty { get { return SM.Empty(Id) || SM.Empty(Name); } }

        /// <summary>Get or set user password.</summary>
        public string Password { get; set; }

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
            SM = SMCode.CurrentOrNew(_SM);
            InitializeInstance();
        }

        /// <summary>Class constructor.</summary>
        public SMUser(SMUser _OtherInstance, SMCode _SM = null)
        {
            SM = SMCode.CurrentOrNew(_SM);
            InitializeInstance();
            Assign(_OtherInstance);
        }

        /// <summary>Class constructor.</summary>
        public SMUser(string _Id, string _Name, string _Password="", string _Email = "", string _Uid = "", SMCode _SM = null)
        {
            SM = SMCode.CurrentOrNew(_SM);
            InitializeInstance();
            Id = _Id;
            Uid = _Uid;
            Name = _Name;
            Email = _Email;
            Password = _Password;
        }

        #endregion

        /* */

        #region Methods

        /*  ===================================================================
         *  Methods
         *  ===================================================================
         */

        /// <summary>Initialize instance.</summary>
        private void InitializeInstance(SMCode _SM = null)
        {
            Properties = new SMDictionary(SM);
            Rules = new SMRules(SM);
            Clear();
        }

        /// <summary>Assign instance properties from another.</summary>
        public void Assign(SMUser _OtherInstance)
        {
            Id = _OtherInstance.Id;
            Uid = _OtherInstance.Uid;
            Name = _OtherInstance.Name;
            Email = _OtherInstance.Email;
            Password = _OtherInstance.Password;
            Properties.Assign(_OtherInstance.Properties);
            Rules.Assign(_OtherInstance.Rules);
        }

        /// <summary>Clear item.</summary>
        public void Clear()
        {
            Id = "";
            Uid = "";
            Name = "";
            Email = "";
            Password = "";
            Properties.Clear();
            Rules.Clear();
        }

        /// <summary>Read item from current record of dataset. Return 1 if success, 0 if fail or -1 if error.</summary>
        public int Read(SMDataset _Dataset, string _IdColumn = null, string _NameColumn = null,
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
                        if (SM.Empty(_UidColumn)) _UidColumn = SMUsers.UidColumn;
                        if (SM.Empty(_NameColumn)) _NameColumn = SMUsers.NameColumn;
                        if (SM.Empty(_EmailColumn)) _EmailColumn = SMUsers.EmailColumn;
                        if (SM.Empty(_PasswordColumn)) _PasswordColumn = SMUsers.PasswordColumn;
                        //
                        if (!SM.Empty(_IdColumn)) Id = _Dataset.FieldStr(_IdColumn);
                        if (!SM.Empty(_UidColumn)) Uid = _Dataset.FieldStr(_UidColumn);
                        if (!SM.Empty(_NameColumn)) Name = _Dataset.FieldStr(_NameColumn);
                        if (!SM.Empty(_PasswordColumn)) Password = _Dataset.FieldStr(_PasswordColumn);
                        if (!SM.Empty(_EmailColumn)) Email = _Dataset.FieldStr(_EmailColumn);
                        //
                        for (i = 0; i < _Dataset.Columns.Count; i++)
                        {
                            c = _Dataset.Columns[i].ColumnName;
                            Properties.Add(new SMDictionaryItem(c, _Dataset.FieldStr(c), _Dataset.Field(c)));
                        }
                        //
                        return LoadRules();
                    }
                    else return 0;
                }
                else return -1;
            }
            catch (Exception ex)
            {
                SM.Error(ex);
                return -1;
            }
        }

        /// <summary>Load user information by id. Return 1 if success, 0 if fail or -1 if error.</summary>
        public int Load(string _Id, string _Password = "", string _TableName = null, string _Alias = null, string _IdColumn = null, string _NameColumn = null,
            string _PasswordColumn = null, string _EmailColumn = null, string _UidColumn = null, string _DeletedColumn = null)
        {
            int rslt = -1;
            string sql,salt;
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
                if (!SM.Empty(_Password))
                {
                    salt = _Password + '-' + _Password.Length.ToString() + '-' + _Id + '-' + _Id.Length.ToString();
                    salt = SM.HashSHA256(SM.HashSHA256(salt));
                    sql += "AND(" + _PasswordColumn + "=" + SM.Quote(salt) + ")";
                }
                if (!SM.Empty(_DeletedColumn)) sql += "AND" + SM.SqlNotDeleted(_DeletedColumn);
                sql += " ORDER BY " + _IdColumn;
                //
                if (ds.Open(sql))
                {
                    if (ds.Eof) rslt = 0;
                    else rslt = Read(ds, _IdColumn, _NameColumn, _PasswordColumn, _EmailColumn, _UidColumn);
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

        /// <summary>Load user related rules. Return 1 if success, 0 if fail or -1 if error.</summary>
        public int LoadRules()
        {
            string sql;
            SMDataset ds;
            SMRule rule;
            try
            {
                Rules.Clear();
                ds = new SMDataset(SMUsersRules.Alias, SM);
                //
                sql = "SELECT " + SMRules.TableName + ".* FROM " + SMUsersRules.TableName;
                sql += " INNER JOIN " + SMRules.TableName + " ON (" + SMUsersRules.TableName + "." + SMUsersRules.RuleIdColumn + "=" + SMRules.TableName + "." + SMRules.IdColumn + ")";
                sql += " WHERE (" + SMUsersRules.TableName + "." + SMUsersRules.UserIdColumn + "=" + SM.Quote(Id) + ")";
                if (!SM.Empty(SMUsersRules.DeletedColumn)) sql += "AND" + SM.SqlNotDeleted(SMUsersRules.DeletedColumn, SMUsersRules.TableName);
                sql += " ORDER BY " + SMUsersRules.TableName + "." + SMUsersRules.RuleIdColumn;
                //
                if (ds.Open(sql))
                {
                    while (!ds.Eof)
                    {
                        rule = new SMRule(SM);
                        if (rule.Read(ds) > 0) Rules.Add(rule);
                        ds.Next();
                    }
                    return Rules.Count;
                }
                else return -1;
            }
            catch (Exception ex)
            {
                SM.Error(ex);
                return -1;
            }
        }

        #endregion

        /* */

    }

    /* */

}