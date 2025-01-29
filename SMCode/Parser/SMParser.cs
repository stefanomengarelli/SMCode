/*  ===========================================================================
 *  
 *  File:       SMParser.cs
 *  Version:    2.0.0
 *  Date:       February 2024
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2024 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *
 *  SMCode formula parser class. Provide a class for expression formula evaluation.
 *  Variables field can store user defined variables (SMParserAtoms).
 *
 *  ===========================================================================
 */

using System;
using System.Collections;

namespace SMCodeSystem
{

    /* */

    /// <summary>SMCode formula parser class. Provide a class for expression formula evaluation.
    /// Variables field can store user defined variables (SMParserAtoms).</summary>
    public class SMParser
    {

        /* */

        #region Declarations

        /*  ===================================================================
         *  Declarations
         *  ===================================================================
         */

        /// <summary>SM session instance.</summary>
        private readonly SMCode SM = null;

        /// <summary>Infix expression.</summary>
        private readonly SMParserAtoms exprInfix = new SMParserAtoms();

        /// <summary>Postfix expression.</summary>
        private readonly SMParserAtoms exprPostfix = new SMParserAtoms();

        #endregion

        /* */

        #region Properties

        /*  ===================================================================
         *  Properties
         *  ===================================================================
         */

        /// <summary>Indicates an error parsing formula.</summary>
        public bool Error { get; set; } = false;

        /// <summary>Indicates error message parsing formula.</summary>
        public string ErrorMessage { get; set; } = "";

        /// <summary>Collection of preassigned variables.</summary>
        public SMParserAtoms Variables { get; } = new SMParserAtoms();

        #endregion

        /* */

        #region Initialization

        /*  ===================================================================
         *  Initialization
         *  ===================================================================
         */

        /// <summary>Instance builder.</summary>
        public SMParser(SMCode _SM)
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

        /// <summary>Returns result for expression in formula string.</summary>
        public double Result(string _Formula)
        {
            int i, h;
            char c;
            Error = false;
            ErrorMessage = "";
            _Formula = SM.Remove(_Formula, ' ').ToUpper();
            _Formula = _Formula.Replace('.', SM.DecimalSeparator).Replace(',', SM.DecimalSeparator);
            i = 0;
            h = _Formula.Length;
            while (!Error && i < h)
            {
                c = _Formula[i];
                if (c >= '0' && c <= '9' || c >= 'A' && c <= 'Z' || c == '_'
                    || c == '+' || c == '-' || c == '*' || c == '/'
                    || c == SM.DecimalSeparator || c == ';' || c == '^' || c == '(' || c == ')') i++;
                else
                {
                    Error = true;
                    ErrorMessage = "Invalid chars on expression";
                }
            }
            if (!Error)
            {
                Parse(_Formula);
                Infix2Postfix();
                return EvaluatePostfix();
            }
            else return 0;
        }

        /// <summary>Parse formula contained in string.</summary>
        private void Parse(string _Formula)
        {
            int q = 1, i, h;
            char c, cp;
            string t = "";
            exprInfix.Count = 0;
            Error = false;
            ErrorMessage = "";
            i = 0;
            h = _Formula.Length;
            try
            {
                while (!Error && i < h)
                {
                    c = _Formula[i];
                    if (i > 0) cp = _Formula[i - 1];
                    else cp = '(';
                    if (q == 1)
                    {
                        if (c >= '0' && c <= '9' || c == '-' &&
                            (cp == '(' || cp == '+' || cp == '-' || cp == '*' || cp == '/' || cp == '^' || cp == ';'))
                        {
                            q = 2;
                            t += c;
                        }
                        else if (c >= 'A' && c <= 'Z' || c == '_')
                        {
                            q = 3;
                            t += c;
                        }
                        else if (c == '(' || c == ')') exprInfix.Add(c.ToString(), 0, SMParserAtomType.Bracket);
                        else if (c == ';') exprInfix.Add(c.ToString(), 0, SMParserAtomType.Comma);
                        else exprInfix.Add(c.ToString(), 0, SMParserAtomType.Operator);
                    }
                    else if (q == 2)
                    {
                        if (c >= '0' && c <= '9' || c == SM.DecimalSeparator) t += c;
                        else if (c >= 'A' && c <= 'Z' || c == '_')
                        {
                            Error = true;
                            ErrorMessage = "Syntax error";
                        }
                        else
                        {
                            q = 1;
                            if (t[t.Length - 1] == SM.DecimalSeparator) t += '0';
                            exprInfix.Add(t, double.Parse(t), SMParserAtomType.Value);
                            if (c == '(' || c == ')') exprInfix.Add(c.ToString(), 0, SMParserAtomType.Bracket);
                            else if (c == ';') exprInfix.Add(c.ToString(), 0, SMParserAtomType.Comma);
                            else exprInfix.Add(c.ToString(), 0, SMParserAtomType.Operator);
                            t = "";
                        }
                    }
                    else if (q == 3)
                    {
                        if (c >= 'A' && c <= 'Z' || c == '_' || c >= '0' && c <= '9') t += c;
                        else
                        {
                            q = 1;
                            if (c == '(') exprInfix.Add(t, 0, SMParserAtomType.Function);
                            else if (t == "PI") exprInfix.Add(t, Math.PI, SMParserAtomType.Variable);
                            else if (t == "E") exprInfix.Add(t, Math.E, SMParserAtomType.Variable);
                            else exprInfix.Add(t, Variables.Get(t), SMParserAtomType.Variable);
                            if (c == '(' || c == ')') exprInfix.Add(c.ToString(), 0, SMParserAtomType.Bracket);
                            else if (c == ';') exprInfix.Add(c.ToString(), 0, SMParserAtomType.Comma);
                            else exprInfix.Add(c.ToString(), 0, SMParserAtomType.Operator);
                            t = "";
                        }
                    }
                    i++;
                }
                if (t != "")
                {
                    if (q == 2) exprInfix.Add(t, double.Parse(t), SMParserAtomType.Value);
                    else if (t == "PI") exprInfix.Add(t, Math.PI, SMParserAtomType.Variable);
                    else if (t == "E") exprInfix.Add(t, Math.E, SMParserAtomType.Variable);
                    else exprInfix.Add(t, Variables.Get(t), SMParserAtomType.Variable);
                }
            }
            catch
            {
                Error = true;
                ErrorMessage = "Syntax error";
            }
        }

        /// <summary>Return precedence index of atom.</summary>
        private int Precedence(SMParserAtom _Atom)
        {
            if (_Atom.Type == SMParserAtomType.Bracket) return 5;
            else if (_Atom.Type == SMParserAtomType.Function) return 4;
            else if (_Atom.Type == SMParserAtomType.Comma) return 0;
            else if (_Atom.Name == "^") return 3;
            else if (_Atom.Name == "/" || _Atom.Name == "*" || _Atom.Name == "%") return 2;
            else if (_Atom.Name == "+" || _Atom.Name == "-") return 1;
            else return -1;
        }

        /// <summary>Convert infix notation to postfix.</summary>
        private void Infix2Postfix()
        {
            SMParserAtom a, b;
            Stack stk = new Stack();
            int i = 0;
            exprPostfix.Count = 0;
            while (i < exprInfix.Count)
            {
                a = exprInfix.Items[i];
                if (a.Type == SMParserAtomType.Value || a.Type == SMParserAtomType.Variable) exprPostfix.Add(a.Name, a.Value, a.Type);
                else if (a.Name == "(") stk.Push(a);
                else if (a.Name == ")")
                {
                    if (stk.Count > 0)
                    {
                        b = (SMParserAtom)stk.Pop();
                        while (b.Name != "(") { exprPostfix.Add(b.Name, b.Value, b.Type); b = (SMParserAtom)stk.Pop(); }
                    }
                }
                else
                {
                    if (stk.Count > 0)
                    {
                        b = (SMParserAtom)stk.Pop();
                        while (stk.Count > 0 && (b.Type == SMParserAtomType.Operator || b.Type == SMParserAtomType.Function || b.Type == SMParserAtomType.Comma) && Precedence(b) >= Precedence(a))
                        {
                            exprPostfix.Add(b.Name, b.Value, b.Type);
                            b = (SMParserAtom)stk.Pop();
                        }
                        if ((b.Type == SMParserAtomType.Operator || b.Type == SMParserAtomType.Function || b.Type == SMParserAtomType.Comma) && Precedence(b) >= Precedence(a))
                        {
                            exprPostfix.Add(b.Name, b.Value, b.Type);
                        }
                        else stk.Push(b);
                    }
                    stk.Push(a);
                }
                i++;
            }
            while (stk.Count > 0)
            {
                b = (SMParserAtom)stk.Pop();
                exprPostfix.Add(b.Name, b.Value, b.Type);
            }
        }

        /// <summary>Returns result atom evaluating atoms a (value), o (operator), b (value).</summary>
        private SMParserAtom Evaluate(SMParserAtom _AtomValueA, SMParserAtom _AtomOperator, SMParserAtom _AtomValueB)
        {
            SMParserAtom r = new SMParserAtom(_AtomValueA.Name + _AtomOperator.Name + _AtomValueB.Name, 0, SMParserAtomType.Result);
            if (_AtomOperator.Name == "^") r.Value = Math.Pow(_AtomValueA.Value, _AtomValueB.Value);
            else if (_AtomOperator.Name == "/")
            {
                if (_AtomValueB.Value == 0) { r.Name = "Divide by zero"; r.Type = SMParserAtomType.Error; }
                else r.Value = _AtomValueA.Value / _AtomValueB.Value;
            }
            else if (_AtomOperator.Name == "*") r.Value = _AtomValueA.Value * _AtomValueB.Value;
            else if (_AtomOperator.Name == "%") r.Value = _AtomValueA.Value % _AtomValueB.Value;
            else if (_AtomOperator.Name == "+") r.Value = _AtomValueA.Value + _AtomValueB.Value;
            else if (_AtomOperator.Name == "-") r.Value = _AtomValueA.Value - _AtomValueB.Value;
            else r.Error("Unknown operator: " + _AtomOperator.Name);
            return r;
        }

        /// <summary>Returns result atom evaluating function by name and arguments.</summary>
        private SMParserAtom EvaluateFunction(string _FunctionName, params object[] _Arguments)
        {
            SMParserAtom r = new SMParserAtom("", 0, SMParserAtomType.Result), a;
            try
            {
                if (_FunctionName == "ABS")
                {
                    if (_Arguments.Length == 1)
                    {
                        a = (SMParserAtom)_Arguments[0];
                        r.Name = _FunctionName + "(" + a.Value.ToString() + ")";
                        r.Value = Math.Abs(a.Value);
                    }
                    else r.Error("Wrong parameters number in: " + _FunctionName);
                }
                else if (_FunctionName == "SGN" || _FunctionName == "SIGN")
                {
                    if (_Arguments.Length == 1)
                    {
                        a = (SMParserAtom)_Arguments[0];
                        r.Name = _FunctionName + "(" + a.Value.ToString() + ")";
                        if (a.Value < 0) r.Value = -1.0d;
                        else if (a.Value > 0) r.Value = 1.0d;
                        else r.Value = 0.0d;
                    }
                    else r.Error("Wrong parameters number in: " + _FunctionName);
                }
                else if (_FunctionName == "INT" || _FunctionName == "TRUNC" || _FunctionName == "FLOOR")
                {
                    if (_Arguments.Length == 1)
                    {
                        a = (SMParserAtom)_Arguments[0];
                        r.Name = _FunctionName + "(" + a.Value.ToString() + ")";
                        r.Value = Math.Floor(a.Value);
                    }
                    else r.Error("Wrong parameters number in: " + _FunctionName);
                }
                else if (_FunctionName == "CEIL")
                {
                    if (_Arguments.Length == 1)
                    {
                        a = (SMParserAtom)_Arguments[0];
                        r.Name = _FunctionName + "(" + a.Value.ToString() + ")";
                        r.Value = Math.Ceiling(a.Value);
                    }
                    else r.Error("Wrong parameters number in: " + _FunctionName);
                }
                else if (_FunctionName == "ROUND")
                {
                    if (_Arguments.Length == 1)
                    {
                        a = (SMParserAtom)_Arguments[0];
                        r.Name = _FunctionName + "(" + a.Value.ToString() + ")";
                        if (a.Value - Math.Floor(a.Value) < 0.5) r.Value = Math.Floor(a.Value);
                        else r.Value = Math.Ceiling(a.Value);
                    }
                    else r.Error("Wrong parameters number in: " + _FunctionName);
                }
                else if (_FunctionName == "FRAC")
                {
                    if (_Arguments.Length == 1)
                    {
                        a = (SMParserAtom)_Arguments[0];
                        r.Name = _FunctionName + "(" + a.Value.ToString() + ")";
                        r.Value = a.Value - Math.Floor(a.Value);
                    }
                    else r.Error("Wrong parameters number in: " + _FunctionName);
                }
                else if (_FunctionName == "DAYS2MINS")
                {
                    if (_Arguments.Length == 1)
                    {
                        a = (SMParserAtom)_Arguments[0];
                        r.Name = _FunctionName + "(" + a.Value.ToString() + ")";
                        r.Value = Math.Floor(a.Value * 1440.0d);
                    }
                    else r.Error("Wrong parameters number in: " + _FunctionName);
                }
                else if (_FunctionName == "DAYS2HOURS")
                {
                    if (_Arguments.Length == 1)
                    {
                        a = (SMParserAtom)_Arguments[0];
                        r.Name = _FunctionName + "(" + a.Value.ToString() + ")";
                        r.Value = Math.Floor(a.Value * 24.0d);
                    }
                    else r.Error("Wrong parameters number in: " + _FunctionName);
                }
                else if (_FunctionName == "HOURS2MINS")
                {
                    if (_Arguments.Length == 1)
                    {
                        a = (SMParserAtom)_Arguments[0];
                        r.Name = _FunctionName + "(" + a.Value.ToString() + ")";
                        r.Value = Math.Floor(a.Value * 60.0d);
                    }
                    else r.Error("Wrong parameters number in: " + _FunctionName);
                }
                else if (_FunctionName == "HOURS2DAYS")
                {
                    if (_Arguments.Length == 1)
                    {
                        a = (SMParserAtom)_Arguments[0];
                        r.Name = _FunctionName + "(" + a.Value.ToString() + ")";
                        r.Value = SM.RoundDouble(a.Value / 24.0d, 2);
                    }
                    else r.Error("Wrong parameters number in: " + _FunctionName);
                }
                else if (_FunctionName == "MINS2HOURS")
                {
                    if (_Arguments.Length == 1)
                    {
                        a = (SMParserAtom)_Arguments[0];
                        r.Name = _FunctionName + "(" + a.Value.ToString() + ")";
                        r.Value = SM.RoundDouble(a.Value / 60.0d, 2);
                    }
                    else r.Error("Wrong parameters number in: " + _FunctionName);
                }
                else if (_FunctionName == "MINS2DAYS")
                {
                    if (_Arguments.Length == 1)
                    {
                        a = (SMParserAtom)_Arguments[0];
                        r.Name = _FunctionName + "(" + a.Value.ToString() + ")";
                        r.Value = SM.RoundDouble(a.Value / 1440.0d, 2);
                    }
                    else r.Error("Wrong parameters number in: " + _FunctionName);
                }
                else if (_FunctionName == "VALPO")
                {
                    if (_Arguments.Length == 1)
                    {
                        a = (SMParserAtom)_Arguments[0];
                        r.Name = _FunctionName + "(" + a.Value.ToString() + ")";
                        if (a.Value > 0.0d) r.Value = a.Value; else r.Value = 0.0d;
                    }
                    else r.Error("Wrong parameters number in: " + _FunctionName);
                }
                else if (_FunctionName == "VALNE")
                {
                    if (_Arguments.Length == 1)
                    {
                        a = (SMParserAtom)_Arguments[0];
                        r.Name = _FunctionName + "(" + a.Value.ToString() + ")";
                        if (a.Value < 0.0d) r.Value = a.Value; else r.Value = 0.0d;
                    }
                    else r.Error("Wrong parameters number in: " + _FunctionName);
                }
                else if (_FunctionName == "IFPO")
                {
                    if (_Arguments.Length == 1)
                    {
                        a = (SMParserAtom)_Arguments[0];
                        r.Name = _FunctionName + "(" + a.Value.ToString() + ")";
                        if (a.Value > 0.0d) r.Value = 1.0d; else r.Value = 0.0d;
                    }
                    else r.Error("Wrong parameters number in: " + _FunctionName);
                }
                else if (_FunctionName == "IFNE")
                {
                    if (_Arguments.Length == 1)
                    {
                        a = (SMParserAtom)_Arguments[0];
                        r.Name = _FunctionName + "(" + a.Value.ToString() + ")";
                        if (a.Value < 0.0d) r.Value = 1.0d; else r.Value = 0.0d;
                    }
                    else r.Error("Wrong parameters number in: " + _FunctionName);
                }
                else if (_FunctionName == "IFZE")
                {
                    if (_Arguments.Length == 1)
                    {
                        a = (SMParserAtom)_Arguments[0];
                        r.Name = _FunctionName + "(" + a.Value.ToString() + ")";
                        if (a.Value == 0.0d) r.Value = 1.0d; else r.Value = 0.0d;
                    }
                    else r.Error("Wrong parameters number in: " + _FunctionName);
                }
                else if (_FunctionName == "IFNZ")
                {
                    if (_Arguments.Length == 1)
                    {
                        a = (SMParserAtom)_Arguments[0];
                        r.Name = _FunctionName + "(" + a.Value.ToString() + ")";
                        if (a.Value == 0.0d) r.Value = 0.0d; else r.Value = 1.0d;
                    }
                    else r.Error("Wrong parameters number in: " + _FunctionName);
                }
                else if (_FunctionName == "MAX")
                {
                    if (_Arguments.Length == 2)
                    {
                        a = (SMParserAtom)_Arguments[0];
                        r.Name = _FunctionName + "(" + a.Value.ToString() + ";" + ((SMParserAtom)_Arguments[1]).Value.ToString() + ")";
                        if (a.Value < ((SMParserAtom)_Arguments[1]).Value) r.Value = ((SMParserAtom)_Arguments[1]).Value;
                        else r.Value = a.Value;
                    }
                    else r.Error("Wrong parameters number in: " + _FunctionName);
                }
                else if (_FunctionName == "MIN")
                {
                    if (_Arguments.Length == 2)
                    {
                        a = (SMParserAtom)_Arguments[0];
                        r.Name = _FunctionName + "(" + a.Value.ToString() + ";" + ((SMParserAtom)_Arguments[1]).Value.ToString() + ")";
                        if (a.Value > ((SMParserAtom)_Arguments[1]).Value) r.Value = ((SMParserAtom)_Arguments[1]).Value;
                        else r.Value = a.Value;
                    }
                    else r.Error("Wrong parameters number in: " + _FunctionName);
                }
                else if (_FunctionName == "LIMINF")
                {
                    if (_Arguments.Length == 2)
                    {
                        a = (SMParserAtom)_Arguments[0];
                        r.Name = _FunctionName + "(" + a.Value.ToString() + ";" + ((SMParserAtom)_Arguments[1]).Value.ToString() + ")";
                        if (a.Value > ((SMParserAtom)_Arguments[1]).Value) r.Value = a.Value;
                        else r.Value = ((SMParserAtom)_Arguments[1]).Value;
                    }
                    else r.Error("Wrong parameters number in: " + _FunctionName);
                }
                else if (_FunctionName == "LIMSUP")
                {
                    if (_Arguments.Length == 2)
                    {
                        a = (SMParserAtom)_Arguments[0];
                        r.Name = _FunctionName + "(" + a.Value.ToString() + ";" + ((SMParserAtom)_Arguments[1]).Value.ToString() + ")";
                        if (a.Value < ((SMParserAtom)_Arguments[1]).Value) r.Value = a.Value;
                        else r.Value = ((SMParserAtom)_Arguments[1]).Value;
                    }
                    else r.Error("Wrong parameters number in: " + _FunctionName);
                }
                else if (_FunctionName == "POWER" || _FunctionName == "POW")
                {
                    if (_Arguments.Length == 2)
                    {
                        a = (SMParserAtom)_Arguments[0];
                        r.Name = _FunctionName + "(" + a.Value.ToString() + ";" + ((SMParserAtom)_Arguments[1]).Value.ToString() + ")";
                        r.Value = Math.Pow(a.Value, ((SMParserAtom)_Arguments[1]).Value);
                    }
                    else r.Error("Wrong parameters number in: " + _FunctionName);
                }
                else if (_FunctionName == "RND")
                {
                    if (_Arguments.Length == 1)
                    {
                        a = (SMParserAtom)_Arguments[0];
                        r.Name = _FunctionName + "(" + a.Value.ToString() + ")";
                        r.Value = new Random().Next(Convert.ToInt32(a.Value));
                    }
                    else r.Error("Wrong parameters number in: " + _FunctionName);
                }
                else if (_FunctionName == "COS")
                {
                    if (_Arguments.Length == 1)
                    {
                        a = (SMParserAtom)_Arguments[0];
                        r.Name = _FunctionName + "(" + a.Value.ToString() + ")";
                        r.Value = Math.Cos(a.Value);
                    }
                    else r.Error("Wrong parameters number in: " + _FunctionName);
                }
                else if (_FunctionName == "SIN")
                {
                    if (_Arguments.Length == 1)
                    {
                        a = (SMParserAtom)_Arguments[0];
                        r.Name = _FunctionName + "(" + a.Value.ToString() + ")";
                        r.Value = Math.Sin(a.Value);
                    }
                    else r.Error("Wrong parameters number in: " + _FunctionName);
                }
                else if (_FunctionName == "TAN")
                {
                    if (_Arguments.Length == 1)
                    {
                        a = (SMParserAtom)_Arguments[0];
                        r.Name = _FunctionName + "(" + a.Value.ToString() + ")";
                        r.Value = Math.Tan(a.Value);
                    }
                    else r.Error("Wrong parameters number in: " + _FunctionName);
                }
                else if (_FunctionName == "COSH")
                {
                    if (_Arguments.Length == 1)
                    {
                        a = (SMParserAtom)_Arguments[0];
                        r.Name = _FunctionName + "(" + a.Value.ToString() + ")";
                        r.Value = Math.Cosh(a.Value);
                    }
                    else r.Error("Wrong parameters number in: " + _FunctionName);
                }
                else if (_FunctionName == "SINH")
                {
                    if (_Arguments.Length == 1)
                    {
                        a = (SMParserAtom)_Arguments[0];
                        r.Name = _FunctionName + "(" + a.Value.ToString() + ")";
                        r.Value = Math.Sinh(a.Value);
                    }
                    else r.Error("Wrong parameters number in: " + _FunctionName);
                }
                else if (_FunctionName == "TANH")
                {
                    if (_Arguments.Length == 1)
                    {
                        a = (SMParserAtom)_Arguments[0];
                        r.Name = _FunctionName + "(" + a.Value.ToString() + ")";
                        r.Value = Math.Tanh(a.Value);
                    }
                    else r.Error("Wrong parameters number in: " + _FunctionName);
                }
                else if (_FunctionName == "LOG")
                {
                    if (_Arguments.Length == 1)
                    {
                        a = (SMParserAtom)_Arguments[0];
                        r.Name = _FunctionName + "(" + a.Value.ToString() + ")";
                        r.Value = Math.Log10(a.Value);
                    }
                    else if (_Arguments.Length == 2)
                    {
                        a = (SMParserAtom)_Arguments[0];
                        r.Name = _FunctionName + "(" + a.Value.ToString() + ";" + ((SMParserAtom)_Arguments[1]).Value.ToString() + ")";
                        r.Value = Math.Log(a.Value, ((SMParserAtom)_Arguments[1]).Value);
                    }
                    else r.Error("Wrong parameters number in: " + _FunctionName);
                }
                else if (_FunctionName == "LN")
                {
                    if (_Arguments.Length == 1)
                    {
                        a = (SMParserAtom)_Arguments[0];
                        r.Name = _FunctionName + "(" + a.Value.ToString() + ")";
                        r.Value = Math.Log(a.Value);
                    }
                    else r.Error("Wrong parameters number in: " + _FunctionName);
                }
                else if (_FunctionName == "SQRT")
                {
                    if (_Arguments.Length == 1)
                    {
                        a = (SMParserAtom)_Arguments[0];
                        r.Name = _FunctionName + "(" + a.Value.ToString() + ")";
                        r.Value = Math.Sqrt(a.Value);
                    }
                    else r.Error("Wrong parameters number in: " + _FunctionName);
                }
                else if (_FunctionName == "ACOS")
                {
                    if (_Arguments.Length == 1)
                    {
                        a = (SMParserAtom)_Arguments[0];
                        r.Name = _FunctionName + "(" + a.Value.ToString() + ")";
                        r.Value = Math.Acos(a.Value);
                    }
                    else r.Error("Wrong parameters number in: " + _FunctionName);
                }
                else if (_FunctionName == "ASIN")
                {
                    if (_Arguments.Length == 1)
                    {
                        a = (SMParserAtom)_Arguments[0];
                        r.Name = _FunctionName + "(" + a.Value.ToString() + ")";
                        r.Value = Math.Asin(a.Value);
                    }
                    else r.Error("Wrong parameters number in: " + _FunctionName);
                }
                else if (_FunctionName == "ATAN")
                {
                    if (_Arguments.Length == 1)
                    {
                        a = (SMParserAtom)_Arguments[0];
                        r.Name = _FunctionName + "(" + a.Value.ToString() + ")";
                        r.Value = Math.Atan(a.Value);
                    }
                    else r.Error("Wrong parameters number in: " + _FunctionName);
                }
                else if (_FunctionName == "EXP")
                {
                    if (_Arguments.Length == 1)
                    {
                        a = (SMParserAtom)_Arguments[0];
                        r.Name = _FunctionName + "(" + a.Value.ToString() + ")";
                        r.Value = Math.Exp(a.Value);
                    }
                    else r.Error("Wrong parameters number in: " + _FunctionName);
                }
                else r.Error("Undefined function: " + _FunctionName);
            }
            catch (Exception ex)
            {
                r.Error("Error computing function: " + _FunctionName + " " + ex.Message);
            }
            return r;
        }

        /// <summary>Returns value evaluating postfix notation.</summary>
        private double EvaluatePostfix()
        {
            double r = 0;
            SMParserAtom a, a1, a2, ar;
            Stack stk = new Stack();
            ArrayList pm = new ArrayList();
            Error = false;
            int i = 0, h = exprPostfix.Count;
            try
            {
                while (!Error && i < h)
                {
                    a = exprPostfix.Items[i];
                    if (a.Type == SMParserAtomType.Value || a.Type == SMParserAtomType.Variable || a.Type == SMParserAtomType.Result || a.Type == SMParserAtomType.Comma) stk.Push(a);
                    else if (a.Type == SMParserAtomType.Operator)
                    {
                        a1 = (SMParserAtom)stk.Pop();
                        a2 = (SMParserAtom)stk.Pop();
                        ar = Evaluate(a2, a, a1);
                        if (ar.Type == SMParserAtomType.Error) { Error = true; ErrorMessage = ar.Name; }
                        else stk.Push(ar);
                    }
                    else if (a.Type == SMParserAtomType.Function)
                    {
                        pm.Clear();
                        a1 = (SMParserAtom)stk.Pop();
                        if (a1.Type == SMParserAtomType.Value || a1.Type == SMParserAtomType.Variable || a1.Type == SMParserAtomType.Result)
                        {
                            ar = EvaluateFunction(a.Name, a1);
                            if (ar.Type == SMParserAtomType.Error) { Error = true; ErrorMessage = ar.Name; }
                            else stk.Push(ar);
                        }
                        else if (a1.Type == SMParserAtomType.Comma)
                        {
                            while (a1.Type == SMParserAtomType.Comma)
                            {
                                a1 = (SMParserAtom)stk.Pop();
                                pm.Add(a1);
                                a1 = (SMParserAtom)stk.Pop();
                            }
                            pm.Add(a1);
                            ar = EvaluateFunction(a.Name, pm.ToArray());
                            if (ar.Type == SMParserAtomType.Error) { Error = true; ErrorMessage = ar.Name; }
                            else stk.Push(ar);
                        }
                        else
                        {
                            stk.Push(a1);
                            ar = EvaluateFunction(a.Name);
                            if (ar.Type == SMParserAtomType.Error) { Error = true; ErrorMessage = ar.Name; }
                            else stk.Push(ar);
                        }
                    }
                    i++;
                }
                if (!Error && stk.Count == 1)
                {
                    ar = (SMParserAtom)stk.Pop();
                    r = ar.Value;
                }
            }
            catch
            {
                Error = true;
                ErrorMessage = "Syntax error";
            }
            return r;
        }

        #endregion

        /* */

    }

    /* */

}