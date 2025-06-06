/*  ===========================================================================
 *  
 *  File:       SMRule.cs
 *  Version:    2.0.252
 *  Date:       May 2025
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2025 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *
 *  SMCode rule class.
 *
 *  ===========================================================================
 */

using System;
using System.Text.Json;

namespace SMCodeSystem
{

    /* */

    /// <summary>SMCode rule class.</summary>
    public class SMRule
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

        /// <summary>Get or set rule id.</summary>
        public int IdRule { get; set; }

        /// <summary>Get or set rule UID.</summary>
        public string UidRule { get; set; }

        /// <summary>Get or set rule description.</summary>
        public string Text { get; set; }

        /// <summary>Get or set rule icon path.</summary>
        public string Icon { get; set; }

        /// <summary>Get or set rule image path.</summary>
        public string Image { get; set; }

        /// <summary>Get or set rule parameters.</summary>
        public SMDictionary Parameters { get; private set; }

        /// <summary>Get or set by-default rule flag.</summary>
        public bool ByDefault { get; set; }

        /// <summary>Return true if rule is empty.</summary>
        public bool Empty { get { return (IdRule < 1) || SM.Empty(Text); } }

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
        public SMRule(SMCode _SM = null)
        {
            SM = SMCode.CurrentOrNew(_SM);
            InitializeInstance();
        }

        /// <summary>Class constructor.</summary>
        public SMRule(SMRule _OtherInstance, SMCode _SM = null)
        {
            if (_SM == null) _SM = _OtherInstance.SM;
            SM = SMCode.CurrentOrNew(_SM);
            InitializeInstance();
            Assign(_OtherInstance);
        }

        /// <summary>Initialize instance.</summary>
        private void InitializeInstance()
        {
            Parameters = new SMDictionary();
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
        public void Assign(SMRule _OtherInstance)
        {
            IdRule = _OtherInstance.IdRule;
            UidRule = _OtherInstance.UidRule;
            Text = _OtherInstance.Text;
            Icon = _OtherInstance.Icon;
            Image = _OtherInstance.Image;
            Parameters.Assign(_OtherInstance.Parameters);
            ByDefault = _OtherInstance.ByDefault;
            Tag = _OtherInstance.Tag;
        }

        /// <summary>Clear item.</summary>
        public void Clear()
        {
            IdRule = 0;
            UidRule = "";
            Text = "";
            Icon = "";
            Image = "";
            Parameters.Clear();
            ByDefault = false;
            Tag = null;
        }

        /// <summary>Assign property from JSON serialization.</summary>
        public bool FromJSON(string _JSON)
        {
            try
            {
                Assign((SMRule)JsonSerializer.Deserialize(_JSON, null));
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

        /// <summary>Load rule information by sql query 
        /// Return 1 if success, 0 if fail or -1 if error.</summary>
        public int Load(string _Sql)
        {
            int rslt = -1;
            SMDataset ds;
            try
            {
                Clear();
                if (!SM.Empty(_Sql))
                {
                    ds = new SMDataset(SM.MainAlias, SM, true);
                    if (ds.Open(_Sql))
                    {
                        if (ds.Eof) rslt = 0;
                        else rslt = Read(ds);
                        ds.Close();
                    }
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

        /// <summary>Load rule information by id.
        /// Return 1 if success, 0 if fail or -1 if error.</summary>
        public int Load(int _IdRule)
        {
            return Load($"SELECT * FROM {SMDefaults.RulesTableName}"
                + $" WHERE ({SMDefaults.RulesTableName_IdRule}={_IdRule})"
                + $" AND{SM.SqlNotDeleted(SMDefaults.RulesTableName_Deleted)}");
        }

        /// <summary>Read item from current record of dataset. Return 1 if success, 0 if fail or -1 if error.</summary>
        public int Read(SMDataset _Dataset)
        {
            try
            {
                if (_Dataset != null)
                {
                    if (!_Dataset.Eof)
                    {
                        Clear();
                        IdRule = _Dataset.FieldInt(SMDefaults.RulesTableName_IdRule);
                        UidRule = _Dataset.FieldStr(SMDefaults.RulesTableName_UidRule);
                        Text = _Dataset.FieldStr(SMDefaults.RulesTableName_Text);
                        Icon = _Dataset.FieldStr(SMDefaults.RulesTableName_Icon);
                        Image = _Dataset.FieldStr(SMDefaults.RulesTableName_Image);
                        Parameters.FromParameters(_Dataset.FieldStr(SMDefaults.RulesTableName_Parameters));
                        ByDefault = _Dataset.FieldBool(SMDefaults.RulesTableName_ByDefault);
                        return 1;
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

        /// <summary>Write current record of dataset from item. Return 1 if success, 0 if fail or -1 if error.</summary>
        public int Write(SMDataset _Dataset)
        {
            try
            {
                if (_Dataset != null)
                {
                    if (!_Dataset.Eof)
                    {
                        _Dataset.Assign(SMDefaults.RulesTableName_IdRule, IdRule);
                        _Dataset.Assign(SMDefaults.RulesTableName_UidRule, UidRule);
                        _Dataset.Assign(SMDefaults.RulesTableName_Text, Text);  
                        _Dataset.Assign(SMDefaults.RulesTableName_Icon, Icon);
                        _Dataset.Assign(SMDefaults.RulesTableName_Image, Image);
                        _Dataset.Assign(SMDefaults.RulesTableName_Parameters, Parameters.Parameters);
                        _Dataset.Assign(SMDefaults.RulesTableName_ByDefault, ByDefault);
                        return 1;
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

        #endregion

        /* */

    }

    /* */

}