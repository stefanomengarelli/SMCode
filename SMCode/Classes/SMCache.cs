/*  ===========================================================================
 *  
 *  File:       SMCache.cs
 *  Version:    2.0.201
 *  Date:       January 2025
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2025 by Stefano Mengarelli - All rights reserved - Use, 
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

        /// <summary>Items collection.</summary>
        private SMDictionary items;

        #endregion

        /* */

        #region Properties

        /*  ===================================================================
         *  Properties
         *  ===================================================================
         */

        /// <summary>Quick access declaration.</summary>
        public SMDictionaryItem this[int _Index]
        {
            get { return items[_Index]; }
        }

        /// <summary>Get or set cache db alias.</summary>
        public string Alias { get; set; } = "MAIN";

        /// <summary>Get items count.</summary>
        public int Count { get { return items.Count; } }

        /// <summary>Return current id user or 0 if public.</summary>
        public int IdUser { 
            get
            {
                if (Public) return 0;
                else return SM.User.IdUser;
            } 
        }

        /// <summary>Get cache dictionary.</summary>
        public SMDictionary Items { get { return items; } } 

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
            items = new SMDictionary(SM);
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
            items.Assign(_Cache.items);
        }

        /// <summary>Clear item.</summary>
        public void Clear()
        {
            items.Clear();
        }

        /// <summary>Get cache value by key.</summary>
        public string Get(string _Key)
        {
            SMDictionaryItem item;
            if (items.Count < 1) Read();
            item = items.GetItem(_Key);
            if (item == null) return "";
            else if (item.Tag == null) return item.Value;
            else if ((DateTime)item.Tag < DateTime.Now) return "";
            else return item.Value;
        }

        /// <summary>Read database cache.</summary>
        public bool Read(bool _Append = false)
        {
            bool obsolete = false, rslt = false;
            DateTime expire, now = DateTime.Now;
            SMDataset ds;
            try
            {
                if (Public || (IdUser > 0))
                {
                    ds = new SMDataset(Alias, SM, true);
                    if (ds.Open($"SELECT * FROM {TableName} WHERE CacheUser={IdUser} ORDER BY CacheKey"))
                    {
                        if (!_Append) items.Clear();
                        rslt = true;
                        while (!ds.Eof)
                        {
                            expire = ds.FieldDateTime("CacheExpire");
                            if (expire > now)
                            {
                                items.Add(new SMDictionaryItem(
                                    ds.FieldStr("CacheKey"),
                                    ds.FieldStr("CacheValue"),
                                    ds.FieldDateTime("CacheExpire")
                                    ));
                            }
                            else obsolete = true;
                            ds.Next();
                        }
                        if (obsolete) ds.Exec($"DELETE FROM {TableName} WHERE CacheExpire<{SM.Quote(now, ds.Database.Type)}");
                        ds.Close();
                    }
                    ds.Dispose();
                }
            }
            catch (Exception ex)
            {
                SM.Error(ex);
                rslt = false;
            }
            return rslt;
        }

        /// <summary>Set database cache.</summary>
        public bool Set(string _Key, string _Value, DateTime? _Expiration = null)
        {
            bool rslt = false;
            SMDataset ds;
            try
            {
                if (Public || (IdUser > 0))
                {
                    if (_Expiration == null) _Expiration = DateTime.Now.AddDays(1);
                    items.Set(_Key, _Value, _Expiration);
                    ds = new SMDataset(Alias, SM, true);
                    if (ds.Open($"SELECT * FROM {TableName} WHERE (CacheUser={IdUser})AND(CacheKey={SM.Quote(_Key)})"))
                    {
                        if (ds.Eof) rslt = ds.Append();
                        else rslt = ds.Edit();
                        if (rslt)
                        {
                            ds.Assign("CacheUser", SM.Iif(Public, 0, SM.User.IdUser));
                            ds.Assign("CacheKey", _Key);
                            ds.Assign("CacheValue", _Value);
                            ds.Assign("CacheExpire", _Expiration);
                            rslt = ds.Post();
                            if (!rslt) ds.Cancel();
                        }
                        ds.Close();
                    }
                    ds.Dispose();
                }
            }
            catch (Exception ex)
            {
                SM.Error(ex);
                rslt = false;
            }
            return rslt;
        }

        #endregion

        /* */

    }

    /* */

}