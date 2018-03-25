using System;
using System.Collections;

namespace MiniPLInterpreter
{
	public class BinOpNode : IExpressionContainer, IOperandContainer, IExpressionNode
	{
		private IExpressionNode leftOperand;
		private IExpressionNode rightOperand;
		private TokenType operation;
		private TokenType evaluationType;
		private Token token;

		public BinOpNode (Token t)
		{
			this.operation = TokenType.BINARY_OP_NO_OP;
			this.token = t;
			this.evaluationType = TokenType.UNDEFINED;
		}

		public IExpressionNode LeftOperand {
			get { return leftOperand; }
			set { this.leftOperand = value; }
		}

		public IExpressionNode RightOperand {
			get { return rightOperand; }
			set { this.rightOperand = value; }
		}

		public void AddOperand (ISyntaxTreeNode operandNode)
		{
			if (leftOperand == null) {
				leftOperand = (IExpressionNode)operandNode;
			} else if (rightOperand == null) {
				rightOperand = (IExpressionNode)operandNode;
			}
		}

		public TokenType Operation {
			get { return this.operation; }
			set { this.operation = value; }
		}

		public void AddExpression(IExpressionNode expressionNode)
		{
			this.rightOperand = expressionNode;
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
			IExpressionNode[] expressions;

			if (rightOperand != null) {
				expressions = new IExpressionNode[] { leftOperand, rightOperand };
			} else {
				expressions = new IExpressionNode[] { leftOperand };
			}

			return expressions;
		}

		public TokenType GetOperation ()
		{
			return operation;
		}

		public ISemanticCheckValue Accept(INodeVisitor visitor) {
			return visitor.VisitBinOpNode (this);
		}

		public Token Token
		{
			get { return this.token; }
			set { }
		}
	}
}