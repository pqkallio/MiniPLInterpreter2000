using System;

namespace MiniPLInterpreter
{
	public class BinOpNode : ISyntaxTreeNode, IExpressionContainer
	{
		private ISyntaxTreeNode leftOperand;
		private ISyntaxTreeNode rightOperand;
		private TokenType operation;

		public BinOpNode ()
		{
			this.operation = TokenType.UNDEFINED;
		}

		public ISyntaxTreeNode LeftOperand {
			get { return leftOperand; }
			set { this.leftOperand = value; }
		}

		public ISyntaxTreeNode RightOperand {
			get { return rightOperand; }
			set { this.rightOperand = value; }
		}

		public void AddOperand (ISyntaxTreeNode operand)
		{
			if (leftOperand == null) {
				leftOperand = operand;
			} else if (rightOperand == null) {
				rightOperand = operand;
			}
		}

		public TokenType Operation {
			get { return this.operation; }
			set { this.operation = value; }
		}

		public TokenType Type () {
			return TokenType.BINARY_OP;
		}

		public object execute () {
			object leftEval = leftOperand.execute ();
			object rightEval = rightOperand.execute ();

			if (leftEval.GetType () == rightEval.GetType ()) {
				if (leftEval.GetType () == typeof(string)) {
					return StringUtils.Evaluate ((string)leftEval, (string)rightEval, operation);
				} else if (leftEval.GetType () == typeof(int)) {
					return NumericUtils.Evaluate ((int)leftEval, (int)rightEval, operation);
				} else if (leftEval.GetType () == typeof(bool)) {
					return BooleanUtils.EvaluateBinOp ((bool)leftEval, (bool)rightEval, operation);
				}
			}

			throw new ArgumentException (String.Format ("the operation {0} is not defined for type {1}", operation, leftEval.GetType ()));
		}

		public void AddExpression(ISyntaxTreeNode expressionNode)
		{
			this.rightOperand = expressionNode;
		}
	}
}