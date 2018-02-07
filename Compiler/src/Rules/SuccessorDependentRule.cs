using System;

namespace MiniPLInterpreter
{
	public class SuccessorDependentRule : IScannerRule
	{
		private readonly string sequence;

		public SuccessorDependentRule (string sequence)
		{
			this.sequence = sequence;
		}

		public bool scanToken(string input, ref string token, ref int index)
		{
			int i, j;

			if (input.Length < index + sequence.Length) {
				return false;
			}

			for (i = 0, j = index; i < sequence.Length; i++, j++) {
				if (input [j] != sequence [i]) {
					return false;
				}
			}

			token = String.Copy (this.sequence);
			index = index + this.sequence.Length - 1;

			return true;
		}
	}
}

