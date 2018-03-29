using System;
using System.Collections.Generic;

namespace MiniPLInterpreter
{
	/// <summary>
	/// A recursive-descent parser for Mini-PL programming language
	/// Performs the syntactical analysis on a source code.
	/// Also, builds an AST and a global symbol table for the
	/// semantical analyzer.
	/// </summary>
	public class Parser : IErrorAggregator
	{
		private SyntaxTree syntaxTree;						// the AST to be built
		private List<Error> errors;							// a list of errors encountered while parsing
		private Scanner scanner;							// we ask tokens from the scanner
		private bool syntaxTreeBuilt;						// true if no errors were encountered, false otherwise
		private Dictionary<string, IProperty> symbolTable;	// the universal symbol table for the compiler frontend
		private NodeBuilder nodeBuilder;					// we ask ST-nodes from the NodeBuilder

		public Parser(Dictionary<string, IProperty> symbolTable)
			: this(symbolTable, null)
		{}

		/// <summary>
		/// Initializes a new instance of the <see cref="MiniPLInterpreter.Parser"/> class.
		/// </summary>
		/// <param name="symbolTable">The universal symbol table provided by the compiler frontend</param>
		/// <param name="scanner">The scanner that works for this parser</param>
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

		/// <summary>
		/// This method is called when we want the parsing to be done.
		/// The AST is returned when finished
		/// <returns>The AST as a SyntaxTree object</returns>
		/// </summary>
		public SyntaxTree Parse () {
			// make the preparations first
			syntaxTreeBuilt = true;
			IStatementsContainer root = nodeBuilder.CreateRootNode ();
			syntaxTree.Root = root;

			// then start parsing by asking for the first token
			ParseProgram (scanner.getNextToken (null), root);

			// if there were errors during parsing, don't return the AST
			if (!syntaxTreeBuilt) {
				return null;
			}

			return syntaxTree;
		}

		/// <summary>
		/// Starting point for the recursive descent parsing.
		/// </summary>
		/// <param name="token">Token from the scanner</param>
		/// <param name="root">The root node of the AST</param>
		private void ParseProgram (Token token, IStatementsContainer root)
		{
			// All the parsing methods use switch statements to decide the actions
			// based on the current tokens type
			switch (token.Type) {
				case TokenType.DECLARATION:					// if it's one of the different statement types
				case TokenType.FOR_LOOP:
				case TokenType.READ:
				case TokenType.PRINT:
				case TokenType.ASSERT:
				case TokenType.ID:
					Token next;
					
					try {	
						next = ParseStatements (token, root);	// try to parse statements
					} catch (UnexpectedTokenException ex) {
						/* The exception handling in the parser is handled using exceptions:
						
						   If a parsing method encounters a token it isn't expecting, it throws a
						   UnexpectedTokenException, which is then catched by some of the methods
						   lower in the call stack. This method then reports the error and finds
						   a safe spot where the parsing can continue, by asking new tokens
						   from the scanner and discarding them until it finds a token it can cope with.

						   This here being the very first parsing method, it means that if the error thrown
						   higher in the stack is not catched before this point, the next safe point
						   is the end of file. */
						next = FastForwardToSourceEnd (ex);
					}
					
					try {
						match (next, TokenType.END_OF_FILE);	// we are excpecting the token to be end of file
					} catch (UnexpectedTokenException ex) {
						notifyError (new SyntaxError (ex.Token, ex.ExpectedType, null));
					}
					
					break;
				case TokenType.END_OF_FILE:
					// the source file was empty, no shame in that
					break;
				default:
					notifyError (new SyntaxError (token, TokenType.UNDEFINED, ParserConstants.EXPECTATION_SET_PROGRAM));
					break;
			}
		}

		/// <summary>
		/// Parses statements.
		/// In the AST a StatementsNode connects a single statement or a block to its successor. 
		/// </summary>
		/// <returns>The next token.</returns>
		/// <param name="token">Token.</param>
		/// <param name="parent">IStatementContainer to affiliate the statements with.</param>
		private Token ParseStatements(Token token, IStatementsContainer parent)
		{
			switch (token.Type) {
				case TokenType.DECLARATION:
				case TokenType.FOR_LOOP:
				case TokenType.READ:
				case TokenType.PRINT:
				case TokenType.ASSERT:
				case TokenType.ID:
					/* Asks the NodeBuilder to create a StatementsNode and affiliate it with the
					   parent node given as argument.

					   Note that the token the scanner provides is also connected to its corresponding
					   node(s). This makes it easier to print informative error messages in case something
					   goes wrong. */
					StatementsNode statements = nodeBuilder.CreateStatementsNode (parent, token);
					Token next;
					
					try {
						// try to parse a statement and affiliate it with the statements node
						next = ParseStatement (token, statements);
					} catch (UnexpectedTokenException ex) {
						// fastforward to the end of statement if it's malformed
						next = FastForwardToStatementEnd (ex);
					}
					
					match (next, TokenType.END_STATEMENT);

					// connect another statements node to this one
					return ParseStatements (scanner.getNextToken(next), statements);
				case TokenType.END_OF_BLOCK:
				case TokenType.END_OF_FILE:
					// if the end of file or block is found, create no more statements to the current block / AST
					return token;
				case TokenType.ERROR:
					// this statement cannot be parsed, notify error and fastforward to a safe spot
					notifyError (new SyntaxError (token, TokenType.UNDEFINED, ParserConstants.EXPECTATION_SET_STATEMENTS));
					
					next = FastForwardToStatementEnd(token);
					// try to parse more statements
					return ParseStatements (scanner.getNextToken(next), parent);
				default:
					return token;
			}
		}

		/// <summary>
		/// Parses a single statement as a StatementsNode's executable statement.
		/// </summary>
		/// <returns>The next token</returns>
		/// <param name="token">Token.</param>
		/// <param name="statementsNode">A StatementsNode.</param>
		private Token ParseStatement(Token token, StatementsNode statementsNode)
		{
			switch (token.Type) {
				case TokenType.DECLARATION:
					return ParseDeclaration (token, statementsNode);
				case TokenType.ID:
					return ParseVariableAssign (token, statementsNode);
				case TokenType.FOR_LOOP:
					return ParseForLoop (token, statementsNode);
				case TokenType.READ:
					return ParseRead (token, statementsNode);
				case TokenType.PRINT:
					return ParsePrint (token, statementsNode);
				case TokenType.ASSERT:
					return ParseAssert (token, statementsNode);
				case TokenType.ERROR:
					notifyError(new SyntaxError (token, TokenType.UNDEFINED, ParserConstants.EXPECTATION_SET_STATEMENT));
					return FastForwardToStatementEnd(token);
				default:
					throw new UnexpectedTokenException (token, TokenType.UNDEFINED, ParserConstants.EXPECTATION_SET_STATEMENT);
			}
		}

		/// <summary>
		/// Parses a declaration statement.
		/// </summary>
		/// <returns>The next token.</returns>
		/// <param name="token">Token.</param>
		/// <param name="statementsNode">A StatementsNode.</param>
		private Token ParseDeclaration(Token token, StatementsNode statementsNode)
		{
			// Try to parse all the pieces that a DeclarationNode needs to be evaluated.
			try {
				VariableIdNode idNode = nodeBuilder.CreateIdNode ();
				// parse the target id
				Token next = ParseVarId (scanner.getNextToken (token), idNode);

				match (next, TokenType.SET_TYPE);
				// parse the id's type
				next = ParseType (scanner.getNextToken (next), idNode);

				// create the actual DeclarationNode
				DeclarationNode declarationNode = nodeBuilder.CreateDeclarationNode(idNode, statementsNode, token);
				// parse the assign for the DeclarationNode
				return ParseAssign (next, declarationNode.AssignNode);
			} catch (UnexpectedTokenException ex) {
				return FastForwardToStatementEnd (ex);
			}
		}

		/// <summary>
		/// Parses an assign statement.
		/// </summary>
		/// <returns>The next token.</returns>
		/// <param name="token">Token.</param>
		/// <param name="statementsNode">Statements node.</param>
		private Token ParseVariableAssign(Token token, StatementsNode statementsNode)
		{
			try {
				VariableIdNode idNode = nodeBuilder.CreateIdNode ();
				// parse the target id
				Token next = ParseVarId (token, idNode);

				match (next, TokenType.ASSIGN);
				AssignNode assignNode = nodeBuilder.CreateAssignNode(idNode, statementsNode, token);

				// parses the expression of the assignment
				return ParseExpression (scanner.getNextToken(next), assignNode);
			} catch (UnexpectedTokenException ex) {
				return FastForwardToStatementEnd (ex);
			}
		}

		/// <summary>
		/// Parses a for-loop block.
		/// </summary>
		/// <returns>The next token.</returns>
		/// <param name="token">Token.</param>
		/// <param name="statementsNode">Statements node.</param>
		private Token ParseForLoop(Token token, StatementsNode statementsNode)
		{
			// first we prepare the ForLoopNode
			VariableIdNode idNode = nodeBuilder.CreateIdNode ();
			ForLoopNode forLoop = nodeBuilder.CreateForLoopNode(idNode, statementsNode, token);
			Token next = null;

			try {
				// then we try to parse the control variables that handle the accumulation and
				// condition checking every time at the beginning of every loop
				next = ParseForLoopControl(forLoop, token);
			} catch (UnexpectedTokenException ex) {
				// fastforward to a safe spot
				if (ex.Token.Type == TokenType.END_OF_BLOCK) {
					return FastForwardToStatementEnd (ex);
				}
				notifyError (new SyntaxError (ex.Token, ex.ExpectedType, ex.ExpectationSet));
				next = FastForwardTo (ParserConstants.BLOCK_DEF_FASTFORWARD_TO, ex.Token);
			}
				
			try {
				// now we parse the statements that are executed during each loop
				next = ParseForLoopStatements(forLoop, next);
			} catch (UnexpectedTokenException ex) {
				return FastForwardToStatementEnd (ex);
			}

			// finally, parse the loop finalization
			return ParseForLoopEndBlock (forLoop, next);
		}

		/// <summary>
		/// Parses for-loop control block.
		/// </summary>
		/// <returns>The next token.</returns>
		/// <param name="forLoop">ForLoopNode.</param>
		/// <param name="token">Token.</param>
		private Token ParseForLoopControl (ForLoopNode forLoop, Token token)
		{
			VariableIdNode idNode = forLoop.IDNode;
			// first, parse the id that holds the accumulator value
			Token next = ParseVarId (scanner.getNextToken (token), idNode);

			match (next, TokenType.RANGE_FROM);
			// second, parse the start index as an assignment for the accumulator id
			forLoop.RangeFrom = nodeBuilder.CreateAssignNode(idNode, next);
			next = ParseExpression (scanner.getNextToken (next), forLoop.RangeFrom);

			match (next, TokenType.RANGE_UPTO);
			// third, parse the maximum value of the index
			next = ParseExpression (scanner.getNextToken (next), forLoop);

			match (next, TokenType.START_BLOCK);
			return next;
		}

		/// <summary>
		/// Parses for-loop's statements.
		/// </summary>
		/// <returns>The next token.</returns>
		/// <param name="forLoop">ForLoopNode.</param>
		/// <param name="token">Token.</param>
		private Token ParseForLoopStatements (ForLoopNode forLoop, Token token)
		{
			StatementsNode statements = nodeBuilder.CreateStatementsNode (forLoop.Token);
			forLoop.Statements = statements;
			// parse the statement's to be executed during each loop
			return ParseStatements (scanner.getNextToken (token), statements);
		}

		/// <summary>
		/// Parses for loop end block.
		/// </summary>
		/// <returns>The next token.</returns>
		/// <param name="forLoop">For loop.</param>
		/// <param name="token">Token.</param>
		private Token ParseForLoopEndBlock(ForLoopNode forLoop, Token token)
		{
			try {
				// try to match the end of block
				match (token, TokenType.END_OF_BLOCK);
				token = scanner.getNextToken (token);
				match (token, TokenType.FOR_LOOP);
				return scanner.getNextToken (token);
			} catch (UnexpectedTokenException ex) {
				// something went wrong, need to find the actual end of the block
				token = FastForwardToEndOfBlock (ex);
			}

			try {
				// if we didn't succeed in the first try, we now found the block's end
				// or the end of file if the block was never finalized
				match (token, TokenType.FOR_LOOP);
				return scanner.getNextToken (token);
			} catch (UnexpectedTokenException ex) {
				return FastForwardToStatementEnd (ex);
			}
		}

		private Token ParseRead(Token token, StatementsNode statementsNode)
		{
			try {
				VariableIdNode varId = nodeBuilder.CreateIdNode ();
				nodeBuilder.CreateIOReadNode(varId, statementsNode, token);
				return ParseVarId (scanner.getNextToken(token), varId);
			} catch (UnexpectedTokenException ex) {
				return FastForwardToStatementEnd (ex);
			}
		}

		private Token ParsePrint (Token token, StatementsNode statementsNode)
		{
			try {
				IOPrintNode printNode = nodeBuilder.CreateIOPrintNode(statementsNode, token);
				return ParseExpression (scanner.getNextToken(token), printNode);
			} catch (UnexpectedTokenException ex) {
				return FastForwardToStatementEnd (ex);
			}
		}

		private Token ParseAssert (Token token, StatementsNode statementsNode)
		{
			try {
				Token next = scanner.getNextToken (token);
				match (next, TokenType.PARENTHESIS_LEFT);

				AssertNode assertNode = nodeBuilder.CreateAssertNode(statementsNode, token);

				next = ParseExpression (scanner.getNextToken (next), assertNode);
				match (next, TokenType.PARENTHESIS_RIGHT);

				nodeBuilder.CreateIOPrintNodeForAssertNode(assertNode);

				return scanner.getNextToken (next);
			} catch (UnexpectedTokenException ex) {
				return FastForwardToStatementEnd (ex);
			}
		}

		private Token ParseVarId(Token token, VariableIdNode idNode)
		{
			switch (token.Type) {
				case TokenType.ID:
					idNode.ID = token.Value;
					idNode.Token = token;
					
					return scanner.getNextToken (token);
				default:
					throw new UnexpectedTokenException (token, TokenType.ID, null);
			}
		}

		private Token ParseType (Token token, VariableIdNode idNode)
		{
			if (!symbolTable.ContainsKey (idNode.ID)) {
				switch (token.Type) {
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
						throw new UnexpectedTokenException (token, TokenType.UNDEFINED, ParserConstants.EXPECTATION_SET_DECLARATION_TYPE);
				}
			}

			return scanner.getNextToken (token);
		}

		private Token ParseAssign (Token token, AssignNode assignNode)
		{
			switch (token.Type) {
				case TokenType.ASSIGN:
					assignNode.Token = token;
					Token next = scanner.getNextToken (token);
					return ParseExpression (next, assignNode);
				case TokenType.END_STATEMENT:
					setDefaultAssignment (assignNode);
					return token;
				default:
					throw new UnexpectedTokenException (token, TokenType.UNDEFINED, ParserConstants.EXPECTATION_SET_ASSIGN);
			}
		}

		private Token ParseExpression (Token token, IExpressionContainer node)
		{
			Token next;

			switch (token.Type) {
				case TokenType.INT_VAL:
				case TokenType.STR_VAL:
				case TokenType.BOOL_VAL:
				case TokenType.PARENTHESIS_LEFT:
				case TokenType.ID:
					BinOpNode binOp = nodeBuilder.CreateBinOpNode(node, token);
					next = ParseOperand (token, binOp);
					return ParseBinaryOp (next, binOp);
				case TokenType.UNARY_OP_LOG_NEG:
					UnOpNode unOp = nodeBuilder.CreateUnOpNode (node, token);
					next = ParseUnaryOp (token, unOp);
					return ParseOperand (next, unOp);
				default:
					throw new UnexpectedTokenException (token, TokenType.UNDEFINED, ParserConstants.EXPECTATION_SET_EXPRESSION);
			}
		}

		private Token ParseOperand (Token token, IExpressionContainer parent)
		{
			switch (token.Type) {
				case TokenType.INT_VAL:
					ParseIntegerOperand(token, parent);	
					break;
				case TokenType.STR_VAL:
					nodeBuilder.CreateStringValueNode(token, parent);
					break;
				case TokenType.BOOL_VAL:
					nodeBuilder.CreateBoolValueNode(token, parent);
					break;
				case TokenType.ID:
					nodeBuilder.CreateIdNode(token, parent);
					break;
				case TokenType.PARENTHESIS_LEFT:
					Token next = ParseExpression (scanner.getNextToken (token), parent);
					match (next, TokenType.PARENTHESIS_RIGHT);
					break;
				default:
					throw new UnexpectedTokenException (token, TokenType.UNDEFINED, ParserConstants.EXPECTATION_SET_EXPRESSION);
			}

			return scanner.getNextToken (token);
		}

		private void ParseIntegerOperand(Token token, IExpressionContainer parent) {
			try {
				nodeBuilder.CreateIntValueNode (token, parent);
			} catch (OverflowException) {
				notifyError (new IntegerOverflowError (token));
				nodeBuilder.CreateDefaultIntValueNode (token, parent);
			}
		}

		private Token ParseBinaryOp (Token token, BinOpNode binOp)
		{
			switch (token.Type) {
				case TokenType.BINARY_OP_ADD:
				case TokenType.BINARY_OP_SUB:
				case TokenType.BINARY_OP_MUL:
				case TokenType.BINARY_OP_DIV:
				case TokenType.BINARY_OP_LOG_LT:
				case TokenType.BINARY_OP_LOG_EQ:
				case TokenType.BINARY_OP_LOG_AND:
					Token next = ParseOperation (token, binOp);
					return ParseOperand (next, binOp);
				default:
					return token;
			}
		}

		private Token ParseUnaryOp (Token token, UnOpNode unOp)
		{
			switch (token.Type) {
				case TokenType.UNARY_OP_LOG_NEG:
					unOp.Operation = TokenType.UNARY_OP_LOG_NEG;
					unOp.Token = token;
					return scanner.getNextToken (token);
				default:
					throw new UnexpectedTokenException (token, TokenType.UNARY_OP_LOG_NEG, null);
			}
		}

		private Token ParseOperation (Token token, BinOpNode binOp)
		{
			switch (token.Type) {
				case TokenType.BINARY_OP_ADD:
				case TokenType.BINARY_OP_DIV:
				case TokenType.BINARY_OP_LOG_AND:
				case TokenType.BINARY_OP_LOG_EQ:
				case TokenType.BINARY_OP_LOG_LT:
				case TokenType.BINARY_OP_MUL:
				case TokenType.BINARY_OP_SUB:
					binOp.Operation = token.Type;
					return scanner.getNextToken (token);
				default:
					throw new UnexpectedTokenException (token, TokenType.UNDEFINED, null);
			}
		}

		private void setDefaultAssignment (AssignNode assignNode)
		{
			TokenType idType = assignNode.IDNode.EvaluationType;

			switch (idType) {
				case TokenType.STR_VAL:
					nodeBuilder.CreateDefaultStringValueNode(assignNode.Token, assignNode);
					break;
				case TokenType.INT_VAL:
					nodeBuilder.CreateDefaultIntValueNode(assignNode.Token, assignNode);
					break;
				case TokenType.BOOL_VAL:
					nodeBuilder.CreateDefaultBoolValueNode (assignNode.Token, assignNode);
					break;
				default:
					throw new UnexpectedTokenException (assignNode.IDNode.Token, TokenType.UNDEFINED, ParserConstants.EXPECTATION_SET_ID_VAL);
			} 
		}

		private void match(Token token, TokenType expectedType)
		{
			if (token.Type != expectedType) {
				syntaxTreeBuilt = false;
				throw new UnexpectedTokenException (token, expectedType, null);
			}
		}

		private Token FastForwardToSourceEnd (UnexpectedTokenException ex)
		{
			notifyError(new SyntaxError(ex.Token, ex.ExpectedType, ex.ExpectationSet));

			Token token;

			do {
				token = scanner.getNextToken (null);
			} while (token.Type != TokenType.END_OF_FILE);

			return token;
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

		private Token FastForwardToEndOfBlock (UnexpectedTokenException ex)
		{
			notifyError (new SyntaxError (ex.Token, ex.ExpectedType, ex.ExpectationSet));
			Token token = ex.Token;
			int blockDepth = 0;

			while (token.Type != TokenType.END_OF_FILE) {
				if (token.Type == TokenType.END_OF_BLOCK) {
					if (blockDepth == 0) {
						return scanner.getNextToken (null);
					}
					blockDepth--;
				}

				if (token.Type == TokenType.FOR_LOOP) {
					blockDepth++;
				}
				token = scanner.getNextToken (null);
			}

			return token;
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

