/*  ===========================================================================
 *  
 *  File:       SMFrontValue.cs
 *  Version:    2.0.82
 *  Date:       November 2024
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2024 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *
 *  SMFront control value management class.
 *
 *  ===========================================================================
 */

using System;

namespace SMFrontSystem
{

    /* */

    /// <summary>SMFront control value management class.</summary>
    public class SMFrontValue
	{

        /* */

        #region Declarations

        /*  ===================================================================
         *  Declarations
         *  ===================================================================
         */

		#endregion

		/* */

		#region Properties

		/*  ===================================================================
         *  Properties
         *  ===================================================================
         */

		/// <summary>Get or set blob value.</summary>
		public byte[] Blob
		{
			get
			{
				if (Data == null) return null;
				else if (Data == DBNull.Value) return null;
				else if (Data is byte[]) return (byte[])Data;
				else return null;
			}
			set { Data = value; }
		}

		/// <summary>Get or set value changed flag.</summary>
		public bool Changed { get; set; }

		/// <summary>Get or set object data.</summary>
		public object Data { get; set; } = null;

        /// <summary>Return true if value has data.</summary>
        public bool HasData 
        { 
            get 
            { 
                return (Data != null) && (Data != DBNull.Value); 
            } 
        }

		/// <summary>Get or set string value.</summary>
		public string Value
		{
			get
			{
                if (Data == null) return "";
                else if (Data == DBNull.Value) return "";
                else return Data.ToString();
			}
			set { Data = value; }
		}

		#endregion

		/* */

		#region Initialization

		/*  ===================================================================
         *  Initialization
         *  ===================================================================
         */

		/// <summary>Class constructor.</summary>
		public SMFrontValue()
        {
            InitializeInstance();
        }

        /// <summary>Class constructor.</summary>
        public SMFrontValue(SMFrontValue _Value)
        {
            InitializeInstance();
            Assign(_Value);
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
        public void Assign(SMFrontValue _Value)
        {
            Changed = _Value.Changed;
            Data = _Value.Data;
        }

        /// <summary>Clear item.</summary>
        public void Clear()
        {
			Changed = false;
			Data = null;
        }

        #endregion

        /* */

    }

    /* */

}
