using System;

namespace MiniPLInterpreter
{
	public class StringLiteralError : Error
	{
		public StringLiteralError (Token token) 
			: base(Constants.SCANNER_ERROR_TITLE,
				   Constants.STRING_LITERAL_ERROR_MESSAGE,
				   token)
		{
		}
	}
}

