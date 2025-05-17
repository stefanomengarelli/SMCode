/*  ===========================================================================
 *  
 *  File:       SMOrganization.cs
 *  Version:    2.0.252
 *  Date:       May 2025
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2025 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *
 *  SMCode organization class.
 *
 *  ===========================================================================
 */

using System;
using System.Text.Json;

namespace SMCodeSystem
{

    /* */

    /// <summary>SMCode organization class.</summary>
    public class SMOrganization
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

        /// <summary>Get or set organization id.</summary>
        public int IdOrganization { get; set; }

        /// <summary>Get or set organization UID.</summary>
        public string UidOrganization { get; set; }

        /// <summary>Get or set organization description.</summary>
        public string Text { get; set; }

        /// <summary>Get or set organization icon path.</summary>
        public string Icon { get; set; }

        /// <summary>Get or set organization image path.</summary>
        public string Image { get; set; }

        /// <summary>Get or set by-default organization flag.</summary>
        public bool ByDefault { get; set; }

        /// <summary>Get or set organization parameters.</summary>
        public SMDictionary Parameters { get; private set; }

        /// <summary>Return true if organization is empty.</summary>
        public bool Empty { get { return (IdOrganization < 1) || SM.Empty(Text); } }

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
        public SMOrganization(SMCode _SM = null)
        {
            SM = SMCode.CurrentOrNew(_SM);
            Initialize();
        }

        /// <summary>Class constructor.</summary>
        public SMOrganization(SMOrganization _OtherInstance, SMCode _SM = null)
        {
            if (_SM == null) _SM = _OtherInstance.SM;
            SM = SMCode.CurrentOrNew(_SM);
            Initialize();
            Assign(_OtherInstance);
        }

        /// <summary>Initialize instance.</summary>
        private void Initialize()
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
        public void Assign(SMOrganization _OtherInstance)
        {
            IdOrganization = _OtherInstance.IdOrganization;
            UidOrganization = _OtherInstance.UidOrganization;
            Text = _OtherInstance.Text;
            Icon = _OtherInstance.Icon;
            Image = _OtherInstance.Image;
            ByDefault = _OtherInstance.ByDefault;
            Parameters.Assign(_OtherInstance.Parameters);
            Tag = _OtherInstance.Tag;
        }

        /// <summary>Clear item.</summary>
        public void Clear()
        {
            IdOrganization = 0;
            UidOrganization = "";
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
                Assign((SMOrganization)JsonSerializer.Deserialize(_JSON, null));
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

        /// <summary>Load organization information by id.
        /// Return 1 if success, 0 if fail or -1 if error.</summary>
        public int Load(int _IdOrganization)
        {
            string sql = $"SELECT * FROM {SMDefaults.OrganizationsTableName}"
                + $" WHERE ({SMDefaults.OrganizationsTableName_IdOrganization}={_IdOrganization})"
                + $" AND{SM.SqlNotDeleted(SMDefaults.OrganizationsTableName_Deleted)}";
            return LoadSQL(sql);
        }

        /// <summary>Load organization information by id.
        /// Return 1 if success, 0 if fail or -1 if error.</summary>
        public int Load(string _UidOrganization)
        {
            string sql = $"SELECT * FROM {SMDefaults.OrganizationsTableName}"
                + $" WHERE ({SMDefaults.OrganizationsTableName_UidOrganization}={SM.Quote(_UidOrganization)})"
                + $" AND{SM.SqlNotDeleted(SMDefaults.OrganizationsTableName_Deleted)}";
            return LoadSQL(sql);
        }

        /// <summary>Load organization information by sql query 
        /// Return 1 if success, 0 if fail or -1 if error.</summary>
        public int LoadSQL(string _Sql)
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
                        IdOrganization = _Dataset.FieldInt(SMDefaults.OrganizationsTableName_IdOrganization);
                        UidOrganization = _Dataset.FieldStr(SMDefaults.OrganizationsTableName_UidOrganization);
                        Text = _Dataset.FieldStr(SMDefaults.OrganizationsTableName_Text);
                        Icon = _Dataset.FieldStr(SMDefaults.OrganizationsTableName_Icon);
                        Image = _Dataset.FieldStr(SMDefaults.OrganizationsTableName_Image);
                        Parameters.FromParameters(SM.ToStr(_Dataset.FieldStr(SMDefaults.OrganizationsTableName_Parameters)));
                        ByDefault = _Dataset.FieldBool(SMDefaults.OrganizationsTableName_ByDefault);
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

        #endregion

        /// <summary>Write current record of dataset with item. Return 1 if success, 0 if fail or -1 if error.</summary>
        public int Write(SMDataset _Dataset)
        {
            try
            {
                if (_Dataset != null)
                {
                    if (!_Dataset.Eof)
                    {
                        _Dataset.Assign(SMDefaults.OrganizationsTableName_IdOrganization, IdOrganization);
                        _Dataset.Assign(SMDefaults.OrganizationsTableName_UidOrganization, UidOrganization);
                        _Dataset.Assign(SMDefaults.OrganizationsTableName_Text, Text);
                        _Dataset.Assign(SMDefaults.OrganizationsTableName_Icon, Icon);
                        _Dataset.Assign(SMDefaults.OrganizationsTableName_Image, Image);
                        _Dataset.Assign(SMDefaults.OrganizationsTableName_Parameters, Parameters.Parameters);
                        _Dataset.Assign(SMDefaults.OrganizationsTableName_ByDefault, ByDefault);
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

        /* */

    }

    /* */

}