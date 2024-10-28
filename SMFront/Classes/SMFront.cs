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
using System;

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

        /// <summary>Get or set last application instance created.</summary>
        public static new SMFront SM { get; set; } = null;

        /// <summary>Get or set controls attribute prefix.</summary>
        public string AttributePrefix { get; set; } = "sm-";

        /// <summary>Application assembly base path.</summary>
        public static string BasePath { get; set; } = "";

        /// <summary>Get or set classes attribute prefix.</summary>
        public string ClassPrefix { get; set; } = "sm-";

        /// <summary>Application configuration (appsettings.json).</summary>
        public SMJson Configuration { get; private set; } = null;

        /// <summary>Application root path on server.</summary>
        public static string RootPath { get; set; } = "";

        #endregion

        /* */

        #region Initialization

        /*  ===================================================================
         *  Initialization
         *  ===================================================================
         */

        /// <summary>Initialize instance.</summary>
        public SMFront(string[] _Arguments = null, string _OEM = "", string _InternalPassword = "", string _ApplicationPath = "") : base(_Arguments, _OEM, _InternalPassword, _ApplicationPath)
        {
            SM = this;
            if (!SM.Empty(_ApplicationPath)) ApplicationPath = _ApplicationPath;
            else if (!SM.Empty(RootPath)) ApplicationPath = RootPath;
            InitializeInstance();
        }

        /// <summary>Initialize control instance.</summary>
        private void InitializeInstance()
        {
            BasePath = AppDomain.CurrentDomain.BaseDirectory;
            Configuration = new SMJson(OnBasePath("appsettings.json"), this);
        }

        #endregion

        /* */

        #region Methods

        /*  ===================================================================
         *  Methods
         *  ===================================================================
         */

        /// <summary>Return full path of file name, on assembly base path.</summary>
        public string OnBasePath(string _FileName = "")
        {
            return Combine(BasePath, _FileName);
        }

        /// <summary>Return full path of file name, on application root path.</summary>
        public string OnRootPath(string _FileName = "")
        {
            return Combine(RootPath, _FileName);
        }

        #endregion

        /* */

        #region Static Methods

        /*  ===================================================================
         *  Static Methods
         *  ===================================================================
         */

        /// <summary>Return current instance of SMApplication or new if not found.</summary>
        public static SMFront CurrentOrNew(SMFront _SM = null, string[] _Arguments = null, string _OEM = "", string _InternalPassword = "")
        {
            if (_SM != null) SM = _SM;
            else if (SM == null) SM = new SMFront(_Arguments, _OEM, _InternalPassword);
            return SM;
        }

        #endregion

        /* */

    }

    /* */

}
