using System;
using System.Collections.Generic;

namespace MiniPLInterpreter
{
	public class TypeCheckingVisitor : INodeVisitor
	{
		private SemanticAnalyzer analyzer;

		public TypeCheckingVisitor (SemanticAnalyzer analyzer)
		{
			this.analyzer = analyzer;
		}

		public ISemanticCheckValue VisitAssertNode(AssertNode node)
		{
			return getEvaluation (node.Expression);
		}

		public ISemanticCheckValue VisitAssignNode(AssignNode node)
		{
			return getEvaluation (node.ExprNode);
		}

		public ISemanticCheckValue VisitBinOpNode(BinOpNode node)
		{
			return VisitOperationNode (node);
		}

		public ISemanticCheckValue VisitDeclarationNode(DeclarationNode node)
		{
			return node.AssignNode.Accept (this);
		}

		public ISemanticCheckValue VisitForLoopNode(ForLoopNode node)
		{
			IProperty rangeFrom = (IProperty)(node.RangeFrom.Accept (this));
			IProperty max = (IProperty)(node.MaxValue.Accept (this));
			bool alright = true;

			if (!SemanticAnalysisConstants.LEGIT_OPERATIONS.ContainsKey(rangeFrom.GetTokenType ()) ||
				!SemanticAnalysisConstants.LEGIT_OPERATIONS [rangeFrom.GetTokenType ()].ContainsKey (TokenType.RANGE_FROM)) {
				analyzer.notifyError(new IllegalTypeError(node.RangeFrom));
				alright = false;
			}

			if (!SemanticAnalysisConstants.LEGIT_OPERATIONS.ContainsKey(max.GetTokenType ()) ||
				!SemanticAnalysisConstants.LEGIT_OPERATIONS [max.GetTokenType ()].ContainsKey (TokenType.RANGE_UPTO)) {
				analyzer.notifyError(new IllegalTypeError(node.MaxValue));
				alright = false;
			}

			if (!alright) {
				return new ErrorProperty ();
			}

			return rangeFrom;
		}

		public ISemanticCheckValue VisitIntValueNode(IntValueNode node)
		{
			return node;
		}

		public ISemanticCheckValue VisitBoolValueNode(BoolValueNode node)
		{
			return node;
		}

		public ISemanticCheckValue VisitIOPrintNode(IOPrintNode node)
		{
			return node.Expression.Accept(this);
		}

		public ISemanticCheckValue VisitIOReadNode(IOReadNode node)
		{
			return node.IDNode.Accept(this);
		}

		public ISemanticCheckValue VisitRootNode(RootNode node)
		{
			return null;
		}

		public ISemanticCheckValue VisitStatementsNode(StatementsNode node)
		{
			return null;
		}

		public ISemanticCheckValue VisitStringValueNode(StringValueNode node)
		{
			return node;
		}

		public ISemanticCheckValue VisitUnOpNode(UnOpNode node)
		{
			return VisitOperationNode (node);
		}

		public ISemanticCheckValue VisitVariableIdNode(VariableIdNode node)
		{
			return analyzer.IDs[node.ID];
		}

		private IProperty VisitOperationNode (IExpressionNode node)
		{
			IProperty evaluationType = getEvaluation(node.GetExpressions());

			if (SemanticAnalysisConstants.LEGIT_OPERATIONS.ContainsKey(evaluationType.GetTokenType ()) &&
				SemanticAnalysisConstants.LEGIT_OPERATIONS [evaluationType.GetTokenType ()].ContainsKey (node.Operation)) {
				if (SemanticAnalysisConstants.LOGICAL_OPERATIONS.ContainsKey (node.Operation)) {
					return new BooleanProperty (SemanticAnalysisConstants.DEFAULT_BOOL_VALUE);
				}
				return evaluationType;
			}
			return new ErrorProperty ();
		}

		private IProperty getEvaluation(params IExpressionNode[] expressions)
		{
			IExpressionNode expression = expressions [0];
			IProperty evaluatedType = expression.Accept (this).asProperty ();
			if (evaluatedType.GetTokenType () == TokenType.ERROR) {
				expression.EvaluationType = TokenType.ERROR;
				return new ErrorProperty ();
			}

			if (expression.EvaluationType == TokenType.UNDEFINED) {
				expression.EvaluationType = evaluatedType.GetTokenType ();
			}

			for (int i = 1; i < expressions.Length; i++) {
				expression = expressions [i];
				IProperty retVal = expression.Accept (this).asProperty();
				if (retVal.GetTokenType () != evaluatedType.GetTokenType ()) {
					expression.EvaluationType = TokenType.ERROR;
					return new ErrorProperty ();
				}

				if (evaluatedType.GetTokenType () == TokenType.ERROR) {
					expression.EvaluationType = TokenType.ERROR;
					break;
				}
			}

			return evaluatedType;
		}
	}
}
