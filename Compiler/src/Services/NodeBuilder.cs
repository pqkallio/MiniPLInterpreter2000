using System;
using System.Collections.Generic;

namespace MiniPLInterpreter
{
	public class NodeBuilder
	{
		private Dictionary<string, IProperty> ids;

		public NodeBuilder (Dictionary<string, IProperty> ids)
		{
			this.ids = ids;
		}

		public RootNode CreateRootNode ()
		{
			return new RootNode ();
		}

		public StatementsNode CreateStatementsNode ()
		{
			return new StatementsNode ();
		}

		public StatementsNode CreateStatementsNode (IStatementsContainer parentNode)
		{
			StatementsNode statementsNode = new StatementsNode ();
			parentNode.Sequitor = statementsNode;

			return statementsNode;
		}

		public VariableIdNode CreateIdNode ()
		{
			return new VariableIdNode (ids);
		}

		public DeclarationNode CreateDeclarationNode (VariableIdNode idNode, StatementsNode statementsNode)
		{
			DeclarationNode declarationNode = new DeclarationNode (idNode, ids);
			declarationNode.AssignNode = CreateAssignNode (idNode);
			statementsNode.Statement = declarationNode;

			return declarationNode;
		}

		public AssignNode CreateAssignNode (VariableIdNode idNode)
		{
			return new AssignNode (idNode, ids);
		}

		public AssignNode CreateAssignNode (VariableIdNode idNode, StatementsNode statementsNode)
		{
			AssignNode assignNode = new AssignNode (idNode, ids);
			statementsNode.Statement = assignNode;

			return assignNode;
		}

		public ForLoopNode CreateForLoopNode (VariableIdNode idNode, StatementsNode statementsNode)
		{
			ForLoopNode node = new ForLoopNode (idNode);
			statementsNode.Statement = node;

			return node;
		}

		public IOReadNode CreateIOReadNode (VariableIdNode idNode, StatementsNode statementsNode)
		{
			IOReadNode ioReadNode = new IOReadNode (idNode, ids);
			statementsNode.Statement = ioReadNode;

			return ioReadNode;
		}

		public IOPrintNode CreateIOPrintNode (StatementsNode statementsNode)
		{
			IOPrintNode ioPrintNode = new IOPrintNode ();
			statementsNode.Statement = ioPrintNode;

			return ioPrintNode;
		}

		public AssertNode CreateAssertNode (StatementsNode statementsNode)
		{
			AssertNode assertNode = new AssertNode ();
			statementsNode.Statement = assertNode;

			return assertNode;
		}

		public BinOpNode CreateBinOpNode (IExpressionContainer parent)
		{
			BinOpNode binOp = new BinOpNode ();
			parent.AddExpression (binOp);

			return binOp;
		}

		public UnOpNode CreateUnOpNode (IExpressionContainer parent)
		{
			UnOpNode unOp = new UnOpNode ();
			parent.AddExpression (unOp);

			return unOp;
		}
	}
}

