using System;
using System.Collections.Generic;

namespace MiniPLInterpreter
{
	public class Parser : IErrorAggregator
	{
		private TokenQueue queue;
		private ParseTree tree;
		private List<Error> errors;
		private Scanner scanner;

		public Parser ()
		{
			this.tree = new ParseTree ();
			this.errors = new List<Error> ();
		}

		public Parser (List<Token> tokens)
		{
			this.queue = new TokenQueue(tokens);
			this.tree = new ParseTree ();
			this.errors = new List<Error> ();
		}

		public Scanner Scanner {
			get { return scanner; }
			set { this.scanner = value; }
		}

		public void Parse () {
			ParseProgram (scanner.getNextToken ());
		}

		public void ParseTokens()
		{
			Token t = this.queue.Dequeue ();
			ParseProgram (t);
		}

		private void ParseProgram (Token t)
		{
			switch (t.Type) {
				case TokenType.DECLARATION:
				case TokenType.FOR_LOOP:
				case TokenType.READ:
				case TokenType.WRITE:
				case TokenType.ASSERT:
				case TokenType.ID:
					Token next = ParseStatements (t);
					match (next, TokenType.END_OF_FILE);
					break;
				case TokenType.END_OF_FILE:
					break;
				default:
					notifyError (new SyntaxError (t));
					break;
			}
		}

		private Token ParseStatements(Token t)
		{
			switch (t.Type) {
				case TokenType.DECLARATION:
				case TokenType.FOR_LOOP:
				case TokenType.READ:
				case TokenType.WRITE:
				case TokenType.ASSERT:
				case TokenType.ID:
					Token next = ParseStatement (t);
					if (!match (next, TokenType.END_STATEMENT)) {
						return next;
					}
				return ParseStatements (scanner.getNextToken());
				case TokenType.END_OF_BLOCK:
				case TokenType.END_OF_FILE:
					return t;
				default:
					notifyError (new SyntaxError (t));
					return t;
			}
		}

		private Token ParseStatement(Token t)
		{
			Token next;
			switch (t.Type) {
				case TokenType.DECLARATION:
					next = ParseVarId (scanner.getNextToken());
					if (!match (next, TokenType.SET_TYPE)) {
						return next;
					}
					next = ParseType (scanner.getNextToken());
					return ParseAssign (next);
				case TokenType.ID:
					next = ParseVarId (t);
					if (!match (next, TokenType.ASSIGN)) {
						return next;
					}
					return ParseExpression (scanner.getNextToken());
				case TokenType.FOR_LOOP:
					next = ParseVarId (scanner.getNextToken());
					if (!match (next, TokenType.RANGE_FROM)) {
						return next;
					}
					next = ParseExpression (scanner.getNextToken());
					if (!match (next, TokenType.RANGE_UPTO)) {
						return next;
					}
					next = ParseExpression (scanner.getNextToken());
					if (!match (next, TokenType.START_BLOCK)) {
						return next;
					}
					next = ParseStatements (scanner.getNextToken());
					if (!match (next, TokenType.END_OF_BLOCK)) {
						return next;
					}
					next = scanner.getNextToken();
					if (!match (next, TokenType.FOR_LOOP)) {
						return next;
					}
					return scanner.getNextToken();
				case TokenType.READ:
					return ParseVarId (scanner.getNextToken());
				case TokenType.WRITE:
					return ParseExpression (scanner.getNextToken());
				case TokenType.ASSERT:
					next = scanner.getNextToken();
					if (!match (next, TokenType.PARENTHESIS_LEFT)) {
						return next;
					}
					next = ParseExpression (scanner.getNextToken ());
					if (!match (next, TokenType.PARENTHESIS_RIGHT)) {
						return next;
					}
					return scanner.getNextToken ();
				default:
					notifyError (new SyntaxError (t));
					return t;
			}
		}

		private Token ParseVarId(Token t)
		{
			switch (t.Type) {
				case TokenType.ID:
					return scanner.getNextToken ();
				default:
					notifyError (new SyntaxError (t));
					return t;
			}
		}

		private Token ParseType (Token t)
		{
			switch (t.Type) {
				case TokenType.INT_VAR:
					return scanner.getNextToken ();
				case TokenType.STR_VAR:
					return scanner.getNextToken ();
				case TokenType.BOOL_VAR:
					return scanner.getNextToken ();
				default:
					notifyError (new SyntaxError (t));
					return t;
			}
		}

		private Token ParseAssign (Token t)
		{
			switch (t.Type) {
				case TokenType.ASSIGN:
					Token next = scanner.getNextToken ();
					return ParseExpression (next);
				case TokenType.END_STATEMENT:
					return t;
				default:
					notifyError (new SyntaxError (t));
					return t;
			}
		}

		private Token ParseExpression (Token t)
		{
			Token next;
			switch (t.Type) {
				case TokenType.INT_VAL:
				case TokenType.STR_VAL:
				case TokenType.PARENTHESIS_LEFT:
				case TokenType.ID:
					next = ParseOperand (t);
					return ParseBinaryOp (next);
				case TokenType.UNARY_OP_LOG_NEG:
					next = ParseUnaryOp (t);
					return ParseOperand (next);
				default:
					notifyError (new SyntaxError (t));
					return t;
			}
		}

		private Token ParseOperand (Token t)
		{
			switch (t.Type) {
				case TokenType.INT_VAL:
					return scanner.getNextToken ();
				case TokenType.STR_VAL:
					return scanner.getNextToken ();
				case TokenType.ID:
					return scanner.getNextToken ();
				case TokenType.PARENTHESIS_LEFT:
					Token next = ParseExpression (scanner.getNextToken ());
					if (!match (next, TokenType.PARENTHESIS_RIGHT)) {
						return next;
					}
					return scanner.getNextToken ();
				default:
					notifyError (new SyntaxError (t));
					return t;
			}
		}

		private Token ParseBinaryOp (Token t)
		{
			switch (t.Type) {
				case TokenType.BINARY_OP_ADD:
				case TokenType.BINARY_OP_SUB:
				case TokenType.BINARY_OP_MUL:
				case TokenType.BINARY_OP_DIV:
				case TokenType.BINARY_OP_LOG_LT:
				case TokenType.BINARY_OP_LOG_EQ:
				case TokenType.BINARY_OP_LOG_AND:
					Token next = ParseOperation (t);
					return ParseOperand (next);
				case TokenType.PARENTHESIS_RIGHT:
				case TokenType.END_STATEMENT:
				case TokenType.RANGE_UPTO:
				case TokenType.START_BLOCK:
					return t;
				default:
					notifyError (new SyntaxError (t));
					return t;
			}
		}

		private Token ParseUnaryOp (Token t)
		{
			switch (t.Type) {
				case TokenType.UNARY_OP_LOG_NEG:
					return scanner.getNextToken ();
				default:
					notifyError (new SyntaxError (t));
					return t;
			}
		}

		private Token ParseOperation (Token t)
		{
			switch (t.Type) {
				case TokenType.BINARY_OP_ADD:
					return scanner.getNextToken ();
				case TokenType.BINARY_OP_DIV:
					return scanner.getNextToken ();
				case TokenType.BINARY_OP_LOG_AND:
					return scanner.getNextToken ();
				case TokenType.BINARY_OP_LOG_EQ:
					return scanner.getNextToken ();
				case TokenType.BINARY_OP_LOG_LT:
					return scanner.getNextToken ();
				case TokenType.BINARY_OP_MUL:
					return scanner.getNextToken ();
				case TokenType.BINARY_OP_SUB:
					return scanner.getNextToken ();
				default:
					notifyError (new SyntaxError (t));
					return t;
			}
		}

		private bool match(Token t, TokenType type)
		{
			if (t.Type != type) {
				notifyError (new SyntaxError (t));
				return false;
			}

			return true;
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

