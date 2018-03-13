using System;
using System.Collections;

namespace MiniPLInterpreter
{
	public class IOPrintNode: IExpressionContainer
	{
		private IExpressionNode expression;

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

		public void AddExpression(IExpressionNode expressionNode)
		{
			this.expression = expressionNode;
		}

		public void AddNodesToQueue (Queue q)
		{
			q.Enqueue (this);
			expression.AddNodesToQueue (q);
		}

		public void Accept(NodeVisitor visitor) {
			visitor.VisitIOPrintNode (this);
		}
	}
}

