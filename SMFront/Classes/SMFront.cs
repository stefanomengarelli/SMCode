/*  ===========================================================================
 *  
 *  File:       SMFront.cs
 *  Version:    2.0.30
 *  Date:       June 2024
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
using static SMFrontSystem.SMFrontControl;

namespace SMFrontSystem
{

    /* */

    /// <summary>SMFront class: initialization.</summary>
    public partial class SMFront
    {

        /* */

        #region Properties

        /*  ===================================================================
         *  Properties
         *  ===================================================================
         */

        /// <summary>Get or set last application instance created.</summary>
        public SMCode SM { get; set; } = null;

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
        public SMFront(SMCode _SM)
        {
            if (_SM == null) SM = SMCode.CurrentOrNew();
            else SM = _SM;
            InitializeInstance();
        }

        /// <summary>Initialize instance.</summary>
        public SMFront(string[] _Arguments = null, string _OEM = "", string _InternalPassword = "")
        {
            SM = new SMCode(_Arguments, _OEM, _InternalPassword);
            InitializeInstance();
        }

        /// <summary>Initialize instance.</summary>
        public SMFront(out SMCode _SM, string[] _Arguments = null, string _OEM = "", string _InternalPassword = "")
        {
            SM = new SMCode(_Arguments, _OEM, _InternalPassword);
            InitializeInstance();
            _SM = SM;
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
