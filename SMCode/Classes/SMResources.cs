/*  ===========================================================================
 *  
 *  File:       SMResources.cs
 *  Version:    2.3.5
 *  Date:       June 2026
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2026 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *
 *  SMCode resource management class.
 *
 *  ===========================================================================
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace SMCodeSystem
{

    /* */

    /// <summary>SMCode resource management class.</summary>
    public class SMResources
    {

        /* */

        #region Declarations

        /*  ===================================================================
         *  Declarations
         *  ===================================================================
         */

        /// <summary>SM session instance.</summary>
        public readonly SMCode SM = null;

        #endregion

        /* */

        #region Properties

        /*  ===================================================================
         *  Properties
         *  ===================================================================
         */

        /// <summary>Get or set resource zip password.</summary>
        public string Password { get; set; }

        /// <summary>Resources file paths collection.</summary>
        public List<string> Paths { get; private set; }

        /// <summary>Instance embedded zip resource dictionary cache collection.</summary>
        public SMDictionary Resources { get; private set; }

        /// <summary>Get or set instance tag object.</summary>
        public object Tag { get; set; } = null;

        #endregion

        /* */

        #region Initialize

        /*  ===================================================================
         *  Initialize
         *  ===================================================================
         */

        /// <summary>Instance constructor.</summary>
        public SMResources(SMCode _SM = null, SMResources _AssignInstance = null)
        {
            SM = SMCode.CurrentOrNew(_SM);
            Initialize();
            if (_AssignInstance != null) Assign(_AssignInstance);
        }

        /// <summary>Initialize instance.</summary>
        private void Initialize()
        {
            Resources = new SMDictionary(SM);
            Paths = new List<string>();
            Password = "";
        }

        #endregion

        /* */

        #region Methods

        /*  ===================================================================
         *  Methods
         *  ===================================================================
         */

        /// <summary>Add path to resource search paths, if exists. If path starts with @ embedded zip file path assumed. Return true if succeed.</summary>
        public bool AddPath(string _Path)
        {
            bool r = false;
            _Path = SM.FixPath(SM.RealPath(_Path));
            if (_Path.Length > 0)
            {
                r = _Path[0] == '@';
                if (!r) r = SM.FileExists(_Path);
                if (!r) r = SM.FolderExists(_Path);
                if (r) Paths.Add(_Path);
            }
            return r;
        }

        /// <summary>Copy values from another instance. Return true if succeed.</summary>
        public SMResources Assign(SMResources _Instance)
        {
            int i;
            Paths.Clear();
            for (i = 0; i < _Instance.Paths.Count; i++) Paths.Add(_Instance.Paths[i]);
            Resources.Assign(_Instance.Resources);
            Password = _Instance.Password;
            Tag = _Instance.Tag;
            return this;
        }

        /// <summary>Clear instance.</summary>
        public void Clear()
        {
            Resources.Clear();
        }

        /// <summary>Return byte array from cache or resource file paths, corresponding to resource path.</summary>
        public SMResource Get(string _ResourcePath, bool _ExtractMacroTopics = false)
        {
            int i;
            bool z;
            string p;
            byte[] b = null;
            Assembly a;
            Stream stream = null;
            SMResource rslt = null;
            if (!SM.Empty(_ResourcePath))
            {
                _ResourcePath = SM.FixPath(_ResourcePath);
                i = Resources.Find(_ResourcePath);
                if (i < 0)
                {
                    // get fixed real path
                    p = SM.FixPath(SM.RealPath(_ResourcePath));
                    // load direct file resource if exists
                    if (SM.FileExists(p)) stream = new MemoryStream(SM.LoadFile(p));
                    // find resource in paths
                    else
                    {
                        // search on paths
                        i = 0;
                        z = (_ResourcePath[0] != '~') && (_ResourcePath.IndexOf(':') < 0);
                        while ((stream == null) && (i < Paths.Count))
                        {
                            p = Paths[i].Trim();
                            // zip file
                            if (z && p.EndsWith(".zip", StringComparison.CurrentCultureIgnoreCase))
                            {
                                // embedded zip file
                                if (p[0] == '@')
                                {
                                    a = Assembly.LoadFrom(SM.Before(p.Substring(1) + '.', ".").Trim());
                                    stream = a.GetManifestResourceStream(p.Substring(1));
                                    stream = SM.UnZipStream(stream, _ResourcePath, Password, null);
                                }
                                // deployed zip file
                                else if (SM.FileExists(SM.RealPath(p)))
                                {
                                    if (SM.UnZipBytes(SM.RealPath(p), _ResourcePath, ref b, Password, null))
                                    {
                                        stream = new MemoryStream(b);
                                    }
                                }
                            }
                            // resource file
                            else
                            {
                                p = SM.RealPath(SM.Merge(p, _ResourcePath));
                                if (SM.FileExists(p)) stream = new MemoryStream(SM.LoadFile(p));
                            }
                            i++;
                        }
                    }
                    // add resource to cache
                    if (stream != null)
                    {
                        rslt = new SMResource(SM);
                        rslt.Key = _ResourcePath;
                        rslt.Stream = stream;
                        if (_ExtractMacroTopics) rslt.Macros.FromMacros(rslt.GetText());
                        Resources.Add(_ResourcePath, "", rslt);
                    }
                }
                else rslt = (SMResource)Resources[i].Tag;
            }
            return rslt;
        }

        /// <summary>Return byte array from cache or embedded zip resource file, corresponding to resource path.</summary>
        public byte[] GetBytes(string _ResourcePath)
        {
            SMResource resource = Get(_ResourcePath, false);
            if (resource != null) return resource.GetBytes();
            else return null;
        }

        /// <summary>Return text from cache or embedded zip resource file, corresponding to resource path.</summary>
        public string GetText(string _ResourcePath, bool _ExtractMacroTopics = true)
        {
            SMResource resource = Get(_ResourcePath, _ExtractMacroTopics);
            if (resource != null) return resource.GetText();
            else return null;
        }

        #endregion

        /* */

    }

    /* */

}
