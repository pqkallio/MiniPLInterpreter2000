using System;

namespace MiniPLInterpreter
{
	public class BooleanUtils
	{
		public static bool EvaluateUnOp (bool operand, TokenType operation)
		{
			switch (operation) {
				case TokenType.UNARY_OP_LOG_NEG:
					return !operand;
				default:
					throw new ArgumentException (String.Format ("operation {0} not defined as a boolean unary operation", operation));
			}
		}

		public static bool EvaluateBinOp(object leftOperand, object rightOperand, TokenType operation)
		{
			if (leftOperand.GetType () == rightOperand.GetType ()) {
				switch (operation) {
					case TokenType.BINARY_OP_LOG_EQ:
						return leftOperand.Equals (rightOperand);
					case TokenType.BINARY_OP_LOG_LT:
						if (leftOperand.GetType () == typeof(int)) {
							int lo = (int)leftOperand;
							int ro = (int)rightOperand;
							return lo < ro;
						} else if (leftOperand.GetType () == typeof(string)) {
							string lo = (string)leftOperand;
							string ro = (string)rightOperand;
							return String.Compare (lo, ro) < 0;
						} else if (leftOperand.GetType () == typeof(bool)) {
							bool lo = (bool)leftOperand;
							bool ro = (bool)rightOperand;
							return lo.CompareTo (ro) < 0;
						}
						break;
					case TokenType.BINARY_OP_LOG_AND:
						if (leftOperand.GetType () == typeof(bool)) {
							bool lo = (bool)leftOperand;
							bool ro = (bool)rightOperand;
							return lo & ro;
						}
						break;
					default:
						throw new ArgumentException (String.Format ("operation {0} not defined as a boolean binary operation for type {1}", operation, leftOperand.GetType ()));
				}
			}

			throw new ArgumentException (String.Format ("binary operation {0} not defined for types {1} and {2}", operation, leftOperand.GetType (), rightOperand.GetType ()));
		}
	}
}