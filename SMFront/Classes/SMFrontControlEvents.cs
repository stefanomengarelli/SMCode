/*  ===========================================================================
 *  
 *  File:       SMFrontControlEvents.cs
 *  Version:    2.0.0
 *  Date:       May 2024
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
using System.Data;

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
		private SMFront SM = null;

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
		public SMFrontControlEvents()
        {
            Clear();
        }

        /// <summary>Class constructor.</summary>
        public SMFrontControlEvents(SMFrontControlEvents _OtherInstance)
        {
            Assign(_OtherInstance);
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
		public bool Read(DataRow _DataRow)
		{
			try
			{
				Clear();
				if (_DataRow != null)
				{
					OnChange = SM.ToStr(_DataRow["ScriptOnChange"]);
					OnEnable = SM.ToStr(_DataRow["ScriptOnEnable"]);
					OnFocus = SM.ToStr(_DataRow["ScriptOnFocus"]);
					OnInitialize = SM.ToStr(_DataRow["ScriptOnInitialize "]);
					OnUpdate = SM.ToStr(_DataRow["ScriptOnUpdate"]);
					OnValidate = SM.ToStr(_DataRow["ScriptOnValidate"]);
					OnVisible = SM.ToStr(_DataRow["ScriptOnVisible"]);
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
