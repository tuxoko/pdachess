using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace pdachess
{
    class Token
    {
        private int po_x, po_y;
        private Image img_l = null, img_d = null, img_l_p = null, img_d_p = null, img_l_s = null, img_d_s = null;
        private string side;
        private bool initial;
        public Token(int x, int y, string side, Image img_l, Image img_d, Image img_l_p, Image img_d_p, Image img_l_s, Image img_d_s)
        {
            this.po_x = x;
            this.po_y = y;
            this.img_l = img_l;
            this.img_d = img_d;
            this.img_l_p = img_l_p;
            this.img_d_p = img_d_p;
            this.img_l_s = img_l_s;
            this.img_d_s = img_d_s;
            this.side = side;
            initial = true;
        }
        public virtual string getType() { return null; }
        public string getSide() { return side; }
        public virtual void setmove(int x,int y)
        {
            po_x = x;
            po_y = y;
            if (initial == true) initial = false;
        }
        public void draw(Graphics g) 
        {
//            Point p=new Point(po_x*30-4,po_y*30-5);
//            g.DrawImage(img, p);
//            g.DrawImage(img, po_x * 30 , po_y * 30 , 30, 30);
//            g.DrawImage(img, new Rectangle(po_x * 30, po_y * 30, 30, 30), new Rectangle(0, 0, 30, 30), GraphicsUnit.Pixel);
            if((po_x+po_y)%2==0)
                g.DrawImage(img_d, po_x * 60, po_y * 60);
            else
                g.DrawImage(img_l, po_x * 60, po_y * 60);
        }
        public void draw(Graphics g, string param)
        {
            switch (param)
            {
                case "p":
                    g.DrawImage(((po_x + po_y) % 2 == 0) ? img_d_p : img_l_p, po_x * 60, po_y * 60);
                    break;
                case "s":
                    g.DrawImage(((po_x + po_y) % 2 == 0) ? img_d_s : img_l_s, po_x * 60, po_y * 60);
                    break;
            }
        }
        public bool isHere(int x, int y)
        { return (x == po_x && y == po_y); }
        public int getX()
        {
            return po_x;
        }
        public int getY()
        {
            return po_y;
        }
        public bool isFirstmove() { return initial; }
    }

    class Pawn:Token
    {

        public Pawn(int x, int y, string side, Image img_l, Image img_d, Image img_l_p, Image img_d_p, Image img_l_s, Image img_d_s)
            : base(x, y,side,img_l,  img_d,  img_l_p,  img_d_p,  img_l_s,  img_d_s)
        {
            
        }
        public override string getType()
        {
            return "Pawn";
        }    
    }

    class Knight : Token
    {
        public Knight(int x, int y, string side, Image img_l, Image img_d, Image img_l_p, Image img_d_p, Image img_l_s, Image img_d_s)
            : base(x, y, side, img_l, img_d, img_l_p, img_d_p, img_l_s, img_d_s)
        {
        }
        public override string getType()
        {
            return "Knight";
        }
    }

    class Queen : Token
    {
        public Queen(int x, int y, string side, Image img_l, Image img_d, Image img_l_p, Image img_d_p, Image img_l_s, Image img_d_s)
            : base(x, y, side, img_l, img_d, img_l_p, img_d_p, img_l_s, img_d_s)
        {
        }
        public override string getType()
        {
            return "Queen";
        }
    }

    class Castle : Token
    {
        public Castle(int x, int y, string side, Image img_l, Image img_d, Image img_l_p, Image img_d_p, Image img_l_s, Image img_d_s)
            : base(x, y, side, img_l, img_d, img_l_p, img_d_p, img_l_s, img_d_s)
        {
        }
        public override string getType()
        {
            return "Castle";
        }
    }

    class Bishop : Token
    {
        public Bishop(int x, int y, string side, Image img_l, Image img_d, Image img_l_p, Image img_d_p, Image img_l_s, Image img_d_s)
            : base(x, y, side, img_l, img_d, img_l_p, img_d_p, img_l_s, img_d_s)
        {
        }
        public override string getType()
        {
            return "Bishop";
        }
    }

    class King : Token
    {

        public King(int x, int y, string side, Image img_l, Image img_d, Image img_l_p, Image img_d_p, Image img_l_s, Image img_d_s)
            : base(x, y, side, img_l, img_d, img_l_p, img_d_p, img_l_s, img_d_s)
        {
        }
        public override string getType()
        {
            return "King";
        }
        

    }

}
