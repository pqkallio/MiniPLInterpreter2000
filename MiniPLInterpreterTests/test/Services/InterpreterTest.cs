using System;
using System.IO;
using System.Collections.Generic;
using NUnit.Framework;
using MiniPLInterpreter;

namespace MiniPLInterpreterTests
{
	[TestFixture ()]
	public class InterpreterTest
	{
		private Scanner s;
		private Parser p;
		private SemanticAnalyzer sa;
		private Interpreter interpreter;
		private MockPrinter printer;

		public InterpreterTest ()
		{}

		private bool InitInterpreter (string[] s, IReader reader)
		{
			Dictionary<string, IProperty> ids = new Dictionary<string, IProperty> ();
			this.s = new Scanner (s);
			this.p = new Parser (ids);
			this.p.Scanner = this.s;
			SyntaxTree tree = this.p.Parse ();
			if (!this.p.SyntaxTreeBuilt)
				return false;
			this.sa = new SemanticAnalyzer (tree, ids);
			sa.Analyze ();
			if (this.sa.getErrors ().Count > 0)
				return false;
			this.printer = new MockPrinter ();
			this.interpreter = new Interpreter (tree, this.printer, reader);
			return true;
		}

		private bool Interpret (string[] s)
		{
			return Interpret (s, null);
		}

		private bool Interpret (string[] s, string readerInput)
		{
			MockReader reader = readerInput == null ? new MockReader ("") : new MockReader (readerInput);
			bool initOk = InitInterpreter (s, reader);
			if (!initOk)
				return false;
			interpreter.Interpret ();
			return true;
		}

		[Test]
		public void TestDeclarationsWorkAsIntended ()
		{
			bool interpreted = Interpret (InterpreterTestInput.declarationsWorkAsIntended);
			Assert.IsTrue (interpreted);
			Assert.AreEqual (this.interpreter.SymbolTable ["x"].asBoolean (), false);
			Assert.AreEqual (this.interpreter.SymbolTable ["y"].asString (), "");
			Assert.AreEqual (this.interpreter.SymbolTable ["z"].asInteger (), 0);
		}

		[Test]
		public void TestAssignmentsWorkAsIntended ()
		{
			bool interpreted = Interpret (InterpreterTestInput.assignmentsWorkAsIntended);
			Assert.IsTrue (interpreted);
			Assert.AreEqual (this.interpreter.SymbolTable ["x"].asBoolean (), true);
			Assert.AreEqual (this.interpreter.SymbolTable ["y"].asString (), "foobar");
			Assert.AreEqual (this.interpreter.SymbolTable ["z"].asInteger (), 1984);
		}

		[Test]
		public void TestValidIntRead ()
		{
			bool interpreted = Interpret (InterpreterTestInput.intRead, "97\tdetr");
			Assert.IsTrue (interpreted);
			Assert.AreEqual (this.interpreter.SymbolTable ["a"].asInteger (), 97);
		}

		[Test]
		[ExpectedException("MiniPLInterpreter.RuntimeException")]
		public void TestInvalidIntRead ()
		{
			Interpret (InterpreterTestInput.intRead, "97d");
		}

		[Test]
		[ExpectedException("MiniPLInterpreter.RuntimeException")]
		public void TestTooBigAddition ()
		{
			Interpret (InterpreterTestInput.tooBigAddition);
		}

		[Test]
		[ExpectedException("MiniPLInterpreter.RuntimeException")]
		public void TestTooSmallSubtraction ()
		{
			Interpret (InterpreterTestInput.tooSmallSubtraction);
		}

		[Test]
		[ExpectedException("MiniPLInterpreter.RuntimeException")]
		public void TestTooBigMultiplication ()
		{
			Interpret (InterpreterTestInput.tooBigMultiplication);
		}

		[Test]
		[ExpectedException("MiniPLInterpreter.RuntimeException")]
		public void TestDivisionByZero ()
		{
			Interpret (InterpreterTestInput.divisionByZero);
		}

		[Test]
		public void TestFailedAssertion ()
		{
			bool interpreted = Interpret (InterpreterTestInput.failedAssertion);
			Assert.IsTrue (interpreted);
			Assert.AreEqual (this.printer.Prints, 1);
		}

		[Test]
		public void TestPrintStatementInForLoop ()
		{
			bool interpreted = Interpret (InterpreterTestInput.printStatementInForLoop);
			Assert.IsTrue (interpreted);
			Assert.AreEqual (this.printer.Prints, 10);
		}
	}
}

