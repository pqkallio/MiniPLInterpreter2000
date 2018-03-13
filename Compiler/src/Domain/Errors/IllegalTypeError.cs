using System;

namespace MiniPLInterpreter
{
	public class IllegalTypeError : SemanticError
	{
		public IllegalTypeError (ISyntaxTreeNode node)
			: base(Constants.ILLEGAL_TYPE_ERROR_MESSAGE, node)
		{
		}
	}
}

