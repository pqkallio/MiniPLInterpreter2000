using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace MiniPLInterpreter
{
	/// <summary>
	/// Scanner class is responsible for the scanning phase.
	/// It scans through the original source code and divides it into string tokens.
	/// These tokens are then passed to the Parser.
	/// </summary>
	public class Scanner : IErrorAggregator
	{
		private List<Error> errors;
		private string[] sourceLines;
		private StreamReader inputStream;
		private int col = -1;
		private int row = 0;
		private bool EndOfStream = false;

		public Scanner (StreamReader inputStream, string[] sourceLines)
		{
			this.inputStream = inputStream;
			this.sourceLines = sourceLines;
			this.errors = new List<Error> ();
		}

		private char peekStream() {
			if (EndOfStream) {
				return Constants.NULL_CHAR;
			}
			
			char c = readStream ();

			col--;

			if (col < 0) {
				row--;
				col = sourceLines [row].Length - 1;
			}

			return c;
		}

		private char readStream() {
			if (EndOfStream) {
				return Constants.NULL_CHAR;
			}

			col++;

			if (col >= sourceLines [row].Length) {
				col = 0;
				row++;
			}

			if (row >= sourceLines.Length) {
				EndOfStream = true;
				return Constants.NULL_CHAR;
			}

			char c = sourceLines [row] [col]; 

			return c;
		}

		/*
		private char peekStream() {
			return inputStream.EndOfStream ? '\0' : (char)inputStream.Peek ();
		}

		private char readStream() {
			if (inputStream.EndOfStream) {
				return '\0';
			}

			char c = (char)inputStream.Read ();
			char d = c;
			long readIndex = inputStream.BaseStream.Position;
			bool isLineBreak = true;

			for (int i = 0; i < Constants.LINEBREAK.Length; i++) {
				if (Constants.LINEBREAK [i] != d) {
					isLineBreak = false;
					break;
				}

				d = (char)inputStream.Read ();
			}

			if (isLineBreak) {
				row++;
				col = -1;
				long posBefore = inputStream.BaseStream.Position;
				inputStream.BaseStream.Position = inputStream.BaseStream.Position - 1;
				long posAfter = inputStream.BaseStream.Position;
				char f = (char)inputStream.Peek ();
				return Constants.NEWLINE;
			}
				
			inputStream.BaseStream.Position = readIndex;
			col++;
			return c;
		}
		*/

		public Token getNextToken(Token previous)
		{
			Token token = null;

			while (!EndOfStream) {
				char c = readStream();

				if (EndOfStream) {
					break;
				}

				if (isWhitespace(c)) {
					continue;
				}

				token = scanToken (previous, c);

				if (token != null) {
					break;
				}
			}

			if (token == null) {
				token = new Token (row, col, "", TokenType.END_OF_FILE);
			}

			Console.WriteLine (token);

			return token;
		}

		private Token scanToken (Token previous, char c)
		{
			Token token = new Token (row, col);

			if (Constants.INDEPENDENT_CHARS.ContainsKey (c)) {
				token.Type = Constants.INDEPENDENT_CHARS [c];
			} else if (c == Constants.SET_TYPE.Item1) {
				parseSetTypeOrAssign (token);
			} else if (c == '.') {
				parseForLoopRangeUpto (token);
			} else if (c == Constants.BINARY_OP_ADD.Item1 || c == Constants.BINARY_OP_SUB.Item1) {
				parseAddOrSub (token, previous, c);
			} else if (c == Constants.STRING_DELIMITER) {
				parseString (token);
			} else if (StringUtils.isInteger (c)) {
				parseInteger (token, c);
			} else if (c == Constants.BINARY_OP_DIV.Item1) {
				parseDivOrComment (ref token, c);
			} else if (StringUtils.isAlpha(c)) {
				parseIdOrKeyword (token, previous, c);
			} else {
				parseErrorToken (token, c);
				token.Type = TokenType.ERROR;
			}

			return token;
		}

		private void parseIdOrKeyword (Token token, Token previous, char c)
		{
			StringBuilder sb = new StringBuilder (c.ToString());
			token.Type = TokenType.ID;

			while (!EndOfStream) {
				c = peekStream ();

				if (isIdCharacter(c)) {
					sb.Append (readStream ());
				} else {
					break;
				}
			}

			string val = sb.ToString ();

			if (Constants.RESERVED_SEQUENCES.ContainsKey (val)) {
				if (previous != null && previous.Type == TokenType.DECLARATION) {
					token.Value = val;
					notifyError (new TokenError (token, "Reserved keyword used as variable identifier"));
				} else {
					token.Type = Constants.RESERVED_SEQUENCES [val];
					if (token.Type == TokenType.BOOL_VAL) {
						token.Value = val;
					}
				}
			} else {
				token.Value = val;
			}
		}

		private void parseDivOrComment (ref Token token, char c)
		{
			token.Type = Constants.BINARY_OP_DIV.Item2;

			if (peekStream () == Constants.BINARY_OP_DIV.Item1) {
				token = null;
				parseSingleLineComment ();
			} else if (peekStream () == '*') {
				token = null;
				parseMultilineComment ();
			}
		}

		private void parseSingleLineComment ()
		{
			while (!EndOfStream) {
				char c = readStream ();
				if (c == Constants.NEWLINE) {
					break;
				}
			}
		}

		private void parseMultilineComment ()
		{
			int nestingDepth = 1;
			bool starHit = false;

			while (!EndOfStream) {
				char c = readStream ();

				if (c == Constants.NEWLINE) {
					starHit = false;
				} else if (c == '*') {
					starHit = true;
				} else if (c == '/') {
					if (starHit) {
						nestingDepth--;
						if (nestingDepth == 0) {
							break;
						}
					} else {
						c = readStream ();

						if (c == '*') {
							nestingDepth++;
						}
					}
				} else {
					starHit = false;
				}
			}
		}

		private void parseInteger (Token token, char c)
		{
			token.Type = TokenType.INT_VAL;
			StringBuilder sb = new StringBuilder (c.ToString());

			while (!EndOfStream) {
				c = peekStream ();
				if (StringUtils.isInteger (c)) {
					sb.Append (readStream ());
				} else {
					break;
				}
			}

			token.Value = sb.ToString ();
		}

		private void parseAddOrSub (Token token, Token previous, char c)
		{
			token.Type = c == Constants.BINARY_OP_ADD.Item1 ? Constants.BINARY_OP_ADD.Item2 : Constants.BINARY_OP_SUB.Item2;

			if (StringUtils.isInteger (peekStream()) && (previous == null || !isBinaryOperand(previous))) {
				StringBuilder sb = new StringBuilder (c.ToString());
				token.Type = TokenType.INT_VAL;

				while (!EndOfStream) {
					c = peekStream ();

					if (StringUtils.isInteger (c)) {
						sb.Append(readStream ());
					} else {
						break;
					}
				}

				token.Value = sb.ToString ();
			}
		}

		private void parseForLoopRangeUpto (Token token)
		{
			if (peekStream() == '.') {
				readStream();
				token.Type = TokenType.RANGE_UPTO;
			} else {
				token.Type = TokenType.ERROR;
			}
		}

		private void parseSetTypeOrAssign(Token token)
		{
			if (peekStream() == Constants.ASSIGN.Item1) {
				readStream();
				token.Type = Constants.ASSIGN.Item2;
			} else {
				token.Type = Constants.SET_TYPE.Item2;
			}
		}

		private void parseString (Token token)
		{
			token.Type = TokenType.STR_VAL;
			bool escapeNextChar = false;
			bool stringEnded = false;
			StringBuilder sb = new StringBuilder ();
			bool errored = false;

			while (!EndOfStream) {
				char c = readStream ();

				if (c == Constants.NEWLINE && !errored) {
					notifyError (new StringLiteralError (token, "String literal mustn't span multiple lines"));
					errored = true;
				} else if (escapeNextChar) {
					escapeNextChar = false;

					switch (c) {
					case 'n':
						sb.Append ('\n');
						break;
					case 't':
						sb.Append ('\t');
						break;
						// and so on until all the cases are covered...
					default:
						sb.Append (c);
						break;
					}
				} else {
					escapeNextChar = c == Constants.ESCAPE_CHAR;
					stringEnded = c == Constants.STRING_DELIMITER;

					if (stringEnded) {
						break;
					}

					if (!escapeNextChar) {
						sb.Append (c);
					}
				}
			}

			if (!stringEnded) {
				notifyError (new StringLiteralError (token, "EOF encountered while parsing string literal"));
				token.Type = TokenType.ERROR;
			} else {
				token.Value = sb.ToString ();
			}
		}

		private void parseErrorToken(Token t, char c)
		{
			StringBuilder sb = new StringBuilder (c.ToString());

			c = peekStream ();

			while (!isWhitespace (c) && c != ';') {
				sb.Append (readStream());
				c = peekStream ();
			}

			t.Value = sb.ToString ();
		}

		private bool isBinaryOperand (Token t)
		{
			TokenType tt = t.Type;

			return  tt == TokenType.INT_VAL ||
					tt == TokenType.STR_VAL ||
					tt == TokenType.BOOL_VAL ||
					tt == TokenType.ID;
		}

		public void notifyError(Error error)
		{
			this.errors.Add (error);
		}

		public List<Error> getErrors () {
			return this.errors;
		}

		private bool isWhitespace (char c)
		{
			return c == Constants.NEWLINE || Constants.WHITESPACES.ContainsKey (c);
		}

		private bool isIdCharacter (char c)
		{
			return StringUtils.isAlpha (c) || StringUtils.isInteger (c) || c == '_';
		}
	}
}
