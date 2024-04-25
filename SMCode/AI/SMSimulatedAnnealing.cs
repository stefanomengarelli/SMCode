/*  ===========================================================================
 *  
 *  File:       SMSimulatedAnnealing.cs
 *  Version:    2.0.15
 *  Date:       April 2024
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2024 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *
 *  SMCode simulated annealing class.
 *
 *  ===========================================================================
 */

using Google.Protobuf.WellKnownTypes;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Threading;

namespace SMCode
{

    /* */

    /// <summary>SMCode simulated annealing class.</summary>
    public class SMSimulatedAnnealing
    {

        /* */

        #region Declarations

        /*  ===================================================================
         *  Declarations
         *  ===================================================================
         */

        /// <summary>SM session instance.</summary>
        private readonly SMApplication SM = null;

        /// <summary>Random number generator.</summary>
        private Random rnd = new Random(DateTime.Now.Day * 1000 + DateTime.Now.Month * 10 + DateTime.Now.Year + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second);

        #endregion

        /* */

        #region Delegates

        /*  ===================================================================
         *  Delegates
         *  ===================================================================
         */

        /// <summary>Occurs when solution cost has to be evaluated.</summary>
        public delegate void OnEvaluateSolution(object _Sender, object[] _Solution, out double _Cost);
        /// <summary>Occurs when solution cost has to be evaluated.</summary>
        public event OnEvaluateSolution EvaluateSolution = null;

        /// <summary>Occurs when new current solution has changed.</summary>
        public delegate void OnNewSolution(object _Sender, object[] _Solution);
        /// <summary>Occurs when new current solution has changed.</summary>
        public event OnNewSolution NewSolution = null;

        #endregion

        /* */

        #region Initialization

        /*  ===================================================================
         *  Initialization
         *  ===================================================================
         */

        /// <summary>Instance constructor.</summary>
        public SMSimulatedAnnealing(SMApplication _SMApplication = null)
        {
            if (_SMApplication == null) SM = SMApplication.CurrentOrNew();
            else SM = _SMApplication;
            Clear();
        }

        #endregion

        /* */

        #region Properties

        /*  ===================================================================
         *  Properties
         *  ===================================================================
         */

        /// <summary>Get or set cooling factor (default 0.999).</summary>
        public double Alpha { get; set; }

        /// <summary>Get or set best solution found.</summary>
        public object[] Best { get; set; }

        /// <summary>Get or set temperature to reach by cooling (default 0.001).</summary>
        public double Epsilon { get; set; }

        /// <summary>Get or set iterations counter.</summary>
        public List<object> Items { get; private set; } = new List<object>();

        /// <summary>Get or set iterations counter.</summary>
        public int Iteration { get; set; }

        /// <summary>Get or set last solution evaluated.</summary>
        public object[] Last { get; set; }

        /// <summary>Get or set solving flag.</summary>
        public bool Solving { get; set; }

        /// <summary>Get or set start temperature (default 400.0).</summary>
        public double Temperature { get; set; }

        #endregion

        /* */

        #region Methods

        /*  --------------------------------------------------------------------
         *  Methods
         *  --------------------------------------------------------------------
         */

        /// <summary>Initialize and reset properties variables.</summary>
        public void Clear()
        {
            Alpha = 0.999;
            Epsilon = 0.001;
            Iteration = -1;
            Solving = false;
            Temperature = 400.0;
        }

        /// <summary>Initialize and reset properties variables.</summary>
        public void Solve()
        {
            int i;
            double cost = 0.0, delta, probability, value = 0.0;

            // test if already solving
            if (!Solving && (Items.Count > 0))
            {
                // initialize solving
                Solving = true;

                // get first solution
                Best = new object[Items.Count];
                Last = new object[Items.Count];
                for (i = 0; i < Items.Count; i++)
                {
                    Best[i] = Items[i];
                    Last[i] = Items[i];
                }
                if (EvaluateSolution != null) EvaluateSolution(this, Last, out cost);

                // cooling loop
                while (Solving && (Temperature > Epsilon))
                {
                    Iteration++;

                    NextSolution();
                    if (EvaluateSolution != null) EvaluateSolution(this, Last, out value);

                    delta = value - cost;
                    if (delta < 0)
                    {
                        for (i = 0; i < Last.Length; i++) Best[i] = Last[i];
                        cost += delta;
                    }
                    else
                    {
                        probability = rnd.NextDouble();
                        if (probability < Math.Exp(-delta/Temperature))
                        {
                            for (i = 0; i < Last.Length; i++) Best[i] = Last[i];
                            cost += delta;
                        }
                    }

                    Temperature *= Alpha;

                }

                // end of solving
                Solving = false;
            }
        }

        /// <summary>Compute next solution.</summary>
        private void NextSolution()
        {
            int a = 0, b = 0;
            object swap;
            while (a == b)
            {
                a = rnd.Next(Last.Length);
                b = rnd.Next(Last.Length);
            }

        }

        #endregion

        /* */

    }

    /* */

}
