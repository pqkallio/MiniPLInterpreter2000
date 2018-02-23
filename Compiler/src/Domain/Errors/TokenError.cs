using System;

namespace MiniPLInterpreter
{
	public class TokenError : Error
	{
		public TokenError (Token token)
			: this(token, Constants.TOKEN_ERROR_MESSAGE)
		{
		}

		public TokenError (Token token, string message) 
			: base(Constants.SCANNER_ERROR_TITLE, message, token)
		{
		}
	}
}

