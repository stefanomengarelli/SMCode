/*  ===========================================================================
 *  
 *  File:       Initialize.cs
 *  Version:    2.0.30
 *  Date:       June 2024
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2024 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *
 *  SMDesk class: initialization.
 *  
 *  ===========================================================================
 */

using SMCodeSystem;

namespace SMDeskSystem
{

    /* */

    /// <summary>SMDesk class: initialization.</summary>
    public partial class SMDesk:SMCode
    {

        /* */

        #region Properties

        /*  ===================================================================
         *  Properties
         *  ===================================================================
         */

        #endregion

        /* */

        #region Initialization

        /*  ===================================================================
         *  Initialization
         *  ===================================================================
         */

        /// <summary>Initialize instance.</summary>
        public SMDesk(string[] _Arguments = null, string _OEM = "", string _InternalPassword = "", string _ApplicationPath = "") : base(_Arguments, _OEM, _InternalPassword, _ApplicationPath)
        {
            //            
        }

        #endregion

        /* */

        #region Methods

        /*  ===================================================================
         *  Methods
         *  ===================================================================
         */

        #endregion

        /* */

    }

    /* */

}
