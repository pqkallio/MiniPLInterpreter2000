using System;
using System.Collections.Generic;

namespace MiniPLInterpreter
{
	public class TypeCheckingVisitor : NodeVisitor
	{
		private IErrorAggregator errorAggregator;
		private Dictionary<string, IProperty> ids;

		public TypeCheckingVisitor (IErrorAggregator errorAggregator, Dictionary<string, IProperty> ids)
		{
			this.errorAggregator = errorAggregator;
			this.ids = ids;
		}

		public void VisitAssertNode(AssertNode node)
		{
			IExpressionNode[] expressions = node.Expression.GetExpressions ();

			foreach (IExpressionNode expression in expressions) {
				expression.Accept (this);
			}
		}

		public void VisitAssignNode(AssignNode node) {}
		public void VisitBinOpNode(BinOpNode node) {}
		public void VisitDeclarationNode(DeclarationNode node) {}
		public void VisitForLoopNode(ForLoopNode node) {}
		public void VisitIntValueNode(IntValueNode node) {}
		public void VisitIOPrintNode(IOPrintNode node) {}
		public void VisitIOReadNode(IOReadNode node) {}
		public void VisitRootNode(RootNode node) {}
		public void VisitStatementsNode(StatementsNode node) {}
		public void VisitStringValueNode(StringValueNode node) {}
		public void VisitUnOpNode(UnOpNode node) {}
		public void VisitVariableIdNode(VariableIdNode node) {}
	}
}
