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
		private StreamReader sr;

		public ParserTest ()
		{}

		[TearDown]
		public void TearDown ()
		{
			if (this.sr != null) {
				this.sr.Close ();
			}
		}

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
		public void TestStatementNotEnded ()
		{
			Parse (TestInputs.statementNotEnded);
			Assert.AreEqual (1, p.getErrors ().Count);
			Assert.AreEqual (TokenType.END_STATEMENT, GetExpectedType(0));
		}

		[Test]
		public void TestRightParenthesisMissing ()
		{
			Parse (TestInputs.rightParenthesisMissing);
			Assert.AreEqual (1, p.getErrors ().Count);
			Assert.AreEqual (TokenType.PARENTHESIS_RIGHT, GetExpectedType(0));
		}

		[Test]
		public void TestDeclarationColonMissing ()
		{
			Parse (TestInputs.declarationColonMissing);
			Assert.AreEqual (1, p.getErrors ().Count);
			Assert.AreEqual (TokenType.SET_TYPE, GetExpectedType(0));
		}

		[Test]
		public void TestDeclarationAssignMissing ()
		{
			Parse (TestInputs.declarationAssignMissing);
			Assert.AreEqual (1, p.getErrors ().Count);
			Assert.AreEqual (TokenType.UNDEFINED, GetExpectedType(0));
		}

		[Test]
		public void TestDeclarationAssignValueMissing ()
		{
			Parse (TestInputs.declarationAssignValueMissing);
			foreach (Error e in p.getErrors()) {
				Console.WriteLine (e);
				Console.WriteLine (e.Token);
			}
			Assert.AreEqual (1, p.getErrors ().Count);
			Assert.AreEqual (TokenType.UNDEFINED, GetExpectedType(0));
		}

		[Test]
		public void TestIllegalStartOfStatement ()
		{
			Parse (TestInputs.illegalStartOfStatement);
			Assert.AreEqual (1, p.getErrors ().Count);
			Assert.AreEqual (TokenType.UNDEFINED, GetExpectedType(0));
		}

		[Test]
		public void TestForLoopMissingVar ()
		{
			Parse (TestInputs.forLoopMissingVar);
			Assert.AreEqual (1, p.getErrors ().Count);
			Assert.AreEqual (TokenType.ID, GetExpectedType(0));
		}

		[Test]
		public void TestForLoopMissingRangeFrom ()
		{
			Parse (TestInputs.forLoopMissingRangeFrom);
			Assert.AreEqual (1, p.getErrors ().Count);
			Assert.AreEqual (TokenType.RANGE_FROM, GetExpectedType(0));
		}

		[Test]
		public void TestForLoopMissingRangeFromExpression ()
		{
			Parse (TestInputs.forLoopMissingRangeFromExpression);
			Assert.AreEqual (1, p.getErrors ().Count);
			Assert.AreEqual (TokenType.UNDEFINED, GetExpectedType(0));
		}

		[Test]
		public void TestForLoopMissingRangeUpto ()
		{
			Parse (TestInputs.forLoopMissingRangeUpto);
			Assert.AreEqual (1, p.getErrors ().Count);
			Assert.AreEqual (TokenType.UNDEFINED, GetExpectedType(0));
		}

		[Test]
		public void TestForLoopMissingRangeUptoExpression ()
		{
			Parse (TestInputs.forLoopMissingRangeUptoExpression);
			Assert.AreEqual (1, p.getErrors ().Count);
			Assert.AreEqual (TokenType.UNDEFINED, GetExpectedType(0));
		}

		[Test]
		public void TestForLoopMissingStartBlock ()
		{
			Parse (TestInputs.forLoopMissingStartBlock);
			Assert.AreEqual (1, p.getErrors ().Count);
			Assert.AreEqual (TokenType.START_BLOCK, GetExpectedType(0));
		}
	}
}