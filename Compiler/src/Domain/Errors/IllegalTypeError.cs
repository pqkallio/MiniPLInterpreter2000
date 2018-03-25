using System;

namespace MiniPLInterpreter
{
	public class IllegalTypeError : SemanticError
	{
		public IllegalTypeError (ISyntaxTreeNode node)
			: base(ErrorConstants.ILLEGAL_TYPE_ERROR_MESSAGE, node)
		{}
	}
}

