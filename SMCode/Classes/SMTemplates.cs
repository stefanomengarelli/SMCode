/*  ===========================================================================
 *  
 *  File:       SMTemplates.cs
 *  Version:    2.0.306
 *  Date:       October 2025
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

using System.Collections.Generic;

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

        /// <summary>Get or set ignore case flag for template names.</summary>
        public bool IgnoreCase
        {
            get { return Items.IgnoreCase; }
            set { Items.IgnoreCase = value; }
        }

        /// <summary>Get cache items collection.</summary>
        public SMDictionary Items { get; private set; }

        /// <summary>Get or set templates path.</summary>
        public string Path { get; set; } = "";

        /// <summary>Get or set templates paths collection.</summary>
        public List<string> Paths { get; private set; } = new List<string>();

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
            int i;
            Items.Assign(_Templates.Items);
            lastTemplateFile = _Templates.lastTemplateFile;
            lastTemplateValue = _Templates.lastTemplateValue;
            Path = _Templates.Path;
            Paths.Clear();
            for (i = 0; i < _Templates.Paths.Count; i++) Paths.Add(_Templates.Paths[i]);
        }

        /// <summary>Clear item.</summary>
        public void Clear()
        {
            Items.Clear();
            lastTemplateFile = "";
            lastTemplateValue = "";
        }

        /// <summary>Return template by file name from collection and if specified 
        /// replace all macros from dictionary. If specified file will be loaded 
        /// from subfolder or from absolute path if folder parameter start by @. 
        /// If start by ~ indicates path on root. If start by &amp; indicates application path.</summary>
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
        /// path on root. If start by &amp; indicates application path.</summary>
        public string Load(string _TemplateFile, string _Folder = null)
        {
            int i = 0;
            string rslt = "", fullPath;
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
                    else if (_Folder.StartsWith("&")) _Folder = SM.Merge(SM.ApplicationPath, SM.Mid(_Folder, 1));
                    else _Folder = SM.Merge(Path, _Folder.Trim());
                    fullPath = SM.Combine(_Folder, _TemplateFile);
                    if (SM.FileExists(fullPath)) rslt = SM.LoadString(fullPath);
                    else
                    {
                        while (i < Paths.Count)
                        {
                            fullPath = SM.Combine(Paths[i], _TemplateFile);
                            if (SM.FileExists(fullPath))
                            {
                                rslt = SM.LoadString(fullPath);
                                i = Paths.Count;
                            }
                            i++;
                        }
                    }
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