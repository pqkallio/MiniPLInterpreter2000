using System;
using System.Collections.Generic;

namespace MiniPLInterpreter
{
	/// <summary>
	/// The Mini-PL interpreter, interpretes a program represented by an AST.
	/// </summary>
	public class Interpreter
	{
		private SyntaxTree syntaxTree;							// the AST
		private Dictionary<string, IProperty> symbolicTable;	
		private ExecutionVisitor executor;
		private IPrinter printer;
		private IReader reader;

		/// <summary>
		/// Initializes a new instance of the <see cref="MiniPLInterpreter.Interpreter"/> class.
		/// </summary>
		/// <param name="syntaxTree">The AST to interpret.</param>
		/// <param name="printer">An IPrinter used for printing output.</param>
		/// <param name="reader">An IReader used for reading input.</param>
		public Interpreter (SyntaxTree syntaxTree, IPrinter printer, IReader reader)
		{
			// The AST is assumed to be analyzed by a semantic analyzer before the interpretation.
			this.syntaxTree = syntaxTree;
			// The symbolic table is not the same used in the compilation, its empty at first.
			this.symbolicTable = new Dictionary<string, IProperty> ();
			this.printer = printer;
			this.reader = reader;
			// The execution visitor is used to execute each AST's node in a depth first style. 
			this.executor = new ExecutionVisitor (symbolicTable, this.printer, this.reader);
		}

		/// <summary>
		/// Interpret the AST.
		/// </summary>
		public void Interpret () {
			try {
				// We try to interpret the AST by asking the root node to accept the ExecutionVisitor.
				this.syntaxTree.Root.Accept (this.executor);
			} catch (RuntimeException ex) {
				// In case of a runtime exception, we catch it, print it and halt the execution.
				printer.printRuntimeException (ex);
				throw ex;
			} catch (Exception ex) {
				// If some error occurs we haven't prepared for, we still catch it, print it and halt the execution.
				printer.printLine ("A runtime error occured:" + StringFormattingConstants.LINEBREAK + ex.GetType().Name + ": " + ex.Message);
				throw ex;
			}
		}

		public Dictionary<string, IProperty> IDs
		{
			get { return symbolicTable; }
		}
	}
}

