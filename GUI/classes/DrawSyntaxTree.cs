using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Tiny_Parser
{
    class DrawSyntaxTree
    {
        //int left_margin = 20;
        int top_margin = 70;
        //int line_length = 15;
        Canvas MyCanvas;

        int child_left_margin = 0;
        int last_left_margin = 0;
        int last_top_margin = 0;
        Boolean isChild = true;

        public DrawSyntaxTree(Canvas c)
        {
            MyCanvas = c;
        }

        public void Draw_Child_Node(Node parent, Node child, int depth)
        {
            Token token = child.getToken();
            int children_count = parent.getChildrenCount();
            
            //draw a line 
            Line line = new Line
            {
                StrokeThickness = 1,
                Stroke = System.Windows.Media.Brushes.Black,
            };

            Line horizontal_line = new Line
            {
                StrokeThickness = 1,
                Stroke = System.Windows.Media.Brushes.Black,
            };

            if (children_count == 1 && depth != 0)
            {
                //left margin of this only child equals the parent left margin
                child_left_margin = parent.getLeftMargin();
            }

            else if(children_count == 2 && depth != 0 && parent.getToken().Tokentype != "ASSIGN")
            {
                //left margin of two children ,, the first is behind the parent and the second is ahead of the parent
                //but for the first node in the tree with children then make the first child have the same left margin
                //so that it wont get out of the window frame
                switch(child.getChildNo())
                {
                    case 0:
                        child_left_margin = (depth != 0) ? parent.getLeftMargin() - 50 : parent.getLeftMargin();
                        break;

                    case 1:
                        child_left_margin = (depth != 0) ? parent.getLeftMargin() + 50 : parent.getLeftMargin() + 100;
                        break;
                }
            }

            else if (children_count == 3 && depth != 0)
            {
                switch (child.getChildNo())
                {
                    case 0:
                        child_left_margin = (depth == 0) ? parent.getLeftMargin() - 80: parent.getLeftMargin();
                        break;

                    case 1:
                        child_left_margin = (depth == 0) ? parent.getLeftMargin() : parent.getLeftMargin() + 200;
                        break;

                    case 2:
                        child_left_margin = (depth == 0) ? parent.getLeftMargin() + 80 : parent.getLeftMargin() + 400;
                        break;
                }
            }

            else if (child.getChildNo() == 0 || (parent.getToken().Tokentype == "ASSIGN" && child.getChildNo() == 1))
            {
                child_left_margin = last_left_margin;
            }

            else
            {
                child_left_margin = last_left_margin + 100;
            }
            //assign & read have their children inside the box not as a child
            if (isExpression(child) && parent.getToken().Tokentype != "READ" && (parent.getToken().Tokentype != "ASSIGN" || (parent.getToken().Tokentype == "ASSIGN" && child.getChildNo() > 0)))
            {
                //draw oval shape
                Border oval = new Border
                {
                    CornerRadius = new CornerRadius(50),
                    Width = 80,
                    Height = 50,
                    Margin = new Thickness { Left = child_left_margin, Top = top_margin * depth},
                    Padding = new Thickness(4),
                    BorderBrush = new SolidColorBrush(Colors.Black),
                    BorderThickness = new Thickness(1),
                };

                child.setLeftMargin((int)oval.Margin.Left);

                TextBlock oval_content = new TextBlock
                {
                    TextAlignment = TextAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    FontSize = 15,
                };

                switch(token.Tokentype)
                {
                    case "IDENTIFIER":
                        oval_content.Text = "id\n(" + token.Tokenvalue + ")";
                        break;

                    case "NUMBER":
                        oval_content.Text = "const\n(" + token.Tokenvalue + ")";
                        break;

                    default:
                        oval_content.Text = "op\n(" + token.Tokenvalue + ")";
                        break;
                }

                oval.Child = oval_content;

                //all oval shapes are connected to their parents
                line.X2 = (oval.Width / 2) + oval.Margin.Left;
                line.Y2 = oval.Margin.Top;

                line.X1 = (oval.Width / 2) + parent.getLeftMargin();
                line.Y1 = top_margin * (depth - 1) + oval.Height;
                

                MyCanvas.Children.Add(oval);
                last_top_margin = (int)oval.Margin.Top;
            }

            else if ((parent.getToken().Tokentype != "ASSIGN" || (parent.getToken().Tokentype == "ASSIGN" && child.getChildNo() > 0)) && parent.getToken().Tokentype != "READ")
            {
                // Create rectangle.
                Border rectangle = new Border
                {
                    Width = 70,
                    Height = 50,
                    Margin = new Thickness { Left = child_left_margin, Top = top_margin * depth },
                    Padding = new Thickness(4),
                    BorderBrush = new SolidColorBrush(Colors.Black),
                    BorderThickness = new Thickness(1),
                };

                child.setLeftMargin((int)rectangle.Margin.Left);

                TextBlock rect_content = new TextBlock
                {
                    TextAlignment = TextAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    FontSize = 15,
                };

                //to write the read/assign first child inside its box
                switch (token.Tokentype)
                {
                    case "READ":
                        rect_content.Text = "read\n(" + child.getChildren()[0].getToken().Tokenvalue + ")";
                        break;

                    case "ASSIGN":
                        rect_content.Text = "assign\n(" + child.getChildren()[0].getToken().Tokenvalue + ")";
                        break;

                    default:
                        rect_content.Text = token.Tokenvalue;
                        break;
                }

                rectangle.Child = rect_content;

                if((depth == 0 && child.getChildNo() > 0) || (parent.getToken().Tokentype == "IF" && !child.getToken().isElsePart && child.getChildNo() > 1) || (parent.getToken().Tokentype == "REPEAT" && child.getChildNo() > 0))
                {
                    horizontal_line.X2 = rectangle.Margin.Left;
                    horizontal_line.Y2 = (rectangle.Height / 2) + rectangle.Margin.Top;

                    horizontal_line.X1 = rectangle.Width + parent.getChildren()[child.getChildNo() - 1].getLeftMargin();
                    horizontal_line.Y1 = top_margin * depth + (rectangle.Height / 2);
                }

                else
                {
                    line.X2 = (rectangle.Width / 2) + rectangle.Margin.Left;
                    line.Y2 = rectangle.Margin.Top;

                    line.X1 = (rectangle.Width / 2) + parent.getLeftMargin();
                    line.Y1 = top_margin * (depth - 1) + rectangle.Height;
                }

                MyCanvas.Children.Add(rectangle);
                last_top_margin = (int)rectangle.Margin.Top;
            }

            last_left_margin = child_left_margin;

            //in order not to draw the line between the root node which is not drawn in the syntax tree & its children
            if(depth != 0) MyCanvas.Children.Add(line);
            MyCanvas.Children.Add(horizontal_line);

            MyCanvas.Width = last_left_margin + 80 + 30;
            MyCanvas.Height = last_top_margin + 50 + 30;
        }


        private Boolean isExpression(Node n)
        {
            string tokenType = n.getToken().Tokentype;
            switch(tokenType)
            {
                case "NUMBER":
                    break;

                case "IDENTIFIER":
                    break;

                case "LESSTHAN":
                    break;

                case "EQUAL":
                    break;

                case "PLUS":
                    break;

                case "MINUS":
                    break;

                case "MULT":
                    break;

                case "DIV":
                    break;

                default:
                    return false;
            }
            return true;
        }
    }
}
