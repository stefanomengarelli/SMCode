/*  ===========================================================================
 *  
 *  File:       SMWorkFlowAction.cs
 *  Version:    2.0.70
 *  Date:       November 2024
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2024 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *
 *  SMCode workflow action class.
 *
 *  ===========================================================================
 */

namespace SMCodeSystem
{

	/* */

	/// <summary>SMCode workflow action class.</summary>
	public class SMWorkFlowAction
    {

        /* */

        #region Declarations

        /*  ===================================================================
         *  Declarations
         *  ===================================================================
         */

        /// <summary>SM session instance.</summary>
        public SMCode SM = null;

        #endregion

        /* */

        #region Properties

        /*  ===================================================================
         *  Properties
         *  ===================================================================
         */

        /// <summary>Get or set action index.</summary>
        public int Index { get; set; }

        /// <summary>Get or set action note.</summary>
        public string Note { get; set; }

        /// <summary>Get or set action state.</summary>
        public SMWorkFlowActionState State { get; set; } = SMWorkFlowActionState.None;

        /// <summary>Get or set object tag.</summary>
        public object Tag { get; set; }

        /// <summary>Get or set action description text.</summary>
        public string Text { get; set; }

        /// <summary>Get or set action workflow.</summary>
        public SMWorkFlow WorkFlow { get; set; }

        #endregion

        /* */

        #region Initialization

        /*  ===================================================================
         *  Initialization
         *  ===================================================================
         */

        /// <summary>Class constructor.</summary>
        public SMWorkFlowAction(SMCode _SM = null)
        {
            SM = SMCode.CurrentOrNew(_SM);
            Initialize();
        }

        /// <summary>Class constructor.</summary>
        public SMWorkFlowAction(SMWorkFlow _WorkFlow = null)
        {
            SM = _WorkFlow.SM;
            Initialize();
            WorkFlow = _WorkFlow;
            Index = WorkFlow.Actions.Count;
            WorkFlow.Actions.Add(this);
        }

        /// <summary>Class constructor.</summary>
        public SMWorkFlowAction(SMWorkFlowAction _OtherInstance, SMCode _SM = null)
        {
            if (_SM == null) _SM = _OtherInstance.SM;
            SM = SMCode.CurrentOrNew(_SM);
            Initialize();
            Assign(_OtherInstance);
        }

        /// <summary>Initialize instance.</summary>
        public void Initialize()
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
        public void Assign(SMWorkFlowAction _Action)
        {
            SM = WorkFlow.SM;
            Index = _Action.Index;
            Note = _Action.Note;
            State = _Action.State;
            Tag = _Action.Tag;
            Text = _Action.Text;
            WorkFlow = _Action.WorkFlow;
        }

        /// <summary>Clear item.</summary>
        public void Clear()
        {
            Index = -1;
            Note = "";
            State = SMWorkFlowActionState.None;
            Tag = null;
            Text = "";
            WorkFlow = null;
        }

        #endregion

        /* */

    }

    /* */

}
