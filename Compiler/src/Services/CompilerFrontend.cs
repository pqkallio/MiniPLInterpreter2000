using System;
using System.Collections.Generic;

namespace MiniPLInterpreter
{
	public class CompilerFrontend
	{
		private Scanner scanner;
		private Parser parser;
		private SemanticAnalyzer semanticAnalyzer;
		private Dictionary<string, IProperty> symbolTable;
		private string[] sourceLines;

		public CompilerFrontend ()
		{}

		private void Init(string filePath) {
			this.sourceLines = readSource (filePath);
			this.symbolTable = new Dictionary<string, IProperty> ();
			this.scanner = new Scanner (sourceLines);
			this.parser = new Parser (symbolTable, scanner);
		}

		public SyntaxTree Compile(string filePath)
		{
			Init (filePath);
			SyntaxTree syntaxTree = this.parser.Parse ();

			if (lexicalErrors ()) {
				return null;
			}

			semanticAnalyzer = new SemanticAnalyzer (syntaxTree, symbolTable);

			semanticAnalyzer.Analyze ();

			return syntaxTree;
		}

		public string[] SourceLines
		{
			get { return sourceLines; }
		}

		private string[] readSource(string filePath)
		{
			SourceBuffer sourceBuffer = new SourceBuffer(filePath);

			return sourceBuffer.SourceLines;
		}

		private bool lexicalErrors ()
		{
			bool errors = false;

			if (scanner != null) {
				errors |= scanner.getErrors ().Count != 0;
			}

			if (parser != null) {
				errors |= parser.getErrors ().Count != 0;
			}

			return errors;
		}

		public List<Error> getErrors ()
		{
			List<Error> errors = new List<Error> ();

			if (scanner != null) {
				errors.AddRange (scanner.getErrors ());
			}

			if (parser != null) {
				errors.AddRange (parser.getErrors ());
			}

			if (semanticAnalyzer != null) {
				errors.AddRange (semanticAnalyzer.getErrors ());
			}

			return errors;
		}
	}
}
