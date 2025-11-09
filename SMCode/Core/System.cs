/*  ===========================================================================
 *  
 *  File:       System.cs
 *  Version:    2.0.0
 *  Date:       February 2024
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2024 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *
 *  SMCode application class: system.
 *
 *  ===========================================================================
 */

using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace SMCodeSystem
{

    /* */

    /// <summary>SMCode application class: system.</summary>
    public partial class SMCode
    {

        /* */

        #region Events

        /*  ===================================================================
         *  Events
         *  ===================================================================
         */

        /// <summary>On do events event.</summary>
        public SMOnEvent OnDoEvents = null;

        #endregion

        /* */

        #region Properties

        /*  ===================================================================
         *  Properties
         *  ===================================================================
         */

        /// <summary>Get or set last do events datetime.</summary>
        public DateTime DoEventsLast { get; set; } = DateTime.Now;

        /// <summary>Get or set default delay timing (in seconds) for DoEvents() function.</summary>
        public double DoEventsTiming { get; set; } = 0.8d;

        /// <summary>Get or set application force exit system flag.</summary>
        public bool ForceExit { get; set; } = false;

        /// <summary>Get or set memory release delay in seconds.</summary>
        public double MemoryReleaseDelay { get; set; } = 4.0d;

        /// <summary>Get or set next memory release time.</summary>
        public DateTime MemoryReleaseNext { get; set; } = DateTime.MinValue;

        /// <summary>Get or set stop now system flag.</summary>
        public bool StopNow { get; set; } = false;

        #endregion

        /* */

        #region Methods

        /*  ===================================================================
         *  Methods
         *  ===================================================================
         */

        /// <summary>Set current directory to dir path.</summary>
        public bool ChangeDir(string _Path)
        {
            try
            {
                if (_Path.Trim().Length > 0) System.Environment.CurrentDirectory = _Path.Trim();
                return true;
            }
            catch (Exception ex)
            {
                Error(ex);
                return false;
            }
        }

        /// <summary>Returns full path of actual current directory.</summary>
        public string CurrentDir()
        {
            try
            {
                return System.Environment.CurrentDirectory;
            }
            catch (Exception ex)
            {
                Error(ex);
                return "";
            }
        }

        /// <summary>Process application events if since last time are passed at least 
        /// passed seconds or with default timing if omitted.</summary>
        public bool DoEvents(double _DoEventsDelay = -1)
        {
            if (_DoEventsDelay < 0) _DoEventsDelay = DoEventsTiming;
            if (DateTime.Now > DoEventsLast.AddSeconds(_DoEventsDelay))
            {
                if (OnDoEvents != null) OnDoEvents();
                DoEventsLast = DateTime.Now;
                return true;
            }
            else return false;
        }

        /// <summary>Return drive info.</summary>
        public DriveInfo DriveInfo(char _Drive)
        {
            int i = 0;
            DriveInfo r = null;
            DriveInfo[] drives;
            try
            {
                drives = System.IO.DriveInfo.GetDrives();
                if (drives != null)
                {
                    while ((r == null) && (i < drives.Length))
                    {
                        if (drives[i].Name == _Drive + @":\") r = drives[i];
                        i++;
                    }
                }
            }
            catch (Exception ex)
            {
                Error(ex);
                r = null;
            }
            return r;
        }

        /// <summary>Return drive label.</summary>
        public string DriveLabel(char _Drive)
        {
            DriveInfo di;
            try
            {
                di = DriveInfo(_Drive);
                if (di != null) return di.VolumeLabel;
                else return "";
            }
            catch (Exception ex)
            {
                Error(ex);
                return "";
            }
        }

        /// <summary>Return drive size or -1 if error.</summary>
        public long DriveSize(char _Drive)
        {
            DriveInfo di;
            try
            {
                di = DriveInfo(_Drive);
                if (di != null) return di.TotalSize;
                else return 0;
            }
            catch (Exception ex)
            {
                Error(ex);
                return -1;
            }
        }

        /// <summary>Return drive free space or -1 if error.</summary>
        public long DriveSpace(char _Drive)
        {
            DriveInfo di;
            try
            {
                di = DriveInfo(_Drive);
                if (di != null) return di.TotalFreeSpace;
                else return 0;
            }
            catch (Exception ex)
            {
                Error(ex);
                return -1;
            }
        }

        /// <summary>Return a string with local IP.</summary>
        public string LocalIP()
        {
            string r = "";
            IPHostEntry host;
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    r = ip.ToString();
                }
            }
            return r;
        }

        /// <summary>Returns the name of local computer where runs application.</summary>
        public string Machine()
        {
            try
            {
                return System.Environment.MachineName;
            }
            catch (Exception ex)
            {
                Error(ex);
                return "";
            }
        }

        /// <summary>Call OS shell to send an email to address.</summary>
        public bool MailTo(string _Address, string _BCC)
        {
            bool r = false;
            _Address = _Address.Trim();
            if (IsEmail(_Address, false))
            {
                if (!_Address.ToLower().StartsWith("mailto:")) _Address = "mailto:" + _Address;
                if (_BCC.Trim().Length > 0)
                {
                    if (IsEmail(_BCC, false)) _Address += "?bcc=" + _BCC;
                }
                r = RunShell(_Address, "", false, false);
            }
            return r;
        }

        /// <summary>Call Garbage Collector memory collect if elapsed 
        /// MemoryReleaseDelay seconds from last.</summary>
        public bool MemoryRelease(bool _ForceRelease)
        {
            if (!_ForceRelease || (DateTime.Now > MemoryReleaseNext))
            {
                try
                {
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    MemoryReleaseNext = DateTime.Now.AddSeconds(MemoryReleaseDelay);
                    DoEvents();
                    return true;
                }
                catch (Exception ex)
                {
                    Error(ex);
                    DoEvents();
                    return false;
                }
            }
            else return false;
        }

        /// <summary>Return true if any network connection interface is available.</summary>
        public bool NetworkAvailable()
        {
            return System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();
        }

        /// <summary>Returns a string with current OS version description as: 95, 98, 98SE
        /// ME, NT, 2000, XP, VISTA, WINDOWS7, WINDOWS8, WINDOWS10 or empty string if fails.
        /// To let function work properly add application manifest file and remove 
        /// remarks on windows 8 and windows 10 compatibility items.</summary>
        public string OS()
        {
            string r = "";
            OperatingSystem oi = System.Environment.OSVersion;
            /* Caso piattaforma Windows 95, 98, 98 SE, ME */
            if (oi.Platform == System.PlatformID.Win32Windows)
            {
                if (oi.Version.Minor == 0) r = "95";
                else if (oi.Version.Minor == 10)
                {
                    if (oi.Version.Revision.ToString() == "2222A") r = "98SE";
                    else r = "98";
                }
                else if (oi.Version.Minor == 90) r = "ME";
            }
            /* Caso piattaforma Windows NT 3.51, NT 4.0, 2000, XP, VISTA, WINDOWS7, WINDOWS8 e WINDOWS10 */
            else if (oi.Platform == System.PlatformID.Win32NT)
            {
                if (oi.Version.Major == 3) r = "NT 3.51";
                else if (oi.Version.Major == 4) r = "NT 4.0";
                else if (oi.Version.Major == 5)
                {
                    if (oi.Version.Minor == 0) r = "2000";
                    else if (oi.Version.Minor == 1) r = "XP";
                    else r = "XP";
                }
                else if (oi.Version.Major == 6)
                {
                    if (oi.Version.Minor == 0) r = "VISTA";
                    else if (oi.Version.Minor == 1) r = "WINDOWS 7";
                    else if (oi.Version.Minor == 2) r = "WINDOWS 8";
                    else r = "WINDOWS 8.1";
                }
                else if (oi.Version.Major == 10) r = "WINDOWS 10";
                if (System.Environment.Is64BitOperatingSystem) r += " 64bit";
            }
            if (r.Trim().Length > 0) r += " (" + oi.Version.ToString() + ")";
            return r;
        }

        /// <summary>Exec a ping forward hostAddress with timeOut seconds for max waiting.
        /// Returns true if succeed.</summary>
        public bool Ping(string _HostAddress, int _TimeOut)
        {
            bool r = false;
            if (_HostAddress != "")
            {
                try
                {
                    Ping pg = new Ping();
                    PingReply rp = pg.Send(_HostAddress, _TimeOut);
                    r = rp.Status == IPStatus.Success;
                    pg.Dispose();
                }
                catch (Exception ex)
                {
                    Error(ex);
                    r = false;
                }
            }
            return r;
        }

        /// <summary>Execute application located in the path fileName, passing argumentList parameters.
        /// If runAndWait is setted to true the program wait the launched application termination.
        /// The function returns true if succeed.</summary>
        public bool RunShell(string _FileName, string _ArgumentList, bool _RunAndWait, bool _AsAdministrator)
        {
            Process p;
            try
            {
                p = new Process();
                p.EnableRaisingEvents = false;
                p.StartInfo.FileName = _FileName;
                p.StartInfo.Arguments = _ArgumentList;
                if (_AsAdministrator) p.StartInfo.Verb = "runas";
                p.Start();
                if (_RunAndWait) p.WaitForExit();
                return true;
            }
            catch (Exception ex)
            {
                Error(ex);
                return false;
            }
        }

        /// <summary>Open via shell the file specified in fileName path with shellVerb operation.
        /// If runAndWait is setted to true the program wait the launched application termination.
        /// The function returns true if succeed.</summary>
        public bool RunShellVerb(string _FileName, string _ShellVerb, bool _RunAndWait, bool _AsAdministrator = false)
        {
            Process p;
            try
            {
                p = new Process();
                p.StartInfo.FileName = _FileName;
                p.StartInfo.Verb = _ShellVerb;
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                p.StartInfo.UseShellExecute = true;
                p.StartInfo.WorkingDirectory = FilePath(_FileName);
                if (_AsAdministrator) p.StartInfo.Verb = "runas";
                p.Start();
                if (_RunAndWait) p.WaitForExit();
                return true;
            }
            catch (Exception ex)
            {
                Error(ex);
                return false;
            }
        }

        /// <summary>Returns the name of user currently logged to system.</summary>
        public string SystemUser()
        {
            try
            {
                return System.Environment.UserName;
            }
            catch (Exception ex)
            {
                Error(ex);
                return "";
            }
        }

        /// <summary>Wait for seconds.</summary>
        public void Wait(double _Seconds, bool _DoEvents = false)
        {
            DateTime t = DateTime.Now.AddSeconds(_Seconds);
            while (DateTime.Now < t) if (_DoEvents) DoEvents();
        }

        #endregion

        /* */

    }

    /* */

}
