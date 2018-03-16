using System;
using System.Collections.Generic;

namespace MiniPLInterpreter
{
	public class ExpressionCheckVisitor : INodeVisitor
	{
		private SemanticAnalyzer analyzer;
		private TypeCheckingVisitor typeChecker;

		public ExpressionCheckVisitor (SemanticAnalyzer analyzer)
		{
			this.analyzer = analyzer;
			this.typeChecker = new TypeCheckingVisitor (analyzer);
		}

		public ISemanticCheckValue VisitAssignNode(AssignNode node)
		{
			VariableIdNode idNode = node.IDNode;
			Console.WriteLine (idNode.ID + ": " + idNode.GetValueType () + " / " + analyzer.IDs [idNode.ID].GetTokenType ());

			if (idNode == null) {
				analyzer.notifyError (new SemanticError (Constants.SEMANTIC_ERROR_MESSAGE, node));
			} else {
				IProperty property = analyzer.IDs [idNode.ID];
				Console.WriteLine (property.GetTokenType ());

				if (!property.Declared) {
					analyzer.notifyError (new UninitializedVariableError (idNode));
				}

				IProperty evaluated = node.Accept (this.typeChecker).asProperty();
				Console.WriteLine (evaluated.GetTokenType ());
				if (property.GetTokenType () != evaluated.GetTokenType ()) {
					analyzer.notifyError (new IllegalTypeError(idNode));
				}
			}
			return null;
		}

		public ISemanticCheckValue VisitBinOpNode(BinOpNode node) {return null;}

		public ISemanticCheckValue VisitDeclarationNode(DeclarationNode node)
		{
			VariableIdNode idNode = node.IDNode;
			Console.WriteLine (idNode.ID + ": " + idNode.GetValueType () + " / " + analyzer.IDs [idNode.ID].GetTokenType ());
			if (idNode == null) {
				analyzer.notifyError (new SemanticError (Constants.SEMANTIC_ERROR_MESSAGE, node));
			} else {
				IProperty property = idNode.Accept(this.typeChecker).asProperty();
				Console.WriteLine (property.GetTokenType ());
				if (property.Declared) {
					analyzer.notifyError (new DeclarationError (idNode));
				} else {
					property.Declared = true;
					node.AssignNode.Accept (this);
				}
			}
			return null;
		}

		public ISemanticCheckValue VisitIntValueNode(IntValueNode node) {return null;}

		public ISemanticCheckValue VisitRootNode(RootNode node) {
			if (node.Sequitor != null) {
				node.Sequitor.Accept (this);
			}
			return null;
		}

		public ISemanticCheckValue VisitStatementsNode(StatementsNode node) {
			if (node.Statement != null) {
				node.Statement.Accept (this);
			}

			if (node.Sequitor != null) {
				node.Sequitor.Accept (this);
			}
			return null;
		}








		public ISemanticCheckValue VisitStringValueNode(StringValueNode node) {return null;}

		public ISemanticCheckValue VisitUnOpNode(UnOpNode node) {return null;}
		
		public ISemanticCheckValue VisitVariableIdNode(VariableIdNode node) {return null;}

		public ISemanticCheckValue VisitAssertNode(AssertNode node)
		{
			IProperty evaluation = node.Accept(this.typeChecker).asProperty();

			if (evaluation.GetTokenType () != TokenType.BOOL_VAL) {
				analyzer.notifyError (new IllegalTypeError (node));
			}

			return null;
		}
		
		public ISemanticCheckValue VisitIOPrintNode(IOPrintNode node) {return null;}

		public ISemanticCheckValue VisitIOReadNode(IOReadNode node) 
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

			return null;
		}

		public ISemanticCheckValue VisitForLoopNode(ForLoopNode node)
		{
			VariableIdNode idNode = node.IDNode;

			if (idNode == null) {
				analyzer.notifyError (new SemanticError (Constants.SEMANTIC_ERROR_MESSAGE, node));
			} else {
				IProperty property = idNode.Accept(this.typeChecker).asProperty();

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
				IProperty p = max.Accept(this.typeChecker).asProperty();

				if (p.GetTokenType () != TokenType.INT_VAL) {
					analyzer.notifyError (new IllegalTypeError(max));
				}
			}

			IProperty p2 = node.RangeFrom.Accept (this.typeChecker).asProperty();

			if (p2.GetTokenType () != TokenType.INT_VAL) {
				analyzer.notifyError (new IllegalTypeError(node.RangeFrom));
			}

			node.Statements.Accept (this);

			return null;
		}
	}
}

