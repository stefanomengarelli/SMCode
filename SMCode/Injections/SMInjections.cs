/*  ===========================================================================
 *  
 *  File:       SMInjections.cs
 *  Version:    2.0.282
 *  Date:       July 2025
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2025 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *
 *  Injection methods collection management class.
 *
 *  ===========================================================================
 */

using System;
using System.Collections.Generic;
using System.Reflection;

namespace SMCodeSystem
{

    /* */

    /// <summary>Injection methods collection management class.</summary>
    public partial class SMInjections
    {

        /* */

        #region Declarations

        /*  ====================================================================
         *  Declarations
         *  ====================================================================
         */

        /// <summary>SM session instance.</summary>
        private readonly SMCode SM = null;

        #endregion

        /* */

        #region Properties

        /*  ====================================================================
         *  Properties
         *  ====================================================================
         */

        /// <summary>Quick access declaration.</summary>
        public SMDictionaryItem this[int _Index]
        {
            get { return Items[_Index]; }
            set { Items[_Index] = value; }
        }

        /// <summary>Quick access declaration.</summary>
        public SMDictionaryItem this[string _MethodId]
        {
            get { return Items.GetItem(_MethodId); }
            set { Items.Set(_MethodId, "", value); }
        }

        /// <summary>Get items count.</summary>
        public int Count { get { return Items.Count; } }

        /// <summary>Methods collection.</summary>
        public SMDictionary Items { get; private set; } = null;

        #endregion

        /* */

        #region Initialization

        /*  ====================================================================
         *  Initialization
         *  ====================================================================
         */

        /// <summary>Instance constructor.</summary>
        public SMInjections(SMCode _SM)
        {
            SM = SMCode.CurrentOrNew(_SM);
            Initialize();
        }

        /// <summary>Initialize instance.</summary>
        private void Initialize()
        {
            Items = new SMDictionary(SM);
        }

        #endregion

        /* */

        #region Methods

        /*  ====================================================================
         *  Methods
         *  ====================================================================
         */

        /// <summary>Add method with id.</summary>
        public int Add(string _Id, SMInjectionMethod _Method)
        {
            return Items.Set(_Id.Trim(), "invoke", _Method);
        }

        /// <summary>Add injection with id.</summary>
        public int Add(string _Id, SMInjection _Injection)
        {
            return Items.Set(_Id.Trim(), "injection", _Injection);
        }

        /// <summary>Add all methods in type class starting by prefix (default: FN_)
        /// Prefix will be excluded by function registered name.</summary>
        public int AddMethods(object _ClassInstance, string _Prefix = "FN_", bool _RemovePrefixFromIdentifier = false)
        {
            int i, rslt;
            Type type;
            MethodInfo[] methods;
            MethodInfo method;
            try
            {
                rslt = 0;
                if (!SM.Empty(_Prefix))
                {
                    _Prefix = _Prefix.Trim();
                    type = _ClassInstance.GetType();
                    methods = type.GetMethods();
                    if (methods != null)
                    {
                        for (i = 0; i < methods.Length; i++)
                        {
                            method = methods[i];
                            if (!method.IsVirtual && method.Name.StartsWith(_Prefix))
                            {
                                if (_RemovePrefixFromIdentifier) Items.Set(SM.Mid(method.Name, _Prefix.Length), "invoke", new SMInjectionMethod(_ClassInstance, method));
                                else Items.Set(method.Name, "invoke", new SMInjectionMethod(_ClassInstance, method));
                                rslt++;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SM.Error(ex);
                rslt = -1;
            }
            return rslt;
        }

        /// <summary>Perform method with statement: MethodId("arg0","arg1",..."argN").</summary>
        public object Exec(string _MethodStatement)
        {
            string id, v;
            object rslt = null;
            List<object> parms = new List<object>();
            if (_MethodStatement != null)
            {
                if ((_MethodStatement.IndexOf("(") < 0) && (_MethodStatement.IndexOf(")") < 0))
                {
                    _MethodStatement = SM.Base64Decode(_MethodStatement);
                }
                id = SM.Before(_MethodStatement, "(").Trim();
                if (SM.Empty(id))
                {
                    SM.Raise("Missing function id.", false);
                }
                else
                {
                    _MethodStatement = SM.After(_MethodStatement, "(").Trim();
                    if (_MethodStatement.EndsWith(")")) _MethodStatement = SM.Mid(_MethodStatement, 0, _MethodStatement.Length - 1);
                    while (!SM.Empty(_MethodStatement))
                    {
                        v = SM.ExtractArgument(ref _MethodStatement, ",;", false);
                        parms.Add(v);
                    }
                    rslt = Exec(id, parms.ToArray());
                }
            }
            return rslt;
        }

        /// <summary>Perform method by id with parameters.</summary>
        public object Exec(string _MethodId, object[] _Parameters)
        {
            object rslt = null;
            SMInjectionMethod method;
            SMDictionaryItem item = Items.GetItem(_MethodId);
            SMInjection fn;
            object[] parameters = new object[1]; 
            try
            {
                if (item != null)
                {
                    if (item.Value == "invoke")
                    {
                        method = (SMInjectionMethod)item.Tag;
                        parameters[0] = _Parameters;
                        if (method != null) rslt = SM.ToStr(method.MethodInfo.Invoke(method.Instance, parameters));
                    }
                    else if (item.Value == "injection")
                    {
                        fn = (SMInjection)item.Tag;
                        if (fn != null) rslt = fn(_Parameters);
                    }
                }
            }
            catch (Exception ex)
            {
                SM.Error(ex);
                rslt = null;
            }
            return rslt;
        }

        #endregion

        /* */

    }

    /* */

}
