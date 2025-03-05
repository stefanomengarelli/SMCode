/*  ===========================================================================
 *  
 *  File:       SMMiniMax.cs
 *  Version:    2.0.218
 *  Date:       March 2025
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2025 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *
 *  Minimax algorithm class.
 *
 *  ===========================================================================
 */

using System;
using SMCodeSystem;

namespace SMFrontSystem
{

    /* */

    /// <summary>Minimax algorithm class.</summary>
    public class SMMiniMax
    {

        /* */

        #region Declarations

        /*  ===================================================================
         *  Declarations
         *  ===================================================================
         */

        /// <summary>SM session instance.</summary>
        private readonly SMCode SM = null;

        /// <summary>Oterations counter.</summary>
        private int iterations = 0;

        /// <summary>Result node.</summary>
        private SMNode result = null;

        #endregion

        /* */

        #region Delegates

        /*  ===================================================================
         *  Delegates
         *  ===================================================================
         */

        /// <summary>Occurs when algorithm neet to calculate heuristic value (integer) of node.</summary>
        public delegate int SMOnHeuristicNodeValue(SMNode _Node);

        /// <summary>Occurs when algorithm neet to calculate heuristic value (integer) of node.</summary>
        public event SMOnHeuristicNodeValue OnHeuristicNodeValue;

        #endregion

        /* */

        #region Properties

        /*  ===================================================================
         *  Properties
         *  ===================================================================
         */

        /// <summary>Get or set iterations counter.</summary>
        public int Iterations { get { return iterations; } set { iterations = value; } }

        /// <summary>Get or set iterations max value.</summary>
        public int MaxIterations { get; set; } = int.MaxValue;

        /// <summary>Get or set result node.</summary>
        public SMNode Result { get { return result; } set { result = value; } }

        #endregion

        /* */

        #region Initialization

        /*  ===================================================================
         *  Initialization
         *  ===================================================================
         */

        /// <summary>Class constructor.</summary>
        public SMMiniMax(SMCode _SM = null)
        {
            SM = SMCode.CurrentOrNew(_SM);
        }

        #endregion

        /* */

        #region Methods

        /*  ===================================================================
         *  Methods
         *  ===================================================================
         */

        /// <summary>Solve minimax tree starting by node.</summary>
        public int Minimax(SMNode _Node, int _Depth, bool _Maximizing, SMOnHeuristicNodeValue _OnMinimaxNode = null)
        {
            iterations = 0;
            result = null;
            if (_OnMinimaxNode != null) OnHeuristicNodeValue = _OnMinimaxNode;
            return MinimaxSolve(_Node, _Depth, _Maximizing);
        }

        /// <summary>Solve minimax tree starting by node.</summary>
        private int MinimaxSolve(SMNode _Node, int _Depth, bool _Maximizing)
        {
            int i, rslt;
            if ((_Depth < 1) || (_Node.Childs.Count < 1))
            {
                if (OnHeuristicNodeValue == null) rslt = _Node.Heuristic;
                else rslt = OnHeuristicNodeValue(_Node);
            }
            else if (_Maximizing)
            {
                rslt = int.MaxValue;
                for (i = 0; i < _Node.Childs.Count; i++) rslt = SM.Max(rslt, MinimaxSolve(_Node.Childs[i], _Depth - 1, false));
            }
            else
            {
                rslt = int.MinValue;
                for (i = 0; i < _Node.Childs.Count; i++) rslt = SM.Min(rslt, MinimaxSolve(_Node.Childs[i], _Depth - 1, true));
            }
            iterations++;
            if (iterations >= MaxIterations) throw new Exception("SMMinimax: Minimax maximum iterations reached.");
            return rslt;
        }

        /// <summary>Solve negamax tree starting by node.</summary>
        public int Negamax(SMNode _Node, int _Depth, int _Alpha, int _Beta, SMOnHeuristicNodeValue _OnMinimaxNode = null)
        {
            iterations = 0;
            result = null;
            if (_OnMinimaxNode != null) OnHeuristicNodeValue = _OnMinimaxNode;
            return NegamaxSolve(_Node, _Depth, _Alpha, _Beta);
        }

        /// <summary>Solve negamax tree starting by node.</summary>
        private int NegamaxSolve(SMNode _Node, int _Depth, int _Alpha, int _Beta)
        {
            int i, rslt;
            if ((_Depth < 1) || (_Node.Childs.Count < 1))
            {
                if (OnHeuristicNodeValue == null) rslt = _Node.Heuristic;
                else rslt = OnHeuristicNodeValue(_Node);
            }
            else
            {
                rslt = _Alpha;
                for (i = 0; i < _Node.Childs.Count; i++)
                {
                    _Alpha = SM.Max(_Alpha, -NegamaxSolve(_Node.Childs[i], _Depth - 1, -_Beta, -_Alpha));
                    if (_Alpha >= _Beta)
                    {
                        rslt = _Beta;
                        break;
                    }
                    else rslt = _Alpha;
                }
            }
            iterations++;
            if (iterations >= MaxIterations) throw new Exception("SMMinimax: Negamax maximum iterations reached.");
            return rslt;
        }

        #endregion

        /* */

    }

    /* */

}
