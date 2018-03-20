using System;
using System.Collections.Generic;
using System.Text;

namespace MiniPLInterpreter
{
	public class StringUtils
	{
		public static bool isAlpha (char c)
		{
			return (c >= Constants.UTF8_CAPITAL_LETTERS_START && c <= Constants.UTF8_CAPITAL_LETTERS_END) ||
				(c >= Constants.UTF8_SMALL_LETTERS_START && c <= Constants.UTF8_SMALL_LETTERS_END);
		}

		public static bool isInteger (char c)
		{
			return c >= 0x30 && c <= 0x39;
		}

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

		public static bool parseToBoolean (string str)
		{
			if (String.IsNullOrEmpty (str)) {
				throw new ArgumentException ();
			}

			if (str.Equals ("true")) {
				return true;
			}

			if (str.Equals ("false")) {
				return false;
			}

			throw new ArgumentException ();
		}

		public static bool sequenceMatch (string input, int index, string sequence)
		{
			if (input == null || sequence == null) {
				throw new ArgumentNullException ();
			}

			if (input == "" && sequence == "") {
				return true;
			}

			if (index < 0 || index >= input.Length) {
				throw new ArgumentOutOfRangeException ();
			}

			int i, j;

			for (i = index, j = 0; j < sequence.Length && i < input.Length; i++, j++) {
				if (i >= input.Length || input [i] != sequence [j]) {
					return false;
				}
			}

			if (j != sequence.Length) {
				return false;
			}

			return true;
		}

		public static bool delimited (string input, char delimiter)
		{
			if (input == null) {
				throw new ArgumentNullException ();
			}

			if (input == "") {
				return false;
			}

			return input [0] == delimiter && input [input.Length - 1] == delimiter;
		}

		public static bool validId(string input)
		{
			if (input == null) {
				throw new ArgumentNullException ();
			}

			if (input == "") {
				return false;
			}

			char c = input [0];

			if (!(NumericUtils.IntBetween ((int)c, Constants.UTF8_SMALL_LETTERS_START, Constants.UTF8_SMALL_LETTERS_END) ||
				NumericUtils.IntBetween ((int)c, Constants.UTF8_CAPITAL_LETTERS_START, Constants.UTF8_CAPITAL_LETTERS_END))) {
				return false;
			}

			for (int i = 1; i < input.Length; i++) {
				c = input [i];
				bool valid = validIdChar (c);

				if (!valid) {
					return false;
				}
			}

			return true;
		}

		private static bool validIdChar(char c) {
			Dictionary<char, char> validChars = Constants.UTF8_VALID_ID_CHAR_RANGES;

			foreach (char key in validChars.Keys) {
				if (NumericUtils.IntBetween ((int)c, key, validChars [key])) {
					return true;
				}
			}

			return false;
		}

		public static object Evaluate (string leftOperand, string rightOperand, TokenType operation)
		{
			switch (operation) {
				case TokenType.BINARY_OP_ADD:
					return leftOperand + rightOperand;
				case TokenType.BINARY_OP_LOG_EQ:
				case TokenType.BINARY_OP_LOG_LT:
					return BooleanUtils.EvaluateBinOp (leftOperand, rightOperand, operation);
				default:
					throw new ArgumentException (String.Format ("operation {0} not defined for string values", operation));
			}
		}

		public static string IntToString (int value)
		{
			if (value == 0) {
				return "0";
			}

			bool neg = value < 0;
			int abs = neg ? -1 * value : value;

			StringBuilder sb = new StringBuilder ();

			while (abs > 0) {
				sb.Insert (0, abs % 10);
				abs /= 10;
			}

			if (neg) {
				sb.Insert (0, '-');
			}

			return sb.ToString ();
		}
	}
}

