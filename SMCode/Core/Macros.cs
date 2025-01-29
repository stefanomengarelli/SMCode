/*  ===========================================================================
 *  
 *  File:       Macros.cs
 *  Version:    2.0.140
 *  Date:       January 2025
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2025 by Stefano Mengarelli - All rights reserved - Use, 
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
        public string Macros(string _Value, SMDictionary _Macros = null, bool _Defaults = true, SMDatabase _Database = null)
        {
            int i = 0;
            if (_Value == null) _Value = "";
            if (_Value.Length > (MacroPrefix + MacroSuffix).Length)
            {
                if (_Macros != null)
                {
                    while (i < _Macros.Count)
                    {
                        _Value = Replace(_Value, MacroPrefix + _Macros[i].Key + MacroSuffix, _Macros[i].Value, true);
                        i++;
                    }
                }
                if (_Defaults)
                {
                    _Value = MacrosSystem(_Value);
                    if (_Database != null) _Value = MacrosDatabase(_Value, _Database);
                }
            }
            return _Value;
        }

        /// <summary>Replace value index macros with related values (eg. %%0%%, %%1%%).</summary>
        public string Macros(string _Value, string[] _Values)
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
                _Value = Replace(_Value, MacroPrefix + "applpath" + MacroSuffix, FixPath(ApplicationPath), true);
                _Value = Replace(_Value, MacroPrefix + "commpath" + MacroSuffix, FixPath(CommonPath), true);
                _Value = Replace(_Value, MacroPrefix + "datapath" + MacroSuffix, FixPath(DataPath), true);
                _Value = Replace(_Value, MacroPrefix + "deskpath" + MacroSuffix, FixPath(DesktopPath), true);
                _Value = Replace(_Value, MacroPrefix + "docspath" + MacroSuffix, FixPath(DocumentsPath), true);
                _Value = Replace(_Value, MacroPrefix + "execname" + MacroSuffix, FixPath(ExecutableName), true);
                _Value = Replace(_Value, MacroPrefix + "execpath" + MacroSuffix, FixPath(ExecutablePath), true);
                _Value = Replace(_Value, MacroPrefix + "machine" + MacroSuffix, Machine(), true);
                _Value = Replace(_Value, MacroPrefix + "mydocspath" + MacroSuffix, FixPath(UserDocumentsPath), true);
                _Value = Replace(_Value, MacroPrefix + "temppath" + MacroSuffix, FixPath(TempPath), true);
                _Value = Replace(_Value, MacroPrefix + "user" + MacroSuffix, User.UserName, true);
                _Value = Replace(_Value, MacroPrefix + "sysuser" + MacroSuffix, SystemUser(), true);
                _Value = Replace(_Value, MacroPrefix + "version" + MacroSuffix, ExtractVersion(Version, 4, -1), true);
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
            string ext;
            if (_Value == null) _Value = "";
            if ((_Value.Length > (MacroPrefix + MacroSuffix).Length) && (_Database != null))
            {
                _Value = Replace(_Value, MacroPrefix + "dbhost" + MacroSuffix, _Database.Host, true);
                _Value = Replace(_Value, MacroPrefix + "database" + MacroSuffix, _Database.Database, true);
                _Value = Replace(_Value, MacroPrefix + "dbpath" + MacroSuffix, _Database.Path, true);
                _Value = Replace(_Value, MacroPrefix + "dbuser" + MacroSuffix, _Database.User, true);
                _Value = Replace(_Value, MacroPrefix + "dbpassword" + MacroSuffix, _Database.Password, true);
                ext = FileExtension(_Database.Database).Trim();
                _Value = Replace(_Value, MacroPrefix + "mdbpath" + MacroSuffix, Combine(_Database.Path, _Database.Database, Iif(ext.Length > 0, ext, "mdb")), true);
                _Value = Replace(_Value, MacroPrefix + "dbtimeout" + MacroSuffix, _Database.ConnectionTimeout.ToString(), true);
                _Value = Replace(_Value, MacroPrefix + "dbcmdtout" + MacroSuffix, _Database.CommandTimeout.ToString(), true);
            }
            return _Value;
        }

        #endregion

        /* */

    }

    /* */

}
