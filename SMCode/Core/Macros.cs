/*  ===========================================================================
 *  
 *  File:       Macros.cs
 *  Version:    2.0.303
 *  Date:       October 2025
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
            macros.Add("deskpath", FixPath(DesktopPath));
            macros.Add("docspath", FixPath(DocumentsPath));
            macros.Add("execname", FixPath(ExecutableName));
            macros.Add("execpath", FixPath(ExecutablePath));
            macros.Add("machine", Machine());
            macros.Add("mydocspath", FixPath(UserDocumentsPath));
            macros.Add("temppath", FixPath(TempPath));
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
        public SMDictionary Macros(SMDatabase _Database)
        {
            string ext;
            SMDictionary macros = Macros();
            if (_Database != null)
            {
                macros.Add("dbhost", _Database.Host);
                macros.Add("database", _Database.Database);
                macros.Add("dbpath", _Database.Path);
                macros.Add("dbuser", _Database.User);
                macros.Add("dbpassword", _Database.Password);
                ext = FileExtension(_Database.Database).Trim();
                macros.Add("mdbpath", Combine(_Database.Path, _Database.Database, Iif(ext.Length > 0, ext, "mdb")));
                macros.Add("dbtimeout", _Database.ConnectionTimeout.ToString());
                macros.Add("dbcmdtout", _Database.CommandTimeout.ToString());
            }
            return macros;
        }

        /// <summary>Returns dictionary with system, database and dataset fields macros.</summary>
        public SMDictionary Macros(SMDataset _Dataset)
        {
            int i;
            string v;
            DataColumn c;
            SMDictionary macros;
            if (_Dataset != null)
            {
                macros = Macros(_Dataset.Database);
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
                        else v = "";
                        macros.Set(MacroFieldPrefix + c.ColumnName + MacroFieldSuffix, v);
                    }
                }
            }
            else macros = Macros();
            return macros;
        }

        #endregion

        /* */

    }

    /* */

}
