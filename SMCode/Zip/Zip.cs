/*  ===========================================================================
 *  
 *  File:       Zip
 *  Version:    2.0.110
 *  Date:       December 2024
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2023 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *
 *  SMCode zip compression and decompression functions require third-party 
 *  DotNetZip 1.16.0 NuGet packet (Author: Henrik/Dino Chiesa).
 *
 *  ===========================================================================
 */

using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.IO;

namespace SMCodeSystem
{

    /* */

    /// <summary>SMCode zip compression and decompression functions require third-party DotNetZip 1.16.0 NuGet packet (Author: Henrik/Dino Chiesa).</summary>
    public partial class SMCode
    {

        /* */

        #region Declaration

        /*  ===================================================================
         *  Declaration
         *  ===================================================================
         */

        /// <summary>Zip progress pointer.</summary>
        private SMOnProgress zipProgressFunction;

        /// <summary>Internal zip error flag.</summary>
        private bool zipErrorFlag;

        #endregion

        /* */

        #region Methods

        /*  ===================================================================
         *  Methods
         *  ===================================================================
         */

        /// <summary>Initialize zip environment (no dependencies).</summary>
        public void Initialize_Zip()
        {
            zipProgressFunction = null;
            zipErrorFlag = false;
        }

        #endregion

        /* */

        #region Methods - Errors

        /*  ===================================================================
         *  Methods - Errors
         *  ===================================================================
         */

        /// <summary>Clear internal zip error variables.</summary>
        public void ZipErrorClear(ZipFile _ZipFile)
        {
            zipErrorFlag = false;
            if (_ZipFile != null)
            {
                _ZipFile.ZipError += ZipErrorHandler;
                _ZipFile.ZipErrorAction = ZipErrorAction.InvokeErrorEvent;
            }
        }

        /// <summary>Internal zip error handler.</summary>
        public void ZipErrorHandler(object _Sender, ZipErrorEventArgs _EventArgs)
        {
            zipErrorFlag = true;
            Error("An error occurred while compressing/decompressing file "
                + FileName(_EventArgs.FileName), _EventArgs.Exception);
        }

        #endregion

        /* */

        #region Methods - Compression

        /*  ===================================================================
         *  Methods - Compression
         *  ===================================================================
         */

        /// <summary>Compress data in byte arras as entry in to zip file. 
        /// Password and zip encryption are user for encryption.
        /// Zip progress function for updating progress bar or other information.
        /// Return true if succeed.</summary>
        public bool ZipBytes(string _ZipFileName, string _EntryName, byte[] _Data, string _PassWord,
            SMOnProgress _ZipProgressFunction, SMZipEncryption _ZipEncryption)
        {
            ZipFile zip;
            try
            {
                _PassWord = _PassWord.Trim();
                zip = new ZipFile();
                ZipErrorClear(zip);
                zip.ParallelDeflateThreshold = -1;
                if (_PassWord.Length > 0)
                {
                    zip.Password = _PassWord;
                    if (_ZipEncryption == SMZipEncryption.AES256) zip.Encryption = EncryptionAlgorithm.WinZipAes256;
                    else if (_ZipEncryption == SMZipEncryption.AES128) zip.Encryption = EncryptionAlgorithm.WinZipAes128;
                    else zip.Encryption = EncryptionAlgorithm.PkzipWeak;
                }
                else zip.Encryption = EncryptionAlgorithm.None;
                zip.AddEntry(_EntryName, _Data);
                if (_ZipProgressFunction != null)
                {
                    zipProgressFunction = _ZipProgressFunction;
                    zip.SaveProgress += ZipSaveProgress;
                }
                zip.Save(_ZipFileName);
                return !zipErrorFlag;
            }
            catch (Exception ex)
            {
                Error(ex);
                return false;
            }
        }

        /// <summary>Compress all files included in dirPath to zip file. 
        /// Password and zip encryption are user for encryption.
        /// Zip progress function for updating progress bar or other information.
        /// Return true if succeed.</summary>
        public bool ZipDir(string _ZipFileName, string _DirPath, string _Password,
            SMOnProgress _ZipProgressFunction, SMZipEncryption _ZipEncryption)
        {
            ZipFile zip;
            try
            {
                _Password=_Password.Trim();
                zip = new ZipFile();
                ZipErrorClear(zip);
                zip.ParallelDeflateThreshold = -1;
                if (_Password.Length > 0)
                {
                    zip.Password = _Password;
                    if (_ZipEncryption == SMZipEncryption.AES256) zip.Encryption = EncryptionAlgorithm.WinZipAes256;
                    else if (_ZipEncryption == SMZipEncryption.AES128) zip.Encryption = EncryptionAlgorithm.WinZipAes128;
                    else zip.Encryption = EncryptionAlgorithm.PkzipWeak;
                }
                else zip.Encryption = EncryptionAlgorithm.None;
                zip.AddDirectory(_DirPath, "");
                if (_ZipProgressFunction != null)
                {
                    zipProgressFunction = _ZipProgressFunction;
                    zip.SaveProgress += ZipSaveProgress;
                }
                zip.Save(_ZipFileName);
                return !zipErrorFlag;
            }
            catch (Exception ex)
            {
                Error(ex);
                return false;
            }
        }

        /// <summary>Compress file specified by file path to zip file. 
        /// Password and zip encryption are user for encryption.
        /// Zip progress function for updating progress bar or other information.
        /// Return true if succeed.</summary>
        public bool ZipFile(string _ZipFileName, string _FilePath, string _Password,
            SMOnProgress _ZipProgressFunction, SMZipEncryption _ZipEncryption)
        {
            ZipFile zip;
            try
            {
                _Password = _Password.Trim();
                zip = new ZipFile();
                ZipErrorClear(zip);
                zip.ParallelDeflateThreshold = -1;
                if (_Password.Length > 0)
                {
                    zip.Password = _Password;
                    if (_ZipEncryption == SMZipEncryption.AES256) zip.Encryption = EncryptionAlgorithm.WinZipAes256;
                    else if (_ZipEncryption == SMZipEncryption.AES128) zip.Encryption = EncryptionAlgorithm.WinZipAes128;
                    else zip.Encryption = EncryptionAlgorithm.PkzipWeak;
                }
                else zip.Encryption = EncryptionAlgorithm.None;
                zip.AddFile(_FilePath);
                if (_ZipProgressFunction != null)
                {
                    zipProgressFunction = _ZipProgressFunction;
                    zip.SaveProgress += ZipSaveProgress;
                }
                zip.Save(_ZipFileName);
                return !zipErrorFlag;
            }
            catch (Exception ex)
            {
                Error(ex);
                return false;
            }
        }

        /// <summary>Compress files included in files path collection (as string list or array list) to zip file. 
        /// Password and zip encryption are user for encryption.
        /// Zip progress function for updating progress bar or other information.
        /// Return true if succeed.</summary>
        public bool ZipFiles(string _ZipFileName, IEnumerable<string> _FilesPathCollection, string _Password,
            SMOnProgress _ZipProgressFunction, SMZipEncryption _ZipEncryption)
        {
            ZipFile zip;
            try
            {
                _Password = _Password.Trim();
                zip = new ZipFile();
                ZipErrorClear(zip);
                zip.ParallelDeflateThreshold = -1;
                if (_Password.Length > 0)
                {
                    zip.Password = _Password;
                    if (_ZipEncryption == SMZipEncryption.AES256) zip.Encryption = EncryptionAlgorithm.WinZipAes256;
                    else if (_ZipEncryption == SMZipEncryption.AES128) zip.Encryption = EncryptionAlgorithm.WinZipAes128;
                    else zip.Encryption = EncryptionAlgorithm.PkzipWeak;
                }
                else zip.Encryption = EncryptionAlgorithm.None;
                zip.AddFiles(_FilesPathCollection, "");
                if (_ZipProgressFunction != null)
                {
                    zipProgressFunction = _ZipProgressFunction;
                    zip.SaveProgress += ZipSaveProgress;
                }
                zip.Save(_ZipFileName);
                return !zipErrorFlag;
            }
            catch (Exception ex)
            {
                Error(ex);
                return false;
            }
        }

        /// <summary>Zip save progress event function.</summary>
        private void ZipSaveProgress(object _Sender, SaveProgressEventArgs _EventArgs)
        {
            bool b = false;
            if (zipProgressFunction != null)
            {
                zipProgressFunction(Percent(_EventArgs.BytesTransferred, _EventArgs.TotalBytesToTransfer), ref b);
                if (b) _EventArgs.Cancel = true;
            }
        }

        #endregion

        /* */

        #region Methods - Decompression

        /*  ===================================================================
         *  Methods - Decompression
         *  ===================================================================
         */

        /// <summary>Extract zip entry in to dir path with password if specified. Return true if succeed.</summary>
        public bool UnZipEntry(ZipEntry _ZipEntry, string _DirPath, string _Password)
        {
            if (_ZipEntry != null)
            {
                try
                {
                    FileDelete(Combine(_DirPath, _ZipEntry.FileName, "") + ".tmp");
                    FileDelete(Combine(_DirPath, _ZipEntry.FileName, ""));
                    if (_Password.Length > 0)
                    {
                        _ZipEntry.ExtractWithPassword(_DirPath, ExtractExistingFileAction.OverwriteSilently, _Password);
                    }
                    else
                    {
                        _ZipEntry.Extract(_DirPath, ExtractExistingFileAction.OverwriteSilently);
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    Error(ex);
                    return false;
                }
            }
            else return true;
        }

        /// <summary>Decompress entry specified by entry name from zip file 
        /// and store it in to data array. Password and zip encryption are user for 
        /// encryption. Zip progress function for updating progress bar or other 
        /// information. Return true if succeed.</summary>
        public bool UnZipBytes(string _ZipFileName, string _EntryName, ref byte[] _Data, 
            string _Password, SMOnProgress _ZipProgressFunction)
        {
            ZipFile zip;
            MemoryStream ms;
            try
            {
                _Password = _Password.Trim();
                zip = Ionic.Zip.ZipFile.Read(_ZipFileName);
                ZipErrorClear(zip);
                if (_ZipProgressFunction != null)
                {
                    zipProgressFunction = _ZipProgressFunction;
                    zip.ExtractProgress += UnZipSaveProgress;
                }
                ms = new MemoryStream();
                if (_Password.Length > 0) zip[_EntryName].ExtractWithPassword(ms, _Password);
                else zip[_EntryName].Extract(ms);
                _Data = ms.ToArray();
                ms.Close();
                ms.Dispose();
                return !zipErrorFlag;
            }
            catch (Exception ex)
            {
                Error(ex);
                return false;
            }
        }

        /// <summary>Decompress all files included in zip file to dir path. 
        /// Password and zip encryption are user for encryption.
        /// Zip progress function for updating progress bar or other information.
        /// Return true if succeed.</summary>
        public bool UnZipDir(string _ZipFileName, string _DirPath, 
            string _Password, SMOnProgress _ZipProgressFunction)
        {
            ZipFile zip;
            try
            {
                _Password = _Password.Trim();
                zip = Ionic.Zip.ZipFile.Read(_ZipFileName);
                ZipErrorClear(zip);
                if (_ZipProgressFunction != null)
                {
                    zipProgressFunction = _ZipProgressFunction;
                    zip.ExtractProgress += UnZipSaveProgress;
                }
                foreach (ZipEntry e in zip)
                {
                    if (!UnZipEntry(e, _DirPath, _Password)) zipErrorFlag = true;
                }
                return !zipErrorFlag;
            }
            catch (Exception ex)
            {
                Error(ex);
                return false;
            }
        }

        /// <summary>Decompress entry specified by entry name contained in zip file 
        /// to dir path. Password and zip encryption are user for encryption.
        /// Zip progress function for updating progress bar or other information.
        /// Return true if succeed.</summary>
        public bool UnZipFile(string _ZipFileName, string _EntryName, string _DirPath, 
            string _Password, SMOnProgress _ZipProgressFunction)
        {
            ZipFile zip;
            try
            {
                _Password = _Password.Trim();
                zip = Ionic.Zip.ZipFile.Read(_ZipFileName);
                ZipErrorClear(zip);
                if (_ZipProgressFunction != null)
                {
                    zipProgressFunction = _ZipProgressFunction;
                    zip.ExtractProgress += UnZipSaveProgress;
                }
                if (!UnZipEntry(zip[_EntryName], _DirPath, _Password)) zipErrorFlag = true;
                return !zipErrorFlag;
            }
            catch (Exception ex)
            {
                Error(ex);
                return false;
            }
        }

        /// <summary>Decompress entries specified by list entries name contained 
        /// in zip file to dir path. Password and zip encryption are user for encryption.
        /// Zip progress function for updating progress bar or other information.
        /// Return true if succeed.</summary>
        public bool UnZipFiles(string _ZipFileName, List<string> _EntriesName, string _DirPath, 
            string _PassWord, SMOnProgress _ZipProgressFunction)
        {
            ZipFile zip;
            if (_EntriesName != null)
            {
                try
                {
                    _PassWord = _PassWord.Trim();
                    zip = Ionic.Zip.ZipFile.Read(_ZipFileName);
                    ZipErrorClear(zip);
                    if (_ZipProgressFunction != null)
                    {
                        zipProgressFunction = _ZipProgressFunction;
                        zip.ExtractProgress += UnZipSaveProgress;
                    }
                    foreach (string e in _EntriesName)
                    {
                        if (!UnZipEntry(zip[e], _DirPath, _PassWord)) zipErrorFlag = true;
                    }
                    return !zipErrorFlag;
                }
                catch (Exception ex)
                {
                    Error(ex);
                    return false;
                }
            }
            else return false;
        }

        /// <summary>Unzip save progress event function.</summary>
        private void UnZipSaveProgress(object _Sender, ExtractProgressEventArgs _EventArgs)
        {
            bool b = false;
            if (zipProgressFunction != null)
            {
                zipProgressFunction(Percent(_EventArgs.BytesTransferred, _EventArgs.TotalBytesToTransfer), ref b);
                if (b) _EventArgs.Cancel = true;
            }
        }

        /// <summary>Decompress entry specified by entry name from zip stream 
        /// and store it to memory stream. Password and zip encryption are user for encryption
        /// Zip progress function for updating progress bar or other information.
        /// Return true if succeed.</summary>
        public MemoryStream UnZipStream(Stream _ZipStream, string _EntryName, 
            string _Password, SMOnProgress _ZipProgressFunction)
        {
            ZipFile zip;
            MemoryStream r = null;
            try
            {
                if ((_ZipStream != null) && (_EntryName.Trim().Length > 0))
                {
                    _Password = _Password.Trim();
                    zip = Ionic.Zip.ZipFile.Read(_ZipStream);
                    ZipErrorClear(zip);
                    if (_ZipProgressFunction != null)
                    {
                        zipProgressFunction = _ZipProgressFunction;
                        zip.ExtractProgress += UnZipSaveProgress;
                    }
                    if (zip.SelectEntries(_EntryName).Count > 0)
                    {
                        r = new MemoryStream();
                        if (_Password.Length > 0) zip[_EntryName].ExtractWithPassword(r, _Password);
                        else zip[_EntryName].Extract(r);
                        r.Position = 0;
                    }
                }
                return r;
            }
            catch (Exception ex)
            {
                Error(ex);
                return null;
            }
        }

        #endregion

        /* */

    }

    /* */

}
