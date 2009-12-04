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
        
        private bool ischeck(Token[] map,string side)
        {
            try
            {
                int k;
                for (k = 0; k < 64; k++)
                {
                    if (map[k] != null && map[k].getType() == "King" && map[k].getSide() == side)
                        break;
                }
                int x = map[k].getX();
                int y = map[k].getY();

                //check for pawn
                if ((side == "b") ? y < 7 : y > 0)
                {
                    if (x > 0 && map[getIndex(x - 1, (side == "b") ? y + 1 : y - 1)] != null &&
                        map[getIndex(x - 1, (side == "b") ? y + 1 : y - 1)].getType() == "Pawn" && map[getIndex(x - 1, (side == "b") ? y + 1 : y - 1)].getSide() != side)
                    {
                        return true;
                    }
                    if (x < 7 && map[getIndex(x + 1, (side == "b") ? y + 1 : y - 1)] != null &&
                        map[getIndex(x + 1, (side == "b") ? y + 1 : y - 1)].getType() == "Pawn" && map[getIndex(x + 1, (side == "b") ? y + 1 : y - 1)].getSide() != side)
                    {
                        return true;
                    }
                }
                //check for king
                for (int i = 0; i < 8; i++)
                {
                    int dx = King_move[i].X + x;
                    int dy = King_move[i].Y + y;
                    if (dx < 0 || dx > 7 || dy < 0 || dy > 7) continue;
                    if (map[getIndex(dx, dy)] != null && map[getIndex(dx, dy)].getType() == "King" && map[getIndex(dx, dy)].getSide() != side)
                        return true;
                }
                //check for knight
                for (int i = 0; i < 8; i++)
                {
                    int dx = Knight_move[i].X + x;
                    int dy = Knight_move[i].Y + y;
                    if (dx < 0 || dx > 7 || dy < 0 || dy > 7) continue;
                    if (map[getIndex(dx, dy)] != null && map[getIndex(dx, dy)].getType() == "Knight" && map[getIndex(dx, dy)].getSide() != side)
                        return true;
                }
                //check for castle&queen
                for (int i = 0; i < 28; i++)
                {
                    int dx = Castle_move[i].X + x;
                    int dy = Castle_move[i].Y + y;
                    if (dx < 0 || dx > 7 || dy < 0 || dy > 7) continue;
                    if (map[getIndex(dx, dy)] != null)
                    {
                        if ((map[getIndex(dx, dy)].getType() == "Castle" || map[getIndex(dx, dy)].getType() == "Queen")
                        && map[getIndex(dx, dy)].getSide() != side)
                            return true;
                        i = ((i / 7) + 1) * 7 - 1;
                    }
                }
                //check for bishop&queen
                for (int i = 0; i < 28; i++)
                {
                    int dx = Bishop_move[i].X + x;
                    int dy = Bishop_move[i].Y + y;
                    if (dx < 0 || dx > 7 || dy < 0 || dy > 7) continue;
                    if (map[getIndex(dx, dy)] != null)
                    {
                        if ((map[getIndex(dx, dy)].getType() == "Bishop" || map[getIndex(dx, dy)].getType() == "Queen")
                        && map[getIndex(dx, dy)].getSide() != side)
                            return true;
                        i = ((i / 7) + 1) * 7 - 1;
                    }
                }
                return false;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); return false; }
        }
        private Token[] map_clone(Token[] map)
        {
            Token[] r = new Token[64];
            for (int i = 0; i < 64; i++)
            {
                if (map[i] != null)
                {
                    r[i] = map[i].Clone() as Token;
                }
            }
            return r;
        }
        private void test_check()
        {
            ArrayList del=new ArrayList();
            Token tempChosed=tokenChosed;
            Token tempEnPassant = enPassant;
            foreach (Point p in possiblemove)
            {
                //Token[] map = t_map.Clone() as Token[];
                Token[] map = map_clone(t_map);
                tokenChosed = map[getIndex(tempChosed.getX(), tempChosed.getY())];
                enPassant = tempEnPassant;
                move(map, p.X, p.Y);
                if (ischeck(map,tokenChosed.getSide()))
                {
                    del.Add(p);
                }
            }
            tokenChosed = tempChosed;
            enPassant = tempEnPassant;
            foreach (Point p in del)
            {
                possiblemove.Remove(p);
            }
        }
        private bool ismate(string side)
        {
            for (int i = 0; i < 64; i++)
            {
                if (t_map[i] != null && t_map[i].getSide() == side)
                {
                    possiblemove.Clear();
                    tokenChosed = t_map[i];
                    displaymove(tokenChosed);
                    if (possiblemove.Count != 0)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}