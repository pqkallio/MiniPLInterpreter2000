using System;
using System.Collections;

namespace MiniPLInterpreter
{
	public class IOPrintNode: IExpressionContainer
	{
		private IExpressionNode expression;
		private Token token;

		public IOPrintNode (Token t)
		{
			this.token = t;
		}

		public TokenType Type ()
		{
			return TokenType.PRINT;
		}

		public IExpressionNode Expression
		{
			get { return expression; }
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

		public ISemanticCheckValue Accept(INodeVisitor visitor) {
			return visitor.VisitIOPrintNode (this);
		}

		public Token Token
		{
			get { return this.token; }
			set { }
		}
	}
}

