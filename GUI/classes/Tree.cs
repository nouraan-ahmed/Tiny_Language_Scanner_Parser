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
        private int left_margin = 0;
        private int child_no = 0;
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

        public int getChildrenCount()
        {
            return children.Count();
        }

        public int getLeftMargin()
        {
            return left_margin;
        }
        public void setLeftMargin(int l)
        {
            left_margin = l;
        }

        public int getChildNo()
        {
            return child_no;
        }
        public void setChildNo(int n)
        {
            child_no = n;
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

        public void traverseTree(Action<Node, Node, int> drawNode)
        {
            treverseTreeAUX(root, drawNode, -1);
        }

        private void treverseTreeAUX(Node n, Action<Node, Node, int> drawNode, int depth)
        {
            if (n.getChildrenCount() == 0)
            {
                return;
            }

            depth++;
            //print child
            //traverse this child
            List<Node> children = n.getChildren();
            for (int i = 0; i < children.Count(); i++)
            {
                children[i].setChildNo(i);
                drawNode(n, children[i], depth);
                treverseTreeAUX(children[i], drawNode, depth);
            }
        }

    }
}
