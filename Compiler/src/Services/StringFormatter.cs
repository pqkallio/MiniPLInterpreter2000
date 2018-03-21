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
			int numRow = error.Token.Row;
			int numCol = error.Token.Column;

			string sourceLine = sourceLines[numRow];
			string errorMessage = line (error.ToString () + ' ' + formatRowAndColumn(numRow, numCol)) + line (sourceLine);

			for (int i = 0; i < numCol; i++) {
				errorMessage += ' ';
			}

			errorMessage += '^';

			return errorMessage;
		}

		public static string formatFailedAssertion (AssertNode assertNode, string[] sourceLines)
		{
			int numRow = assertNode.AssertStatementRow;
			int numStartCol = assertNode.AssertStatementStartCol;
			int lenAssertion = assertNode.AssertStatementEndCol - numStartCol;

			string assertion = Constants.ASSERTION_FAILED;
			assertion += sourceLines [numRow].Substring (numStartCol, lenAssertion) + ' ';
			assertion += line (formatRowAndColumn (numRow, numStartCol));

			return assertion;
		}

		public static string formatRowAndColumn (int numRow, int numCol)
		{
			Tuple<char, char> parentheses = Constants.PRINT_ROW_AND_COLUMN_PARENTHESES;

			string rowAndCol = parentheses.Item1 + Constants.PRINT_ROW + formatSourcePosition(numRow);
			rowAndCol += Constants.PRINT_ROW_COL_DELIMITER + Constants.PRINT_COL + formatSourcePosition(numCol);
			rowAndCol += parentheses.Item2;

			return rowAndCol;
		}

		public static string formatSourcePosition (int pos)
		{
			return StringUtils.IntToString (pos + 1);
		}

		public static string line (string str)
		{
			return str + '\n';
		}
	}
}