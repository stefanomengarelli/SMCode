/*  ===========================================================================
 *  
 *  File:       SMWebControl.cs
 *  Version:    2.0.0
 *  Date:       March 2024
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2024 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *
 *  SMFront web control base class.
 *
 *  ===========================================================================
 */

using System.Text;

namespace SMCode
{

    /* */

    /// <summary>SMFront web control base class.</summary>
    public class SMWebControl
    {

        /* */

        #region Declarations

        /*  ===================================================================
         *  Declarations
         *  ===================================================================
         */

        /// <summary>SM session instance.</summary>
        private readonly SMApplication SM = null;

        #endregion

        /* */

        #region Properties

        /*  ===================================================================
         *  Properties
         *  ===================================================================
         */

        /// <summary>Ger or set control order value.</summary>
        public int Order { get; set; } = 0;

        /// <summary>Ger or set parent control instance.</summary>
        public SMWebControl Parent { get; set; } = null;

        #endregion

        /* */

        #region Initialization

        /*  ===================================================================
         *  Initialization
         *  ===================================================================
         */

        /// <summary>Class constructor.</summary>
        public SMWebControl(SMApplication _SMApplication = null, SMWebControl _ParentControl = null)
        {
            if (_SMApplication == null) SM = SMApplication.CurrentOrNew();
            else SM = _SMApplication;
            Parent = _ParentControl;
        }

        #endregion

        /* */

        #region Methods

        /*  ===================================================================
         *  Methods
         *  ===================================================================
         */

        /// <summary>Return string containing control rendered HTML code.</summary>
        public string Render()
        {
            StringBuilder r = new StringBuilder();
            return r.ToString();
        }

        #endregion

        /* */

    }

    /* */

}
