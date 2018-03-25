using System;

namespace MiniPLInterpreter
{
	public class NullPointerError : SemanticError
	{
		public NullPointerError (ISyntaxTreeNode node)
			: base(ErrorConstants.NULL_POINTER_ERROR_MESSAGE, node)
		{}
	}
}

