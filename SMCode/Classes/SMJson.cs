/*  ===========================================================================
 *  
 *  File:       SMJson.cs
 *  Version:    2.0.112
 *  Date:       December 2024
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
using System.Text;
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
		public SMJson(SMCode _SM)
		{
			SM = SMCode.CurrentOrNew(_SM);
			InitializeInstance();
		}

		/// <summary>Class constructor.</summary>
		public SMJson(string _FilePath, SMCode _SM)
		{
			SM = SMCode.CurrentOrNew(_SM);
			InitializeInstance();
            Load(_FilePath);
		}

		/// <summary>Class constructor.</summary>
		public SMJson(SMJson _OtherInstance, SMCode _SM)
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
        public void Assign(SMJson _JSON)
        {
            Document = _JSON.Document;
            Root = Document.RootElement;
        }

        /// <summary>Clear item.</summary>
        public void Clear()
        {
			Document = null;
            Root = new JsonElement();
        }

        /// <summary>Return JSON document property as string. Properties tree can be specified with : or . or &gt; separator.</summary>
        public string Get(string _KeyPath, string _Default = "", bool _RaiseException = false)
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
                if (SM.Empty(rslt)) rslt = _Default;
            }
            catch (Exception ex)
            {
                if (_RaiseException) SM.Raise(ex.Message, true);
                rslt = "";
            }
            return rslt;
        }

		/// <summary>Return JSON document property as date, including time if specified.</summary>
		public DateTime GetDate(string _KeyPath, bool _IncludeTime = false, bool _RaiseException = false)
		{
			return SM.ToDate(Get(_KeyPath,"", _RaiseException), _IncludeTime);
		}

		/// <summary>Return JSON document property as double.</summary>
		public double GetDouble(string _KeyPath, bool _RaiseException = false)
		{
            return SM.ToDouble(Get(_KeyPath, "", _RaiseException));
		}

		/// <summary>Return JSON document property as integer.</summary>
		public int GetInt(string _KeyPath, bool _RaiseException = false)
		{
            return SM.ToInt(Get(_KeyPath, "", _RaiseException));
		}

		/// <summary>Return JSON document property as long.</summary>
		public long GetLong(string _KeyPath, bool _RaiseException = false)
		{
            return SM.ToLong(Get(_KeyPath, "", _RaiseException));
		}

        /// <summary>Load JSON from serializing object.</summary>
        public bool FromObject(object _Object)
        {
            try
            {
                Document = JsonSerializer.SerializeToDocument(_Object);
                Root = Document.RootElement;
                return true;
            }
            catch (Exception ex)
            {
                SM.Error(ex);
                return false;
            }
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

        /// <summary>Return document as object.</summary>
        public object ToObject(Type _ReturnType, bool _Indented = false)
        {
            try
            {
                return JsonSerializer.Deserialize(Document, _ReturnType);
            }
            catch (Exception ex)
            {
                SM.Error(ex);
                return null;
            }
        }

        /// <summary>Return document as JSON string.</summary>
        public override string ToString()
        {
            return ToString(false);
        }

        /// <summary>Return document as JSON string.</summary>
        public string ToString(bool _Indented)
        {
            MemoryStream ms = new MemoryStream();
            JsonWriterOptions options = new JsonWriterOptions() { Indented = _Indented };
            Utf8JsonWriter writer = new Utf8JsonWriter(ms, options);
            Document.WriteTo(writer);
            writer.Flush();
            return Encoding.UTF8.GetString(ms.ToArray());
        }

        #endregion

        /* */

    }

    /* */

}
