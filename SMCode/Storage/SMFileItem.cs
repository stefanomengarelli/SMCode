/*  ===========================================================================
 *  
 *  File:       SMFileItem.cs
 *  Version:    2.4.0
 *  Date:       August 2026
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2026 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *
 *  SMCode file item management class.
 *
 *  ===========================================================================
 */

using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SMCodeSystem
{

	/* */

	/// <summary>SMCode file item management class.</summary>
	public class SMFileItem
    {

        /* */

        #region Declarations

        /*  ===================================================================
         *  Declarations
         *  ===================================================================
         */

        /// <summary>SM session instance.</summary>
        public readonly SMCode SM = null;

        /// <summary>File UID.</summary>
        private Guid? uid = null;

        /// <summary>File user UID.</summary>
        private Guid? user = null;

        #endregion

        /* */

        #region Properties

        /*  ===================================================================
         *  Properties
         *  ===================================================================
         */

        /// <summary>Get or set file extension.</summary>
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public byte[] Content { get; set; } = null;

        /// <summary>Get or set file creation datetime.</summary>
		public DateTime Created { get; set; } = DateTime.MinValue;

        /// <summary>Get or set error flag.</summary>
        public bool Error { get; set; } = false;

        /// <summary>Get or set file extension.</summary>
		public string Extension
        {
            get { return SM.FileExtension(Name); }
            set { Name = SM.ChangeExtension(Name, value); }
        }

        /// <summary>Get or set file full path.</summary>
		public string FullPath 
        {
            get { return SM.Combine(Path, Name); }
            set
            {
                Name = SM.FileName(value);
                Path = SM.FilePath(value);
            }
        }

        /// <summary>Get or set file hash.</summary>
        public string Hash { get; set; } = "";

        /// <summary>Get or set last file read datetime.</summary>
		public DateTime LastRead { get; set; } = DateTime.MinValue;

        /// <summary>Get or set last file write datetime.</summary>
		public DateTime LastWrite { get; set; } = DateTime.MinValue;

        /// <summary>Get or set file name.</summary>
		public string Name { get; set; } = "";

        /// <summary>Get or set file path.</summary>
		public string Path { get; set; } = "";

        /// <summary>Get or set file size.</summary>
		public long Size { get; set; } = 0;

        /// <summary>Get or set instance tag object.</summary>
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public object Tag { get; set; } = null;

        /// <summary>Get or set file description.</summary>
		public string Text { get; set; } = "";

        /// <summary>Get or set file UID.</summary>
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Uid
        {
            get { return SM.FromGuid(uid); }
            set { uid = SM.ToGuid(value); }
        }

        /// <summary>Get or set file user UID.</summary>
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string User
        {
            get { return SM.FromGuid(user); }
            set { user = SM.ToGuid(value); }
        }

        #endregion

        /* */

        #region Initialization

        /*  ===================================================================
         *  Initialization
         *  ===================================================================
         */

        /// <summary>Class constructor.</summary>
        public SMFileItem(SMCode _SM = null)
        {
            SM = SMCode.CurrentOrNew(_SM);
        }

        /// <summary>Class constructor.</summary>
        public SMFileItem(SMFileItem _FileItem, SMCode _SM = null)
        {
            SM = SMCode.CurrentOrNew(_SM);
            Assign(_FileItem);
        }

        /// <summary>Class constructor.</summary>
        public SMFileItem(string _FileName, bool _LoadContent = true, SMCode _SM = null)
        {
            SM = SMCode.CurrentOrNew(_SM);
            Load(_FileName, _LoadContent);
        }

        #endregion

        /* */

        #region Methods

        /*  ===================================================================
         *  Methods
         *  ===================================================================
         */

        /// <summary>Assign instance properties from another.</summary>
        public void Assign(SMFileItem _FileItem)
		{
            Content = _FileItem.Content;
            Created = _FileItem.Created;
            Error = _FileItem.Error;
            LastRead = _FileItem.LastRead;
            LastWrite = _FileItem.LastWrite;
            Hash = _FileItem.Hash;
            Name = _FileItem.Name;
			Path = _FileItem.Path;
			Size = _FileItem.Size;
            Tag = _FileItem.Tag;
            Text = _FileItem.Text;
            uid = _FileItem.uid;
            user = _FileItem.user;
        }

		/// <summary>Clear item.</summary>
		public void Clear()
		{
            Content = null;
            Created = DateTime.MinValue;
            Error = false;
            LastRead = DateTime.MinValue;
            LastWrite = DateTime.MinValue;
            Hash = "";
            Name = "";
            Path = "";
            Size = 0;
            Tag = null;
            Text = "";
            uid = null;
            user = null;
        }

        /// <summary>Delete file.</summary>
        public bool Delete()
        {
            if (Name == null) return false;
            else if (Name.Trim().Length < 1) return false;
            else if (Path == null) return false;
            else if (Path.Trim().Length < 1) return false;
            else return SM.FileDelete(FullPath);
        }

        /// <summary>Assign property from JSON serialization.</summary>
        public bool FromJSON(string _JSON)
		{
			try
			{
				Assign((SMFileItem)JsonSerializer.Deserialize(_JSON, null));
				return true;
			}
			catch
			{
				return false;
			}
		}

        /// <summary>Load file name. Return true if succeed.</summary>
        public bool Load(string _FullPath, bool _LoadContent = true, int _FileRetries = -1)
        {
            bool mr = false, rslt = false;
            FileInfo fi;
            try
            {
                Clear();
                if (_FullPath != null)
                {
                    if (_FullPath.Trim().Length > 0)
                    {
                        if (_FileRetries < 0) _FileRetries = SM.FileRetries;
                        if ((_FileRetries < 0) || (_FileRetries > 100)) _FileRetries = 1;
                        Name = SM.FileName(_FullPath);
                        Path = SM.FilePath(_FullPath);
                        while (!rslt  && (_FileRetries > 0))
                        {
                            _FileRetries--;
                            try
                            {
                                fi = new FileInfo(_FullPath);
                                if (fi.Exists)
                                {
                                    rslt = true;
                                    Created = fi.CreationTime;
                                    Size = fi.Length;
                                    LastRead = fi.LastAccessTime;
                                    LastWrite = fi.LastWriteTime;
                                    if (_LoadContent)
                                    {
                                        Content = File.ReadAllBytes(_FullPath);
                                        if (Content != null) Hash = SM.HashSHA256(Content);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                SM.Error(ex);
                                if (!mr) mr = SM.MemoryRelease(true);
                                SM.Wait(SM.FileRetriesDelay, true);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SM.Error(ex);
                Error = true;
                rslt = false;
            }
            return rslt;
        }

        /// <summary>Move file to full path.</summary>
        public bool Move(string _FullPath)
        {
            if (Name == null) return false;
            else if (Name.Trim().Length < 1) return false;
            else if (Path == null) return false;
            else if (Path.Trim().Length < 1) return false;
            else if (_FullPath == null) return false;
            else if (_FullPath.Trim().Length < 1) return false;
            else return SM.FileMove(FullPath, _FullPath);
        }

        /// <summary>Read item from dataset.</summary>
        public void Read(SMDataset _Dataset)
        {
            Error = false;
            Name = _Dataset.FieldStr("Name");
            Path = _Dataset.FieldStr("Path");
            Size = _Dataset.FieldInt("Size");
            Content = _Dataset.FieldBlob("Content");
            Hash = _Dataset.FieldStr("Hash");
            Text = _Dataset.FieldStr("Text");
            Tag = null;
            uid = SM.ToGuid(_Dataset.FieldStr("Uid"));
            user = SM.ToGuid(_Dataset.FieldStr("User"));
        }

        /// <summary>Write item on dataset.</summary>
        public void Write(SMDataset _Dataset)
        {
            _Dataset.Assign("Name", Name);
            _Dataset.Assign("Path", Path);
            _Dataset.Assign("Size", Size);
            _Dataset.Assign("Content", Content);
            _Dataset.Assign("Hash", Hash);
            _Dataset.Assign("Text", Text);
            _Dataset.Assign("Uid", SM.FromGuid(uid));
            _Dataset.Assign("User", SM.FromGuid(user));
        }

        /// <summary>Save file.</summary>
        public bool Save()
        {
            if (Name == null) return false;
            else if (Name.Trim().Length < 1) return false;
            else if (Path == null) return false;
            else if (Path.Trim().Length < 1) return false;
            else return SM.SaveFile(FullPath, Content);
        }

        /// <summary>Return JSON serialization of instance.</summary>
        public string ToJSON()
		{
			try
			{
				return JsonSerializer.Serialize(this);
			}
			catch
			{
				return "";
			}
		}

        /// <summary>Return string representation of this instance.</summary>
        public override string ToString()
        {
            return $"SMCodeSystem.SMFileItem {{ Name: \"{Name}\"; Path: \"{Path}\"; Size: {Size} }}";
        }

        #endregion

        /* */

    }

    /* */

}
