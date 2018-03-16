using System;

namespace MiniPLInterpreter
{
	public class MismatchProperty : IProperty
	{
		public MismatchProperty ()
		{
		}

		public Type GetPropertyType ()
		{
			return typeof(Error);
		}

		public TokenType GetTokenType ()
		{
			return TokenType.ERROR;
		}

		public bool Declared {
			get { return false; }
			set { }
		}

		public IProperty asProperty ()
		{
			return null;
		}
	}
}

