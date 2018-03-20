using System;

namespace MiniPLInterpreter
{
	public class SemanticError : Error, ISemanticCheckValue
	{
		public SemanticError (string title, string message, ISyntaxTreeNode node)
			: base (title, message, node)
		{
		}

		public SemanticError (string message, ISyntaxTreeNode node)
			: this (Constants.SEMANTIC_ERROR_TITLE, message, node)
		{
		}

		public IProperty asProperty ()
		{
			return null;
		}
	}
}

