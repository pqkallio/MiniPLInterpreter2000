using System;

namespace MiniPLInterpreter
{
	public class InvalidIdentifierError : Error
	{
		public InvalidIdentifierError (Token token)
			: base(ErrorConstants.SCANNER_ERROR_TITLE, ErrorConstants.INVALID_IDENTIFIER_MESSAGE, token)
		{}
	}
}

