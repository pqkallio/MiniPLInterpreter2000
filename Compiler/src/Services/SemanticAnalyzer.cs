using System;
using System.Collections;
using System.Collections.Generic;

namespace MiniPLInterpreter
{
	public class SemanticAnalyzer : IErrorAggregator
	{
		private Queue nodes;
		private List<Error> errors;
		Dictionary<string, IProperty> ids;

		public SemanticAnalyzer (Queue nodes, Dictionary<string, IProperty> ids)
		{
			this.nodes = nodes;
			this.ids = ids;
			this.errors = new List<Error> ();
		}
			
		public void Analyze ()
		{
			ExpressionCheckVisitor v = new ExpressionCheckVisitor (this);

			ISyntaxTreeNode node = (ISyntaxTreeNode)nodes.Dequeue ();
			Console.WriteLine (node.ToString ());
			node.Accept (v);
		}

		public void notifyError (Error error)
		{
			this.errors.Add (error);
		}

		public List<Error> getErrors ()
		{
			return this.errors;
		}

		private TokenType checkExpressionIsLegit(IExpressionNode node)
		{
			IExpressionNode[] expressions = node.GetExpressions ();

			if (expressions == null) {
				return node.GetEvaluationType (TokenType.UNDEFINED);
			}

			TokenType tt = TokenType.UNDEFINED;

			for (int i = 0; i < expressions.Length; i++) {
				IExpressionNode en = expressions [i];

				if (en == null) {
					continue;
				}

				TokenType childType = checkExpressionIsLegit (en);

				if (childType == TokenType.ERROR) {
					return childType;
				}

				if (tt == TokenType.UNDEFINED) {
					tt = childType;
				} else if (childType != tt) {
					return TokenType.ERROR;
				}
			}

			if (Constants.LEGIT_OPERATIONS [tt].ContainsKey (node.Operation)) {
				return tt;
			}

			return TokenType.ERROR;
		}

		private bool checkExpressionMatchesType (IExpressionNode node, TokenType expectedType)
		{
			TokenType evaluationType = node.GetEvaluationType (TokenType.UNDEFINED);

			return evaluationType == expectedType;
		}

		private void match(ISyntaxTreeNode node, TokenType evaluated, TokenType expected)
		{
			if (evaluated != expected) {
				notifyError (new SemanticError (Constants.SEMANTIC_ERROR_MESSAGE, node));
			}
		}

		public Dictionary<string, IProperty> IDs
		{
			get { return this.ids; }
		}
	}
}

