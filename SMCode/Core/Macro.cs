/*  ===========================================================================
 *  
 *  File:       Macro.cs
 *  Version:    2.0.54
 *  Date:       October 2024
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
        public string Macro(string _Value, SMDatabase _Database = null, string _MacroQuoteBegin = null, string _MacroQuoteEnd = null)
        {
            if (_MacroQuoteBegin == null) _MacroQuoteBegin = MacroQuoteBegin;
            if (_MacroQuoteEnd == null) _MacroQuoteEnd = MacroQuoteEnd;
            _Value = _Value.Replace(_MacroQuoteBegin + "APPLPATH" + _MacroQuoteEnd, FixPath(ApplicationPath));
            _Value = _Value.Replace(_MacroQuoteBegin + "COMMPATH" + _MacroQuoteEnd, FixPath(CommonPath));
            _Value = _Value.Replace(_MacroQuoteBegin + "DATAPATH" + _MacroQuoteEnd, FixPath(DataPath));
            _Value = _Value.Replace(_MacroQuoteBegin + "DESKPATH" + _MacroQuoteEnd, FixPath(DesktopPath));
            _Value = _Value.Replace(_MacroQuoteBegin + "DOCSPATH" + _MacroQuoteEnd, FixPath(DocumentsPath));
            _Value = _Value.Replace(_MacroQuoteBegin + "EXECNAME" + _MacroQuoteEnd, FixPath(ExecutableName));
            _Value = _Value.Replace(_MacroQuoteBegin + "EXECPATH" + _MacroQuoteEnd, FixPath(ExecutablePath));
            _Value = _Value.Replace(_MacroQuoteBegin + "MACHINE" + _MacroQuoteEnd, Machine());
            _Value = _Value.Replace(_MacroQuoteBegin + "MYDOCSPATH" + _MacroQuoteEnd, FixPath(UserDocumentsPath));
            _Value = _Value.Replace(_MacroQuoteBegin + "TEMPPATH" + _MacroQuoteEnd, FixPath(TempPath));
            _Value = _Value.Replace(_MacroQuoteBegin + "USER" + _MacroQuoteEnd, User.UserName);
            _Value = _Value.Replace(_MacroQuoteBegin + "SYSUSER" + _MacroQuoteEnd, SystemUser());
            _Value = _Value.Replace(_MacroQuoteBegin + "VERSION" + _MacroQuoteEnd, ExtractVersion(Version,4,-1));
            if (_Database != null)
            {
                _Value = _Value.Replace(_MacroQuoteBegin + "DBHOST" + _MacroQuoteEnd, _Database.Host);
                _Value = _Value.Replace(_MacroQuoteBegin + "DATABASE" + _MacroQuoteEnd, _Database.Database);
                _Value = _Value.Replace(_MacroQuoteBegin + "DBPATH" + _MacroQuoteEnd, _Database.Path);
                _Value = _Value.Replace(_MacroQuoteBegin + "DBUSER" + _MacroQuoteEnd, _Database.User);
                _Value = _Value.Replace(_MacroQuoteBegin + "DBPASSWORD" + _MacroQuoteEnd, _Database.Password);
                _Value = _Value.Replace(_MacroQuoteBegin + "MDBPATH" + _MacroQuoteEnd, SM.Combine(_Database.Path, _Database.Database, SM.Iif(SM.FileExtension(_Database.Database).Trim().Length > 0, "", "mdb")));
                _Value = _Value.Replace(_MacroQuoteBegin + "DBTIMEOUT" + _MacroQuoteEnd, _Database.ConnectionTimeout.ToString());
            }
            return _Value;
        }

        #endregion

        /* */

    }

    /* */

}
