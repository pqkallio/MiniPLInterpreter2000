using System;
using System.IO;
using System.Text;
using System.Linq;
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
		private StreamReader inputStream;
		private int col = 0;
		private int row = 1;

		public Scanner (StreamReader inputStream)
		{
			this.inputStream = inputStream;
			this.errors = new List<Error> ();
		}

		private char peekStream() {
			return inputStream.EndOfStream ? '\0' : (char)inputStream.Peek ();
		}

		private char readStream() {
			if (inputStream.EndOfStream) {
				return '\0';
			}

			char c = (char)inputStream.Read ();

			if (c == Constants.LINEBREAK) {
				row++;
				col = 0;
			} else {
				col++;
			}

			return c;
		}

		public Token getNextToken(Token previous)
		{
			Token token = null;

			while (!inputStream.EndOfStream) {
				char c = readStream();

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
				StringBuilder sb = new StringBuilder (c.ToString());
				token.Type = TokenType.ID;
				while (!inputStream.EndOfStream) {

					c = peekStream ();
					if (StringUtils.isAlpha (c) || StringUtils.isInteger (c) || c == '_') {
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
					}
				} else {
					token.Value = val;
				}
			} else {
				notifyError (new TokenError (token));
				token.Type = TokenType.ERROR;
			}

			return token;
		}

		private void parseDivOrComment (ref Token token, char c)
		{
			token.Type = Constants.BINARY_OP_DIV.Item2;
			if (peekStream () == Constants.BINARY_OP_DIV.Item1) {
				token = null;
				parseSingleLineComment ();
			} else if (peekStream () == '*') {
				int nestingDepth = 1;
				bool starHit = false;
				token = null;
				while (!inputStream.EndOfStream) {
					c = readStream ();

					if (c == Constants.LINEBREAK) {
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
		}

		private void parseSingleLineComment ()
		{
			while (!inputStream.EndOfStream) {
				char c = readStream ();
				if (c == Constants.LINEBREAK) {
					break;
				}
			}
		}

		private void parseInteger (Token token, char c)
		{
			token.Type = TokenType.INT_VAL;
			StringBuilder sb = new StringBuilder (c.ToString());

			while (!inputStream.EndOfStream) {
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

				while (!inputStream.EndOfStream) {
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

			while (!inputStream.EndOfStream) {
				char c = readStream ();

				if (c == Constants.LINEBREAK) {
					notifyError (new StringLiteralError (token, "String literal mustn't span multiple lines"));
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
			return c == Constants.LINEBREAK || Constants.WHITESPACES.ContainsKey (c);
		}
	}
}
