/*  ===========================================================================
 *  
 *  File:       SMFrontControlEvents.cs
 *  Version:    2.0.95
 *  Date:       December 2024
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2024 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *
 *  SMFront web control events class.
 *
 *  ===========================================================================
 */

using System;
using SMCodeSystem;

namespace SMFrontSystem
{

    /* */

    /// <summary>SMFront web control events class.</summary>
    public class SMFrontControlEvents
	{

		/* */

		#region Declarations

		/*  ===================================================================
         *  Declarations
         *  ===================================================================
         */

		/// <summary>SM session instance.</summary>
		private readonly SMFront SM = null;

		#endregion

		/* */

		#region Properties

		/*  ===================================================================
         *  Properties
         *  ===================================================================
         */

		/// <summary>Get or set on change event evaluate javascript.</summary>
		public string OnChange { get; set; } = "";

		/// <summary>Get or set on enable event evaluate javascript.</summary>
		public string OnEnable { get; set; } = "";

		/// <summary>Get or set on focus event evaluate javascript.</summary>
		public string OnFocus { get; set; } = "";

		/// <summary>Get or set on initialize event evaluate javascript.</summary>
		public string OnInitialize { get; set; } = "";

		/// <summary>Get or set on update event evaluate javascript.</summary>
		public string OnUpdate { get; set; } = "";

		/// <summary>Get or set on validate event evaluate javascript.</summary>
		public string OnValidate { get; set; } = "";

		/// <summary>Get or set on visible event evaluate javascript.</summary>
		public string OnVisible { get; set; } = "";

		#endregion

		/* */

		#region Initialization

		/*  ===================================================================
         *  Initialization
         *  ===================================================================
         */

		/// <summary>Class constructor.</summary>
		public SMFrontControlEvents(SMFront _SM = null)
        {
			SM = SMFront.CurrentOrNew(_SM);
            InitializeInstance();
        }

        /// <summary>Class constructor.</summary>
        public SMFrontControlEvents(SMFrontControlEvents _OtherInstance, SMFront _SM = null)
        {
			if (_SM == null) _SM = _OtherInstance.SM;
            SM = SMFront.CurrentOrNew(_SM);
            InitializeInstance();
            Assign(_OtherInstance);
        }

        /// <summary>Initialize instance.</summary>
        public void InitializeInstance()
        {
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
        public void Assign(SMFrontControlEvents _OtherInstance)
        {
            OnChange = _OtherInstance.OnChange;
            OnEnable = _OtherInstance.OnEnable;
            OnFocus = _OtherInstance.OnFocus;
            OnInitialize = _OtherInstance.OnInitialize;
            OnUpdate = _OtherInstance.OnUpdate;
            OnValidate = _OtherInstance.OnValidate;
            OnVisible = _OtherInstance.OnVisible;
        }

        /// <summary>Clear item.</summary>
        public void Clear()
        {
            OnChange = "";
            OnEnable = "";
            OnFocus = "";
            OnInitialize = "";
            OnUpdate = "";
            OnValidate = "";
            OnValidate = "";
        }

        /// <summary>Read control data from current record on dataset.</summary>
        public bool Read(SMDataset _Dataset, string _Prefix = "")
        {
            try
            {
                Clear();
                if (_Dataset != null)
                {
                    OnChange = _Dataset.FieldStr(_Prefix + "OnChange");
                    OnEnable = _Dataset.FieldStr(_Prefix + "OnEnable");
                    OnFocus = _Dataset.FieldStr(_Prefix + "OnFocus");
                    OnInitialize = _Dataset.FieldStr(_Prefix + "OnInitialize");
                    OnUpdate = _Dataset.FieldStr(_Prefix + "OnUpdate");
                    OnValidate = _Dataset.FieldStr(_Prefix + "OnValidate");
                    OnVisible = _Dataset.FieldStr(_Prefix + "OnVisible");
                    return true;
                }
                else return false;
            }
            catch (Exception ex)
            {
                SM.Error(ex);
                return false;
            }
        }

        /// <summary>Write control data on current record on dataset.</summary>
        public bool Write(SMDataset _Dataset, string _Prefix = "")
        {
            try
            {
                if (_Dataset != null)
                {
                    _Dataset.Assign(_Prefix + "OnChange", OnChange);
                    _Dataset.Assign(_Prefix + "OnEnable", OnEnable);
                    _Dataset.Assign(_Prefix + "OnFocus", OnFocus);
                    _Dataset.Assign(_Prefix + "OnInitialize", OnInitialize);
                    _Dataset.Assign(_Prefix + "OnUpdate", OnUpdate);
                    _Dataset.Assign(_Prefix + "OnValidate", OnValidate);
                    _Dataset.Assign(_Prefix + "OnVisible", OnVisible);
                    return true;
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
