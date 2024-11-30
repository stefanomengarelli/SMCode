/*  ===========================================================================
 *  
 *  File:       SMUser.cs
 *  Version:    2.0.61
 *  Date:       November 2024
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

        /// <summary>User sex flag.</summary>
        private char sex = ' ';

        /// <summary>User text.</summary>
        private string text = "";

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
        public string Text 
        {
            get
            {
                if (SM.Empty(text)) return (FirstName.Trim() + " " + LastName.Trim()).Trim();
                else return text;
            }
            set { text = value; }
        }

        /// <summary>Get or set user first name.</summary>
        public string FirstName { get; set; }

        /// <summary>Get or set user last name.</summary>
        public string LastName { get; set; }

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
        public char Sex 
        {
            get { return sex; }
            set
            {
                if ((value == 'f') || (value == 'F')) sex = 'F';
                else if ((value == 'm') || (value == 'M')) sex = 'M';
                else sex = ' ';
            }
        }

        /// <summary>Get or set user icon path.</summary>
        public string Icon { get; set; }

        /// <summary>Get or set user image path.</summary>
        public string Image { get; set; }

        /// <summary>Get or set user note.</summary>
        public string Note { get; set; }

        /// <summary>Get or set user selected organization.</summary>
        public SMOrganization Organization { get; private set; }

        /// <summary>Get or set user related organizations.</summary>
        public SMOrganizations Organizations { get; set; }

        /// <summary>Get or set user properties.</summary>
        public SMDictionary Properties { get; private set; }

        /// <summary>Get user related rules.</summary>
        public SMRules Rules { get; private set; }

        /// <summary>Get or set object tag.</summary>
        public object Tag { get; set; }

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
            Initialize();
        }

        /// <summary>Class constructor.</summary>
        public SMUser(SMUser _OtherInstance, SMCode _SM = null)
        {
            if (_SM == null) _SM = _OtherInstance.SM;
            SM = SMCode.CurrentOrNew(_SM);
            Initialize();
            Assign(_OtherInstance);
        }

        /// <summary>Initialize instance.</summary>
        private void Initialize()
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
            FirstName = _OtherInstance.FirstName;
            LastName = _OtherInstance.LastName;
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
            Tag = _OtherInstance.Tag;
        }

        /// <summary>Clear item.</summary>
        public void Clear()
        {
            IdUser = 0;
            UidUser = "";
            UserName = "";
            Text = "";
            FirstName = "";
            LastName = "";
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
            Tag = null;
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

        /// <summary>Returns a string based on the user's gender.</summary>
        public string Gender(string _IfMale, string _IfFemale, string _IfNeutral = null)
        {
            if (sex == 'F') return _IfFemale;
            else if (sex == 'M') return _IfMale;
            else if (_IfNeutral == null) return _IfMale;
            else return _IfNeutral;
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
        public int Load(string _Sql, string _LogDetails = "")
        {
            int rslt = -1;
            SMDataset ds;
            SMLogItem log;
            try
            {
                Clear();
                if (!SM.Empty(_Sql))
                {
                    ds = new SMDataset(SM.MainAlias, SM);
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
                            log.Application = SM.ExecutableName;
                            log.Version = SM.Cat(SM.Version, SM.ToStr(SM.ExecutableDate, true), " - ");
                            log.IdUser = SM.User.IdUser;
                            log.UidUser = SM.User.UidUser;
                            log.LogType = SMLogType.Login;
                            log.Action = "LOGIN";
                            log.Message = "User " + UserName + " logged.";
                            log.Details = _LogDetails;
                            if (!SM.LoginEvent(log, this)) rslt = 0;
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
        public int LoadById(int _IdUser, string _LogDetails = "")
        {
            return Load("SELECT * FROM " + SMDefaults.UsersTableName + " WHERE (IdUser=" + _IdUser.ToString() + ")AND" + SM.SqlNotDeleted(), _LogDetails);
        }

        /// <summary>Load user information by uid. Log details can be specified as parameter.
        /// Return 1 if success, 0 if fail or -1 if error.</summary>
        public int LoadByUid(string _UidUser, string _LogDetails = "")
        {
            return Load("SELECT * FROM " + SMDefaults.UsersTableName + " WHERE (UidUser=" + SM.Quote(_UidUser) + ")AND" + SM.SqlNotDeleted(), _LogDetails);
        }

        /// <summary>Load user information by user-id and password. Log details can be specified as parameter.
        /// Return 1 if success, 0 if fail or -1 if error.</summary>
        public int LoadByCredentials(string _UserName, string _Password, string _LogDetails = "")
        {
            return Load("SELECT * FROM " + SMDefaults.UsersTableName + " WHERE (UserName=" + SM.Quote(_UserName.ToString()) 
                + ")AND(Password=" + SM.Quote(Hash(_UserName, _Password)) + ")AND" + SM.SqlNotDeleted(), _LogDetails);
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
                ds = new SMDataset(SM.MainAlias, SM);
                //
                sql = "SELECT " + SMDefaults.RulesTableName + ".* FROM " + SMDefaults.UsersRulesTableName
                    + " INNER JOIN " + SMDefaults.RulesTableName + " ON ("+ SMDefaults.UsersRulesTableName + ".IdRule=sm_rules.IdRule)"
                    + " WHERE ("+ SMDefaults.UsersRulesTableName + ".IdUser=" + IdUser.ToString()
                    + ")AND" + SM.SqlNotDeleted("Deleted", SMDefaults.UsersRulesTableName)
                    + "AND" + SM.SqlNotDeleted("Deleted", SMDefaults.RulesTableName)
                    + " ORDER BY "+ SMDefaults.UsersRulesTableName + ".IdRule";
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
                                ds.Exec("INSERT INTO "+ SMDefaults.UsersRulesTableName + " (IdUser,IdRule,Deleted,InsertionDate,InsertionUser) VALUES ("
                                    + IdUser.ToString() + "," + rules[i].IdRule.ToString() + ",0," + SM.Quote(DateTime.Now, ds.Database.Type) + "," + SM.Quote(SM.ExecutableName) + ")");
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
                Organizations.Clear();
                ds = new SMDataset(SM.MainAlias, SM);
                //
                sql = "SELECT " + SMDefaults.OrganizationsTableName + ".* FROM " + SMDefaults.UsersOrganizationsTableName
                    + " INNER JOIN " + SMDefaults.OrganizationsTableName + " ON (" + SMDefaults.UsersOrganizationsTableName + ".IdOrganization=sm_organizations.IdOrganization)"
                    + " WHERE ("+ SMDefaults.UsersOrganizationsTableName + ".IdUser=" + IdUser.ToString()
                    + ")AND" + SM.SqlNotDeleted("Deleted", SMDefaults.UsersOrganizationsTableName)
                    + "AND" + SM.SqlNotDeleted("Deleted", SMDefaults.OrganizationsTableName)
                    + " ORDER BY "+ SMDefaults.UsersOrganizationsTableName + ".IdOrganization";
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
                                ds.Exec("INSERT INTO "+ SMDefaults.UsersOrganizationsTableName + " (IdUser,IdOrganization,Deleted,InsertionDate,InsertionUser) VALUES ("
                                    + IdUser.ToString() + "," + organizations[i].IdOrganization.ToString() + ",0," + SM.Quote(DateTime.Now, ds.Database.Type) + "," + SM.Quote(SM.ExecutableName) + ")");
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
                        FirstName = SM.ToStr(_Dataset["FirstName"]);
                        LastName = SM.ToStr(_Dataset["LastName"]);
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
                            if (i > -1)
                            {
                                if (Organizations.Count > 0)
                                {
                                    Organization.Assign(Organizations[0]);
                                }
                                rslt += i;
                            }
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

        /// <summary>Set current organization by id.</summary>
        public bool SetOrganization(int _IdOrganization)
        {
            int i = Organizations.Find(_IdOrganization);
            if (i > -1)
            {
                Organization.Assign(Organizations[i]);
                return true;
            }
            else return false;
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