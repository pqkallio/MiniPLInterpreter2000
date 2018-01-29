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
	}
}