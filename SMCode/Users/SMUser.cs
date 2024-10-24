/*  ===========================================================================
 *  
 *  File:       SMUser.cs
 *  Version:    2.0.54
 *  Date:       October 2024
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
using System.Text.Json;

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
        public int IdUser { get; set; }

        /// <summary>Get or set user UID.</summary>
        public string UidUser { get; set; }

        /// <summary>Get or set user name.</summary>
        public string UserName { get; set; }

        /// <summary>Get or set user description.</summary>
        public string Text { get; set; }

        /// <summary>Get or set user email.</summary>
        public string Email { get; set; }

        /// <summary>Return true if user is empty.</summary>
        public bool Empty { get { return (IdUser < 1) || SM.Empty(UserName) || SM.Empty(Text); } }

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

        /// <summary>Get or set user selected organization.</summary>
        public SMOrganization Organization { get; set; }

        /// <summary>Get or set user related organizations.</summary>
        public SMOrganizations Organizations { get; set; }

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
            Organization = new SMOrganization(SM);
            Organizations = new SMOrganizations(SM);
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
            IdUser = _OtherInstance.IdUser;
            UidUser = _OtherInstance.UidUser;
            UserName = _OtherInstance.UserName;
            Text = _OtherInstance.Text;
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
            Organization.Assign(_OtherInstance.Organization);
            Organizations.Assign(_OtherInstance.Organizations);
            Properties.Assign(_OtherInstance.Properties);
            Rules.Assign(_OtherInstance.Rules);
        }

        /// <summary>Clear item.</summary>
        public void Clear()
        {
            IdUser = 0;
            UidUser = "";
            UserName = "";
            Text = "";
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
            Organization.Clear();
            Organizations.Clear();
            Properties.Clear();
            Rules.Clear();
        }

        /// <summary>Assign property from JSON serialization.</summary>
        public bool FromJSON(string _JSON)
        {
            try
            {
                Assign((SMUser)JsonSerializer.Deserialize(_JSON, null));
                return true;
            }
            catch (Exception ex)
            {
                SM.Error(ex);
                return false;
            }
        }

        /// <summary>Assign property from JSON64 serialization.</summary>
        public bool FromJSON64(string _JSON64)
        {
            return FromJSON(SM.Base64Decode(_JSON64));
        }

        /// <summary>Return HASH code of user and password.</summary>
        public string Hash(string _User = null, string _Password=null)
        {
            if (_User != null) _User = UserName;
            if (_Password != null) _Password = Password;
            return SM.HashSHA256(_User + _Password + _User.Length.ToString() + _Password.Length.ToString());
        }

        /// <summary>Load user information by sql query. Log details can be specified as parameter. 
        /// Return 1 if success, 0 if user cannot be found or -1 if error.</summary>
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
                            SM.Log(SMLogType.Error, "Invalid user.", "", "");
                        }
                        else
                        {
                            rslt = Read(ds);
                            log = new SMLogItem();
                            log.DateTime = DateTime.Now;
                            log.LogType = SMLogType.Login;
                            log.About = SM.ExecutableName;
                            log.Version = SM.Version;
                            log.Message = "User " + UserName + " logged.";
                            log.Details = _Details;
                            if (SM.OnLoginEvent == null) SM.Log(log);
                            else if (SM.OnLoginEvent(log, this, _Details)) SM.Log(log);
                            else SM.Log(SMLogType.Error, "Unauthorized user.", "", "");
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

        /// <summary>Load user information by id. Log details can be specified as parameter.
        /// Return 1 if success, 0 if fail or -1 if error.</summary>
        public int Load(int _Id, string _Details = "")
        {
            return Load("SELECT * FROM sm_users WHERE (Id=" + _Id.ToString() + ")AND" + SM.SqlNotDeleted(), _Details);
        }

        /// <summary>Load user information by user-id and password. Log details can be specified as parameter.
        /// Return 1 if success, 0 if fail or -1 if error.</summary>
        public int Load(string _UserName, string _Password, string _Details = "")
        {
            return Load("SELECT * FROM sm_users WHERE (UserName=" + SM.Quote(_UserName.ToString()) 
                + ")AND(Password=" + SM.Quote(Hash()) + ")AND" + SM.SqlNotDeleted(), _Details);
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
                    + " WHERE (sm_users_rules.IdUser=" + IdUser.ToString()
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
                            for (i = 0; i < rules.Count; i++)
                            {
                                ds.Exec("INSERT INTO sm_users_rules (IdUser,IdRule,Deleted,InsertionDate,InsertionUser) VALUES ("
                                    + IdUser.ToString() + "," + rules[i].Id.ToString() + ",0," + SM.Quote(DateTime.Now, ds.Database.Type) + "," + SM.Quote(SM.ExecutableName) + ")");
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

        /// <summary>Load user related organizations. Return 1 if success, 0 if fail or -1 if error.</summary>
        public int LoadOrganizations()
        {
            int i;
            string sql;
            SMDataset ds;
            SMOrganization organization;
            SMOrganizations organizations;
            try
            {
                Rules.Clear();
                ds = new SMDataset(SM.UserDBAlias, SM);
                //
                sql = "SELECT sm_organizations.* FROM sm_users_organizations"
                    + " INNER JOIN sm_organizations ON (sm_users_organizations.IdOrganization=sm_organizations.IdOrganization)"
                    + " WHERE (sm_users_organizations.IdUser=" + IdUser.ToString()
                    + ")AND" + SM.SqlNotDeleted("Deleted", "sm_users_organizations")
                    + "AND" + SM.SqlNotDeleted("Deleted", "sm_organizations")
                    + " ORDER BY sm_users_organizations.IdOrganizations";
                //
                if (ds.Open(sql))
                {
                    if (ds.Eof)
                    {
                        organizations = new SMOrganizations(SM);
                        if (organizations.Load(true) > 0)
                        {
                            for (i = 0; i < organizations.Count; i++)
                            {
                                ds.Exec("INSERT INTO sm_users_organizations (IdUser,IdOrganization,Deleted,InsertionDate,InsertionUser) VALUES ("
                                    + IdUser.ToString() + "," + organizations[i].Id.ToString() + ",0," + SM.Quote(DateTime.Now, ds.Database.Type) + "," + SM.Quote(SM.ExecutableName) + ")");
                            }
                            ds.Open(sql);
                        }
                    }
                    if (ds.State == SMDatasetState.Browse)
                    {
                        while (!ds.Eof)
                        {
                            organization = new SMOrganization(SM);
                            if (organization.Read(ds) > 0) Organizations.Add(organization);
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

        /// <summary>Read item from current record of dataset. Return 1 or more (plus rules loaded) if success, 0 if fail or -1 if error.</summary>
        public int Read(SMDataset _Dataset)
        {
            int i, rslt = -1;
            string c;
            try
            {
                if (_Dataset != null)
                {
                    if (!_Dataset.Eof)
                    {
                        Clear();
                        rslt = 0;
                        //
                        IdUser = SM.ToInt(_Dataset["IdUser"]);
                        UidUser = SM.ToStr(_Dataset["UidUser"]);
                        UserName = SM.ToStr(_Dataset["UserName"]);
                        Text = SM.ToStr(_Dataset["Text"]);
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
                        i = LoadRules();
                        if (i > -1)
                        {
                            rslt += i * 1000;
                            i = LoadOrganizations();
                            if (i > -1) rslt += i;
                            else rslt = -1;
                        }
                        else rslt = -1;
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

        /// <summary>Return JSON serialization of instance.</summary>
        public string ToJSON()
        {
            try
            {
                return JsonSerializer.Serialize(this);
            }
            catch (Exception ex)
            {
                SM.Error(ex);
                return "";
            }
        }

        /// <summary>Return JSON64 serialization of instance.</summary>
        public string ToJSON64()
        {
            return SM.Base64Encode(ToJSON());
        }

        #endregion

        /* */

    }

    /* */

}