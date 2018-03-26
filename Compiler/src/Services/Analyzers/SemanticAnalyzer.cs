using System;
using System.Collections;
using System.Collections.Generic;

namespace MiniPLInterpreter
{
	public class SemanticAnalyzer : IErrorAggregator
	{
		private List<Error> errors;
		Dictionary<string, IProperty> symbolTable;
		private SyntaxTree syntaxTree;

		public SemanticAnalyzer (SyntaxTree syntaxTree, Dictionary<string, IProperty> symbolTable)
		{
			this.syntaxTree = syntaxTree;
			this.symbolTable = symbolTable;
			this.errors = new List<Error> ();
		}
			
		public void Analyze ()
		{
			ExpressionCheckVisitor expressionChecker = new ExpressionCheckVisitor (this);

			IStatementsContainer rootNode = syntaxTree.Root;
			rootNode.Accept (expressionChecker);
		}

		public void notifyError (Error error)
		{
			this.errors.Add (error);
		}

		public List<Error> getErrors ()
		{
			return this.errors;
		}

		public Dictionary<string, IProperty> SymbolicTable
		{
			get { return this.symbolTable; }
		}
	}
}

