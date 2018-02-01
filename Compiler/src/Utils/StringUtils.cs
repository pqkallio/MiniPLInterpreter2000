using System;

namespace MiniPLInterpreter
{
	public class StringUtils
	{
		public static bool isInteger (string str)
		{
			if (String.IsNullOrEmpty (str)) {
				return false;
			}

			if (str [0] != '-' && str [0] != '+' 
				&& !NumericUtils.IntBetween ((int) Char.GetNumericValue(str [0]), 0, 9)) {
				return false;
			}
			
			for (int i = 1; i < str.Length; i++) {
				int value = (int) Char.GetNumericValue (str[i]);

				if (!NumericUtils.IntBetween(value, 0, 9)) {
					return false;
				}
			}

			return true;
		}

		public static int parseToInt(string str)
		{
			if (String.IsNullOrEmpty(str) || !isInteger(str)) {
				throw new ArgumentException ();
			}

			bool negative = str [0] == '-';
			bool signed = negative | str [0] == '+';
			int downTo = signed ? 0 : -1;
			int coefficient = 1;
			int value = 0;

			for (int i = str.Length - 1; i > downTo; i--) {
				value += (int)Char.GetNumericValue (str [i]) * coefficient;
				coefficient *= 10;
			}

			return negative ? -1 * value : value;
		}

		public static bool sequenceMatch (string input, int index, string sequence)
		{
			if (input == null) {
				throw new ArgumentNullException ();
			}

			int i, j;

			for (i = index, j = 0; j < sequence.Length && i < input.Length; i++, j++) {
				if (input [i] != sequence [j]) {
					return false;
				}
			}

			return true;
		}

		public static bool delimited (string input, char delimiter)
		{
			return input [0] == delimiter && input [input.Length - 1] == delimiter;
		}
	}
}

