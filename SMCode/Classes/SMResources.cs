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

        /// <summary>Instance embedded zip resource dictionary cache collection.</summary>
        public SMDictionary Resources { get; private set; }

        /// <summary>Internal resources zip file paths collection.</summary>
        public List<string> ResourcesPaths { get; private set; }

        /// <summary>Indicates if zip file is to be considered before resource paths.</summary>
        public bool ZipBeforeResources { get; set; }

        /// <summary>Get or set resource zip password.</summary>
        public string ZipPassword { get; set; }

        /// <summary>Get or set resource zip file full path.</summary>
        public string ZipPath { get; set; }

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
            string s;
            Resources = new SMDictionary(SM);
            ResourcesPaths = new List<string>();
            ZipBeforeResources = false;
            ZipPassword = "";
            ZipPath = SM.OnExecPath("Resources.zip");
            s = SM.Merge(SM.ExecutablePath, "Library", "Resources");
            if (SM.FolderExists(s))
            {
                ResourcesPaths.Add(s);
                if (!SM.FileExists(ZipPath))
                {
                    ZipPath = SM.Merge(s, "Resources.zip");
                }
            }
            if (!SM.FileExists(ZipPath)) ZipPath = "";
        }

        #endregion

        /* */

        #region Methods

        /*  ===================================================================
         *  Methods
         *  ===================================================================
         */

        /// <summary>Clear instance.</summary>
        public void Clear()
        {
            Resources.Clear();
        }

        /// <summary>Return byte array from cache or embedded zip resource file, corresponding to resource path.</summary>
        public byte[] GetBytes(string _ResourcePath)
        {
            Stream st;
            BinaryReader br;
            st = GetResource(_ResourcePath);
            if (st != null)
            {
                br = new BinaryReader(st);
                return br.ReadBytes((int)st.Length);
            }
            else return null;
        }

        /// <summary>Return byte array from cache or resource file paths, corresponding to resource path.</summary>
        public Stream GetResource(string _ResourcePath)
        {
            int i;
            Stream r = null;
            _ResourcePath = SM.FixPath(_ResourcePath);
            i = Resources.Find(_ResourcePath);
            if (i < 0)
            {
                if (ZipBeforeResources)
                {
                    r = GetResourceFromZip(_ResourcePath);
                    if (r == null) r = GetResourceFromPaths(_ResourcePath);
                }
                else
                {
                    r = GetResourceFromPaths(_ResourcePath);
                    if (r == null) r = GetResourceFromZip(_ResourcePath);
                }
                if (r != null) Resources.Add(_ResourcePath, "", r);
            }
            else r = (Stream)Resources[i].Tag;
            return r;
        }

        /// <summary>Get resource byte array finding on resource paths.</summary>
        public Stream GetResourceFromPaths(string _ResourcePath)
        {
            int i = 0;
            string f;
            Stream r = null;
            _ResourcePath = SM.FixPath(_ResourcePath);
            while ((r == null) && (i < ResourcesPaths.Count))
            {
                f = SM.Merge(ResourcesPaths[i], _ResourcePath);
                if (SM.FileExists(f)) r = new MemoryStream(SM.LoadFile(f));
                i++;
            }
            return r;
        }

        /// <summary>Get resource byte array finding on zip file.</summary>
        public Stream GetResourceFromZip(string _ResourcePath)
        {
            byte[] r = null;
            _ResourcePath = SM.FixPath(_ResourcePath);
            if (SM.FileExists(ZipPath))
            {
                if (SM.UnZipBytes(ZipPath, _ResourcePath, ref r, ZipPassword, null))
                {
                    return new MemoryStream(r);
                }
                else return null;
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
            st = GetResource(_ResourcePath);
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
