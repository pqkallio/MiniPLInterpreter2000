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
		bool Constant {
			get;
			set;
		}
		int asInteger ();
		string asString ();
		bool asBoolean ();
		void setInteger (int value);
		void setString (string value);
		void setBoolean (bool value);
	}
}

