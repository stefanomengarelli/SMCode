/*  ===========================================================================
 *  
 *  File:       SMResources.cs
 *  Version:    2.0.15
 *  Date:       April 2024
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2024 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *
 *  SMCode resource management class.
 *
 *  ===========================================================================
 */

using System;
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

        /// <summary>Internal resources zip file path.</summary>
        public string ResourcesPath { get; set; } = "";

        #endregion

        /* */

        #region Initialize

        /*  ===================================================================
         *  Initialize
         *  ===================================================================
         */

        /// <summary>Instance constructor.</summary>
        public SMResources(SMCode _SM)
        {
            SM = SMCode.CurrentOrNew(_SM);
            Resources = new SMDictionary(SM);
            Clear();
        }

        /// <summary>Instance constructor.</summary>
        public SMResources(string _ResourcesZipPath, SMCode _SM)
        {
            SM = SMCode.CurrentOrNew(_SM);
            Resources = new SMDictionary(SM);
            Clear();
            ResourcesPath = _ResourcesZipPath;
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
            ResourcesPath = SM.OnLibraryPath("Resources", "Resources.zip");
            if (!SM.FileExists(ResourcesPath)) ResourcesPath = "";
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

        /// <summary>Return object from cache or embedded zip resource file, corresponding to resource path.
        /// If resource path start by @ stream will be loaded from library\resource path. If resource
        /// path included by [] stream will be loaded from current theme resource path.</summary>
        public Stream GetResource(string _ResourcePath)
        {
            int i;
            byte[] bytes = null;
            Stream st = null;
            try
            {
                _ResourcePath = _ResourcePath.Trim().Replace('/', '\\');
                //
                // search resource on embedded cache or load it
                //
                if (_ResourcePath.Length > 0)
                {
                    i = Resources.Find(_ResourcePath);
                    if (i < 0)
                    {
                        //
                        // load resource from library\resources
                        //
                        if (SM.Empty(ResourcesPath)) st = new MemoryStream(SM.FileLoad(SM.Merge(SM.OnLibraryPath("Resources"), _ResourcePath)));
                        //
                        // load resource from embedded zip file
                        //
                        else
                        {
                            if (SM.UnZipBytes(ResourcesPath, _ResourcePath, ref bytes, "", null))
                            {
                                st = new MemoryStream(bytes);
                            }
                        }
                        //
                        // add resource to embedded cache
                        //
                        Resources.Add(new SMDictionaryItem(_ResourcePath, Resources.Count.ToString(), st));
                    }
                    else st = (Stream)Resources[i].Tag;
                }
                if (st != null) st.Position = 0;
            }
            catch (Exception ex)
            {
                SM.Error(ex);
                st = null;
            }
            return st;
        }


        /// <summary>Return text from cache or embedded zip resource file, corresponding to resource path.</summary>
        public string GetText(string _ResourcePath)
        {
            Encoding encoding;
            Stream st;
            StreamReader sr;
            st = GetResource(_ResourcePath);
            if (st != null)
            {
                encoding = SM.FileEncoding(st);
                sr = new StreamReader(st, encoding);
                return sr.ReadToEnd();
            }
            else return "";
        }

        #endregion

        /* */

    }

    /* */

}
