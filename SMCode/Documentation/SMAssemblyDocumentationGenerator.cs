/*  ===========================================================================
 *  
 *  File:       SMAssemblyDocumentationGenerator.cs
 *  Version:    2.0.x
 *  Date:       March 2025
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2025 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *
 *  SMCode assembly documentation generator class.
 *
 *  ===========================================================================
 */

using SMCodeSystem;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace SMFrontSystem
{

	/* */

	/// <summary>SMCode assembly documentation generator class.</summary>
	public class SMAssemblyDocumentationGenerator
	{

        /* */

        #region Declarations

        /*  ===================================================================
         *  Declarations
         *  ===================================================================
         */

        /// <summary>SM session instance.</summary>
        private readonly SMCode SM = null;

        #endregion

        /* */

        #region Properties

        /*  ===================================================================
         *  Properties
         *  ===================================================================
         */

        /// <summary>Get or set assembly file full path.</summary>
        public Assembly Assembly { get; set; } = null;

        /// <summary>Get or set assembly file full path.</summary>
        public string AssemblyPath { get; set; } = null;

        /// <summary>Get or set assembly xml.</summary>
        public XDocument AssemblyXml { get; set; } = null;

        /// <summary>Get assembly documentation string builder.</summary>
        public StringBuilder Text { get; private set; } = new StringBuilder();

		#endregion

		/* */

		#region Initialization

		/*  ===================================================================
         *  Initialization
         *  ===================================================================
         */

		/// <summary>Class constructor.</summary>
		public SMAssemblyDocumentationGenerator(SMCode _SM = null)
        {
            SM = SMCode.CurrentOrNew(_SM);
            InitializeInstance();
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

        /// <summary>Clear item.</summary>
        public void Clear()
        {
            Assembly = null;
            AssemblyPath = null;
            AssemblyXml = null;
        }

        /// <summary>Load assembly from file.</summary>
        public bool Load(string _AssemblyPath, string _XmlPath = null)
        {
            try
            {
                Clear();
                if (!SM.Empty(_AssemblyPath))
                {
                    Assembly = Assembly.Load(_AssemblyPath);
                    if (SM.Empty(_XmlPath)) _XmlPath = SM.ChangeExtension(_AssemblyPath, "xml");
                    if (SM.FileExists(_XmlPath)) AssemblyXml = XDocument.Load(_XmlPath);
                    return true;
                }
                else return false;
            }
            catch (Exception ex)
            {
                SM.Error(ex);
                return false;
            }
        }

		/// <summary>Gets the summary documentation for a specific member from the XML file.</summary>
		public string GetXmlSummary(string _Member)
		{
			int i;
			string rslt = "";
			XAttribute attr;
			XElement element, summary;
			List<XElement> elements;
			try
			{
				element = AssemblyXml.Root.Element("members");
				if (element != null)
				{
					elements = new List<XElement>(element.Elements("member"));
					if (elements != null)
					{
						for (i = 0; i < elements.Count; i++)
						{
							attr = elements[i].Attribute("name");
							if (attr != null)
							{
								if (attr.Value != null)
								{
									if (attr.Value == _Member)
									{
										summary = elements[i].Element("summary");
										if (summary != null)
										{
											rslt = SM.ToStr(summary.Value);
										}
									}
								}
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				SM.Error(ex);
			}
			return rslt;
		}

		/// <summary>Save documentation to file.</summary>
		public bool Save(string _Path)
		{
			return SM.SaveString(_Path, Text.ToString());
		}

		#endregion

		/* */

		#region Methods - Generate 

		/*  ===================================================================
         *  Methods - Generate
         *  ===================================================================
         */

		/// <summary>Generate whole assembly documentation.</summary>
		public bool Generate()
        {
            int i;
            Type[] types;
            try
            {
                Text.Clear();
                if (Assembly != null)
                {
                    types = Assembly.GetTypes();
                    if (types != null)
                    {
                        for (i = 0; i < types.Length; i++)
                        {
                            Generate(types[i]);
                        }
                    }
                }
				return true;
			}
			catch (Exception ex) 
            { 
                SM.Error(ex); 
                return false; 
            }
        }

		/// <summary>Generate whole method documentation.</summary>
		public bool Generate(MethodInfo _Method)
		{
			string s;
			try
			{
				if (_Method != null)
				{
					Text.AppendLine($"## {_Method.Name}");
                    Text.AppendLine("");

					s = GetXmlSummary($"M:{_Method.DeclaringType.FullName}.{_Method.Name}");
                    if (!SM.Empty(s))
                    {
                        Text.AppendLine(s);
						Text.AppendLine("");
					}
				}
				return true;
			}
			catch (Exception ex)
			{
				SM.Error(ex);
				return false;
			}
		}

		/// <summary>Generate whole type documentation.</summary>
		public bool Generate(Type _Type)
		{
            int i;
			string s;
            MethodInfo[] methods;
			try
			{
				if (_Type != null)
				{
					Text.AppendLine($"# Documentation for {_Type.Namespace}.{_Type.Name}");
					s = GetXmlSummary($"T:{_Type.FullName}");
					if (!SM.Empty(s)) Text.AppendLine(s);
					//
					methods = _Type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);
                    if (methods!=null)
                    {
                        for (i = 0; i < methods.Length; i++)
                        {
                            Generate(methods[i]);
                        }
                    }
				}
				return true;
			}
			catch (Exception ex)
			{
				SM.Error(ex);
				return false;
			}
		}

		#endregion

		/* */

	}

    /* */

}
