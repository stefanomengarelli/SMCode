/*  ===========================================================================
 *  
 *  File:       SMCache.cs
 *  Version:    2.0.82
 *  Date:       November 2024
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2024 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *
 *  SMCode db cache management class.
 *
 *  ===========================================================================
 */

using System;

namespace SMCodeSystem
{

    /* */

    /// <summary>SMCode db cache management class.</summary>
    public class SMCache
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

        /// <summary>Get or set cache db alias.</summary>
        public string Alias { get; set; } = "MAIN";

        /// <summary>Get cache items collection.</summary>
        public SMDictionary Items { get; private set; }

        /// <summary>Get or set public cache property.</summary>
        public bool Public { get; set; } = false;

        /// <summary>Get or set cache db table name.</summary>
        public string TableName { get; set; } = SMDefaults.CacheTableName;

        #endregion

        /* */

        #region Initialization

        /*  ===================================================================
         *  Initialization
         *  ===================================================================
         */

        /// <summary>Class constructor.</summary>
        public SMCache(SMCode _SM = null)
        {
            SM = SMCode.CurrentOrNew(_SM);
            Initialize();
        }

        /// <summary>Class constructor.</summary>
        public SMCache(SMCache _Cache, SMCode _SM = null)
        {
            if (_SM == null) _SM = _Cache.SM;
            SM = SMCode.CurrentOrNew(_SM);
            Initialize();
            Assign(_Cache);
        }

        /// <summary>Initialize instance.</summary>
        private void Initialize()
        {
            Items = new SMDictionary(SM);
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
        public void Assign(SMCache _Cache)
        {
            Items.Assign(_Cache.Items);
        }

        /// <summary>Clear item.</summary>
        public void Clear()
        {
            Items.Clear();
        }

        /// <summary>Get cache value by key.</summary>
        public string Get(string _Key)
        {
            int i;
            if (Items.Count < 1) Read();
            i = Items.Find(_Key);
            SMDictionaryItem item = Items.Get(_Key);
            if (item == null) return "";
            else if (item.Tag == null) return item.Value;
            else if ((DateTime)item.Tag < DateTime.Now) return "";
            else return item.Value;
        }

        /// <summary>Read database cache.</summary>
        public bool Read()
        {
            bool obsolete = false;
            DateTime expire, now = DateTime.Now;
            SMDataset ds = new SMDataset(Alias);
            if (ds.Open("SELECT * FROM " + TableName + " WHERE CacheUser=" + SM.Iif(Public, "0", SM.User.IdUser.ToString()) + " ORDER BY CacheKey"))
            {
                while (!ds.Eof)
                {
                    expire = ds.FieldDateTime("CacheExpire");
                    if (expire > now)
                    {
                        Items.Add(new SMDictionaryItem(
                            ds.FieldStr("CacheKey"),
                            ds.FieldStr("CacheValue"),
                            ds.FieldDateTime("CacheExpire")
                            ));
                    }
                    else obsolete = true;
                    ds.Next();
                }
                if (obsolete) ds.Exec("DELETE FROM " + TableName + " WHERE CacheExpire<" + SM.Quote(now, ds.Database.Type));
                ds.Close();
                return true;
            }
            else return false;
        }

        /// <summary>Set database cache.</summary>
        public bool Set(string _Key, string _Value, DateTime? _Expiration)
        {
            bool r = false;
            if (_Expiration == null) _Expiration = DateTime.Now.AddDays(1);
            Items.Set(_Key, _Value, _Expiration);
            SMDataset ds = new SMDataset(Alias);
            if (ds.Open("SELECT * FROM " + TableName + " WHERE (CacheUser="
                + SM.Iif(Public, "0", SM.User.IdUser.ToString())
                + ")AND(CacheKey=" + SM.Quote(_Key) + ")"))
            {
                if (ds.Eof) r = ds.Append();
                else r = ds.Edit();
                if (r)
                {
                    ds.Assign("CacheUser", SM.Iif(Public, 0, SM.User.IdUser));
                    ds.Assign("CacheKey", _Key);
                    ds.Assign("CacheValue", _Value);
                    ds.Assign("CacheExpire", _Expiration);
                    r = ds.Post();
                    if (!r) ds.Cancel();
                }
                ds.Close();
                return true;
            }
            else return false;
        }

        #endregion

        /* */

    }

    /* */

}