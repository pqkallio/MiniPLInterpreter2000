using System;

namespace MiniPLInterpreter
{
	public class TokenError : Error
	{
		public TokenError (Token token)
			: this(token, ErrorConstants.TOKEN_ERROR_MESSAGE)
		{
		}

		public TokenError (Token token, string message) 
			: base(ErrorConstants.SCANNER_ERROR_TITLE, message, token)
		{
		}
	}
}

