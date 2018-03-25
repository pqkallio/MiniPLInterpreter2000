using System;

namespace MiniPLInterpreter
{
	public class UninitializedVariableError : SemanticError
	{
		public UninitializedVariableError (ISyntaxTreeNode node)
			: base(ErrorConstants.UNINITIALIZED_VARIABLE_ERROR_MESSAGE, node)
		{}
	}
}

