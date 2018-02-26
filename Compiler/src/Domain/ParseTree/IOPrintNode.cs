using System;

namespace MiniPLInterpreter
{
	public class IOPrintNode: ISyntaxTreeNode, IExpressionContainer
	{
		private ISyntaxTreeNode expression;

		public IOPrintNode ()
		{}

		public TokenType Type ()
		{
			return TokenType.PRINT;
		}

		public object execute ()
		{
			Console.WriteLine (expression.execute ());
			return null;
		}

		public void AddExpression(ISyntaxTreeNode expressionNode)
		{
			this.expression = expressionNode;
		}
	}
}

