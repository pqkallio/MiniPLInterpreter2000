using System;
using System.Collections.Generic;

namespace MiniPLInterpreter
{
	public class ParserConstants
	{
		private static readonly Dictionary<TokenType, string> TOKEN_TYPE_STRINGS = StringFormattingConstants.TOKEN_TYPE_STRINGS;

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
			
		public static readonly string IDENTIFIER_STR = "identifier";
		public static readonly string EOF_STR = "end of file";
		public static readonly string OPERAND_STR = "operand literal";
		public static readonly string EXPRESSION_STR = "expression";
		public static readonly string END_OF_EXPR_STR = "end of expression";
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

