/*  ===========================================================================
 *  
 *  File:       SMWorkFlow.cs
 *  Version:    2.0.70
 *  Date:       November 2024
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2024 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *
 *  SMCode workflow class.
 *
 *  ===========================================================================
 */

using System.Collections.Generic;

namespace SMCodeSystem
{

	/* */

	/// <summary>SMCode workflow class.</summary>
	public class SMWorkFlow
    {

        /* */

        #region Declarations

        /*  ===================================================================
         *  Declarations
         *  ===================================================================
         */

        /// <summary>SM session instance.</summary>
        public SMCode SM = null;

        /// <summary>Actions collection.</summary>
        private List<SMWorkFlowAction> actions = new List<SMWorkFlowAction>();

        #endregion

        /* */

        #region Properties

        /*  ===================================================================
         *  Properties
         *  ===================================================================
         */

        /// <summary>Get actions collection.</summary>
        public List<SMWorkFlowAction> Actions { get { return actions; } }

		/// <summary>Get or set workflow Note.</summary>
		public string Note { get; set; }

		/// <summary>Get or set workflow description text.</summary>
		public string Text { get; set; }

		#endregion

		/* */

		#region Initialization

		/*  ===================================================================
         *  Initialization
         *  ===================================================================
         */

		/// <summary>Class constructor.</summary>
		public SMWorkFlow(SMCode _SM = null)
        {
            SM = SMCode.CurrentOrNew(_SM);
            InitializeInstance();
        }

        /// <summary>Class constructor.</summary>
        public SMWorkFlow(SMWorkFlow _OtherInstance, SMCode _SM = null)
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
        public void Assign(SMWorkFlow _OtherInstance)
        {
            int i;
            SMWorkFlowAction action;
            actions.Clear();
            for (i = 0; i < _OtherInstance.Actions.Count; i++)
            {
                action = new SMWorkFlowAction(_OtherInstance.Actions[i]);
                action.SM = SM;
                action.Index = i;
                actions.Add(action);
            }
        }

        /// <summary>Clear item.</summary>
        public void Clear()
        {
            actions.Clear();
        }

        /// <summary>Return index of first action in execute state.</summary>
        public int CurrentAction()
        {
            int i = 0, r = -1;
            while ((r < 0) && (i < actions.Count))
            {
                if (actions[i].State == SMWorkFlowActionState.Perform) r = i;
                i++;
            }
            return r;
        }

        #endregion

        /* */

    }

    /* */

}
