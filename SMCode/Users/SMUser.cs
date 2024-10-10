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

using Org.BouncyCastle.Asn1.Sec;
using System;
using System.Data;

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

        /// <summary>Get or set id.</summary>
        public int Id { get; set; }

        /// <summary>Get or set user UID.</summary>
        public string Uid { get; set; }

        /// <summary>Get or set user-id.</summary>
        public string UserId { get; set; }

        /// <summary>Get or set user name.</summary>
        public string Name { get; set; }

        /// <summary>Get or set user email.</summary>
        public string Email { get; set; }

        /// <summary>Return true if user is empty.</summary>
        public bool Empty { get { return (Id < 1) || SM.Empty(UserId) || SM.Empty(Name); } }

        /// <summary>Get or set user password.</summary>
        public string Password { get; set; }

        /// <summary>Get or set user PIN.</summary>
        public int Pin { get; set; }

        /// <summary>Get or set user register code.</summary>
        public int Register { get; set; }

        /// <summary>Get or set user tax code.</summary>
        public string TaxCode { get; set; }

        /// <summary>Get or set user birth date.</summary>
        public DateTime BirthDate { get; set; }

        /// <summary>Get or set user sex.</summary>
        public char Sex { get; set; }

        /// <summary>Get or set user icon path.</summary>
        public string Icon { get; set; }

        /// <summary>Get or set user image path.</summary>
        public string Image { get; set; }

        /// <summary>Get or set user note.</summary>
        public string Note { get; set; }

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
            if (_SM == null) _SM = _OtherInstance.SM;
            SM = SMCode.CurrentOrNew(_SM);
            InitializeInstance();
            Assign(_OtherInstance);
        }

        /// <summary>Initialize instance.</summary>
        private void InitializeInstance(SMCode _SM = null)
        {
            Properties = new SMDictionary(SM);
            Rules = new SMRules(SM);
            Clear();
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
            Uid = _OtherInstance.Uid;
            UserId = _OtherInstance.UserId;
            Name = _OtherInstance.Name;
            Email = _OtherInstance.Email;
            Password = _OtherInstance.Password;
            Pin = _OtherInstance.Pin;
            Register = _OtherInstance.Register;
            TaxCode = _OtherInstance.TaxCode;
            BirthDate = _OtherInstance.BirthDate;
            Sex = _OtherInstance.Sex;
            Icon = _OtherInstance.Icon;
            Image= _OtherInstance.Image;
            Note = _OtherInstance.Note;
            Properties.Assign(_OtherInstance.Properties);
            Rules.Assign(_OtherInstance.Rules);
        }

        /// <summary>Clear item.</summary>
        public void Clear()
        {
            Id = 0;
            Uid = "";
            UserId = "";
            Name = "";
            Email = "";
            Password = "";
            Pin = 0;
            Register = 0;
            TaxCode = "";
            BirthDate = DateTime.MinValue;
            Sex = ' ';
            Icon = "";
            Image = "";
            Note = "";
            Properties.Clear();
            Rules.Clear();
        }

        /// <summary>Read item from current record of dataset. Return 1 or more (plus rules loaded) if success, 0 if fail or -1 if error.</summary>
        public int Read(SMDataset _Dataset)
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
                        Id = SM.ToInt(_Dataset["IdUser"]);
                        Uid = SM.ToStr(_Dataset["UidUser"]);
                        UserId = SM.ToStr(_Dataset["UserId"]);
                        Name = SM.ToStr(_Dataset["Name"]);
                        Email = SM.ToStr(_Dataset["Email"]);
                        Password = SM.ToStr(_Dataset["Password"]);
                        Pin = SM.ToInt(_Dataset["Pin"]);
                        Register = SM.ToInt(_Dataset["Register"]); ;
                        TaxCode = SM.ToStr(_Dataset["TaxCode"]);
                        BirthDate = SM.ToDate(_Dataset["BirthDate"]);
                        Sex = SM.ToChar(_Dataset["Sex"]);
                        Icon = SM.ToStr(_Dataset["Icon"]);
                        Image = SM.ToStr(_Dataset["Image"]);
                        Note = SM.ToStr(_Dataset["Note"]);
                        //
                        for (i = 0; i < _Dataset.Columns.Count; i++)
                        {
                            c = _Dataset.Columns[i].ColumnName;
                            Properties.Add(new SMDictionaryItem(c, _Dataset.FieldStr(c), _Dataset.Field(c)));
                        }
                        //
                        return 1 + LoadRules();
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

        /// <summary>Load user information by sql query. Return 1 if success, 0 if fail or -1 if error.</summary>
        public int Load(string _Sql, string _Details = "")
        {
            int rslt = -1;
            SMDataset ds;
            SMLogItem log;
            try
            {
                Clear();
                if (!SM.Empty(_Sql))
                {
                    ds = new SMDataset(SM.UserDBAlias, SM);
                    if (ds.Open(_Sql))
                    {
                        if (ds.Eof)
                        {
                            rslt = 0;
                            SM.Log(SMLogType.Error, "Invalid user login.", "", "");
                        }
                        else
                        {
                            rslt = Read(ds);
                            log = new SMLogItem();
                            log.Date = DateTime.Now;
                            log.Type = SMLogType.Login;
                            log.Application = SM.ExecutableName;
                            log.Version = SM.Version;
                            log.Message = "User " + UserId + " logged.";
                            log.Details = _Details;
                            SM.Log(log);
                        }
                        ds.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                SM.Error(ex);
                rslt = -1;
            }
            return rslt;
        }

        /// <summary>Load user information by id. Return 1 if success, 0 if fail or -1 if error.</summary>
        public int Load(int _Id, string _Details = "")
        {
            return Load("SELECT * FROM sm_users WHERE (IdUser=" + _Id.ToString() + ")AND" + SM.SqlNotDeleted(), _Details);
        }

        /// <summary>Load user information by user-id and password. Return 1 if success, 0 if fail or -1 if error.</summary>
        public int Load(string _UserId, string _Password, string _Details = "")
        {
            string hash = SM.HashSHA256(_UserId + _Password + _UserId.Length.ToString() + _Password.Length.ToString());
            return Load("SELECT * FROM sm_users WHERE (UserId=" + SM.Quote(_UserId.ToString()) + ")AND(Password=" + SM.Quote(hash) + ")AND" + SM.SqlNotDeleted(), _Details);
        }

        /// <summary>Load user related rules. Return 1 if success, 0 if fail or -1 if error.</summary>
        public int LoadRules()
        {
            int i;
            string sql;
            SMDataset ds;
            SMRule rule;
            SMRules rules;
            try
            {
                Rules.Clear();
                ds = new SMDataset(SM.UserDBAlias, SM);
                //
                sql = "SELECT sm_rules.* FROM sm_users_rules"
                    + " INNER JOIN sm_rules ON (sm_users_rules.IdRule=sm_rules.IdRule)"
                    + " WHERE (sm_users_rules.IdUser=" + Id.ToString()
                    + ")AND" + SM.SqlNotDeleted("Deleted", "sm_users_rules")
                    + "AND" + SM.SqlNotDeleted("Deleted", "sm_rules")
                    + " ORDER BY sm_users_rules.IdRule";
                //
                if (ds.Open(sql))
                {
                    if (ds.Eof)
                    {
                        rules = new SMRules(SM);
                        if (rules.Load(true) > 0)
                        {
                            for (i = 0; i<rules.Count; i++)
                            {
                                ds.Exec("INSERT INTO sm_users_rules (IdUser,IdRule,Deleted,InsertionDate,InsertionUser) VALUES ("
                                    + Id.ToString() + "," + rules[i].Id.ToString() + ",0," + SM.Quote(DateTime.Now, ds.Database.Type) + "," + SM.Quote(SM.ExecutableName) + ")");
                            }
                            ds.Open(sql);
                        }
                    }
                    if (ds.State == SMDatasetState.Browse)
                    {
                        while (!ds.Eof)
                        {
                            rule = new SMRule(SM);
                            if (rule.Read(ds) > 0) Rules.Add(rule);
                            ds.Next();
                        }
                    }
                    ds.Close();
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