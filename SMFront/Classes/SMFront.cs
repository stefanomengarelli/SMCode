/*  ===========================================================================
 *  
 *  File:       SMFront.cs
 *  Version:    2.0.85
 *  Date:       November 2024
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2024 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *
 *  SMFront class.
 *  
 *  ===========================================================================
 */

using Microsoft.AspNetCore.Http;
using SMCodeSystem;
using System;
using System.Linq;

namespace SMFrontSystem
{

    /* */

    /// <summary>SMFront class: initialization.</summary>
    public partial class SMFront:SMCode
    {

        /* */

        #region Declarations

        /*  ===================================================================
         *  Declarations
         *  ===================================================================
         */

        /// <summary>Current session id.</summary>
        public string session = "";

        /// <summary>Templates collection.</summary>
        private SMDictionary templates = null;

        /// <summary>Last template.</summary>
        private int lastTemplate = -1;

        #endregion

        /* */

        #region Properties

        /*  ===================================================================
         *  Properties
         *  ===================================================================
         */

        /// <summary>Get or set last application instance created.</summary>
        public static new SMFront SM { get; set; } = null;

        /// <summary>Get or set controls attribute prefix.</summary>
        public string AttributePrefix { get; set; } = "sm-";

        /// <summary>Application assembly base path.</summary>
        public static string BasePath { get; set; } = "";

        /// <summary>Database cache.</summary>
        public SMCache Cache { get; private set; } = null;

        /// <summary>Get or set classes attribute prefix.</summary>
        public string ClassPrefix { get; set; } = "sm-";

        /// <summary>Application configuration (appsettings.json).</summary>
        public SMJson Configuration { get; private set; } = null;

        /// <summary>Get current HTTP request cookies.</summary>
        public SMDictionary Cookies { get; private set; } = new SMDictionary();

        /// <summary>Get or set current HTTP context.</summary>
        public HttpContext Context { get; set; } = null;

        /// <summary>Get or set current Form.</summary>
        public SMFrontForm Form { get; private set; } = null;

        /// <summary>Get current HTTP request query.</summary>
        public SMDictionary Query { get; private set; } = new SMDictionary();

        /// <summary>Get or set current HTTP request.</summary>
        public HttpRequest Request { get; set; } = null;

        /// <summary>Get or set current HTTP response.</summary>
        public HttpResponse Response { get; set; } = null;

        /// <summary>Application root path on server.</summary>
        public static string RootPath { get; set; } = "";
        
        /// <summary>Get or set current web session.</summary>
        public string Session
        {
            get
            {
                if (SM.Empty(session)) session = Guid.NewGuid().ToString();
                return session;
            }
            set
            {
                if (session != value) session = value;
            }
        }

        /// <summary>Get templates instance.</summary>
        public SMTemplates Templates { get; private set; }

        #endregion

        /* */

        #region Initialization

        /*  ===================================================================
         *  Initialization
         *  ===================================================================
         */

        /// <summary>Initialize instance.</summary>
        public SMFront(HttpContext _Context, string[] _Arguments = null, string _OEM = "",
            string _InternalPassword = "", string _ApplicationPath = "") : base(_Arguments, _OEM, _InternalPassword, _ApplicationPath)
        {
            SM = this;
            Context = _Context;
            if (!SM.Empty(_ApplicationPath)) ApplicationPath = _ApplicationPath;
            else if (!SM.Empty(RootPath)) ApplicationPath = RootPath;
            InitializeInstance();
        }

        /// <summary>Initialize control instance.</summary>
        private void InitializeInstance()
        {
            int i;
            string host, database, connstr, q;
            string[] arr;
            SMDatabaseType dbtype;
            SMDictionary dc;
            //
            // set current context request and response
            //
            if (Context != null)
            {
                Request = Context.Request;
                Response = Context.Response;
            }
            //
            // set base path
            //
            BasePath = AppDomain.CurrentDomain.BaseDirectory;
            //
            // load configuration
            //
            Configuration = new SMJson(OnBasePath("appsettings.json"), this);
            //
            // set configuration
            //
            InternalPassword = Configuration.Get("Application:InternalPassword", InternalPassword).Trim();
            OEM = Configuration.Get("Application:OEM", OEM).Trim();
            Test = SM.ToBool(Configuration.Get("Application:Test").Trim());
            Repath();
            //
            // add default main database
            //
            host = Configuration.Get("Databases:MAIN:Host");
            database = Configuration.Get("Databases.MAIN.Database");
            connstr = Configuration.Get("Databases:MAIN:ConnectionString");
            dbtype = SMDatabase.TypeFromString(Configuration.Get("Databases:MAIN:Type"));
            SM.Databases.Add("MAIN", dbtype, host, database, connstr);
            SM.LogAlias = "MAIN";
            SM.MainAlias = "MAIN";
            //
            // form
            //
            Form = new SMFrontForm(this);
            //
            // cache
            //
            Cache = new SMCache(this);
            //
            // templates
            //
            Templates = new SMTemplates(this);
            Templates.Path = SMFront.RootPath;
            //
            // get query string values
            //
            if (Request != null)
            {
                if (Request.Query != null)
                {
                    q = "";
                    arr = Request.Query.Keys.ToArray();
                    if (arr != null)
                    {
                        for (i = 0; i < arr.Length; i++)
                        {
                            if (!SM.Empty(arr[i]))
                            {
                                if (arr[i].ToLower() == "sm_q") q = Request.Query[arr[i]];
                                Query.Set(arr[i], SM.ToStr(Request.Query[arr[i]]));
                            }
                        }
                    }
                    if (q != "")
                    {
                        q = Base64Decode(q);
                        dc = new SMDictionary(q, SM);
                        for (i = 0; i < dc.Count; i++)
                        {
                            Query.Set(dc[i].Key, dc[i].Value);
                        }
                    }
                }
            }
            //
            // get form values
            //
            if (Request != null)
            {
                if (Request.Method == "POST")
                {
                    if (Request.Form != null)
                    {
                        if (Request.Form.Keys != null)
                        {
                            arr = Request.Form.Keys.ToArray();
                            if (arr != null)
                            {
                                for (i = 0; i < arr.Length; i++)
                                {
                                    Query.Set(arr[i], SM.ToStr(Request.Form[arr[i]]));
                                }
                            }
                        }
                    }
                }
            }
            //
            // get cookies
            //
            if (Request != null)
            {
                if (Request.Cookies != null)
                {
                    Cookies.FromJSON64(SM.ToStr(Request.Cookies["sm_cookies"]));
                }
            }
        }

        #endregion

        /* */

        #region Methods

        /*  ===================================================================
         *  Methods
         *  ===================================================================
         */

        /// <summary>Add parameter to query string.</summary>
        public string AddParameter(string _QueryString, string _Parameter, string _Value)
        {
            _QueryString = _QueryString.Trim();
            if (_QueryString.IndexOf('?') < 1) _QueryString += '?';
            else _QueryString += '&';
            return _QueryString + _Parameter.Trim() + '=' + System.Net.WebUtility.UrlEncode(_Value.Trim());
        }

        /// <summary>Ritorna l'URL di ritorno alla pagina precedente.</summary>
        public string BackUrl(string _BackUrl)
        {
            if (SM.Empty(_BackUrl)) _BackUrl = Query.ValueOf("sm_bku");
            if (SM.Empty(_BackUrl) && (Request != null)) _BackUrl = Request.QueryString.Value;
            if (SM.Empty(_BackUrl)) _BackUrl = "/Index";
            if (_BackUrl.IndexOf('?') < 0) _BackUrl += "?sm_q=" + Q();
            return _BackUrl;
        }

        /// <summary>Set cookie value.</summary>
        public bool CookieSet(string _CookieId, string _Value)
        {
            try
            {
                Cookies.Set(_CookieId, _Value);
                if (Response != null)
                {
                    if (Response.Cookies != null)
                    {
                        Response.Cookies.Append("sm_cookies", Cookies.ToJSON64());
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>Return value of cookie parameter, if not present return querty with same name.</summary>
        public string CookieOrQuery(string _IdParameter)
        {
            int i;
            string rslt = "";
            i = Cookies.Find(_IdParameter);
            if (i < 0) rslt = Query.ValueOf(_IdParameter);
            else rslt = Cookies[i].Value;
            return rslt;
        }

        /// <summary>Return full path of file name, on assembly base path.</summary>
        public string OnBasePath(string _FileName = "")
        {
            return Combine(BasePath, _FileName);
        }

        /// <summary>Return full path of file name, on application root path.</summary>
        public string OnRootPath(string _FileName = "")
        {
            return Combine(RootPath, _FileName);
        }

        /// <summary>Return url with page path and message, backurl and default parameters
        /// encoded in base 64.</summary>
        public string Page(string _PagePath, string _Message, string _BackUrl = "")
        {
            return AddParameter(_PagePath, "sm_q", Q(new string[] { "sm_msg", _Message, "sm_bku", BackUrl(_BackUrl) }));
        }

        /// <summary>Return base64 string serialization of JSON containing parameters represented 
        /// in key-values array (i.e. { "Key1", "Value1",... "KeyN","ValueN"}). 
        /// Function also add following parameters: sm_usr, sm_org, sm_tim, sm_rnd.</summary>
        public string Q(string[] _KeyValuesArray = null)
        {
            int i = 0, h = 0;
            long t = DateTime.Now.Ticks / TimeSpan.TicksPerSecond;
            SMDictionary dict = new SMDictionary();
            dict.Set("sm_usr", SM.User.UidUser);
            dict.Set("sm_org", SM.User.Organization.UidOrganization);
            dict.Set("sm_tim", t.ToString());
            if (Request != null) dict.Set("sm_bku", Request.QueryString.Value);
            if (_KeyValuesArray != null)
            {
                h = _KeyValuesArray.Length - 1;
                while (i < h)
                {
                    dict.Set(_KeyValuesArray[i], _KeyValuesArray[i + 1]);
                    i += 2;
                }
            }
            return dict.ToJSON64();
        }

        /// <summary>Return value of query parameter, if not present return cookie with same name.</summary>
        public string QueryOrCookie(string _IdParameter)
        {
            int i;
            string rslt = "";
            i = Query.Find(_IdParameter);
            if (i < 0) rslt = Cookies.ValueOf(_IdParameter);
            else rslt = Query[i].Value;
            return rslt;
        }

        #endregion

        /* */

        #region Static Methods

        /*  ===================================================================
         *  Static Methods
         *  ===================================================================
         */

        /// <summary>Return current instance of SMApplication or new if not found.</summary>
        public static SMFront CurrentOrNew(SMFront _SM = null, HttpContext _Context = null, string[] _Arguments = null, string _OEM = "", string _InternalPassword = "")
        {
            if (_SM != null) SM = _SM;
            else if (SM == null) SM = new SMFront(_Context, _Arguments, _OEM, _InternalPassword);
            return SM;
        }

        #endregion

        /* */

    }

    /* */

}
