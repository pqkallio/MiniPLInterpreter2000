using System;

namespace MiniPLInterpreter
{
	public class StatementNode : ISyntaxTreeNode
	{
		public StatementNode ()
		{
		}

		public Object execute ()
		{
			return null;
		}

		public TokenType Type()
		{
			return TokenType.UNDEFINED;
		}
	}
}

