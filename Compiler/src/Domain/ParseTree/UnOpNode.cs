﻿using System;

namespace MiniPLInterpreter
{
	public class UnOpNode : ISyntaxTreeNode, IExpressionContainer
	{
		private ISyntaxTreeNode operand;
		private TokenType operation;

		public UnOpNode (TokenType operation)
		{
			this.operation = operation;
		}

		public ISyntaxTreeNode Operand {
			get { return operand; }
			set { this.operand = value; }
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
	}
}
