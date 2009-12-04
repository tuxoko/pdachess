using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace pdachess
{
    public partial class Form1 : Form
    {
        private void setTokenmoves()
        {
            //set bishop moves
            for (int i = 1; i < 8; i++) Bishop_move[0 + (i - 1)] = new Point(i, -1 * i);
            for (int i = 1; i < 8; i++) Bishop_move[7 + (i - 1)] = new Point(-1 * i, -1 * i);
            for (int i = 1; i < 8; i++) Bishop_move[14 + (i - 1)] = new Point(i, i);
            for (int i = 1; i < 8; i++) Bishop_move[21 + (i - 1)] = new Point(-1 * i, i);
            //set Knight moves
            Knight_move[0] = new Point(1, 2);
            Knight_move[1] = new Point(2, 1);
            Knight_move[2] = new Point(-1, 2);
            Knight_move[3] = new Point(-2, 1);
            Knight_move[4] = new Point(1, -2);
            Knight_move[5] = new Point(2, -1);
            Knight_move[6] = new Point(-1, -2);
            Knight_move[7] = new Point(-2, -1);
            //set Castle moves
            for (int i = 1; i < 8; i++) Castle_move[0 + (i - 1)] = new Point(0, -1 *i);
            for (int i = 1; i < 8; i++) Castle_move[7 + (i - 1)] = new Point(-1 * i, 0);
            for (int i = 1; i < 8; i++) Castle_move[14 + (i - 1)] = new Point(0, i);
            for (int i = 1; i < 8; i++) Castle_move[21 + (i - 1)] = new Point(i, 0);
            //set King moves
            King_move[0] = new Point(1, 0);
            King_move[1] = new Point(0, 1);
            King_move[2] = new Point(-1, 0);
            King_move[3] = new Point(0, -1);
            King_move[4] = new Point(1, 1);
            King_move[5] = new Point(1, -1);
            King_move[6] = new Point(-1, 1);
            King_move[7] = new Point(-1, -1);
        }
        private void movecontrol_Pawn(Pawn t)
        {
            int po_x = t.getX();
            int po_y = t.getY();
            possiblemove.Clear();
            if (t.getSide() == "b")
            {
                if (po_y < 7)
                {
                    if (t_map[getIndex(po_x, po_y + 1)] == null)
                    {
                        possiblemove.Add(new Point(po_x, po_y + 1));
                        if (t.isFirstmove())
                        {
                            if (po_y < 6 && t_map[getIndex(po_x, po_y + 2)] == null)
                                possiblemove.Add(new Point(po_x, po_y + 2));
                        }
                    }

                    if (po_x>0 && t_map[getIndex(po_x - 1, po_y + 1)] != null && t_map[getIndex(po_x - 1, po_y + 1)].getSide() == "w")
                    {
                        possiblemove.Add(new Point(po_x - 1, po_y + 1));
                    }

                    if (po_x<7 && t_map[getIndex(po_x + 1, po_y + 1)] != null && t_map[getIndex(po_x + 1, po_y + 1)].getSide() == "w")
                    {
                        possiblemove.Add(new Point(po_x + 1, po_y + 1));
                    }
                    if (enPassant != null && enPassant.getSide() != t.getSide() && enPassant.getY() == po_y && Math.Abs(enPassant.getX() - po_x) == 1)
                    {
                        possiblemove.Add(new Point(enPassant.getX(), enPassant.getY() + 1));
                    }
                }
            }
            else
            {
                if (po_y > 0)
                {
                    if (t_map[getIndex(po_x, po_y - 1)] == null)
                    {
                        possiblemove.Add(new Point(po_x, po_y - 1));
                        if (t.isFirstmove())
                        {
                            if (po_y > 1 && t_map[getIndex(po_x, po_y - 2)] == null)
                                possiblemove.Add(new Point(po_x, po_y - 2));
                        }
                    }

                    if (po_x>0 && t_map[getIndex(po_x - 1, po_y - 1)] != null && t_map[getIndex(po_x - 1, po_y - 1)].getSide() == "b")
                    {
                        possiblemove.Add(new Point(po_x - 1, po_y - 1));
                    }
                    if (po_x<7 && t_map[getIndex(po_x + 1, po_y - 1)] != null && t_map[getIndex(po_x + 1, po_y - 1)].getSide() == "b")
                    {
                        possiblemove.Add(new Point(po_x + 1, po_y - 1));
                    }
                    if (enPassant != null && enPassant.getSide() != t.getSide() && enPassant.getY() == po_y && Math.Abs(enPassant.getX() - po_x) == 1)
                    {
                        possiblemove.Add(new Point(enPassant.getX(), enPassant.getY() - 1));
                    }
                }
            }
        }

        private void movecontrol_Bishop(Bishop t)
        {
            int po_x = t.getX();
            int po_y = t.getY();

            possiblemove.Clear();

            for (int i = 0; i < 28; i++)
            {
                if (po_x + Bishop_move[i].X < 8 && po_x + Bishop_move[i].X >= 0 && po_y + Bishop_move[i].Y >= 0 && po_y + Bishop_move[i].Y < 8)
                {
                    if (t_map[getIndex(po_x + Bishop_move[i].X, po_y + Bishop_move[i].Y)] != null)
                    {
                        if (t_map[getIndex(po_x + Bishop_move[i].X, po_y + Bishop_move[i].Y)].getSide() != tokenChosed.getSide())
                        {
                            possiblemove.Add(new Point(po_x + Bishop_move[i].X, po_y + Bishop_move[i].Y));
                        }
                        i = ((i / 7) + 1) * 7 - 1;
                    }
                    else
                    {
                        possiblemove.Add(new Point(po_x + Bishop_move[i].X, po_y + Bishop_move[i].Y));
                    }
                }
            }
        }
        private void movecontrol_Knight(Knight t)
        {
            int po_x = t.getX();
            int po_y = t.getY();
            possiblemove.Clear();

            for (int i = 0; i < 8; i++)
            {
                if (po_x + Knight_move[i].X < 8 && po_x + Knight_move[i].X >= 0 && po_y + Knight_move[i].Y >= 0 && po_y + Knight_move[i].Y < 8)
                {
                    if (t_map[getIndex(po_x + Knight_move[i].X, po_y + Knight_move[i].Y)] != null)
                    {
                        if (t_map[getIndex(po_x + Knight_move[i].X, po_y + Knight_move[i].Y)].getSide() != tokenChosed.getSide())
                        {
                            possiblemove.Add(new Point(po_x + Knight_move[i].X, po_y + Knight_move[i].Y));
                        }
                    }
                    else
                    {
                        possiblemove.Add(new Point(po_x + Knight_move[i].X, po_y + Knight_move[i].Y));
                    }
                }
            }
        }
        private void movecontrol_Castle(Castle t)
        {
            int po_x = t.getX();
            int po_y = t.getY();
            possiblemove.Clear();

            for (int i = 0; i < 28; i++)
            {
                if (po_x + Castle_move[i].X < 8 && po_x + Castle_move[i].X >= 0 && po_y + Castle_move[i].Y >= 0 && po_y + Castle_move[i].Y < 8)
                {
                    if (t_map[getIndex(po_x + Castle_move[i].X, po_y + Castle_move[i].Y)] != null)
                    {
                        if (t_map[getIndex(po_x + Castle_move[i].X, po_y + Castle_move[i].Y)].getSide() != tokenChosed.getSide())
                        {
                            possiblemove.Add(new Point(po_x + Castle_move[i].X, po_y + Castle_move[i].Y));
                        }
                        i = ((i / 7) + 1) * 7 - 1;
                    }
                    else
                    {
                        possiblemove.Add(new Point(po_x + Castle_move[i].X, po_y + Castle_move[i].Y));
                    }
                }
            }
        }
        private void movecontrol_Queen(Queen t)
        {
            int po_x = t.getX();
            int po_y = t.getY();
            possiblemove.Clear();

            for (int i = 0; i < 28; i++)
            {
                if (po_x + Bishop_move[i].X < 8 && po_x + Bishop_move[i].X >= 0 && po_y + Bishop_move[i].Y >= 0 && po_y + Bishop_move[i].Y < 8)
                {
                    if (t_map[getIndex(po_x + Bishop_move[i].X, po_y + Bishop_move[i].Y)] != null)
                    {
                        if (t_map[getIndex(po_x + Bishop_move[i].X, po_y + Bishop_move[i].Y)].getSide() != tokenChosed.getSide())
                        {
                            possiblemove.Add(new Point(po_x + Bishop_move[i].X, po_y + Bishop_move[i].Y));
                        }
                        i = ((i / 7) + 1) * 7 - 1;
                    }
                    else
                    {
                        possiblemove.Add(new Point(po_x + Bishop_move[i].X, po_y + Bishop_move[i].Y));
                    }
                }
            }

            for (int i = 0; i < 28; i++)
            {
                if (po_x + Castle_move[i].X < 8 && po_x + Castle_move[i].X >= 0 && po_y + Castle_move[i].Y >= 0 && po_y + Castle_move[i].Y < 8)
                {
                    if (t_map[getIndex(po_x + Castle_move[i].X, po_y + Castle_move[i].Y)] != null)
                    {
                        if (t_map[getIndex(po_x + Castle_move[i].X, po_y + Castle_move[i].Y)].getSide() != tokenChosed.getSide())
                        {
                            possiblemove.Add(new Point(po_x + Castle_move[i].X, po_y + Castle_move[i].Y));
                        }
                        i = ((i / 7) + 1) * 7 - 1;
                    }
                    else
                    {
                        possiblemove.Add(new Point(po_x + Castle_move[i].X, po_y + Castle_move[i].Y));
                    }
                }
            }
        }
        private void movecontrol_King(King t)
        {
            int po_x = t.getX();
            int po_y = t.getY();
            possiblemove.Clear();

            for (int i = 0; i < 8; i++)
            {
                if (po_x + King_move[i].X < 8 && po_x + King_move[i].X >= 0 && po_y + King_move[i].Y >= 0 && po_y + King_move[i].Y < 8)
                {
                    if (t_map[getIndex(po_x + King_move[i].X, po_y + King_move[i].Y)] != null)
                    {
                        if (t_map[getIndex(po_x + King_move[i].X, po_y + King_move[i].Y)].getSide() != tokenChosed.getSide())
                        {
                            possiblemove.Add(new Point(po_x + King_move[i].X, po_y + King_move[i].Y));
                        }
                    }
                    else
                    {
                        possiblemove.Add(new Point(po_x + King_move[i].X, po_y + King_move[i].Y));
                    }
                }
            }
            if(!ischeck(t_map,t.getSide()) && t.isFirstmove())
            {
                try
                {
                    if (t_map[getIndex(0, po_y)].getType() == "Castle" && t_map[getIndex(0, po_y)].getSide()==t.getSide() && t_map[getIndex(0, po_y)].isFirstmove())
                    {
                        bool block = false;
                        for (int i = 1; i < po_x; i++)
                        {
                            if (t_map[getIndex(i, po_y)] != null)
                            {
                                block = true;
                                break;
                            }
                        }
                        if (!block)
                        {
                            Token[] map = map_clone(t_map);
                            Token temp=tokenChosed;
                            Token tempEnPassant=enPassant;
                            tokenChosed=map[getIndex(po_x,po_y)];
                            move(map, po_x - 1, po_y);
                            if (!ischeck(map,t.getSide()))
                            {
                                possiblemove.Add(new Point(po_x - 2, po_y));
                            }
                            enPassant = tempEnPassant;
                            tokenChosed = temp;
                        }
                    }
                }
                catch { }
                try
                {
                    if (t_map[getIndex(7, po_y)].getType() == "Castle" && t_map[getIndex(0, po_y)].getSide() == t.getSide() && t_map[getIndex(7, po_y)].isFirstmove())
                    {
                        bool block = false;
                        for (int i = po_x + 1; i < 7; i++)
                        {
                            if (t_map[getIndex(i, po_y)] != null)
                            {
                                block = true;
                                break;
                            }
                        }
                        if (!block)
                        {
                            Token[] map = map_clone(t_map);
                            Token temp = tokenChosed;
                            Token tempEnPassant = enPassant;
                            tokenChosed = map[getIndex(po_x, po_y)];
                            move(map, po_x + 1, po_y);
                            if (!ischeck(map,t.getSide()))
                            {
                                possiblemove.Add(new Point(po_x + 2, po_y));
                            }
                            enPassant = tempEnPassant;
                            tokenChosed = temp;
                        }
                    }
                }
                catch { }
            }
        }

        //private delegate void InvokeFunction2(Token[] tmap, int a, int b, string c);
        //private delegate void InvokeFunction3();

        private void opponentmove(string [] cmds)
        {
            
            int chose_x = int.Parse(cmds[1]);
            int chose_y = int.Parse(cmds[2]);
            int dest_x = int.Parse(cmds[3]);
            int dest_y = int.Parse(cmds[4]);
            string promote = (cmds.Length > 5) ? cmds[5] : "";
            tokenChosed = t_map[getIndex(chose_x, chose_y)] as Token;
            //MessageBox.Show(tokenChosed.getType());
            enPassant = null;

            move(t_map,dest_x,dest_y,promote);

            tokenChosed = null;
            possiblemove.Clear();

            drawScreen();
            whosturn = (whosturn == "w") ? "b" : "w";
            label1.Text = (whosturn == "w") ? "White's turn" : "Black's turn";
            if (ischeck(t_map, whosturn))
            {
                if (ismate(whosturn))
                {
                    MessageBox.Show("Checkmate!");
                    lose();
                }
                else
                {
                    MessageBox.Show("Check!");
                }
            }
            else if (ismate(whosturn))
            {
                MessageBox.Show("Stalemate!");
                drawgame();
            }
        }
    }
}
