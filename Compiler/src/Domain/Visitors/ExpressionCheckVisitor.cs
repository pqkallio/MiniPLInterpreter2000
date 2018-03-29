using System;
using System.Collections.Generic;

namespace MiniPLInterpreter
{
	public class ExpressionCheckVisitor : INodeVisitor
	{
		private SemanticAnalyzer analyzer;
		private TypeCheckingVisitor typeChecker;
		private VoidProperty voidProperty;

		public ExpressionCheckVisitor (SemanticAnalyzer analyzer)
		{
			this.analyzer = analyzer;
			this.typeChecker = new TypeCheckingVisitor (analyzer);
			this.voidProperty = new VoidProperty ();
		}

		public ISemanticCheckValue VisitAssignNode(AssignNode node)
		{
			IProperty property = getVariableProperty (node);

			if (property == voidProperty) {
				return voidProperty;
			}

			VariableIdNode idNode = node.IDNode;

			checkPropertyDeclared (idNode, property, true);

			if (property.Constant) {
				analyzer.notifyError (new IllegalAssignmentError (node));
			}

			IProperty evaluated = node.Accept (this.typeChecker).asProperty ();

			if (!checkPropertyType(property, evaluated.GetTokenType ())) {
				analyzer.notifyError (new IllegalTypeError(idNode));
			}

			return voidProperty;
		}

		public ISemanticCheckValue VisitBinOpNode(BinOpNode node)
		{
			return voidProperty;
		}

		public ISemanticCheckValue VisitDeclarationNode(DeclarationNode node)
		{
			IProperty property = getVariableProperty (node);

			if (property == voidProperty) {
				return voidProperty;
			}

			VariableIdNode idNode = node.IDNode;

			if (property.Declared) {
				analyzer.notifyError (new DeclarationError (idNode));
			} else {
				property.Declared = true;
				node.AssignNode.Accept (this);
			}

			return voidProperty;
		}

		public ISemanticCheckValue VisitIntValueNode(IntValueNode node)
		{
			return voidProperty;
		}

		public ISemanticCheckValue VisitBoolValueNode(BoolValueNode node)
		{
			return voidProperty;
		}

		public ISemanticCheckValue VisitRootNode(RootNode node)
		{
			if (node.Sequitor != null) {
				node.Sequitor.Accept (this);
			}

			return voidProperty;
		}

		public ISemanticCheckValue VisitStatementsNode(StatementsNode node)
		{
			if (node.Statement != null) {
				node.Statement.Accept (this);
			}

			if (node.Sequitor != null) {
				node.Sequitor.Accept (this);
			}

			return voidProperty;
		}

		public ISemanticCheckValue VisitStringValueNode(StringValueNode node)
		{
			return voidProperty;
		}

		public ISemanticCheckValue VisitUnOpNode(UnOpNode node)
		{
			return voidProperty;
		}
		
		public ISemanticCheckValue VisitVariableIdNode(VariableIdNode node)
		{
			return voidProperty;
		}

		public ISemanticCheckValue VisitAssertNode(AssertNode node)
		{
			IProperty evaluation = node.Accept(this.typeChecker).asProperty();

			if (!checkPropertyType(evaluation, TokenType.BOOL_VAL)) {
				analyzer.notifyError (new IllegalTypeError (node));
			}

			return voidProperty;
		}
		
		public ISemanticCheckValue VisitIOPrintNode(IOPrintNode node)
		{
			node.Expression.Accept (this.typeChecker).asProperty ();

			return voidProperty;
		}

		public ISemanticCheckValue VisitIOReadNode(IOReadNode node) 
		{
			IProperty property = getVariableProperty (node);

			if (property == voidProperty) {
				return voidProperty;
			}

			VariableIdNode idNode = node.IDNode;

			if (!property.Declared) {
				analyzer.notifyError (new UninitializedVariableError (idNode));
			}

			if (checkPropertyType(property, TokenType.BOOL_VAL)) {
				analyzer.notifyError (new IllegalTypeError(idNode));
			}

			return voidProperty;
		}

		public ISemanticCheckValue VisitForLoopNode(ForLoopNode node)
		{
			IProperty property = getVariableProperty (node);

			if (property == voidProperty) {
				return voidProperty;
			}

			VariableIdNode idNode = node.IDNode;

			if (!property.Declared) {
				analyzer.notifyError (new UninitializedVariableError (idNode));
			}

			if (property.GetTokenType () != TokenType.INT_VAL) {
				analyzer.notifyError (new IllegalTypeError(idNode));
			}

			analyzer.SymbolicTable [idNode.ID].Constant = true;

			IExpressionNode max = node.MaxValue;

			if (max == null) {
				analyzer.notifyError (new SemanticError(node));
			} else {
				IProperty maxProperty = max.Accept(this.typeChecker).asProperty();

				if (!checkPropertyType(maxProperty, TokenType.INT_VAL)) {
					analyzer.notifyError (new IllegalTypeError(max));
				}
			}

			IProperty rangeFromProperty = node.RangeFrom.Accept (this.typeChecker).asProperty();

			if (!checkPropertyType(rangeFromProperty, TokenType.INT_VAL)) {
				analyzer.notifyError (new IllegalTypeError(node.RangeFrom));
			}

			node.Statements.Accept (this);

			analyzer.SymbolicTable [idNode.ID].Constant = false;

			return voidProperty;
		}

		private IProperty getVariableProperty(IIdentifierContainer node)
		{
			if (node.IDNode == null) {
				analyzer.notifyError(new UninitializedVariableError(node));
				return voidProperty;
			}

			VariableIdNode idNode = node.IDNode;

			IProperty property = idNode.Accept(this.typeChecker).asProperty();

			if (property.GetTokenType () == TokenType.ERROR) {
				analyzer.notifyError(new UninitializedVariableError(node));
				return voidProperty;
			}

			return property;
		}

		private bool checkPropertyType (IProperty property, TokenType expected)
		{
			TokenType tokenType = property.GetTokenType ();

			return tokenType == TokenType.ERROR || tokenType == expected;
		}

		private void checkPropertyDeclared(ISyntaxTreeNode node, IProperty property, bool declarationExpected)
		{
			if (declarationExpected) {
				if (!property.Declared) {
					analyzer.notifyError (new UninitializedVariableError (node));
				}
			} else {
				if (property.Declared) {
					analyzer.notifyError (new DeclarationError (node));
				}
			}
		}
	}
}

