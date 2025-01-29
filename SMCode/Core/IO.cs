/*  ===========================================================================
 *  
 *  File:       IO.cs
 *  Version:    2.0.140
 *  Date:       January 2025
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2025 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *
 *  SMCode application class: I/O.
 *
 *  ===========================================================================
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace SMCodeSystem
{

    /* */

    /// <summary>SMCode application class: I/O.</summary>
    public partial class SMCode
    {

        /* */

        #region Properties

        /*  ===================================================================
         *  Properties
         *  ===================================================================
         */

        /// <summary>Get or set file operations retries time.</summary>
        public int FileRetries { get; set; }

        /// <summary>Get or set file operations retries delay.</summary>
        public double FileRetriesDelay { get; set; }

        /// <summary>Get or set max load file size in bytes (default 16MB).</summary>
        public int MaxLoadFileSize { get; set; }

        #endregion

        /* */

        #region Initialization

        /*  ===================================================================
         *  Initialization
         *  ===================================================================
         */

        /// <summary>Initialize I/O class environment.</summary>
        public void InitializeIO()
        {
            FileRetries = 10;
            FileRetriesDelay = 1.0d;
            MaxLoadFileSize = 33554432;
        }

        #endregion

        /* */

        #region Methods

        /*  ===================================================================
         *  Methods
         *  ===================================================================
         */

        /// <summary>Append text string content to text file specified by file name.
        /// Returns true if succeed.</summary>
        public bool AppendString(string _FileName, string _Text, Encoding _Encoding = null, int _FileRetries = -1)
        {
            bool r = false, mr = false;
            StreamWriter sw;
            if (_FileRetries < 0) _FileRetries = FileRetries;
            if ((_FileRetries < 0) || (_FileRetries > 100)) _FileRetries = 1;
            while (!r && (_FileRetries > 0))
            {
                _FileRetries--;
                try
                {
                    if (_Encoding == null) _Encoding = FileEncoding(_FileName);
                    if (_Encoding != null)
                    {
                        sw = new StreamWriter(_FileName, true, _Encoding);
                        sw.Write(_Text);
                        sw.Close();
                        sw.Dispose();
                        r = true;
                    }
                }
                catch (Exception ex)
                {
                    Error(ex);
                    if (!mr) mr = MemoryRelease(true);
                    Wait(FileRetriesDelay, true);
                }
            }
            return r;
        }

        /// <summary>Copy file retrying if fail which path is specified in source file 
        /// to path target file eventually overwriting existing file with same name. 
        /// If not succeed, retry for specified times. Returns true if succeed.</summary>
        public bool FileCopy(string _SourceFile, string _TargetFile, int _FileRetries = -1)
        {
            bool r = false, mr = false;
            if (_FileRetries < 0) _FileRetries = FileRetries;
            if ((_FileRetries < 0) || (_FileRetries > 100)) _FileRetries = 1;
            while (!r && (_FileRetries > 0))
            {
                _FileRetries--;
                try
                {
                    File.Copy(_SourceFile, _TargetFile, true);
                    r = true;
                }
                catch (Exception ex)
                {
                    Error(ex);
                    if (!mr) mr = MemoryRelease(true);
                    Wait(FileRetriesDelay, true);
                }
            }
            return r;
        }

        /// <summary>Returns the datetime of file creation data.</summary>
        public DateTime FileCreated(string _FileName, int _FileRetries = -1)
        {
            bool mr = false;
            DateTime r = DateTime.MinValue;
            FileInfo fi;
            if (_FileRetries < 0) _FileRetries = FileRetries;
            if ((_FileRetries < 0) || (_FileRetries > 100)) _FileRetries = 1;
            while ((r == DateTime.MinValue) && (_FileRetries > 0))
            {
                _FileRetries--;
                try
                {
                    fi = new FileInfo(_FileName);
                    r = fi.CreationTime;
                }
                catch (Exception ex)
                {
                    Error(ex);
                    if (!mr) mr = MemoryRelease(true);
                    Wait(FileRetriesDelay, true);
                }
            }
            return r;
        }

        /// <summary>Returns the datetime of last changes about file located in file name path.</summary>
        public DateTime FileDate(string _FileName, int _FileRetries = -1)
        {
            bool mr = false;
            DateTime r = DateTime.MinValue;
            FileInfo fi;
            if (_FileRetries < 0) _FileRetries = FileRetries;
            if ((_FileRetries < 0) || (_FileRetries > 100)) _FileRetries = 1;
            while ((r == DateTime.MinValue) && (_FileRetries > 0))
            {
                _FileRetries--;
                try
                {
                    fi = new FileInfo(_FileName);
                    r = fi.LastWriteTime;
                }
                catch (Exception ex)
                {
                    Error(ex);
                    if (!mr) mr = MemoryRelease(true);
                    Wait(FileRetriesDelay, true);
                }
            }
            return r;
        }

        /// <summary>Set the datetime of last changes about file located in file name path. 
        /// Return true if succeed.</summary>
        public bool FileDate(string _FileName, DateTime _DateTime, int _FileRetries = -1)
        {
            bool r = false, mr = false;
            if (_FileRetries < 0) _FileRetries = FileRetries;
            if ((_FileRetries < 0) || (_FileRetries > 100)) _FileRetries = 1;
            while (!r && (_FileRetries > 0))
            {
                _FileRetries--;
                try
                {
                    File.SetLastWriteTime(_FileName, _DateTime);
                    r = true;
                }
                catch (Exception ex)
                {
                    Error(ex);
                    if (!mr) mr = MemoryRelease(true);
                    Wait(FileRetriesDelay, true);
                }
            }
            return r;
        }

        /// <summary>Delete file specified retrying for settings planned times. Return true if succeed.</summary>
        public bool FileDelete(string _FileName, int _FileRetries = -1)
        {
            bool r = false, mr = false;
            if (_FileRetries < 0) _FileRetries = FileRetries;
            if ((_FileRetries < 0) || (_FileRetries > 100)) _FileRetries = 1;
            while (!r && (_FileRetries > 0))
            {
                _FileRetries--;
                try
                {
                    if (File.Exists(_FileName)) File.Delete(_FileName);
                    r = true;
                }
                catch (Exception ex)
                {
                    Error(ex);
                    if (!mr) mr = MemoryRelease(true);
                    Wait(FileRetriesDelay, true);
                }
            }
            return r;
        }

        /// <summary>Returns the extension of file path file without starting comma.</summary>
        public string FileExtension(string _FilePath)
        {
            string r;
            try
            {
                r = Path.GetExtension(_FilePath);
                if (r != "")
                {
                    if (r[0] == '.')
                    {
                        if (r.Length > 1) r = r.Substring(1);
                        else r = "";
                    }
                }
                return r;
            }
            catch (Exception ex)
            {
                Error(ex);
                return "";
            }
        }

        /// <summary>Determines a text file's encoding by analyzing its byte order mark (BOM).
        /// Defaults to ASCII when detection of the text file's endianness fails.</summary>
        public Encoding FileEncoding(string _FileName, int _FileRetries = -1)
        {
            bool mr = false;
            byte[] bom = new byte[4] { 0x00, 0x00, 0x00, 0x00 };
            Encoding r = null;
            FileStream fileStream;
            if (_FileRetries < 0) _FileRetries = FileRetries;
            if ((_FileRetries < 0) || (_FileRetries > 100)) _FileRetries = 1;
            while ((r == null) && (_FileRetries > 0))
            {
                _FileRetries--;
                try
                {
                    if (File.Exists(_FileName))
                    {
                        fileStream = new FileStream(_FileName, FileMode.Open, FileAccess.Read);
                        fileStream.Read(bom, 0, 4);
                        fileStream.Close();
                        fileStream.Dispose();
                        if ((bom[0] == 0x2b) && (bom[1] == 0x2f) && (bom[2] == 0x76)) r = Encoding.UTF7;
                        else if ((bom[0] == 0xef) && (bom[1] == 0xbb) && (bom[2] == 0xbf)) r = Encoding.UTF8;
                        else if ((bom[0] == 0xff) && (bom[1] == 0xfe)) r = Encoding.Unicode; //UTF-16LE
                        else if ((bom[0] == 0xfe) && (bom[1] == 0xff)) r = Encoding.BigEndianUnicode; //UTF-16BE
                        else if ((bom[0] == 0) && (bom[1] == 0) && (bom[2] == 0xfe) && (bom[3] == 0xff)) r = Encoding.UTF32;
                        else r = Encoding.Default;
                    }
                    else r = TextEncoding;
                }
                catch (Exception ex)
                {
                    Error(ex);
                    if (!mr) mr = MemoryRelease(true);
                    Wait(FileRetriesDelay, true);
                }
            }
            return r;
        }

        /// <summary>Determines a text stream encoding by analyzing its byte order mark (BOM).
        /// Defaults to ASCII when detection of the text file's endianness fails.</summary>
        public Encoding FileEncoding(Stream _TextStream)
        {
            byte[] bom = new byte[4] { 0x00, 0x00, 0x00, 0x00 };
            try
            {
                if (_TextStream != null)
                {
                    _TextStream.Read(bom, 0, 4);
                    _TextStream.Seek(0, SeekOrigin.Begin);
                    if ((bom[0] == 0x2b) && (bom[1] == 0x2f) && (bom[2] == 0x76)) return Encoding.UTF7;
                    else if ((bom[0] == 0xef) && (bom[1] == 0xbb) && (bom[2] == 0xbf)) return Encoding.UTF8;
                    else if ((bom[0] == 0xff) && (bom[1] == 0xfe)) return Encoding.Unicode; //UTF-16LE
                    else if ((bom[0] == 0xfe) && (bom[1] == 0xff)) return Encoding.BigEndianUnicode; //UTF-16BE
                    else if ((bom[0] == 0) && (bom[1] == 0) && (bom[2] == 0xfe) && (bom[3] == 0xff)) return Encoding.UTF32;
                    else return Encoding.ASCII;
                }
                else return TextEncoding;
            }
            catch (Exception ex)
            {
                Error(ex);
                return null;
            }
        }

        /// <summary>Return true if file specified exists.</summary>
        public bool FileExists(string _FileName, int _FileRetries = -1)
        {
            bool lp = true, mr = false, rslt = false;
            if (!Empty(_FileName))
            {
                if (_FileRetries < 0) _FileRetries = FileRetries;
                if ((_FileRetries < 0) || (_FileRetries > 100)) _FileRetries = 1;
                while (lp && (_FileRetries > 0))
                {
                    _FileRetries--;
                    try
                    {
                        rslt = File.Exists(_FileName);
                        lp = false;
                    }
                    catch (Exception ex)
                    {
                        Error(ex);
                        if (!mr) mr = MemoryRelease(true);
                        Wait(FileRetriesDelay, true);
                    }
                }
            }
            return rslt;
        }

        /// <summary>Find simple history backup file name with - Copy (#) suffix.</summary>
        public string FileHistory(string _FileName)
        {
            int i = 0;
            string p = FilePath(_FileName), f = FileNameWithoutExt(_FileName), e = FileExtension(_FileName), c = "Copy";
            if (language.Trim().ToLower() == "it") c = "Copia";
            while (FileExists(_FileName) && (i < 99999))
            {
                i++;
                _FileName = Combine(p, f + Iif(i > 1, " - " + c + " (" + i.ToString() + ")", " - " + c), e);
            }
            return _FileName;
        }

        /// <summary>Create history backup of file specified, mantaining a maximum of files.</summary>
        public bool FileHistory(string _FileName, int _MaximumFiles)
        {
            if (FileExists(_FileName))
            {
                FileHistoryWipe(_FileName, _MaximumFiles);
                return FileCopy(_FileName, FileTimeStamp(_FileName, DateTime.Now));
            }
            else return false;
        }

        /// <summary>Wipe history backup of file specified mantaining a maximum-1 of files.</summary>
        public void FileHistoryWipe(string _FileName, int _MaximumFiles = 16)
        {
            string p;
            List<string> l = FileTimeStampList(_FileName);
            if (l.Count > 0)
            {
                l.Sort();
                _MaximumFiles--;
                p = FilePath(_FileName);
                while (_MaximumFiles < l.Count)
                {
                    FilesDelete(Combine(p, l[0], ""));
                    l.RemoveAt(0);
                }
            }
        }

        /// <summary>Returns FileInfo array of all file matches file path or null if function fails.</summary>
        public FileInfo[] FileList(string _Path, bool _Recursive = false)
        {
            int i;
            string p, k;
            DirectoryInfo di;
            DirectoryInfo[] dr;
            List<FileInfo> fi = null;
            try
            {
                fi = new List<FileInfo>();
                k = FileName(_Path);
                p = FilePath(_Path);
                if (FolderExists(p))
                {
                    di = new DirectoryInfo(p);
                    fi.AddRange(di.GetFiles(k));
                    if (_Recursive)
                    {
                        dr = di.GetDirectories();
                        if (dr != null)
                        {
                            for (i = 0; i < dr.Length; i++)
                            {
                                fi.AddRange(FileList(Combine(dr[i].FullName, k, ""), true));
                            }
                        }
                    }
                    return fi.ToArray();
                }
                else return null;
            }
            catch (Exception ex)
            {
                Error(ex);
                return null;
            }
        }

        /// <summary>Returns string list of all file names (or full path if specified) 
        /// matching file path or null if function fails.</summary>
        public List<string> FileList(string _Path, bool _StoreFullPath, bool _Recursive)
        {
            List<string> sl = new List<string>();
            FileInfo[] fi = FileList(_Path, _Recursive);
            if (fi != null)
            {
                int i, h = fi.Length;
                if (_StoreFullPath)
                {
                    for (i = 0; i < h; i++) sl.Add(fi[i].FullName);
                }
                else for (i = 0; i < h; i++) sl.Add(fi[i].Name);
            }
            return sl;
        }

        /// <summary>Returns array of bytes with file content (see MaxLoadFileSize).</summary>
        public byte[] FileLoad(string _FileName, int _FileRetries = -1)
        {
            bool mr = false;
            byte[] r = null;
            FileStream fs;
            BinaryReader br;
            if (FileExists(_FileName))
            {
                if (FileSize(_FileName) > MaxLoadFileSize)
                {
                    Raise("File exceed maximum load size (" + MaxLoadFileSize.ToString() + ")", false);
                }
                else
                {
                    if (_FileRetries < 0) _FileRetries = FileRetries;
                    if ((_FileRetries < 0) || (_FileRetries > 100)) _FileRetries = 1;
                    while ((r == null) && (_FileRetries > 0))
                    {
                        _FileRetries--;
                        try
                        {
                            fs = new FileStream(_FileName, FileMode.Open);
                            br = new BinaryReader(fs);
                            r = br.ReadBytes(MaxLoadFileSize);
                            br.Close();
                            fs.Close();
                            fs.Dispose();
                        }
                        catch (Exception ex)
                        {
                            Error(ex);
                            if (!mr) mr = MemoryRelease(true);
                            Wait(FileRetriesDelay, true);
                        }
                    }
                }
            }
            return r;
        }

        /// <summary>Try to move the file located in to old file path in to new file path 
        /// retrying for passed times. Returns true if succeed.</summary>
        public bool FileMove(string _OldFile, string _NewFile, int _FileRetries = -1)
        {
            bool r = false, mr = false;
            if (_FileRetries < 0) _FileRetries = FileRetries;
            if ((_FileRetries < 0) || (_FileRetries > 100)) _FileRetries = 1;
            while (!r && (_FileRetries > 0))
            {
                _FileRetries--;
                try
                {
                    if (FileExists(_OldFile))
                    {
                        FileDelete(_NewFile);
                        File.Move(_OldFile, _NewFile);
                        r = true;
                    }
                    else return false;
                }
                catch (Exception ex)
                {
                    Error(ex);
                    if (!mr) mr = MemoryRelease(true);
                    Wait(FileRetriesDelay, true);
                }
            }
            return r;
        }

        /// <summary>Returns the name of file specified in file path, with extension.</summary>
        public string FileName(string _FullPath)
        {
            try
            {
                if (_FullPath.Trim().Length > 0) return System.IO.Path.GetFileName(_FullPath);
                else return "";
            }
            catch (Exception ex)
            {
                Error(ex);
                return "";
            }
        }

        /// <summary>Returns the name of file specified in file path, without extension.</summary>
        public string FileNameWithoutExt(string _FullPath)
        {
            try
            {
                if (_FullPath.Trim().Length > 0) return System.IO.Path.GetFileNameWithoutExtension(_FullPath);
                else return "";
            }
            catch (Exception ex)
            {
                Error(ex);
                return "";
            }
        }

        /// <summary>Returns directory path of file specified in full file path.</summary>
        public string FilePath(string _FullPath)
        {
            try
            {
                if (_FullPath.Trim().Length > 0) return System.IO.Path.GetDirectoryName(_FullPath);
                else return "";
            }
            catch (Exception ex)
            {
                Error(ex);
                return "";
            }
        }

        /// <summary>Delete files that matches the file path.
        /// Returns true if succeed.</summary>
        public bool FilesDelete(string _Path)
        {
            bool b = false;
            FileInfo[] fi;
            try
            {
                if (_Path.Trim().Length > 0)
                {
                    fi = FileList(_Path);
                    if (fi != null)
                    {
                        b = true;
                        for (int i = 0; i < fi.Length; i++)
                        {
                            if (!FileDelete(fi[i].FullName)) b = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Error(ex);
                b = false;
            }
            return b;
        }

        /// <summary>Returns the size of file specified or -1 if does not exists.</summary>
        public long FileSize(string _FileName, int _FileRetries = -1)
        {
            long r = -1;
            bool mr = false;
            FileInfo fi;
            if (_FileName.Trim().Length > 0)
            {
                if (_FileRetries < 0) _FileRetries = FileRetries;
                if ((_FileRetries < 0) || (_FileRetries > 100)) _FileRetries = 1;
                while ((r < 0) && (_FileRetries > 0))
                {
                    _FileRetries--;
                    try
                    {
                        fi = new FileInfo(_FileName);
                        r = fi.Length;
                    }
                    catch (Exception ex)
                    {
                        Error(ex);
                        if (!mr) mr = MemoryRelease(true);
                        Wait(FileRetriesDelay, true);
                    }
                }
            }
            return r;
        }

        /// <summary>Return a string containing a timestamp with format 
        /// "yyyy-MM-dd_hhmmss_fff" suitable for file naming usage.</summary>
        public string FileTimeStamp(DateTime _DateTime)
        {
            return _DateTime.ToString("yyyy-MM-dd_HHmmss_fff");
        }

        /// <summary>Return a string containing file name full path with timestamp of datetime passed.</summary>
        public string FileTimeStamp(string _FileName, DateTime _DateTime)
        {
            return Combine(
                FilePath(_FileName),
                FileNameWithoutExt(_FileName) + " (" + FileTimeStamp(_DateTime) + ")",
                FileExtension(_FileName)
                );
        }

        /// <summary>Return a string containing file name full path with timestamp of last modified datetime.</summary>
        public string FileTimeStamp(string _FileName)
        {
            return FileTimeStamp(_FileName, FileDate(_FileName));
        }

        /// <summary>Return a list of history files (with timestamp) with same file name.</summary>
        public List<string> FileTimeStampList(string _FileName)
        {
            int i = 0;
            string s;
            List<string> l = FileList(Combine(FilePath(_FileName), FileNameWithoutExt(_FileName) + " (*)", FileExtension(_FileName)), false, false);
            while (i < l.Count)
            {
                s = FileNameWithoutExt(l[i]);
                if (s.Length > 24)
                {
                    s = s.Substring(s.Length - 23, 23);
                    if ((s[0] == '(') && (s[5] == '-') && (s[8] == '-')
                        && (s[11] == '_') && (s[18] == '_') && (s[22] == ')')) i++;
                    else l.RemoveAt(i);
                }
                else l.RemoveAt(i);
            }
            if (l.Count > 1) l.Sort();
            return l;
        }

        /// <summary>Return first existing file name of specified in array or default if none exists.</summary>
        public string FirstExists(string[] _FileNames, string _DefaultFileName)
        {
            int i = 0;
            string r = "";
            if (_FileNames != null)
            {
                while ((r == "") && (i < _FileNames.Length))
                {
                    if (FileExists(_FileNames[i])) r = _FileNames[i];
                    i++;
                }
            } 
            if (r == "") return _DefaultFileName;
            else return r;
        }

        /// <summary>Returns true if folder path exists.</summary>
        public bool FolderExists(string _Path, int _FileRetries=-1)
        {
            bool lp = true, mr = false, r = false;
            if (_Path.Trim().Length > 0)
            {
                if (_FileRetries < 0) _FileRetries = FileRetries;
                if ((_FileRetries < 0) || (_FileRetries > 100)) _FileRetries = 1;
                while (lp && (_FileRetries > 0))
                {
                    _FileRetries--;
                    try
                    {
                        r = System.IO.Directory.Exists(_Path);
                        lp = false;
                    }
                    catch (Exception ex)
                    {
                        Error(ex);
                        if (!mr) mr = MemoryRelease(true);
                        Wait(FileRetriesDelay, true);
                    }
                }
            }
            return r;
        }

        /// <summary>Returns DirectoryInfo array of all folders 
        /// matches path or null if function fails.</summary>
        public DirectoryInfo[] FolderList(string _Path, int _FileRetries = -1)
        {
            bool mr = false;
            DirectoryInfo di;
            DirectoryInfo[] r = null;
            if (_Path.Trim().Length > 0)
            {
                if (_FileRetries < 0) _FileRetries = FileRetries;
                if ((_FileRetries < 0) || (_FileRetries > 100)) _FileRetries = 1;
                while ((r == null) && (_FileRetries > 0))
                {
                    _FileRetries--;
                    try
                    {
                        di = new DirectoryInfo(FilePath(_Path));
                        r = di.GetDirectories(FileName(_Path));
                    }
                    catch (Exception ex)
                    {
                        Error(ex);
                        if (!mr) mr = MemoryRelease(true);
                        Wait(FileRetriesDelay, true);
                    }
                }
            }
            return r;
        }

        /// <summary>Create folders included in file path if does not exists.
        /// Returns true if succeed.</summary>
        public bool ForceFolders(string _Path, int _FileRetries = -1)
        {
            bool r = false, mr = false;
            if (_Path.Trim().Length > 0)
            {
                if (_FileRetries < 0) _FileRetries = FileRetries;
                if ((_FileRetries < 0) || (_FileRetries > 100)) _FileRetries = 1;
                while (!r && (_FileRetries > 0))
                {
                    _FileRetries--;
                    try
                    {
                        System.IO.Directory.CreateDirectory(_Path);
                        r = true;
                    }
                    catch (Exception ex)
                    {
                        Error(ex);
                        if (!mr) mr = MemoryRelease(true);
                        Wait(FileRetriesDelay, true);
                    }
                }
            }
            return r;
        }

        /// <summary>Return path and try to create if not exists. Return always passed path.</summary>
        public string ForcePath(string _Path)
        {
            if (!FolderExists(_Path)) ForceFolders(_Path);
            return _Path;
        }

        /// <summary>Returns a string containing all chars of text file with encoding.</summary>
        public string LoadString(string _FileName, Encoding _TextEncoding = null, int _FileRetries = -1)
        {
            string r = "";
            bool lp = true, mr = false;
            if (_FileName.Trim().Length > 0)
            {
                if (FileExists(_FileName))
                {
                    if (_FileRetries < 0) _FileRetries = FileRetries;
                    if ((_FileRetries < 0) || (_FileRetries > 100)) _FileRetries = 1;
                    if (_TextEncoding == null) _TextEncoding = TextEncoding;
                    while (lp && (_FileRetries > 0))
                    {
                        _FileRetries--;
                        try
                        {
                            r = File.ReadAllText(_FileName, _TextEncoding);
                            lp = false;
                        }
                        catch (Exception ex)
                        {
                            Error(ex);
                            if (!mr) mr = MemoryRelease(true);
                            Wait(FileRetriesDelay, true);
                        }
                    }
                }
            }
            return r;
        }

        /// <summary>Load sl string list with lines of text file fileName.
        /// Returns true if succeed.</summary>
        public bool LoadString(string _FileName, List<string> _StringList, bool _Append, Encoding _TextEncoding = null, int _FileRetries = -1)
        {
            bool r = false, mr = false;
            StreamReader sr;
            if ((_FileName.Trim().Length > 0) && (_StringList != null))
            {
                if (!_Append) _StringList.Clear();
                if (FileExists(_FileName))
                {
                    if (_FileRetries < 0) _FileRetries = FileRetries;
                    if ((_FileRetries < 0) || (_FileRetries > 100)) _FileRetries = 1;
                    if (_TextEncoding == null) _TextEncoding = TextEncoding;
                    while (!r && (_FileRetries > 0))
                    {
                        _FileRetries--;
                        try
                        {
                            sr = new StreamReader(_FileName, _TextEncoding);
                            while (!sr.EndOfStream) _StringList.Add(sr.ReadLine());
                            sr.Close();
                            sr.Dispose();
                            r = true;
                        }
                        catch (Exception ex)
                        {
                            Error(ex);
                            if (!mr) mr = MemoryRelease(true);
                            Wait(FileRetriesDelay, true);
                        }
                    }
                }
            }
            return r;
        }

        /// <summary>Return MIME type of file passed.</summary>
        public string MimeType(string _FileName)
        {
            string ext = Path.GetExtension(_FileName.ToLower());
            if (ext == "pdf") return "application/pdf";
            else if (ext == "jpg") return "image/jpeg";
            else if (ext == "png") return "image/png";
            else if (ext == "gif") return "image/gif";
            else if (ext == "bmp") return "image/bmp";
            else if (ext == "ico") return "image/ico";
            else if (ext == "tif") return "image/tiff";
            else if (ext == "tiff") return "image/tiff";
            else if (ext == "svg") return "image/svg+xml";
            else if (ext == "webp") return "image/webp";
            else if (ext == "txt") return "text/plain";
            else if (ext == "csv") return "text/csv";
            else if (ext == "css") return "text/css";
            else if (ext == "htm") return "text/html";
            else if (ext == "html") return "text/html";
            else if (ext == "js") return "text/javascript";
            else if (ext == "mp4") return "video/mp4";
            else return "application/octet-stream";
        }

        /// <summary>Move directory indicate by dir path and all subdirs to a new path (no retries). 
        /// Returns true if succeed.</summary>
        public bool MoveFolder(string _FolderPath, string _NewPath, int _FileRetries = -1)
        {
            bool r = false, mr = false;
            if (_FolderPath.Trim().Length > 0)
            {
                if (_FileRetries < 0) _FileRetries = FileRetries;
                if ((_FileRetries < 0) || (_FileRetries > 100)) _FileRetries = 1;
                while (!r && (_FileRetries > 0))
                {
                    _FileRetries--;
                    try
                    {
                        System.IO.Directory.Move(_FolderPath, _NewPath);
                        r = true;
                    }
                    catch (Exception ex)
                    {
                        Error(ex);
                        if (!mr) mr = MemoryRelease(true);
                        Wait(FileRetriesDelay, true);
                    }
                }
            }
            return r;
        }

        /// <summary>Se si è in modalità di debug scrive il messaggio passato
        /// sulla finestra di output.</summary>
        public void Output(string _Message)
        {
            if (IsDebugger()) Debug.WriteLine(_Message);
        }

        /// <summary>Remove directory indicate by dir path and all subdirs (no retries). 
        /// Returns true if succeed.</summary>
        public bool RemoveFolder(string _FolderPath, int _FileRetries = -1)
        {
            bool r = false, mr = false;
            if (_FolderPath.Trim().Length > 0)
            {
                if (_FileRetries < 0) _FileRetries = FileRetries;
                if ((_FileRetries < 0) || (_FileRetries > 100)) _FileRetries = 1;
                while (!r && (_FileRetries > 0))
                {
                    _FileRetries--;
                    try
                    {
                        System.IO.Directory.Delete(_FolderPath, true);
                        r = true;
                    }
                    catch (Exception ex)
                    {
                        Error(ex);
                        if (!mr) mr = MemoryRelease(true);
                        Wait(FileRetriesDelay, true);
                    }
                }
            }
            return r;
        }

        /// <summary>Save text string content in the text file specified retrying for specified times.
        /// Returns true if succeed.</summary>
        public bool SaveString(string _FileName, string _Text, Encoding _TextEncoding = null, int _FileRetries = -1)
        {
            bool r = false, mr = false;
            if (_FileName.Trim().Length > 0)
            {
                if (_FileRetries < 0) _FileRetries = FileRetries;
                if ((_FileRetries < 0) || (_FileRetries > 100)) _FileRetries = 1;
                if (_TextEncoding == null) _TextEncoding = TextEncoding;
                while (!r && (_FileRetries > 0))
                {
                    _FileRetries--;
                    try
                    {
                        File.WriteAllText(_FileName, _Text, _TextEncoding);
                        r = true;
                    }
                    catch (Exception ex)
                    {
                        Error(ex);
                        if (!mr) mr = MemoryRelease(true);
                        Wait(FileRetriesDelay, true);
                    }
                }
            }
            return r;
        }

        /* revised like here */

        /// <summary>Save string list in the text file specified by file name
        /// with specified text encoding. Returns true if succeed.</summary>
        public bool SaveString(string _FileName, List<string> _StringList, Encoding _TextEncoding = null, int _FileRetries = -1)
        {
            int i = 0;
            bool r = false, mr = false;
            StreamWriter sw;
            if ((_FileName.Trim().Length > 0) && (_StringList != null))
            {
                if (_FileRetries < 0) _FileRetries = FileRetries;
                if ((_FileRetries < 0) || (_FileRetries > 100)) _FileRetries = 1;
                if (_TextEncoding == null) _TextEncoding = TextEncoding;
                while (!r && (_FileRetries > 0))
                {
                    _FileRetries--;
                    try
                    {
                        sw = new StreamWriter(_FileName, false, _TextEncoding);
                        sw.NewLine = CR;
                        while (i < _StringList.Count)
                        {
                            sw.WriteLine(_StringList[i]);
                            i++;
                        }
                        sw.Close();
                        sw.Dispose();
                        r = true;
                    }
                    catch (Exception ex)
                    {
                        Error(ex);
                        if (!mr) mr = MemoryRelease(true);
                        Wait(FileRetriesDelay, true);
                    }
                }
            }
            return r;
        }

        /// <summary>Delete on temp path temporary files matches ~*.*</summary>
        public bool WipeTemp()
        {
            return FilesDelete(Combine(TempPath, "~*.*", ""));
        }

        #endregion

        /* */

    }

    /* */

}
