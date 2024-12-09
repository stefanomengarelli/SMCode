/*  ===========================================================================
 *  
 *  File:       SMFrontRender.cs
 *  Version:    2.0.95
 *  Date:       December 2024
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2024 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *
 *  SMFront form render class.
 *
 *  ===========================================================================
 */

using SMCodeSystem;
using System.Collections.Generic;
using System.Text;

namespace SMFrontSystem
{

    /* */

    /// <summary>SMCode form render class.</summary>
    public partial class SMFrontRender
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

        /// <summary>Get or set property.</summary>
        public SMFrontForm Form { get; private set; } = null;

        /// <summary>Get render templates.</summary>
        public SMTemplates Templates { get; private set; } = null;

        #endregion

        /* */

        #region Initialization

        /*  ===================================================================
         *  Initialization
         *  ===================================================================
         */

        /// <summary>Class constructor.</summary>
        public SMFrontRender(SMFrontForm _Form, SMFront _SM = null)
        {
            SM = SMFront.CurrentOrNew(_SM);
            InitializeInstance();
            Form = _Form;
        }

        /// <summary>Initialize instance.</summary>
        public void InitializeInstance()
        {
            Templates = new SMTemplates(SM);
            Templates.Path = @"templates\render";
            Clear();
        }

        #endregion

        /* */

        #region Methods

        /*  ===================================================================
         *  Methods
         *  ===================================================================
         */

        /// <summary>Clear item.</summary>
        public void Clear()
        {
            //
        }

        #endregion

        /* */

    }

    /* */

}
