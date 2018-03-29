using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace MiniPLInterpreter
{
	/// <summary>
	/// A lexical analyzer for Mini-PL programming language
	/// Provides tokenizing service for the parser
	/// </summary>
	public class Scanner : IErrorAggregator
	{
		private List<Error> errors; 	// a list of errors encountered during scanning
		private string[] sourceLines;	// the source code as independent lines
		private int column = -1;		// the current reading position in the current source line
		private int line = 0;			// the number of current source line.
		private bool endOfStream = false; // true if the source has been read in full

		/// <summary>
		/// Initializes a new instance of the <see cref="MiniPLInterpreter.Scanner"/> class.
		/// </summary>
		/// <param name="sourceLines">The source code's lines as an array of strings.</param>
		public Scanner (string[] sourceLines)
		{
			this.sourceLines = sourceLines;
			this.errors = new List<Error> ();
		}

		/// <summary>
		/// Peeks the next character from the current source line.
		/// </summary>
		/// <returns>The next char in the current source line or a null char if there's nothing more to read.</returns>
		private char peekStream() {
			if (endOfStream) {
				return ScannerConstants.NULL_CHAR;
			}
			
			char c = readStream ();		// read the next char
			stepBack ();				// unread the next char

			return c;
		}

		/// <summary>
		/// Reads the next character from the current source line.
		/// </summary>
		/// <returns>The next char in the current source line or a null char if there's nothing more to read.</returns>
		private char readStream() {
			if (endOfStream || sourceLines == null || sourceLines.Length < 1) {
				endOfStream = true;
				return ScannerConstants.NULL_CHAR;
			}

			column++;

			if (column >= sourceLines [line].Length) {	// if the current line is read, go to the next one
				column = -1;
				line++;

				if (line >= sourceLines.Length) {		// if there are no more lines to be read, mark the EOS reached
					endOfStream = true;
				}

				return ScannerConstants.NEWLINE;		// return a newline character if new line was met
			}

			char c = sourceLines [line] [column];		// otherwise, read the next char and return it

			return c;
		}

		/// <summary>
		/// Unreads the last read char.
		/// </summary>
		private void stepBack () {
			column--;

			if (column < 0) {
				if (line != 0) {
					line--;
					column = sourceLines [line].Length - 1;
				} else {
					column = 0;
				}
			}
		}

		/// <summary>
		/// This is the method the parser calls when it wants another token.
		/// The previous token is given as an argument to help the scanner choose the right token type if there are options.
		/// </summary>
		/// <returns>The next token.</returns>
		/// <param name="previous">The previous token the parser received, can be null</param>
		public Token getNextToken(Token previous)
		{
			Token token = null;

			while (!endOfStream) {			// try to read the next token until the end of the source is reached
				char c = readStream();

				if (endOfStream) {			// in case this was the end of the stream
					break;
				}

				if (isWhitespace(c)) {		// if the char is a whitespace, we read the next char.
					continue;
				}

				token = scanToken (previous, c);	// dive into the DFA.

				if (token != null) {		// if the DFA returned null (screened a comment for example), read another char
					break;					// otherwise, break out from the loop
				}
			}

			if (token == null) {			// if no token was read, it must be the end of file
				token = new Token (line, column, "", TokenType.END_OF_FILE);
			}

			return token;
		}

		/// <summary>
		/// Scans and return the next token.
		/// </summary>
		/// <returns>The scanned token.</returns>
		/// <param name="previous">The previously scanned token from the parser</param>
		/// <param name="c">The character read by the scanner</param>
		private Token scanToken (Token previous, char c)
		{
			Token token = new Token (line, column);						// prepare an empty token to be filled

			if (ScannerConstants.INDEPENDENT_CHARS.ContainsKey (c)) {	// if it's special, its a token of its own
				token.Type = ScannerConstants.INDEPENDENT_CHARS [c];
			} else if (c == ScannerConstants.SET_TYPE.Item1) {			// if it's a colon, it might set type or be a part of assignment
				parseSetTypeOrAssign (token);
			} else if (c == ScannerConstants.DOT) {						// if it's a dot, we'll go look for another one
				parseForLoopRangeUpto (token);
			} else if (c == ScannerConstants.BINARY_OP_ADD.Item1		// if it's a minus or plus sign we'll have to check whether
				|| c == ScannerConstants.BINARY_OP_SUB.Item1) {			// it starts an integer or not
				parseAddOrSub (token, previous, c);
			} else if (c == ScannerConstants.STRING_DELIMITER) {		// it's a a start of a string literal
				parseString (token);
			} else if (StringUtils.isInteger (c)) {						// it's clearly an integer literal
				parseInteger (token, c);
			} else if (c == ScannerConstants.BINARY_OP_DIV.Item1) {		// it's a division sign, but it might also be a single- or
				parseDivOrComment (ref token, c);						// a multiline comment.
			} else if (StringUtils.isAlpha(c)) {						// it's some kind of an identifier
				parseIdOrKeyword (token, previous, c);
			} else {													// it's no good
				parseErrorToken (token, c);
			}

			return token;
		}

		/// <summary>
		/// Parses an identifier and later screens whether it is a keyword.
		/// Obeying the maximal munch rule, we keep building the token until it isn't a char fit for an id.
		/// </summary>
		/// <param name="token">The token to fill with information</param>
		/// <param name="previous">The previously scanned token from the parser</param>
		/// <param name="c">The character read by the scanner</param>
		private void parseIdOrKeyword (Token token, Token previous, char c)
		{
			StringBuilder sb = new StringBuilder (c.ToString());	// let's start building
			token.Type = TokenType.ID;								// we make an initial guess: it's a variable id

			while (!endOfStream) {									// keep reading until there's nothing to be read
				c = peekStream ();

				if (isIdCharacter(c)) {								// append the char if it can be a part of id
					sb.Append (readStream ());
				} else {											// else break from the loop
					break;
				}
			}

			string val = sb.ToString ();

			if (ScannerConstants.RESERVED_SEQUENCES.ContainsKey (val)) {						// if the value is a reserved keyword
				if (previous != null && previous.Type == TokenType.DECLARATION) {				// and a declaration was made using it as an id
					token.Value = val;
					notifyError (new TokenError (token, ErrorConstants.INVALID_ID_MESSAGE));	// report an error
				} else {
					token.Type = ScannerConstants.RESERVED_SEQUENCES [val];						// else, we set the type to match the keyword
					if (token.Type == TokenType.BOOL_VAL) {										// and if it's a boolean value, save the value as well
						token.Value = val;
					}
				}
			} else {																			// if it wasn't a keyword, save the value as id
				token.Value = val;
			}
		}

		/// <summary>
		/// Scans for a division operator or a comment, depending on what comes after the slash character
		/// </summary>
		/// <param name="token">A pointer to the token</param>
		/// <param name="c">the char read by the scanner</param>
		private void parseDivOrComment (ref Token token, char c)
		{
			token.Type = ScannerConstants.BINARY_OP_DIV.Item2;				// it's a division operator until proven other

			if (peekStream () == ScannerConstants.BINARY_OP_DIV.Item1) {	// if it's a start of a singleline comment
				token = null;												// discard the token and screen the comment
				parseSingleLineComment ();
			} else if (peekStream () == '*') {								// the same goes if it's a multiline comment
				token = null;
				parseMultilineComment ();
			}
		}

		/// <summary>
		/// Screens a single line comment.
		/// </summary>
		private void parseSingleLineComment ()
		{
			while (!endOfStream) {						// keep reading the character stream until an end of line or an end of file
				char c = readStream ();					// or an end of file is read
				if (c == ScannerConstants.NEWLINE) {
					break;
				}
			}
		}

		/// <summary>
		/// Screens a multiline comment.
		/// </summary>
		private void parseMultilineComment ()
		{
			int nestingDepth = 1;						// multiline comments can be nested
			bool starHit = false;						// stars can be inside the comment without ending it

			while (!endOfStream) {						// keep reading
				char c = readStream ();

				if (c == ScannerConstants.NEWLINE) {
					starHit = false;
				} else if (c == '*') {					// we found a star
					starHit = true;
				} else if (c == '/') {
					if (starHit) {						// if the star is followed by a slash
						nestingDepth--;					// decrease the nesting depth
						if (nestingDepth == 0) {		// and break if we're out of the comment for real
							break;
						}
					} else {
						c = readStream ();

						if (c == '*') {					// if the slash is followed by a star, the nesting goes deeper
							nestingDepth++;
						}
					}
				} else {
					starHit = false;
				}
			}
		}

		/// <summary>
		/// Scans an integer.
		/// </summary>
		/// <param name="token">The token to fill with information</param>
		/// <param name="c">the char read by the scanner</param>
		private void parseInteger (Token token, char c)
		{
			token.Type = TokenType.INT_VAL;
			StringBuilder sb = new StringBuilder (c.ToString());	// let's start reading integers

			token.Value = readAnInteger (sb);						// save the value to the token
		}

		/// <summary>
		/// Scans an add or sub operator, or an integer.
		/// </summary>
		/// <param name="token">The token to fill with info</param>
		/// <param name="previous">The previous token from the parser, very important!</param>
		/// <param name="c">the char read by the scanner</param>
		private void parseAddOrSub (Token token, Token previous, char c)
		{
			// first we define the token's type as an addition or a subtraction
			token.Type = c == ScannerConstants.BINARY_OP_ADD.Item1 ? ScannerConstants.BINARY_OP_ADD.Item2 : ScannerConstants.BINARY_OP_SUB.Item2;

			// if the next character is an integer and the previous token wasn't a binary operator
			// then it wasn't an addition or a subtraction, but a sign of an integer
			if (StringUtils.isInteger (peekStream()) && (previous == null || !isBinaryOperand(previous))) {
				StringBuilder sb = new StringBuilder (c.ToString());
				token.Type = TokenType.INT_VAL;

				token.Value = readAnInteger (sb);
			}
		}

		/// <summary>
		/// Reads integers into the StringBuilder.
		/// </summary>
		/// <returns>An integer as a string</returns>
		/// <param name="sb">A StringBuilder buffering the input</param>
		private string readAnInteger (StringBuilder sb)
		{
			char c;

			while (!endOfStream) {
				c = peekStream ();

				if (StringUtils.isInteger (c)) {
					sb.Append (readStream ());
				} else {											// if it's not an integer, break
					break;
				}
			}

			return sb.ToString ();									// return the buffered string
		}

		/// <summary>
		/// Scans for a ".." symbol used to define the range of a for loop's control variable.
		/// </summary>
		/// <param name="token">Token.</param>
		private void parseForLoopRangeUpto (Token token)
		{
			if (peekStream() == ScannerConstants.DOT) {
				readStream();
				token.Type = TokenType.RANGE_UPTO;
			} else {
				token.Type = TokenType.ERROR;				// it's an error if it's not a double dot
			}
		}

		/// <summary>
		/// Scans either a type setting ":" or an assignment ":=".
		/// </summary>
		/// <param name="token">Token.</param>
		private void parseSetTypeOrAssign(Token token)
		{
			if (peekStream() == ScannerConstants.ASSIGN.Item1) {	// if the next one is "=", then it's ":="
				readStream();
				token.Type = ScannerConstants.ASSIGN.Item2;
			} else {
				token.Type = ScannerConstants.SET_TYPE.Item2;		// otherwise it's a ":"
			}
		}

		/// <summary>
		/// Parses a string literal.
		/// </summary>
		/// <param name="token">Token.</param>
		private void parseString (Token token)
		{
			token.Type = TokenType.STR_VAL;
			bool escapeNextChar = false;
			bool stringEnded = false;
			StringBuilder sb = new StringBuilder ();
			bool errored = false;

			while (!endOfStream) {
				char c = readStream ();

				if (c == ScannerConstants.NEWLINE && !errored) {	// if the string spans multiple lines, report an error, but only once!
					notifyError (new StringLiteralError (token, ErrorConstants.LINEBREAK_IN_STR_LITERAL_MESSAGE));
					errored = true;
				} else if (escapeNextChar) {						// if this char is meant to be escaped, escape it
					escapeNextChar = false;

					switch (c) {
						case 'n':									// a newline char
							sb.Append ('\n');
							break;
						case 't':
							sb.Append ('\t');						// a tab char
							break;
						case 'r':
							sb.Append ('\r');						// a carriage return
							break;
						case '0':									// a null char
							break;
						default:
							sb.Append (c);							// otherwise the char itself
							break;
					}
				} else {
					escapeNextChar = c == ScannerConstants.ESCAPE_CHAR;
					stringEnded = c == ScannerConstants.STRING_DELIMITER;

					if (stringEnded) {								// the end of string, break
						break;
					}

					if (!escapeNextChar) {
						sb.Append (c);
					}
				}
			}

			// if we got out but the string never ended, we hit the end of file, not good, report an error
			if (!stringEnded) {
				notifyError (new StringLiteralError (token, ErrorConstants.EOF_WHILE_SCANNING_MESSAGE));
				token.Type = TokenType.ERROR;
			} else {
				// otherwise, save the value
				token.Value = sb.ToString ();
			}
		}

		/// <summary>
		/// Scans an completely invalid token
		/// </summary>
		/// <param name="t">T.</param>
		/// <param name="c">C.</param>
		private void parseErrorToken(Token t, char c)
		{
			notifyError(new InvalidIdentifierError (t)); // first report an error
			t.Type = TokenType.ERROR;

			StringBuilder sb = new StringBuilder (c.ToString());

			c = peekStream ();

			while (!isWhitespace (c) && c != ';') {		// then read the whole problem token
				sb.Append (readStream());
				c = peekStream ();
			}

			t.Value = sb.ToString ();					// save this for error messaging
		}

		/// <summary>
		/// A helper method to decide whether a token can be a part of a binary operation
		/// </summary>
		/// <returns><c>true</c>, if binary operand was ised, <c>false</c> otherwise.</returns>
		/// <param name="t">T.</param>
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
			return StringUtils.isAlpha (c) || StringUtils.isInteger (c) || c == ScannerConstants.UNDERSCORE;
		}
	}
}
