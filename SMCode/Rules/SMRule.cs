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
        public string Id { get; set; }

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
            SM = SMCode.CurrentOrNew(_SM);
            InitializeInstance();
            Assign(_OtherInstance);
        }

        /// <summary>Class constructor.</summary>
        public SMRule(string _Id, string _Description, string _Icon = "", string _Image = "", bool _Default = false, string _Uid = "", SMCode _SM = null)
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

        #endregion

        /* */

        #region Methods

        /*  ===================================================================
         *  Methods
         *  ===================================================================
         */

        /// <summary>Initialize instance.</summary>
        private void InitializeInstance(SMCode _SM = null)
        {
            Parameters = new SMDictionary();
            Clear();
        }

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
            Id = "";
            Uid = "";
            Description = "";
            Icon = "";
            Image = "";
            Default = false;
            Parameters.Clear();
        }

        /// <summary>Read item from current record of dataset. Return 1 if success, 0 if fail or -1 if error.</summary>
        public int Read(SMDataset _Dataset, string _IdColumn = null, string _ImageColumn = null, string _DescriptionColumn = null,
            string _IconColumn = null, string _DefaultColumn = null, string _UidColumn = null, string _ParametersColumn = null)
        {
            try
            {
                if (_Dataset != null)
                {
                    if (!_Dataset.Eof)
                    {
                        Clear();
                        //
                        if (SM.Empty(_IdColumn)) _IdColumn = SMRules.IdColumn;
                        if (SM.Empty(_UidColumn)) _UidColumn = SMRules.UidColumn;
                        if (SM.Empty(_DescriptionColumn)) _DescriptionColumn = SMRules.DescriptionColumn;
                        if (SM.Empty(_IconColumn)) _IconColumn = SMRules.IconColumn;
                        if (SM.Empty(_ImageColumn)) _ImageColumn = SMRules.ImageColumn;
                        if (SM.Empty(_DefaultColumn)) _DefaultColumn = SMRules.DefaultColumn;
                        if (SM.Empty(_ParametersColumn)) _ParametersColumn = SMRules.ParametersColumn;
                        //
                        if (!SM.Empty(_IdColumn)) Id = _Dataset.FieldStr(_IdColumn);
                        if (!SM.Empty(_UidColumn)) Uid = _Dataset.FieldStr(_UidColumn);
                        if (!SM.Empty(_DescriptionColumn)) Description = _Dataset.FieldStr(_DescriptionColumn);
                        if (!SM.Empty(_IconColumn)) Icon = _Dataset.FieldStr(_IconColumn);
                        if (!SM.Empty(_ImageColumn)) Image = _Dataset.FieldStr(_ImageColumn);
                        if (!SM.Empty(_DefaultColumn)) Default = _Dataset.FieldBool(_DefaultColumn);
                        if (!SM.Empty(_ParametersColumn)) Parameters.FromParameters(_Dataset.FieldStr(_ParametersColumn));
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