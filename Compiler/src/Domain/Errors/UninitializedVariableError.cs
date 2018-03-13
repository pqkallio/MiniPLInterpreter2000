using System;

namespace MiniPLInterpreter
{
	public class UninitializedVariableError : SemanticError
	{
		public UninitializedVariableError (ISyntaxTreeNode node)
			: base(Constants.UNINITIALIZED_VARIABLE_ERROR_MESSAGE, node)
		{
		}
	}
}

