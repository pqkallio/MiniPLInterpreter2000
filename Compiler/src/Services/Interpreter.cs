using System;
using System.Collections.Generic;

namespace MiniPLInterpreter
{
	public class Interpreter
	{
		private SyntaxTree syntaxTree;
		private Dictionary<string, IProperty> ids;
		private ExecutionVisitor executor;
		private IPrinter printer;
		private IReader reader;

		public Interpreter (SyntaxTree syntaxTree, IPrinter printer, IReader reader)
		{
			this.syntaxTree = syntaxTree;
			this.ids = new Dictionary<string, IProperty> ();
			this.printer = printer;
			this.reader = reader;
			this.executor = new ExecutionVisitor (ids, this.printer, this.reader);
		}

		public void Interpret () {
			try {
				this.syntaxTree.Root.Accept (this.executor);
			} catch (RuntimeException ex) {
				printer.printRuntimeException (ex);
				throw ex;
			} catch (Exception ex) {
				printer.printLine ("A runtime error occured:" + StringFormattingConstants.LINEBREAK + ex.GetType().Name + ": " + ex.Message);
				throw ex;
			}
		}

		public Dictionary<string, IProperty> IDs
		{
			get { return ids; }
		}
	}
}

