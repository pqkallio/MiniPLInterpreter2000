using System;

namespace MiniPLInterpreter
{
	public class IntegerOverflowError : Error
	{
		public IntegerOverflowError (Token token)
			: base(ErrorConstants.INTEGER_OVERFLOW_ERROR_TITLE, ErrorConstants.INTEGER_OVERFLOW_ERROR_MESSAGE, token)
		{}
	}
}

