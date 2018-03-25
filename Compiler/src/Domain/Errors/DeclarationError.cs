using System;

namespace MiniPLInterpreter
{
	public class DeclarationError : SemanticError
	{
		public DeclarationError (ISyntaxTreeNode node)
			:base(ErrorConstants.DECLARATION_ERROR_MESSAGE, node)
		{
		}
	}
}

