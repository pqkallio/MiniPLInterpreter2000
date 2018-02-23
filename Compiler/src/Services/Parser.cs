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
				case TokenType.WRITE:
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
				case TokenType.WRITE:
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

		private Token ParseStatement(Token t, ISyntaxTreeNode parent)
		{
			Token next;
			switch (t.Type) {
				case TokenType.DECLARATION:
					ISyntaxTreeNode idNode = new VariableIdNode (ids);
					next = ParseVarId (scanner.getNextToken (t), idNode);
					match (next, TokenType.SET_TYPE);
					next = ParseType (scanner.getNextToken (next), idNode);
					return ParseAssign (next, idNode);
				case TokenType.ID:
					next = ParseVarId (t, null);
					match (next, TokenType.ASSIGN);
					return ParseExpression (scanner.getNextToken(next), null);
				case TokenType.FOR_LOOP:
					next = ParseVarId (scanner.getNextToken(t), null);
					match (next, TokenType.RANGE_FROM);
					next = ParseExpression (scanner.getNextToken(next), null);
					match (next, TokenType.RANGE_UPTO);
					next = ParseExpression (scanner.getNextToken(next), null);
					match (next, TokenType.START_BLOCK);
					next = ParseStatements (scanner.getNextToken(next), null);
					match (next, TokenType.END_OF_BLOCK);
					next = scanner.getNextToken(next);
					match (next, TokenType.FOR_LOOP);
					return scanner.getNextToken(next);
				case TokenType.READ:
					return ParseVarId (scanner.getNextToken(t), null);
				case TokenType.WRITE:
					return ParseExpression (scanner.getNextToken(t), null);
				case TokenType.ASSERT:
					next = scanner.getNextToken(t);
					match (next, TokenType.PARENTHESIS_LEFT);
					next = ParseExpression (scanner.getNextToken (next), null);
					match (next, TokenType.PARENTHESIS_RIGHT);
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

		private Token ParseAssign (Token t, ISyntaxTreeNode idNode)
		{
			switch (t.Type) {
			case TokenType.ASSIGN:
					ISyntaxTreeNode assignNode = new AssignNode ((VariableIdNode)idNode, ids);
					Token next = scanner.getNextToken (t);
					return ParseExpression (next, assignNode);
				case TokenType.END_STATEMENT:
					return t;
				default:
					notifyUndefinedToken(t, "assign or end of statement");
					return t;
			}
		}

		private Token ParseExpression (Token t, ISyntaxTreeNode node)
		{
			Token next;
			switch (t.Type) {
				case TokenType.INT_VAL:
				case TokenType.STR_VAL:
				case TokenType.PARENTHESIS_LEFT:
				case TokenType.ID:
					ISyntaxTreeNode binOp = new BinOpNode ();
					IExpressionContainer containerNode = (IExpressionContainer)node;
					containerNode.AddExpression(binOp);
					next = ParseOperand (t, binOp);
					return ParseBinaryOp (next, binOp);
				case TokenType.UNARY_OP_LOG_NEG:
					next = ParseUnaryOp (t);
					return ParseOperand (next, null);
				default:
					if (errorReportingEnabled) notifyError (new SyntaxError (t));
					return t;
			}
		}

		private Token ParseOperand (Token t, ISyntaxTreeNode node)
		{
			switch (t.Type) {
				case TokenType.INT_VAL:
					ISyntaxTreeNode intVal = new IntValueNode (StringUtils.parseToInt (t.Value));
					BinOpNode binOp = (BinOpNode)node;
					binOp.AddOperand (intVal);
					return scanner.getNextToken (t);
				case TokenType.STR_VAL:
					ISyntaxTreeNode strVal = new StringValueNode (t.Value);
					binOp = (BinOpNode)node;
					binOp.AddOperand (strVal);
					return scanner.getNextToken (t);
				case TokenType.ID:
					ISyntaxTreeNode varId = new VariableIdNode (t.Value, ids);
					binOp = (BinOpNode)node;
					binOp.AddOperand (varId);
					return scanner.getNextToken (t);
				case TokenType.PARENTHESIS_LEFT:
					Token next = ParseExpression (scanner.getNextToken (t), null);
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

		private Token ParseUnaryOp (Token t)
		{
			switch (t.Type) {
				case TokenType.UNARY_OP_LOG_NEG:
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

