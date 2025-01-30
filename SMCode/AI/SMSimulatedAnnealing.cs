/*  ===========================================================================
 *  
 *  File:       SMSimulatedAnnealing.cs
 *  Version:    2.0.200
 *  Date:       January 2025
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2025 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *
 *  SMCode simulated annealing class.
 *
 *  ===========================================================================
 */

using System;
using System.Collections.Generic;

namespace SMCodeSystem
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

        /// <summary>Random number generator.</summary>
        private Random rnd = new Random(DateTime.Now.Day * 1000 + DateTime.Now.Month * 10 + DateTime.Now.Year + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second);

        /// <summary>Last solution cost.</summary>
        private double cost = 0.0d;

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

        /// <summary>Occurs when new current solution has changed.</summary>
        public delegate void OnProgress(object _Sender);
        /// <summary>Occurs when new current solution has changed.</summary>
        public event OnProgress Progress = null;

        #endregion

        /* */

        #region Initialization

        /*  ===================================================================
         *  Initialization
         *  ===================================================================
         */

        /// <summary>Instance constructor.</summary>
        public SMSimulatedAnnealing()
        {
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

        /// <summary>Get or set cost of solution found.</summary>
        public double Cost 
        { 
            get { return cost; }
            set { cost = value; }
        }

        /// <summary>Get or set temperature to reach by cooling (default 0.001).</summary>
        public double Epsilon { get; set; }

        /// <summary>Get or set iterations counter.</summary>
        public List<object> Items { get; private set; } = new List<object>();

        /// <summary>Get or set iterations counter.</summary>
        public int Iteration { get; set; }

        /// <summary>Get or set last solution evaluated.</summary>
        public object[] Last { get; set; }

        /// <summary>Get or set rate of progress event every number of iterations (default 400).</summary>
        public int ProgressRate { get; set; }

        /// <summary>Get or set solving flag.</summary>
        public bool Solving { get; set; }

        /// <summary>Get or set start temperature (default 400.0).</summary>
        public double Temperature { get; set; }

        #endregion

        /* */

        #region Methods

        /*  ===================================================================
         *  Methods
         *  ===================================================================
         */

        /// <summary>Initialize and reset properties variables.</summary>
        public void Clear()
        {
            Alpha = 0.999;
            cost = 0.0;
            Epsilon = 0.001;
            Iteration = -1;
            ProgressRate = 400;
            Solving = false;
            Temperature = 400.0;
        }

        /// <summary>Assign last solution as best.</summary>
        private void BestSolution(double _Delta)
        {
            int i;
            for (i = 0; i < Last.Length; i++) Best[i] = Last[i];
            cost += _Delta;
            if (NewSolution != null) NewSolution(this, Best);
        }

        /// <summary>Compute next solution.</summary>
        private void NextSolution()
        {
            int i = 10, a = 0, b = 0;
            object swap;
            while ((i > 0) && (a == b))
            {
                a = rnd.Next(Best.Length);
                b = rnd.Next(Best.Length);
                i--;
            }
            for (i = 0; i < Best.Length; i++) Last[i] = Best[i];
            swap = Last[a];
            Last[a] = Last[b];
            Last[b] = swap;
        }

        /// <summary>Initialize and reset properties variables.</summary>
        public void Solve()
        {
            int i;
            double delta, probability, value = 0.0;

            // test if already solving
            if (!Solving && (Items.Count > 0))
            {
                // initialize solving
                Solving = true;
                cost = 0.0;

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
                    if (delta < 0) BestSolution(delta);
                    else
                    {
                        probability = rnd.NextDouble();
                        if (probability < Math.Exp(-delta/Temperature))
                        {
                            BestSolution(delta);
                        }
                    }

                    Temperature *= Alpha;

                    if ((Progress != null) && (Iteration % ProgressRate == 0)) Progress(this);

                }

                // end of solving
                Solving = false;
            }
        }

        #endregion

        /* */

    }

    /* */

}
