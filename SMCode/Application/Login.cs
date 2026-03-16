/*  ===========================================================================
 *  
 *  File:       Login.cs
 *  Version:    2.0.325
 *  Date:       March 2026
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2026 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *
 *  SMCode application class: login functions.
 *
 *  ===========================================================================
 */

using System;

namespace SMCodeSystem
{

    /* */

    /// <summary>SMCode application class: login functions.</summary>
    public partial class SMCode
    {

        /* */

        #region Delegates and events

        /*  ===================================================================
         *  Delegates and events
         *  ===================================================================
         */

        /// <summary>Occurs when login event succeed.</summary>
        public event SMOnLogin OnLoginEvent = null;

        #endregion

        /* */

        #region Methods

        /*  ===================================================================
         *  Methods
         *  ===================================================================
         */

        /// <summary>Perform user login with user-id and password. Log details can be specified as parameters.
        /// Return 1 if success, 0 if fail or -1 if error.</summary>
        public int Login(string _UserId, string _Password, string _LogDetails = "")
        {
            if (User.LoadByCredentials(_UserId, _Password)<0) return -1;
            else return LoginEvent(User, _LogDetails);  
        }

        /// <summary>Perform user login with id. Log details can be specified as parameters.
        /// Return user id if success, 0 if fail or -1 if error.</summary>
        public int LoginById(int _Id, string _LogDetails = "")
        {
            if (User.LoadById(_Id) < 0) return -1;
            else return LoginEvent(User, _LogDetails);
        }

        /// <summary>Perform user login by tax code. Log details can be specified as parameters.
        /// Return user id if success, 0 if fail or -1 if error.</summary>
        public int LoginByTaxCode(string _TaxCode, string _LogDetails = "")
        {
            string sql = $"SELECT * FROM {SMDefaults.UsersTableName} WHERE (TaxCode={Quote(_TaxCode)})AND{SqlNotDeleted(SMDefaults.UsersTableName_Deleted)}";
            if (User.Load(sql) < 0) return -1;
            else return LoginEvent(User, _LogDetails);
        }

        /// <summary>Perform user login with uid. Log details can be specified as parameters.
        /// Return user id if success, 0 if fail or -1 if error.</summary>
        public int LoginByUid(string _Uid, string _LogDetails = "")
        {
            if (User.LoadByUid(_Uid) < 0) return -1;
            else return LoginEvent(User, _LogDetails);
        }

        /// <summary>Perform login event if defined with passed user.
        /// Return user id if success, 0 if fail or -1 if error.</summary>
        public int LoginEvent(SMUser _User, string _LogDetails = "")
        {
            SMLogItem log;
            if (_User == null) return -1;
            else if (_User.IdUser > 0)
            {
                log = new SMLogItem();
                log.DateTime = DateTime.Now;
                log.Application = ExecutableName;
                log.Version = Cat(Version, ToStr(ExecutableDate, true), " - ");
                log.IdUser = User.IdUser;
                log.UidUser = User.UidUser;
                log.LogType = SMLogType.Login;
                log.Action = "LOGIN";
                log.Message = "User " + _User.UserName + " logged.";
                log.Details = _LogDetails;
                if (LoginEvent(log, _User)) return _User.IdUser;
                else return 0;
            }
            else return 0;
        }

        /// <summary>Perform login event if defined.</summary>
        public bool LoginEvent(SMLogItem _LogItem, SMUser _User)
        {
            bool rslt = true;
            if (OnLoginEvent != null) OnLoginEvent(_LogItem, _User, ref rslt);
            if (rslt) Log(_LogItem);
            else Log(SMLogType.Error, "Unauthorized user.", "", "");
            return rslt;
        }

        #endregion

        /* */

    }

    /* */

}
