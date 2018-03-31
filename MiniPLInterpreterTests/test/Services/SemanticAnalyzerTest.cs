using System;
using System.IO;
using System.Collections.Generic;
using NUnit.Framework;
using MiniPLInterpreter;

namespace MiniPLInterpreterTests
{
	[TestFixture ()]
	public class SemanticAnalyzerTest
	{
		private Scanner s;
		private Parser p;
		private SemanticAnalyzer sa;

		public SemanticAnalyzerTest ()
		{}

		private bool InitAnalyzer (string[] s)
		{
			Dictionary<string, IProperty> ids = new Dictionary<string, IProperty> ();
			this.s = new Scanner (s);
			this.p = new Parser (ids);
			this.p.Scanner = this.s;
			SyntaxTree tree = this.p.Parse ();
			if (!this.p.SyntaxTreeBuilt)
				return false;
			this.sa = new SemanticAnalyzer (tree, ids);
			return true;
		}

		private bool Analyze (string[] s)
		{
			bool initOk = InitAnalyzer (s);
			if (!initOk)
				return false;
			sa.Analyze ();
			return true;
		}

		[Test]
		public void TestMassiveCorrectInput ()
		{
			bool analyzed = Analyze (SemanticAnalyzerTestInputs.validMassiveInputForSemanticTesting);
			Assert.IsTrue (analyzed);
			Assert.AreEqual (sa.getErrors ().Count, 0);
		}

		[Test]
		public void TestCorrectBoolAssignments ()
		{
			bool analyzed = Analyze (SemanticAnalyzerTestInputs.correctBoolAssignments);
			Assert.IsTrue (analyzed);
			Assert.AreEqual (sa.getErrors ().Count, 0);
		}

		[Test]
		public void TestIncorrectBoolAssignment1 ()
		{
			bool analyzed = Analyze (SemanticAnalyzerTestInputs.incorrectBoolAssignment1);
			Assert.IsTrue (analyzed);
			Assert.AreEqual (sa.getErrors ().Count, 1);
		}

		[Test]
		public void TestIncorrectBoolAssignment2 ()
		{
			bool analyzed = Analyze (SemanticAnalyzerTestInputs.incorrectBoolAssignment2);
			Assert.IsTrue (analyzed);
			Assert.AreEqual (sa.getErrors ().Count, 1);
		}

		[Test]
		public void TestIncorrectBoolAssignment3 ()
		{
			bool analyzed = Analyze (SemanticAnalyzerTestInputs.incorrectBoolAssignment3);
			Assert.IsTrue (analyzed);
			Assert.AreEqual (sa.getErrors ().Count, 1);
		}

		[Test]
		public void TestIncorrectBoolAssignment4 ()
		{
			bool analyzed = Analyze (SemanticAnalyzerTestInputs.incorrectBoolAssignment4);
			Assert.IsTrue (analyzed);
			Assert.AreEqual (sa.getErrors ().Count, 1);
		}

		[Test]
		public void TestIncorrectBoolAssignment5 ()
		{
			bool analyzed = Analyze (SemanticAnalyzerTestInputs.incorrectBoolAssignment5);
			Assert.IsTrue (analyzed);
			Assert.AreEqual (sa.getErrors ().Count, 1);
		}

		[Test]
		public void TestIncorrectBoolAssignment6 ()
		{
			bool analyzed = Analyze (SemanticAnalyzerTestInputs.incorrectBoolAssignment6);
			Assert.IsTrue (analyzed);
			Assert.AreEqual (sa.getErrors ().Count, 1);
		}

		[Test]
		public void TestIncorrectBoolAssignment7 ()
		{
			bool analyzed = Analyze (SemanticAnalyzerTestInputs.incorrectBoolAssignment7);
			Assert.IsTrue (analyzed);
			Assert.AreEqual (sa.getErrors ().Count, 1);
		}

		[Test]
		public void TestIncorrectBoolAssignment8 ()
		{
			bool analyzed = Analyze (SemanticAnalyzerTestInputs.incorrectBoolAssignment8);
			Assert.IsTrue (analyzed);
			Assert.AreEqual (sa.getErrors ().Count, 1);
		}

		[Test]
		public void TestIncorrectStringAssignment1 ()
		{
			bool analyzed = Analyze (SemanticAnalyzerTestInputs.incorrectStringAssignment1);
			Assert.IsTrue (analyzed);
			Assert.AreEqual (sa.getErrors ().Count, 1);
		}

		[Test]
		public void TestIncorrectStringAssignment2 ()
		{
			bool analyzed = Analyze (SemanticAnalyzerTestInputs.incorrectStringAssignment2);
			Assert.IsTrue (analyzed);
			Assert.AreEqual (sa.getErrors ().Count, 1);
		}

		[Test]
		public void TestIncorrectStringAssignment3 ()
		{
			bool analyzed = Analyze (SemanticAnalyzerTestInputs.incorrectStringAssignment3);
			Assert.IsTrue (analyzed);
			Assert.AreEqual (sa.getErrors ().Count, 1);
		}

		[Test]
		public void TestIncorrectStringAssignment4 ()
		{
			bool analyzed = Analyze (SemanticAnalyzerTestInputs.incorrectStringAssignment4);
			Assert.IsTrue (analyzed);
			Assert.AreEqual (sa.getErrors ().Count, 1);
		}

		[Test]
		public void TestIncorrectStringAssignment5 ()
		{
			bool analyzed = Analyze (SemanticAnalyzerTestInputs.incorrectStringAssignment5);
			Assert.IsTrue (analyzed);
			Assert.AreEqual (sa.getErrors ().Count, 1);
		}

		[Test]
		public void TestIncorrectStringAssignment6 ()
		{
			bool analyzed = Analyze (SemanticAnalyzerTestInputs.incorrectStringAssignment6);
			Assert.IsTrue (analyzed);
			Assert.AreEqual (sa.getErrors ().Count, 1);
		}

		[Test]
		public void TestIncorrectStringAssignment7 ()
		{
			bool analyzed = Analyze (SemanticAnalyzerTestInputs.incorrectStringAssignment7);
			Assert.IsTrue (analyzed);
			Assert.AreEqual (sa.getErrors ().Count, 1);
		}

		[Test]
		public void TestIncorrectStringAssignment8 ()
		{
			bool analyzed = Analyze (SemanticAnalyzerTestInputs.incorrectStringAssignment8);
			Assert.IsTrue (analyzed);
			Assert.AreEqual (sa.getErrors ().Count, 1);
		}

		[Test]
		public void TestIncorrectStringAssignment9 ()
		{
			bool analyzed = Analyze (SemanticAnalyzerTestInputs.incorrectStringAssignment9);
			Assert.IsTrue (analyzed);
			Assert.AreEqual (sa.getErrors ().Count, 1);
		}

		[Test]
		public void TestIncorrectStringAssignment10 ()
		{
			bool analyzed = Analyze (SemanticAnalyzerTestInputs.incorrectStringAssignment10);
			Assert.IsTrue (analyzed);
			Assert.AreEqual (sa.getErrors ().Count, 1);
		}

		[Test]
		public void TestCorrectStringAssignments ()
		{
			bool analyzed = Analyze (SemanticAnalyzerTestInputs.correctStringAssignments);
			Assert.IsTrue (analyzed);
			Assert.AreEqual (sa.getErrors ().Count, 0);
		}

		[Test]
		public void TestIncorrectIntAssignment1 ()
		{
			bool analyzed = Analyze (SemanticAnalyzerTestInputs.incorrectIntAssignment1);
			Assert.IsTrue (analyzed);
			Assert.AreEqual (sa.getErrors ().Count, 1);
		}

		[Test]
		public void TestIncorrectIntAssignment2 ()
		{
			bool analyzed = Analyze (SemanticAnalyzerTestInputs.incorrectIntAssignment2);
			Assert.IsTrue (analyzed);
			Assert.AreEqual (sa.getErrors ().Count, 1);
		}

		[Test]
		public void TestIncorrectIntAssignment3 ()
		{
			bool analyzed = Analyze (SemanticAnalyzerTestInputs.incorrectIntAssignment3);
			Assert.IsTrue (analyzed);
			Assert.AreEqual (sa.getErrors ().Count, 1);
		}

		[Test]
		public void TestIncorrectIntAssignment4 ()
		{
			bool analyzed = Analyze (SemanticAnalyzerTestInputs.incorrectIntAssignment4);
			Assert.IsTrue (analyzed);
			Assert.AreEqual (sa.getErrors ().Count, 1);
		}

		[Test]
		public void TestIncorrectIntAssignment5 ()
		{
			bool analyzed = Analyze (SemanticAnalyzerTestInputs.incorrectIntAssignment5);
			Assert.IsTrue (analyzed);
			Assert.AreEqual (sa.getErrors ().Count, 1);
		}

		[Test]
		public void TestCorrectIntAssignments ()
		{
			bool analyzed = Analyze (SemanticAnalyzerTestInputs.correctIntAssignments);
			Assert.IsTrue (analyzed);
			Assert.AreEqual (sa.getErrors ().Count, 0);
		}

		[Test]
		public void TestNotDeclaredForRead ()
		{
			bool analyzed = Analyze (SemanticAnalyzerTestInputs.notDeclaredForRead);
			foreach (Error e in sa.getErrors()) {
				Console.WriteLine (e);
			}
			Assert.IsTrue (analyzed);
			Assert.IsTrue (sa.getErrors ().Count > 0);
		}

		[Test]
		public void TestTryToReadToIntVar ()
		{
			bool analyzed = Analyze (SemanticAnalyzerTestInputs.tryToReadToIntVar);
			Assert.IsTrue (analyzed);
			Assert.AreEqual (sa.getErrors ().Count, 0);
		}

		[Test]
		public void TestTryToReadToStringVar ()
		{
			bool analyzed = Analyze (SemanticAnalyzerTestInputs.tryToReadToStringVar);
			Assert.IsTrue (analyzed);
			Assert.AreEqual (sa.getErrors ().Count, 0);
		}

		[Test]
		public void TestTryToReadToBoolVar ()
		{
			bool analyzed = Analyze (SemanticAnalyzerTestInputs.tryToReadToBoolVar);
			Assert.IsTrue (analyzed);
			Assert.AreEqual (sa.getErrors ().Count, 1);
		}

		[Test]
		public void TestNotDeclaredForPrint ()
		{
			bool analyzed = Analyze (SemanticAnalyzerTestInputs.notDeclaredForPrint);
			foreach (Error e in sa.getErrors()) {
				Console.WriteLine (e);
			}
			Assert.IsTrue (analyzed);
			Assert.AreEqual (sa.getErrors ().Count, 1);
		}

		[Test]
		public void TestDoubleDeclaration ()
		{
			bool analyzed = Analyze (SemanticAnalyzerTestInputs.doubleDeclaration);
			Assert.IsTrue (analyzed);
			Assert.AreEqual (sa.getErrors ().Count, 1);
		}

		[Test]
		public void TestAssertionBoolean ()
		{
			bool analyzed = Analyze (SemanticAnalyzerTestInputs.assertionBoolean);
			Assert.IsTrue (analyzed);
			Assert.AreEqual (sa.getErrors ().Count, 0);
		}

		[Test]
		public void TestAssertionNotBoolean ()
		{
			bool analyzed = Analyze (SemanticAnalyzerTestInputs.assertionNotBoolean);
			Assert.IsTrue (analyzed);
			Assert.AreEqual (sa.getErrors ().Count, 1);
		}

		[Test]
		public void TestForLoopOk ()
		{
			bool analyzed = Analyze (SemanticAnalyzerTestInputs.forLoopOk);
			Assert.IsTrue (analyzed);
			Assert.AreEqual (sa.getErrors ().Count, 0);
		}

		[Test]
		public void TestForControlVarNotDeclared ()
		{
			bool analyzed = Analyze (SemanticAnalyzerTestInputs.forLoopControlVarNotDeclared);
			Assert.IsTrue (analyzed);
			Assert.IsTrue (sa.getErrors ().Count > 0);
		}

		[Test]
		public void TestForRangeFromNotInt ()
		{
			bool analyzed = Analyze (SemanticAnalyzerTestInputs.forLoopRangeFromNotInt);
			Assert.IsTrue (analyzed);
			Assert.AreEqual (sa.getErrors ().Count, 1);
		}

		[Test]
		public void TestForRangeUptoNotInt ()
		{
			bool analyzed = Analyze (SemanticAnalyzerTestInputs.forLoopRangeUptoNotInt);
			Assert.IsTrue (analyzed);
			Assert.AreEqual (sa.getErrors ().Count, 1);
		}

		[Test]
		public void TestForLoopControlVariableReassignInsideLoop ()
		{
			bool analyzed = Analyze (SemanticAnalyzerTestInputs.forControlVariableReassignInsideLoop);
			Assert.IsTrue (analyzed);
			Assert.AreEqual (sa.getErrors ().Count, 1);
		}

		[Test]
		public void TestForLoopControlVariableReassignOutsideLoop ()
		{
			bool analyzed = Analyze (SemanticAnalyzerTestInputs.forControlVariableReassignOutsideLoop);
			Assert.IsTrue (analyzed);
			Assert.AreEqual (sa.getErrors ().Count, 0);
		}

		[Test]
		public void TestAssignWhenNotDeclared ()
		{
			bool analyzed = Analyze (SemanticAnalyzerTestInputs.assignWhenNotDeclared);
			Assert.IsTrue (analyzed);
			Assert.IsTrue (sa.getErrors ().Count > 0);
		}

		[Test]
		public void TestSourceEmpty ()
		{
			bool analyzed = Analyze (SemanticAnalyzerTestInputs.sourceEmpty);
			Assert.IsTrue (analyzed);
			Assert.AreEqual (sa.getErrors ().Count, 0);
		}
	}
}

