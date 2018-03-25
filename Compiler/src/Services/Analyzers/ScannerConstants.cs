using System;
using System.Collections.Generic;

namespace MiniPLInterpreter
{
	public class ScannerConstants
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

		public static readonly Tuple<char, TokenType> BINARY_OP_ADD = Tuple.Create('+', TokenType.BINARY_OP_ADD);
		public static readonly Tuple<char, TokenType> BINARY_OP_SUB = Tuple.Create('-', TokenType.BINARY_OP_SUB);
		public static readonly Tuple<char, TokenType> BINARY_OP_DIV = Tuple.Create('/', TokenType.BINARY_OP_DIV);
		public static readonly Tuple<char, TokenType> SET_TYPE = Tuple.Create(':', TokenType.SET_TYPE);
		public static readonly Tuple<char, TokenType> ASSIGN = Tuple.Create('=', TokenType.ASSIGN);

		public static readonly char ESCAPE_CHAR = '\\';
		public static readonly char STRING_DELIMITER = '"';
		public static readonly char NULL_CHAR = '\0';
		public static readonly char NEWLINE = '\n';
	}
}

