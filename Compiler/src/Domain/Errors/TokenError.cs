using System;

namespace MiniPLInterpreter
{
	public class TokenError : Error
	{
		public TokenError (Token token) 
			: base(Constants.SCANNER_ERROR_TITLE, 
				   Constants.TOKEN_ERROR_MESSAGE, 
				   token)
		{
		}
	}
}

