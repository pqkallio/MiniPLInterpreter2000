using System;

namespace MiniPLInterpreter
{
	public class ReservedSequenceRule : IScannerRule
	{
		private readonly string sequence;

		public ReservedSequenceRule (string sequence)
		{
			this.sequence = sequence;
		}

		public bool scanToken(string input, ref string token, ref int index)
		{
			int i, j;
			for (i = index, j = 0; j < sequence.Length; i++, j++) {
				if (input [i] != sequence [j]) {
					return false;
				}
			}

			if (!Char.IsWhiteSpace (input [i])) {
				return false;
			}

			token = String.Copy(this.sequence);
			index = i;

			return true;
		}
	}
}

