/*  ===========================================================================
 *  
 *  File:       SMInjectionMethod.cs
 *  Version:    2.0.280
 *  Date:       July 2025
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2025 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *
 *  Injection method class.
 *
 *  ===========================================================================
 */

using System.Reflection;

namespace SMCodeSystem
{

    /* */

    /// <summary>Injection method class.</summary>
    public class SMInjectionMethod
    {

        /* */

        #region Properties

        /*  ===================================================================
         *  Properties
         *  ===================================================================
         */

        /// <summary>Get or set class instance object.</summary>
        public object Instance { get; set; }

        /// <summary>Get or set string value.</summary>
        public MethodInfo MethodInfo { get; set; }

        #endregion

        /* */

        #region Initialization

        /*  ===================================================================
         *  Initialization
         *  ===================================================================
         */

        /// <summary>Instance constructor.</summary>
        public SMInjectionMethod(object _Instance, MethodInfo _MethodInfo)
        {
            Instance = _Instance;
            MethodInfo = _MethodInfo;
        }

        #endregion

        /* */

    }

    /* */

}
