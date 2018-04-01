using System;

namespace MiniPLInterpreter
{
	/// <summary>
	/// Enumerates different token types.
	/// </summary>
	public enum TokenType
	{
		ID,					// an id variable
		UNARY_OP_LOG_NEG,	// unary operation, negation
		BINARY_OP_ADD,		// binary operation, addition
		BINARY_OP_SUB,		// binary operation, subtraction
		BINARY_OP_MUL,		// binary operation, multiplication
		BINARY_OP_DIV,		// binary operation, division
		BINARY_OP_LOG_EQ,	// binary operation, logical equation
		BINARY_OP_LOG_LT,	// binary operation, logical less than
		BINARY_OP_LOG_AND,	// binary operation, logical and
		TYPE,				// a declaration's type
		PARENTHESIS_LEFT,	// left parenthesis, "(" terminal
		PARENTHESIS_RIGHT,	// right parenthesis ")" terminal
		RANGE_FROM,			// range from, "in" terminal
		RANGE_UPTO,			// range upto, ".." terminal
		STR_VAL,			// a string value
		END_STATEMENT,		// end of statement, ";" terminal
		DECLARATION,		// declaration, "var" terminal
		FOR_LOOP,			// for-loop, "for" terminal
		START_BLOCK,		// start of a block, "do" terminal
		READ,				// "read" terminal
		PRINT,				// "print" terminal
		END_OF_BLOCK,		// end of block, "end" terminal
		ASSERT,				// "assert" terminal
		INT_VAL,			// an integer value
		INT_VAR,			// an integer variable
		BOOL_VAL,			// a boolean value, true or false
		BOOL_VAR,			// a boolean variable
		STR_VAR,			// a string variable
		UNDEFINED,			// token's type is undefined
		END_OF_FILE,		// end of file, EOF, you know..
		SET_TYPE,			// set type, ":" terminal
		ASSIGN,				// assign, ":=" terminal
		PROGRAM,			// the start of a program
		ERROR,				// error token, if something goes wrong
		BINARY_OP_NO_OP,	// binary expression, no operation (only lefthand side)
		VOID				// a void property
	}
}

