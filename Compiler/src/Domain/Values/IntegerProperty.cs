using System;

namespace MiniPLInterpreter
{
	public class IntegerProperty : IProperty
	{
		private int value;
		private bool declared;

		public IntegerProperty (int value)
		{
			this.value = value;
			this.declared = false;
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

		public IProperty asProperty ()
		{
			return this;
		}
	}
}

