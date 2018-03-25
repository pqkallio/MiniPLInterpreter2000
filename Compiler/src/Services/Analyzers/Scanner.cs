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
		private int col = -1;
		private int row = 0;
		private bool EndOfStream = false;

		public Scanner (string[] sourceLines)
		{
			this.sourceLines = sourceLines;
			this.errors = new List<Error> ();
		}

		private char peekStream() {
			if (EndOfStream) {
				return ScannerConstants.NULL_CHAR;
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
				return ScannerConstants.NULL_CHAR;
			}

			col++;

			if (col >= sourceLines [row].Length) {
				col = -1;
				row++;

				if (row >= sourceLines.Length) {
					EndOfStream = true;
				}

				return ScannerConstants.NEWLINE;
			}

			char c = sourceLines [row] [col];

			return c;
		}

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

			return token;
		}

		private Token scanToken (Token previous, char c)
		{
			Token token = new Token (row, col);

			if (ScannerConstants.INDEPENDENT_CHARS.ContainsKey (c)) {
				token.Type = ScannerConstants.INDEPENDENT_CHARS [c];
			} else if (c == ScannerConstants.SET_TYPE.Item1) {
				parseSetTypeOrAssign (token);
			} else if (c == '.') {
				parseForLoopRangeUpto (token);
			} else if (c == ScannerConstants.BINARY_OP_ADD.Item1 || c == ScannerConstants.BINARY_OP_SUB.Item1) {
				parseAddOrSub (token, previous, c);
			} else if (c == ScannerConstants.STRING_DELIMITER) {
				parseString (token);
			} else if (StringUtils.isInteger (c)) {
				parseInteger (token, c);
			} else if (c == ScannerConstants.BINARY_OP_DIV.Item1) {
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

			if (ScannerConstants.RESERVED_SEQUENCES.ContainsKey (val)) {
				if (previous != null && previous.Type == TokenType.DECLARATION) {
					token.Value = val;
					notifyError (new TokenError (token, "reserved keyword used as variable identifier"));
				} else {
					token.Type = ScannerConstants.RESERVED_SEQUENCES [val];
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
			token.Type = ScannerConstants.BINARY_OP_DIV.Item2;

			if (peekStream () == ScannerConstants.BINARY_OP_DIV.Item1) {
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
				if (c == ScannerConstants.NEWLINE) {
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

				if (c == ScannerConstants.NEWLINE) {
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
			token.Type = c == ScannerConstants.BINARY_OP_ADD.Item1 ? ScannerConstants.BINARY_OP_ADD.Item2 : ScannerConstants.BINARY_OP_SUB.Item2;

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
			if (peekStream() == ScannerConstants.ASSIGN.Item1) {
				readStream();
				token.Type = ScannerConstants.ASSIGN.Item2;
			} else {
				token.Type = ScannerConstants.SET_TYPE.Item2;
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

				if (c == ScannerConstants.NEWLINE && !errored) {
					notifyError (new StringLiteralError (token, ErrorConstants.LINEBREAK_IN_STR_LITERAL_MESSAGE));
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
						case 'r':
							sb.Append ('\r');
							break;
						case '0':
							break;
						default:
							sb.Append (c);
							break;
					}
				} else {
					escapeNextChar = c == ScannerConstants.ESCAPE_CHAR;
					stringEnded = c == ScannerConstants.STRING_DELIMITER;

					if (stringEnded) {
						break;
					}

					if (!escapeNextChar) {
						sb.Append (c);
					}
				}
			}

			if (!stringEnded) {
				notifyError (new StringLiteralError (token, ErrorConstants.EOF_WHILE_SCANNING_MESSAGE));
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
			return c == ScannerConstants.NEWLINE || ScannerConstants.WHITESPACES.ContainsKey (c);
		}

		private bool isIdCharacter (char c)
		{
			return StringUtils.isAlpha (c) || StringUtils.isInteger (c) || c == '_';
		}
	}
}
