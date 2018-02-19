using System;
using System.Collections.Generic;

namespace MiniPLInterpreter
{
	public class DefaultScannerRule : IScannerRule
	{
		private readonly Dictionary<char, string> escapeChars = new Dictionary<char, string> ();

		public DefaultScannerRule ()
		{
			initEscapeChars ();
		}

		public bool scanToken(string input, ref string token, ref int index)
		{
			int next = index;

			for (int i = index; i < input.Length; i++) {
				char c = input [i];

				if (!Char.IsWhiteSpace (c) && !this.escapeChars.ContainsKey(c)) {
					token += c;
					next++;
				} else {
					break;
				}
			}

			index = next - 1;

			return true;
		}

		private void initEscapeChars()
		{
			/*
			foreach (string s in Constants.INDEPENDENT_CHARS.Keys) {
				this.escapeChars [s [0]] = null;
			}

			foreach (string s in Constants.SUCCESSOR_DEPENDENT.Keys) {
				this.escapeChars [s [0]] = null;
			}
			*/
		}
	}
}

