/*  ===========================================================================
 *  
 *  File:       Web.cs
 *  Version:    2.0.18
 *  Date:       May 2024
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2024 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *
 *  Web functions class.
 *  
 *  ===========================================================================
 */

using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SMCodeSystem
{

    /* */

    /// <summary>Web functions class.</summary>
    public partial class SMCode
    {

        /* */

        #region Methods

        /*  ===================================================================
         *  Methods
         *  ===================================================================
         */

        /// <summary>Call OS shell to browse url.</summary>
        public bool Browse(string _URL)
        {
            bool r = false;
            _URL = _URL.Trim();
            if (_URL.Length > 0)
            {
                if (_URL.ToLower().StartsWith("www.")) _URL = "http://" + _URL;
                r = RunShell(_URL, "", false, false);
            }
            return r;
        }

        /// <summary>Returns IP of host name resolved calling DNS.
        /// If expiration days is greater than 0 function will return cached
        /// result if not elapses these days.</summary>
        public string DnsIP(string _Host, int _ExpirationDays)
        {
            IPAddress[] ips;
            string r = CacheRead("DNS_IP", _Host, _ExpirationDays);
            if (((r.Length < 1) || (_ExpirationDays < 1)) && NetworkAvailable())
            {
                try
                {
                    ips = Dns.GetHostAddresses(_Host);
                }
                catch (Exception ex)
                {
                    ips = null;
                    Error(ex);
                }
                if (ips != null)
                {
                    if (ips.Length > 0)
                    {
                        r = ips[0].ToString();
                        CacheWrite("DNS_IP", _Host, r);
                    }
                }
            }
            return r;
        }

        /// <summary>Download URL from web and save it in to localFile file.
        /// Returns true if succeed.</summary>
        public bool Download(string _URL, string _LocalFile)
        {
            SecurityProtocolType spt = ServicePointManager.SecurityProtocol;
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
            WebClient wc = new WebClient();
            try
            {
                FileDelete(_LocalFile);
                wc.DownloadFile(_URL, _LocalFile);
                wc.Dispose();
                ServicePointManager.SecurityProtocol = spt;
                return FileExists(_LocalFile);
            }
            catch (Exception ex)
            {
                Error(ex);
                ServicePointManager.SecurityProtocol = spt;
                return false;
            }
        }

        /// <summary>Send web request to URL with GET method and get reply. Return true if succeed.</summary>
        public bool Download(string _URL, ref string _Reply)
        {
            bool r;
            byte[] bytes;
            WebClient wc;
            SecurityProtocolType spt = ServicePointManager.SecurityProtocol;
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
            _Reply = "";
            try
            {
                wc = new WebClient();
                bytes = wc.DownloadData(_URL);
                _Reply = Encoding.UTF8.GetString(bytes);
                wc.Dispose();
                r = true;
            }
            catch (Exception ex)
            {
                Error(ex);
                r = false;
            }
            ServicePointManager.SecurityProtocol = spt;
            return r;
        }

        /// <summary>Asyncronous download of URL from web in to localFile file.
        /// Returns true if succeed. Using domain name DNS will be used blocking main thread
        /// for a while. Using direct IP reference function will be truly asyncronous.</summary>
        public bool DownloadAsync(string _URL, string _LocalFile,
            DownloadProgressChangedEventHandler _DownloadProgressChangedEventHandler,
            AsyncCompletedEventHandler _AsyncCompletedEventHandler)
        {
            SecurityProtocolType spt = ServicePointManager.SecurityProtocol;
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
            WebClient wc = new WebClient();
            Uri u = new Uri(_URL);
            try
            {
                FileDelete(_LocalFile);
                if (_DownloadProgressChangedEventHandler != null) wc.DownloadProgressChanged += _DownloadProgressChangedEventHandler;
                if (_AsyncCompletedEventHandler != null) wc.DownloadFileCompleted += _AsyncCompletedEventHandler;
                wc.DownloadFileAsync(u, _LocalFile);
                ServicePointManager.SecurityProtocol = spt;
                return true;
            }
            catch (Exception ex)
            {
                Error(ex);
                ServicePointManager.SecurityProtocol = spt;
                return false;
            }
        }

        /// <summary>Send web request to url with GET method and perform callback event when completed.</summary>
        public bool DownloadAsync(string _URL, DownloadDataCompletedEventHandler _DownloadDataCompletedEventHandler)
        {
            bool r;
            WebClient wc;
            SecurityProtocolType spt = ServicePointManager.SecurityProtocol;
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
            try
            {
                wc = new WebClient();
                wc.DownloadDataCompleted += _DownloadDataCompletedEventHandler;
                wc.DownloadDataAsync(new Uri(_URL));
                r = true;
            }
            catch (Exception ex)
            {
                Error(ex);
                r = false;
            }
            ServicePointManager.SecurityProtocol = spt;
            return r;
        }

        /// <summary>Download URL from web and return it as string.
        /// If function fails return empty string.</summary>
        public string DownloadString(string _URL)
        {
            StringBuilder r = new StringBuilder();
            SecurityProtocolType spt = ServicePointManager.SecurityProtocol;
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
            WebClient wc = new WebClient();
            try
            {
                Stream st = wc.OpenRead(_URL);
                StreamReader sr = new StreamReader(st);
                string ln = "";
                while (ln != null)
                {
                    ln = sr.ReadLine();
                    if (ln != null) r.AppendLine(ln);
                }
                st.Close();
                st.Dispose();
                wc.Dispose();
                ServicePointManager.SecurityProtocol = spt;
                return r.ToString();
            }
            catch (Exception ex)
            {
                Error(ex);
                ServicePointManager.SecurityProtocol = spt;
                return "";
            }
        }

        /// <summary>Return URL adding http:// at start if not present any prefix.</summary>
        public string HttpPrefix(string _URL)
        {
            if (_URL.IndexOf("://") < 0) return "http://" + _URL;
            else return _URL;
        }

        /// <summary>Returns list of first or all local machine IP separed by ";".</summary>
        public string LocalIP(bool _GetAllIP)
        {
            int i, h;
            string r = "";
            IPHostEntry host;
            IPAddress[] a;
            try
            {
                host = Dns.GetHostEntry(Dns.GetHostName());
                a = host.AddressList;
                if (a != null)
                {
                    h = a.Length;
                    i = 0;
                    while (((r.Length < 1) || (_GetAllIP)) && (i < h))
                    {
                        if (a[i].AddressFamily == AddressFamily.InterNetwork)
                        {
                            if (r.Length > 0) r += ";";
                            r += a[i].ToString();
                        }
                        i++;
                    }
                }
            }
            catch (Exception ex)
            {
                Error(ex);
            }
            return r;
        }

        /// <summary>Send web request to url with POST method and get reply. Return true if succeed.
        /// Values array of parameters must have the form [parameter1],[value1]..[parameterN],[valueN]</summary>
        public bool Post(string _URL, string[] _Values, ref string _Reply)
        {
            bool r = false;
            int i, h;
            byte[] bytes;
            WebClient wc;
            SecurityProtocolType spt = ServicePointManager.SecurityProtocol;
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
            NameValueCollection vals = new NameValueCollection();
            _Reply = "";
            if (_Values != null)
            {
                //
                // load values collection
                //
                h = _Values.Length;
                i = 0;
                while (i < h - 1)
                {
                    vals.Add(_Values[i], _Values[i + 1]);
                    i += 2;
                }
                //
                // try request
                //
                try
                {
                    wc = new WebClient();
                    wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                    bytes = wc.UploadValues(_URL, "POST", vals);
                    _Reply = Encoding.UTF8.GetString(bytes);
                    wc.Dispose();
                    r = true;
                }
                catch (Exception ex)
                {
                    Error(ex);
                    r = false;
                }
            }
            ServicePointManager.SecurityProtocol = spt;
            return r;
        }

        /// <summary>Send web request to url with POST method and perform callback event when completed. 
        /// Values array of parameters must have the form [parameter1],[value1]..[parameterN],[valueN]</summary>
        public bool PostAsync(string _URL, string[] _Values, UploadValuesCompletedEventHandler _UploadValuesCompletedEventHandler)
        {
            bool r = false;
            int i, h;
            WebClient wc;
            SecurityProtocolType spt = ServicePointManager.SecurityProtocol;
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
            NameValueCollection vals = new NameValueCollection();
            if (_Values != null)
            {
                //
                // load values collection
                //
                h = _Values.Length;
                i = 0;
                while (i < h - 1)
                {
                    vals.Add(_Values[i], _Values[i + 1]);
                    i += 2;
                }
                //
                // try request
                //
                try
                {
                    wc = new WebClient();
                    wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                    if (_UploadValuesCompletedEventHandler != null) wc.UploadValuesCompleted += _UploadValuesCompletedEventHandler;
                    wc.UploadValuesAsync(new Uri(_URL), "POST", vals);
                    r = true;
                }
                catch (Exception ex)
                {
                    Error(ex);
                    r = false;
                }
            }
            ServicePointManager.SecurityProtocol = spt;
            return r;
        }

        /// <summary>Upload local file content to remote URL on web. Returns true if succeed.</summary>
        public bool Upload(string _RemoteURL, string _LocalFile,
            UploadProgressChangedEventHandler _UploadProgressChangedEventHandler)
        {
            SecurityProtocolType spt = ServicePointManager.SecurityProtocol;
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
            WebClient wc = new WebClient();
            try
            {
                wc.UploadFile(_RemoteURL, _LocalFile);
                if (_UploadProgressChangedEventHandler != null) wc.UploadProgressChanged += _UploadProgressChangedEventHandler;
                wc.Dispose();
                ServicePointManager.SecurityProtocol = spt;
                return true;
            }
            catch (Exception ex)
            {
                Error(ex);
                ServicePointManager.SecurityProtocol = spt;
                return false;
            }
        }

        /// <summary>Upload localFile content to remote URL on web. Returns true if succeed.</summary>
        public bool UploadAsync(string _RemoteURL, string _LocalFile,
            UploadProgressChangedEventHandler _UploadProgressChangedEventHandler,
            UploadFileCompletedEventHandler _UploadFileCompletedEventHandler)
        {
            SecurityProtocolType spt = ServicePointManager.SecurityProtocol;
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
            WebClient wc = new WebClient();
            try
            {
                if (_UploadProgressChangedEventHandler != null) wc.UploadProgressChanged += _UploadProgressChangedEventHandler;
                if (_UploadFileCompletedEventHandler != null) wc.UploadFileCompleted += _UploadFileCompletedEventHandler;
                wc.UploadFileAsync(new Uri(_RemoteURL), _LocalFile);
                wc.Dispose();
                ServicePointManager.SecurityProtocol = spt;
                return true;
            }
            catch (Exception ex)
            {
                Error(ex);
                ServicePointManager.SecurityProtocol = spt;
                return false;
            }
        }

        #endregion

        /* */

    }

    /* */

}