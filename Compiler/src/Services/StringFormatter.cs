using System;
using System.Collections.Generic;

namespace MiniPLInterpreter
{
	public class StringFormatter
	{
		public StringFormatter ()
		{}

		public static string formatError (Error error, string[] sourceLines)
		{
			int numRow = error.Token.Row < sourceLines.Length ? error.Token.Row : sourceLines.Length - 1;
			int numCol = Math.Max(error.Token.Column, 0);

			string errorMessage = line(error.ToString () + ' ' + formatRowAndColumn(numRow, numCol));

			string sourceLine = sourceLines[numRow];
			errorMessage += line (sourceLine);

			for (int i = 0; i < numCol; i++) {
				errorMessage += ' ';
			}

			errorMessage += '^';

			return errorMessage;
		}

		public static string formatRuntimeException (RuntimeException exception, string[] sourceLines)
		{
			int numRow = exception.Token.Row;
			int numCol = exception.Token.Column;

			string sourceLine = sourceLines [numRow];
			string errorMessage = line (exception.ToString () + ' ' + formatRowAndColumn (numRow, numCol)) + line (sourceLine);
			return errorMessage;
		}

		public static string formatFailedAssertion (AssertNode assertNode, string[] sourceLines)
		{
			int numRow = assertNode.AssertStatementRow;
			int numStartCol = assertNode.AssertStatementStartCol;
			int lenAssertion = assertNode.AssertStatementEndCol - numStartCol;

			string assertion = StringFormattingConstants.ASSERTION_FAILED;
			assertion += sourceLines [numRow].Substring (numStartCol, lenAssertion) + ' ';
			assertion += line (formatRowAndColumn (numRow, numStartCol));

			return assertion;
		}

		public static string formatRowAndColumn (int numRow, int numCol)
		{
			Tuple<char, char> parentheses = StringFormattingConstants.PRINT_ROW_AND_COLUMN_PARENTHESES;

			string rowAndCol = parentheses.Item1 + StringFormattingConstants.PRINT_ROW + formatSourcePosition(numRow);
			rowAndCol += StringFormattingConstants.PRINT_ROW_COL_DELIMITER + StringFormattingConstants.PRINT_COL + formatSourcePosition(numCol);
			rowAndCol += parentheses.Item2;

			return rowAndCol;
		}

		public static string formatSourcePosition (int pos)
		{
			return StringUtils.IntToString (pos + 1);
		}

		public static string line (string str)
		{
			return str + StringFormattingConstants.LINEBREAK;
		}
	}
}