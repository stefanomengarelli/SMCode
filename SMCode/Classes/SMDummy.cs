/*  ===========================================================================
 *  
 *  File:       SMDummy.cs
 *  Version:    2.0.0
 *  Date:       May 2024
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2024 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *
 *  SMCode dummy class.
 *
 *  ===========================================================================
 */

namespace SMCodeSystem
{

    /* */

    /// <summary>SMCode dummy class.</summary>
    public class SMDummy
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

        /// <summary>Get or set property.</summary>
        public string Property { get; set; }

       #endregion

        /* */

        #region Initialization

        /*  ===================================================================
         *  Initialization
         *  ===================================================================
         */

        /// <summary>Class constructor.</summary>
        public SMDummy()
        {
            Clear();
        }

        /// <summary>Class constructor.</summary>
        public SMDummy(SMDummy _OtherInstance)
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
        public void Assign(SMDummy _OtherInstance)
        {
            Property = _OtherInstance.Property;
        }

        /// <summary>Clear item.</summary>
        public void Clear()
        {
            Property = "";
        }

        #endregion

        /* */

    }

    /* */

}
