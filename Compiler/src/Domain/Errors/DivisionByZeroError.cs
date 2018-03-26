using System;

namespace MiniPLInterpreter
{
	public class DivisionByZeroError : Error
	{
		public DivisionByZeroError (Token token)
			: base(ErrorConstants.DIVISION_BY_ZERO_TITLE, ErrorConstants.DIVISION_BY_ZERO_MESSAGE, token)
		{}
	}
}

