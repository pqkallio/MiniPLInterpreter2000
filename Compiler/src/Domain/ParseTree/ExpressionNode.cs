using System;

namespace MiniPLInterpreter
{
	public class ExpressionNode : ISyntaxTreeNode
	{
		ISyntaxTreeNode expression;

		public ExpressionNode ()
		{
		}

		public Object execute ()
		{
			return null;
		}

		public TokenType Type ()
		{
			return TokenType.EXPRESSION;
		}
	}
}

