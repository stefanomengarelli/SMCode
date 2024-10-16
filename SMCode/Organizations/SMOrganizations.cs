/*  ===========================================================================
 *  
 *  File:       SMOrganizations.cs
 *  Version:    2.0.52
 *  Date:       October 2024
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2024 by Stefano Mengarelli - All rights reserved - Use, 
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
            items.Add(new SMDictionaryItem(_Organization.Id.ToString(), _Organization.Caption, _Organization));
            return items.Count - 1;
        }

        /// <summary>Add organization item.</summary>
        public int Add(int _Id, string _Description, string _Icon = "", string _Image = "", bool _Default = false, string _Uid = "", SMCode _SM = null)
        {
            if (_SM == null) _SM = SM;
            return Add(new SMOrganization(_Id, _Description, _Icon, _Image, _Default, _Uid, _SM));
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
        public int Find(int _Id)
        {
            return items.Find(_Id.ToString());
        }

        /// <summary>Get organization by id.</summary>
        public SMOrganization Get(int _Id, bool _ReturnNewInstanceIfNotFound = false)
        {
            int i = items.Find(_Id.ToString());
            if (i < 0)
            {
                if (_ReturnNewInstanceIfNotFound) return new SMOrganization(SM);
                else return null;
            }
            else return (SMOrganization)items[i].Tag;
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
                ds = new SMDataset(SM.UserDBAlias, SM);
                sql = "SELECT * FROM sm_organizations WHERE " + SM.SqlNotDeleted();
                if (_OnlyByDefault) sql += "AND(ByDefault=1)";
                sql += " ORDER BY IdOrganization";
                if (ds.Open(sql))
                {
                    while (!ds.Eof)
                    {
                        organization = new SMOrganization(SM);
                        if (organization.Read(ds) > 0) Add(organization);
                        ds.Next();
                    }
                    rslt = Count;
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