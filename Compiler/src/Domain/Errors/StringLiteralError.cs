using System;

namespace MiniPLInterpreter
{
	public class StringLiteralError : Error
	{

		public StringLiteralError (Token token) 
			: this(token,
				ErrorConstants.STRING_LITERAL_ERROR_MESSAGE)
		{}

		public StringLiteralError (Token token, string message) 
			: base(ErrorConstants.SCANNER_ERROR_TITLE,
				message, token)
		{}
	}
}

