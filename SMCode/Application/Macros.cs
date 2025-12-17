/*  ===========================================================================
 *  
 *  File:       Macros.cs
 *  Version:    2.0.311
 *  Date:       November 2025
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
using System.Data;

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

        /// <summary>Replace value macros with current values.</summary>
        public string ParseMacro(string _Value, SMDictionary _Macros)
        {
            int i = 0;
            if (_Value == null) _Value = "";
            if (_Macros != null)
            {
                if (_Value.Length > (MacroPrefix + MacroSuffix).Length)
                {
                    while (i < _Macros.Count)
                    {
                        _Value = Replace(_Value, MacroPrefix + _Macros[i].Key + MacroSuffix, _Macros[i].Value, MacroIgnoreCase);
                        i++;
                    }
                }
            }
            return _Value;
        }

        /// <summary>Replace value index macros with related values (eg. %%0%%, %%1%%).</summary>
        public string ParseMacro(string _Value, string[] _Values)
        {
            int i = 0;
            if (_Value == null) _Value = "";
            if (_Values != null)
            {
                if (_Values.Length > (MacroPrefix + MacroSuffix).Length)
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

        /// <summary>Returns dictionary with system macros.
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
        public SMDictionary Macros()
        {
            SMDictionary macros = new SMDictionary(this);
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
            macros.Add("sysuser" , SystemUser());
            macros.Add("version" , ExtractVersion(Version, 4, -1));
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
        public SMDictionary Macros(SMDatabase _Database, bool _IncludeDefaults = true)
        {
            string ext;
            SMDictionary macros;
            if (_IncludeDefaults) macros = Macros();
            else macros = new SMDictionary(this);
            if (_Database != null)
            {
                macros.Set("dbhost", _Database.Host);
                macros.Set("database", _Database.Database);
                macros.Set("dbpath", _Database.Path);
                macros.Set("dbuser", _Database.User);
                macros.Set("dbpassword", _Database.Password);
                ext = FileExtension(_Database.Database).Trim();
                macros.Set("mdbpath", Combine(_Database.Path, _Database.Database, Iif(ext.Length > 0, ext, "mdb")));
                macros.Set("dbtimeout", _Database.ConnectionTimeout.ToString());
                macros.Set("dbcmdtout", _Database.CommandTimeout.ToString());
            }
            return macros;
        }

        /// <summary>Returns dictionary with system, database and dataset fields macros.</summary>
        public SMDictionary Macros(SMDataset _Dataset, bool _IncludeBlobs = false, bool _IncludeDatabaseDefaults = true)
        {
            int i;
            string v;
            DataColumn c;
            SMDictionary macros;
            if (_IncludeDatabaseDefaults)
            {
                if (_Dataset == null) macros = Macros(); 
                else macros = Macros(_Dataset.Database);
            }
            else macros = new SMDictionary(this);
            if (_Dataset != null)
            {
                if (_Dataset.DataReady())
                {
                    for (i = 0; i < _Dataset.Columns.Count; i++)
                    {
                        c = _Dataset.Columns[i];
                        if (SMDataType.IsText(c.DataType)) v = _Dataset.FieldStr(c.ColumnName);
                        else if (SMDataType.IsInteger(c.DataType)) v = _Dataset.FieldInt(c.ColumnName).ToString();
                        else if (SMDataType.IsNumeric(c.DataType)) v = _Dataset.FieldDouble(c.ColumnName).ToString("###############0.############");
                        else if (SMDataType.IsDate(c.DataType)) v = ToStr(_Dataset.FieldDateTime(c.ColumnName), MacroDateFormat);
                        else if (SMDataType.IsBoolean(c.DataType)) v = ToBool(_Dataset.FieldBool(c.ColumnName));
                        else if (_IncludeBlobs && SMDataType.IsBlob(c.DataType)) v = Base64EncodeBytes(_Dataset.FieldBlob(c.ColumnName));
                        else v = _Dataset.FieldStr(c.ColumnName);
                        macros.Set(MacroFieldPrefix + c.ColumnName + MacroFieldSuffix, v);
                    }
                }
            }
            return macros;
        }

        #endregion

        /* */

    }

    /* */

}
