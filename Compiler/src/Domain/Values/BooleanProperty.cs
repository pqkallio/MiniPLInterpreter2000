using System;

namespace MiniPLInterpreter
{
	public class BooleanProperty : IProperty
	{
		private bool value;
		private bool declared;

		public BooleanProperty ()
			: this(false)
		{}

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

		public IProperty asProperty ()
		{
			return this;
		}

		public int asInteger ()
		{
			return Value ? 1 : 0;
		}

		public string asString ()
		{
			return Value ? "true" : "false";
		}

		public bool asBoolean ()
		{
			return Value;
		}

		public void setInteger (int value) {}

		public void setString (string value) {}

		public void setBoolean (bool value)
		{
			Value = value;
		}
	}
}

