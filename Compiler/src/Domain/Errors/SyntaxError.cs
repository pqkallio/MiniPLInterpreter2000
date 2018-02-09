using System;

namespace MiniPLInterpreter
{
	public class SyntaxError : Error
	{
		public SyntaxError (Token token) 
			: base(Constants.SYNTAX_ERROR_TITLE, Constants.SYNTAX_ERROR_MESSAGE, token)
		{
		}
	}
}

