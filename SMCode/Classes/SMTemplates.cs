/*  ===========================================================================
 *  
 *  File:       SMTemplates.cs
 *  Version:    2.0.125
 *  Date:       January 2025
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2025 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *
 *  SMCode templates management class.
 *
 *  ===========================================================================
 */

using Org.BouncyCastle.Crypto.Parameters;

namespace SMCodeSystem
{

    /* */

    /// <summary>SMCode templates management class.</summary>
    public class SMTemplates
    {

        /* */

        #region Declarations

        /*  ===================================================================
         *  Declarations
         *  ===================================================================
         */

        /// <summary>SM session instance.</summary>
        private readonly SMCode SM = null;

        /// <summary>Last template file.</summary>
        private string lastTemplateFile = "";

        /// <summary>Last template value.</summary>
        private string lastTemplateValue = "";

        #endregion

        /* */

        #region Properties

        /*  ===================================================================
         *  Properties
         *  ===================================================================
         */

        /// <summary>Get cache items collection.</summary>
        public SMDictionary Items { get; private set; }

        /// <summary>Get or set templates path.</summary>
        public string Path { get; set; } = "";

        #endregion

        /* */

        #region Initialization

        /*  ===================================================================
         *  Initialization
         *  ===================================================================
         */

        /// <summary>Class constructor.</summary>
        public SMTemplates(SMCode _SM = null)
        {
            SM = SMCode.CurrentOrNew(_SM);
            Initialize();
        }

        /// <summary>Class constructor.</summary>
        public SMTemplates(SMTemplates _Cache, SMCode _SM = null)
        {
            if (_SM == null) _SM = _Cache.SM;
            SM = SMCode.CurrentOrNew(_SM);
            Initialize();
            Assign(_Cache);
        }

        /// <summary>Initialize instance.</summary>
        private void Initialize()
        {
            Items = new SMDictionary(SM);
            Path = SM.Combine(SM.ExecutablePath, "templates");
            Clear();
        }

        #endregion

        /* */

        #region Methods

        /*  ===================================================================
         *  Methods
         *  ===================================================================
         */

        /// <summary>Assign instance properties from another.</summary>
        public void Assign(SMTemplates _Templates)
        {
            Items.Assign(_Templates.Items);
            lastTemplateFile = _Templates.lastTemplateFile;
            lastTemplateValue = _Templates.lastTemplateValue;
            Path = _Templates.Path;
        }

        /// <summary>Clear item.</summary>
        public void Clear()
        {
            Items.Clear();
            lastTemplateFile = "";
            lastTemplateValue = "";
        }

        /// <summary>Return template by file name from collection and if specified replace all macros 
        /// from dictionary. If specified file will be loaded from subfolder or from
        /// absolute path if folder parameter start by @.</summary>
        public string Get(string _TemplateFile, SMDictionary _Macros = null, string _Folder = null)
        {
            int i;
            string rslt = "";
            if (SM.Empty(_TemplateFile)) return "";
            else
            {
                _TemplateFile = _TemplateFile.Trim();
                if (_TemplateFile == lastTemplateFile) rslt = lastTemplateValue;
                else
                {
                    i = Items.Find(_TemplateFile);
                    if (i < 0) rslt = Load(_TemplateFile, _Folder);
                    else rslt = Items[i].Value;
                    if (rslt.Length > 0)
                    {
                        lastTemplateFile = _TemplateFile;
                        lastTemplateValue = rslt;
                    }
                }
            }
            return SM.ParseMacro(rslt, _Macros);
        }

        /// <summary>Load template from file, and return raw template contents.
        /// If specified file will be loaded from subfolder or from absolute 
        /// path if folder parameter start by @. If start by ~ indicates 
        /// path on root.</summary>
        public string Load(string _TemplateFile, string _Folder = null)
        {
            string rslt = "";
            lastTemplateFile = "";
            lastTemplateValue = "";
            if (_TemplateFile != null)
            {
                _TemplateFile = _TemplateFile.Trim();
                if (_TemplateFile.Length > 0)
                {
                    if (_Folder == null) _Folder = Path;
                    else if (_Folder.StartsWith("@")) _Folder = SM.FixPath(_Folder.Substring(1));
                    else if (_Folder.StartsWith("~")) _Folder = SM.Merge(SMCode.RootPath, SM.Mid(_Folder, 1));
                    else _Folder = SM.Merge(Path, _Folder.Trim());
                    _Folder = SM.Combine(_Folder, _TemplateFile);
                    rslt = SM.LoadString(_Folder);
                    if (rslt.Length > 0) Items.Set(_TemplateFile, rslt);
                }
            }
            return rslt;
        }

        #endregion

        /* */

    }

    /* */

}