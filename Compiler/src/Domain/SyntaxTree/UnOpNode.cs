using System;
using System.Collections;

namespace MiniPLInterpreter
{
	public class UnOpNode : IExpressionContainer, IOperandContainer, IExpressionNode
	{
		private IExpressionNode operand;
		private TokenType operation;
		private Token token;
		private TokenType evaluationType;

		public UnOpNode (Token t)
		{
			this.token = t;
			this.evaluationType = TokenType.UNDEFINED;
		}

		public IExpressionNode Operand {
			get { return operand; }
			set { this.operand = value; }
		}

		public TokenType Operation {
			get { return operation; }
			set { this.operation = value; }
		}

		public void AddExpression(IExpressionNode expressionNode)
		{
			this.operand = expressionNode;
		}

		public void AddOperand(ISyntaxTreeNode operandNode)
		{
			this.operand = (IExpressionNode)operandNode;
		}

		public override string ToString ()
		{
			return this.operation.ToString ();
		}

		public TokenType EvaluationType
		{
			get { return this.evaluationType; }
			set { this.evaluationType = value; }
		}

		public IExpressionNode[] GetExpressions()
		{
			IExpressionNode[] expressions = { this.operand };

			return expressions;
		}

		public TokenType GetOperation ()
		{
			return operation;
		}

		public ISemanticCheckValue Accept(INodeVisitor visitor) {
			return visitor.VisitUnOpNode (this);
		}

		public Token Token
		{
			get { return this.token; }
			set { }
		}
	}
}

