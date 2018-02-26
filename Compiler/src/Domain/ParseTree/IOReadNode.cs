﻿using System;
using System.Collections.Generic;

namespace MiniPLInterpreter
{
	public class IOReadNode : ISyntaxTreeNode
	{
		private VariableIdNode idNode;
		private AssignNode assignNode;
		private Dictionary<string, IProperty> ids;

		public IOReadNode (VariableIdNode idNode, Dictionary<string, IProperty> ids)
		{
			this.idNode = idNode;
			this.assignNode = new AssignNode (this.idNode, ids);
			this.ids = ids;
		}

		public TokenType Type ()
		{
			return TokenType.READ;
		}

		public object execute ()
		{
			string input = Console.ReadLine ();
			Type nodeType = ids [idNode.ID].GetPropertyType ();

			if (nodeType == typeof(int)) {
				this.assignNode.AddExpression (new IntValueNode (StringUtils.parseToInt (input)));
			} else if (nodeType == typeof(string)) {
				this.assignNode.AddExpression (new StringValueNode (input));
			} else {
				throw new ArgumentException (String.Format("assignment to a variable of type {0} not supported", ids[idNode.ID].GetPropertyType()));
			}

			assignNode.execute ();

			return null;
		}
	}
}

