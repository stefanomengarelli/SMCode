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

using Microsoft.AspNetCore.Http;
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

        /// <summary>Legge o imposta il contesto della chiamata HTTP.</summary>
        public HttpContext Context { get; set; } = null;

        /// <summaryGet or set current HTTP request.</summary>
        public HttpRequest Request { get; set; } = null;

        /// <summaryGet or set current HTTP response.</summary>
        public HttpResponse Response { get; set; } = null;

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
        public SMFront(HttpContext _Context, string[] _Arguments = null, string _OEM = "", string _InternalPassword = "", 
            string _ApplicationPath = "") : base(_Arguments, _OEM, _InternalPassword, _ApplicationPath)
        {
            SM = this;
            Context = _Context;
            if (!SM.Empty(_ApplicationPath)) ApplicationPath = _ApplicationPath;
            else if (!SM.Empty(RootPath)) ApplicationPath = RootPath;
            InitializeInstance();
        }

        /// <summary>Initialize control instance.</summary>
        private void InitializeInstance()
        {
            string host, database, connstr;
            SMDatabaseType dbtype;
            // set current context request and response
            if (Context != null)
            {
                Request = Context.Request;
                Response = Context.Response;
            }
            // set base path
            BasePath = AppDomain.CurrentDomain.BaseDirectory;
            // load configuration
            Configuration = new SMJson(OnBasePath("appsettings.json"), this);
            // add default main database
            host = Configuration.Get("Databases:MAIN:Host");
            database = Configuration.Get("Databases.MAIN.Database");
            connstr = Configuration.Get("Databases:MAIN:ConnectionString");
            dbtype = SMDatabase.TypeFromString(Configuration.Get("Databases:MAIN:Type"));
            SM.Databases.Add("MAIN", dbtype, host, database, connstr);
            SM.LogAlias = "MAIN";
            SM.MainAlias = "MAIN";
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
        public static SMFront CurrentOrNew(SMFront _SM = null, HttpContext _Context = null, string[] _Arguments = null, string _OEM = "", string _InternalPassword = "")
        {
            if (_SM != null) SM = _SM;
            else if (SM == null) SM = new SMFront(_Context, _Arguments, _OEM, _InternalPassword);
            return SM;
        }

        #endregion

        /* */

    }

    /* */

}
