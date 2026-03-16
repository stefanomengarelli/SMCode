/*  ===========================================================================
 *  
 *  File:       SMUser_Load.cs
 *  Version:    2.0.252
 *  Date:       March 2026
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2026 by Stefano Mengarelli - All rights reserved - Use, 
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
    public partial class SMUser
    {

        /* */

        #region Methods

        /*  ===================================================================
         *  Methods
         *  ===================================================================
         */

        /// <summary>Load user information by sql query.  
        /// Return user id if success, 0 if user cannot be found or -1 if error.</summary>
        public int Load(string _Sql, bool _LoadAllDependencies = true)
        {
            int rslt = -1;
            SMDataset ds;
            try
            {
                Clear();
                if (!SM.Empty(_Sql))
                {
                    ds = new SMDataset(SM.MainAlias, SM, true);
                    if (ds.Open(_Sql)) rslt = Read(ds, _LoadAllDependencies);
                    ds.Close();
                    ds.Dispose();
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
        /// Return user id if success, 0 if user cannot be found or -1 if error.</summary>
        public int LoadById(int _IdUser, bool _LoadAllDependencies = true)
        {
            if (_IdUser < 1)
            {
                Clear();
                return 0;
            }
            else return Load(
                $"SELECT * FROM {SMDefaults.UsersTableName}"
                + $" WHERE ({SMDefaults.UsersTableName_IdUser}={_IdUser})"
                + $" AND{SM.SqlNotDeleted(SMDefaults.UsersTableName_Deleted)}",
                _LoadAllDependencies
                );
        }

        /// <summary>Load user information by uid. Log details can be specified as parameter.
        /// Return 1 if success, 0 if user cannot be found or -1 if error.</summary>
        public int LoadByUid(string _UidUser, bool _LoadAllDependencies = true)
        {
            if (SM.Empty(_UidUser))
            {
                Clear();
                return 0;
            }
            else return Load(
                $"SELECT * FROM {SMDefaults.UsersTableName}"
                + $" WHERE ({SMDefaults.UsersTableName_UidUser}={SM.Quote(_UidUser)})"
                + $" AND{SM.SqlNotDeleted(SMDefaults.UsersTableName_Deleted)}",
                _LoadAllDependencies
                );
        }

        /// <summary>Load user information by user-id and password. Log details can be specified as parameter.
        /// Return 1 if success, 0 if user cannot be found or -1 if error.</summary>
        public int LoadByCredentials(string _UserName, string _Password, bool _LoadAllDependencies = true)
        {
            string sql = $"SELECT * FROM {SMDefaults.UsersTableName}"
                + $" WHERE ({SMDefaults.UsersTableName_UserName}={SM.Quote(_UserName.ToString())})"
                + $" AND({SMDefaults.UsersTableName_Password}={SM.Quote(Hash(_UserName, _Password))})"
                + $" AND{SM.SqlNotDeleted(SMDefaults.UsersTableName_Deleted)}";
            return Load(sql, _LoadAllDependencies);
        }

        /// <summary>Load user related rules. Return 1 if success, 0 if fail or -1 if error.</summary>
        public int LoadRules()
        {
            int i, rslt = -1;
            string sql;
            SMDataset ds;
            SMRule rule;
            SMRules rules;
            try
            {
                Rules.Clear();
                ds = new SMDataset(SM.MainAlias, SM, true);
                //
                sql = $"SELECT {SMDefaults.RulesTableName}.* FROM {SMDefaults.UsersRulesTableName}"
                    + $" INNER JOIN {SMDefaults.RulesTableName}"
                    + $" ON {SMDefaults.UsersRulesTableName}.{SMDefaults.UsersRulesTableName_IdRule}={SMDefaults.RulesTableName}.{SMDefaults.RulesTableName_IdRule}"
                    + $" WHERE ({SMDefaults.UsersRulesTableName}.{SMDefaults.UsersRulesTableName_IdUser}={IdUser})"
                    + $" AND{SM.SqlNotDeleted(SMDefaults.UsersRulesTableName_Deleted, SMDefaults.UsersRulesTableName)}"
                    + $" AND{SM.SqlNotDeleted(SMDefaults.RulesTableName_Deleted, SMDefaults.RulesTableName)}"
                    + $" ORDER BY {SMDefaults.UsersRulesTableName}.{SMDefaults.UsersRulesTableName_IdRule}";
                //
                if (ds.Open(sql))
                {
                    if (ds.Eof)
                    {
                        rules = new SMRules(SM);
                        if (rules.Load(true) > 0)
                        {
                            if (ds.Open($"SELECT * FROM {SMDefaults.UsersRulesTableName} WHERE {SMDefaults.UsersRulesTableName_IdUser}<0"))
                            {
                                for (i = 0; i < rules.Count; i++)
                                {
                                    if (ds.Append())
                                    {
                                        ds.Assign(SMDefaults.UsersRulesTableName_IdUser, IdUser);
                                        ds.Assign(SMDefaults.UsersRulesTableName_IdRule, rules[i].IdRule);
                                        ds.Assign(SMDefaults.UsersRulesTableName_Deleted, 0);
                                        if (!ds.Post()) ds.Cancel();
                                    }
                                }
                                ds.Close();
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
                    rslt = Rules.Count;
                    ds.Close();
                }
                ds.Dispose();
            }
            catch (Exception ex)
            {
                SM.Error(ex);
                rslt = -1;
            }
            return rslt;
        }

        /// <summary>Load user related organizations. Return 1 if success, 0 if fail or -1 if error.</summary>
        public int LoadOrganizations()
        {
            int i, rslt = -1;
            string sql;
            SMDataset ds;
            SMOrganization organization;
            SMOrganizations organizations;
            try
            {
                Organizations.Clear();
                ds = new SMDataset(SM.MainAlias, SM, true);
                //
                sql = $"SELECT {SMDefaults.OrganizationsTableName}.* FROM {SMDefaults.UsersOrganizationsTableName} INNER JOIN {SMDefaults.OrganizationsTableName}"
                    + $" ON {SMDefaults.UsersOrganizationsTableName}.IdOrganization={SMDefaults.OrganizationsTableName}.IdOrganization"
                    + $" WHERE ({SMDefaults.UsersOrganizationsTableName}.IdUser={IdUser})AND{SM.SqlNotDeleted("Deleted", SMDefaults.UsersOrganizationsTableName)}"
                    + $" AND{SM.SqlNotDeleted("Deleted", SMDefaults.OrganizationsTableName)} ORDER BY {SMDefaults.UsersOrganizationsTableName}.IdOrganization";
                //
                if (ds.Open(sql))
                {
                    if (ds.Eof)
                    {
                        organizations = new SMOrganizations(SM);
                        if (organizations.Load(true) > 0)
                        {
                            if (ds.Open($"SELECT * FROM {SMDefaults.UsersOrganizationsTableName} WHERE {SMDefaults.UsersOrganizationsTableName_IdUser}<0"))
                            {
                                for (i = 0; i < organizations.Count; i++)
                                {
                                    if (ds.Append())
                                    {
                                        ds.Assign(SMDefaults.UsersOrganizationsTableName_IdUser, IdUser);
                                        ds.Assign(SMDefaults.UsersOrganizationsTableName_IdOrganization, organizations[i].IdOrganization);
                                        ds.Assign(SMDefaults.UsersOrganizationsTableName_Deleted, 0);
                                        if (!ds.Post()) ds.Cancel();
                                    }
                                }
                                ds.Close();
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
                    rslt = Rules.Count;
                    ds.Close();
                }
                ds.Dispose();
            }
            catch (Exception ex)
            {
                SM.Error(ex);
                rslt = -1;
            }
            return rslt;
        }

        /// <summary>Read item from current record of dataset. Return id user if success, 0 if fail or -1 if error.</summary>
        public int Read(SMDataset _Dataset, bool _LoadAllDependencies = true)
        {
            int i, rslt = -1;
            string c;
            SMDataset ds;
            try
            {
                if (_Dataset != null)
                {
                    Clear();
                    if (_Dataset.Eof) rslt = 0;
                    else
                    {
                        //
                        // load user properties
                        // 
                        IdUser = _Dataset.FieldInt(SMDefaults.UsersTableName_IdUser);
                        UidUser = _Dataset.FieldStr(SMDefaults.UsersTableName_UidUser);
                        UserName = _Dataset.FieldStr(SMDefaults.UsersTableName_UserName);
                        Text = _Dataset.FieldStr(SMDefaults.UsersTableName_Text);
                        FirstName = _Dataset.FieldStr(SMDefaults.UsersTableName_FirstName);
                        LastName = _Dataset.FieldStr(SMDefaults.UsersTableName_LastName);
                        Email = _Dataset.FieldStr(SMDefaults.UsersTableName_Email);
                        Password = _Dataset.FieldStr(SMDefaults.UsersTableName_Password);
                        Pin = _Dataset.FieldInt(SMDefaults.UsersTableName_Pin);
                        Register = _Dataset.FieldInt(SMDefaults.UsersTableName_Register);
                        TaxCode = _Dataset.FieldStr(SMDefaults.UsersTableName_TaxCode);
                        BirthDate = _Dataset.FieldDate(SMDefaults.UsersTableName_BirthDate);
                        Sex = _Dataset.FieldChar(SMDefaults.UsersTableName_Sex);
                        Icon = _Dataset.FieldStr(SMDefaults.UsersTableName_Icon);
                        Image = _Dataset.FieldStr(SMDefaults.UsersTableName_Image);
                        Note = _Dataset.FieldStr(SMDefaults.UsersTableName_Note);
                        rslt = IdUser;
                        //
                        // load all dependencies
                        //
                        if (_LoadAllDependencies)
                        {
                            //
                            // load properties to dictionary
                            //
                            for (i = 0; i < _Dataset.Columns.Count; i++)
                            {
                                c = _Dataset.Columns[i].ColumnName;
                                Properties.Add(new SMDictionaryItem(c, _Dataset.FieldStr(c), _Dataset.Field(c)));
                            }
                            //
                            // load user extended properties to dictionary
                            //
                            if (!SM.Empty(SM.UserExtendTable))
                            {
                                ds = new SMDataset(_Dataset.Database, SM);
                                if (ds.Open($"SELECT * FROM {SM.UserExtendTable} WHERE {SM.UserExtendTableIdUserFieldName}={IdUser}"))
                                {
                                    if (!ds.Eof)
                                    {
                                        for (i = 0; i < ds.Columns.Count; i++)
                                        {
                                            c = ds.Columns[i].ColumnName;
                                            Properties.Add(new SMDictionaryItem(c, ds.FieldStr(c), ds.Field(c)));
                                        }
                                    }
                                    ds.Close();
                                }
                                else rslt = -1;
                                ds.Dispose();
                            }
                            //
                            // load user rules and organizations
                            //
                            if (LoadRules() > -1)
                            {
                                if (LoadOrganizations() > -1)
                                {
                                    if (Organizations.Count > 0)
                                    {
                                        Organization.Assign(Organizations[0]);
                                    }
                                }
                                else rslt = -1;
                            }
                            else rslt = -1;
                        }
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

        #endregion

        /* */

    }

    /* */

}