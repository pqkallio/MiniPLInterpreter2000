using System;
using System.Collections.Generic;

namespace MiniPLInterpreter
{
	public class Constants
	{
		public static readonly bool UNIX = Environment.OSVersion.ToString ().ToLower ().StartsWith ("unix"); 
		public static readonly Dictionary<char, string> WHITESPACES = new Dictionary<char, string> ()
		{
			{' ', null},
			{'\t', null}
		};

		public static readonly Dictionary<string, TokenType> RESERVED_SEQUENCES = new Dictionary<string, TokenType> ()	
		{
			{"var", TokenType.DECLARATION},
			{"for", TokenType.FOR_LOOP},
			{"false", TokenType.BOOL_VAL},
			{"true", TokenType.BOOL_VAL},
			{"end", TokenType.END_OF_BLOCK},
			{"in", TokenType.RANGE_FROM},
			{"int", TokenType.INT_VAR},
			{"do", TokenType.START_BLOCK},
			{"read", TokenType.READ},
			{"print", TokenType.PRINT},
			{"string", TokenType.STR_VAR},
			{"bool", TokenType.BOOL_VAR},
			{"assert", TokenType.ASSERT}
		};

		public static readonly Dictionary<char, TokenType> INDEPENDENT_CHARS = new Dictionary<char, TokenType> ()	
		{
			{'<', TokenType.BINARY_OP_LOG_LT},
			{'=', TokenType.BINARY_OP_LOG_EQ},
			{'&', TokenType.BINARY_OP_LOG_AND}, 
			{'!', TokenType.UNARY_OP_LOG_NEG}, 
			{';', TokenType.END_STATEMENT}, 
			{'(', TokenType.PARENTHESIS_LEFT}, 
			{')', TokenType.PARENTHESIS_RIGHT},
			{'*', TokenType.BINARY_OP_MUL}
		};

		public static readonly Dictionary<TokenType, string> TOKEN_TYPE_STRINGS = new Dictionary<TokenType, string> () 
		{
			{TokenType.DECLARATION, "var"},
			{TokenType.FOR_LOOP, "for"},
			{TokenType.END_OF_BLOCK, "end"},
			{TokenType.RANGE_FROM, "in"},
			{TokenType.INT_VAR, "int"},
			{TokenType.START_BLOCK, "do"},
			{TokenType.READ, "read"},
			{TokenType.PRINT, "print"},
			{TokenType.STR_VAR, "string"},
			{TokenType.BOOL_VAR, "bool"},
			{TokenType.ASSERT, "assert"},
			{TokenType.BINARY_OP_LOG_LT, "<"},
			{TokenType.BINARY_OP_LOG_EQ, "="},
			{TokenType.BINARY_OP_LOG_AND, "&"},
			{TokenType.UNARY_OP_LOG_NEG, "!"},
			{TokenType.END_STATEMENT, ";"},
			{TokenType.PARENTHESIS_LEFT, "("},
			{TokenType.PARENTHESIS_RIGHT, ")"},
			{TokenType.BINARY_OP_MUL, "*"},
			{TokenType.BINARY_OP_ADD, "+"},
			{TokenType.BINARY_OP_SUB, "-"},
			{TokenType.BINARY_OP_DIV, "/"},
			{TokenType.SET_TYPE, ":"},
			{TokenType.ASSIGN, ":="},
			{TokenType.ID, "identifier"},
			{TokenType.END_OF_FILE, "end of file"},
			{TokenType.UNDEFINED, "undefined"}
		};

		public static readonly Tuple<char, TokenType> BINARY_OP_ADD = Tuple.Create('+', TokenType.BINARY_OP_ADD);
		public static readonly Tuple<char, TokenType> BINARY_OP_SUB = Tuple.Create('-', TokenType.BINARY_OP_SUB);
		public static readonly Tuple<char, TokenType> BINARY_OP_DIV = Tuple.Create('/', TokenType.BINARY_OP_DIV);
		public static readonly Tuple<char, TokenType> SET_TYPE = Tuple.Create(':', TokenType.SET_TYPE);
		public static readonly Tuple<char, TokenType> ASSIGN = Tuple.Create('=', TokenType.ASSIGN);

		public static readonly char ESCAPE_CHAR = '\\';
		public static readonly char STRING_DELIMITER = '"';
		public static readonly char NULL_CHAR = '\0';
		public static readonly string LINEBREAK = UNIX ? "\n" : "\r\n";
		public static readonly char NEWLINE = '\n';
		public static readonly char COMMENT_START_CHAR = '/';
		public static readonly char UTF8_SMALL_LETTERS_START = (char)0x41;
		public static readonly char UTF8_SMALL_LETTERS_END = (char)0x5a;
		public static readonly char UTF8_CAPITAL_LETTERS_START = (char)0x61;
		public static readonly char UTF8_CAPITAL_LETTERS_END = (char)0x7a;
		public static readonly char UTF8_LOWLINE = (char)0x5f;
		public static readonly char UTF8_NUMERIC_START = (char)0x30;
		public static readonly char UTF8_NUMERIC_END = (char)0x39;

		public static readonly string ASSERTION_FAILED = "Assertion failed: ";
		public static readonly Tuple<char, char> PRINT_ROW_AND_COLUMN_PARENTHESES = Tuple.Create('[', ']');
		public static readonly string PRINT_ROW = "Line: ";
		public static readonly string PRINT_COL = "Column: ";
		public static readonly string PRINT_ROW_COL_DELIMITER = "; ";

		public static readonly Dictionary<char, char> UTF8_VALID_ID_CHAR_RANGES = new Dictionary<char, char> ()
		{
			{UTF8_SMALL_LETTERS_START, UTF8_SMALL_LETTERS_END},
			{UTF8_CAPITAL_LETTERS_START, UTF8_CAPITAL_LETTERS_END},
			{UTF8_LOWLINE, UTF8_LOWLINE},
			{UTF8_NUMERIC_START, UTF8_NUMERIC_END}
		};

		public static readonly Dictionary<TokenType, Dictionary<TokenType, string>> LEGIT_OPERATIONS = new Dictionary<TokenType, Dictionary<TokenType, string>> ()
		{
			{TokenType.INT_VAL, 
				new Dictionary<TokenType, string> () 
				{
					{TokenType.BINARY_OP_ADD, null},
					{TokenType.BINARY_OP_DIV, null},
					{TokenType.BINARY_OP_MUL, null},
					{TokenType.BINARY_OP_NO_OP, null},
					{TokenType.BINARY_OP_SUB, null},
					{TokenType.BINARY_OP_LOG_EQ, null},
					{TokenType.BINARY_OP_LOG_LT, null},
					{TokenType.RANGE_FROM, null},
					{TokenType.RANGE_UPTO, null}
				}
			},
			{TokenType.STR_VAL, 
				new Dictionary<TokenType, string> () 
				{
					{TokenType.BINARY_OP_ADD, null},
					{TokenType.BINARY_OP_NO_OP, null},
					{TokenType.BINARY_OP_LOG_EQ, null},
					{TokenType.BINARY_OP_LOG_LT, null}
				}
			},
			{TokenType.BOOL_VAL, 
				new Dictionary<TokenType, string> () 
				{
					{TokenType.BINARY_OP_NO_OP, null},
					{TokenType.BINARY_OP_LOG_EQ, null},
					{TokenType.BINARY_OP_LOG_LT, null},
					{TokenType.BINARY_OP_LOG_AND, null},
					{TokenType.UNARY_OP_LOG_NEG, null}
				}
			}
		};

		public static readonly Dictionary<TokenType, string> LOGICAL_OPERATIONS = new Dictionary<TokenType, string> ()
		{
			{TokenType.BINARY_OP_LOG_AND, null},
			{TokenType.BINARY_OP_LOG_EQ, null},
			{TokenType.BINARY_OP_LOG_LT, null},
			{TokenType.UNARY_OP_LOG_NEG, null}
		};

		public static readonly Dictionary<TokenType, string> STATEMENT_FASTFORWARD_TO = new Dictionary<TokenType, string> () 
		{
			{TokenType.END_STATEMENT, null},
			{TokenType.END_OF_FILE, null}
		};

		public static readonly Dictionary<TokenType, string> BLOCK_DEF_FASTFORWARD_TO = new Dictionary<TokenType, string> () 
		{
			{TokenType.START_BLOCK, null},
			{TokenType.END_STATEMENT, null},
			{TokenType.END_OF_FILE, null}
		};

		public static readonly bool DEFAULT_BOOL_VALUE = false;
		public static readonly int DEFAULT_INTEGER_VALUE = 0;
		public static readonly string DEFAULT_STRING_VALUE = "";

		public static readonly string IDENTIFIER_STR = "identifier";
		public static readonly string EOF_STR = "end of file";
		public static readonly string OPERAND_STR = "operand literal";
		public static readonly string EXPRESSION_STR = "expression";
		public static readonly string END_OF_EXPR_STR = "end of expression";
		public static readonly string OPERATION_STR = "operation symbol";
		public static readonly string INT_VAL_STR = "integer";
		public static readonly string STR_VAL_STR = "string";
		public static readonly string BOOL_VAL_STR = "boolean";

		// Expectation sets
		public static readonly string[] EXPECTATION_SET_PROGRAM = new string[] 
		{
			TOKEN_TYPE_STRINGS[TokenType.DECLARATION],
			TOKEN_TYPE_STRINGS[TokenType.FOR_LOOP],
			TOKEN_TYPE_STRINGS[TokenType.READ],
			TOKEN_TYPE_STRINGS[TokenType.PRINT],
			TOKEN_TYPE_STRINGS[TokenType.ASSERT],
			IDENTIFIER_STR,
			EOF_STR
		};

		public static readonly string[] EXPECTATION_SET_STATEMENTS = new string[] 
		{
			TOKEN_TYPE_STRINGS[TokenType.DECLARATION],
			TOKEN_TYPE_STRINGS[TokenType.FOR_LOOP],
			TOKEN_TYPE_STRINGS[TokenType.READ],
			TOKEN_TYPE_STRINGS[TokenType.PRINT],
			TOKEN_TYPE_STRINGS[TokenType.ASSERT],
			IDENTIFIER_STR,
			TOKEN_TYPE_STRINGS[TokenType.END_OF_FILE]
		};

		public static readonly string[] EXPECTATION_SET_STATEMENT = new string[] 
		{
			TOKEN_TYPE_STRINGS[TokenType.DECLARATION],
			TOKEN_TYPE_STRINGS[TokenType.FOR_LOOP],
			TOKEN_TYPE_STRINGS[TokenType.READ],
			TOKEN_TYPE_STRINGS[TokenType.PRINT],
			TOKEN_TYPE_STRINGS[TokenType.ASSERT],
			IDENTIFIER_STR
		};

		public static readonly string[] EXPECTATION_SET_EOF = new string[] 
		{
			TOKEN_TYPE_STRINGS[TokenType.END_OF_FILE]
		};

		public static readonly string[] EXPECTATION_SET_DECLARATION_TYPE = new string[] 
		{
			TOKEN_TYPE_STRINGS[TokenType.STR_VAR],
			TOKEN_TYPE_STRINGS[TokenType.INT_VAR],
			TOKEN_TYPE_STRINGS[TokenType.BOOL_VAR]
		};

		public static readonly string[] EXPECTATION_SET_ASSIGN = new string[] 
		{
			TOKEN_TYPE_STRINGS[TokenType.ASSIGN],
			TOKEN_TYPE_STRINGS[TokenType.END_STATEMENT]
		};

		public static readonly string[] EXPECTATION_SET_EXPRESSION = new string[] 
		{
			EXPRESSION_STR
		};

		public static readonly string[] EXPECTATION_SET_BINOP = new string[]
		{
			OPERAND_STR,
			END_OF_EXPR_STR
		};

		public static readonly string[] EXPECTATION_SET_ID_VAL = new string[]
		{
			INT_VAL_STR,
			STR_VAL_STR,
			BOOL_VAL_STR
		};
	}
}