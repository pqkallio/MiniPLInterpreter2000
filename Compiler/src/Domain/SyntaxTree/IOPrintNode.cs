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

		public IExpressionNode Expression
		{
			get { return expression; }
		}

		public void AddExpression(IExpressionNode expressionNode)
		{
			this.expression = expressionNode;
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

