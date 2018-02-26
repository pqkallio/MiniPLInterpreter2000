using System;
using System.Collections.Generic;

namespace MiniPLInterpreter
{
	public class Parser : IErrorAggregator
	{
		private SyntaxTree tree;
		private List<Error> errors;
		private Scanner scanner;
		private bool buildSyntaxTree;
		private bool errorReportingEnabled;
		private Dictionary<string, IProperty> ids;

		public Parser ()
		{
			this.tree = new SyntaxTree ();
			this.errors = new List<Error> ();
			this.buildSyntaxTree = true;
			this.errorReportingEnabled = true;
			this.ids = new Dictionary<string, IProperty> ();
		}

		public Parser (List<Token> tokens)
		{
			this.tree = new SyntaxTree ();
			this.errors = new List<Error> ();
		}

		public Scanner Scanner {
			get { return scanner; }
			set { this.scanner = value; }
		}

		public SyntaxTree SyntaxTree {
			get { return tree; }
		}

		public void Parse () {
			ISyntaxTreeNode root = new RootNode ();
			tree.Root = root;
			ParseProgram (scanner.getNextToken (null), root);
		}

		private void ParseProgram (Token t, ISyntaxTreeNode root)
		{
			switch (t.Type) {
				case TokenType.DECLARATION:
				case TokenType.FOR_LOOP:
				case TokenType.READ:
				case TokenType.PRINT:
				case TokenType.ASSERT:
				case TokenType.ID:
					Token next = ParseStatements (t, root);
					match (next, TokenType.END_OF_FILE);
					break;
				case TokenType.END_OF_FILE:
					break;
				default:
					if (errorReportingEnabled) notifyError (new SyntaxError (t));
					break;
			}
		}

		private Token ParseStatements(Token t, ISyntaxTreeNode parent)
		{
			switch (t.Type) {
				case TokenType.DECLARATION:
				case TokenType.FOR_LOOP:
				case TokenType.READ:
				case TokenType.PRINT:
				case TokenType.ASSERT:
				case TokenType.ID:
				ISyntaxTreeNode statements = new StatementsNode ();
					if (parent == tree.Root) {
						RootNode rn = (RootNode)parent;
						rn.Sequitor = statements;
					} else {
						StatementsNode sn = (StatementsNode)parent;
						sn.Sequitor = statements;
					}
					Token next = ParseStatement (t, statements);
					match (next, TokenType.END_STATEMENT);
					// parent.AddChild (child);
					this.errorReportingEnabled = true;
					return ParseStatements (scanner.getNextToken(next), statements);
				case TokenType.END_OF_BLOCK:
				case TokenType.END_OF_FILE:
					return t;
				default:
					if (errorReportingEnabled) notifyError (new SyntaxError (t));
					return t;
			}
		}

		private Token ParseStatement(Token t, ISyntaxTreeNode statementsNode)
		{
			Token next;
			StatementsNode sn = (StatementsNode)statementsNode;
			switch (t.Type) {
				case TokenType.DECLARATION:
					VariableIdNode idNode = new VariableIdNode (ids);
					next = ParseVarId (scanner.getNextToken (t), idNode);
					match (next, TokenType.SET_TYPE);
					next = ParseType (scanner.getNextToken (next), idNode);
					ISyntaxTreeNode assignNode = new AssignNode ((VariableIdNode)idNode, ids);
					sn.Statement = assignNode;
					return ParseAssign (next, assignNode);
				case TokenType.ID:
					idNode = new VariableIdNode (ids);
					next = ParseVarId (t, idNode);
					match (next, TokenType.ASSIGN);
					assignNode = new AssignNode ((VariableIdNode)idNode, ids);
					sn.Statement = assignNode;
					return ParseExpression (scanner.getNextToken(next), (IExpressionContainer)assignNode);
				case TokenType.FOR_LOOP:
					idNode = new VariableIdNode (ids);
					next = ParseVarId (scanner.getNextToken (t), idNode);
					ForLoopNode forLoop = new ForLoopNode (idNode);
					match (next, TokenType.RANGE_FROM);
					AssignNode rangeFrom = new AssignNode (idNode, ids);
					forLoop.RangeFrom = rangeFrom;
					next = ParseExpression (scanner.getNextToken (next), rangeFrom);
					match (next, TokenType.RANGE_UPTO);
					next = ParseExpression (scanner.getNextToken (next), forLoop);
					match (next, TokenType.START_BLOCK);
					StatementsNode statements = new StatementsNode ();
					forLoop.Statements = statements;
					next = ParseStatements (scanner.getNextToken (next), statements);
					match (next, TokenType.END_OF_BLOCK);
					next = scanner.getNextToken (next);
					match (next, TokenType.FOR_LOOP);
					sn.Statement = forLoop;
					return scanner.getNextToken(next);
				case TokenType.READ:
					VariableIdNode varId = new VariableIdNode (ids);
					IOReadNode readNode = new IOReadNode (varId, ids);
					sn.Statement = readNode;
					return ParseVarId (scanner.getNextToken(t), varId);
				case TokenType.PRINT:
					IOPrintNode printNode = new IOPrintNode ();
					sn.Statement = printNode;
					return ParseExpression (scanner.getNextToken(t), printNode);
				case TokenType.ASSERT:
					next = scanner.getNextToken (t);
					match (next, TokenType.PARENTHESIS_LEFT);
					AssertNode assertNode = new AssertNode ();
					next = ParseExpression (scanner.getNextToken (next), assertNode);
					match (next, TokenType.PARENTHESIS_RIGHT);
					sn.Statement = assertNode;
					return scanner.getNextToken (next);
				default:
					if (errorReportingEnabled) notifyError (new SyntaxError (t));
					return t;
			}
		}

		private Token ParseVarId(Token t, ISyntaxTreeNode idNode)
		{
			switch (t.Type) {
				case TokenType.ID:
					VariableIdNode id = (VariableIdNode)idNode;
					id.ID = t.Value;
					return scanner.getNextToken (t);
				default:
					if (errorReportingEnabled) notifyError (new SyntaxError (t));
					return t;
			}
		}

		private Token ParseType (Token t, ISyntaxTreeNode idNode)
		{
			VariableIdNode id = (VariableIdNode)idNode;
			switch (t.Type) {
				case TokenType.INT_VAR:
					ids.Add(id.ID, (new IntegerProperty(0)));
					return scanner.getNextToken (t);
				case TokenType.STR_VAR:
					ids.Add(id.ID, new StringProperty(""));
					return scanner.getNextToken (t);
				case TokenType.BOOL_VAR:
					ids.Add(id.ID, new BooleanProperty(false));
					return scanner.getNextToken (t);
				default:
					notifyUndefinedToken(t, "type");
					return t;
			}
		}

		private Token ParseAssign (Token t, ISyntaxTreeNode assignNode)
		{
			switch (t.Type) {
			case TokenType.ASSIGN:
					Token next = scanner.getNextToken (t);
					return ParseExpression (next, (IExpressionContainer)assignNode);
				case TokenType.END_STATEMENT:
					return t;
				default:
					notifyUndefinedToken(t, "assign or end of statement");
					return t;
			}
		}

		private Token ParseExpression (Token t, IExpressionContainer node)
		{
			Token next;
			switch (t.Type) {
				case TokenType.INT_VAL:
				case TokenType.STR_VAL:
				case TokenType.PARENTHESIS_LEFT:
				case TokenType.ID:
					ISyntaxTreeNode binOp = new BinOpNode ();
					node.AddExpression(binOp);
					next = ParseOperand (t, binOp);
					return ParseBinaryOp (next, binOp);
				case TokenType.UNARY_OP_LOG_NEG:
				ISyntaxTreeNode unOp = new UnOpNode ();
					node.AddExpression (unOp);
					next = ParseUnaryOp (t, unOp);
					return ParseOperand (next, unOp);
				default:
					if (errorReportingEnabled) notifyError (new SyntaxError (t));
					return t;
			}
		}

		private Token ParseOperand (Token t, ISyntaxTreeNode node)
		{
			IOperandContainer parent = (IOperandContainer)node;
			switch (t.Type) {
				case TokenType.INT_VAL:
					ISyntaxTreeNode intVal = new IntValueNode (StringUtils.parseToInt (t.Value));
					parent = (BinOpNode)node;
					parent.AddOperand (intVal);
					return scanner.getNextToken (t);
				case TokenType.STR_VAL:
					ISyntaxTreeNode strVal = new StringValueNode (t.Value);
					parent = (BinOpNode)node;
					parent.AddOperand (strVal);
					return scanner.getNextToken (t);
				case TokenType.ID:
					ISyntaxTreeNode varId = new VariableIdNode (t.Value, ids);
					parent = (BinOpNode)node;
					parent.AddOperand (varId);
					return scanner.getNextToken (t);
				case TokenType.PARENTHESIS_LEFT:
					IExpressionContainer exprContainer = (IExpressionContainer)node;
					Token next = ParseExpression (scanner.getNextToken (t), exprContainer);
					match (next, TokenType.PARENTHESIS_RIGHT);
					return scanner.getNextToken (next);
				default:
					if (errorReportingEnabled) notifyError (new SyntaxError (t));
					return t;
			}
		}

		private Token ParseBinaryOp (Token t, ISyntaxTreeNode node)
		{
			switch (t.Type) {
				case TokenType.BINARY_OP_ADD:
				case TokenType.BINARY_OP_SUB:
				case TokenType.BINARY_OP_MUL:
				case TokenType.BINARY_OP_DIV:
				case TokenType.BINARY_OP_LOG_LT:
				case TokenType.BINARY_OP_LOG_EQ:
				case TokenType.BINARY_OP_LOG_AND:
					Token next = ParseOperation (t, node);
					return ParseOperand (next, node);
				case TokenType.PARENTHESIS_RIGHT:
				case TokenType.END_STATEMENT:
				case TokenType.RANGE_UPTO:
				case TokenType.START_BLOCK:
					return t;
				default:
					if (errorReportingEnabled) notifyError (new SyntaxError (t));
					return t;
			}
		}

		private Token ParseUnaryOp (Token t, ISyntaxTreeNode node)
		{
			switch (t.Type) {
				case TokenType.UNARY_OP_LOG_NEG:
					UnOpNode unOp = (UnOpNode)node;
					unOp.Operation = TokenType.UNARY_OP_LOG_NEG;
					return scanner.getNextToken (t);
				default:
					if (errorReportingEnabled) notifyError (new SyntaxError (t));
					return t;
			}
		}

		private Token ParseOperation (Token t, ISyntaxTreeNode node)
		{
			BinOpNode binOp = (BinOpNode)node;
			switch (t.Type) {
				case TokenType.BINARY_OP_ADD:
					binOp.Operation = TokenType.BINARY_OP_ADD;
					return scanner.getNextToken (t);
				case TokenType.BINARY_OP_DIV:
					binOp.Operation = TokenType.BINARY_OP_DIV;
					return scanner.getNextToken (t);
				case TokenType.BINARY_OP_LOG_AND:
					binOp.Operation = TokenType.BINARY_OP_LOG_AND;
					return scanner.getNextToken (t);
				case TokenType.BINARY_OP_LOG_EQ:
					binOp.Operation = TokenType.BINARY_OP_LOG_EQ;	
					return scanner.getNextToken (t);
				case TokenType.BINARY_OP_LOG_LT:
					binOp.Operation = TokenType.BINARY_OP_LOG_LT;
					return scanner.getNextToken (t);
				case TokenType.BINARY_OP_MUL:
					binOp.Operation = TokenType.BINARY_OP_MUL;
					return scanner.getNextToken (t);
				case TokenType.BINARY_OP_SUB:
					binOp.Operation = TokenType.BINARY_OP_SUB;
					return scanner.getNextToken (t);
				default:
					if (errorReportingEnabled) notifyError (new SyntaxError (t));
					return t;
			}
		}

		private bool match(Token t, TokenType type)
		{
			if (t.Type != type && errorReportingEnabled) {
				notifyError (new SyntaxError (t, type));
				errorReportingEnabled = false;
				return false;
			}

			return true;
		}

		private void notifyUndefinedToken(Token t, string type) {
			if (errorReportingEnabled) {
				notifyError (new SyntaxError (t, type));
				errorReportingEnabled = false;
			}
		}

		public void notifyError (Error error)
		{
			this.errors.Add (error);
		}

		public List<Error> getErrors ()
		{
			return this.errors;
		}
	}
}

