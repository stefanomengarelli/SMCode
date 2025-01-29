/*  ===========================================================================
 *  
 *  File:       SMRules.cs
 *  Version:    2.0.140
 *  Date:       January 2025
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2025 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *
 *  SMCode rules collection class.
 *
 *  ===========================================================================
 */

using System;

namespace SMCodeSystem
{

    /* */

    /// <summary>SMCode rules collection class.</summary>
    public class SMRules
    {

        /* */

        #region Declarations

        /*  ===================================================================
         *  Declarations
         *  ===================================================================
         */

        /// <summary>SM session instance.</summary>
        private readonly SMCode SM = null;

        /// <summary>Rules collection.</summary>
        private SMDictionary items;

        #endregion

        /* */

        #region Properties

        /*  ===================================================================
         *  Properties
         *  ===================================================================
         */

        /// <summary>Quick access declaration.</summary>
        public SMRule this[int _Index]
        {
            get { return (SMRule)items[_Index].Tag; }
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
        public SMRules(SMCode _SM)
        {
            SM = SMCode.CurrentOrNew(_SM);
            items = new SMDictionary(SM);
            Clear();
        }

        /// <summary>Class constructor.</summary>
        public SMRules(SMRules _OtherInstance, SMCode _SM)
        {
            if (_SM == null) _SM = _OtherInstance.SM;
            SM = SMCode.CurrentOrNew(_SM);
            items = new SMDictionary(SM);
            Assign(_OtherInstance);
        }

        #endregion

        /* */

        #region Methods

        /*  ===================================================================
         *  Methods
         *  ===================================================================
         */

        /// <summary>Add rule item.</summary>
        public int Add(SMRule _Rule)
        {
            return items.Add(new SMDictionaryItem(_Rule.IdRule.ToString(), _Rule.Text, _Rule));
        }

        /// <summary>Assign instance properties from another.</summary>
        public void Assign(SMRules _OtherInstance)
        {
            items.Assign(_OtherInstance.items);
        }

        /// <summary>Clear item.</summary>
        public void Clear()
        {
            items.Clear();
        }

        /// <summary>Find rule by id.</summary>
        public int Find(int _IdRule)
        {
            return items.Find(_IdRule.ToString());
        }

        /// <summary>Get rule by id.</summary>
        public SMRule Get(int _IdRule, bool _ReturnNewInstanceIfNotFound = false)
        {
            int i = items.Find(_IdRule.ToString());
            if (i < 0)
            {
                if (_ReturnNewInstanceIfNotFound) return new SMRule(SM);
                else return null;
            }
            else return (SMRule)items[i].Tag;
        }

        /// <summary>Return true if user has rule with specified id.</summary>
        public bool Has(int _IdRule)
        {
            return Has(new int[] { _IdRule });
        }

        /// <summary>Return true if user has at least one of rule with specified id.</summary>
        public bool Has(int[] _IdRules)
        {
            int i = 0;
            bool r = false;
            if (_IdRules != null)
            {
                while (!r && (i < _IdRules.Length))
                {
                    r = Find(_IdRules[i]) > -1;
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

        /// <summary>Load rule collection. Return 1 if success, 0 if fail or -1 if error.</summary>
        public int Load(bool _OnlyByDefault = false)
        {
            int rslt = -1;
            string sql;
            SMDataset ds;
            SMRule rule;
            try
            {
                Clear();
                ds = new SMDataset(SM.MainAlias, SM);
                sql = "SELECT * FROM "+ SMDefaults.RulesTableName + " WHERE " + SM.SqlNotDeleted();
                if (_OnlyByDefault) sql += "AND(ByDefault=1)";
                sql += " ORDER BY IdRule";
                if (ds.Open(sql))
                {
                    while (!ds.Eof)
                    {
                        rule = new SMRule(SM);
                        if (rule.Read(ds) > 0) Add(rule);
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