/*  ===========================================================================
 *  
 *  File:       SMRule.cs
 *  Version:    2.0.38
 *  Date:       Jul 2024
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
        public int Id { get; set; }

        /// <summary>Get or set rule UID.</summary>
        public string Uid { get; set; }

        /// <summary>Get or set rule description.</summary>
        public string Description { get; set; }

        /// <summary>Get or set rule icon path.</summary>
        public string Icon { get; set; }

        /// <summary>Get or set rule image path.</summary>
        public string Image { get; set; }

        /// <summary>Get or set default rule flag.</summary>
        public bool Default { get; set; }

        /// <summary>Get or set rule parameters.</summary>
        public SMDictionary Parameters { get; private set; }

        /// <summary>Return true if rule is empty.</summary>
        public bool Empty { get { return SM.Empty(Id) || SM.Empty(Description); } }

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

        /// <summary>Class constructor.</summary>
        public SMRule(int _Id, string _Description, string _Icon = "", string _Image = "", bool _Default = false, string _Uid = "", SMCode _SM = null)
        {
            SM = SMCode.CurrentOrNew(_SM);
            InitializeInstance();
            Id = _Id;
            Description = _Description;
            Icon = _Icon;
            Image = _Image;
            Default = _Default;
            Uid = _Uid;
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
            Id = _OtherInstance.Id;
            Uid = _OtherInstance.Uid;
            Description = _OtherInstance.Description;
            Icon = _OtherInstance.Icon;
            Image = _OtherInstance.Image;
            Default = _OtherInstance.Default;
            Parameters.Assign(_OtherInstance.Parameters);
        }

        /// <summary>Clear item.</summary>
        public void Clear()
        {
            Id = 0;
            Uid = "";
            Description = "";
            Icon = "";
            Image = "";
            Default = false;
            Parameters.Clear();
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
                        //
                        if (!SM.Empty(SMRules.IdColumn)) Id = _Dataset.FieldInt(SMRules.IdColumn);
                        if (!SM.Empty(SMRules.UidColumn)) Uid = _Dataset.FieldStr(SMRules.UidColumn);
                        if (!SM.Empty(SMRules.DescriptionColumn)) Description = _Dataset.FieldStr(SMRules.DescriptionColumn);
                        if (!SM.Empty(SMRules.IconColumn)) Icon = _Dataset.FieldStr(SMRules.IconColumn);
                        if (!SM.Empty(SMRules.ImageColumn)) Image = _Dataset.FieldStr(SMRules.ImageColumn);
                        if (!SM.Empty(SMRules.DefaultColumn)) Default = _Dataset.FieldBool(SMRules.DefaultColumn);
                        if (!SM.Empty(SMRules.ParametersColumn)) Parameters.FromParameters(_Dataset.FieldStr(SMRules.ParametersColumn));
                        //
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