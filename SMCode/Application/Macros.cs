/*  ===========================================================================
 *  
 *  File:       Macros.cs
 *  Version:    2.1.5
 *  Date:       June 2026
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2026 by Stefano Mengarelli - All rights reserved - Use, 
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

        /// <summary>Get or set macro date format.</summary>
        public string MacroDateFormat { get; set; } = "yyyy-MM-dd";

        /// <summary>Get or set macro field prefix.</summary>
        public string MacroFieldPrefix { get; set; } = "";

        /// <summary>Get or set macro field suffix.</summary>
        public string MacroFieldSuffix { get; set; } = "";

        /// <summary>Get or set macro ignore case mode.</summary>
        public bool MacroIgnoreCase { get; set; } = true;

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

        /// <summary>Return true if value contains macros, false otherwise.</summary>
        public bool HasMacros(string _Value)
        {
            if ((_Value == null) || (MacroPrefix == null) || (MacroSuffix == null)) return false;
            else if ((_Value.Length < 1) || (MacroPrefix.Length < 1) || (MacroSuffix.Length < 1)) return false;
            else if (_Value.IndexOf(MacroPrefix) < 0) return false;
            else if (_Value.IndexOf(MacroSuffix) < 0) return false;
            else return true;
        }

        /// <summary>Returns dictionary with system macros.
        /// APPLPATH   = application path
        /// COMMPATH   = common path
        /// DATAPATH   = data path
        /// DATE       = current date
        /// DESKPATH   = user desktop path
        /// DOCSPATH   = common documents path
        /// EXECNAME   = executable name without extension
        /// EXECPATH   = executable path
        /// MACHINE    = current PC name
        /// MYDOCSPATH = user documents path
        /// TEMPPATH   = application temporary files path
        /// TIME       = current time
        /// USER       = application logged user name
        /// USERID     = application logged user id
        /// USERFIRSTNAME = application logged user first name
        /// USERLASTNAME  = application logged user last name
        /// USERBIRTHDATE = application logged user birth date
        /// USERSEX    = application logged user sex
        /// USERTAXCODE   = application logged user tax code
        /// USERTEXT   = application logged user text
        /// SYSUSER    = system logged user id
        /// VERSION    = application version
        /// </summary>
        public SMDictionary Macros(bool _IncludeSystemMacros = true)
        {
            SMDictionary macros = new SMDictionary(this);
            if (_IncludeSystemMacros)
            {
                macros.Add("applpath", FixPath(ApplicationPath));
                macros.Add("commpath", FixPath(CommonPath));
                macros.Add("datapath", FixPath(DataPath));
                macros.Add("date", ToStr(DateTime.Now, false));
                macros.Add("deskpath", FixPath(DesktopPath));
                macros.Add("docspath", FixPath(DocumentsPath));
                macros.Add("execname", FixPath(ExecutableName));
                macros.Add("execpath", FixPath(ExecutablePath));
                macros.Add("machine", Machine());
                macros.Add("mydocspath", FixPath(UserDocumentsPath));
                macros.Add("temppath", FixPath(TempPath));
                macros.Add("time", DateTime.Now.ToString("HH:mm:ss"));
                macros.Add("user", User.UserName);
                macros.Add("userid", User.IdUser.ToString());
                macros.Add("userfirstname", User.FirstName);
                macros.Add("userlastname", User.LastName);
                macros.Add("userbirthdate", ToStr(User.BirthDate, false));
                macros.Add("usersex", "" + User.Sex);
                macros.Add("usertaxcode", User.TaxCode);
                macros.Add("usertext", User.Text);
                macros.Add("sysuser", SystemUser());
                macros.Add("version", ExtractVersion(Version, 4, -1));
            }
            return macros;
        }

        /// <summary>Returns dictionary with system and database macros.
        /// DBHOST     = database host
        /// DATABASE   = database name
        /// DBPATH     = data path
        /// DBUSER     = database user name
        /// DBPASSWORD = database user password
        /// MDBPATH    = access mdb database path
        /// DBTIMEOUT  = database connection timeout
        /// DBCMDTOUT  = database command timeout
        /// </summary>
        public SMDictionary Macros(SMDatabase _Database, bool _IncludeSystemMacros = true)
        {
            string ext;
            SMDictionary macros = Macros(_IncludeSystemMacros);
            if (_Database != null)
            {
                macros.Add("dbhost", _Database.Host);
                macros.Add("database", _Database.Database);
                macros.Add("dbpath", _Database.Path);
                macros.Add("dbuser", _Database.User);
                macros.Add("dbpassword", _Database.Password);
                ext = FileExtension(_Database.Database).Trim();
                if (ext.Length < 1) ext = "mdb";
                macros.Add("mdbpath", Combine(_Database.Path, _Database.Database, ext));
                macros.Add("dbtimeout", _Database.ConnectionTimeout.ToString());
                macros.Add("dbcmdtout", _Database.CommandTimeout.ToString());
            }
            return macros;
        }

        /// <summary>Returns dictionary with system, database and dataset fields macros.</summary>
        public SMDictionary Macros(SMDataset _Dataset, bool _IncludeBlobMacros = false, bool _IncludeDatabaseMacros = true, bool _IncludeSystemMacros = true)
        {
            int i;
            SMDictionary macros;
            if (_IncludeDatabaseMacros && (_Dataset != null)) macros = Macros(_Dataset.Database, _IncludeSystemMacros);
            else macros = Macros(_IncludeSystemMacros);
            if (_Dataset != null)
            {
                if (_Dataset.DataReady())
                {
                    for (i = 0; i < _Dataset.Columns.Count; i++)
                    {
                        macros.Add(MacroFieldPrefix + _Dataset.Columns[i].ColumnName + MacroFieldSuffix, _Dataset.FieldMacro(i));
                    }
                }
            }
            return macros;
        }

        /// <summary>Replace value macros with current values of dictionary and dataset if specified. 
        /// It's possible specify topic database to list only contained macros to speedup.</summary>
        public string ParseMacro(string _Value, SMDictionary _Macros, SMDataset _Dataset = null, SMDictionary _Topics = null)
        {
            int i, j;
            string a, b;
            // validate parameters
            if ((_Value != null) && ((_Macros != null) || (_Dataset != null)))
            {
                // check if value can contains macros
                if (_Value.Length > MacroPrefix.Length)
                {
                    if (_Value.IndexOf(MacroPrefix) > -1)
                    {
                        // consider only topic with macros contained
                        if (_Topics != null)
                        {
                            if (_Topics.Count < 1) _Topics = null;
                        }
                        // case without topics: replace all macros
                        if (_Topics == null)
                        {
                            // replace dataset macros
                            if (_Dataset != null)
                            {
                                if (_Dataset.DataReady())
                                {
                                    a = MacroPrefix + MacroFieldPrefix;
                                    b = MacroFieldSuffix + MacroSuffix;
                                    for (i = 0; i < _Dataset.Columns.Count; i++)
                                    {
                                        _Value = Replace(_Value, a + _Dataset.Columns[i].ColumnName + b, _Dataset.FieldMacro(i), MacroIgnoreCase);
                                    }
                                }
                            }
                            // replace dictionary macros
                            if (_Macros != null)
                            {
                                for (i = 0; i < _Macros.Count; i++)
                                {
                                    _Value = Replace(_Value, MacroPrefix + _Macros[i].Key + MacroSuffix, _Macros[i].Value, MacroIgnoreCase);
                                }
                            }
                        }
                        // case with topic: replace only macros contained in topics
                        else
                        {
                            // check if dataset is ready
                            if (_Dataset != null)
                            {
                                if (!_Dataset.DataReady()) _Dataset = null;
                            }
                            // replace macros in topics
                            for (i = 0; i < _Topics.Count; i++)
                            {
                                j = -1;
                                a = _Topics[i].Key;
                                // test if macro is a dataset field
                                if (_Dataset != null)
                                {
                                    // remove macro field prefix and suffix if specified
                                    b = a;
                                    if (MacroFieldPrefix.Length > 0)
                                    {
                                        if (b.StartsWith(MacroFieldPrefix))
                                        {
                                            b = b.Substring(MacroFieldPrefix.Length, b.Length - MacroFieldPrefix.Length);
                                        }
                                    }
                                    if (MacroFieldSuffix.Length > 0)
                                    {
                                        if (b.EndsWith(MacroFieldSuffix))
                                        {
                                            b = b.Substring(0, b.Length - MacroFieldSuffix.Length);
                                        }
                                    }
                                    // find field index
                                    j = _Dataset.Columns.IndexOf(b);
                                    if (j > -1)
                                    {
                                        _Value = Replace(_Value, MacroPrefix + a + MacroSuffix, _Dataset.FieldMacro(j), MacroIgnoreCase);
                                    }
                                }
                                // test if macro is a dictionary item
                                if ((j < 0) && (_Macros != null))
                                {
                                    j = _Macros.Find(a);
                                    if (j > -1)
                                    {
                                        _Value = Replace(_Value, MacroPrefix + a + MacroSuffix, _Macros[j].Value, MacroIgnoreCase);
                                    }
                                }
                            }
                        }
                    }
                }
                return _Value;
            }
            else return "";
        }

        /// <summary>Replace value index macros with related values (eg. %%0%%, %%1%%).</summary>
        public string ParseMacro(string _Value, string[] _Values)
        {
            int i = 0;
            if (_Value != null)
            {
                if ((_Values != null) && (_Value.Length > MacroPrefix.Length))
                {
                    if (_Value.IndexOf(MacroPrefix) > -1)
                    {
                        while (i < _Values.Length)
                        {
                            _Value = Replace(_Value, MacroPrefix + i.ToString() + MacroSuffix, _Values[i], MacroIgnoreCase);
                            i++;
                        }
                    }
                }
                return _Value;
            }
            else return "";
        }

        #endregion

        /* */

    }

    /* */

}
