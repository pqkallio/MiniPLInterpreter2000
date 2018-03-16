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
			return getEvaluation (node.Expression.GetExpressions ());
		}

		public ISemanticCheckValue VisitAssignNode(AssignNode node)
		{
			Console.WriteLine (node.ExprNode);
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

			if (!Constants.LEGIT_OPERATIONS.ContainsKey(rangeFrom.GetTokenType ()) ||
				!Constants.LEGIT_OPERATIONS [rangeFrom.GetTokenType ()].ContainsKey (TokenType.RANGE_FROM)) {
				analyzer.notifyError(new IllegalTypeError(node.RangeFrom));
				alright = false;
			}

			if (!Constants.LEGIT_OPERATIONS.ContainsKey(max.GetTokenType ()) ||
				!Constants.LEGIT_OPERATIONS [max.GetTokenType ()].ContainsKey (TokenType.RANGE_UPTO)) {
				analyzer.notifyError(new IllegalTypeError(node.MaxValue));
				alright = false;
			}

			if (!alright) {
				return new MismatchProperty ();
			}

			return rangeFrom;
		}

		public ISemanticCheckValue VisitIntValueNode(IntValueNode node)
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

		public IProperty VisitOperationNode (IExpressionNode node)
		{
			IProperty evaluationType = getEvaluation(node.GetExpressions());

			if (Constants.LEGIT_OPERATIONS.ContainsKey(evaluationType.GetTokenType ()) &&
				Constants.LEGIT_OPERATIONS [evaluationType.GetTokenType ()].ContainsKey (node.Operation)) {
				return evaluationType;
			}

			return new MismatchProperty ();
		}

		private IProperty getEvaluation(params IExpressionNode[] expressions)
		{
			IProperty evaluatedType = expressions [0].Accept (this).asProperty ();
			Console.WriteLine ("first evaluation: " + evaluatedType);
			for (int i = 1; i < expressions.Length; i++) {
				IExpressionNode expression = expressions [i];
				Console.WriteLine ("expression: " + expression);
				IProperty retVal = expression.Accept (this).asProperty();
				Console.WriteLine ("retVal: " + retVal);
				if (retVal.GetTokenType () != evaluatedType.GetTokenType ()) {
					Console.WriteLine ("Oh shit!");
					return new MismatchProperty ();
				}

				if (evaluatedType.GetTokenType () == TokenType.ERROR) {
					break;
				}
			}

			return evaluatedType;
		}
	}
}
