/*  ===========================================================================
 *  
 *  File:       SMFrontRender.cs
 *  Version:    2.0.82
 *  Date:       November 2024
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

namespace SMFrontSystem
{

    /* */

    /// <summary>SMCode form render class.</summary>
    public class SMFrontRender
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
        public SMFrontForm Form { get; set; } = null;

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

        /// <summary>Class constructor.</summary>
        public SMFrontRender(SMFrontRender _Render, SMFront _SM = null)
        {
            if (_SM==null) _SM = _Render.SM;
            SM = SMFront.CurrentOrNew(_SM);
            InitializeInstance();
            Assign(_Render);
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
        public void Assign(SMFrontRender _Render)
        {
            Form = _Render.Form;
        }

        /// <summary>Clear item.</summary>
        public void Clear()
        {
            Form = null;
        }

        #endregion

        /* */

    }

    /* */

}
