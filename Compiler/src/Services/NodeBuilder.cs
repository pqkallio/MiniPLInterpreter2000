using System;
using System.Collections.Generic;

namespace MiniPLInterpreter
{
	public class NodeBuilder
	{
		private Dictionary<string, IProperty> symbolTable;

		public NodeBuilder (Dictionary<string, IProperty> symbolTable)
		{
			this.symbolTable = symbolTable;
		}

		public RootNode CreateRootNode ()
		{
			return new RootNode ();
		}

		public StatementsNode CreateStatementsNode (Token token)
		{
			return new StatementsNode (token);
		}

		public StatementsNode CreateStatementsNode (IStatementsContainer parentNode, Token token)
		{
			StatementsNode statementsNode = new StatementsNode (token);
			parentNode.Sequitor = statementsNode;

			return statementsNode;
		}

		public VariableIdNode CreateIdNode ()
		{
			return new VariableIdNode (symbolTable);
		}

		public IExpressionNode CreateIdNode(Token t)
		{
			string value = t.Value;
			return new VariableIdNode (value, symbolTable, t);
		}

		public DeclarationNode CreateDeclarationNode (VariableIdNode idNode, StatementsNode statementsNode, Token t)
		{
			DeclarationNode declarationNode = new DeclarationNode (idNode, symbolTable, t);
			declarationNode.AssignNode = CreateAssignNode (idNode, t);
			statementsNode.Statement = declarationNode;

			return declarationNode;
		}

		public AssignNode CreateAssignNode (VariableIdNode idNode, Token t)
		{
			if (idNode.Token == null) {
				idNode.Token = t;
			}
			return new AssignNode (idNode, symbolTable, t);
		}

		public AssignNode CreateAssignNode (VariableIdNode idNode, StatementsNode statementsNode, Token t)
		{
			AssignNode assignNode = new AssignNode (idNode, symbolTable, t);
			statementsNode.Statement = assignNode;

			return assignNode;
		}

		public ForLoopNode CreateForLoopNode (VariableIdNode idNode, StatementsNode statementsNode, Token t)
		{
			ForLoopNode node = new ForLoopNode (idNode, symbolTable, t);
			statementsNode.Statement = node;

			return node;
		}

		public IOReadNode CreateIOReadNode (VariableIdNode idNode, StatementsNode statementsNode, Token t)
		{
			IOReadNode ioReadNode = new IOReadNode (idNode, symbolTable, t);
			statementsNode.Statement = ioReadNode;

			return ioReadNode;
		}

		public IOPrintNode CreateIOPrintNode (StatementsNode statementsNode, Token t)
		{
			IOPrintNode ioPrintNode = new IOPrintNode (t);
			statementsNode.Statement = ioPrintNode;

			return ioPrintNode;
		}

		public AssertNode CreateAssertNode (StatementsNode statementsNode, Token t, int assertStatementRow, int assertStatementStartCol)
		{
			AssertNode assertNode = new AssertNode (t, assertStatementRow, assertStatementStartCol);
			statementsNode.Statement = assertNode;

			return assertNode;
		}

		public BinOpNode CreateBinOpNode (IExpressionContainer parent, Token t)
		{
			BinOpNode binOp = new BinOpNode (t);
			parent.AddExpression (binOp);

			return binOp;
		}

		public UnOpNode CreateUnOpNode (IExpressionContainer parent, Token t)
		{
			UnOpNode unOp = new UnOpNode (t);
			parent.AddExpression (unOp);

			return unOp;
		}

		public IExpressionNode CreateIntValueNode(Token t)
		{
			int value = StringUtils.parseToInt (t.Value);
			return new IntValueNode (value, t);
		}

		public IExpressionNode CreateStringValueNode (Token t)
		{
			string value = t.Value;
			return new StringValueNode (value, t);
		}

		public IExpressionNode CreateBoolValueNode (Token t)
		{
			bool value = StringUtils.parseToBoolean (t.Value);
			return new BoolValueNode (value, t);
		}

		public IExpressionNode CreateDefaultIntValueNode(Token t)
		{
			return new IntValueNode (SemanticAnalysisConstants.DEFAULT_INTEGER_VALUE, t);
		}

		public IExpressionNode CreateDefaultStringValueNode (Token t)
		{
			return new StringValueNode (SemanticAnalysisConstants.DEFAULT_STRING_VALUE, t);
		}

		public IExpressionNode CreateDefaultBoolValueNode (Token t)
		{
			return new BoolValueNode (SemanticAnalysisConstants.DEFAULT_BOOL_VALUE, t);
		}
	}
}

