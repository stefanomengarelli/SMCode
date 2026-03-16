/*  ===========================================================================
 *  
 *  File:       Login.cs
 *  Version:    2.0.324
 *  Date:       March 2026
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2024 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *
 *  SMCode application class: login functions.
 *
 *  ===========================================================================
 */

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
        public int Login(string _UserId, string _Password)
        {
            return User.LoadByCredentials(_UserId, _Password);
        }

        /// <summary>Perform user login with id. Log details can be specified as parameters.
        /// Return 1 if success, 0 if fail or -1 if error.</summary>
        public int LoginById(int _Id, string _Details = "")
        {
            return User.LoadById(_Id, _Details);
        }

        /// <summary>Perform user login by tax code. Log details can be specified as parameters.
        /// Return 1 if success, 0 if fail or -1 if error.</summary>
        public int LoginByTaxCode(string _TaxCode, string _Details = "")
        {
            return User.Load($"SELECT * FROM {SMDefaults.UsersTableName} WHERE (TaxCode={Quote(_TaxCode)})AND{SqlNotDeleted()}", _Details);
        }

        /// <summary>Perform user login with uid. Log details can be specified as parameters.
        /// Return 1 if success, 0 if fail or -1 if error.</summary>
        public int LoginByUid(string _Uid, string _Details = "")
        {
            return User.LoadByUid(_Uid, _Details);
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
