using System;

namespace MiniPLInterpreter
{
	public interface IProperty
	{
		Type GetPropertyType ();
		TokenType GetTokenType ();
		bool Declared {
			get;
			set;
		}
	}
}

