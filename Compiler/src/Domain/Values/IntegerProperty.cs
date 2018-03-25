using System;

namespace MiniPLInterpreter
{
	public class IntegerProperty : IProperty
	{
		private int value;
		private bool declared;
		private bool constant;

		public IntegerProperty (int value)
		{
			this.value = value;
			this.declared = false;
			this.constant = false;
		}

		public Type GetPropertyType ()
		{
			return typeof(int);
		}

		public TokenType GetTokenType ()
		{
			return TokenType.INT_VAL;
		}

		public int Value {
			get { return this.value; }
			set { this.value = value; }
		}

		public bool Declared
		{
			get { return declared; }
			set { declared = value; }
		}

		public bool Constant
		{
			get { return this.constant; }
			set { this.constant = value; }
		}

		public IProperty asProperty ()
		{
			return this;
		}

		public int asInteger ()
		{
			return Value;
		}

		public string asString ()
		{
			return StringUtils.IntToString (Value);
		}

		public bool asBoolean ()
		{
			return Value != 0;
		}

		public void setInteger (int value)
		{
			Value = value;
		}

		public void setString (string value) {}

		public void setBoolean (bool value) {}
	}
}

