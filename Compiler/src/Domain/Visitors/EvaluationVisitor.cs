using System;
using System.Collections.Generic;
using System.Text;

namespace MiniPLInterpreter
{
	public class EvaluationVisitor : INodeVisitor
	{
		private Dictionary<string, IProperty> ids;
		private VoidProperty voidProperty;

		public EvaluationVisitor (Dictionary<string, IProperty> ids)
		{
			this.ids = ids;
			this.voidProperty = new VoidProperty ();
		}

		public ISemanticCheckValue VisitAssertNode(AssertNode node)
		{
			return node.Expression.Accept (this);
		}

		public ISemanticCheckValue VisitAssignNode(AssignNode node)
		{
			return voidProperty;
		}

		public ISemanticCheckValue VisitBinOpNode(BinOpNode node)
		{
			IProperty leftHandEval = getEvaluation (node.LeftOperand);

			if (node.Operation == TokenType.BINARY_OP_NO_OP) {
				return leftHandEval;
			}

			IProperty rightHandEval = getEvaluation (node.RightOperand);
			IProperty evaluation = binaryOperation (node.Operation, leftHandEval, rightHandEval);

			return evaluation;
		}

		public ISemanticCheckValue VisitDeclarationNode(DeclarationNode node)
		{
			return voidProperty;
		}

		public ISemanticCheckValue VisitForLoopNode(ForLoopNode node)
		{
			return voidProperty;
		}

		public ISemanticCheckValue VisitIntValueNode(IntValueNode node)
		{
			IntValueNode newNode = new IntValueNode (node.Value);

			return newNode;
		}

		public ISemanticCheckValue VisitBoolValueNode(BoolValueNode node)
		{
			BoolValueNode newNode = new BoolValueNode (node.Value);

			return newNode;
		}

		public ISemanticCheckValue VisitIOPrintNode(IOPrintNode node)
		{
			return voidProperty;
		}

		public ISemanticCheckValue VisitIOReadNode(IOReadNode node)
		{
			return voidProperty;
		}

		public ISemanticCheckValue VisitRootNode(RootNode node)
		{
			return voidProperty;
		}

		public ISemanticCheckValue VisitStatementsNode(StatementsNode node)
		{
			return voidProperty;
		}

		public ISemanticCheckValue VisitStringValueNode(StringValueNode node)
		{
			StringValueNode newNode = new StringValueNode (node.Value);

			return newNode;
		}

		public ISemanticCheckValue VisitUnOpNode(UnOpNode node)
		{
			IProperty operandEval = getEvaluation (node.Operand);

			return unaryOperation (node.Operation, operandEval);
		}

		public ISemanticCheckValue VisitVariableIdNode(VariableIdNode node)
		{
			IProperty idProperty = ids [node.ID];

			switch (idProperty.GetTokenType ()) {
				case TokenType.INT_VAL:
					return new IntegerProperty (idProperty.asInteger ());
				case TokenType.STR_VAL:
					return new StringProperty (idProperty.asString ());
				case TokenType.BOOL_VAL:
					return new BooleanProperty (idProperty.asBoolean ());
				default:
					throw new ArgumentException ();
			}
		}

		private IProperty getEvaluation(params IExpressionNode[] expressions)
		{
			IProperty evaluatedType = expressions [0].Accept (this).asProperty ();
			for (int i = 1; i < expressions.Length; i++) {
				IExpressionNode expression = expressions [i];
				IProperty retVal = expression.Accept (this).asProperty();
				if (retVal.GetTokenType () != evaluatedType.GetTokenType ()) {
					return new ErrorProperty ();
				}

				if (evaluatedType.GetTokenType () == TokenType.ERROR) {
					break;
				}
			}

			return evaluatedType;
		}

		public IProperty binaryOperation (TokenType operation, IProperty firstOperand, IProperty secondOperand) {
			if (Constants.LOGICAL_OPERATIONS.ContainsKey (operation)) {
				return booleanBinOp (operation, firstOperand, secondOperand);
			}

			switch (firstOperand.GetTokenType ()) {
				case TokenType.INT_VAL:
					return integerBinOp (operation, firstOperand, secondOperand);
				case TokenType.STR_VAL:
					return stringBinOp (operation, firstOperand, secondOperand);
				case TokenType.BOOL_VAL:
					return booleanBinOp (operation, firstOperand, secondOperand);
				default:
					throw new ArgumentException ();
			}
		}

		public IProperty integerBinOp (TokenType operation, IProperty firstOperand, IProperty secondOperand)
		{
			int evaluation;

			switch (operation) {
				case TokenType.BINARY_OP_ADD:
					evaluation = firstOperand.asInteger () + secondOperand.asInteger ();
					break;
				case TokenType.BINARY_OP_SUB:
					evaluation = firstOperand.asInteger () - secondOperand.asInteger ();
					break;
				case TokenType.BINARY_OP_MUL:
					evaluation = firstOperand.asInteger () * secondOperand.asInteger ();
					break;
				case TokenType.BINARY_OP_DIV:
					evaluation = firstOperand.asInteger () / secondOperand.asInteger ();
					break;
				default:
					throw new ArgumentException (String.Format ("operation {0} not defined for integer values", operation));
			}

			firstOperand.setInteger (evaluation);

			return firstOperand;
		}

		public IProperty stringBinOp (TokenType operation, IProperty firstOperand, IProperty secondOperand)
		{
			StringBuilder sb = new StringBuilder (firstOperand.asString ());

			switch (operation) {
				case TokenType.BINARY_OP_ADD:
					sb.Append (secondOperand.asString ());
					break;
				default:
					throw new ArgumentException (String.Format ("operation {0} not defined for string values", operation));
			}

			firstOperand.setString (sb.ToString());

			return firstOperand;
		}

		public IProperty booleanBinOp(TokenType operation, IProperty firstOperand, IProperty secondOperand)
		{
			switch (firstOperand.GetTokenType ()) {
				case TokenType.INT_VAL:
					return integerComparison (operation, firstOperand, secondOperand);
				case TokenType.STR_VAL:
					return stringComparison (operation, firstOperand, secondOperand);
				case TokenType.BOOL_VAL:
					return booleanComparison (operation, firstOperand, secondOperand);
				default:
					throw new ArgumentException ();
			}

		}

		private IProperty integerComparison (TokenType operation, IProperty firstOperand, IProperty secondOperand)
		{
			BooleanProperty evaluation = new BooleanProperty ();

			switch (operation) {
				case TokenType.BINARY_OP_LOG_EQ:
					evaluation.setBoolean (firstOperand.asInteger () == secondOperand.asInteger ());
					break;
				case TokenType.BINARY_OP_LOG_LT:
					evaluation.setBoolean (firstOperand.asInteger () < secondOperand.asInteger ());
					break;
				default:
					throw new ArgumentException ();
			}

			return evaluation;
		}

		private IProperty stringComparison (TokenType operation, IProperty firstOperand, IProperty secondOperand)
		{
			BooleanProperty evaluation = new BooleanProperty ();

			switch (operation) {
				case TokenType.BINARY_OP_LOG_EQ:
					evaluation.setBoolean (firstOperand.asString ().CompareTo(secondOperand.asString ()) == 0);
					break;
				case TokenType.BINARY_OP_LOG_LT:
					evaluation.setBoolean (firstOperand.asString ().CompareTo(secondOperand.asString ()) < 0);
					break;
				default:
					throw new ArgumentException ();
			}

			return evaluation;
		}

		private IProperty booleanComparison (TokenType operation, IProperty firstOperand, IProperty secondOperand)
		{
			switch (operation) {
				case TokenType.BINARY_OP_LOG_EQ:
					firstOperand.setBoolean (firstOperand.asBoolean () == secondOperand.asBoolean ());
					break;
				case TokenType.BINARY_OP_LOG_LT:
					firstOperand.setBoolean (firstOperand.asBoolean ().CompareTo(secondOperand.asBoolean ()) < 0);
					break;
				case TokenType.BINARY_OP_LOG_AND:
					firstOperand.setBoolean (firstOperand.asBoolean () && secondOperand.asBoolean ());
					break;
				default:
					throw new ArgumentException ();
			}

			return firstOperand;
		}

		private IProperty unaryOperation (TokenType operation, IProperty operandEvaluation)
		{
			switch (operandEvaluation.GetTokenType ()) {
				case TokenType.BOOL_VAL:
					return booleanUnOp (operation, operandEvaluation);
				case TokenType.INT_VAL:
					return integerUnOp (operation, operandEvaluation);
				case TokenType.STR_VAL:
					return stringUnOp (operation, operandEvaluation);
				default:
					throw new ArgumentException();
			}	
		}

		private IProperty booleanUnOp (TokenType operation, IProperty operandEvaluation)
		{
			switch (operation) {
				case TokenType.UNARY_OP_LOG_NEG:
					operandEvaluation.setBoolean (!operandEvaluation.asBoolean ());
					return operandEvaluation;
				default:
					throw new ArgumentException ();
			}
		}

		private IProperty integerUnOp (TokenType operation, IProperty operandEvaluation)
		{
			switch (operation) {
				default:
					throw new ArgumentException ();
			}
		}

		private IProperty stringUnOp (TokenType operation, IProperty operandEvaluation)
		{
			switch (operation) {
				default:
					throw new ArgumentException ();
			}
		}
	}
}