/*  ===========================================================================
 *  
 *  File:       SMFrontFormEvents.cs
 *  Version:    2.0.95
 *  Date:       December 2024
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2024 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *
 *  SMFront web form events class.
 *
 *  ===========================================================================
 */

using SMCodeSystem;
using System;

namespace SMFrontSystem
{

    /* */

    /// <summary>SMFront web form events class.</summary>
    public class SMFrontFormEvents
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

        /// <summary>Get or set on cancel event.</summary>
        public string OnCancel { get; set; } = "";

        /// <summary>Get or set on delete event.</summary>
        public string OnDelete { get; set; } = "";

		/// <summary>Get or set on enable event.</summary>
		public string OnEdit { get; set; } = "";

		/// <summary>Get or set on focus event.</summary>
		public string OnInsert { get; set; } = "";

		/// <summary>Get or set on initialize event.</summary>
		public string OnPost { get; set; } = "";

		/// <summary>Get or set on update event.</summary>
		public string OnReadOnly { get; set; } = "";

		/// <summary>Get or set on validate event.</summary>
		public string OnValidate { get; set; } = "";

		#endregion

		/* */

		#region Initialization

		/*  ===================================================================
         *  Initialization
         *  ===================================================================
         */

		/// <summary>Class constructor.</summary>
		public SMFrontFormEvents(SMFront _SM = null)
        {
			SM = SMFront.CurrentOrNew(_SM);
            InitializeInstance();
        }

        /// <summary>Class constructor.</summary>
        public SMFrontFormEvents(SMFrontFormEvents _Events, SMFront _SM = null)
        {
			if (_SM == null) _SM = _Events.SM;
            SM = SMFront.CurrentOrNew(_SM);
            InitializeInstance();
            Assign(_Events);
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
        public void Assign(SMFrontFormEvents _Events)
        {
            OnCancel = _Events.OnCancel;
            OnDelete = _Events.OnDelete;
            OnEdit = _Events.OnEdit;
            OnInsert = _Events.OnInsert;
            OnPost = _Events.OnPost;
            OnReadOnly = _Events.OnReadOnly;
            OnValidate = _Events.OnValidate;
        }

        /// <summary>Clear item.</summary>
        public void Clear()
        {
            OnCancel = "";
            OnDelete = "";
            OnEdit = "";
            OnInsert = "";
            OnPost = "";
            OnReadOnly = "";
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
                    _Prefix = _Prefix.Trim();
                    OnCancel = _Dataset.FieldStr(_Prefix + "OnCancel");
                    OnDelete = _Dataset.FieldStr(_Prefix + "OnDelete");
                    OnEdit = _Dataset.FieldStr(_Prefix + "OnEdit");
                    OnInsert = _Dataset.FieldStr(_Prefix + "OnInsert");
                    OnPost = _Dataset.FieldStr(_Prefix + "OnPost");
                    OnReadOnly = _Dataset.FieldStr(_Prefix + "OnReadOnly");
                    OnValidate = _Dataset.FieldStr(_Prefix + "OnValidate");
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

        /// <summary>Read control data from current record on dataset.</summary>
        public bool Write(SMDataset _Dataset, string _Prefix = "")
        {
            try
            {
                if (_Dataset != null)
                {
                    _Prefix = _Prefix.Trim();
                    _Dataset.Assign(_Prefix + "OnCancel", OnCancel);
                    _Dataset.Assign(_Prefix + "OnDelete", OnDelete);
                    _Dataset.Assign(_Prefix + "OnEdit", OnEdit);
                    _Dataset.Assign(_Prefix + "OnInsert", OnInsert);
                    _Dataset.Assign(_Prefix + "OnPost", OnPost);
                    _Dataset.Assign(_Prefix + "OnReadOnly", OnReadOnly);
                    _Dataset.Assign(_Prefix + "OnValidate", OnValidate);
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
