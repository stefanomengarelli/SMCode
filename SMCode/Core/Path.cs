/*  ===========================================================================
 *  
 *  File:       Path.cs
 *  Version:    2.0.262
 *  Date:       May 2025
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2025 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *
 *  SMCode application class: path.
 *
 *  ===========================================================================
 */

using System;
using System.Collections.Generic;

namespace SMCodeSystem
{

    /* */

    /// <summary>SMCode application class: path.</summary>
    public partial class SMCode
    {

        /* */

        #region Declarations

        /*  ===================================================================
         *  Declarations
         *  ===================================================================
         */

        /// <summary>Application path.</summary>
        private string applicationPath = "";

        #endregion

        /* */

        #region Properties

        /*  ===================================================================
         *  Properties
         *  ===================================================================
         */

        /// <summary>Get or set application path.</summary>
        public string ApplicationPath
        {
            get { return applicationPath; }
            set
            {
                applicationPath = value;
                Repath();
            }
        }

        /// <summary>Get or set system paths auto creation flag.</summary>
        public virtual bool AutoCreatePath { get; set; } = true;

        /// <summary>Get or set common path.</summary>
        public string CommonPath { get; set; }

        /// <summary>Get or set data path.</summary>
        public string DataPath { get; set; }

        /// <summary>Get or set desktop path.</summary>
        public string DesktopPath { get; set; }

        /// <summary>Get or set documents path.</summary>
        public string DocumentsPath { get; set; }

        /// <summary>Get or set executable datetime.</summary>
        public DateTime ExecutableDate { get; set; }

        /// <summary>Get or set executable name without extension.</summary>
        public string ExecutableName { get; set; }

        /// <summary>Get or set executable path.</summary>
        public string ExecutablePath { get; set; }

        /// <summary>Get or set temporary path.</summary>
        public string TempPath { get; set; } = "";

        /// <summary>Get or set static root path.</summary>
        public static string RootPath { get; set; } = "";

        /// <summary>Get or set user personal path.</summary>
        public string UserPath { get; set; } = "";

        /// <summary>Get or set user documents path.</summary>
        public string UserDocumentsPath { get; set; }

        /// <summary>Get or set application version.</summary>
        public string Version { get; set; } = "";


        #endregion

        /* */

        #region Initialization

        /*  ===================================================================
         *  Initialization
         *  ===================================================================
         */

        /// <summary>Initialize path class environment.</summary>
        public void InitializePath()
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetEntryAssembly();
            System.Diagnostics.FileVersionInfo fileVersionInfo;
            try
            {
                ExecutableName = "";
                ExecutablePath = "";
                Version = "";
                if (assembly != null)
                {
                    fileVersionInfo = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location);
                    ExecutableName = assembly.GetName().Name;
                    ExecutablePath = FixPath(FilePath(assembly.Location));
                    Version = fileVersionInfo.FileVersion;
                    ExecutableDate = FileDate(assembly.Location);
                }
                CommonPath = FixPath(System.Environment.GetFolderPath(System.Environment.SpecialFolder.CommonApplicationData));
                DesktopPath = FixPath(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop));
                DocumentsPath = FixPath(System.Environment.GetFolderPath(System.Environment.SpecialFolder.CommonDocuments));
                UserPath = FixPath(System.Environment.GetFolderPath(System.Environment.SpecialFolder.UserProfile));
                UserDocumentsPath = FixPath(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments));
                Repath();
            }
            catch (Exception ex)
            {
                Error(ex);
            }
        }

        #endregion

        /* */

        #region Methods

        /*  ===================================================================
         *  Methods
         *  ===================================================================
         */

        /// <summary>Returns the full file path changing extension in new extension.</summary>
        public string ChangeExtension(string _FilePath, string _NewExtension)
        {
            return Combine(FilePath(_FilePath), FileNameWithoutExt(_FilePath), _NewExtension);
        }

        /// <summary>Returns a string containing full path of file name, in file path directory and with file extension.</summary>
        public string Combine(string _FilePath, string _FileName, string _FileExtension = "")
        {
            int i;
            string p = FixPath(_FilePath), f = FileName(_FileName).Trim(), e = _FileExtension.Trim();
            if (p != "")
            {
                if (p[p.Length - 1] != TrailingChar) p += TrailingChar;
            }
            if (e != "")
            {
                if (e[0] != '.') e = '.' + e;
                i = PosR('.', f);
                if (i > -1) f = Mid(f, 0, i);
            }
            return p + f + e;
        }

        /// <summary>Returns a string containing full path of file name, in file path and subfolder directory (created if not exists) and with file extension.</summary>
        public string Combine(string _FilePath, string _SubFolder, string _FileName, string _FileExtension)
        {
            return Combine(AutoPath(Combine(_FilePath, _SubFolder, "")), _FileName, _FileExtension);
        }

        /// <summary>Return configuration file full path.</summary>
        public string ConfiguratioFile(string _FileName, bool _EnvironmentDependency = true, string[] _Paths = null)
        {
            int i;
            string ext = FileExtension(_FileName), file = FileNameWithoutExt(_FileName).Trim(), full = FilePath(_FileName);
            List<string> paths = new List<string>();
            List<string> fileList = new List<string>();
            if (!Empty(full)) paths.Add(full.Trim());
            if (_Paths != null)
            {
                for (i = 0; i < _Paths.Length; i++)
                {
                    if (!Empty(_Paths[i])) paths.Add(FixPath(ToStr(_Paths[i]).Trim()));
                }
            }
            for (i = 0; i < paths.Count; i++)
            {
                if (!Empty(paths[i]))
                {
                    full = Merge(paths[i], file);
                    if (_EnvironmentDependency)
                    {
                        fileList.Add($"{full}.{Deploy}.{ext}");
                        fileList.Add($"{full}.{ext}");
                    }
                    else
                    {
                        fileList.Add($"{full}.{ext}");
                        fileList.Add($"{full}.{Deploy}.{ext}");
                    }
                    if (Deploy.Trim().ToLower().StartsWith("dev")) fileList.Add($"{full}.Production.{ext}");
                    else fileList.Add($"{full}.Development.{ext}");
                }
            }
            return FirstExists(fileList.ToArray(), _FileName);
        }

        /// <summary>Return fixed path without ending trailing char or default trailing char if omitted.</summary>
        public string FixPath(string _Path, char _TrailingChar = '\0')
        {
            int l;
            if (Empty(_Path)) return "";
            else
            {
                if (_TrailingChar == '\0') _TrailingChar = TrailingChar;
                _Path = TrailingChars(_Path, _TrailingChar);
                l = _Path.Length;
                if (l > 1)
                {
                    if ((_Path[l - 1] == _TrailingChar) && (_Path[l - 2] != ':') && (_Path[l - 2] != _TrailingChar)) return Mid(_Path, 0, l - 1);
                    else return _Path;
                }
                else if (_Path == "" + _TrailingChar) return _Path;
                else return "";
            }
        }

        /// <summary>Returns a string with path 1 and path 2 merged considering trailing char. 
        /// Paths will be normalized to trailing char replacing all \ and / chars with it.</summary>
        public string Merge(string _Path1, string _Path2, char _TrailingChar = '\0')
        {
            bool b1, b2;
            if (_TrailingChar == '\0') _TrailingChar = TrailingChar;
            _Path1 = ToStr(_Path1);
            _Path2 = ToStr(_Path2);
            if (_TrailingChar != '\\')
            {
                _Path1 = _Path1.Replace('\\', _TrailingChar);
                _Path2 = _Path2.Replace('\\', _TrailingChar);
            }
            if (_TrailingChar != '/')
            {
                _Path1 = _Path1.Replace('/', _TrailingChar);
                _Path2 = _Path2.Replace('/', _TrailingChar);
            }
            _Path1 = FixPath(_Path1.Trim(), _TrailingChar);
            _Path2 = FixPath(_Path2.Trim(), _TrailingChar);
            if (_Path1.Length < 1) return _Path2;
            else if (_Path2.Length < 1) return _Path1;
            else
            {
                if (_Path1.Length > 0) b1 = _Path1[_Path1.Length - 1] == _TrailingChar; else b1 = false;
                if (_Path2.Length > 0) b2 = _Path2[0] == _TrailingChar; else b2 = false;
                if (b1 && b2)
                {
                    if (_Path2.Length > 1) return _Path1 + _Path2.Substring(1);
                    else return _Path1;
                }
                else if (!b1 && !b2) return _Path1 + _TrailingChar + _Path2;
                else return _Path1 + _Path2;
            }
        }

        /// <summary>Returns a string with path 1 and path 2 and path 3 merged considering trailing char. 
        /// Paths will be normalized to trailing char replacing all \ and / chars with it.</summary>
        public string Merge(string _Path1, string _Path2, string _Path3, char _TrailingChar = '\0')
        {
            return Merge(Merge(_Path1, _Path2, _TrailingChar), _Path3, _TrailingChar);
        }

        /// <summary>Returns a string with path 1 and path 2 and path 3 and path 4 merged considering trailing char. 
        /// Paths will be normalized to trailing char replacing all \ and / chars with it.</summary>
        public string Merge(string _Path1, string _Path2, string _Path3, string _Path4, char _TrailingChar = '\0')
        { 
            return Merge(Merge(_Path1, _Path2, _TrailingChar), Merge(_Path3, _Path4, _TrailingChar), _TrailingChar);
        }

        /// <summary>Normalize path with trailing char replacing all \ and / chars with it.</summary>
        public string TrailingChars(string _Path, char _TrailingChar = '\0')
        {
            if (Empty(_Path)) return "";
            else if (_TrailingChar == '\0') return _Path.Trim();
            else
            {
                if (_TrailingChar != '\\') _Path = _Path.Replace('\\', _TrailingChar);
                if (_TrailingChar != '/') _Path = _Path.Replace('/', _TrailingChar);
                return _Path.Trim();
            }
        }

        /// <summary>Return full path of file name, on application folder.</summary>
        public string OnApplicationPath(string _FileName = "")
        {
            return Combine(ApplicationPath, _FileName);
        }

        /// <summary>Return full path of file name, on application subfolder.</summary>
        public string OnApplicationPath(string _SubFolder, string _FileName)
        {
            return Combine(Combine(ApplicationPath, _SubFolder), _FileName);
        }

        /// <summary>Return full path of file name, on data folder.</summary>
        public string OnDataPath(string _FileName = "")
        {
            return Combine(AutoPath(DataPath), _FileName);
        }

        /// <summary>Return full path of file name, on data subfolder.</summary>
        public string OnDataPath(string _SubFolder, string _FileName)
        {
            return Combine(Combine(AutoPath(DataPath), _SubFolder), _FileName);
        }

        /// <summary>Return full path of file name, on desktop folder.</summary>
        public string OnDesktopPath(string _FileName = "")
        {
            return Combine(DesktopPath, _FileName);
        }

        /// <summary>Return full path of file name, on desktop subfolder.</summary>
        public string OnDesktopPath(string _SubFolder, string _FileName)
        {
            return Combine(AutoPath(Combine(DesktopPath, _SubFolder)), _FileName);
        }

        /// <summary>Return full path of file name, on documents folder.</summary>
        public string OnDocumentsPath(string _FileName = "")
        {
            return Combine(DocumentsPath, _FileName);
        }

        /// <summary>Return full path of file name, on documents subfolder.</summary>
        public string OnDocumentsPath(string _SubFolder, string _FileName)
        {
            return Combine(AutoPath(Combine(DocumentsPath, _SubFolder)), _FileName);
        }

        /// <summary>Return full path of file name, on executable folder.</summary>
        public string OnExecPath(string _FileName = "")
        {
            return Combine(ExecutablePath, _FileName);
        }

        /// <summary>Return full path of file name, on executable subfolder.</summary>
        public string OnExecPath(string _SubFolder, string _FileName)
        {
            return Combine(Combine(ExecutablePath, _SubFolder), _FileName);
        }

        /// <summary>Return full path of file name, on library folder.</summary>
        public string OnLibraryPath(string _FileName = "")
        {
            if (Empty(_FileName)) return Combine(ExecutablePath, "Library");
            return Combine(Combine(ExecutablePath, "Library"), _FileName);
        }

        /// <summary>Return full path of file name, on library subfolder.</summary>
        public string OnLibraryPath(string _SubFolder, string _FileName)
        {
            return Combine(Combine(Combine(ExecutablePath, "Library"), _SubFolder), _FileName);
        }

        /// <summary>Return full path of file name, on root folder.</summary>
        public string OnRootPath(string _FileName = "")
        {
            return Combine(RootPath, _FileName);
        }

        /// <summary>Return full path of file name, on root subfolder.</summary>
        public string OnRootPath(string _SubFolder, string _FileName)
        {
            return Combine(Combine(RootPath, _SubFolder), _FileName);
        }

        /// <summary>Return full path of file name, on user profile documents folder.</summary>
        public string OnUserDocPath(string _FileName = "")
        {
            return Combine(UserDocumentsPath, _FileName);
        }

        /// <summary>Return full path of file name, on user profile documents subfolder.</summary>
        public string OnUserDocPath(string _SubFolder, string _FileName)
        {
            return Combine(AutoPath(Combine(UserDocumentsPath, _SubFolder)), _FileName);
        }

        /// <summary>Return full path of file name, on user profile folder.</summary>
        public string OnUserPath(string _FileName = "")
        {
            return Combine(UserPath, _FileName);
        }

        /// <summary>Return full path of file name, on user profile subfolder.</summary>
        public string OnUserPath(string _SubFolder, string _FileName)
        {
            return Combine(Combine(UserPath, _SubFolder), _FileName);
        }

        /// <summary>Remove from file path, initial base path if found.</summary>
        public string RemoveBase(string _FilePath, string _BasePath)
        {
            _FilePath = _FilePath.Trim();
            _BasePath = FixPath(_BasePath.Trim());
            if (_FilePath.StartsWith(_BasePath))
            {
                if (_FilePath.Length == _BasePath.Length) return "";
                else return _FilePath.Substring(_BasePath.Length);
            }
            else return _FilePath;
        }

        /// <summary>Rebuild application related paths.</summary>
        public void Repath()
        {
            if (Empty(applicationPath))
            {
                if (Empty(OEM)) applicationPath = AutoPath(Combine(CommonPath, ExecutableName));
                else applicationPath = AutoPath(Combine(Combine(CommonPath, OEM), ExecutableName));
            }
            DataPath = AutoPath(Combine(ApplicationPath, SMDefaults.DataFolderName));
            TempPath = AutoPath(Combine(ApplicationPath, SMDefaults.TempFolderName));
            DefaultLogFilePath = "";
        }

        /// <summary>Returns a random temporary file full path with extension.</summary>
        public string TempFile(string _Extension)
        {
            return Combine(TempPath, "~" + RndName(7), _Extension);
        }

        /// <summary>Returns a temporary ini file with full path with extension.</summary>
        public string TempIniFile(string _FileNameWithoutExtension)
        {
            return Combine(TempPath, _FileNameWithoutExtension, "ini");
        }

        #endregion

        /* */

    }

    /* */

}
