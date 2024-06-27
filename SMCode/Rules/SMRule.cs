/*  ===========================================================================
 *  
 *  File:       SMRule.cs
 *  Version:    2.0.30
 *  Date:       May 2024
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2024 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *
 *  SMCode rule class.
 *
 *  ===========================================================================
 */

using System;

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
        public string Id { get; set; }

        /// <summary>Get or set rule description.</summary>
        public string Description { get; set; }

        /// <summary>Get or set rule icon.</summary>
        public string Icon { get; set; }

        /// <summary>Return true if rule is empty.</summary>
        public bool Empty { get { return SM.Empty(Id) || SM.Empty(Description); } }

        /// <summary>Get or set default rule flag.</summary>
        public bool Default { get; set; }

        /// <summary>Get or set rule UID.</summary>
        public string Uid { get; set; }

        /// <summary>Get or set rule parameters.</summary>
        public SMDictionary Parameters { get; private set; } = new SMDictionary();

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
            if (_SM == null) SM = SMCode.CurrentOrNew();
            else SM = _SM;
            Clear();
        }

        /// <summary>Class constructor.</summary>
        public SMRule(SMRule _OtherInstance, SMCode _SM = null)
        {
            if (_SM == null) SM = SMCode.CurrentOrNew();
            else SM = _SM;
            Assign(_OtherInstance);
        }

        /// <summary>Class constructor.</summary>
        public SMRule(string _Id, string _Description, string _Icon="", bool _Default = false, string _Uid = "", SMCode _SM = null)
        {
            if (_SM == null) SM = SMCode.CurrentOrNew();
            else SM = _SM;
            Clear();
            Id = _Id;
            Description = _Description;
            Icon = _Icon;
            Default = _Default;
            Uid = _Uid; 
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
            Id = _OtherInstance.Id;
            Description = _OtherInstance.Description;
            Icon = _OtherInstance.Icon;
            Default = _OtherInstance.Default;
            Uid = _OtherInstance.Uid;
            Parameters.Assign(_OtherInstance.Parameters);
        }

        /// <summary>Clear item.</summary>
        public void Clear()
        {
            Id = "";
            Description = "";
            Icon = "";
            Default = false;
            Uid = "";
            Parameters.Clear();
        }

        /// <summary>Read item from current record of dataset.</summary>
        public bool Read(SMDataset _Dataset, string _IdColumn = "Id", string _DescriptionColumn = "Description",
            string _IconColumn = "Icon", string _DefaultColumn = "Default", string _UidColumn = "Uid", 
            string _ParametersColumn = "Parameters")
        {
            int i;
            string c;
            try
            {
                if (_Dataset != null)
                {
                    if (!_Dataset.Eof)
                    {
                        Clear();
                        if (!SM.Empty(_IdColumn)) Id = _Dataset.FieldStr(_IdColumn);
                        if (!SM.Empty(_DescriptionColumn)) Description = _Dataset.FieldStr(_DescriptionColumn);
                        if (!SM.Empty(_IconColumn)) Icon = _Dataset.FieldStr(_IconColumn);
                        if (!SM.Empty(_DefaultColumn)) Default = _Dataset.FieldBool(_DefaultColumn);
                        if (!SM.Empty(_UidColumn)) Uid = _Dataset.FieldStr(_UidColumn);
                        if (!SM.Empty(_ParametersColumn)) Parameters.FromParameters(_Dataset.FieldStr(_ParametersColumn));
                        return true;
                    }
                    else return false;
                }
                else return false;
            }
            catch (Exception ex)
            {
                SM.Error(ex);
                return false;
            }
        }

        #endregion

        /* */

    }

    /* */

}