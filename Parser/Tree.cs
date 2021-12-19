using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tiny_Parser
{
    class Node
    {
        private Token token;
        private List<Node> children = new List<Node>();

        public Node(Token t)
        {
            token = t;
        }
        public Node()
        {
            token = null;
        }

        public List<Node> getChildren()
        {
            return children;
        }

        public Token getToken()
        {
            return token;
        }

        public void setToken(Token t)
        {
            token = t;
        }

    }
    class Tree
    {
        private Node root;
        public Tree(Node root)
        {
            this.root = root;
        }

        public Tree()
        {
            Token program_token = new Token("Program", "PROGRAM");
            root = new Node(program_token);
        }

        public Node getTreeRoot()
        {
            return this.root;
        }
        
        public void appendChild(Node parent, Node child)
        {
            parent.getChildren().Add(child);
        }
    }
}
