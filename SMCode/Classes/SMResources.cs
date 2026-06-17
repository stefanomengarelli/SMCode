/*  ===========================================================================
 *  
 *  File:       SMResources.cs
 *  Version:    2.1.1
 *  Date:       April 2026
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
        public SMResources(SMCode _SM = null)
        {
            SM = SMCode.CurrentOrNew(_SM);
            Initialize();
            Clear();
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

        /// <summary>Add path to resource search paths, if exists. Return true if succeed.</summary>
        public bool AddPath(string _Path)
        {
            bool r = false;
            _Path = SM.FixPath(_Path).Trim();
            r = _Path.StartsWith("@");
            if (!r) r = SM.FileExists(_Path);
            if (r) Paths.Add(_Path);
            return r;
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
            string f, p;
            byte[] b = null;
            Assembly a;
            Stream stream = null;
            SMResource rslt = null;
            _ResourcePath = SM.FixPath(_ResourcePath);
            i = Resources.Find(_ResourcePath);
            if (i < 0)
            {
                i = 0;
                while ((stream == null) && (i < Paths.Count))
                {
                    p = Paths[i].Trim();
                    // zip file
                    if (p.ToLower().EndsWith(".zip"))
                    {
                        // embedded zip file
                        if (p.ToLower().StartsWith("@"))
                        {
                            a = Assembly.LoadFrom(SM.Before(p.Substring(1) + '.', ".").Trim());
                            stream = a.GetManifestResourceStream(p.Substring(1));
                            stream = SM.UnZipStream(stream, _ResourcePath, Password, null);
                        }
                        // deployed zip file
                        else if (SM.FileExists(p))
                        {
                            if (SM.UnZipBytes(p, _ResourcePath, ref b, Password, null))
                            {
                                stream = new MemoryStream(b);
                            }
                        }
                    }
                    // resource file
                    else
                    {
                        f = SM.Merge(p, _ResourcePath);
                        if (SM.FileExists(f)) stream = new MemoryStream(SM.LoadFile(f));
                    }
                    i++;
                }
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
