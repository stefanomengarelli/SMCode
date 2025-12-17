/*  ===========================================================================
 *  
 *  File:       SMResources.cs
 *  Version:    2.0.312
 *  Date:       December 2025
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2025 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *
 *  SMCode resource management class.
 *
 *  ===========================================================================
 */

using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

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
        private readonly SMCode SM = null;

        #endregion

        /* */

        #region Properties

        /*  ===================================================================
         *  Properties
         *  ===================================================================
         */

        /// <summary>Get or set resource zip password.</summary>
        public string Password { get; set; }

        /// <summary>Internal resources zip file paths collection.</summary>
        public List<string> Paths { get; private set; }

        /// <summary>Instance embedded zip resource dictionary cache collection.</summary>
        public SMDictionary Resources { get; private set; }

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
        public Stream Get(string _ResourcePath)
        {
            int i;
            string f, p;
            byte[] b = null;
            Stream r = null;
            Assembly a;
            _ResourcePath = SM.FixPath(_ResourcePath);
            i = Resources.Find(_ResourcePath);
            if (i < 0)
            {
                i = 0;
                while ((r == null) && (i < Paths.Count))
                {
                    p = Paths[i].Trim();
                    // zip file
                    if (p.ToLower().EndsWith(".zip"))
                    {
                        // embedded zip file
                        if (p.ToLower().StartsWith("@"))
                        {
                            a = Assembly.LoadFrom(SM.Before(p.Substring(1) + '.', ".").Trim());
                            r = a.GetManifestResourceStream(p.Substring(1));
                            r = SM.UnZipStream(r, _ResourcePath, Password, null);
                        }
                        // deployed zip file
                        else if (SM.FileExists(p))
                        {
                            if (SM.UnZipBytes(p, _ResourcePath, ref b, Password, null))
                            {
                                r = new MemoryStream(b);
                            }
                        }
                    }
                    // resource file
                    else
                    {
                        f = SM.Merge(p, _ResourcePath);
                        if (SM.FileExists(f)) r = new MemoryStream(SM.LoadFile(f));
                    }
                    i++;
                }
                if (r != null) Resources.Add(_ResourcePath, "", r);
            }
            else r = (Stream)Resources[i].Tag;
            return r;
        }

        /// <summary>Return byte array from cache or embedded zip resource file, corresponding to resource path.</summary>
        public byte[] GetBytes(string _ResourcePath)
        {
            Stream st;
            BinaryReader br;
            st = Get(_ResourcePath);
            if (st != null)
            {
                br = new BinaryReader(st);
                return br.ReadBytes((int)st.Length);
            }
            else return null;
        }

        /// <summary>Return text from cache or embedded zip resource file, corresponding to resource path.</summary>
        public string GetText(string _ResourcePath, SMDictionary _Macros = null)
        {
            string r = "";
            Encoding encoding;
            Stream st;
            StreamReader sr;
            st = Get(_ResourcePath);
            if (st != null)
            {
                encoding = SM.FileEncoding(st);
                sr = new StreamReader(st, encoding);
                r = sr.ReadToEnd();
                if (_Macros != null) r = SM.ParseMacro(r, _Macros);
            }
            return r;
        }

        #endregion

        /* */

    }

    /* */

}
