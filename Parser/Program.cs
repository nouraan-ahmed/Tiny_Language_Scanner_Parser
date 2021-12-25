using System;

namespace Tiny_Parser
{
    class Program
    {
        static void Main(string[] args)
        {
            Tree tree = new Tree();

            //Token token1 = new Token("X", "IDENTIFIER");
            //Token token2 = new Token("if", "IF");
            //Token token3 = new Token("5", "NUMBER");
            //Token token4 = new Token("=", "EQUAL");
            //Token token5 = new Token("read", "READ");
            //Token token6 = new Token("Y", "IDENTIFIER");

            //Node root = tree.getTreeRoot();
            //Node ifnode = new Node(token2);
            //Node opnode = new Node(token4);
            //Node op1node = new Node(token1);
            //Node op2node = new Node(token3);

            //Node ifbody_read = new Node(token5);
            //Node readid = new Node(token6);


            //tree.appendChild(root, ifnode);
            //tree.appendChild(ifnode, opnode);
            //tree.appendChild(opnode, op1node);
            //tree.appendChild(opnode, op2node);
            //tree.appendChild(ifnode, ifbody_read);
            //tree.appendChild(ifbody_read, readid);
            string x = "{ Sample program in TINY language – computes factorial } read x;{ input an integer }if  0 < x   then     { don’t compute if x <= 0 }fact:= 1; repeat fact  := fact * x ; x:= x - 1 until x = 0 ; write fact { output factorial of x }end";
            Parser p = new Parser(x, tree);
            p.program(tree.getTreeRoot());

            tree.traverseTree(printNode);
            //Console.WriteLine("Hello World!");
        }
        static public void printNode(Node parent, Node child, int depth)
        {
            Console.WriteLine("Parent is " + parent.getToken().Tokenvalue + " .. Child is " + child.getToken().Tokenvalue);
        }

    }
}
