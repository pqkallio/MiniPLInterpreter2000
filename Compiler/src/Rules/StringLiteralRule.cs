using System;

namespace MiniPLInterpreter
{
	public class StringLiteralRule : IScannerRule
	{
		public StringLiteralRule ()
		{
		}

		public bool scanToken(string input, ref string token, ref int index)
		{
			bool escapeNextChar = false;
			bool stringEnded = false;

			token += input [index];
			int i;

			for (i = index + 1; i < input.Length; i++) {
				char c = input [i];

				parseChar (c, ref token, ref escapeNextChar, ref stringEnded);

				if (stringEnded) {
					break;
				}
			}

			index = i;

			return true;
		}

		private void parseChar(char c, ref string token, ref bool escapeNextChar, ref bool stringEnded)
		{
			if (escapeNextChar) {
				escapeNextChar = false;
			} else {
				escapeNextChar = c != '\n' && c == Constants.ESCAPE_CHAR;
				stringEnded = c == Constants.STRING_DELIMITER;
			}

			token += c;
		}
	}
}

