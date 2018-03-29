using System;
using System.IO;
using NUnit.Framework;
using System.Collections.Generic;
using MiniPLInterpreter;

namespace MiniPLInterpreterTests
{
	[TestFixture ()]
	public class ScannerTest
	{
		private Scanner s;
		private List<Token> tokens;

		private void InitScanner(string[] s)
		{
			this.s = new Scanner (s);
		}

		[SetUp]
		public void SetUp()
		{
			this.tokens = new List<Token> ();
		}

		[Test]
		public void TestValidInput1Tokens ()
		{
			InitScanner (ScannerTestInputs.validInput1);
			Token t = null;

			while (t == null || t.Type != TokenType.END_OF_FILE) {
				t = s.getNextToken (t);
				tokens.Add(t);
			}

			Assert.AreEqual (44, tokens.Count);
			Assert.AreEqual (0, s.getErrors ().Count);
		}


		[Test]
		public void TestValidInput2Tokens ()
		{
			InitScanner (ScannerTestInputs.validInput2);
			Token t = null;

			while (t == null || t.Type != TokenType.END_OF_FILE) {
				t = s.getNextToken (t);
				tokens.Add(t);
			}

			Assert.AreEqual (17, tokens.Count);
			Assert.AreEqual (0, s.getErrors ().Count);
		}

		[Test]
		public void TestValidInput3Tokens ()
		{
			InitScanner (ScannerTestInputs.validInput3);
			Token t = null;

			while (t == null || t.Type != TokenType.END_OF_FILE) {
				t = s.getNextToken (t);
				tokens.Add(t);
			}

			Assert.AreEqual (46, tokens.Count);
			Assert.AreEqual (0, s.getErrors ().Count);
		}

		[Test]
		public void TestValidInput4Tokens ()
		{
			InitScanner (ScannerTestInputs.validInput4);
			Token t = null;

			while (t == null || t.Type != TokenType.END_OF_FILE) {
				t = s.getNextToken (t);
				tokens.Add(t);
			}

			Assert.AreEqual (1, tokens.Count);
			Assert.AreEqual (tokens [0].Type, TokenType.END_OF_FILE);
			Assert.AreEqual (0, s.getErrors ().Count);
		}

		[Test]
		public void TestInvalidInput1Tokens ()
		{
			InitScanner (ScannerTestInputs.invalidInput1);
			Token t = null;

			while (t == null || t.Type != TokenType.END_OF_FILE) {
				t = s.getNextToken (t);
				tokens.Add(t);
			}

			Assert.AreEqual (2, s.getErrors ().Count);
		}

		[Test]
		public void TestInvalidInput2Tokens ()
		{
			InitScanner (ScannerTestInputs.invalidInput2);
			Token t = null;

			while (t == null || t.Type != TokenType.END_OF_FILE) {
				t = s.getNextToken (t);
				tokens.Add(t);
			}

			Assert.AreEqual (1, s.getErrors ().Count);
			Assert.AreEqual (s.getErrors()[0].GetType().Name, nameof(InvalidIdentifierError));
		}

		[Test]
		public void TestInvalidInput3Tokens ()
		{
			InitScanner (ScannerTestInputs.invalidInput3);
			Token t = null;

			while (t == null || t.Type != TokenType.END_OF_FILE) {
				t = s.getNextToken (t);
				tokens.Add(t);
			}

			Assert.AreEqual (1, s.getErrors ().Count);
			foreach (Error e in s.getErrors()) {
				Assert.AreEqual (e.GetType ().Name, nameof(TokenError));
			}
		}

		[Test]
		public void TestTwoErrorsInOneStatement ()
		{
			InitScanner (ScannerTestInputs.invalidInput4);
			Token t = null;

			while (t == null || t.Type != TokenType.END_OF_FILE) {
				t = s.getNextToken (t);
				tokens.Add(t);
			}

			Assert.AreEqual (2, s.getErrors ().Count);
		}
	}
}

