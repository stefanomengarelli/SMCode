/*  ===========================================================================
 *  
 *  File:       Properties.cs
 *  Version:    2.4.2
 *  Date:       October 2026
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2026 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *
 *  SMCode application class: properties.
 *  
 *  ===========================================================================
 */

using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;

namespace SMCodeSystem
{

    /* */

    /// <summary>SM application class.</summary>
    public partial class SMCode
    {

        /* */

        #region Properties

        /*  ===================================================================
         *  Properties
         *  ===================================================================
         */

        /// <summary>Get or set last application instance created.</summary>
        public static SMCode SM { get; private set; } = null;

        /// <summary>Get or set application passed arguments array (parameters).</summary>
        public string[] Arguments { get; set; } = null;

        /// <summary>Get application passed arguments array (parameters) count.</summary>
        public int ArgumentsCount
        {
            get
            {
                if (Arguments == null) return 0;
                else return Arguments.Length;
            }
        }

        /// <summary>Get application assemblies dictionary.</summary>
        public SMDictionary Assemblies { get; private set; } = null;

        /// <summary>Get or set client mode.</summary>
        public virtual bool ClientMode { get; set; } = false;

        /// <summary>Application current culture.</summary>
        private CultureInfo Culture { get; set; } = null;

        /// <summary>Database connections collection.</summary>
        public SMDatabases Databases { get; private set; } = null;

        /// <summary>Get or set demonstration mode flag.</summary>
        public virtual bool Demo { get; set; } = false;

        /// <summary>Get or set deploy environment.</summary>
        public string Deploy
        {
            get { return deploy; }
            set
            {
                deploy = ToStr(value).Trim().ToLower();
                if (deploy.StartsWith("dev")) deploy = "Development";
                else if (deploy.StartsWith("prod")) deploy = "Production";
                else if (Debugger.IsAttached) deploy = "Development";
                else deploy = "Production";
            }
        }

        /// <summary>Get or set main INI configuration write defaults flag.</summary>
        public virtual bool IniDefaults { get; set; } = true;

        /// <summary>Get or set main INI settings configuration flag.</summary>
        public virtual bool IniSettings { get; set; } = true;

        /// <summary>SMCode core class initialized flag.</summary>
        public bool Initialized { get; private set; } = false;

        /// <summary>SMCode core class initializing flag.</summary>
        public bool Initializing { get; private set; } = false;

        /// <summary>Get injections collection.</summary>
        public SMInjections Injections { get; private set; } = null;

        /// <summary>Generic internal password.</summary>
        public virtual string InternalPassword { get; set; } = "";

        /// <summary>Get or set application selected language.</summary>
        public string Language
        {
            get { return language; }
            set { InitializeLanguage(value); }
        }

        /// <summary>Main database alias (default: MAIN).</summary>
        public virtual string MainAlias { get; set; } = "MAIN";

        /// <summary>Application configuration parameters.</summary>
        public SMDictionary Parameters { get; private set; } = null;

        /// <summary>Application resources manager.</summary>
        public SMResources Resources { get; private set; } = null;

        /// <summary>OEM id.</summary>
        public virtual string OEM { get; set; } = "";

        /// <summary>.NET environment.</summary>
        public SMEnvironment Environment { get; private set; } = SMEnvironment.Unknown;

        /// <summary>Session UID.</summary>
        public virtual string SessionUID { get; set; } = "";

        /// <summary>Get static settings strings dictionary.</summary>   
        public static Dictionary<string, object> StaticSettings { get; private set; } = new Dictionary<string, object>();

        /// <summary>Get or set instance tag object.</summary>
        public object Tag { get; set; } = null;

        /// <summary>Get or set test mode flag.</summary>
        public virtual bool Test { get; set; } = false;

        /// <summary>Current user.</summary>
        public SMUser User { get; private set; } = null;

        /// <summary>Get or set user extend table.</summary>
        public virtual string UserExtendTable { get; set; } = null;

        /// <summary>Get or set user extend table used id field name (default: IdUser).</summary>
        public virtual string UserExtendTableIdUserFieldName{ get; set; } = "IdUser";

        /// <summary>Get or set initialization wipe temporary file flag.</summary>
        public virtual bool WipeTemporaryFiles { get; set; } = true;

        #endregion

        /* */

    }

    /* */

}
