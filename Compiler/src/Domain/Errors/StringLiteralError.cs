using System;

namespace MiniPLInterpreter
{
	public class StringLiteralError : Error
	{

		public StringLiteralError (Token token) 
			: this(token,
				Constants.STRING_LITERAL_ERROR_MESSAGE)
		{}

		public StringLiteralError (Token token, string message) 
			: base(Constants.SCANNER_ERROR_TITLE,
				message, token)
		{}
	}
}

