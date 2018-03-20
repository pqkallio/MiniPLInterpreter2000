using System;
using System.Collections.Generic;

namespace MiniPLInterpreter
{
	public class Constants
	{
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

		public static readonly Dictionary<TokenType, string> tokenTypeStrings = new Dictionary<TokenType, string> () 
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
														};

		public static readonly Tuple<char, TokenType> BINARY_OP_ADD = Tuple.Create('+', TokenType.BINARY_OP_ADD);
		public static readonly Tuple<char, TokenType> BINARY_OP_SUB = Tuple.Create('-', TokenType.BINARY_OP_SUB);
		public static readonly Tuple<char, TokenType> BINARY_OP_DIV = Tuple.Create('/', TokenType.BINARY_OP_DIV);
		public static readonly Tuple<char, TokenType> SET_TYPE = Tuple.Create(':', TokenType.SET_TYPE);
		public static readonly Tuple<char, TokenType> ASSIGN = Tuple.Create('=', TokenType.ASSIGN);

		/*
		public static readonly Dictionary<string, TokenType> SUCCESSOR_DEPENDENT = new Dictionary<string, TokenType> ()
														{
															{":=", TokenType.ASSIGN},
															{"..", TokenType.RANGE_UPTO}
														};

		public static readonly Dictionary<string, string> COMMENT_DELIMITERS = new Dictionary<string, string>()
														{
															{"//", "\n"},
															{"/*", "*\/"}
														};

		public static readonly Dictionary<string, TokenType>[] ALL_SEQUENCES = 
														{
															//INDEPENDENT_CHARS, 
															SUCCESSOR_DEPENDENT, 
															RESERVED_SEQUENCES
														};
		*/

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
		public static readonly string TOKEN_ERROR_MESSAGE = "Error while scanning token";
		public static readonly string SYNTAX_ERROR_TITLE = "Syntax error";
		public static readonly string SYNTAX_ERROR_MESSAGE = "Error while parsing token";
		public static readonly string SEMANTIC_ERROR_TITLE = "Semantic error";
		public static readonly string SEMANTIC_ERROR_MESSAGE = "That's some weird shit... Error's in the semantix";
		public static readonly string UNINITIALIZED_VARIABLE_ERROR_MESSAGE = "Uninitialized variable";
		public static readonly string ILLEGAL_TYPE_ERROR_MESSAGE = "The type is not supported by this expression";
		public static readonly string DECLARATION_ERROR_MESSAGE = "The variable has already been declared";

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
	}
}