using System;

namespace MiniPLInterpreter
{
	public class IllegalAssignmentError : SemanticError
	{
		public IllegalAssignmentError (ISyntaxTreeNode node)
			: base(ErrorConstants.ILLEGAL_ASSIGNMENT_ERROR_MESSAGE, node)
		{}
	}
}

