﻿using System;

namespace MiniPLInterpreter
{
	public class StringProperty : IProperty
	{
		private string value;
		private bool declared;

		public StringProperty (string value)
		{
			this.value = value;
		}

		public Type GetPropertyType ()
		{
			return typeof(string);
		}

		public TokenType GetTokenType ()
		{
			return TokenType.STR_VAL;
		}

		public string Value {
			get { return this.value; }
			set { this.value = value; }
		}

		public bool Declared
		{
			get { return declared; }
			set { declared = value; }
		}
	}
}

