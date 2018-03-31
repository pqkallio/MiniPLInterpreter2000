using System;
using System.IO;
using System.Collections.Generic;
using NUnit.Framework;
using MiniPLInterpreter;

namespace MiniPLInterpreterTests
{
	[TestFixture ()]
	public class ParserTest
	{
		private Scanner s;
		private Parser p;

		public ParserTest ()
		{}

		private void InitParser (string[] s)
		{
			Dictionary<string, IProperty> ids = new Dictionary<string, IProperty> ();
			this.s = new Scanner (s);
			this.p = new Parser (ids);
			this.p.Scanner = this.s;
		}

		private void Parse (string[] s)
		{
			InitParser (s);
			p.Parse ();
		}

		private TokenType GetExpectedType(int index)
		{
			return ((SyntaxError)p.getErrors () [index]).ExpectedType;
		}

		[Test]
		public void TestEmptyInput ()
		{
			Parse (ParserTestInputs.emptyInput);
			Assert.AreEqual (0, p.getErrors ().Count);
		}

		[Test]
		public void TestStatementNotEnded ()
		{
			Parse (ParserTestInputs.statementNotEnded);
			Assert.AreEqual (1, p.getErrors ().Count);
			Assert.AreEqual (TokenType.END_STATEMENT, GetExpectedType(0));
		}

		[Test]
		public void TestRightParenthesisMissing ()
		{
			Parse (ParserTestInputs.rightParenthesisMissing);
			Assert.AreEqual (1, p.getErrors ().Count);
			Assert.AreEqual (TokenType.PARENTHESIS_RIGHT, GetExpectedType(0));
		}

		[Test]
		public void TestDeclarationColonMissing ()
		{
			Parse (ParserTestInputs.declarationColonMissing);
			Assert.AreEqual (1, p.getErrors ().Count);
			Assert.AreEqual (TokenType.SET_TYPE, GetExpectedType(0));
		}

		[Test]
		public void TestDeclarationAssignMissing ()
		{
			Parse (ParserTestInputs.declarationAssignMissing);
			Assert.AreEqual (1, p.getErrors ().Count);
			Assert.AreEqual (TokenType.UNDEFINED, GetExpectedType(0));
		}

		[Test]
		public void TestDeclarationAssignValueMissing ()
		{
			Parse (ParserTestInputs.declarationAssignValueMissing);
			Assert.AreEqual (1, p.getErrors ().Count);
			Assert.AreEqual (TokenType.UNDEFINED, GetExpectedType(0));
		}

		[Test]
		public void TestIllegalStartOfStatement ()
		{
			Parse (ParserTestInputs.illegalStartOfStatement);
			Assert.AreEqual (1, p.getErrors ().Count);
			Assert.AreEqual (TokenType.UNDEFINED, GetExpectedType(0));
		}

		[Test]
		public void TestForLoopMissingVar ()
		{
			Parse (ParserTestInputs.forLoopMissingVar);
			Assert.AreEqual (1, p.getErrors ().Count);
			Assert.AreEqual (TokenType.ID, GetExpectedType(0));
		}

		[Test]
		public void TestForLoopMissingRangeFrom ()
		{
			Parse (ParserTestInputs.forLoopMissingRangeFrom);
			Assert.AreEqual (1, p.getErrors ().Count);
			Assert.AreEqual (TokenType.RANGE_FROM, GetExpectedType(0));
		}

		[Test]
		public void TestForLoopMissingRangeFromExpression ()
		{
			Parse (ParserTestInputs.forLoopMissingRangeFromExpression);
			Assert.AreEqual (1, p.getErrors ().Count);
			Assert.AreEqual (TokenType.UNDEFINED, GetExpectedType(0));
		}

		[Test]
		public void TestForLoopMissingRangeUpto ()
		{
			Parse (ParserTestInputs.forLoopMissingRangeUpto);
			Assert.AreEqual (1, p.getErrors ().Count);
			Assert.AreEqual (TokenType.RANGE_UPTO, GetExpectedType(0));
		}

		[Test]
		public void TestForLoopMissingRangeUptoExpression ()
		{
			Parse (ParserTestInputs.forLoopMissingRangeUptoExpression);
			Assert.AreEqual (1, p.getErrors ().Count);
			Assert.AreEqual (TokenType.UNDEFINED, GetExpectedType(0));
		}

		[Test]
		public void TestForLoopMissingStartBlock ()
		{
			Parse (ParserTestInputs.forLoopMissingStartBlock);
			Assert.AreEqual (1, p.getErrors ().Count);
			Assert.AreEqual (TokenType.START_BLOCK, GetExpectedType(0));
		}

		[Test]
		public void TestForLoopNoStatements ()
		{
			Parse (ParserTestInputs.forLoopNoStatements);
			Assert.AreEqual (0, p.getErrors ().Count);
		}

		[Test]
		public void TestForLoopMissingEnd ()
		{
			Parse (ParserTestInputs.forLoopMissingEnd);
			Assert.IsTrue (p.getErrors().Count > 0);
		}

		[Test]
		public void TestForLoopMissingForAfterEnd ()
		{
			Parse (ParserTestInputs.forLoopMissingForAfterEnd);
			Assert.IsTrue (p.getErrors().Count > 0);
		}

		[Test]
		public void TestForLoopMissingEndStatementAfterEndFor ()
		{
			Parse (ParserTestInputs.forLoopMissingEndStatementAfterEndFor);
			Assert.IsTrue (p.getErrors().Count > 0);
		}

		[Test]
		public void TestIntegerOverflowOnPositiveInt ()
		{
			Parse (ParserTestInputs.integerOverflowOnPositiveInt);
			Assert.AreEqual (p.getErrors ().Count, 1);
			Assert.AreEqual (p.getErrors () [0].GetType ().Name, "IntegerOverflowError");
		}

		[Test]
		public void TestIntegerOverflowOnNegativeInt ()
		{
			Parse (ParserTestInputs.integerOverflowOnNegativeInt);
			Assert.AreEqual (p.getErrors ().Count, 1);
			Assert.AreEqual (p.getErrors () [0].GetType ().Name, "IntegerOverflowError");
		}

		[Test]
		public void TestNoIntegerOverflowOnPositiveInt ()
		{
			Parse (ParserTestInputs.noIntegerOverflowOnPositiveInt);
			Assert.AreEqual (p.getErrors ().Count, 0);
		}

		[Test]
		public void TestNoIntegerOverflowOnNegativeInt ()
		{
			Parse (ParserTestInputs.noIntegerOverflowOnNegativeInt);
			Assert.AreEqual (p.getErrors ().Count, 0);
		}

		[Test]
		public void TestValidDeclaration ()
		{
			Parse (ParserTestInputs.validDeclaration);
			Assert.AreEqual (0, p.getErrors ().Count);
		}

		[Test]
		public void TestInvalidTypeOnDeclaration ()
		{
			Parse (ParserTestInputs.invalidTypeOnDeclaration);
			Assert.AreEqual (1, p.getErrors ().Count);
		}

		[Test]
		public void TestInvalidIdInDeclaration ()
		{
			Parse (ParserTestInputs.invalidIdInDeclaration);
			Assert.AreEqual (1, p.getErrors ().Count);
		}

		[Test]
		public void TestInvalidDeclarationStart ()
		{
			Parse (ParserTestInputs.invalidDeclarationStart);
			Assert.AreEqual (1, p.getErrors ().Count);
		}

		[Test]
		public void TestThreeErrorsInDeclaration ()
		{
			Parse (ParserTestInputs.threeErrorsInDeclaration);
			Assert.AreEqual (1, p.getErrors ().Count);
		}

		[Test]
		public void TestReadStatementOk ()
		{
			Parse (ParserTestInputs.readStatementOk);
			Assert.AreEqual (0, p.getErrors ().Count);
		}

		[Test]
		public void TestReadStatementMissingVarId ()
		{
			Parse (ParserTestInputs.readStatementMissingVarId);
			Assert.AreEqual (1, p.getErrors ().Count);
		}

		[Test]
		public void TestReadStatementInvalidId ()
		{
			Parse (ParserTestInputs.readStatementInvalidId);
			Assert.AreEqual (1, p.getErrors ().Count);
		}

		[Test]
		public void TestPrintStatementOk ()
		{
			Parse (ParserTestInputs.printStatementOk);
			Assert.AreEqual (0, p.getErrors ().Count);
		}

		[Test]
		public void TestPrintStatementMissingExpression ()
		{
			Parse (ParserTestInputs.printStatementMissingExpression);
			Assert.AreEqual (1, p.getErrors ().Count);
		}

		[Test]
		public void TestPrintStatementInvalidId ()
		{
			Parse (ParserTestInputs.printStatementInvalidId);
			Assert.AreEqual (1, p.getErrors ().Count);
		}

		[Test]
		public void TestAssertOk ()
		{
			Parse (ParserTestInputs.assertOk);
			Assert.AreEqual (0, p.getErrors ().Count);
		}

		[Test]
		public void TestAssertNoParentheses ()
		{
			Parse (ParserTestInputs.assertNoParentheses);
			Assert.AreEqual (1, p.getErrors ().Count);
		}

		[Test]
		public void TestAllIsWell ()
		{
			Parse (ParserTestInputs.validMassiveInputForParserTesting);
			Assert.AreEqual (0, p.getErrors ().Count);
		}

	}
}