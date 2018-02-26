using System;

namespace MiniPLInterpreter
{
	public class UnOpNode : ISyntaxTreeNode, IExpressionContainer, IOperandContainer
	{
		private ISyntaxTreeNode operand;
		private TokenType operation;

		public UnOpNode ()
		{}

		public ISyntaxTreeNode Operand {
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
			bool evaluation = (bool)operand.execute ();

			if (operation == TokenType.UNARY_OP_LOG_NEG) {
				return !evaluation;
			}

			throw new ArgumentException (String.Format ("the operation {0} is not defined", operation));
		}

		public void AddExpression(ISyntaxTreeNode expressionNode)
		{
			this.operand = expressionNode;
		}

		public void AddOperand(ISyntaxTreeNode operandNode)
		{
			this.operand = operandNode;
		}
	}
}

