using System;

namespace MiniPLInterpreter
{
	public class BooleanProperty : IProperty
	{
		private bool value;
		private bool declared;

		public BooleanProperty (bool value)
		{
			this.value = value;
			this.declared = false;
		}

		public Type GetPropertyType ()
		{
			return typeof(bool);
		}

		public TokenType GetTokenType ()
		{
			return TokenType.BOOL_VAL;
		}

		public bool Value {
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

