﻿using System;
using System.Collections;
using System.Collections.Generic;

namespace MiniPLInterpreter
{
	/// <summary>
	/// Represents a Declaration statement in the AST
	/// </summary>
	public class DeclarationNode : IIdentifierContainer
	{
		private VariableIdNode idNode;
		private AssignNode assignNode;
		private Token token;

		public DeclarationNode (VariableIdNode idNode, Dictionary<string, IProperty> ids, Token t)
		{
			this.idNode = idNode;
			this.token = t;
		}

		public AssignNode AssignNode
		{
			get { return assignNode; }
			set { assignNode = value; }
		}

		public VariableIdNode IDNode
		{
			get { return idNode; }
			set { this.idNode = value; }
		}

		public ISemanticCheckValue Accept(INodeVisitor visitor) {
			return visitor.VisitDeclarationNode (this);
		}

		public Token Token
		{
			get { return this.token; }
			set { }
		}
	}
}

