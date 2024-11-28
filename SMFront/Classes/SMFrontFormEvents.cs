/*  ===========================================================================
 *  
 *  File:       SMFrontFormEvents.cs
 *  Version:    2.0.82
 *  Date:       November 2024
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

using System;
using System.Data;

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
        public bool Read(DataRow _DataRow, string _Prefix = "")
        {
            try
            {
                Clear();
                if (_DataRow != null)
                {
                    OnCancel = SM.ToStr(_DataRow[_Prefix + "OnCancel"]);
                    OnDelete = SM.ToStr(_DataRow[_Prefix + "OnDelete"]);
                    OnEdit = SM.ToStr(_DataRow[_Prefix + "OnEdit"]);
                    OnInsert = SM.ToStr(_DataRow[_Prefix + "OnInsert"]);
                    OnPost = SM.ToStr(_DataRow[_Prefix + "OnPost"]);
                    OnReadOnly = SM.ToStr(_DataRow[_Prefix + "OnReadOnly"]);
                    OnValidate = SM.ToStr(_DataRow[_Prefix + "OnValidate"]);
                }
                return true;
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
