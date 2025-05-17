/*  ===========================================================================
 *  
 *  File:       SMOrganizations.cs
 *  Version:    2.0.252
 *  Date:       May 2025
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2025 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *
 *  SMCode organizations collection class.
 *
 *  ===========================================================================
 */

using System;

namespace SMCodeSystem
{

    /* */

    /// <summary>SMCode organizations collection class.</summary>
    public class SMOrganizations
    {

        /* */

        #region Declarations

        /*  ===================================================================
         *  Declarations
         *  ===================================================================
         */

        /// <summary>SM session instance.</summary>
        private readonly SMCode SM = null;

        /// <summary>Organizations collection.</summary>
        private SMDictionary items = new SMDictionary();

        #endregion

        /* */

        #region Properties

        /*  ===================================================================
         *  Properties
         *  ===================================================================
         */

        /// <summary>Quick access declaration.</summary>
        public SMOrganization this[int _Index]
        {
            get { return (SMOrganization)items[_Index].Tag; }
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
        public SMOrganizations(SMCode _SM = null)
        {
            SM = SMCode.CurrentOrNew(_SM);
            Clear();
        }

        /// <summary>Class constructor.</summary>
        public SMOrganizations(SMOrganizations _OtherInstance, SMCode _SM = null)
        {
            if (_SM == null) _SM = _OtherInstance.SM;
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

        /// <summary>Add organization item.</summary>
        public int Add(SMOrganization _Organization)
        {
            return items.Add(new SMDictionaryItem(_Organization.IdOrganization.ToString(), _Organization.Text, _Organization));
        }

        /// <summary>Assign instance properties from another.</summary>
        public void Assign(SMOrganizations _OtherInstance)
        {
            items.Assign(_OtherInstance.items);
        }

        /// <summary>Clear item.</summary>
        public void Clear()
        {
            items.Clear();
        }

        /// <summary>Find organization by id.</summary>
        public int Find(int _IdOrganization)
        {
            return items.Find(_IdOrganization.ToString());
        }

        /// <summary>Get organization by id.</summary>
        public SMOrganization Get(int _IdOrganization, bool _ReturnNewInstanceIfNotFound = false)
        {
            int i = items.Find(_IdOrganization.ToString());
            if (i < 0)
            {
                if (_ReturnNewInstanceIfNotFound) return new SMOrganization(SM);
                else return null;
            }
            else return (SMOrganization)items[i].Tag;
        }

        /// <summary>Return true if user has organization with specified id.</summary>
        public bool Has(int _IdOrganization)
        {
            return Has(new int[] { _IdOrganization });
        }

        /// <summary>Return true if user has at least one of organization with specified id.</summary>
        public bool Has(int[] _IdOrganizations)
        {
            int i = 0;
            bool r = false;
            if (_IdOrganizations != null)
            {
                while (!r && (i < _IdOrganizations.Length))
                {
                    r = Find(_IdOrganizations[i]) > -1;
                    i++;
                }
            }
            return r;
        }

        /// <summary>Return keys list as a string with separator and quote specified.</summary>
        public string Keys(string _Quote = "", string _Separator = ",")
        {
            return items.Keys(_Quote, _Separator);
        }

        /// <summary>Load organization collection. Return 1 if success, 0 if fail or -1 if error.</summary>
        public int Load(bool _OnlyByDefault = false)
        {
            int rslt = -1;
            string sql;
            SMDataset ds;
            SMOrganization organization;
            try
            {
                Clear();
                ds = new SMDataset(SM.MainAlias, SM, true);
                sql = $"SELECT * FROM {SMDefaults.OrganizationsTableName} WHERE {SM.SqlNotDeleted(SMDefaults.OrganizationsTableName_Deleted)}";
                if (_OnlyByDefault) sql += $"AND({SMDefaults.OrganizationsTableName_ByDefault}=1)";
                sql += $" ORDER BY {SMDefaults.OrganizationsTableName_IdOrganization}";
                if (ds.Open(sql))
                {
                    while (!ds.Eof)
                    {
                        organization = new SMOrganization(SM);
                        if (organization.Read(ds) > 0) Add(organization);
                        ds.Next();
                    }
                    rslt = Count;
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

        #endregion

        /* */

    }

    /* */

}