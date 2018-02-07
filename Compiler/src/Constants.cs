using System;
using System.Collections.Generic;

namespace MiniPLInterpreter
{
	public class Constants
	{
		public static readonly Dictionary<string, TokenType> RESERVED_SEQUENCES =	new Dictionary<string, TokenType> ()	
														{
															{"var", TokenType.DECLARATION},
															{"for", TokenType.FOR_LOOP},
															{"false", TokenType.BOOL_VAL},
															{"true", TokenType.BOOL_VAL},
															{"end", TokenType.END},
															{"in", TokenType.RANGE},
															{"int", TokenType.INT_VAR},
															{"do", TokenType.LOOP_BODY},
															{"read", TokenType.INPUT},
															{"print", TokenType.OUTPUT},
															{"string", TokenType.STR_VAR},
															{"bool", TokenType.BOOL_VAR},
															{"assert", TokenType.ASSERT}
														};

		public static readonly Dictionary<string, TokenType> INDEPENDENT_CHARS = new Dictionary<string, TokenType> ()	
														{
															{"+", TokenType.BINARY_OP}, 
															{"-", TokenType.BINARY_OP},
															{"<", TokenType.BINARY_OP},
															{"=", TokenType.BINARY_OP},
															{"&", TokenType.BINARY_OP}, 
															{"!", TokenType.UNARY_OP}, 
															{";", TokenType.END_STATEMENT}, 
															{"(", TokenType.EXPR_START}, 
															{")", TokenType.EXPR_END},
															{"/", TokenType.BINARY_OP},
															{"*", TokenType.BINARY_OP},
															{":", TokenType.BINARY_OP},
															{"\n", TokenType.LINEBREAK}
														};

		public static readonly Dictionary<string, TokenType> SUCCESSOR_DEPENDENT = new Dictionary<string, TokenType> ()
														{
															{":=", TokenType.DECLARATION},
															{"..", TokenType.UPTO}
														};

		public static readonly Dictionary<string, string> COMMENT_DELIMITERS = new Dictionary<string, string>()
														{
															{"//", "\n"},
															{"/*", "*/"}
														};

		public static readonly Dictionary<string, TokenType>[] ALL_SEQUENCES = 
														{
															INDEPENDENT_CHARS, 
															SUCCESSOR_DEPENDENT, 
															RESERVED_SEQUENCES
														};

		public static readonly char ESCAPE_CHAR = '\\';
		public static readonly char STRING_DELIMITER = '"';
		public static readonly char LINEBREAK = '\n';
		public static readonly char COMMENT_START_CHAR = '/';
		public static readonly char UTF8_SMALL_LETTERS_START = (char)0x41;
		public static readonly char UTF8_SMALL_LETTERS_END = (char)0x5a;
		public static readonly char UTF8_CAPITAL_LETTERS_START = (char)0x61;
		public static readonly char UTF8_CAPITAL_LETTERS_END = (char)0x7a;
		public static readonly char UTF8_LOWLINE = (char)0x5f;
		public static readonly char UTF8_NUMERIC_START = (char)0x30;
		public static readonly char UTF8_NUMERIC_END = (char)0x39;

		public static readonly string SCANNER_ERROR_TITLE = "Scanner error";
		public static readonly string STRING_LITERAL_ERROR_MESSAGE = "Error while scanning string literal";
		public static readonly string ID_ERROR_MESSAGE = "Error while scanning variable identifier";

		public static readonly Dictionary<char, char> UTF8_VALID_ID_CHAR_RANGES = new Dictionary<char, char> ()
														{
															{UTF8_SMALL_LETTERS_START, UTF8_SMALL_LETTERS_END},
															{UTF8_CAPITAL_LETTERS_START, UTF8_CAPITAL_LETTERS_END},
															{UTF8_LOWLINE, UTF8_LOWLINE},
															{UTF8_NUMERIC_START, UTF8_NUMERIC_END}
														};
	}
}