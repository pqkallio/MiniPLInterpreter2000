using System;
using System.Collections.Generic;

namespace MiniPLInterpreter
{
	public class Parser : IErrorAggregator
	{
		private TokenQueue queue;
		private ParseTree tree;
		private List<Error> errors;

		public Parser (List<Token> tokens)
		{
			this.queue = new TokenQueue(tokens);
			this.tree = new ParseTree ();
			this.errors = new List<Error> ();
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
					return ParseStatements (queue.Dequeue());
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
					next = ParseVarId (queue.Dequeue ());
					if (!match (next, TokenType.SET_TYPE)) {
						return next;
					}
					next = ParseType (queue.Dequeue());
					return ParseAssign (next);
				case TokenType.ID:
					next = ParseVarId (t);
					if (!match (next, TokenType.ASSIGN)) {
						return next;
					}
					return ParseExpression (queue.Dequeue ());
				case TokenType.FOR_LOOP:
					next = ParseVarId (queue.Dequeue ());
					if (!match (next, TokenType.RANGE_FROM)) {
						return next;
					}
					next = ParseExpression (queue.Dequeue ());
					if (!match (next, TokenType.RANGE_UPTO)) {
						return next;
					}
					next = ParseExpression (queue.Dequeue ());
					if (!match (next, TokenType.START_BLOCK)) {
						return next;
					}
					next = ParseStatements (queue.Dequeue ());
					if (!match (next, TokenType.END_OF_BLOCK)) {
						return next;
					}
					next = queue.Dequeue ();
					if (!match (next, TokenType.FOR_LOOP)) {
						return next;
					}
					return queue.Dequeue ();
				case TokenType.READ:
					return ParseVarId (queue.Dequeue ());
				case TokenType.WRITE:
					return ParseExpression (queue.Dequeue ());
				case TokenType.ASSERT:
					next = queue.Dequeue ();
					if (!match (next, TokenType.PARENTHESIS_LEFT)) {
						return next;
					}
					next = ParseExpression (queue.Dequeue ());
					if (!match (next, TokenType.PARENTHESIS_RIGHT)) {
						return next;
					}
					return queue.Dequeue ();
				default:
					notifyError (new SyntaxError (t));
					return t;
			}
		}

		private Token ParseVarId(Token t)
		{
			switch (t.Type) {
				case TokenType.ID:
					return queue.Dequeue ();
				default:
					notifyError (new SyntaxError (t));
					return t;
			}
		}

		private Token ParseType (Token t)
		{
			switch (t.Type) {
				case TokenType.INT_VAR:
					return queue.Dequeue ();
				case TokenType.STR_VAR:
					return queue.Dequeue ();
				case TokenType.BOOL_VAR:
					return queue.Dequeue ();
				default:
					notifyError (new SyntaxError (t));
					return t;
			}
		}

		private Token ParseAssign (Token t)
		{
			switch (t.Type) {
				case TokenType.ASSIGN:
					Token next = queue.Dequeue ();
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
					return queue.Dequeue ();
				case TokenType.STR_VAL:
					return queue.Dequeue ();
				case TokenType.ID:
					return queue.Dequeue ();
				case TokenType.PARENTHESIS_LEFT:
					Token next = ParseExpression (queue.Dequeue ());
					if (!match (next, TokenType.PARENTHESIS_RIGHT)) {
						return next;
					}
					return queue.Dequeue ();
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
					return queue.Dequeue ();
				default:
					notifyError (new SyntaxError (t));
					return t;
			}
		}

		private Token ParseOperation (Token t)
		{
			switch (t.Type) {
				case TokenType.BINARY_OP_ADD:
					return queue.Dequeue ();
				case TokenType.BINARY_OP_DIV:
					return queue.Dequeue ();
				case TokenType.BINARY_OP_LOG_AND:
					return queue.Dequeue ();
				case TokenType.BINARY_OP_LOG_EQ:
					return queue.Dequeue ();
				case TokenType.BINARY_OP_LOG_LT:
					return queue.Dequeue ();
				case TokenType.BINARY_OP_MUL:
					return queue.Dequeue ();
				case TokenType.BINARY_OP_SUB:
					return queue.Dequeue ();
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

