using System;
using System.Collections.Generic;

namespace MiniPLInterpreter
{
	public class Parser : IErrorAggregator
	{
		private SyntaxTree syntaxTree;
		private List<Error> errors;
		private Scanner scanner;
		private bool syntaxTreeBuilt;
		private Dictionary<string, IProperty> symbolTable;
		private NodeBuilder nodeBuilder;

		public Parser(Dictionary<string, IProperty> symbolTable)
			: this(symbolTable, null)
		{}

		public Parser (Dictionary<string, IProperty> symbolTable, Scanner scanner)
		{
			this.syntaxTree = new SyntaxTree ();
			this.errors = new List<Error> ();
			this.syntaxTreeBuilt = false;
			this.symbolTable = symbolTable;
			this.nodeBuilder = new NodeBuilder (symbolTable);
			this.scanner = scanner;
		}

		public Scanner Scanner {
			get { return scanner; }
			set { this.scanner = value; }
		}

		public SyntaxTree SyntaxTree {
			get { return syntaxTree; }
		}

		public SyntaxTree Parse () {
			syntaxTreeBuilt = true;
			IStatementsContainer root = nodeBuilder.CreateRootNode ();
			syntaxTree.Root = root;
			ParseProgram (scanner.getNextToken (null), root);

			if (!syntaxTreeBuilt) {
				return null;
			}

			return syntaxTree;
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
						notifyError(new SyntaxError(ex.Token, ex.ExpectedType, ex.ExpectationSet));

						do {
							next = scanner.getNextToken (null);
						} while (next.Type != TokenType.END_OF_FILE);
					}
					
					try {
						match (next, TokenType.END_OF_FILE);
					} catch (UnexpectedTokenException ex) {
						notifyError (new SyntaxError (ex.Token, ex.ExpectedType, null));
					}
					
					break;
				case TokenType.END_OF_FILE:
					break;
				default:
					notifyError (new SyntaxError (t, TokenType.UNDEFINED, ParserConstants.EXPECTATION_SET_PROGRAM));
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
					} catch (UnexpectedTokenException ex) {
						next = FastForwardToStatementEnd (ex);
					}
					match (next, TokenType.END_STATEMENT);
					return ParseStatements (scanner.getNextToken(next), statements);
				case TokenType.END_OF_BLOCK:
				case TokenType.END_OF_FILE:
					return t;
				case TokenType.ERROR:
					notifyError (new SyntaxError (t, TokenType.UNDEFINED, ParserConstants.EXPECTATION_SET_STATEMENTS));
					next = FastForwardToStatementEnd(t);
					return ParseStatements (scanner.getNextToken(next), parent);
				default:
					throw new UnexpectedTokenException (t, TokenType.UNDEFINED, ParserConstants.EXPECTATION_SET_STATEMENTS);
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
					notifyError(new SyntaxError (t, TokenType.UNDEFINED, ParserConstants.EXPECTATION_SET_STATEMENT));
					return FastForwardToStatementEnd(t);
				default:
					throw new UnexpectedTokenException (t, TokenType.UNDEFINED, ParserConstants.EXPECTATION_SET_STATEMENT);
			}
		}

		private Token ParseDeclaration(Token t, StatementsNode statementsNode)
		{
			try {
				VariableIdNode idNode = nodeBuilder.CreateIdNode ();
				Token next = ParseVarId (scanner.getNextToken (t), idNode);
				match (next, TokenType.SET_TYPE);
				next = ParseType (scanner.getNextToken (next), idNode);
				DeclarationNode declarationNode = nodeBuilder.CreateDeclarationNode(idNode, statementsNode, t);
				return ParseAssign (next, declarationNode.AssignNode);
			} catch (UnexpectedTokenException ex) {
				return FastForwardToStatementEnd (ex);
			}
		}

		private Token ParseVariableAssign(Token t, StatementsNode statementsNode)
		{
			try {
				VariableIdNode idNode = nodeBuilder.CreateIdNode ();
				Token next = ParseVarId (t, idNode);
				match (next, TokenType.ASSIGN);
				AssignNode assignNode = nodeBuilder.CreateAssignNode(idNode, statementsNode, t);
				return ParseExpression (scanner.getNextToken(next), assignNode);
			} catch (UnexpectedTokenException ex) {
				return FastForwardToStatementEnd (ex);
			}
		}

		private Token ParseForLoop(Token t, StatementsNode statementsNode)
		{
			VariableIdNode idNode = nodeBuilder.CreateIdNode ();
			ForLoopNode forLoop = nodeBuilder.CreateForLoopNode(idNode, statementsNode, t);
			Token next;

			try {
				next = ParseVarId (scanner.getNextToken (t), idNode);
				match (next, TokenType.RANGE_FROM);
				AssignNode rangeFrom = nodeBuilder.CreateAssignNode(idNode, next);
				forLoop.RangeFrom = rangeFrom;
				next = ParseExpression (scanner.getNextToken (next), rangeFrom);
				match (next, TokenType.RANGE_UPTO);
				next = ParseExpression (scanner.getNextToken (next), forLoop);
				match (next, TokenType.START_BLOCK);
			} catch (UnexpectedTokenException ex) {
				notifyError (new SyntaxError (ex.Token, ex.ExpectedType, ex.ExpectationSet));
				next = FastForwardTo (ParserConstants.BLOCK_DEF_FASTFORWARD_TO, ex.Token);
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
				VariableIdNode varId = nodeBuilder.CreateIdNode ();
				nodeBuilder.CreateIOReadNode(varId, statementsNode, t);
				return ParseVarId (scanner.getNextToken(t), varId);
			} catch (UnexpectedTokenException ex) {
				return FastForwardToStatementEnd (ex);
			}
		}

		private Token ParsePrint (Token t, StatementsNode statementsNode)
		{
			try {
				IOPrintNode printNode = nodeBuilder.CreateIOPrintNode(statementsNode, t);
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
				AssertNode assertNode = nodeBuilder.CreateAssertNode(statementsNode, t, next.Row, next.Column + 1);
				next = ParseExpression (scanner.getNextToken (next), assertNode);
				match (next, TokenType.PARENTHESIS_RIGHT);
				assertNode.IOPrintNode = new IOPrintNode (assertNode.Expression.Token);
				assertNode.IOPrintNode.AddExpression (new StringValueNode("Assertion failed: " + assertNode.Expression.ToString()  + "\n"));
				assertNode.AssertStatementEndCol = next.Column;
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
					idNode.Token = t;
					return scanner.getNextToken (t);
				default:
					throw new UnexpectedTokenException (t, TokenType.ID, null);
			}
		}

		private Token ParseType (Token t, VariableIdNode idNode)
		{
			if (!symbolTable.ContainsKey (idNode.ID)) {
				switch (t.Type) {
					case TokenType.INT_VAR:
						idNode.VariableType = TokenType.INT_VAL;
						symbolTable.Add (idNode.ID, (new IntegerProperty (SemanticAnalysisConstants.DEFAULT_INTEGER_VALUE)));
						break;
					case TokenType.STR_VAR:
						idNode.VariableType = TokenType.STR_VAL;
						symbolTable.Add (idNode.ID, new StringProperty (SemanticAnalysisConstants.DEFAULT_STRING_VALUE));
						break;
					case TokenType.BOOL_VAR:
						idNode.VariableType = TokenType.BOOL_VAL;
						symbolTable.Add (idNode.ID, new BooleanProperty (SemanticAnalysisConstants.DEFAULT_BOOL_VALUE));
						break;
					default:
						throw new UnexpectedTokenException (t, TokenType.UNDEFINED, ParserConstants.EXPECTATION_SET_DECLARATION_TYPE);
				}
			}

			return scanner.getNextToken (t);
		}

		private Token ParseAssign (Token t, AssignNode assignNode)
		{
			switch (t.Type) {
				case TokenType.ASSIGN:
					assignNode.Token = t;
					Token next = scanner.getNextToken (t);
					return ParseExpression (next, assignNode);
				case TokenType.END_STATEMENT:
					setDefaultAssignment (assignNode);
					return t;
				default:
					throw new UnexpectedTokenException (t, TokenType.UNDEFINED, ParserConstants.EXPECTATION_SET_ASSIGN);
			}
		}

		private Token ParseExpression (Token t, IExpressionContainer node)
		{
			Token next;
			switch (t.Type) {
				case TokenType.INT_VAL:
				case TokenType.STR_VAL:
				case TokenType.BOOL_VAL:
				case TokenType.PARENTHESIS_LEFT:
				case TokenType.ID:
					BinOpNode binOp = nodeBuilder.CreateBinOpNode(node, t);
					next = ParseOperand (t, binOp);
					return ParseBinaryOp (next, binOp);
				case TokenType.UNARY_OP_LOG_NEG:
					UnOpNode unOp = nodeBuilder.CreateUnOpNode (node, t);
					next = ParseUnaryOp (t, unOp);
					return ParseOperand (next, unOp);
				default:
					throw new UnexpectedTokenException (t, TokenType.UNDEFINED, ParserConstants.EXPECTATION_SET_EXPRESSION);
			}
		}

		private Token ParseOperand (Token t, IOperandContainer node)
		{
			switch (t.Type) {
				case TokenType.INT_VAL:
					ISyntaxTreeNode intVal;
					try {
						intVal = nodeBuilder.CreateIntValueNode(t);
					} catch (OverflowException) {
						notifyError(new IntegerOverflowError(t));
						intVal = nodeBuilder.CreateDefaultIntValueNode (t);
					}
					node.AddOperand (intVal);
					return scanner.getNextToken (t);
				case TokenType.STR_VAL:
					ISyntaxTreeNode strVal = nodeBuilder.CreateStringValueNode(t);
					node.AddOperand (strVal);
					return scanner.getNextToken (t);
				case TokenType.BOOL_VAL:
					ISyntaxTreeNode boolVal = nodeBuilder.CreateBoolValueNode(t);
					node.AddOperand (boolVal);
					return scanner.getNextToken (t);
				case TokenType.ID:
					ISyntaxTreeNode varId = nodeBuilder.CreateIdNode(t);
					node.AddOperand (varId);
					return scanner.getNextToken (t);
				case TokenType.PARENTHESIS_LEFT:
					IExpressionContainer exprContainer = (IExpressionContainer)node;
					Token next = ParseExpression (scanner.getNextToken (t), exprContainer);
					match (next, TokenType.PARENTHESIS_RIGHT);
					return scanner.getNextToken (next);
				default:
					throw new UnexpectedTokenException (t, TokenType.UNDEFINED, ParserConstants.EXPECTATION_SET_EXPRESSION);
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
					throw new UnexpectedTokenException (t, TokenType.UNDEFINED, ParserConstants.EXPECTATION_SET_BINOP);
			}
		}

		private Token ParseUnaryOp (Token t, UnOpNode unOp)
		{
			switch (t.Type) {
				case TokenType.UNARY_OP_LOG_NEG:
					unOp.Operation = TokenType.UNARY_OP_LOG_NEG;
					unOp.Token = t;
					return scanner.getNextToken (t);
				default:
					throw new UnexpectedTokenException (t, TokenType.UNARY_OP_LOG_NEG, null);
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
					throw new UnexpectedTokenException (t, TokenType.UNDEFINED, null);
			}
		}

		private void setDefaultAssignment (AssignNode assignNode)
		{
			TokenType idType = assignNode.IDNode.EvaluationType;

			switch (idType) {
				case TokenType.STR_VAL:
					assignNode.AddExpression (nodeBuilder.CreateDefaultStringValueNode(assignNode.Token));
					break;
				case TokenType.INT_VAL:
					assignNode.AddExpression (nodeBuilder.CreateDefaultIntValueNode(assignNode.Token));
					break;
				case TokenType.BOOL_VAL:
					assignNode.AddExpression (nodeBuilder.CreateDefaultBoolValueNode (assignNode.Token));
					break;
				default:
					throw new UnexpectedTokenException (assignNode.IDNode.Token, TokenType.UNDEFINED, ParserConstants.EXPECTATION_SET_ID_VAL);
			} 
		}

		private void match(Token t, TokenType expectedType)
		{
			if (t.Type != expectedType) {
				syntaxTreeBuilt = false;
				throw new UnexpectedTokenException (t, expectedType, null);
			}
		}

		private Token FastForwardTo (Dictionary<TokenType, string> tokenTypes, Token errorToken)
		{
			syntaxTreeBuilt = false;

			Token token = errorToken;

			while (!tokenTypes.ContainsKey(token.Type)) {
				token = scanner.getNextToken (token);
			}

			return token;
		}

		private Token FastForwardToStatementEnd (UnexpectedTokenException ex)
		{
			notifyError (new SyntaxError (ex.Token, ex.ExpectedType, ex.ExpectationSet));

			return FastForwardTo (ParserConstants.STATEMENT_FASTFORWARD_TO, ex.Token);
		}

		private Token FastForwardToStatementEnd (Token token)
		{
			return FastForwardTo (ParserConstants.STATEMENT_FASTFORWARD_TO, token);
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

