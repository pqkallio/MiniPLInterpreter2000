using System;

namespace MiniPLInterpreter
{
	public class AssertNode : ISyntaxTreeNode, IExpressionContainer
	{
		private ISyntaxTreeNode expressionNode;

		public AssertNode ()
		{}

		public TokenType Type ()
		{
			return TokenType.ASSERT;
		}

		public object execute ()
		{
			bool eval = (bool)expressionNode.execute ();

			if (!eval) {
				Console.WriteLine (String.Format("Assertion failed: {0}", expressionNode.ToString()));
			}

			return null;
		}

		public void AddExpression (ISyntaxTreeNode expressionNode)
		{
			this.expressionNode = expressionNode;
		}
	}
}

