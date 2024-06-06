/*  ===========================================================================
 *  
 *  File:       SMFrontPage.cs
 *  Version:    2.0.0
 *  Date:       March 2024
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2024 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *
 *  SMFront web page base class.
 *
 *  ===========================================================================
 */

using SMCode;

namespace SMFront
{

    /* */

    /// <summary>SMFront web page base class.</summary>
    public partial class SMFrontPage
    {

        /* */

        #region Declarations

        /*  ===================================================================
         *  Declarations
         *  ===================================================================
         */

        /// <summary>Max controls number.</summary>
        public const int MAX_CONTROLS = 2048;

        /// <summary>SM session instance.</summary>
        private readonly SMApplication SM = null;

        #endregion

        /* */

        #region Delegates

        /*  ===================================================================
         *  Delegates
         *  ===================================================================
         */

        #endregion

        /* */

        #region Properties

        /*  ===================================================================
         *  Properties
         *  ===================================================================
         */

        /// <summary>Get page arguments.</summary>
        public SMDictionary Arguments { get; private set; } = null;

        /// <summary>Get controls collection.</summary>
        public SMFrontControls Controls { get; private set; } = new SMFrontControls();

        /// <summary>Get or set control id.</summary>
        public int Id { get; set; } = 0;

        /// <summary>Get or set control text.</summary>
        public string Text { get; set; } = "";

        #endregion

        /* */

        #region Initialization

        /*  ===================================================================
         *  Initialization
         *  ===================================================================
         */

        /// <summary>Class constructor.</summary>
        public SMFrontPage(SMApplication _SMApplication = null)
        {
            if (_SMApplication == null) SM = SMApplication.CurrentOrNew();
            else SM = _SMApplication;
            InitializeControl();
        }

        #endregion

        /* */

        #region Methods

        /*  ===================================================================
         *  Methods
         *  ===================================================================
         */

        /// <summary>Initialize control.</summary>
        private void InitializeControl()
        {
            Controls.Parent = this;
        }

        #endregion

        /* */

    }

    /* */

}
