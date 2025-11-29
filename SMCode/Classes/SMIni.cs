/*  ===========================================================================
 *  
 *  File:       SMIni.cs
 *  Version:    2.0.0
 *  Date:       March 2024
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2024 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *
 *  SMCode INI configuration file management class.
 *
 *  ===========================================================================
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace SMCodeSystem
{

    /* */

    /// <summary>SMCode INI configuration file management class.</summary>
    public class SMIni
    {

        /* */

        #region Declarations

        /*  ===================================================================
         *  Declarations
         *  ===================================================================
         */

        /// <summary>SM session instance.</summary>
        private readonly SMCode SM = null;

        /// <summary>Last section found index.</summary>
        private int lastSectionFound = -1;

        #endregion

        /* */

        #region Properties

        /*  ===================================================================
         *  Properties
         *  ===================================================================
         */

        /// <summary>Get or set INI file changed flag.</summary>
        public bool Changed { get; set; } = false;

        /// <summary>Get or set INI text enconding.</summary>
        public Encoding TextEncoding { get; set; } = null;

        /// <summary>Get configuration INI file lines collection.</summary>
        public List<string> Lines { get; private set; } = new List<string>();

        /// <summary>Get or set INI file encoding password.</summary>
        public string Password { get; set; } = "";

        /// <summary>Get or set INI file full path.</summary>
        public string Path { get; set; } = "";

        /// <summary>Get or set write default flag.</summary>
        public bool WriteDefault { get; set; } = false;

        #endregion

        /* */

        #region Initialization

        /*  ===================================================================
         *  Initialization
         *  ===================================================================
         */

        /// <summary>Class constructor.</summary>
        public SMIni(SMCode _SM = null)
        {
            SM = SMCode.CurrentOrNew(_SM);
            TextEncoding = SM.TextEncoding;
            Clear();
        }

        /// <summary>Class constructor.</summary>
        public SMIni(string _FileName, SMCode _SM = null)
        {
            SM = SMCode.CurrentOrNew(_SM);
            TextEncoding = SM.TextEncoding;
            Load(_FileName);
        }

        /// <summary>Class constructor.</summary>
        public SMIni(string _FileName, string _Password, SMCode _SM = null)
        {
            SM = SMCode.CurrentOrNew(_SM);
            TextEncoding = SM.TextEncoding;
            Load(_FileName, _Password);
        }

        #endregion

        /* */

        #region Methods

        /*  ===================================================================
         *  Methods
         *  ===================================================================
         */

        /// <summary>Clear item.</summary>
        public void Clear()
        {
            Changed = false;
            Lines.Clear();
            Path = "";
        }

        /// <summary>Return index of section line with key. Return -1 if not found.</summary>
        public int Find(string _Section, string _Key)
        {
            int r = -1, i = Lines.Count, j, lastKey = -1;
            string l;
            _Section = _Section.Trim().ToLower();
            _Key = _Key.Trim().ToLower();
            lastSectionFound = -1;
            if ((_Section.Length > 0) && (_Key.Length > 0))
            {
                while ((r < 0) && (i > 0))
                {
                    i--;
                    l = Lines[i].Trim();
                    if (l.Length > 2)
                    {
                        if ((l[0] == '[') && (l[l.Length - 1] == ']'))
                        {
                            if (_Section == l.Substring(1, l.Length - 2).Trim().ToLower())
                            {
                                lastSectionFound = i;
                                if (lastKey > -1) r = lastKey;
                            }
                            else lastKey = -1;
                        }
                        else
                        {
                            j = l.IndexOf('=');
                            if (j > 0)
                            {
                                if (_Key == l.Substring(0, j).Trim().ToLower()) lastKey = i;
                            }
                        }
                    }
                }
            }
            return r;
        }

        /// <summary>Get INI file values and content from string.</summary>
        public void FromString(string _String)
        {
            if (!SM.Empty(Password)) _String = SM.FromHexMask(_String, Password);
            else if (SM.IsHexMask(_String)) _String = "";
            SM.ToLines(_String, false, false, null, Lines, false);
            Changed = true;
        }

        /// <summary>Load INI file values and content from file. A password for decoding 
        /// encrypted INI files can be specified. Return true if succeed.</summary>
        public bool Load(string _FileName, string _Password = null)
        {
            _FileName = _FileName.Trim();
            if (SM.Empty(_FileName))
            {
                _FileName = SM.Combine(SM.AutoPath(SM.Combine(SM.ApplicationPath, SMDefaults.ConfigFolderName)), SM.ExecutableName, "ini");
            }
            if (!SM.Empty(_Password)) Password = _Password;
            this.Clear();
            SM.Error();
            if (SM.FileExists(_FileName)) FromString(SM.LoadString(_FileName, TextEncoding, SM.FileRetries));
            Path = _FileName;
            Changed = false;
            return !SM.IsError;
        }

        /// <summary>Read string value of key at section. Return default value if not found.</summary>
        public string ReadString(string _Section, string _Key, string _Default)
        {
            int i = Find(_Section, _Key);
            if (i < 0)
            {
                if (WriteDefault) WriteString(_Section, _Key, _Default);
                return _Default;
            }
            else return SM.Unquote2(SM.After(Lines[i], "=").Trim());
        }

        /// <summary>Read boolean value of key at section. Return default value if not found.</summary>
        public bool ReadBool(string _Section, string _Key, bool _Default)
        {
            return SM.ToBool(ReadString(_Section, _Key, SM.ToBool(_Default)));
        }

        /// <summary>Read date-time value of key at section. Return default value if not found.</summary>
        public DateTime ReadDateTime(string _Section, string _Key, DateTime _Default)
        {
            return SM.ToDate(ReadString(_Section, _Key,
                SM.ToStr(_Default, SMDateFormat.iso8601, true)),
                SMDateFormat.iso8601, true);
        }

        /// <summary>Read double value of key at section. Return default value if not found.</summary>
        public double ReadDouble(string _Section, string _Key, double _Default)
        {
            return SM.ToDouble(ReadString(_Section, _Key,
                SM.ToStr(_Default).Replace(SM.DecimalSeparator, '.')
                ).Replace('.', SM.DecimalSeparator));
        }

        /// <summary>Read hex masked value of key at section. Return default value if not found.</summary>
        public string ReadHexMask(string _Section, string _Key, string _Default)
        {
            string r = ReadString(_Section, _Key, _Default), p = Password;
            if (SM.Empty(p)) p = SM.InternalPassword;
            if (SM.IsHexMask(r)) return SM.FromHexMask(r, p);
            else 
            {
                WriteHexMask(_Section, _Key, r);
                return r;
            }
        }

        /// <summary>Read integer value of key at section. Return default value if not found.</summary>
        public int ReadInteger(string _Section, string _Key, int _Default)
        {
            return SM.ToInt(ReadString(_Section, _Key, _Default.ToString()));
        }

        /// <summary>Save INI file values and content to file.</summary>
        public bool Save(string _FileName)
        {
            if (SM.FolderExists(SM.FilePath(_FileName)))
            {
                if (SM.SaveString(_FileName, this.ToString(), TextEncoding, SM.FileRetries))
                {
                    Changed = false;
                    return true;
                }
                else return false;
            }
            else return false;
        }

        /// <summary>Save INI file values and content to file.</summary>
        public bool Save()
        {
            if (Changed && (Path.Trim().Length > 0)) return Save(Path);
            else return !Changed;
        }

        /// <summary>Return INI file values and content as string.</summary>
        public override string ToString()
        {
            if (Password.Trim().Length > 0) return SM.ToHexMask(SM.ToStr(Lines, false), Password.Trim());
            else return SM.ToStr(Lines, false);
        }

        /// <summary>Write string value of key at section. Return line index if success otherwise -1.</summary>
        public int WriteString(string _Section, string _Key, string _Value)
        {
            int r = Find(_Section, _Key), i;
            string ln;
            if (r < 0)
            {
                if (lastSectionFound < 0)
                {
                    if (Lines.Count > 0)
                    {
                        if (Lines[Lines.Count - 1].Trim().Length > 0) Lines.Add("");
                    }
                    Lines.Add("[" + _Section + "]");
                    r = Lines.Count;
                    Lines.Add(_Key + " = " + _Value);
                }
                else
                {
                    r = -1;
                    i = lastSectionFound + 1;
                    while ((r < 0) && (i < Lines.Count))
                    {
                        ln = Lines[i].Trim();
                        if (ln.Length > 1)
                        {
                            if ((ln[0] == '[') && (ln[ln.Length - 1] == ']')) r = i;
                        }
                        i++;
                    }
                    if (r > -1)
                    {
                        if (Lines[r - 1].Trim().Length > 0)
                        {
                            Lines.Insert(r, "");
                            Lines.Insert(r, _Key + " = " + _Value);
                        }
                        else Lines.Insert(r - 1, _Key + " = " + _Value);
                    }
                    else Lines.Add(_Key + " = " + _Value);
                }
            }
            else Lines[r] = Lines[r].Substring(0, Lines[r].IndexOf('=') + 1) + ' ' + _Value;
            Changed = true;
            return r;
        }

        /// <summary>Write double value of key at section. Return line index if success otherwise -1.</summary>
        public int WriteBool(string _Section, string _Key, bool _Value)
        {
            return WriteString(_Section, _Key, SM.ToBool(_Value));
        }

        /// <summary>Write double value of key at section. Return line index if success otherwise -1.</summary>
        public int WriteDateTime(string _Section, string _Key, DateTime _Value)
        {
            return WriteString(_Section, _Key, SM.ToStr(_Value, SMDateFormat.iso8601, true));
        }

        /// <summary>Write double value of key at section. Return line index if success otherwise -1.</summary>
        public int WriteDouble(string _Section, string _Key, double _Value)
        {
            return WriteString(_Section, _Key, SM.ToStr(_Value).Replace(SM.ThousandSeparator + "", "").Replace(SM.DecimalSeparator, '.'));
        }

        /// <summary>Write hex masked value of key at section. Return line index if success otherwise -1.</summary>
        public int WriteHexMask(string _Section, string _Key, string _Value)
        {
            string p = Password;
            if (SM.Empty(p)) p = SM.InternalPassword;
            if (SM.IsHexMask(_Value)) return WriteString(_Section, _Key, _Value);
            else return WriteString(_Section, _Key, SM.ToHexMask(_Value, p));
        }

        /// <summary>Write integer value of key at section. Return line index if success otherwise -1.</summary>
        public int WriteInteger(string _Section, string _Key, int _Value)
        {
            return WriteString(_Section, _Key, _Value.ToString());
        }

        #endregion

        /* */

    }

    /* */

}
