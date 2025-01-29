/*  ===========================================================================
 *  
 *  File:       Hash.cs
 *  Version:    2.0.140
 *  Date:       January 2025
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2025 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *
 *  SMCode application class: hash.
 *  
 *  ===========================================================================
 */

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace SMCodeSystem
{

    /* */

    /// <summary>SMCode application class: hash.</summary>
    public partial class SMCode
    {

        /* */

        #region Methods

        /*  ===================================================================
         *  Methods
         *  ===================================================================
         */

        /// <summary>Return MD5 hash about bytes array.</summary>
        public string HashMD5(byte[] _Bytes)
        {
            int i;
            byte[] data;
            MD5 md5;
            StringBuilder sb;
            try
            {
                sb = new StringBuilder();
                md5 = MD5.Create();
                data = md5.ComputeHash(_Bytes);
                for (i = 0; i < data.Length; i++) sb.Append(data[i].ToString("x2"));
                return sb.ToString();
            }
            catch (Exception ex)
            {
                Error(ex);
                return "";
            }
        }

        /// <summary>Return MD5 hash about string.</summary>
        public string HashMD5(string _Value)
        {
            byte[] data;
            try
            {
                data = Encoding.UTF8.GetBytes(_Value);
                return HashMD5(data);
            }
            catch (Exception ex)
            {
                Error(ex);
                return "";
            }
        }

        /// <summary>Return MD5 hash about stream.</summary>
        public string HashMD5(Stream _Stream)
        {
            MemoryStream memory;
            try
            {
                memory = new MemoryStream();
                _Stream.CopyTo(memory);
                return HashMD5(memory.ToArray());
            }
            catch (Exception ex)
            {
                Error(ex);
                return "";
            }
        }

        /// <summary>Return MD5 hash about file.</summary>
        public string HashMD5File(string _FileName, int _FileRetries = -1)
        {
            bool lp = true, mr = false;
            string r = "";
            if (_FileRetries < 0) _FileRetries = FileRetries;
            if ((_FileRetries < 0) || (_FileRetries > 100)) _FileRetries = 1;
            while (lp && (_FileRetries > 0))
            {
                _FileRetries--;
                try
                {
                    r = HashMD5(File.ReadAllBytes(_FileName));
                    lp = false;
                }
                catch (Exception ex)
                {
                    r = "";
                    Error(ex);
                    if (!mr) mr = MemoryRelease(true);
                    Wait(FileRetriesDelay, true);
                }
            }
            return r;
        }

        /// <summary>Return SHA256 hash about bytes array.</summary>
        public string HashSHA256(byte[] _Bytes)
        {
            int i;
            byte[] data;
            SHA256 sha;
            StringBuilder sb;
            try
            {
                sb = new StringBuilder();
                sha = SHA256.Create();
                data = sha.ComputeHash(_Bytes);
                for (i = 0; i < data.Length; i++) sb.Append(data[i].ToString("x2"));
                return sb.ToString();
            }
            catch (Exception ex)
            {
                Error(ex);
                return "";
            }
        }

        /// <summary>Return SHA256 hash about string.</summary>
        public string HashSHA256(string _Value)
        {
            byte[] data;
            try
            {
                data = Encoding.UTF8.GetBytes(_Value);
                return HashSHA256(data);
            }
            catch (Exception ex)
            {
                Error(ex);
                return "";
            }
        }

        /// <summary>Return SHA256 hash about stream.</summary>
        public string HashSHA256(Stream _Stream)
        {
            MemoryStream memory;
            try
            {
                memory = new MemoryStream();
                _Stream.CopyTo(memory);
                return HashSHA256(memory.ToArray());
            }
            catch (Exception ex)
            {
                Error(ex);
                return "";
            }
        }

        /// <summary>Return SHA256 hash about file.</summary>
        public string HashSHA256File(string _FileName, int _FileRetries = -1)
        {
            bool lp = true, mr = false;
            string r = "";
            if (_FileRetries < 0) _FileRetries = FileRetries;
            if ((_FileRetries < 0) || (_FileRetries > 100)) _FileRetries = 1;
            while (lp && (_FileRetries > 0))
            {
                _FileRetries--;
                try
                {
                    r = HashSHA256(File.ReadAllBytes(_FileName));
                    lp = false;
                }
                catch (Exception ex)
                {
                    r = "";
                    Error(ex);
                    if (!mr) mr = MemoryRelease(true);
                    Wait(FileRetriesDelay, true);
                }
            }
            return r;
        }

        #endregion

        /* */

    }

    /* */

}
