/*  ===========================================================================
 *  
 *  File:       SMRule.cs
 *  Version:    2.0.50
 *  Date:       October 2024
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
        public string Caption { get; set; }

        /// <summary>Get or set rule icon path.</summary>
        public string Icon { get; set; }

        /// <summary>Get or set rule image path.</summary>
        public string Image { get; set; }

        /// <summary>Get or set by-default rule flag.</summary>
        public bool ByDefault { get; set; }

        /// <summary>Get or set rule parameters.</summary>
        public SMDictionary Parameters { get; private set; }

        /// <summary>Return true if rule is empty.</summary>
        public bool Empty { get { return (Id < 1) || SM.Empty(Caption); } }

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
        public SMRule(int _Id, string _Caption, string _Icon = "", string _Image = "", bool _Default = false, string _Uid = "", SMCode _SM = null)
        {
            SM = SMCode.CurrentOrNew(_SM);
            InitializeInstance();
            Id = _Id;
            Caption = _Caption;
            Icon = _Icon;
            Image = _Image;
            ByDefault = _Default;
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
            Caption = _OtherInstance.Caption;
            Icon = _OtherInstance.Icon;
            Image = _OtherInstance.Image;
            ByDefault = _OtherInstance.ByDefault;
            Parameters.Assign(_OtherInstance.Parameters);
        }

        /// <summary>Clear item.</summary>
        public void Clear()
        {
            Id = 0;
            Uid = "";
            Caption = "";
            Icon = "";
            Image = "";
            ByDefault = false;
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
                        Id = SM.ToInt(_Dataset["IdRule"]);
                        Uid = SM.ToStr(_Dataset["UidRule"]);
                        Caption = SM.ToStr(_Dataset["Caption"]);
                        Icon = SM.ToStr(_Dataset["Icon"]);
                        Image = SM.ToStr(_Dataset["Image"]);
                        ByDefault = SM.ToBool(_Dataset["ByDefault"]);
                        Parameters.FromParameters(SM.ToStr(_Dataset["Parameters"]));
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