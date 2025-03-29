/*  ===========================================================================
 *  
 *  File:       SMField.cs
 *  Version:    2.0.230
 *  Date:       March 2025
 *  Author:     Stefano Mengarelli  
 *  E-mail:     info@stefanomengarelli.it
 *  
 *  Copyright (C) 2010-2025 by Stefano Mengarelli - All rights reserved - Use, 
 *  permission and restrictions under license.
 *
 *  SMCode fields management class.
 *
 *  ===========================================================================
 */

using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SMCodeSystem
{

	/* */

	/// <summary>SMCode fields management class.</summary>
	public class SMField
    {

		/* */

		#region Declarations

		/*  ===================================================================
         *  Declarations
         *  ===================================================================
         */

		#endregion

		/* */

		#region Properties

		/*  ===================================================================
         *  Properties
         *  ===================================================================
         */

		/// <summary>Get or set field description.</summary>
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string Description { get; set; } = null;

		/// <summary>Get or set field name.</summary>
		public string Field { get; set; } = string.Empty;

		/// <summary>Get or set field value format.</summary>
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string Format { get; set; } = null;

		/// <summary>Get or set field informations.</summary>
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string Informations { get; set; } = null;

		/// <summary>Sub items.</summary>   
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public List<SMField> Items { get; set; } = null;

		/// <summary>Field value max length.</summary>
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public int? Length { get; set; } = null;

		/// <summary>Get or set field options.</summary>
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string Options { get; set; } = null;

		/// <summary>Get or set required flag.</summary>
		public bool Required { get; set; } = false;

		/// <summary>Get or set field name.</summary>
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
		public string Type { get; set; } = null;

        /// <summary>Get or set field name.</summary>
        public object Value { get; set; } = null;

		#endregion

		/* */

		#region Methods

		/*  ===================================================================
         *  Methods
         *  ===================================================================
         */

		/// <summary>Assign instance properties from another.</summary>
		public void Assign(SMField _Field)
		{
			int i;
			Description = _Field.Description;
			Field = _Field.Field;
			Format = _Field.Format;
			Informations = _Field.Informations;
			Items.Clear();
			for (i = 0; i <_Field.Items.Count; i++) Items.Add(_Field.Items[i]);
			Options = _Field.Options;
			Required = _Field.Required;
			Type = _Field.Type;
			Value = _Field.Value;
		}

		/// <summary>Clear item.</summary>
		public void Clear()
		{
			Description = string.Empty;
			Field = string.Empty;
			Format = string.Empty;
			Informations = null;
			Items.Clear();
			Options = null;
			Required = false;
			Type = string.Empty;
			Value = null;
		}

		/// <summary>Assign property from JSON serialization.</summary>
		public bool FromJSON(string _JSON)
		{
			try
			{
				Assign((SMField)JsonSerializer.Deserialize(_JSON, null));
				return true;
			}
			catch
			{
				return false;
			}
		}

		/// <summary>Return JSON serialization of instance.</summary>
		public string ToJSON()
		{
			try
			{
				return JsonSerializer.Serialize(this);
			}
			catch
			{
				return "";
			}
		}

		#endregion

		/* */

	}

	/* */

}
