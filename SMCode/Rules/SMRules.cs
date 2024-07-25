/*  ===========================================================================
 *  
 *  File:       SMRules.cs
 *  Version:    2.0.38
 *  Date:       July 2024
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2024 by Stefano Mengarelli - All rights reserved - Use, 
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
        private SMDictionary items = new SMDictionary();

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
        public SMRules(SMCode _SM = null)
        {
            SM = SMCode.CurrentOrNew(_SM);
            Clear();
        }

        /// <summary>Class constructor.</summary>
        public SMRules(SMRules _OtherInstance, SMCode _SM = null)
        {
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

        /// <summary>Add user item.</summary>
        public int Add(SMRule _Rule)
        {
            items.Add(new SMDictionaryItem(_Rule.Id.ToString(), _Rule.Description, _Rule));
            return items.Count - 1;
        }

        /// <summary>Add user item.</summary>
        public int Add(int _Id, string _Description, string _Icon = "", string _Image = "", bool _Default = false, string _Uid = "", SMCode _SM = null)
        {
            if (_SM == null) _SM = SM;
            return Add(new SMRule(_Id, _Description, _Icon, _Image, _Default, _Uid, _SM));
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
        public int Find(int _Id)
        {
            return items.Find(_Id.ToString());
        }

        /// <summary>Get rule by id.</summary>
        public SMRule Get(int _Id)
        {
            int i = items.Find(_Id.ToString());
            if (i < 0) return null;
            else return (SMRule)items[i].Tag;
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
                ds = new SMDataset(Alias, SM);
                sql = "SELECT * FROM " + TableName;
                if (!SM.Empty(DeletedColumn)) sql += " WHERE " + SM.SqlNotDeleted(DeletedColumn);
                if (_OnlyByDefault) sql += "AND(ByDefault=1)";
                sql += " ORDER BY " + IdColumn;
                if (ds.Open(sql))
                {
                    Clear();
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

        #region Static Properties

        /*  ===================================================================
         *  Static Properties
         *  ===================================================================
         */

        /// <summary>Database alias.</summary>
        public static string Alias { get; set; } = "MAIN";

        /// <summary>Users table name.</summary>
        public static string TableName { get; set; } = "sm_rules";

        /// <summary>Users table id column.</summary>
        public static string IdColumn { get; set; } = "IdRule";

        /// <summary>Users table UID column.</summary>
        public static string UidColumn { get; set; } = "UidRule";

        /// <summary>Users table description column.</summary>
        public static string DescriptionColumn { get; set; } = "Description";

        /// <summary>Users table icon column.</summary>
        public static string IconColumn { get; set; } = "Icon";

        /// <summary>Users table image column.</summary>
        public static string ImageColumn { get; set; } = "Image";

        /// <summary>Users table parameters column.</summary>
        public static string ParametersColumn { get; set; } = "Parameters";

        /// <summary>Users table default column.</summary>
        public static string DefaultColumn { get; set; } = "ByDefault";

        /// <summary>Users table deleted column.</summary>
        public static string DeletedColumn { get; set; } = "Deleted";

        #endregion

        /* */

    }

    /* */

}