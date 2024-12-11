/*  ===========================================================================
 *  
 *  File:       Macros.cs
 *  Version:    2.0.85
 *  Date:       November 2024
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2024 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *
 *  SMCode application class: macros management.
 *  
 *  ===========================================================================
 */

using System;

namespace SMCodeSystem
{

    /* */

    /// <summary>SMCode application class: macros management.</summary>
    public partial class SMCode
    {

        /* */

        #region Properties

        /*  ===================================================================
         *  Properties
         *  ===================================================================
         */

        /// <summary>Get or set macro prefix (default %%).</summary>
        public string MacroPrefix { get; set; } = "%%";

        /// <summary>Get or set macro suffix (default %%).</summary>
        public string MacroSuffix { get; set; } = "%%";

        #endregion

        /* */

        #region Methods

        /*  ===================================================================
         *  Methods
         *  ===================================================================
         */

        /// <summary>Replace value macros with current values.</summary>
        public string Macros(string _Value, string[] _Macros = null, SMDatabase _Database = null)
        {
            int i = 0;
            if (_Value == null) _Value = "";
            if (_Value.Length > (MacroPrefix + MacroSuffix).Length)
            {
                _Value = MacrosSystem(_Value);
                if (_Database != null) _Value = MacrosDatabase(_Value, _Database);
                if (_Macros != null)
                {
                    while (i < _Macros.Length - 1)
                    {
                        _Value = _Value.Replace(MacroPrefix + _Macros[i] + MacroSuffix, _Macros[i + 1]);
                        i++;
                    }
                }
            }
            return _Value;
        }

        /// <summary>Replace value index macros with related values (eg. %%0%%, %%1%%).</summary>
        public string MacrosIndexes(string _Value, string[] _Values)
        {
            int i = 0;
            if (_Value == null) _Value = "";
            if (_Values.Length > (MacroPrefix + MacroSuffix).Length)
            {
                while (i < _Values.Length)
                {
                    _Value = _Value.Replace(MacroPrefix + i.ToString() + MacroSuffix, _Values[i]);
                    i++;
                }
            }
            return _Value;
        }

        /// <summary>Replace system macros.
        /// APPLPATH   = application path
        /// COMMPATH   = common path
        /// DATAPATH   = data path
        /// DESKPATH   = user desktop path
        /// DOCSPATH   = common documents path
        /// EXECNAME   = executable name without extension
        /// EXECPATH   = executable path
        /// MACHINE    = current PC name
        /// MYDOCSPATH = user documents path
        /// TEMPPATH   = application temporary files path
        /// USER       = application logged user id
        /// SYSUSER    = system logged user id
        /// VERSION    = application version
        /// </summary>
        public string MacrosSystem(string _Value)
        {
            if (_Value == null) _Value = "";
            if (_Value.Length > (MacroPrefix + MacroSuffix).Length)
            {
                _Value = _Value.Replace(MacroPrefix + "APPLPATH" + MacroSuffix, SM.FixPath(SM.ApplicationPath));
                _Value = _Value.Replace(MacroPrefix + "COMMPATH" + MacroSuffix, SM.FixPath(SM.CommonPath));
                _Value = _Value.Replace(MacroPrefix + "DATAPATH" + MacroSuffix, SM.FixPath(SM.DataPath));
                _Value = _Value.Replace(MacroPrefix + "DESKPATH" + MacroSuffix, SM.FixPath(SM.DesktopPath));
                _Value = _Value.Replace(MacroPrefix + "DOCSPATH" + MacroSuffix, SM.FixPath(SM.DocumentsPath));
                _Value = _Value.Replace(MacroPrefix + "EXECNAME" + MacroSuffix, SM.FixPath(SM.ExecutableName));
                _Value = _Value.Replace(MacroPrefix + "EXECPATH" + MacroSuffix, SM.FixPath(SM.ExecutablePath));
                _Value = _Value.Replace(MacroPrefix + "MACHINE" + MacroSuffix, SM.Machine());
                _Value = _Value.Replace(MacroPrefix + "MYDOCSPATH" + MacroSuffix, SM.FixPath(SM.UserDocumentsPath));
                _Value = _Value.Replace(MacroPrefix + "TEMPPATH" + MacroSuffix, SM.FixPath(SM.TempPath));
                _Value = _Value.Replace(MacroPrefix + "USER" + MacroSuffix, SM.User.UserName);
                _Value = _Value.Replace(MacroPrefix + "SYSUSER" + MacroSuffix, SM.SystemUser());
                _Value = _Value.Replace(MacroPrefix + "VERSION" + MacroSuffix, SM.ExtractVersion(SM.Version, 4, -1));
            }
            return _Value;
        }

        /// <summary>Replace database macros.
        /// DBHOST     = database host
        /// DATABASE   = database name
        /// DBPATH     = data path
        /// DBUSER     = database user name
        /// DBPASSWORD = database user password
        /// MDBPATH    = access mdb database path
        /// DBTIMEOUT  = database connection timeout
        /// DBCMDTOUT  = database command timeout
        /// </summary>
        public string MacrosDatabase(string _Value, SMDatabase _Database)
        {
            if (_Value == null) _Value = "";
            if ((_Value.Length > (MacroPrefix + MacroSuffix).Length) && (_Database != null))
            {
                _Value = _Value.Replace(MacroPrefix + "DBHOST" + MacroSuffix, _Database.Host);
                _Value = _Value.Replace(MacroPrefix + "DATABASE" + MacroSuffix, _Database.Database);
                _Value = _Value.Replace(MacroPrefix + "DBPATH" + MacroSuffix, _Database.Path);
                _Value = _Value.Replace(MacroPrefix + "DBUSER" + MacroSuffix, _Database.User);
                _Value = _Value.Replace(MacroPrefix + "DBPASSWORD" + MacroSuffix, _Database.Password);
                _Value = _Value.Replace(MacroPrefix + "MDBPATH" + MacroSuffix, SM.Combine(_Database.Path, _Database.Database, SM.Iif(SM.FileExtension(_Database.Database).Trim().Length > 0, "" + MacroSuffix, "mdb")));
                _Value = _Value.Replace(MacroPrefix + "DBTIMEOUT" + MacroSuffix, _Database.ConnectionTimeout.ToString());
                _Value = _Value.Replace(MacroPrefix + "DBCMDTOUT" + MacroSuffix, _Database.CommandTimeout.ToString());
            }
            return _Value;
        }

        #endregion

        /* */

    }

    /* */

}
