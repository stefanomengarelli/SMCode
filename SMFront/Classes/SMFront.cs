/*  ===========================================================================
 *  
 *  File:       SMFront.cs
 *  Version:    2.0.34
 *  Date:       July 2024
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2024 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *
 *  SMFront class.
 *  
 *  ===========================================================================
 */

using SMCodeSystem;

namespace SMFrontSystem
{

    /* */

    /// <summary>SMFront class: initialization.</summary>
    public partial class SMFront:SMCode
    {

        /* */

        #region Properties

        /*  ===================================================================
         *  Properties
         *  ===================================================================
         */

        /// <summary>Get or set last controls attribute prefix.</summary>
        public string AttributePrefix { get; set; } = "sm-";

        #endregion

        /* */

        #region Initialization

        /*  ===================================================================
         *  Initialization
         *  ===================================================================
         */

        /// <summary>Initialize instance.</summary>
        public SMFront(string[] _Arguments = null, string _OEM = "", string _InternalPassword = "") : base(_Arguments, _OEM, _InternalPassword)
        {
            InitializeInstance();
        }

        #endregion

        /* */

        #region Methods

        /*  ===================================================================
         *  Methods
         *  ===================================================================
         */

        /// <summary>Initialize control instance.</summary>
        private void InitializeInstance()
        {
            //
        }

        #endregion

        /* */

    }

    /* */

}
