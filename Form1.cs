using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using pdachess.Properties;

namespace pdachess
{
    public partial class Form1 : Form
    {

        //private Token pawn = null;
        Graphics g = null;
        private Bitmap chessboard = null;
   /*     private Image pawn_p = null;
        private Image bishop_p = null;
        private Image knight_p = null;
        private Image queen_p = null;
        private Image king_p = null;
        private Image castle_p = null;
        private Image pawn_p_w = null;
        private Image bishop_p_w = null;
        private Image knight_p_w = null;
        private Image queen_p_w = null;
        private Image king_p_w = null;
        private Image castle_p_w = null;*/
        private Image move_l = null;
        private Image move_d = null;
  //      private Image select = null;
        private Bitmap picshow = new Bitmap(480, 480);


        //ways of move
        private Point[] Bishop_move = new Point[28];
        private Point[] Knight_move = new Point[8];
        private Point[] Castle_move = new Point[28];
        private Point[] King_move = new Point[8];

        private Token[] t_map = new Token[64];
        private ArrayList possiblemove = new ArrayList();
        private Token tokenChosed = null;

        private string whosturn = "w";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //loading all image files
            setTokenmoves();
            loadImage();
            initTokens();
            drawScreen();

        }
        private void loadImage()
        {
            //resize chessboard to proper size
            Size picsize = new Size(480, 480);
            Bitmap chessboard_ori = Properties.Resources.board;
//            chessboard = new Bitmap(chessboard_ori, picsize);
            chessboard = new Bitmap(chessboard_ori);

            //load all token images

            move_l = Resources.l_p;
            move_d = Resources.d_p;
            

        }
        private void initTokens()
        {
            //castle
            t_map[getIndex(0, 0)] = new Castle(0, 0, "b", Resources.Castle_b_l, Resources.Castle_b_d, Resources.Castle_b_l_p, Resources.Castle_b_d_p, Resources.Castle_b_l_s, Resources.Castle_b_d_s);
            t_map[getIndex(7, 0)] = new Castle(7, 0, "b", Resources.Castle_b_l, Resources.Castle_b_d, Resources.Castle_b_l_p, Resources.Castle_b_d_p, Resources.Castle_b_l_s, Resources.Castle_b_d_s);

            t_map[getIndex(0, 7)] = new Castle(0, 7, "w", Resources.Castle_w_l, Resources.Castle_w_d, Resources.Castle_w_l_p, Resources.Castle_w_d_p, Resources.Castle_w_l_s, Resources.Castle_w_d_s);
            t_map[getIndex(7, 7)] = new Castle(7, 7, "w", Resources.Castle_w_l, Resources.Castle_w_d, Resources.Castle_w_l_p, Resources.Castle_w_d_p, Resources.Castle_w_l_s, Resources.Castle_w_d_s);

            //Knight
            t_map[getIndex(1, 0)] = new Knight(1, 0, "b", Resources.Knight_b_l, Resources.Knight_b_d, Resources.Knight_b_l_p, Resources.Knight_b_d_p, Resources.Knight_b_l_s, Resources.Knight_b_d_s);
            t_map[getIndex(6, 0)] = new Knight(6, 0, "b", Resources.Knight_b_l, Resources.Knight_b_d, Resources.Knight_b_l_p, Resources.Knight_b_d_p, Resources.Knight_b_l_s, Resources.Knight_b_d_s);

            t_map[getIndex(1, 7)] = new Knight(1, 7, "w", Resources.Knight_w_l, Resources.Knight_w_d, Resources.Knight_w_l_p, Resources.Knight_w_d_p, Resources.Knight_w_l_s, Resources.Knight_w_d_s);
            t_map[getIndex(6, 7)] = new Knight(6, 7, "w", Resources.Knight_w_l, Resources.Knight_w_d, Resources.Knight_w_l_p, Resources.Knight_w_d_p, Resources.Knight_w_l_s, Resources.Knight_w_d_s);
            //Bishop
            t_map[getIndex(2, 0)] = new Bishop(2, 0, "b", Resources.Bishop_b_l, Resources.Bishop_b_d, Resources.Bishop_b_l_p, Resources.Bishop_b_d_p, Resources.Bishop_b_l_s, Resources.Bishop_b_d_s);
            t_map[getIndex(5, 0)] = new Bishop(5, 0, "b", Resources.Bishop_b_l, Resources.Bishop_b_d, Resources.Bishop_b_l_p, Resources.Bishop_b_d_p, Resources.Bishop_b_l_s, Resources.Bishop_b_d_s);

            t_map[getIndex(2, 7)] = new Bishop(2, 7, "w", Resources.Bishop_w_l, Resources.Bishop_w_d, Resources.Bishop_w_l_p, Resources.Bishop_w_d_p, Resources.Bishop_w_l_s, Resources.Bishop_w_d_s);
            t_map[getIndex(5, 7)] = new Bishop(5, 7, "w", Resources.Bishop_w_l, Resources.Bishop_w_d, Resources.Bishop_w_l_p, Resources.Bishop_w_d_p, Resources.Bishop_w_l_s, Resources.Bishop_w_d_s);

            //Queen
            t_map[getIndex(3, 0)] = new Queen(3, 0, "b", Resources.Queen_b_l, Resources.Queen_b_d, Resources.Queen_b_l_p, Resources.Queen_b_d_p, Resources.Queen_b_l_s, Resources.Queen_b_d_s);

            t_map[getIndex(3, 7)] = new Queen(3, 7, "w", Resources.Queen_w_l, Resources.Queen_w_d, Resources.Queen_w_l_p, Resources.Queen_w_d_p, Resources.Queen_w_l_s, Resources.Queen_w_d_s);

            //King
            t_map[getIndex(4, 0)] = new King(4, 0, "b", Resources.King_b_l, Resources.King_b_d, Resources.King_b_l_p, Resources.King_b_d_p, Resources.King_b_l_s, Resources.King_b_d_s);

            t_map[getIndex(4, 7)] = new King(4, 7, "w", Resources.King_w_l, Resources.King_w_d, Resources.King_w_l_p, Resources.King_w_d_p, Resources.King_w_l_s, Resources.King_w_d_s);

            //Pawn
            for (int i = 0; i < 8; i++)
            {
                t_map[getIndex(i, 1)] = new Pawn(i, 1, "b", Resources.Pawn_b_l, Resources.Pawn_b_d, Resources.Pawn_b_l_p, Resources.Pawn_b_d_p, Resources.Pawn_b_l_s, Resources.Pawn_b_d_s);
                t_map[getIndex(i, 6)] = new Pawn(i, 6, "w", Resources.Pawn_w_l, Resources.Pawn_w_d, Resources.Pawn_w_l_p, Resources.Pawn_w_d_p, Resources.Pawn_w_l_s, Resources.Pawn_w_d_s);
            }

        }

        private int getIndex(int x, int y)
        {
            return x + y * 8;
        }

        private void drawScreen()
        {
            try
            {
                picshow = chessboard.Clone() as Bitmap;
                g = Graphics.FromImage(picshow);

                for (int i = 0; i < 64; i++)
                {
                    try
                    {
                        t_map[i].draw(g);
                    }
                    catch { }
                }

                if (tokenChosed != null)
                {
                    //                g.FillRectangle(Brushes.Cyan, tokenChosed.getX() * 30, tokenChosed.getY() * 30, 30, 30);
                    //g.DrawImage(select, tokenChosed.getX() * 30, tokenChosed.getY() * 30, 30, 30);
                    // g.DrawImage(select, new Rectangle(tokenChosed.getX() * 30, tokenChosed.getY() * 30, 30, 30), new Rectangle(0, 0, 30, 30), GraphicsUnit.Pixel);
                    //g.DrawImage(select, tokenChosed.getX() * 30, tokenChosed.getY() * 30);
                    tokenChosed.draw(g, "s");
                }

                foreach (Point p in possiblemove)
                {
                    //                g.FillRectangle(Brushes.Yellow, p.X * 30, p.Y * 30, 30, 30);
                    //               g.DrawImage(move, p.X * 30, p.Y * 30, 30, 30);
                    //                g.DrawImage(move, new Rectangle(p.X * 30, p.Y * 30, 30, 30), new Rectangle(0, 0, 30, 30), GraphicsUnit.Pixel);
                    if (p.X < 0 || p.X > 7 || p.Y < 0 || p.Y > 7)
                        continue;
                    if (t_map[getIndex(p.X, p.Y)] == null)
                        g.DrawImage(((p.X + p.Y) % 2 == 0) ? move_d : move_l, p.X * 60, p.Y * 60);
                    else
                        t_map[getIndex(p.X, p.Y)].draw(g, "p");
                }



                gamebox.Image = picshow;
                gamebox.Refresh();
            }
            catch { }
        }


        private void gamebox_MouseDown(object sender, MouseEventArgs e)
        {
            int po_x = e.X / (Screen.PrimaryScreen.WorkingArea.Width/8);
            int po_y = e.Y / (Screen.PrimaryScreen.WorkingArea.Width/8);
            
            Point p = new Point(po_x, po_y);
            try
            {
                if (possiblemove.Count != 0 && possiblemove.Contains(p))
                {
                    //MessageBox.Show("choosed");
                    //castling
                    if (tokenChosed.getType() == "King" && Math.Abs(p.X - tokenChosed.getX()) == 2)
                    {
                        if (p.X < tokenChosed.getX())
                        {
                            t_map[getIndex(tokenChosed.getX() - 1, tokenChosed.getY())] = t_map[getIndex(0, tokenChosed.getY())];
                            t_map[getIndex(0, tokenChosed.getY())] = null;
                            t_map[getIndex(tokenChosed.getX() - 1, tokenChosed.getY())].setmove(tokenChosed.getX() - 1, tokenChosed.getY());
                        }
                        else
                        {
                            t_map[getIndex(tokenChosed.getX() + 1, tokenChosed.getY())] = t_map[getIndex(7, tokenChosed.getY())];
                            t_map[getIndex(7, tokenChosed.getY())] = null;
                            t_map[getIndex(tokenChosed.getX() + 1, tokenChosed.getY())].setmove(tokenChosed.getX() + 1, tokenChosed.getY());
                        }
                    }

                    t_map[getIndex(tokenChosed.getX(), tokenChosed.getY())] = null as Token;
                    tokenChosed.setmove(po_x, po_y);
                    t_map[getIndex(tokenChosed.getX(), tokenChosed.getY())] = tokenChosed;

                    if (tokenChosed.getType() == "Pawn" && (tokenChosed.getY() == 0 || tokenChosed.getY() == 7))
                    {
                        if(tokenChosed.getSide()=="b")
                            t_map[getIndex(tokenChosed.getX(), tokenChosed.getY())] = new Queen(tokenChosed.getX(), tokenChosed.getY(),
                                                                          tokenChosed.getSide(), Resources.Queen_b_l, Resources.Queen_b_d, Resources.Queen_b_l_p, Resources.Queen_b_d_p, Resources.Queen_b_l_s, Resources.Queen_b_d_s);
                        else
                            t_map[getIndex(tokenChosed.getX(), tokenChosed.getY())] = new Queen(tokenChosed.getX(), tokenChosed.getY(),tokenChosed.getSide(), Resources.Queen_w_l, Resources.Queen_w_d, Resources.Queen_w_l_p, Resources.Queen_w_d_p, Resources.Queen_w_l_s, Resources.Queen_w_d_s);
                    }

                    tokenChosed = null;
                    possiblemove.Clear();
                    drawScreen();
                    whosturn = (whosturn == "w") ? "b" : "w";
                }
                else if (t_map[getIndex(po_x, po_y)].isHere(po_x, po_y) && t_map[getIndex(po_x, po_y)].getSide() == whosturn)
                {
                    tokenChosed = t_map[getIndex(po_x, po_y)] as Token;
                    displaymove(t_map[getIndex(po_x, po_y)]);
                }
                else
                {
                    tokenChosed = null;
                    possiblemove.Clear();
                    drawScreen();
                }
            }
            catch
            {
                tokenChosed = null;
                possiblemove.Clear();
                drawScreen();
            }
        }

        private void displaymove(Token t)
        {
            switch (t.getType())
            {
                case ("Pawn"):
                    movecontrol_Pawn((Pawn)t);
                    break;
                case ("Bishop"):
                    movecontrol_Bishop((Bishop)t);
                    break;
                case ("King"):
                    movecontrol_King((King)t);
                    break;
                case ("Queen"):
                    movecontrol_Queen((Queen)t);
                    break;
                case ("Castle"):
                    movecontrol_Castle((Castle)t);
                    break;
                case ("Knight"):
                    movecontrol_Knight((Knight)t);
                    break;
                default:
                    break;
            }
        }

        private void OnMenuExit(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
