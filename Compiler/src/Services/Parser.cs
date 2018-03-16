using System;
using System.Collections.Generic;

namespace MiniPLInterpreter
{
	public class Parser : IErrorAggregator
	{
		private SyntaxTree tree;
		private List<Error> errors;
		private Scanner scanner;
		private bool syntaxTreeBuilt;
		private Dictionary<string, IProperty> ids;
		private NodeBuilder nodeBuilder;

		public Parser (Dictionary<string, IProperty> ids)
		{
			this.tree = new SyntaxTree ();
			this.errors = new List<Error> ();
			this.syntaxTreeBuilt = true;
			this.ids = ids;
			this.nodeBuilder = new NodeBuilder (ids);
		}

		public Scanner Scanner {
			get { return scanner; }
			set { this.scanner = value; }
		}

		public SyntaxTree SyntaxTree {
			get { return tree; }
		}

		public void Parse () {
			IStatementsContainer root = nodeBuilder.CreateRootNode ();
			tree.Root = root;
			ParseProgram (scanner.getNextToken (null), root);
		}

		private void ParseProgram (Token t, IStatementsContainer root)
		{
			switch (t.Type) {
				case TokenType.DECLARATION:
				case TokenType.FOR_LOOP:
				case TokenType.READ:
				case TokenType.PRINT:
				case TokenType.ASSERT:
				case TokenType.ID:
					Token next;
					
					try {	
						next = ParseStatements (t, root);
					} catch (UnexpectedTokenException ex) {
						notifyError(new SyntaxError(ex.Token));

						do {
							next = scanner.getNextToken (null);
						} while (next.Type != TokenType.END_OF_FILE);
					}
					
					try {
						match (next, TokenType.END_OF_FILE);
					} catch (UnexpectedTokenException ex) {
						notifyError (new SyntaxError (ex.Token));
					}
					
					break;
				case TokenType.END_OF_FILE:
					break;
				default:
					notifyError (new SyntaxError (t));
					break;
			}
		}

		private Token ParseStatements(Token t, IStatementsContainer parent)
		{
			switch (t.Type) {
				case TokenType.DECLARATION:
				case TokenType.FOR_LOOP:
				case TokenType.READ:
				case TokenType.PRINT:
				case TokenType.ASSERT:
				case TokenType.ID:
					StatementsNode statements = nodeBuilder.CreateStatementsNode (parent, t);
					Token next;
					try {
						next = ParseStatement (t, statements);
						match (next, TokenType.END_STATEMENT);
					} catch (UnexpectedTokenException ex) {
						next = FastForwardToStatementEnd (ex);
					}
					match (next, TokenType.END_STATEMENT);
					return ParseStatements (scanner.getNextToken(next), statements);
				case TokenType.END_OF_BLOCK:
				case TokenType.END_OF_FILE:
					return t;
				case TokenType.ERROR:
					next = FastForwardToStatementEnd(t);
					return ParseStatements (scanner.getNextToken(next), parent);
				default:
					throw new UnexpectedTokenException (t);
			}
		}

		private Token ParseStatement(Token t, StatementsNode statementsNode)
		{
			switch (t.Type) {
				case TokenType.DECLARATION:
					return ParseDeclaration (t, statementsNode);
				case TokenType.ID:
					return ParseVariableAssign (t, statementsNode);
				case TokenType.FOR_LOOP:
					return ParseForLoop (t, statementsNode);
				case TokenType.READ:
					return ParseRead (t, statementsNode);
				case TokenType.PRINT:
					return ParsePrint (t, statementsNode);
				case TokenType.ASSERT:
					return ParseAssert (t, statementsNode);
				case TokenType.ERROR:
					return FastForwardToStatementEnd(t);
				default:
					throw new UnexpectedTokenException (t);
			}
		}

		private Token ParseDeclaration(Token t, StatementsNode statementsNode)
		{
			try {
				VariableIdNode idNode = nodeBuilder.CreateIdNode (t);
				Token next = ParseVarId (scanner.getNextToken (t), idNode);
				match (next, TokenType.SET_TYPE);
				next = ParseType (scanner.getNextToken (next), idNode);
				DeclarationNode declarationNode = nodeBuilder.CreateDeclarationNode(idNode, statementsNode);
				return ParseAssign (next, declarationNode.AssignNode);
			} catch (UnexpectedTokenException ex) {
				return FastForwardToStatementEnd (ex);
			}
		}

		private Token ParseVariableAssign(Token t, StatementsNode statementsNode)
		{
			try {
				VariableIdNode idNode = nodeBuilder.CreateIdNode (t);
				Token next = ParseVarId (t, idNode);
				match (next, TokenType.ASSIGN);
				AssignNode assignNode = nodeBuilder.CreateAssignNode(idNode, statementsNode);
				return ParseExpression (scanner.getNextToken(next), assignNode);
			} catch (UnexpectedTokenException ex) {
				return FastForwardToStatementEnd (ex);
			}
		}

		private Token ParseForLoop(Token t, StatementsNode statementsNode)
		{
			VariableIdNode idNode = nodeBuilder.CreateIdNode (t);
			ForLoopNode forLoop = nodeBuilder.CreateForLoopNode(idNode, statementsNode);
			Token next;

			try {
				next = ParseVarId (scanner.getNextToken (t), idNode);
				match (next, TokenType.RANGE_FROM);
				AssignNode rangeFrom = nodeBuilder.CreateAssignNode(idNode);
				forLoop.RangeFrom = rangeFrom;
				next = ParseExpression (scanner.getNextToken (next), rangeFrom);
				match (next, TokenType.RANGE_UPTO);
				next = ParseExpression (scanner.getNextToken (next), forLoop);
				match (next, TokenType.START_BLOCK);
			} catch (UnexpectedTokenException ex) {
				notifyError (new SyntaxError (ex.Token));
				next = FastForwardTo (Constants.BLOCK_DEF_FASTFORWARD_TO);
			}

			try {
				StatementsNode statements = nodeBuilder.CreateStatementsNode (t);
				forLoop.Statements = statements;
				next = ParseStatements (scanner.getNextToken (next), statements);
				match (next, TokenType.END_OF_BLOCK);
				next = scanner.getNextToken (next);
				match (next, TokenType.FOR_LOOP);
				return scanner.getNextToken(next);	
			} catch (UnexpectedTokenException ex) {
				return FastForwardToStatementEnd (ex);
			}
		}

		private Token ParseRead(Token t, StatementsNode statementsNode)
		{
			try {
				VariableIdNode varId = nodeBuilder.CreateIdNode (t);
				nodeBuilder.CreateIOReadNode(varId, statementsNode);
				return ParseVarId (scanner.getNextToken(t), varId);
			} catch (UnexpectedTokenException ex) {
				return FastForwardToStatementEnd (ex);
			}
		}

		private Token ParsePrint (Token t, StatementsNode statementsNode)
		{
			try {
				IOPrintNode printNode = nodeBuilder.CreateIOPrintNode(statementsNode);
				return ParseExpression (scanner.getNextToken(t), printNode);
			} catch (UnexpectedTokenException ex) {
				return FastForwardToStatementEnd (ex);
			}
		}

		private Token ParseAssert (Token t, StatementsNode statementsNode)
		{
			try {
				Token next = scanner.getNextToken (t);
				match (next, TokenType.PARENTHESIS_LEFT);
				AssertNode assertNode = nodeBuilder.CreateAssertNode(statementsNode);
				next = ParseExpression (scanner.getNextToken (next), assertNode);
				match (next, TokenType.PARENTHESIS_RIGHT);
				return scanner.getNextToken (next);
			} catch (UnexpectedTokenException ex) {
				return FastForwardToStatementEnd (ex);
			}
		}

		private Token ParseVarId(Token t, VariableIdNode idNode)
		{
			switch (t.Type) {
				case TokenType.ID:
					idNode.ID = t.Value;
					return scanner.getNextToken (t);
				default:
					throw new UnexpectedTokenException (t);
			}
		}

		private Token ParseType (Token t, VariableIdNode idNode)
		{
			switch (t.Type) {
				case TokenType.INT_VAR:
					idNode.VariableType = TokenType.INT_VAL;
					ids.Add(idNode.ID, (new IntegerProperty(0)));
					return scanner.getNextToken (t);
				case TokenType.STR_VAR:
					idNode.VariableType = TokenType.STR_VAL;
					ids.Add(idNode.ID, new StringProperty(""));
					return scanner.getNextToken (t);
				case TokenType.BOOL_VAR:
					idNode.VariableType = TokenType.BOOL_VAL;
					ids.Add(idNode.ID, new BooleanProperty(false));
					return scanner.getNextToken (t);
				default:
					throw new UnexpectedTokenException (t);
			}
		}

		private Token ParseAssign (Token t, AssignNode assignNode)
		{
			switch (t.Type) {
				case TokenType.ASSIGN:
					Token next = scanner.getNextToken (t);
					return ParseExpression (next, assignNode);
				case TokenType.END_STATEMENT:
					setDefaultAssignment (assignNode);
					return t;
				default:
					throw new UnexpectedTokenException (t);
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
					BinOpNode binOp = nodeBuilder.CreateBinOpNode(node);
					next = ParseOperand (t, binOp);
					return ParseBinaryOp (next, binOp);
				case TokenType.UNARY_OP_LOG_NEG:
					UnOpNode unOp = nodeBuilder.CreateUnOpNode (node);
					next = ParseUnaryOp (t, unOp);
					return ParseOperand (next, unOp);
				default:
					throw new UnexpectedTokenException (t);
			}
		}

		private Token ParseOperand (Token t, IOperandContainer node)
		{
			switch (t.Type) {
				case TokenType.INT_VAL:
					ISyntaxTreeNode intVal = new IntValueNode (StringUtils.parseToInt (t.Value));
					node.AddOperand (intVal);
					return scanner.getNextToken (t);
				case TokenType.STR_VAL:
					ISyntaxTreeNode strVal = new StringValueNode (t.Value);
					node.AddOperand (strVal);
					return scanner.getNextToken (t);
				case TokenType.ID:
					ISyntaxTreeNode varId = new VariableIdNode (t.Value, ids, t);
					node.AddOperand (varId);
					return scanner.getNextToken (t);
				case TokenType.PARENTHESIS_LEFT:
					IExpressionContainer exprContainer = (IExpressionContainer)node;
					Token next = ParseExpression (scanner.getNextToken (t), exprContainer);
					match (next, TokenType.PARENTHESIS_RIGHT);
					return scanner.getNextToken (next);
				default:
					throw new UnexpectedTokenException (t);
			}
		}

		private Token ParseBinaryOp (Token t, BinOpNode binOp)
		{
			switch (t.Type) {
				case TokenType.BINARY_OP_ADD:
				case TokenType.BINARY_OP_SUB:
				case TokenType.BINARY_OP_MUL:
				case TokenType.BINARY_OP_DIV:
				case TokenType.BINARY_OP_LOG_LT:
				case TokenType.BINARY_OP_LOG_EQ:
				case TokenType.BINARY_OP_LOG_AND:
					Token next = ParseOperation (t, binOp);
					return ParseOperand (next, binOp);
				case TokenType.PARENTHESIS_RIGHT:
				case TokenType.END_STATEMENT:
				case TokenType.RANGE_UPTO:
				case TokenType.START_BLOCK:
					return t;
				default:
					throw new UnexpectedTokenException (t);
			}
		}

		private Token ParseUnaryOp (Token t, UnOpNode unOp)
		{
			switch (t.Type) {
				case TokenType.UNARY_OP_LOG_NEG:
					unOp.Operation = TokenType.UNARY_OP_LOG_NEG;
					return scanner.getNextToken (t);
				default:
					throw new UnexpectedTokenException (t);
			}
		}

		private Token ParseOperation (Token t, BinOpNode binOp)
		{
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
					throw new UnexpectedTokenException (t);
			}
		}

		private void setDefaultAssignment (AssignNode assignNode)
		{
			TokenType idType = assignNode.IDNode.GetValueType ();

			switch (idType) {
				case TokenType.STR_VAL:
					assignNode.AddExpression (new StringValueNode (""));
					break;
				case TokenType.INT_VAL:
					assignNode.AddExpression (new IntValueNode(0));
					break;
			} 
		}

		private void match(Token t, TokenType type)
		{
			if (t.Type != type) {
				syntaxTreeBuilt = false;
				throw new UnexpectedTokenException (t);
			}
		}

		private Token FastForwardTo (Dictionary<TokenType, string> tokenTypes)
		{
			syntaxTreeBuilt = false;

			Token token = scanner.getNextToken (null);

			while (!tokenTypes.ContainsKey(token.Type)) {
				token = scanner.getNextToken (token);
			}

			return token;
		}

		private Token FastForwardToStatementEnd (UnexpectedTokenException ex)
		{
			notifyError (new SyntaxError (ex.Token));
			return FastForwardTo (Constants.STATEMENT_FASTFORWARD_TO);
		}

		private Token FastForwardToStatementEnd (Token token)
		{
			notifyError (new SyntaxError (token));
			return FastForwardTo (Constants.STATEMENT_FASTFORWARD_TO);
		}

		public void notifyError (Error error)
		{
			this.errors.Add (error);
		}

		public List<Error> getErrors ()
		{
			return this.errors;
		}

		public bool SyntaxTreeBuilt
		{
			get { return syntaxTreeBuilt; }
		}
	}
}

