/*  ===========================================================================
 *  
 *  File:       SMFtp.cs
 *  Version:    2.0.124
 *  Date:       January 2025
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2025 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *
 *  FTP management functions.
 *  
 *  ===========================================================================
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace SMCodeSystem
{

    /* */

    /// <summary>FTP management functions.</summary>
    public class SMFtp
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

        #region Initialization

        /*  ===================================================================
         *  Initialization
         *  ===================================================================
         */

        /// <summary>Class constructor.</summary>
        public SMFtp(SMCode _SM = null)
        {
            SM = SMCode.CurrentOrNew(_SM);
        }

        #endregion

        /* */

        #region Methods - Download

        /*  ===================================================================
         *  Methods - Download
         *  ===================================================================
         */

        /// <summary>Download remote file from default FTP account and save it in to local file.
        /// Returns true if succeed.</summary>
        public bool Download(string _FullRemotePath, string _LocalFile, SMOnProgress _ProgressFunction)
        {
            return Download(_FullRemotePath, _LocalFile, new SMFtpAccount(""), _ProgressFunction);
        }

        /// <summary>Download remote file from FTP account and save it in to local file.
        /// Returns true if succeed.</summary>
        public bool Download(string _FullRemotePath, string _LocalFile,
            SMFtpAccount _Account, SMOnProgress _ProgressFunction)
        {
            if (_Account.Empty) return false;
            else return Download(_FullRemotePath, _LocalFile, _Account.Host, _Account.User,
                _Account.Password, _Account.Port, _Account.SSL, _Account.Passive, _Account.Binary,
                _Account.KeepAlive, _ProgressFunction);
        }

        /// <summary>Download remote file from FTP host and save it in to local file.
        /// Login to FTP is done with user and password. If progress event function specified 
        /// progress readed bytes count will be passed. Returns true if succeed.</summary>
        public bool Download(string _FullRemotePath, string _LocalFile, string _Host,
            string _User, string _Password, int _Port, bool _SSL, bool _Passive, bool _Binary,
            bool _KeepAlive, SMOnProgress _ProgressFunction)
        {
            int i;
            long p;
            bool r = false, stop = false;
            byte[] buf;
            FtpWebRequest rq;
            FtpWebResponse rp;
            Stream st;
            FileStream os;
            //
            try
            {
                buf = new byte[4096];
                rq = (FtpWebRequest)WebRequest.Create(new Uri("ftp://" + _Host
                    + SM.Iif((_Port > 0) && (_Port != 21), ":" + _Port.ToString(), "")
                    + "/" + _FullRemotePath));
                rq.Credentials = new NetworkCredential(_User, _Password);
                rq.Method = WebRequestMethods.Ftp.DownloadFile;
                rq.KeepAlive = _KeepAlive;
                rq.UsePassive = _Passive;
                rq.EnableSsl = _SSL;
                rq.UseBinary = _Binary;
                rp = (FtpWebResponse)rq.GetResponse();
                st = rp.GetResponseStream();
                os = new FileStream(_LocalFile, FileMode.Create);
                if (os != null)
                {
                    r = true;
                    try
                    {
                        p = 0;
                        i = st.Read(buf, 0, buf.Length);
                        while (!stop && (i > 0))
                        {
                            os.Write(buf, 0, i);
                            i = st.Read(buf, 0, buf.Length);
                            p += i;
                            if (_ProgressFunction != null) _ProgressFunction(p, ref stop);
                        }
                    }
                    catch (Exception ex)
                    {
                        SM.Error(ex);
                        r = false;
                    }
                    os.Close();
                    os.Dispose();
                }
                st.Close();
                st.Dispose();
                rp.Close();
            }
            catch (Exception ex)
            {
                SM.Error(ex);
            }
            return r;
        }

        #endregion

        /* */

        #region Methods - Upload

        /*  ===================================================================
         *  Methods - Upload
         *  ===================================================================
         */

        /// <summary>Upload local file to default FTP account and save it in to full remote path.
        /// Returns true if succeed.</summary>
        public bool Upload(string _LocalFile, string _FullRemotePath, SMOnProgress _ProgressFunction)
        {
            return Upload(_LocalFile, _FullRemotePath, new SMFtpAccount("", SM), _ProgressFunction);
        }

        /// <summary>Upload local file to FTP account and save it in to full remote path.
        /// Returns true if succeed.</summary>
        public bool Upload(string _LocalFile, string _FullRemotePath, SMFtpAccount _Account, SMOnProgress _ProgressFunction)
        {
            if (_Account.Empty) return false;
            else return Upload(_LocalFile, _FullRemotePath, _Account.Host, _Account.User,
                _Account.Password, _Account.Port, _Account.SSL, _Account.Passive, _Account.Binary,
                _Account.KeepAlive, _ProgressFunction);
        }

        /// <summary>Upload local file to FTP host and save it in to full remote path.
        /// Login to FTP is done with user and password. If progress event function specified 
        /// progress percentage will be passed. Returns true if succeed.</summary>
        public bool Upload(string _LocalFile, string _FullRemotePath, string _Host, string _UserID, string _Password,
            int _Port, bool _SSL, bool _Passive, bool _Binary, bool _KeepAlive, SMOnProgress _ProgressFunction)
        {
            int i;
            long p;
            bool r, stop = false;
            byte[] buf;
            FileInfo fi;
            FtpWebRequest rq;
            FileStream sr;
            Stream st;
            //
            try
            {
                buf = new byte[4096];
                fi = new FileInfo(_LocalFile);
                rq = (FtpWebRequest)WebRequest.Create(new Uri("ftp://" + _Host
                    + SM.Iif((_Port > 0) && (_Port != 21), ":" + _Port.ToString(), "")
                    + "/" + _FullRemotePath));
                rq.Credentials = new NetworkCredential(_UserID, _Password);
                rq.Method = WebRequestMethods.Ftp.UploadFile;
                rq.KeepAlive = _KeepAlive;
                rq.UsePassive = _Passive;
                rq.EnableSsl = _SSL;
                rq.UseBinary = _Binary;
                rq.ContentLength = fi.Length;
                sr = fi.OpenRead();
                st = rq.GetRequestStream();
                r = true;
                try
                {
                    p = 0;
                    i = sr.Read(buf, 0, buf.Length);
                    while (!stop && (i > 0))
                    {
                        st.Write(buf, 0, i);
                        p += i;
                        if (_ProgressFunction != null) _ProgressFunction(SM.Percent(p, fi.Length, true), ref stop);
                        i = sr.Read(buf, 0, buf.Length);
                    }
                }
                catch (Exception ex)
                {
                    SM.Error(ex);
                    r = false;
                }
                st.Close();
                sr.Close();
                st.Dispose();
                sr.Dispose();
            }
            catch (Exception ex)
            {
                r = false;
                SM.Error(ex);
            }
            return r;
        }

        #endregion

        /* */

        #region Methods - List

        /*  ===================================================================
         *  Methods - List
         *  ===================================================================
         */

        /// <summary>Returns a string list filled of file names on default FTP account server and located in full remote path.</summary>
        public List<string> List(string _FullRemotePath)
        {
            return List(new SMFtpAccount("", SM), _FullRemotePath);
        }

        /// <summary>Returns a string list filled of file names on FTP account server and located in full remote path.</summary>
        public List<string> List(SMFtpAccount _Account, string _FullRemotePath)
        {
            if (_Account.Empty) return null;
            else return List(_FullRemotePath, _Account.Host, _Account.User,
                _Account.Password, _Account.Port, _Account.SSL, _Account.Passive, _Account.Binary,
                _Account.KeepAlive);
        }

        /// <summary>Returns a string list filled of file names on ftpServer, with ftpUserID and ftpPassword
        /// login and located in ftpFullRemotePath. Return null if fails.</summary>
        public List<string> List(string _FullRemotePath, string _Host, string _User, string _Password,
            int _Port, bool _SSL, bool _Passive, bool _Binary, bool _KeepAlive)
        {
            string ln;
            List<string> r = new List<string>();
            FtpWebRequest rq;
            FtpWebResponse rp;
            Stream st;
            StreamReader sr;
            //
            try
            {
                rq = (FtpWebRequest)WebRequest.Create(new Uri("ftp://" + _Host
                    + SM.Iif((_Port > 0) && (_Port != 21), ":" + _Port.ToString(), "")
                    + "/" + _FullRemotePath));
                rq.Credentials = new NetworkCredential(_User, _Password);
                rq.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                rq.KeepAlive = _KeepAlive;
                rq.UsePassive = _Passive;
                rq.EnableSsl = _SSL;
                rq.UseBinary = _Binary;
                rp = (FtpWebResponse)rq.GetResponse();
                st = rp.GetResponseStream();
                sr = new StreamReader(st);
                ln = sr.ReadLine();
                while (ln != null)
                {
                    r.Add(ln.Trim());
                    ln = sr.ReadLine();
                }
                r.Add(rp.StatusDescription);
                sr.Close();
                rp.Close();
                sr.Dispose();
            }
            catch (Exception ex)
            {
                SM.Error(ex);
                r = null;
            }
            return r;
        }

        #endregion

        /* */

        #region Methods - Delete

        /*  ===================================================================
         *  Methods - Delete
         *  ===================================================================
         */

        /// <summary>Delete remote file on default FTP account server.</summary>
        public bool Delete(string _FullRemotePath)
        {
            return Delete(_FullRemotePath, new SMFtpAccount("", SM));
        }

        /// <summary>Delete remote file file on account server.</summary>
        public bool Delete(string _FullRemotePath, SMFtpAccount _Account)
        {
            if (_Account.Empty) return false;
            else return Delete(_FullRemotePath, _Account.Host, _Account.User,
                _Account.Password, _Account.Port, _Account.SSL, _Account.Passive, _Account.Binary,
                _Account.KeepAlive);
        }

        /// <summary>Delete remote file to server.</summary>
        public bool Delete(string _FullRemotePath, string _Host, string _User, string _Password,
            int _Port, bool _SSL, bool _Passive, bool _Binary, bool _KeepAlive)
        {
            FtpWebRequest rq;
            FtpWebResponse rp;
            //
            try
            {
                rq = (FtpWebRequest)WebRequest.Create(new Uri("ftp://" + _Host
                    + SM.Iif((_Port > 0) && (_Port != 21), ":" + _Port.ToString(), "")
                    + "/" + _FullRemotePath));
                rq.Credentials = new NetworkCredential(_User, _Password);
                rq.Method = WebRequestMethods.Ftp.DeleteFile;
                rq.KeepAlive = _KeepAlive;
                rq.UsePassive = _Passive;
                rq.EnableSsl = _SSL;
                rq.UseBinary = _Binary;
                rp = (FtpWebResponse)rq.GetResponse();
                rp.Close();
                return true;
            }
            catch (Exception ex)
            {
                SM.Error(ex);
                return false;
            }
        }

        #endregion

        /* */

        #region Methods - Rename

        /*  ===================================================================
         *  Methods - Rename
         *  ===================================================================
         */

        /// <summary>Rename remote file on default FTP account server.</summary>
        public bool Rename(string _FullRemotePath, string _RenameTo)
        {
            return Rename(_FullRemotePath, _RenameTo, new SMFtpAccount("", SM));
        }

        /// <summary>Delete remote file on FTP account server.</summary>
        public bool Rename(string _FullRemotePath, string _RenameTo, SMFtpAccount _Account)
        {
            if (_Account.Empty) return false;
            else return Rename(_FullRemotePath, _RenameTo, _Account.Host, _Account.User,
                _Account.Password, _Account.Port, _Account.SSL, _Account.Passive, _Account.Binary,
                _Account.KeepAlive);
        }

        /// <summary>Rename ftpFullRemotePath file to server.</summary>
        public bool Rename(string _FullRemotePath, string _RenameTo, string _Host, string _User,
            string _Password, int _Port, bool _SSL, bool _Passive, bool _Binary, bool _KeepAlive)
        {
            bool r;
            FtpWebRequest rq;
            FtpWebResponse rp;
            //
            try
            {
                rq = (FtpWebRequest)WebRequest.Create(new Uri("ftp://" + _Host
                    + SM.Iif((_Port > 0) && (_Port != 21), ":" + _Port.ToString(), "")
                    + "/" + _FullRemotePath));
                rq.Credentials = new NetworkCredential(_User, _Password);
                rq.Method = WebRequestMethods.Ftp.Rename;
                rq.RenameTo = _RenameTo;
                rq.KeepAlive = _KeepAlive;
                rq.UsePassive = _Passive;
                rq.EnableSsl = _SSL;
                rq.UseBinary = _Binary;
                rp = (FtpWebResponse)rq.GetResponse();
                r = rp.StatusCode == FtpStatusCode.FileActionOK;
                rp.Close();
            }
            catch (Exception ex)
            {
                SM.Error(ex);
                r = false;
            }
            return r;
        }

        #endregion

        /* */

    }

    /* */

}
