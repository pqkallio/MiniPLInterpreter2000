using System;
using System.Collections.Generic;

namespace MiniPLInterpreter
{
	public class VariableIdNode : ISyntaxTreeNode
	{
		private string id;
		private Dictionary<string, IProperty> ids;

		public VariableIdNode(Dictionary<string, IProperty> ids)
			: this(null, ids)
		{}

		public VariableIdNode (string id, Dictionary<string, IProperty> ids)
		{
			this.id = id;
			this.ids = ids;
		}

		public string ID {
			get { return id; }
			set { id = value; }
		}

		public object execute ()
		{
			return this.ids[ID];
		}

		public TokenType Type ()
		{
			return TokenType.ID;
		}
	}
}

