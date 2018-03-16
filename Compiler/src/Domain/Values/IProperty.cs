using System;

namespace MiniPLInterpreter
{
	public interface IProperty : ISemanticCheckValue
	{
		Type GetPropertyType ();
		TokenType GetTokenType ();
		bool Declared {
			get;
			set;
		}
	}
}

