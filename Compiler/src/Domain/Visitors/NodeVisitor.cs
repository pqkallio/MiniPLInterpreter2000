using System;

namespace MiniPLInterpreter
{
	public interface NodeVisitor
	{
		void VisitAssertNode(AssertNode node);
		void VisitAssignNode(AssignNode node);
		void VisitBinOpNode(BinOpNode node);
		void VisitDeclarationNode(DeclarationNode node);
		void VisitForLoopNode(ForLoopNode node);
		void VisitIntValueNode(IntValueNode node);
		void VisitIOPrintNode(IOPrintNode node);
		void VisitIOReadNode(IOReadNode node);
		void VisitRootNode(RootNode node);
		void VisitStatementsNode(StatementsNode node);
		void VisitStringValueNode(StringValueNode node);
		void VisitUnOpNode(UnOpNode node);
		void VisitVariableIdNode(VariableIdNode node);
	}
}

