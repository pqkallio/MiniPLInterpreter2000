using System;
using System.Collections.Generic;

namespace MiniPLInterpreter
{
	public class ExecutionVisitor : INodeVisitor
	{
		private Dictionary<string, IProperty> ids;
		private EvaluationVisitor evaluator;
		private VoidProperty voidProperty;
		private IPrinter printer;

		public ExecutionVisitor (Dictionary<string, IProperty> ids, IPrinter printer)
		{
			this.ids = ids;
			this.evaluator = new EvaluationVisitor (this.ids);
			this.voidProperty = new VoidProperty ();
			this.printer = printer;
		}

		public ISemanticCheckValue VisitRootNode(RootNode node)
		{
			if (node.Sequitor != null) {
				return node.Sequitor.Accept (this);
			}

			return voidProperty;
		}

		public ISemanticCheckValue VisitStatementsNode (StatementsNode node)
		{
			if (node.Statement != null) {
				node.Statement.Accept (this);
			}

			if (node.Sequitor != null) {
				return node.Sequitor.Accept (this);
			}

			return voidProperty;
		}

		public ISemanticCheckValue VisitAssignNode(AssignNode node)
		{
			VariableIdNode idNode = node.IDNode;
			IProperty assignValue = node.ExprNode.Accept (this).asProperty ();
			ids [idNode.ID] = assignValue;

			return voidProperty;
		}

		public ISemanticCheckValue VisitBinOpNode(BinOpNode node)
		{
			return node.Accept (evaluator);
		}

		public ISemanticCheckValue VisitDeclarationNode(DeclarationNode node)
		{
			addNewId (node.IDNode);
			node.AssignNode.Accept (this);

			return voidProperty;
		}

		public ISemanticCheckValue VisitIntValueNode(IntValueNode node)
		{
			return node.Accept (evaluator);
		}

		public ISemanticCheckValue VisitBoolValueNode(BoolValueNode node)
		{
			return node.Accept (evaluator);
		}

		public ISemanticCheckValue VisitStringValueNode(StringValueNode node)
		{
			return node.Accept (evaluator);
		}

		public ISemanticCheckValue VisitUnOpNode(UnOpNode node)
		{
			return node.Accept (evaluator);
		}

		public ISemanticCheckValue VisitVariableIdNode(VariableIdNode node)
		{
			return node.Accept (evaluator);
		}

		public ISemanticCheckValue VisitAssertNode(AssertNode node)
		{
			IProperty evaluation = node.Accept(this.evaluator).asProperty();
			if (!evaluation.asBoolean ()) {
				printer.printAssertionFailure (node);
			}

			return voidProperty;
		}

		public ISemanticCheckValue VisitIOPrintNode(IOPrintNode node)
		{
			IProperty evaluation = node.Expression.Accept (this).asProperty ();
			printer.print (evaluation.asString ());

			return voidProperty;
		}

		public ISemanticCheckValue VisitIOReadNode(IOReadNode node) 
		{
			string input = Console.ReadLine ();
			input = input.Split (new[] {' ', '\t', '\n'})[0];

			AssignNode assignNode = node.AssignNode;
			setAssignValue (input, assignNode);
			assignNode.Accept (this);

			return voidProperty;
		}

		public ISemanticCheckValue VisitForLoopNode(ForLoopNode node)
		{
			int max = node.MaxValue.Accept (this).asProperty ().asInteger ();
			node.RangeFrom.Accept (this);
			VariableIdNode idNode = node.IDNode;

			while (idNode.Accept (this).asProperty ().asInteger () <= max) {
				node.Statements.Accept (this);
				node.Accumulator.Accept (this);
			}
			return voidProperty;
		}

		private void addNewId (VariableIdNode idNode)
		{
			switch (idNode.VariableType) {
			case TokenType.INT_VAL:
				ids [idNode.ID] = new IntegerProperty (SemanticAnalysisConstants.DEFAULT_INTEGER_VALUE);
				break;
			case TokenType.STR_VAL:
				ids [idNode.ID] = new StringProperty (SemanticAnalysisConstants.DEFAULT_STRING_VALUE);
				break;
			case TokenType.BOOL_VAL:
				ids [idNode.ID] = new BooleanProperty (SemanticAnalysisConstants.DEFAULT_BOOL_VALUE);
				break;
			default:
				throw new ArgumentException ();
			}
		}

		private void setAssignValue(string input, AssignNode assignNode)
		{
			TokenType expectedType = assignNode.IDNode.EvaluationType;
			switch (expectedType) {
				case TokenType.INT_VAL:
					if (!StringUtils.isInteger (input)) {
						throw new ArgumentException ();
					}
					assignNode.AddExpression (new IntValueNode (StringUtils.parseToInt (input)));
					break;
				case TokenType.STR_VAL:
					assignNode.AddExpression (new StringValueNode (input));
					break;
				default:
					throw new ArgumentException ();	
			}
		}
	}
}

