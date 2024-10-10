/*  ===========================================================================
 *  
 *  File:       Macro.cs
 *  Version:    2.0.30
 *  Date:       June 2024
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2024 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *
 *  SMCode macro functions.
 *
 *  ===========================================================================
 */

using System.IO;

namespace SMCodeSystem
{

    /* */

    /// <summary>SMCode macro functions.</summary>
    public partial class SMCode
    {

        /* */

        #region Methods

        /*  ===================================================================
         *  Methods
         *  ===================================================================
         */

        /// <summary>Return value with macros replaced.
        /// %%APPLPATH%%   = application path
        /// %%COMMPATH%%   = common path
        /// %%DATAPATH%%   = data path
        /// %%DESKPATH%%   = user desktop path
        /// %%DOCSPATH%%   = common documents path
        /// %%EXECNAME%%   = executable name without extension
        /// %%EXECPATH%%   = executable path
        /// %%MACHINE%%    = current PC name
        /// %%MYDOCSPATH%% = user documents path
        /// %%TEMPPATH%%   = application temporary files path
        /// %%USER%%       = application logged user id
        /// %%SYSUSER%%    = system logged user id
        /// %%VERSION%%    = application version
        /// </summary>
        public string Macro(string _Value, SMDatabase _Database = null)
        {
            const string macroQuote = "%%"; 
            _Value = _Value.Replace(macroQuote + "APPLPATH" + macroQuote, FixPath(ApplicationPath));
            _Value = _Value.Replace(macroQuote + "COMMPATH" + macroQuote, FixPath(CommonPath));
            _Value = _Value.Replace(macroQuote + "DATAPATH" + macroQuote, FixPath(DataPath));
            _Value = _Value.Replace(macroQuote + "DESKPATH" + macroQuote, FixPath(DesktopPath));
            _Value = _Value.Replace(macroQuote + "DOCSPATH" + macroQuote, FixPath(DocumentsPath));
            _Value = _Value.Replace(macroQuote + "EXECNAME" + macroQuote, FixPath(ExecutableName));
            _Value = _Value.Replace(macroQuote + "EXECPATH" + macroQuote, FixPath(ExecutablePath));
            _Value = _Value.Replace(macroQuote + "MACHINE" + macroQuote, Machine());
            _Value = _Value.Replace(macroQuote + "MYDOCSPATH" + macroQuote, FixPath(UserDocumentsPath));
            _Value = _Value.Replace(macroQuote + "TEMPPATH" + macroQuote, FixPath(TempPath));
            _Value = _Value.Replace(macroQuote + "USER" + macroQuote, User.UserId);
            _Value = _Value.Replace(macroQuote + "SYSUSER" + macroQuote, SystemUser());
            _Value = _Value.Replace(macroQuote + "VERSION" + macroQuote, ExtractVersion(Version,4,-1));
            if (_Database != null)
            {
                _Value = _Value.Replace(macroQuote + "DBHOST" + macroQuote, _Database.Host);
                _Value = _Value.Replace(macroQuote + "DATABASE" + macroQuote, _Database.Database);
                _Value = _Value.Replace(macroQuote + "DBPATH" + macroQuote, _Database.Path);
                _Value = _Value.Replace(macroQuote + "DBUSER" + macroQuote, _Database.User);
                _Value = _Value.Replace(macroQuote + "DBPASSWORD" + macroQuote, _Database.Password);
                _Value = _Value.Replace(macroQuote + "MDBPATH" + macroQuote, SM.Combine(_Database.Path, _Database.Database, SM.Iif(SM.FileExtension(_Database.Database).Trim().Length > 0, "", "mdb")));
                _Value = _Value.Replace(macroQuote + "DBTIMEOUT" + macroQuote, _Database.ConnectionTimeout.ToString());
            }
            return _Value;
        }

        #endregion

        /* */

    }

    /* */

}
