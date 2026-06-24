/*  ===========================================================================
 *  
 *  File:       SMResource.cs
 *  Version:    2.3.5
 *  Date:       June 2026
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2026 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *
 *  SMCode resource item class.
 *
 *  ===========================================================================
 */

using System.IO;
using System.Text;

namespace SMCodeSystem
{

    /* */

    /// <summary>SMCode dictionary item class.</summary>
    public class SMResource
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

        /// <summary>Get or set item key.</summary>
        public string Key { get; set; }

        /// <summary>Resource macros dictionary.</summary>
        public SMDictionary Macros { get; private set; } = new SMDictionary(null);

        /// <summary>Get or set item value.</summary>
        public Stream Stream { get; set; }

        /// <summary>Get or set item tag object.</summary>
        public object Tag { get; set; }

        #endregion

        /* */

        #region Initialization

        /*  ===================================================================
         *  Initialization
         *  ===================================================================
         */

        /// <summary>Class constructor.</summary>
        public SMResource(SMCode _SM)
        {
            SM = SMCode.CurrentOrNew(_SM);
            Clear();
        }

        /// <summary>Class constructor.</summary>
        public SMResource(SMCode _SM, SMResource _Instance)
        {
            SM = SMCode.CurrentOrNew(_SM);  
            Assign(_Instance);
        }

        /// <summary>Class constructor.</summary>
        public SMResource(SMCode _SM, string _Key, Stream _Stream, SMDictionary _Macros = null, object _Tag = null)
        {
            SM = SMCode.CurrentOrNew(_SM);
            Key = _Key;
            Macros.Assign(_Macros);
            Stream = _Stream;
            Tag = _Tag;
        }

        #endregion

        /* */

        #region Methods

        /*  ===================================================================
         *  Methods
         *  ===================================================================
         */

        /// <summary>Assign instance properties from another.</summary>
        public void Assign(SMResource _Instance)
        {
            Key = _Instance.Key;
            Macros.Assign(_Instance.Macros);
            Stream = _Instance.Stream;
            Tag = _Instance.Tag;
        }

        /// <summary>Clear item.</summary>
        public void Clear()
        {
            Key = "";
            Macros.Clear();
            Stream = null;
            Tag = null;
        }

        /// <summary>Return byte array from resource.</summary>
        public byte[] GetBytes()
        {
            BinaryReader br;
            if (Stream != null)
            {
                br = new BinaryReader(Stream);
                return br.ReadBytes((int)Stream.Length);
            }
            else return null;
        }

        /// <summary>Return text from resource.</summary>
        public string GetText()
        {
            string r = "";
            Encoding encoding;
            StreamReader sr;
            if (Stream != null)
            {
                encoding = SM.FileEncoding(Stream);
                sr = new StreamReader(Stream, encoding);
                r = sr.ReadToEnd();
            }
            return r;
        }


        #endregion

        /* */

    }

    /* */

}
