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
		private Dictionary<char, List<IScannerRule>> scannerRules;
		private IScannerRule defaultRule;
		private List<Error> errors;
		private StreamReader inputStream;
		private int col = 0;
		private int row = 1;

		public Scanner (StreamReader inputStream)
		{
			this.inputStream = inputStream;
			this.errors = new List<Error> ();
			initScannerRules ();
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

		public Token getNextToken()
		{
			Token token = null;

			while (!inputStream.EndOfStream) {
				char c = readStream();
				if (c == Constants.LINEBREAK || Constants.WHITESPACES.ContainsKey (c)) {
					continue;
				} else {
					token = new Token (row, col);
					if (Constants.INDEPENDENT_CHARS.ContainsKey (c)) {
						token.Type = Constants.INDEPENDENT_CHARS [c];
					} else if (c == Constants.SET_TYPE.Item1) {
						if (peekStream() == Constants.ASSIGN.Item1) {
							readStream();
							token.Type = Constants.ASSIGN.Item2;
						} else {
							token.Type = Constants.SET_TYPE.Item2;
						}
					} else if (c == '.') {
						if (peekStream() == '.') {
							readStream();
							token.Type = TokenType.RANGE_UPTO;
						} else {
							token.Type = TokenType.ERROR;
						}
					} else if (c == Constants.BINARY_OP_ADD.Item1 || c == Constants.BINARY_OP_SUB.Item1) {
						token.Type = c == Constants.BINARY_OP_ADD.Item1 ? Constants.BINARY_OP_ADD.Item2 : Constants.BINARY_OP_SUB.Item2;

						if (StringUtils.isInteger (peekStream())) {
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
					} else if (c == Constants.STRING_DELIMITER) {
						token.Type = TokenType.STR_VAL;
						bool escapeNextChar = false;
						bool stringEnded = false;
						StringBuilder sb = new StringBuilder ();

						while (!inputStream.EndOfStream) {
							c = readStream ();

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
					} else if (StringUtils.isInteger (c)) {
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
					} else if (c == Constants.BINARY_OP_DIV.Item1) {
						token.Type = Constants.BINARY_OP_DIV.Item2;
						if (peekStream () == Constants.BINARY_OP_DIV.Item1) {
							token = null;
							while (!inputStream.EndOfStream) {
								c = readStream ();
								if (c == Constants.LINEBREAK) {
									break;
								}
							}
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
							token.Type = Constants.RESERVED_SEQUENCES [val];
						} else {
							token.Value = val;
						}
					} else {
						notifyError (new TokenError (token));
						token.Type = TokenType.ERROR;
					}

					if (token != null) {
						break;
					}
				}
			}

			if (token == null) {
				token = new Token (row, col, "", TokenType.END_OF_FILE);
			}

			Console.WriteLine (token);

			return token;
		}
		/*
		public List<Token> tokenize(string input)
		{
			// List<Token> tokens = parseTokens (input);
			// setTypes (tokens);

			// tokens.Add (new Token (0, 0, "$$", TokenType.END_OF_FILE));

			return tokens;
		}

		private List<Token> parseTokens(string input) {
			List<Token> tokens = new List<Token> ();

			for (int i = 0; i < input.Length; i++) {
				char c = input [i];

				if (input [i] == Constants.LINEBREAK) {
					row++;
					col = 0;
					continue;
				} else if (Constants.WHITESPACES.ContainsKey (input [i])) {
					col++;
					continue;
				} else {
					Token token = new Token (row, col);
					if (Constants.INDEPENDENT_CHARS.ContainsKey (c)) {
						token.Value = c.ToString ();
						token.Type = Constants.INDEPENDENT_CHARS [c];
					} else if (c == Constants.SET_TYPE.Item1) {
						if (input [i + 1] == Constants.ASSIGN.Item1) {
							token.Value = ":=";
							token.Type = Constants.ASSIGN.Item2;
							i++;
							col += 2;
						} else {
							token.Value = Constants.SET_TYPE.Item1.ToString ();
							token.Type = Constants.SET_TYPE.Item2;
							col++;
						}
					} else if (c == '.') {
						if (input [i + 1] == '.') {
							token.Value = "..";
							token.Type = TokenType.RANGE_UPTO;
							i++;
							col += 2;
						}
					} else if (c == Constants.BINARY_OP_ADD.Item1 || c == Constants.BINARY_OP_SUB.Item1) {
						token.Value = c == Constants.BINARY_OP_ADD.Item1 ? Constants.BINARY_OP_ADD.Item1.ToString () : Constants.BINARY_OP_SUB.Item1.ToString ();
						token.Type = c == Constants.BINARY_OP_ADD.Item1 ? Constants.BINARY_OP_ADD.Item2 : Constants.BINARY_OP_SUB.Item2;
						col++;

						if (StringUtils.isInteger (input [i + 1])) {
							i++;
							col++;
							token.Type = TokenType.INT_VAL;
							for (; i < input.Length; i++) {
								c = input [i];
								if (StringUtils.isInteger (c)) {
									token.AppendToValue (c);
									col++;
								} else if (c == Constants.BINARY_OP_ADD.Item1 || c == Constants.BINARY_OP_SUB.Item1) {
									tokens.Add (token);
									token = new Token (row, col, c.ToString(), c == Constants.BINARY_OP_ADD.Item1 ? Constants.BINARY_OP_ADD.Item2 : Constants.BINARY_OP_SUB.Item2);
									break;
								} else {
									i--;
									break;
								}
							}
						}
					} else if (c == Constants.STRING_DELIMITER) {
						token.TokenType = TokenType.STR_VAL;
						bool escapeNextChar = false;
						bool stringEnded = false;

						for (i = i + 1; i < input.Length; i++) {
							col++;
							c = input [i];

							if (c == Constants.LINEBREAK) {
								notifyError (new StringLiteralError (token, "String literal mustn't span multiple lines"));
							} else if (escapeNextChar) {
								escapeNextChar = false;

								switch (c) {
								case 'n':
									token.AppendToValue ('\n');
									break;
								case 't':
									token.AppendToValue ('\t');
									break;
								// and so on until all the cases are covered...
								default:
									token.AppendToValue (c);
									break;
								}
							} else {
								escapeNextChar = c == Constants.ESCAPE_CHAR;
								stringEnded = c == Constants.STRING_DELIMITER;

								if (stringEnded) {
									col++;
									break;
								}

								if (!escapeNextChar) {
									token.AppendToValue (c);
								}
							}
						}

						if (!stringEnded) {
							notifyError (new StringLiteralError (token, "EOF encountered while parsing string literal"));
						}
					} else if (StringUtils.isInteger (c)) {
						token.TokenType = TokenType.INT_VAL;
						for (; i < input.Length; i++) {
							c = input [i];
							if (StringUtils.isInteger (c)) {
								col++;
								token.AppendToValue (c);
							} else if (c == Constants.BINARY_OP_ADD.Item1 || c == Constants.BINARY_OP_SUB.Item1) {
								tokens.Add (token);
								token = new Token (row, col, c.ToString(), c == Constants.BINARY_OP_ADD.Item1 ? Constants.BINARY_OP_ADD.Item2 : Constants.BINARY_OP_SUB.Item2);
								break;
							} else {
								i--;
								break;
							}
						}
					} else if (c == Constants.BINARY_OP_DIV.Item1) {
						token.TokenType = Constants.BINARY_OP_DIV.Item2;
						col++;
						if (input [i + 1] == Constants.BINARY_OP_DIV.Item1) {
							token = null;
							col += 2;
							for (i = i + 2; i < input.Length; i++) {
								c = input [i];
								if (c == Constants.LINEBREAK) {
									i--;
									break;
								} else {
									col++;
								}
							}
						} else if (input [i + 1] == '*') {
							bool starHit = false;
							token = null;
							col += 2;
							for (i = i + 2; i < input.Length; i++) {
								c = input [i];
								if (c == Constants.LINEBREAK) {
									starHit = false;
									row++;
								} else if (c == '*') {
									starHit = true;
									col++;
								} else if (c == '/' && starHit) {
									col++;
									break;
								} else {
									starHit = false;
									col++;
								}
							}
						}
					} else if (StringUtils.isAlpha(c)) {
						token.AppendToValue (c);
						token.TokenType = TokenType.ID;
						col++;
						for (i = i + 1; i < input.Length; i++) {

							c = input [i];
							if (StringUtils.isAlpha (c) || StringUtils.isInteger (c) || c == '_') {
								col++;
								token.AppendToValue (c);
							} else if (c == Constants.BINARY_OP_ADD.Item1 || c == Constants.BINARY_OP_SUB.Item1) {
								tokens.Add (token);
								token = new Token (row, col, c.ToString(), c == Constants.BINARY_OP_ADD.Item1 ? Constants.BINARY_OP_ADD.Item2 : Constants.BINARY_OP_SUB.Item2);
								break;
							} else {
								i--;
								break;
							}
						}
						if (Constants.RESERVED_SEQUENCES.ContainsKey (token.Value)) {
							token.TokenType = Constants.RESERVED_SEQUENCES [token.Value];
						}
					} else {
						notifyError (new TokenError (token));
						token = null;
					}

					if (token != null) {
						tokens.Add (token);
					}
				}
			}

			return tokens;
		}

		private List<Token> OldparseTokens(string input)
		{
			List<Token> tokens = new List<Token> ();
			string temp = "";
			int row = 0;
			bool tokenScanned = false;

			for (int i = 0; i < input.Length; i++) {
				if (input [i] == Constants.LINEBREAK) {
					row++;
				} else {
					tokenScanned = scan (input, ref temp, ref i);

					if (tokenScanned && !String.IsNullOrEmpty(temp)) {
						tokens.Add (new Token(row, i, String.Copy(temp)));
					}

					temp = "";
					tokenScanned = false;
				}
			}

			return tokens;
		}

		private void setTypes(List<Token> tokens)
		{
			foreach (Token token in tokens) {
				setTokenType (token);

				if (token.TokenType == TokenType.UNDEFINED) {
					defineTokenType (token);
				}
			}
		}

		private void defineTokenType(Token token)
		{
			string value = token.Value;

			if (StringUtils.isInteger (value)) {
				token.TokenType = TokenType.INT_VAL;
			} else if (value[0] == Constants.STRING_DELIMITER) {
				token.TokenType = TokenType.STR_VAL;

				bool wellFormed = StringUtils.delimited (value, Constants.STRING_DELIMITER);

				if (!wellFormed) {
					errors.Add (new StringLiteralError (token));
				}
			} else {
				bool wellFormedId = StringUtils.validId (value);

				if (!wellFormedId) {
					errors.Add (new TokenError (token));
					token.TokenType = TokenType.UNDEFINED;
				} else {
					token.TokenType = TokenType.ID;
				}
			}
		}

		private void setTokenType(Token token)
		{
			string value = token.Value;

			foreach (Dictionary<string, TokenType> dict in Constants.ALL_SEQUENCES) {
				if (dict.ContainsKey (value)) {
					token.TokenType = dict [value];
					return;
				}
			}

			token.TokenType = TokenType.UNDEFINED;
		}

		private bool scan(string input, ref string temp, ref int index)
		{
			char c = input [index];

			if (Char.IsWhiteSpace (c)) {
				return false;
			}

			bool tokenParsed = false;

			if (this.scannerRules.ContainsKey (c)) {
				tokenParsed = scanRules (scannerRules [c], input, ref temp, ref index);
			} 

			if (!tokenParsed) {
				tokenParsed = this.defaultRule.scanToken (input, ref temp, ref index);
			}

			return tokenParsed;
		}
		*/

		private bool scanRules(List<IScannerRule> rules, string input, ref string temp, ref int index) {
			bool parsed = false;

			foreach (IScannerRule rule in rules) {
				parsed = rule.scanToken (input, ref temp, ref index);
				if (parsed) {
					return true;
				}
			}

			return false;
		}

		private void initScannerRules()
		{
			this.defaultRule = new DefaultScannerRule ();
			this.scannerRules = new Dictionary<char, List<IScannerRule>>();

			addRule (Constants.COMMENT_START_CHAR, new CommentRule ());

			addReservedSequenceRules ();

			addSuccessorDependentRules ();

			addRule (Constants.STRING_DELIMITER, new StringLiteralRule ());

			IScannerRule independentCharRule = new IndependentCharRule ();
			//addRules (independentCharRule, Enumerable.ToList(Constants.INDEPENDENT_CHARS.Keys).ToArray());
		}

		private void addReservedSequenceRules()
		{
			foreach (string sequence in Constants.RESERVED_SEQUENCES.Keys) {
				if (!String.IsNullOrEmpty (sequence)) {
					char key = sequence [0];

					addRule(key, new ReservedSequenceRule (sequence));
				}
			}
		}

		private void addSuccessorDependentRules ()
		{
			foreach (string boundString in Constants.SUCCESSOR_DEPENDENT.Keys) {
				char key = boundString [0];

				addRule(key, new SuccessorDependentRule (boundString));
			}
		}

		private void addRules(IScannerRule rule, string[] strs)
		{
			foreach (string str in strs) {
				if (!String.IsNullOrEmpty(str)) {
					char key = str [0];

					addRule (key, rule);
				}
			}
		}

		private void addRule(char key, IScannerRule rule)
		{
			if (!this.scannerRules.ContainsKey (key)) {
				this.scannerRules [key] = new List<IScannerRule> ();
			}

			this.scannerRules[key].Add(rule);
		}

		public void notifyError(Error error)
		{
			this.errors.Add (error);
		}

		public List<Error> getErrors () {
			return this.errors;
		}
	}
}
