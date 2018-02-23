using System;

namespace MiniPLInterpreter
{
	public class NumericUtils
	{
		public static bool IntBetween(int num, int min, int max)
		{
			return num >= min && num <= max;
		}

		public static object Evaluate (int leftOperand, int rightOperand, TokenType operation)
		{
			switch (operation) {
				case TokenType.BINARY_OP_ADD:
					return leftOperand + rightOperand;
				case TokenType.BINARY_OP_SUB:
					return leftOperand - rightOperand;
				case TokenType.BINARY_OP_MUL:
					return leftOperand * rightOperand;
				case TokenType.BINARY_OP_DIV:
					return leftOperand / rightOperand;
				case TokenType.BINARY_OP_LOG_EQ:
				case TokenType.BINARY_OP_LOG_LT:
					return BooleanUtils.EvaluateBinOp (leftOperand, rightOperand, operation);
				default:
					throw new ArgumentException (String.Format ("operation {0} not defined for integer values", operation));
			}
		}
	}
}

