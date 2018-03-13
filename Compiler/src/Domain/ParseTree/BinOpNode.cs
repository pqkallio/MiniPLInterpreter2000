using System;
using System.Collections;

namespace MiniPLInterpreter
{
	public class BinOpNode : IExpressionContainer, IOperandContainer, IExpressionNode
	{
		private IExpressionNode leftOperand;
		private IExpressionNode rightOperand;
		private TokenType operation;

		public BinOpNode ()
		{
			this.operation = TokenType.BINARY_OP_NO_OP;
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

		public TokenType Type () {
			return TokenType.BINARY_OP;
		}

		public object execute () {
			object leftEval = ((ISyntaxTreeNode)leftOperand).execute ();

			if (rightOperand != null) {
				object rightEval = ((ISyntaxTreeNode)rightOperand).execute ();

				if (leftEval.GetType () == rightEval.GetType ()) {
					if (leftEval.GetType () == typeof(string)) {
						return StringUtils.Evaluate ((string)leftEval, (string)rightEval, operation);
					} else if (leftEval.GetType () == typeof(int)) {
						return NumericUtils.Evaluate ((int)leftEval, (int)rightEval, operation);
					} else if (leftEval.GetType () == typeof(bool)) {
						return BooleanUtils.EvaluateBinOp ((bool)leftEval, (bool)rightEval, operation);
					}
				}

				// throw new ArgumentException (String.Format ("the operation {0} is not defined for type {1}", operation, leftEval.GetType ()));
			}

			return leftEval;

		}

		public void AddExpression(IExpressionNode expressionNode)
		{
			this.rightOperand = expressionNode;
		}

		public override string ToString ()
		{
			return this.operation.ToString ();
		}

		public void AddNodesToQueue (Queue q)
		{
			q.Enqueue (this);
			((ISyntaxTreeNode)leftOperand).AddNodesToQueue (q);
			if (rightOperand != null) {
				((ISyntaxTreeNode)rightOperand).AddNodesToQueue (q);
			}
		}

		public TokenType GetEvaluationType (TokenType parentType)
		{
			if (parentType == TokenType.ERROR) {
				return parentType;
			}

			TokenType leftOperandType = leftOperand.GetEvaluationType (parentType);

			if (leftOperandType == TokenType.ERROR) {
				return leftOperandType;
			}

			TokenType rightOperandType = rightOperand.GetEvaluationType (leftOperandType);

			return rightOperandType;
		}

		public IExpressionNode[] GetExpressions()
		{
			IExpressionNode[] expressions = new IExpressionNode[2];
			expressions [0] = leftOperand;

			if (rightOperand != null) {
				expressions [1] = rightOperand;
			}

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

		public void Accept(NodeVisitor visitor) {
			visitor.VisitBinOpNode (this);
		}
	}
}