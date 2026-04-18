/*  ===========================================================================
 *  
 *  File:       SMFileItem.cs
 *  Version:    2.1.0
 *  Date:       April 2026
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
        private readonly SMCode SM = null;

        #endregion

        /* */

        #region Properties

        /*  ===================================================================
         *  Properties
         *  ===================================================================
         */

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

        /// <summary>Get or set file name.</summary>
		public string Name { get; set; } = "";

        /// <summary>Get or set file path.</summary>
		public string Path { get; set; } = "";

        /// <summary>Get or set file size.</summary>
		public long Size { get; set; } = 0;

        /// <summary>Get or set file extension.</summary>
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public byte[] Content { get; set; } = null;

        /// <summary>Get or set file description.</summary>
		public string Text { get; set; } = "";

        /// <summary>Get or set instance tag object.</summary>
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public object Tag { get; set; } = null;

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
			Name = _FileItem.Name;
			Path = _FileItem.Path;
			Size = _FileItem.Size;
			Content = _FileItem.Content;
			Text = _FileItem.Text;
			Tag = _FileItem.Tag;
		}

		/// <summary>Clear item.</summary>
		public void Clear()
		{
            Name = "";
            Path = "";
            Size = 0;
            Content = null;
            Text = "";
            Tag = null;
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
        public bool Load(string _FullPath, bool _LoadContent = true)
        {
            bool rslt = false;
            try
            {
                Clear();
                if (_FullPath != null)
                {
                    if (_FullPath.Trim().Length > 0)
                    {
                        Name = SM.FileName(_FullPath);
                        Path = SM.FilePath(_FullPath);
                        rslt = true;
                        if (_LoadContent)
                        {
                            Content = SM.LoadFile(_FullPath);
                            if (Content == null) rslt = false;
                            else Size = Content.Length;
                        }
                        else Size = SM.FileSize(_FullPath);
                    }
                }
            }
            catch (Exception ex)
            {
                SM.Error(ex);
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

		#endregion

		/* */

	}

	/* */

}
