using System;

namespace MiniPLInterpreter
{
	public class IdError : Error
	{
		public IdError (Token token) 
			: base(Constants.SCANNER_ERROR_TITLE, 
				   Constants.ID_ERROR_MESSAGE, 
				   token)
		{
		}
	}
}

