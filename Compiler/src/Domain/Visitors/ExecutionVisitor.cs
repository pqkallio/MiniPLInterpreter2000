﻿using System;
using System.Collections.Generic;

namespace MiniPLInterpreter
{
	public class ExecutionVisitor : INodeVisitor
	{
		private Dictionary<string, IProperty> ids;
		private EvaluationVisitor evaluator;
		private VoidProperty voidProperty;

		public ExecutionVisitor (Dictionary<string, IProperty> ids)
		{
			this.ids = ids;
			this.evaluator = new EvaluationVisitor (this.ids);
			this.voidProperty = new VoidProperty ();
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
			Console.WriteLine (evaluation.GetTokenType());

			return voidProperty;
		}

		public ISemanticCheckValue VisitIOPrintNode(IOPrintNode node)
		{
			IProperty evaluation = node.Expression.Accept (this).asProperty ();
			Console.WriteLine (evaluation.asString ());

			return voidProperty;
		}

		public ISemanticCheckValue VisitIOReadNode(IOReadNode node) 
		{
			string input = Console.ReadLine ();

			AssignNode assignNode = node.AssignNode;
			setAssignValue (input, assignNode);
			assignNode.Accept (this);

			return voidProperty;
		}

		public ISemanticCheckValue VisitForLoopNode(ForLoopNode node)
		{
			VariableIdNode idNode = node.IDNode;

			IExpressionNode max = node.MaxValue;

			node.Statements.Accept (this);

			return voidProperty;
		}

		private void addNewId (VariableIdNode idNode)
		{
			switch (idNode.VariableType) {
			case TokenType.INT_VAL:
				ids [idNode.ID] = new IntegerProperty (Constants.DEFAULT_INTEGER_VALUE);
				break;
			case TokenType.STR_VAL:
				ids [idNode.ID] = new StringProperty (Constants.DEFAULT_STRING_VALUE);
				break;
			case TokenType.BOOL_VAL:
				ids [idNode.ID] = new BooleanProperty (Constants.DEFAULT_BOOL_VALUE);
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
