using System;
using System.Collections;

namespace MiniPLInterpreter
{
	public class UnOpNode : IExpressionContainer, IOperandContainer, IExpressionNode
	{
		private IExpressionNode operand;
		private TokenType operation;
		private Token token;

		public UnOpNode ()
		{}

		public IExpressionNode Operand {
			get { return operand; }
			set { this.operand = value; }
		}

		public TokenType Operation {
			get { return operation; }
			set { this.operation = value; }
		}

		public TokenType Type () {
			return TokenType.UNARY_OP;
		}

		public object execute () {
			bool evaluation = (bool)((ISyntaxTreeNode)operand).execute ();

			if (operation == TokenType.UNARY_OP_LOG_NEG) {
				return !evaluation;
			}

			throw new ArgumentException (String.Format ("the operation {0} is not defined", operation));
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

		public void AddNodesToQueue (Queue q)
		{
			q.Enqueue (this);
			((ISyntaxTreeNode)operand).AddNodesToQueue (q);
		}

		public TokenType GetEvaluationType (TokenType parentType)
		{
			if (parentType == TokenType.ERROR) {
				return parentType;
			}

			return operand.GetEvaluationType (parentType);
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

		public TokenType GetValueType ()
		{
			return TokenType.UNDEFINED;
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

