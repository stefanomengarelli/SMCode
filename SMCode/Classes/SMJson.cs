/*  ===========================================================================
 *  
 *  File:       SMJson.cs
 *  Version:    2.0.56
 *  Date:       October 2024
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2024 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *
 *  SMCode JSON class.
 *
 *  ===========================================================================
 */

using System;
using System.IO;
using System.Text.Json;

namespace SMCodeSystem
{

	/* */

	/// <summary>SMCode JSON class.</summary>
	public class SMJson
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

        /// <summary>Get JSON document.</summary>
        public JsonDocument Document { get; private set; } = null;

		/// <summary>Get JSON root element.</summary>
		public JsonElement Root { get; private set; } = new JsonElement();

		#endregion

		/* */

		#region Initialization

		/*  ===================================================================
         *  Initialization
         *  ===================================================================
         */

		/// <summary>Class constructor.</summary>
		public SMJson(SMCode _SM = null)
		{
			SM = SMCode.CurrentOrNew(_SM);
			InitializeInstance();
		}

		/// <summary>Class constructor.</summary>
		public SMJson(string _FilePath, SMCode _SM = null)
		{
			SM = SMCode.CurrentOrNew(_SM);
			InitializeInstance();
            Load(_FilePath);
		}

		/// <summary>Class constructor.</summary>
		public SMJson(SMJson _OtherInstance, SMCode _SM = null)
        {
            if (_SM==null) _SM = _OtherInstance.SM;
            SM = SMCode.CurrentOrNew(_SM);
            InitializeInstance();
            Assign(_OtherInstance);
        }

        /// <summary>Initialize instance.</summary>
        public void InitializeInstance()
        {
            Clear();
        }

        #endregion

        /* */

        #region Methods

        /*  ===================================================================
         *  Methods
         *  ===================================================================
         */

        /// <summary>Assign instance properties from another.</summary>
        public void Assign(SMJson _OtherInstance)
        {
            Document = _OtherInstance.Document;
            Root = Document.RootElement;
        }

        /// <summary>Clear item.</summary>
        public void Clear()
        {
			Document = null;
            Root = new JsonElement();
        }

        /// <summary>Return JSON document property as string.</summary>
        public string Get(string _KeyPath)
        {
            string kpth = _KeyPath, rslt = "";
            JsonElement j;
            try
            {
                j = Root;
                while (!SM.Empty(kpth))
                {
                    rslt = SM.Extract(ref kpth, ";.:>\\/").Trim();
                    if (!SM.Empty(rslt)) j = j.GetProperty(rslt);
                }
                rslt = j.GetString();
            }
            catch (Exception ex) 
            {
                SM.Error(ex);
                rslt = "";
            }
            return rslt;
        }

		/// <summary>Return JSON document property as date, including time if specified.</summary>
		public DateTime GetDate(string _KeyPath, bool _IncludeTime = false)
		{
			return SM.ToDate(Get(_KeyPath), _IncludeTime);
		}

		/// <summary>Return JSON document property as double.</summary>
		public double GetDouble(string _KeyPath)
		{
			return SM.ToDouble(Get(_KeyPath));
		}

		/// <summary>Return JSON document property as integer.</summary>
		public int GetInt(string _KeyPath)
		{
			return SM.ToInt(Get(_KeyPath));
		}

		/// <summary>Return JSON document property as long.</summary>
		public long GetLong(string _KeyPath)
		{
			return SM.ToLong(Get(_KeyPath));
		}

		/// <summary>Load JSON from string.</summary>
		public bool FromString(string _Json)
        {
			try
			{
				Document = JsonDocument.Parse(_Json);
				Root = Document.RootElement;
				return true;
			}
			catch (Exception ex)
			{
				SM.Error(ex);
				return false;
			}
		}

		/// <summary>Load JSON from file.</summary>
		public bool Load(string _FilePath)
        {
            FileStream fs;
            try
            {
                if (SM.FileExists(_FilePath))
                {
                    fs = new FileStream(_FilePath, FileMode.Open, FileAccess.Read);
                    Document = JsonDocument.Parse(fs);
                    Root = Document.RootElement;
					return true;
				}
                else return false;
            }
            catch (Exception ex)
            {
                SM.Error(ex);
                return false;
            }
        }

        #endregion

        /* */

    }

    /* */

}
