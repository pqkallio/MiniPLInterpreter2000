using System;
using System.Linq;
using System.Collections.Generic;

namespace MiniPLInterpreter
{
	/// <summary>
	/// Scanner class is responsible for the scanning phase.
	/// It scans through the original source code and divides it into string tokens.
	/// These tokens are then passed to the Parser.
	/// </summary>
	public class Scanner
	{
		private Dictionary<char, List<ScannerRule>> scannerRules;
		private ScannerRule defaultRule;

		public Scanner ()
		{
			initScannerRules ();
		}

		public List<Token> tokenize(string input)
		{
			List<Token> tokens = parseTokens (input);
			setTypes (tokens);

			return tokens;
		}

		private List<Token> parseTokens(string input)
		{
			List<Token> tokens = new List<Token> ();
			string temp = "";
			int row = 0;
			bool tokenScanned = false;

			for (int i = 0; i < input.Length; i++) {
				if (input [i] == Constants.LINEBREAK) {
					tokens.Add (new Token (row, i, Constants.LINEBREAK.ToString ()));
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

				if (token.Type == TokenType.UNDEFINED) {
					defineTokenType (token);
				}
			}
		}

		private void defineTokenType(Token token)
		{
			string value = token.Value;

			if (StringUtils.isNumeric (value)) {
				token.Type = TokenType.INT_VAL;
			} else if (StringUtils.delimited (value, Constants.STRING_DELIMITER)) {
				token.Type = TokenType.STR_VAL;
			} else {
				token.Type = TokenType.ID;
			}
		}

		private void setTokenType(Token token)
		{
			string value = token.Value;

			foreach (Dictionary<string, TokenType> dict in Constants.ALL_SEQUENCES) {
				if (dict.ContainsKey (value)) {
					token.Type = dict [value];
					return;
				}
			}

			token.Type = TokenType.UNDEFINED;
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

		private bool scanRules(List<ScannerRule> rules, string input, ref string temp, ref int index) {
			bool parsed = false;

			foreach (ScannerRule rule in rules) {
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
			this.scannerRules = new Dictionary<char, List<ScannerRule>>();

			addRule (Constants.COMMENT_START_CHAR, new CommentRule ());

			addReservedSequenceRules ();

			addSuccessorDependentRules ();

			addRule (Constants.STRING_DELIMITER, new StringLiteralRule ());

			ScannerRule independentCharRule = new IndependentCharRule ();
			addRules (independentCharRule, Enumerable.ToList(Constants.INDEPENDENT_CHARS.Keys).ToArray());
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

		private void addRules(ScannerRule rule, string[] strs)
		{
			foreach (string str in strs) {
				if (!String.IsNullOrEmpty(str)) {
					char key = str [0];

					addRule (key, rule);
				}
			}
		}

		private void addRule(char key, ScannerRule rule)
		{
			if (!this.scannerRules.ContainsKey (key)) {
				this.scannerRules [key] = new List<ScannerRule> ();
			}

			this.scannerRules[key].Add(rule);
		}
	}
}
