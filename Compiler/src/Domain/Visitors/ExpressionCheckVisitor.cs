using System;
using System.Collections.Generic;

namespace MiniPLInterpreter
{
	public class ExpressionCheckVisitor : NodeVisitor
	{
		private SemanticAnalyzer analyzer;

		public ExpressionCheckVisitor (SemanticAnalyzer analyzer)
		{
			this.analyzer = analyzer;
		}

		public void VisitAssertNode(AssertNode node)
		{
			TokenType type = checkExpressionIsLegit (node.Expression);
			matchEvaluationType (node, type, TokenType.BOOL_VAL);
		}

		public void VisitAssignNode(AssignNode node)
		{
			VariableIdNode idNode = node.IDNode;

			if (idNode == null) {
				analyzer.notifyError (new SemanticError (Constants.SEMANTIC_ERROR_MESSAGE, node));
			} else {
				IProperty property = analyzer.IDs [idNode.ID];

				if (!property.Declared) {
					analyzer.notifyError (new UninitializedVariableError (idNode));
				}

				TokenType evaluated = checkExpressionIsLegit(node.ExprNode);

				if (property.GetTokenType () != evaluated) {
					analyzer.notifyError (new IllegalTypeError(idNode));
				}
			}
		}

		public void VisitBinOpNode(BinOpNode node) {}

		public void VisitDeclarationNode(DeclarationNode node)
		{
			VariableIdNode idNode = node.IDNode;

			if (idNode == null) {
				analyzer.notifyError (new SemanticError (Constants.SEMANTIC_ERROR_MESSAGE, node));
			} else {
				IProperty property = analyzer.IDs [idNode.ID];

				if (property.Declared) {
					analyzer.notifyError (new DeclarationError (idNode));
				} else {
					property.Declared = true;
					node.AssignNode.Accept (this);
				}
			}
		}

		public void VisitForLoopNode(ForLoopNode node)
		{
			VariableIdNode idNode = node.IDNode;

			if (idNode == null) {
				analyzer.notifyError (new SemanticError (Constants.SEMANTIC_ERROR_MESSAGE, node));
			} else {
				IProperty property = analyzer.IDs [idNode.ID];

				if (!property.Declared) {
					analyzer.notifyError (new UninitializedVariableError (idNode));
				}

				if (property.GetTokenType () != TokenType.INT_VAL) {
					analyzer.notifyError (new IllegalTypeError(idNode));
				}
			}

			IExpressionNode max = node.MaxValue;

			if (max == null) {
				analyzer.notifyError (new SemanticError (Constants.SEMANTIC_ERROR_MESSAGE, node));
			} else {
				TokenType type = checkExpressionIsLegit (max);

				if (type != TokenType.INT_VAL) {
					analyzer.notifyError (new IllegalTypeError(idNode));
				}
			}

			node.RangeFrom.Accept (this);

			node.Statements.Accept (this);
		}

		public void VisitIntValueNode(IntValueNode node) {}

		public void VisitIOPrintNode(IOPrintNode node) {}

		public void VisitIOReadNode(IOReadNode node) 
		{
			VariableIdNode idNode = node.IDNode;

			if (idNode == null) {
				analyzer.notifyError (new SemanticError (Constants.SEMANTIC_ERROR_MESSAGE, node));
			} else {
				IProperty property = analyzer.IDs [idNode.ID];

				if (!property.Declared) {
					analyzer.notifyError (new UninitializedVariableError (idNode));
				}

				if (property.GetTokenType () == TokenType.BOOL_VAL) {
					analyzer.notifyError (new IllegalTypeError(idNode));
				}
			}
		}

		public void VisitRootNode(RootNode node) {
			if (node.Sequitor != null) {
				node.Sequitor.Accept (this);
			}
		}

		public void VisitStatementsNode(StatementsNode node) {
			if (node.Statement != null) {
				node.Statement.Accept (this);
			}

			if (node.Sequitor != null) {
				node.Sequitor.Accept (this);
			}
		}

		public void VisitStringValueNode(StringValueNode node) {}

		public void VisitUnOpNode(UnOpNode node) {}

		public void VisitVariableIdNode(VariableIdNode node) {}

		private TokenType checkExpressionIsLegit(IExpressionNode node)
		{
			IExpressionNode[] expressions = node.GetExpressions ();

			if (expressions == null) {
				return node.GetEvaluationType (TokenType.UNDEFINED);
			}

			TokenType tt = TokenType.UNDEFINED;

			for (int i = 0; i < expressions.Length; i++) {
				IExpressionNode en = expressions [i];

				if (en == null) {
					continue;
				}

				TokenType childType = checkExpressionIsLegit (en);

				if (childType == TokenType.ERROR) {
					return childType;
				}

				if (tt == TokenType.UNDEFINED) {
					tt = childType;
				} else if (childType != tt) {
					return TokenType.ERROR;
				}
			}

			if (Constants.LEGIT_OPERATIONS [tt].ContainsKey (node.GetOperation ())) {
				return tt;
			}

			return TokenType.ERROR;
		}

		private bool checkExpressionMatchesType (IExpressionNode node, TokenType expectedType)
		{
			TokenType evaluationType = node.GetEvaluationType (TokenType.UNDEFINED);

			return evaluationType == expectedType;
		}

		private void matchEvaluationType(ISyntaxTreeNode node, TokenType evaluated, TokenType expected)
		{
			if (evaluated != expected) {
				analyzer.notifyError (new IllegalTypeError (node));
			}
		}
	}
}

